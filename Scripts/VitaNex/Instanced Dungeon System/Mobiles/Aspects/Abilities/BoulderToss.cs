#region Header
//   Vorspire    _,-'/-'/  BoulderToss.cs
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
using System.Linq;

using Server.Spells;

using VitaNex.FX;
#endregion

namespace Server.Mobiles
{
	public class AspectAbilityBoulderToss : AspectAbility
	{
		public override string Name { get { return "Boulder Toss"; } }

		public override AspectFlags Aspects { get { return AspectFlags.Earth; } }

		public override TimeSpan Lockdown { get { return TimeSpan.FromSeconds(20); } }
		public override TimeSpan Cooldown { get { return TimeSpan.FromSeconds(10); } }

		public override TimeSpan Duration { get { return TimeSpan.FromSeconds(5); } }

		protected override void OnInvoke(BaseAspect aspect)
		{
			if (aspect == null || aspect.Deleted)
			{
				return;
			}

			var t =
				AcquireTargets<Mobile>(aspect).Where(m => aspect.GetDistanceToSqrt(m) >= aspect.RangePerception * 0.25).GetRandom();

			if (t == null || t.Deleted || !t.Alive)
			{
				return;
			}

			aspect.CantWalk = true;

			SpellHelper.Turn(aspect, t);

			int x = 0, y = 0;

			Movement.Movement.Offset(aspect.Direction & Direction.Mask, ref x, ref y);

			var loc = aspect.Location.Clone3D(x, y, 5);

			if (aspect.PlayAttackAnimation())
			{
				aspect.PlayAttackSound();
			}

			new MovingEffectInfo(loc, t.Location, aspect.Map, 4534)
			{
				SoundID = 541
			}.MovingImpact(
				e =>
				{
					BoulderImpact(aspect, e.Target.Location, 4);
					aspect.CantWalk = false;
				});
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