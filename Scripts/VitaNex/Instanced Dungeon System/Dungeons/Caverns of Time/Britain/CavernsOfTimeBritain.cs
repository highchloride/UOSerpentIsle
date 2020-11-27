#region Header
//   Vorspire    _,-'/-'/  CavernsOfTimeBritain.cs
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
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using Server;
using Server.Items;

using VitaNex.FX;
using VitaNex.Network;
#endregion

namespace VitaNex.Dungeons
{
	public sealed class CavernsOfTimeBritain : Dungeon
	{
		private static readonly Point3D[][] _SouthSpawnPoints =
		{
			//bank
			new[]
			{
				new Point3D(1441, 1706, 1), new Point3D(1443, 1706, 0), new Point3D(1442, 1707, 0), new Point3D(1418, 1703, 7),
				new Point3D(1421, 1704, 9), new Point3D(1422, 1706, 13), new Point3D(1416, 1684, 0), new Point3D(1416, 1688, 0),
				new Point3D(1413, 1657, 10), new Point3D(1413, 1659, 10), new Point3D(1426, 1671, 10), new Point3D(1429, 1671, 10),
				new Point3D(1428, 1667, 10), new Point3D(1445, 1671, 10), new Point3D(1409, 1717, 40), new Point3D(1411, 1717, 40),
				new Point3D(1409, 1716, 20)
			},
			//bridge
			new[]
			{
				new Point3D(1407, 1734, 9), new Point3D(1409, 1736, 9), new Point3D(1412, 1737, 12), new Point3D(1407, 1739, 5),
				new Point3D(1405, 1736, 7), new Point3D(1396, 1744, 1), new Point3D(1397, 1754, 0), new Point3D(1381, 1747, 1),
				new Point3D(1380, 1751, 1)
			},
			//square
			new[]
			{
				new Point3D(1483, 1715, 0), new Point3D(1487, 1718, 0), new Point3D(1485, 1712, 0), new Point3D(1503, 1703, 20),
				new Point3D(1503, 1707, 20)
			},
			//streets
			new[]
			{
				new Point3D(1425, 1747, 11), new Point3D(1426, 1749, 12), new Point3D(1435, 1747, 18), new Point3D(1432, 1730, 20),
				new Point3D(1434, 1733, 20), new Point3D(1450, 1714, 0), new Point3D(1450, 1716, 0), new Point3D(1453, 1714, 0),
				new Point3D(1466, 1715, 0), new Point3D(1466, 1723, 0), new Point3D(1473, 1715, 0), new Point3D(1458, 1696, 0),
				new Point3D(1457, 1700, 0), new Point3D(1461, 1699, 0), new Point3D(1461, 1679, 0), new Point3D(1461, 1687, 0),
				new Point3D(1458, 1665, 6), new Point3D(1447, 1656, 10), new Point3D(1505, 1645, 20), new Point3D(1500, 1646, 20),
				new Point3D(1514, 1649, 20), new Point3D(1505, 1670, 20), new Point3D(1505, 1672, 20), new Point3D(1516, 1672, 20),
				new Point3D(1528, 1674, 21), new Point3D(1503, 1682, 20)
			}
		};

		private static readonly Point3D[][] _NorthSpawnPoints =
		{
			//decay
			new[]
			{
				new Point3D(1464, 1620, 20), new Point3D(1457, 1625, 20), new Point3D(1452, 1631, 20), new Point3D(1449, 1622, 20),
				new Point3D(1447, 1626, 20), new Point3D(1441, 1630, 20), new Point3D(1434, 1630, 20), new Point3D(1439, 1621, 20),
				new Point3D(1431, 1618, 20), new Point3D(1426, 1625, 20), new Point3D(1419, 1629, 20), new Point3D(1424, 1619, 20),
				new Point3D(1410, 1634, 30), new Point3D(1412, 1634, 30), new Point3D(1410, 1615, 30), new Point3D(1412, 1615, 30)
			},
			//castle_gate
			new[]
			{
				new Point3D(1367, 1614, 50), new Point3D(1370, 1614, 50), new Point3D(1370, 1636, 50), new Point3D(1366, 1636, 50),
				new Point3D(1363, 1625, 50)
			},
			//castle_court
			new[]
			{new Point3D(1386, 1611, 30), new Point3D(1390, 1610, 30), new Point3D(1387, 1613, 30), new Point3D(1377, 1612, 30)},
			//castle_rooms
			new[]
			{new Point3D(1385, 1602, 30), new Point3D(1391, 1601, 30), new Point3D(1389, 1589, 30), new Point3D(1391, 1589, 30)},
			//castle_middle
			new[]
			{
				new Point3D(1354, 1637, 50), new Point3D(1353, 1638, 50), new Point3D(1355, 1652, 50), new Point3D(1354, 1651, 50),
				new Point3D(1353, 1611, 50), new Point3D(1355, 1613, 50), new Point3D(1353, 1598, 50), new Point3D(1355, 1596, 50)
			},
			//castle_throne
			new[] {new Point3D(1345, 1623, 50), new Point3D(1345, 1626, 50)}
		};

