#region Header
//   Vorspire    _,-'/-'/  SewersOfBritain.cs
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
using System.Linq;

using Server;
using Server.Items;
using Server.Mobiles;
#endregion

namespace VitaNex.Dungeons
{
	public sealed class SewersOfBritain : Dungeon
	{
		public override DungeonID ID { get { return DungeonID.SoB; } }

		public override Map MapParent { get { return Server.Map.Felucca; } }

		public override TimeSpan Duration { get { return TimeSpan.FromHours(4.0); } }
		public override TimeSpan Lockout { get { return TimeSpan.FromHours(6.0); } }
		
		public override Point3D Entrance { get { return new Point3D(6032, 1500, 25); } }
		public override Point3D Exit { get { return new Point3D(6141, 1430, 4); } }

		public override int GroupMax { get { return 5; } }

		public override int GoldMin { get { return 500; } }
		public override int GoldMax { get { return 2500; } }

		public override int LootPropsMin { get { return 3; } }
		public override int LootPropsMax { get { return 5; } }

		public override int LootIntensityMin { get { return 60; } }
		public override int LootIntensityMax { get { return 100; } }

		public override string Name { get { return "Sewers of Britain"; } }
		public override string Desc { get { return "Something stinks in the sewers, and it's not just the water..."; } }

		public Mobile Boss1 { get; private set; }
		public Mobile Boss2 { get; private set; }
		public Mobile Boss3 { get; private set; }

		private List<Static>[] _BossFields = {new List<Static>(), new List<Static>(), new List<Static>()};

		public SewersOfBritain()
		{ }

		public SewersOfBritain(DungeonSerial serial)
			: base(serial)
		{ }
		
		protected override void OnGenerate()
		{
			base.OnGenerate();

			CreateZone("Sewers", new Rectangle2D(5886, 1281, 257, 254));
			//CreateZone("Control Room", new Rectangle2D(6036, 1430, 27, 14), new Rectangle2D(6042, 1444, 15, 10));

			var flushy = CreateMobile<Alligator>(new Point3D(6038, 1496, 0), false, true);

			flushy.Name = "Flushy";
			flushy.Hue = 2967;

			if (Utility.RandomDouble() < 0.05)
			{
				flushy.Tamable = false;
				flushy.IsParagon = true;
			}

			GenerateStairs();
			GenerateTraps();

			GenerateEasySpawn();
			GenerateHardSpawn();
			GenerateBossSpawn();
		}

		private void GenerateStairs()
		{
			var tiles = new[] { 1959, 1958, 1957, 1956, 1962, 767, 766 };
			
			var points = new[]
			{
				//1959
				new[]
				{
					new Point3D(6082, 1449, 5), new Point3D(6082, 1450, 5), new Point3D(6036, 1469, 5), new Point3D(6035, 1457, 5),
					new Point3D(6047, 1467, 5), new Point3D(6077, 1485, 5), new Point3D(6115, 1450, 5), new Point3D(6112, 1482, -20),
					new Point3D(6113, 1482, -15), new Point3D(6112, 1483, -20), new Point3D(6113, 1483, -15),
					new Point3D(6041, 1442, 6), new Point3D(6041, 1442, 6), new Point3D(6041, 1440, 6), new Point3D(6045, 1435, 0),
					new Point3D(6045, 1434, 0), new Point3D(6051, 1435, 6), new Point3D(6051, 1434, 6)
				},
				//1958
				new[] {new Point3D(6084, 1447, 0), new Point3D(6036, 1480, 5), new Point3D(6041, 1489, 6)},
				//1957
				new[]
				{
					new Point3D(6084, 1449, 5), new Point3D(6084, 1450, 5), new Point3D(6037, 1469, 0), new Point3D(6036, 1457, 0),
					new Point3D(6048, 1467, 0), new Point3D(6078, 1485, 0), new Point3D(6116, 1450, 0), new Point3D(6046, 1435, 6),
					new Point3D(6046, 1434, 6)
				},
				//1956
				new[] {new Point3D(6036, 1481, 0), new Point3D(6041, 1490, 0)},
				//1962,
				new[] {new Point3D(6084, 1448, 5)},
				//767,
				new[] {new Point3D(6105, 1454, 25), new Point3D(6105, 1455, 25)},
				//766,
				new[] {new Point3D(6106, 1455, 25)}
			};

			for (int i = 0, id; i < tiles.Length; i++)
			{
				id = tiles[i];

				foreach (var p in points[i])
				{
					CreateStatic(id, p, true);
				}
			}
		}

