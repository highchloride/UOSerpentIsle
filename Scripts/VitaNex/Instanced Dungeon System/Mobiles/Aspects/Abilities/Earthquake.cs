#region Header
//   Vorspire    _,-'/-'/  Earthquake.cs
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

using VitaNex.Network;
#endregion

namespace Server.Mobiles
{
	public class AspectAbilityEarthQuake : AspectAbility
	{
		public override string Name { get { return "Earthquake"; } }

		public override AspectFlags Aspects { get { return AspectFlags.Earth; } }

		public override TimeSpan Lockdown { get { return TimeSpan.FromSeconds(20); } }
		public override TimeSpan Cooldown { get { return TimeSpan.FromSeconds(20); } }

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

			foreach (var t in AcquireTargets<Mobile>(aspect))
			{
				ScreenFX.LightFlash.Send(t);

				t.PlaySound(1230);
				Damage(aspect, t);
				t.Paralyze(TimeSpan.FromSeconds(2.0));
			}
		}
	}
}