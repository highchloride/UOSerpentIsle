#region Header
//   Vorspire    _,-'/-'/  GammaRay.cs
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

using Server.Misc;
using Server.Spells;

using VitaNex.Collections;
using VitaNex.FX;
using VitaNex.Network;
#endregion

namespace Server.Mobiles
{
	public class AspectAbilityGammaRay : AspectAbility
	{
		private static readonly TileFlag[] _BlockingFlags = //
			{ TileFlag.Impassable, TileFlag.Wall, TileFlag.Roof, TileFlag.Door };

		public override string Name { get { return "Gamma Ray"; } }

		public override AspectFlags Aspects { get { return AspectFlags.Energy | AspectFlags.Light | AspectFlags.Tech; } }

		public override TimeSpan Lockdown { get { return TimeSpan.FromSeconds(30); } }
		public override TimeSpan Cooldown { get { return TimeSpan.FromSeconds(20); } }

		public override TimeSpan Duration { get { return TimeSpan.FromSeconds(20); } }

		protected override void OnInvoke(BaseAspect aspect)
		{
			if (aspect == null || aspect.Deleted)
			{
				return;
			}

			var list = ListPool<EffectQueue>.AcquireObject();
			var cw = Utility.RandomBool();

			for (int a = (cw ? 0 : 360), h = 0; (cw ? a <= 360 : a >= 0); a += (cw ? 1 : -1))
			{
				var x = (int)Math.Round(aspect.X + (aspect.RangePerception * Math.Sin(Geometry.DegreesToRadians(a))));
				var y = (int)Math.Round(aspect.Y + (aspect.RangePerception * Math.Cos(Geometry.DegreesToRadians(a))));

				if (((x * 397) ^ y) == h)
				{
					// This location was just handled, ignore it to avoid small increments
					continue;
				}

				h = ((x * 397) ^ y);

				var start = aspect.Clone3D(0, 0, 10);
				var end = new Point3D(x, y, aspect.Z);

				end.Z = end.GetTopZ(aspect.Map);

				var l = start.GetLine3D(end, aspect.Map, false);

				var q = new EffectQueue
				{
					Deferred = false,
					Handler = e => HandleGammaRay(aspect, e)
				};

				for (var i = 0; i < l.Length; i++)
				{
					var p = new Block3D(l[i], 5);
					var blocked = i + 1 >= l.Length;

					if (!blocked)
					{
						var land = aspect.Map.GetLandTile(p);

						if (p.Intersects(land.Z, land.Height))
						{
							var o = TileData.LandTable[land.ID];

							if (o.Flags.AnyFlags(_BlockingFlags))
							{
								blocked = true;
							}
						}
					}

					if (!blocked)
					{
						var tiles = aspect.Map.GetStaticTiles(p);

						var data = tiles.Where(o => p.Intersects(o.Z, o.Height)).Select(t => TileData.ItemTable[t.ID]);

						if (data.Any(o => o.Flags.AnyFlags(_BlockingFlags)))
						{
							blocked = true;
						}
					}

					if (!blocked)
					{
						var items = p.FindItemsAt(aspect.Map);

						var data = items.Where(p.Intersects).Select(o => TileData.ItemTable[o.ItemID]);

						if (data.Any(o => o.Flags.AnyFlags(_BlockingFlags)))
						{
							blocked = true;
						}
					}

					var effect = blocked ? 14120 : Utility.RandomMinMax(12320, 12324);
					var hue = blocked ? 0 : 2050;

					if (blocked)
					{
						p = p.Clone3D(0, 0, -8);
					}

					q.Add(
						new EffectInfo(p, aspect.Map, effect, hue, 10, 10, EffectRender.LightenMore)
						{
							QueueIndex = i
						});

					if (blocked)
					{
						break;
					}
				}

				if (q.Queue.Count > 0)
				{
					list.Add(q);
				}
			}

			if (list.Count == 0)
			{
				ObjectPool.Free(list);

				return;
			}

			for (var i = 0; i < list.Count; i++)
			{
				var cur = list[i];

				if (i + 1 < list.Count)
				{
					var next = list[i + 1];

					cur.Callback = () =>
					{
						if (aspect.Deleted || !aspect.Alive)
						{
							list.ForEach(q => q.Dispose());

							ObjectPool.Free(list);

							return;
						}

						if (next.Queue.Count > 0)
						{
							SpellHelper.Turn(aspect, next.Queue.Last().Source);
						}

						Timer.DelayCall(TimeSpan.FromSeconds(0.05), next.Process);
					};
				}
				else
				{
					cur.Callback = () =>
					{
						list.ForEach(q => q.Dispose());

						ObjectPool.Free(list);

						aspect.CantWalk = false;
						aspect.LockDirection = false;
					};
				}
			}

			if (list.Count == 0)
			{
				ObjectPool.Free(list);

				return;
			}

			aspect.CantWalk = true;
			aspect.LockDirection = true;

			if (list[0].Queue.Count > 0)
			{
				SpellHelper.Turn(aspect, list[0].Queue.Last().Source);
			}

			Timer.DelayCall(TimeSpan.FromSeconds(0.1), list[0].Process);
		}

		private void HandleGammaRay(BaseAspect aspect, EffectInfo e)
		{
			if (aspect.Deleted || !aspect.Alive || e.ProcessIndex != 0)
			{
				return;
			}

			foreach (var t in AcquireTargets<Mobile>(aspect, e.Source.Location, 0))
			{
				Effects.SendBoltEffect(t, true, e.Hue);

				Damage(aspect, t);
			}
		}
	}
}