		private void GenerateTraps()
		{
			var traps = new[]
			{
				new Point3D(6032, 1488, 5), new Point3D(6035, 1479, 5), new Point3D(6034, 1463, 5), new Point3D(6049, 1470, 5),
				new Point3D(6064, 1472, 5), new Point3D(6064, 1468, 5), new Point3D(6048, 1492, 5), new Point3D(6066, 1491, 5),
				new Point3D(6073, 1506, 5), new Point3D(6074, 1484, 5), new Point3D(6090, 1433, 5), new Point3D(6090, 1457, 5),
				new Point3D(6086, 1443, 0), new Point3D(6089, 1443, 0), new Point3D(6084, 1443, 0)
			};

			foreach (var t in traps.Select(p => CreateItem<GasTrap>(p, false)).Where(t => t != null))
			{
				t.Movable = false;

				t.Type = GasTrapType.Floor;
				t.Poison = Poison.GetPoison(Utility.Random(5)) ?? Poison.Deadly;
			}
		}

		private void GenerateEasySpawn()
		{
			var types = new[] {typeof(Sewerling), typeof(AnimatedSludge), typeof(BoneFlayer)};

			var points = new[]
			{
				new Point3D(6033, 1482, 0), new Point3D(6034, 1477, 5), new Point3D(6039, 1479, 5), new Point3D(6041, 1478, 5),
				new Point3D(6038, 1472, 0), new Point3D(6035, 1469, 5), new Point3D(6046, 1470, 5), new Point3D(6046, 1467, 5),
				new Point3D(6048, 1475, 5), new Point3D(6048, 1477, 5), new Point3D(6049, 1486, 5), new Point3D(6049, 1488, 5),
				new Point3D(6034, 1458, 5), new Point3D(6034, 1460, 5), new Point3D(6058, 1467, 5), new Point3D(6058, 1471, 5),
				new Point3D(6056, 1472, 5), new Point3D(6056, 1469, 5), new Point3D(6059, 1490, 5), new Point3D(6061, 1490, 5),
				new Point3D(6063, 1490, 5), new Point3D(6072, 1485, 10), new Point3D(6076, 1486, 5), new Point3D(6076, 1484, 5)
			};

			foreach (var p in points)
			{
				CreateMobile(types.GetRandom(), p, false, true);
			}

			// Water

			types = new[] {typeof(SludgeElemental), typeof(MutatedTentacles)};
			points = new[]
			{
				new Point3D(6051, 1465, 0), new Point3D(6037, 1458, 0), new Point3D(6078, 1493, 0), new Point3D(6087, 1446, 0),
				new Point3D(6087, 1457, 0), new Point3D(6087, 1476, 0), new Point3D(6090, 1485, 0), new Point3D(6072, 1451, 0),
				new Point3D(6091, 1493, 0), new Point3D(6111, 1484, -20), new Point3D(6117, 1452, 0)
			};

			foreach (var p in points)
			{
				CreateMobile(types.GetRandom(), p, false, true);
			}
		}

