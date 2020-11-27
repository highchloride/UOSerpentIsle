#region Header
//   Vorspire    _,-'/-'/  TimeWarp.cs
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
#endregion

namespace Server.Mobiles
{
	public class AspectAbilityTimeWarp : ExplodeAspectAbility
	{
		public override string Name { get { return "Time Warp"; } }

		public override AspectFlags Aspects { get { return AspectFlags.Time | AspectFlags.Illusion; } }

		public override TimeSpan Lockdown { get { return TimeSpan.FromSeconds(60); } }
		public override TimeSpan Cooldown { get { return TimeSpan.FromSeconds(30); } }

		public override TimeSpan Duration { get { return TimeSpan.FromSeconds(10); } }

		protected override BaseExplodeEffect CreateEffect(BaseAspect aspect)
		{
			return new EnergyExplodeEffect(aspect.Location, aspect.Map, Math.Max(5, aspect.RangePerception / 2));
		}

		protected override void OnLocked(BaseAspect aspect)
		{
			base.OnLocked(aspect);
			
			aspect.Yell("TIME... IS RELATIVE!");
		}

		protected override void OnUnlocked(BaseAspect aspect)
		{
			base.OnUnlocked(aspect);

			aspect.Yell("I SEE THE FUTURE. THERE IS NO FUTURE!");
		}

		protected override void OnDamage(BaseAspect aspect, Mobile target, ref int damage)
		{
			base.OnDamage(aspect, target, ref damage);

			Effects.SendBoltEffect(target, true, aspect.Hue);
		}

		protected override void OnAdded(State state)
		{
			base.OnAdded(state);

			if (state.Aspect == null || state.Target == null)
			{
				return;
			}

			state.Target.SendMessage(
				state.Aspect.SpeechHue,
				"[{0}]: Time is just an illusion... Let me show you.",
				state.Aspect.RawName);
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
			state.Target.SendMessage(85, "You escape the influence of the time warp.");
		}
	}
}