		private static readonly Point3D _CrystalPoint = new Point3D(1476, 1645, 37);

		private const TileFlag _FieldFlags =
			TileFlag.Damaging | TileFlag.Impassable | TileFlag.LightSource | TileFlag.Animation | TileFlag.NoShoot;

		private static void FixField(Item field)
		{
			//if (field.GetTileFlags() != _FieldFlags)
			{
				//field.SetTileFlags(_FieldFlags);
			}

			//if (field.GetTileHeight() < 20)
			{
				//field.SetTileHeight(20);
			}
		}

		public override DungeonID ID { get { return DungeonID.CoTB; } }

		public override Map MapParent { get { return Server.Map.Felucca; } }

		public override TimeSpan Duration { get { return TimeSpan.FromHours(12); } }
		public override TimeSpan Lockout { get { return TimeSpan.FromHours(24); } }

		public override Point3D Entrance { get { return new Point3D(1475, 1659, 10); } }
		public override Point3D Exit { get { return new Point3D(1322, 1624, 55); } }

		public override int GroupMax { get { return 10; } }

		public override int GoldMin { get { return 1000; } }
		public override int GoldMax { get { return 5000; } }

		public override int LootPropsMin { get { return 4; } }
		public override int LootPropsMax { get { return 8; } }

		public override int LootIntensityMin { get { return 80; } }
		public override int LootIntensityMax { get { return 100; } }

		public override double SpawnFactor { get { return 2.0; } }

		public override string Name { get { return "Caverns of Time: Britain"; } }

		public override string Desc
		{
			get
			{
				return
					"Britain was in a state of corruption having been overthrown by an evil power known as the 'Infernal Horde'. " +
					"The guards and civilians were unable to defend against an all-out assault " +
					"and called for assistance, which never came. " +
					"It is up to you and your party to provide assistance and correct the time-line..." +
					"The past, present and future of Britain depends on it!";
			}
		}

		public InfernalBoss Boss1 { get; private set; }
		public InfernalBoss Boss2 { get; private set; }
		public InfernalBoss Boss3 { get; private set; }
		public InfernalBoss Boss4 { get; private set; }

		public InfernalBoss[] Bosses { get; private set; }

        private DamageableItem _Crystal;

		private List<Static> _Fields1;
		private List<Static> _Fields2;

		private List<WayPoint> _WayPoints;

		public CavernsOfTimeBritain()
		{ }

		public CavernsOfTimeBritain(DungeonSerial serial)
			: base(serial)
		{ }

		protected override void OnGenerate()
		{
			base.OnGenerate();

			var z = CreateZone("Britain (Infernal Control)", new Rectangle2D(1280, 1400, 440, 400));

			if (z != null)
			{
				z.Music = MusicName.Death;
				//z.Ambiance = "5000";
				//z.EnterNotify = true;
				z.GoLocation = Entrance;
			}

			z = CreateZone("Staging Area", new Rectangle2D(1463, 1646, 25, 18));

			if (z != null)
			{
				z.Music = MusicName.Approach;
				//z.Ambiance = "5000";
				//z.EnterNotify = true;
				z.GoLocation = Entrance;
			}

			Components.Instance.Generate(this);

			GenerateWayPoints();
			GenerateBosses();
			GenerateSpawn();
			GenerateFields();
		}