		private void GenerateHardSpawn()
		{
			var types = new[] {typeof(SewerMutant), typeof(SewerFiend), typeof(FecalConstruct)};
			var points = new[]
			{
				new Point3D(6092, 1447, 5), new Point3D(6090, 1460, 5), new Point3D(6090, 1463, 5), new Point3D(6090, 1467, 6),
				new Point3D(6107, 1452, 25), new Point3D(6107, 1454, 25), new Point3D(6103, 1469, 5), new Point3D(6098, 1469, 5),
				new Point3D(6098, 1478, 5), new Point3D(6083, 1485, 5), new Point3D(6107, 1454, 25), new Point3D(6107, 1452, 25),
				new Point3D(6080, 1448, 5), new Point3D(6077, 1445, 20), new Point3D(6067, 1455, 20), new Point3D(6062, 1458, 5),
				new Point3D(6067, 1445, 20), new Point3D(6083, 1491, 5), new Point3D(6092, 1491, 10), new Point3D(6097, 1491, 5),
				new Point3D(6102, 1491, 5), new Point3D(6107, 1491, 5), new Point3D(6107, 1483, 5), new Point3D(6115, 1491, -15),
				new Point3D(6115, 1481, -10), new Point3D(6115, 1471, 5), new Point3D(6115, 1463, 5), new Point3D(6112, 1448, 5)
			};

			foreach (var p in points)
			{
				CreateMobile(types.GetRandom(), p, false, true);
			}

			types = new[] {typeof(SewerRatman), typeof(SewerRatmanArcher), typeof(SewerRatmanMage)};
			points = new[]
			{
				//Control Room
				new Point3D(6046, 1456, 4), new Point3D(6049, 1456, 4), new Point3D(6052, 1456, 4), new Point3D(6051, 1447, 5),
				new Point3D(6051, 1445, 5), new Point3D(6051, 1443, 5), new Point3D(6051, 1441, 5), new Point3D(6047, 1441, 5),
				new Point3D(6047, 1443, 5), new Point3D(6047, 1445, 5), new Point3D(6047, 1447, 5), new Point3D(6060, 1441, 4),
				new Point3D(6060, 1438, 4), new Point3D(6038, 1438, 4), new Point3D(6038, 1441, 4),
				//Cave
				new Point3D(6121, 1451, 5), new Point3D(6123, 1451, 5), new Point3D(6121, 1440, 5), new Point3D(6123, 1440, 5),
				new Point3D(6129, 1441, 4)
			};

			foreach (var p in points)
			{
				CreateMobile(types.GetRandom(), p, false, true);
			}
		}

		private void GenerateBossSpawn()
		{
			// Cave
			Boss1 = CreateMobile<Sycophant>(new Point3D(6140, 1432, 4), true, true);

			// Sewage Intake
			Boss2 = CreateMobile<SewerThing>(new Point3D(6065, 1444, 20), true, true);

			// Control Room
			Boss3 = CreateMobile<LittleBarracoon>(new Point3D(6049, 1433, 4), true, true);
		}

		protected override void OnSpawnActivate(Mobile m)
		{
			base.OnSpawnActivate(m);

			if (m != null && (m == Boss1 || m == Boss2 || m == Boss3))
			{
				CheckBossFields();
			}
		}

		protected override void OnSpawnDeactivate(Mobile m)
		{
			base.OnSpawnDeactivate(m);

			if (m != null && (m == Boss1 || m == Boss2 || m == Boss3))
			{
				CheckBossFields();
			}
		}

		public override bool CheckCanMoveThrough(DungeonZone zone, Mobile m, IEntity e)
		{
			if (m != null && !m.Deleted && m.Alive && e != null && !e.Deleted && e is Static)
			{
				var s = (Static)e;
				
				if (s.Name == "Magical Barrier" && s.ItemID == 130)
				{
					return false;
				}
			}

			return base.CheckCanMoveThrough(zone, m, e);
		}

