#region Header
//   Vorspire    _,-'/-'/  ReflectSpell.cs
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
#endregion

namespace Server.Mobiles
{
	public class AspectAbilityReflectSpell : AspectAbility
	{
		public override string Name { get { return "Reflect Spells"; } }

		public override AspectFlags Aspects { get { return AspectFlags.All; } }

		public override TimeSpan Lockdown { get { return TimeSpan.FromSeconds(60); } }
		public override TimeSpan Cooldown { get { return TimeSpan.FromSeconds(30); } }

		public override TimeSpan Duration { get { return TimeSpan.FromSeconds(10); } }

		public override bool CanInvoke(BaseAspect aspect)
		{
			return base.CanInvoke(aspect) && !aspect.ReflectSpell && !aspect.ReflectMelee;
		}

		protected override void OnInvoke(BaseAspect aspect)
		{
			if (aspect == null || aspect.Deleted)
			{
				return;
			}

			aspect.ReflectSpell = true;

			Timer.DelayCall(Duration, a => a.ReflectSpell = false, aspect);
		}
	}
}