		private void GenerateWayPoints()
		{
			if (_WayPoints != null)
			{
				_WayPoints.ForEachReverse(f => f.Delete());
				_WayPoints.Free(true);
			}
			else
			{
				_WayPoints = new List<WayPoint>();
			}

			var points = new[]
			{
				new Point3D(1419, 1669, 10), new Point3D(1434, 1668, 10), new Point3D(1444, 1679, 0), new Point3D(1444, 1697, 0),
				new Point3D(1419, 1696, 0)
			};

			foreach (var p in points)
			{
				_WayPoints.Add(CreateItem<WayPoint>(p, false));
			}

			for (var i = 0; i < _WayPoints.Count; i++)
			{
				_WayPoints[i].NextPoint = _WayPoints[(i + 1) % _WayPoints.Count];
			}
		}

		private void GenerateBosses()
		{
			if (Bosses != null)
			{
				Bosses.ForEachReverse(f => f.Delete());
				Bosses.Clear();
			}
			else
			{
				Bosses = new InfernalBoss[4];
			}

			Bosses[0] = Boss1 = CreateMobile<InfernalCaptain>(new Point3D(1481, 1703, 0), true, false);
			Bosses[1] = Boss2 = CreateMobile<InfernalGeneral>(new Point3D(1417, 1643, 20), true, false);
			Bosses[2] = Boss3 = CreateMobile<InfernalCommander>(new Point3D(1386, 1632, 30), true, false);
			Bosses[3] = Boss4 = CreateMobile<InfernalOverlord>(new Point3D(1325, 1624, 55), true, false);

			var boss = Boss2;

			if (boss != null)
			{
				if (!_WayPoints.IsNullOrEmpty())
				{
					boss.ApproachWait = false;
					boss.CurrentWayPoint = _WayPoints[0];
				}

				for (var i = 0; i < 3; i++)
				{
					var c = CreateMobile<InfernalStalker>(boss.GetRandomPoint3D(1, 3), false, true);

					if (c != null && boss.CurrentWayPoint != null)
					{
						c.ApproachWait = false;
						c.CurrentWayPoint = boss.CurrentWayPoint;
					}
				}
			}
		}

		private void GenerateSpawn()
		{
			#region South
			var index = 0;

			var types = new[] {typeof(InfernalMinion), typeof(InfernalDrone)};

			var o = Enumerable.Empty<Point3D>().Union(_SouthSpawnPoints[index++]).Union(_SouthSpawnPoints[index++]);

			foreach (var p in o)
			{
				CreateMobile(types.GetRandom(), p, false, true);
			}

			types = new[] {typeof(InfernalMinion), typeof(InfernalDrone), typeof(InfernalFlame)};

			o = Enumerable.Empty<Point3D>().Union(_SouthSpawnPoints[index++]).Union(_SouthSpawnPoints[index++]);

			foreach (var p in o)
			{
				CreateMobile(types.GetRandom(), p, false, true);
			}
			#endregion

			#region North
			index = 0;

			types = new[] {typeof(InfernalStalker), typeof(InfernalConstruct)};

			o = Enumerable.Empty<Point3D>().Union(_NorthSpawnPoints[index++]).Union(_NorthSpawnPoints[index++]);

			foreach (var p in o)
			{
				CreateMobile(types.GetRandom(), p, false, true);
			}

			types = new[] {typeof(InfernalStalker), typeof(InfernalMinion), typeof(InfernalFlame)};

			o = Enumerable.Empty<Point3D>().Union(_NorthSpawnPoints[index++]).Union(_NorthSpawnPoints[index++]);

			foreach (var p in o)
			{
				CreateMobile(types.GetRandom(), p, false, true);
			}

			types = new[] {typeof(InfernalConstruct), typeof(InfernalStalker), typeof(InfernalDrone)};

			o = Enumerable.Empty<Point3D>().Union(_NorthSpawnPoints[index++]).Union(_NorthSpawnPoints[index++]);

			foreach (var p in o)
			{
				CreateMobile(types.GetRandom(), p, false, true);
			}
			#endregion
		}

		public override Mobile CreateMobile(Type type, Point3D p, bool replacePack, bool scale, double factor, object[] args)
		{
			var m = base.CreateMobile(type, p, replacePack, scale, factor, args);

			if (m is InfernalStalker)
			{
				m.Hidden = true;
				m.IsStealthing = true;
			}

			return m;
		}