		private void CheckBossFields()
		{
			if (Deleted)
			{
				return;
			}

            var heat = TimeSpan.FromSeconds(1.0);

			var wipe1 = (Boss1 == null || Boss1.Deleted || !Boss1.Alive || Boss1.Map != Map || !Boss1.InCombat(heat));
			var wipe2 = (Boss2 == null || Boss2.Deleted || !Boss2.Alive || Boss2.Map != Map || !Boss2.InCombat(heat));
			var wipe3 = (Boss3 == null || Boss3.Deleted || !Boss3.Alive || Boss3.Map != Map || !Boss3.InCombat(heat));

			var wipe = (wipe1 && wipe2 && wipe3) || !FindMobiles<PlayerMobile>(p => p.Alive).Any();

			if (wipe)
			{
				foreach (var fields in _BossFields)
				{
					fields.ForEachReverse(f => f.Delete());
				}

				_BossFields.Free(true);
				return;
			}

			foreach (var fields in _BossFields)
			{
				fields.RemoveAll(f => f == null || f.Deleted);
			}

			// Boss 1 (Cave)
			if (wipe1)
			{
				_BossFields[0].ForEachReverse(f => f.Delete());
				_BossFields[0].Free(true);
			}
			else
			{
				var range = Enumerable.Empty<Static>() //
				    .With(TileStatic(130, new Point3D(6123, 1440, 5), 1, 3, true))
				    .With(TileStatic(130, new Point3D(6123, 1444, 5), 1, 2, true))
				    .With(TileStatic(130, new Point3D(6140, 1431, 5), 2, 1, true)) //
				    .Not(_BossFields[0].Contains);

				_BossFields[0].AddRange(range);
			}

			// Boss 2 (Sewage Intake)
			if (wipe2)
			{
				_BossFields[1].ForEachReverse(f => f.Delete());
				_BossFields[1].Free(true);
			}
			else
			{
				var range = Enumerable.Empty<Static>() //
				    .With(TileStatic(130, new Point3D(6078, 1443, 22), 1, 4, true))
				    .With(CreateStatic(130, new Point3D(6078, 1447, 7), true))
				    .With(TileStatic(130, new Point3D(6078, 1448, 9), 1, 2, true))
				    .With(CreateStatic(130, new Point3D(6096, 1445, 5), true))
				    .With(CreateStatic(130, new Point3D(6096, 1445, 25), true))
				    .With(TileStatic(130, new Point3D(6084, 1455, 5), 8, 1, true)) //
				  .Not(_BossFields[1].Contains);

				_BossFields[1].AddRange(range);
			}

			// Boss 3 (Control Room)
			if (wipe3)
			{
				_BossFields[2].ForEachReverse(f => f.Delete());
				_BossFields[2].Free(true);
			}
			else
			{
				var range = Enumerable.Empty<Static>() //
				    .With(TileStatic(130, new Point3D(6044, 1453, 5), 13, 1, true)) //
				    .Not(_BossFields[2].Contains);

				_BossFields[2].AddRange(range);
			}

			foreach (var field in _BossFields.SelectMany(fields => fields))
			{
				field.Light = LightType.Circle300;
				field.Name = "Magical Barrier";
			}
		}

		protected override void OnSlice()
		{
			base.OnSlice();

			CheckBossFields();
		}

		protected override void OnDelete()
		{
			base.OnDelete();

			foreach (var fields in _BossFields)
			{
				fields.ForEachReverse(f => f.Delete());
			}

			_BossFields.Free(true);
		}

		protected override void OnAfterDelete()
		{
			base.OnAfterDelete();

			Boss1 = Boss2 = Boss3 = null;
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.SetVersion(0);

			writer.Write(Boss1);
			writer.Write(Boss2);
			writer.Write(Boss3);

			writer.WriteBlockArray(_BossFields, (w, f) => w.WriteItemList(f));
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			reader.GetVersion();

			Boss1 = reader.ReadMobile();
			Boss2 = reader.ReadMobile();
			Boss3 = reader.ReadMobile();

			_BossFields = reader.ReadBlockArray(r => r.ReadStrongItemList<Static>());
		}
	}
}
