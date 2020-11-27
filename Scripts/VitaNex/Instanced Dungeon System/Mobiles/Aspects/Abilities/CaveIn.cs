#region Header
//   Vorspire    _,-'/-'/  CaveIn.cs
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
	public class AspectAbilityCaveIn : AspectAbility
	{
		public override string Name { get { return "Cave In"; } }

		public override AspectFlags Aspects { get { return AspectFlags.Earth; } }

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

						CaveInBoulder(aspect, loc);
					},
					aspect.Location.GetRandomPoint2D(range, range).GetSurfaceTop(aspect.Map));
			}
		}

		private void CaveInBoulder(BaseAspect aspect, Point3D loc)
		{
			new MovingEffectInfo(loc.Clone3D(-4, 0, 15), loc.Clone3D(0, 0, 4), aspect.Map, 4534, 2999, 1).Send();
			new MovingEffectInfo(loc.Clone3D(0, 0, 60), loc.Clone3D(0, 0, 5), aspect.Map, 4534, 0, 4)
			{
				SoundID = 541
			}.MovingImpact(e => BoulderImpact(aspect, e.Target.Location, 3));
		}

		private void BoulderImpact(BaseAspect aspect, Point3D loc, int blastRange)
		{
			if (aspect.Deleted || !aspect.Alive)
			{
				return;
			}

			new EarthExplodeEffect(loc, aspect.Map, blastRange)
			{
				AverageZ = false,
				EffectMutator = e => e.SoundID = 1231,
				EffectHandler = e => BoulderBlast(aspect, e)
			}.Send();
		}

		private void BoulderBlast(BaseAspect aspect, EffectInfo e)
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