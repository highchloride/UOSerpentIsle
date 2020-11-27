#region Header
//   Vorspire    _,-'/-'/  Meteorites.cs
//   .      __,-; ,'( '/
//    \.    `-.__`-._`:_,-._       _ , . ``
//     `:-._,------' ` _,`--` -: `_ , ` ,' :
//        `---..__,,--'  (C) 2017  ` -'. -'
//        #  Vita-Nex [http://core.vita-nex.com]  #
//  {o)xxx|===============-   #   -===============|xxx(o}
//        #        The MIT License (MIT)          #
#endregion

#region References
using System;

using Server.Spells;

using VitaNex.FX;
#endregion

namespace Server.Mobiles
{
	public class AspectAbilityMeteorites : AspectAbility
	{
		public override string Name { get { return "Meteorites"; } }

		public override AspectFlags Aspects { get { return AspectFlags.Fire; } }

		public override TimeSpan Lockdown { get { return TimeSpan.FromSeconds(30); } }
		public override TimeSpan Cooldown { get { return TimeSpan.FromSeconds(20); } }

		public override TimeSpan Duration { get { return TimeSpan.FromSeconds(15); } }

		protected override void OnInvoke(BaseAspect aspect)
		{
			if (aspect == null || aspect.Deleted)
			{
				return;
			}

			if (aspect.PlayAttackAnimation())
			{
				aspect.PlayAttackSound();
			}

			aspect.PlaySound(1230);

			var delay = 500;

			for (var range = 4; range <= aspect.RangePerception; range++, delay += 500)
			{
				Timer.DelayCall(
					TimeSpan.FromMilliseconds(delay),
					loc =>
					{
						SpellHelper.Turn(aspect, loc);

						if (aspect.PlayAttackAnimation())
						{
							aspect.PlayAttackSound();
						}

						Meteorite(aspect, loc);
					},
					aspect.Location.GetRandomPoint2D(range, range).GetSurfaceTop(aspect.Map));
			}
		}

		private void Meteorite(BaseAspect aspect, Point3D loc)
		{
			new MovingEffectInfo(loc.Clone3D(-4, 0, 15), loc.Clone3D(0, 0, 4), aspect.Map, 4534, 2999, 1).Send();
			new MovingEffectInfo(loc.Clone3D(0, 0, 60), loc.Clone3D(0, 0, 5), aspect.Map, 4534, 1258, 4)
			{
				SoundID = 541
			}.MovingImpact(e => MeteoriteImpact(aspect, e.Target.Location, 4));
		}

		private void MeteoriteImpact(BaseAspect aspect, Point3D loc, int blastRange)
		{
			if (aspect.Deleted || !aspect.Alive)
			{
				return;
			}

			new FireExplodeEffect(loc, aspect.Map, blastRange)
			{
				AverageZ = false,
				EffectMutator = e => e.SoundID = 520,
				EffectHandler = e => MeteoriteBlast(aspect, e)
			}.Send();
		}

		private void MeteoriteBlast(BaseAspect aspect, EffectInfo e)
		{
			if (aspect.Deleted || !aspect.Alive || e.ProcessIndex != 0)
			{
				return;
			}

			foreach (var t in AcquireTargets<Mobile>(aspect, e.Source.Location, 0))
			{
				Damage(aspect, t);
			}
		}
	}
}