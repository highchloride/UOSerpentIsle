#region Header
//   Vorspire    _,-'/-'/  PoisonGas.cs
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
#endregion

namespace Server.Mobiles
{
	public class AspectAbilityPoisonGas : ExplodeAspectAbility
	{
		public override string Name { get { return "Poison Gas"; } }

		public override AspectFlags Aspects
		{
			get
			{
				return AspectFlags.Elements | AspectFlags.Death | AspectFlags.Famine | AspectFlags.Decay | AspectFlags.Poison;
			}
		}

		public override TimeSpan Lockdown { get { return TimeSpan.FromSeconds(30); } }
		public override TimeSpan Cooldown { get { return TimeSpan.FromSeconds(20); } }

		public override TimeSpan Duration { get { return TimeSpan.FromSeconds(10); } }

		protected override BaseExplodeEffect CreateEffect(BaseAspect aspect)
		{
			return new PoisonExplodeEffect(aspect.Location, aspect.Map, Math.Max(5, aspect.RangePerception / 2));
		}

		protected override void OnDamage(BaseAspect aspect, Mobile target, ref int damage)
		{
			base.OnDamage(aspect, target, ref damage);

			if (target.ApplyPoison(aspect, Poison.Lethal) != ApplyPoisonResult.Poisoned)
			{
				damage *= 2;
			}
		}
	}
}