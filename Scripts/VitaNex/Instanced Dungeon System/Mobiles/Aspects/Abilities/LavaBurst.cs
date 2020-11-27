#region Header
//   Vorspire    _,-'/-'/  LavaBurst.cs
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

using Server.Spells.Fourth;

using VitaNex.FX;
using VitaNex.Network;
#endregion

namespace Server.Mobiles
{
	public class AspectAbilityLavaBurst : AspectAbility
	{
		public override string Name { get { return "Lava Burst"; } }

		public override AspectFlags Aspects { get { return AspectFlags.Earth | AspectFlags.Fire; } }

		public override TimeSpan Lockdown { get { return TimeSpan.FromSeconds(45); } }
		public override TimeSpan Cooldown { get { return TimeSpan.FromSeconds(30); } }

		public override double DamageFactor { get { return 0.5; } }

		public override bool CanInvoke(BaseAspect aspect)
		{
			return base.CanInvoke(aspect) && aspect.Map.HasLand(aspect) && !aspect.Map.HasWater(aspect);
		}

		protected override void OnInvoke(BaseAspect aspect)
		{
			if (aspect == null || aspect.Deleted)
			{
				return;
			}

			var map = aspect.Map;
			var x = aspect.X;
			var y = aspect.Y;
			var z = aspect.Z;

			var count = Utility.RandomMinMax(5, 10);
			var sect = 360 / count;

			var shift = Utility.RandomMinMax(0, sect);
			var range = Math.Max(3, aspect.RangePerception - 3);

			for (var i = 0; i < count; i++)
			{
				var t = Angle.GetPoint3D(x, y, z, shift + (i * sect), range);
				var l = aspect.PlotLine3D(t).TakeWhile(p => map.HasLand(p) && !map.HasWater(p));

				var q = new EffectQueue(range);

				var c = -1;

				foreach (var p in l)
				{
					q.Add(
						new EffectInfo(p, map, 14089, 0, 8, 15, EffectRender.Darken)
						{
							QueueIndex = ++c
						});
				}

				if (q.Count == 0)
				{
					q.Dispose();
					continue;
				}

				q.Handler = fx =>
				{
					var isEnd = fx.QueueIndex >= c;

					if (!TryBurst(aspect, fx, ref isEnd) || isEnd)
					{
						q.Clear();
					}
				};

				q.Callback = q.Dispose;

				Timer.DelayCall(
					TimeSpan.FromSeconds(0.2 * i),
					() =>
					{
						aspect.Direction = aspect.GetDirection(t);

						if (aspect.PlayAttackAnimation())
						{
							aspect.PlayAttackSound();
						}

						q.Process();
					});
			}
		}

		protected virtual bool TryBurst(BaseAspect aspect, EffectInfo e, ref bool isEnd)
		{
			if (aspect.Deleted || !aspect.Alive)
			{
				return false;
			}

			if (!isEnd)
			{
				var lf = TileData.LandTable[e.Map.GetLandTile(e.Source).ID].Flags;

				if (lf.AnyFlags(TileFlag.Door, TileFlag.Impassable, TileFlag.NoShoot, TileFlag.Wall))
				{
					isEnd = true;
				}
			}

			if (!isEnd)
			{
				var flags = e.Map.GetStaticTiles(e.Source).Select(t => TileData.ItemTable[t.ID].Flags);

				if (flags.Any(tf => tf.AnyFlags(TileFlag.Door, TileFlag.Impassable, TileFlag.NoShoot, TileFlag.Wall)))
				{
					isEnd = true;
				}
			}

			if (!isEnd)
			{
				var flags = e.Source.FindItemsInRange(e.Map, 0).Select(o => TileData.ItemTable[o.ItemID].Flags);

				if (flags.Any(f => f.AnyFlags(TileFlag.Door, TileFlag.Impassable, TileFlag.NoShoot, TileFlag.Wall)))
				{
					isEnd = true;
				}
			}

			if (!isEnd)
			{
				isEnd = AcquireTargets<Mobile>(aspect, 0, false).Any();
			}

			if (!isEnd)
            {
                new FireFieldSpell.FireFieldItem(6571, e.Source.Location, aspect, e.Map, TimeSpan.FromSeconds(5.0), 1)
                {
                    Hue = e.Hue
                };

				return true;
			}

			new TornadoEffect(e.Source, e.Map, aspect.GetDirection(e.Source), 3)
			{
				Size = 3,
				Climb = 5,
				Height = 40,
				EffectMutator = efx =>
				{
					efx.Hue = e.Hue;
					efx.EffectID = 14027;
					efx.SoundID = 519;
				},
				EffectHandler = efx =>
				{
					if (efx.ProcessIndex != 0 || efx.Source.Z > e.Source.Z)
					{
						return;
					}

					foreach (var t in AcquireTargets<Mobile>(aspect, efx.Source.Location, 0, false))
					{
						Damage(aspect, t);
					}

                    new FireFieldSpell.FireFieldItem(
                        6571,
                        efx.Source.Location,
                        aspect,
                        efx.Map,
                        TimeSpan.FromSeconds(5.0),
                        1)
                    {
                        Hue = efx.Hue
                    };
                }
			}.Send();

			return true;
		}

		protected override void OnDamage(BaseAspect aspect, Mobile target, ref int damage)
		{
			base.OnDamage(aspect, target, ref damage);

			using (var fx = new EffectInfo(target, target.Map, 14000, 0, 10, 30))
			{
				fx.SoundID = 519;
				fx.Send();
			}

			target.TryParalyze(TimeSpan.FromSeconds(1.0));
			target.Z = Math.Max(target.Z, Math.Min(aspect.Z + 40, target.Z + 5));
		}
	}
}