        private void GenerateFields()
        {
            GenerateCrystal();
            GenerateField1();
            GenerateField2();

            FixFields();
        }

        private void GenerateCrystal()
        {
            if (_Crystal != null)
            {
                _Crystal.Delete();
                _Crystal = null;
            }

			_Crystal = CreateItem<DamageableItem>(_CrystalPoint, false, new object[] { 13807 });

            if (_Crystal != null)
            {
                _Crystal.Name = "Infernal Energy Crystal";
                _Crystal.Hue = 2075;
                _Crystal.Light = LightType.DarkCircle300;
                _Crystal.Hits = _Crystal.HitsMax = 10000;
                _Crystal.CanDamage = true;
            }
        }

        private void GenerateField1()
		{
			if (_Fields1 != null)
			{
				_Fields1.ForEachReverse(f => f.Delete());
				_Fields1.Free(true);
			}
			else
			{
				_Fields1 = new List<Static>();
			}

			_Fields1.AddRange(TileStatic(14662, new Point3D(1461, 1645, 20), 28, 1, true));
			_Fields1.AddRange(TileStatic(13040, new Point3D(1475, 1644, 20), 2, 2, true));
		}

		private void GenerateField2()
		{
			if (_Fields2 != null)
			{
				_Fields2.ForEachReverse(f => f.Delete());
				_Fields2.Free(true);
			}
			else
			{
				_Fields2 = new List<Static>();
			}

			_Fields2.AddRange(TileStatic(14678, new Point3D(1373, 1622, 50), 1, 8, true));
			_Fields2.Add(CreateStatic(13807, new Point3D(1373, 1622, 50), true));
			_Fields2.Add(CreateStatic(13807, new Point3D(1373, 1629, 50), true));
		}

		private void FixFields()
		{
			var fields = Enumerable.Empty<Static>();

			fields = fields.Union(_Fields1);
			fields = fields.Union(_Fields2);

			foreach (var field in fields)
			{
				field.Hue = 2075;

				switch (field.ItemID)
				{
					case 14662:
					case 14678:
					{
						field.Light = LightType.DarkCircle300;
						field.Name = "Infernal Barrier";

						FixField(field);
					}
						break;
					case 13807:
					{
						field.Light = LightType.DarkCircle300;
						field.Name = "Infernal Barrier Generator";
					}
						break;
					default:
						field.Name = "Infernal Structure";
						break;
				}
			}
		}

		private void CheckBossFields()
		{
			CheckBossField1();
			CheckBossField2();
		}

		private void CheckBossField1()
		{
			if (Deleted || _Fields1.IsNullOrEmpty())
			{
				return;
			}

			if (_Crystal != null)
			{
				if (_Crystal.Deleted)
				{
					_Crystal = null;
				}
				else if ((Boss1 == null || Boss1.Deleted || !Boss1.Alive || Boss1.Map != Map) &&
						 (Boss2 == null || Boss2.Deleted || !Boss2.Alive || Boss2.Map != Map))
				{
					if (_Crystal.CanDamage)
					{
                        _Crystal.CanDamage = false;

						GroupMessage(0x22, "Energy seems to be draining from the barrier crystal...  It is vulnerable to attack!");
					}
				}
				else
				{
					_Crystal.CanDamage = true;
				}
			}

			if (_Crystal == null)
            {
                _Fields1.ForEachReverse(
                    f =>
					{
						var delay = f.GetTravelTime(_CrystalPoint, 10);

						new EffectInfo(f.Location.Clone3D(0, 0, -5), f.Map, 14120, 2075, 10, 10, EffectRender.SemiTransparent, delay)
						{
							SoundID = 1306
						}.Send();

                        f.Delete();
                    });

                _Fields1.Free(true);
            }
            else
            {
                var mobs = _Fields1.SelectMany(f => f.FindPlayersInRange(f.Map, 1)).Distinct();

                foreach (var m in mobs.Where(m => m != null && !m.Deleted && m.Alive))
                {
                    Effects.SendBoltEffect(m, true, 2075);
                    ScreenFX.DarkFlash.Send(m);
                }
            }
        }

