#region Header
//   Vorspire    _,-'/-'/  Hailstorm.cs
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
	public class AspectAbilityHailstorm : AspectAbility
	{
		public override string Name { get { return "Hailstorm"; } }

		public override AspectFlags Aspects { get { return AspectFlags.Frost; } }

		public override TimeSpan Lockdown { get { return TimeSpan.FromSeconds(90); } }
		public override TimeSpan Cooldown { get { return TimeSpan.FromSeconds(30); } }

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
				var loc = aspect.Location.GetRandomPoint2D(range, range).GetSurfaceTop(aspect.Map);

				Timer.DelayCall(
					TimeSpan.FromMilliseconds(delay),
					() =>
					{
						SpellHelper.Turn(aspect, loc);

						if (aspect.PlayAttackAnimation())
						{
							aspect.PlayAttackSound();
						}

						Hailstorm(aspect, loc);
					});
			}
		}

		private void Hailstorm(BaseAspect aspect, Point3D loc)
		{
			var effect = Utility.RandomMinMax(9006, 9007);

			new MovingEffectInfo(loc.Clone3D(-4, 0, 15), loc.Clone3D(0, 0, 4), aspect.Map, effect, 2999, 1).Send();
			new MovingEffectInfo(loc.Clone3D(0, 0, 60), loc.Clone3D(0, 0, 5), aspect.Map, effect, 0, 4)
			{
				SoundID = 247
			}.MovingImpact(e => HailstormImpact(aspect, e.Target.Location, 4));
		}

		private void HailstormImpact(BaseAspect aspect, Point3D loc, int blastRange)
		{
			if (aspect.Deleted || !aspect.Alive)
			{
				return;
			}

			Effects.PlaySound(loc, aspect.Map, Utility.RandomBool() ? 910 : 912);

			new AirExplodeEffect(loc, aspect.Map, blastRange)
			{
				AverageZ = false,
				EffectMutator = e => e.SoundID = 21,
				EffectHandler = e => HailstormBlast(aspect, e)
			}.Send();
		}

		private void HailstormBlast(BaseAspect aspect, EffectInfo e)
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