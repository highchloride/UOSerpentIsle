#region Header
//   Vorspire    _,-'/-'/  WaveAspectAbility.cs
//   .      __,-; ,'( '/
//    \.    `-.__`-._`:_,-._       _ , . ``
//     `:-._,------' ` _,`--` -: `_ , ` ,' :
//        `---..__,,--'  (C) 2017  ` -'. -'
//        #  Vita-Nex [http://core.vita-nex.com]  #
//  {o)xxx|===============-   #   -===============|xxx(o}
//        #        The MIT License (MIT)          #
#endregion

#region References
using VitaNex.FX;
#endregion

namespace Server.Mobiles
{
	public abstract class WaveAspectAbility : AspectAbility
	{
		protected abstract BaseWaveEffect CreateEffect(BaseAspect aspect);

		protected override void OnInvoke(BaseAspect aspect)
		{
			var fx = CreateEffect(aspect);

			if (fx == null)
			{
				return;
			}

			fx.AverageZ = false;

			fx.EffectHandler = e =>
			{
				if (e.ProcessIndex != 0)
				{
					return;
				}

				foreach (var t in AcquireTargets<Mobile>(aspect, e.Source.Location, 0))
				{
					OnTargeted(aspect, t);
				}
			};

			fx.Send();
		}

		protected virtual void OnTargeted(BaseAspect aspect, Mobile target)
		{
			Damage(aspect, target);
		}
	}
}