		private void CheckBossField2()
		{
			if (Deleted || _Fields2.IsNullOrEmpty())
			{
				return;
			}

			if (Boss3 == null || Boss3.Deleted || !Boss3.Alive || Boss3.Map != Map)
			{
				_Fields2.ForEachReverse(
					f =>
					{
						new EffectInfo(f.Location.Clone3D(0, 0, -5), f.Map, 14120, 2075, 10, 10, EffectRender.SemiTransparent)
						{
							SoundID = 1306
						}.Send();

						f.Delete();
					});

				_Fields2.Free(true);
			}
			else
			{
				var mobs = _Fields2.SelectMany(f => f.FindPlayersInRange(f.Map, 1)).Distinct();

				foreach (var m in mobs.Where(m => m != null && !m.Deleted && m.Alive))
				{
					Effects.SendBoltEffect(m, true, 2075);
					ScreenFX.DarkFlash.Send(m);
				}
			}
		}

        protected override void MutateLootItem(Item item)
        {
            base.MutateLootItem(item);

            if (item is IWearableDurability)
                item.Hue = 2075;
        }

		public override void OnAlterLightLevel(DungeonZone zone, Mobile m, ref int global, ref int personal)
		{
			base.OnAlterLightLevel(zone, m, ref global, ref personal);

			if (zone.Name != "Staging Area")
			{
				global = Math.Min(5, global);
				personal = Math.Min(10, personal);
			}
		}

		public override bool CheckCanMoveThrough(DungeonZone zone, Mobile m, IEntity e)
		{
			if (m != null && !m.Deleted && e != null && !e.Deleted && e is Static)
			{
				var s = (Static)e;

				if (s.Name == "Infernal Barrier" && (s.ItemID == 14662 || s.ItemID == 14678))
				{
					return false;
				}
			}

			return base.CheckCanMoveThrough(zone, m, e);
		}

		protected override void OnSlice()
		{
			base.OnSlice();

			CheckBossFields();
		}

		protected override void OnDelete()
		{
			base.OnDelete();

			if (_Fields1 != null)
			{
				_Fields1.ForEachReverse(f => f.Delete());
				_Fields1.Free(true);
			}
		}

		protected override void OnAfterDelete()
		{
			base.OnAfterDelete();

			Boss1 = Boss2 = Boss3 = Boss4 = null;

			if (Bosses != null)
			{
				Bosses.Clear();
				Bosses = null;
			}
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			var v = writer.SetVersion(1);

            if (v > 0)
            {
                writer.Write(_Crystal);
                writer.WriteItemList(_Fields1, true);
                writer.WriteItemList(_Fields2, true);
            }
            else
            {
                writer.WriteItemList(_Fields1, true);
            }

            writer.WriteItemList(_WayPoints, true);

			writer.Write(Boss1);
			writer.Write(Boss2);
			writer.Write(Boss3);
			writer.Write(Boss4);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			var v = reader.GetVersion();

            if (v > 0)
            {
                _Crystal = reader.ReadItem<DamageableItem>();
                _Fields1 = reader.ReadStrongItemList<Static>();
                _Fields2 = reader.ReadStrongItemList<Static>();
            }
            else
            {
                _Fields1 = reader.ReadStrongItemList<Static>();
            }

            _WayPoints = reader.ReadStrongItemList<WayPoint>();

			Boss1 = reader.ReadMobile<InfernalBoss>();
			Boss2 = reader.ReadMobile<InfernalBoss>();
			Boss3 = reader.ReadMobile<InfernalBoss>();
			Boss4 = reader.ReadMobile<InfernalBoss>();

			Bosses = new[] {Boss1, Boss2, Boss3, Boss4};

            if (v < 1)
            {
                GenerateFields();
            }
        }

		public sealed class Components : DungeonComponents<CavernsOfTimeBritain>
		{
			public static Components Instance { get; private set; }

			static Components()
			{
				Instance = new Components();
			}

			private Components()
			{ }

			protected override void OnGenerate(CavernsOfTimeBritain dungeon)
			{
				LoadFile(dungeon, @"Data\Dungeons\CoT\Britain\components.cfg");
			}
		}
	}
}
