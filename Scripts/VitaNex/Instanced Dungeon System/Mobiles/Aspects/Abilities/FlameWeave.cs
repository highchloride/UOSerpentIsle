#region Header
//   Vorspire    _,-'/-'/  FlameWeave.cs
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

using Server.Network;

using VitaNex.FX;
using VitaNex.Network;
#endregion

namespace Server.Mobiles
{
	public class AspectAbilityFlameWeave : WaveAspectAbility
	{
		public override string Name { get { return "Flame Weave"; } }

		public override AspectFlags Aspects { get { return AspectFlags.Fire; } }

		public override TimeSpan Lockdown { get { return TimeSpan.FromSeconds(30); } }
		public override TimeSpan Cooldown { get { return TimeSpan.FromSeconds(20); } }

		public override TimeSpan Duration { get { return TimeSpan.FromSeconds(10); } }

		protected override BaseWaveEffect CreateEffect(BaseAspect aspect)
		{
			var dir = aspect.Direction & Direction.ValueMask;

			return new FireWaveEffect(aspect.Location, aspect.Map, dir, Math.Max(5, aspect.RangePerception / 2));
		}

		protected override void OnLocked(BaseAspect aspect)
		{
			base.OnLocked(aspect);

			aspect.Yell("BURN!");
		}

		protected override void OnDamage(BaseAspect aspect, Mobile target, ref int damage)
		{
			base.OnDamage(aspect, target, ref damage);

			if (Utility.RandomBool())
			{
				int x = 0, y = 0;

				Movement.Movement.Offset(aspect.GetDirectionTo(target), ref x, ref y);

				var loc = target.Clone3D(x, y, target.Map.GetTopZ(target.Clone2D(x, y)));

				if (target.Map.CanSpawnMobile(loc))
				{
					ScreenFX.LightFlash.Send(target);

					target.Location = loc;

					if (target.PlayDamagedAnimation())
					{
						target.PlayHurtSound();
					}
				}
			}
		}

		protected override void OnAdded(State state)
		{
			base.OnAdded(state);

			if (state.Aspect == null || state.Target == null)
			{
				return;
			}

			state.Target.SendMessage(state.Aspect.SpeechHue, "[{0}]: Burn!", state.Aspect.RawName);
			state.Target.Send(SpeedControl.WalkSpeed);
		}

		protected override void OnRemoved(State state)
		{
			base.OnRemoved(state);

			if (state.Target == null)
			{
				return;
			}

			state.Target.Send(SpeedControl.Disable);
			state.Target.SendMessage(85, "Your flaming tomb fades away.");
		}
	}
}