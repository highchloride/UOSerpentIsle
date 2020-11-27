#region Header
//   Vorspire    _,-'/-'/  Decrepify.cs
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

using VitaNex.FX;
using VitaNex.Network;
#endregion

namespace Server.Mobiles
{
	public class AspectAbilityDecrepify : ExplodeAspectAbility
	{
		public override string Name { get { return "Decrepify"; } }

		public override AspectFlags Aspects { get { return AspectFlags.Death; } }

		public override TimeSpan Lockdown { get { return TimeSpan.FromSeconds(60); } }
		public override TimeSpan Cooldown { get { return TimeSpan.FromSeconds(30); } }

		public override TimeSpan Duration { get { return TimeSpan.FromSeconds(10); } }

		protected override BaseExplodeEffect CreateEffect(BaseAspect aspect)
		{
			return new PoisonExplodeEffect(
				aspect.Location,
				aspect.Map,
				Math.Max(5, aspect.RangePerception / 2),
				0,
				TimeSpan.FromMilliseconds(500));
		}

		protected override void OnLocked(BaseAspect aspect)
		{
			base.OnLocked(aspect);

			aspect.Yell("YOUR SOUL IN CHAINS!");
		}

		protected override void OnUnlocked(BaseAspect aspect)
		{
			base.OnUnlocked(aspect);

			aspect.Yell("YOUR SOUL WITHERS!");
		}

		protected override void OnAdded(State state)
		{
			base.OnAdded(state);

			if (state.Aspect == null || state.Target == null)
			{
				return;
			}

			state.Target.TryParalyze(Duration);

			if (!state.Target.Paralyzed)
			{
				return;
			}

			state.Target.SendMessage("Decrepify paralyzes your soul!");

			new MovingEffectQueue(deferred: false)
			{
				new MovingEffectInfo(
					state.Target.Clone3D(-1, -1, 50),
					state.Target,
					state.Target.Map,
					8700,
					0,
					1,
					EffectRender.Darken),
				new MovingEffectInfo(
					state.Target.Clone3D(-1, +1, 50),
					state.Target,
					state.Target.Map,
					8700,
					0,
					1,
					EffectRender.Darken),
				new MovingEffectInfo(
					state.Target.Clone3D(+1, -1, 50),
					state.Target,
					state.Target.Map,
					8700,
					0,
					1,
					EffectRender.Darken),
				new MovingEffectInfo(
					state.Target.Clone3D(+1, +1, 50),
					state.Target,
					state.Target.Map,
					8700,
					0,
					1,
					EffectRender.Darken)
			}.Process();
		}

		protected override void OnRemoved(State state)
		{
			base.OnRemoved(state);

			if (state.Target == null)
			{
				return;
			}

			if (state.Target.Paralyzed)
			{
				state.Target.Paralyzed = false;
			}

			state.Target.SendMessage("Decrepify has faded, your soul is unchained!");
		}
	}
}