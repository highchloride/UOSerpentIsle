#region Header
//   Vorspire    _,-'/-'/  Swap.cs
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

using VitaNex.Collections;
using VitaNex.FX;
using VitaNex.Network;
#endregion

namespace Server.Mobiles
{
	public class AspectAbilitySwap : AspectAbility
	{
		public override string Name { get { return "Swap"; } }

		public override AspectFlags Aspects { get { return AspectFlags.Time | AspectFlags.Chaos | AspectFlags.Tech | AspectFlags.Illusion; } }

		public override TimeSpan Lockdown { get { return TimeSpan.FromSeconds(30); } }
		public override TimeSpan Cooldown { get { return TimeSpan.FromSeconds(15); } }

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

			var targets = ListPool<Mobile>.AcquireObject();

			targets.AddRange(AcquireTargets<Mobile>(aspect));

			if (targets.Count > 0)
			{
				using (var fx = new EffectInfo(aspect, aspect.Map, 14120, 0, 10, 10, EffectRender.Lighten))
				{
					fx.SoundID = 510;

					Mobile t;

					var i = targets.Count;

					foreach (var p in targets.Select(o => o.Location))
					{
						t = targets[--i];

						fx.SetSource(t);

						fx.Send();
						t.Location = p;
						fx.Send();
					}

					var l = aspect.Location;

					t = targets.GetRandom();

					fx.SetSource(aspect);

					fx.Send();
					aspect.Location = t.Location;
					fx.Send();

					t.Location = l;
				}
			}

			ObjectPool.Free(targets);
		}

		protected override void OnLocked(BaseAspect aspect)
		{
			base.OnLocked(aspect);

			aspect.Yell("TIME FOR A SWITCH!");
		}
	}
}