#region Header
//   Vorspire    _,-'/-'/  TrialOfElements.cs
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
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

using Ultima;

using VitaNex.FX;
using VitaNex.InstanceMaps;
using VitaNex.Mobiles;
using VitaNex.Network;
using VitaNex.SuperGumps;

using Map = Server.Map;
#endregion

namespace VitaNex.Dungeons
{
	public sealed class TrialOfElements : Dungeon
	{
		public override DungeonID ID { get { return DungeonID.ToE; } }

		public override Map MapParent { get { return Server.Map.TerMur; } }

		public override Expansion Expansion { get { return Expansion.EJ; } }

		public override TimeSpan Duration { get { return TimeSpan.FromHours(5); } }
		public override TimeSpan Lockout { get { return Core.AOS ? TimeSpan.FromHours(12) : TimeSpan.FromDays(3); } }

		public override Point3D Entrance { get { return new Point3D(160, 2208, 0); } }
		public override Point3D Exit { get { return new Point3D(160, 2208, 0); } }

		public override int GroupMax { get { return 10; } }

		public override int GoldMin { get { return 1000; } }
		public override int GoldMax { get { return 5000; } }

		public override int LootPropsMin { get { return 3; } }
		public override int LootPropsMax { get { return 5; } }

		public override int LootIntensityMin { get { return 70; } }
		public override int LootIntensityMax { get { return 100; } }

		public override string Name { get { return "Trial of Elements"; } }

		public override string Desc
		{
			get
			{
				return
					"The Elemental Aspects have grown bored in their realm of infinity and have challenged all to attempt to defeat them. " +
					"Their confidence may mean their deaths, but the entertainment value is priceless!";
			}
		}

		public VoltaicAltar Altar { get; private set; }
		public VoltaicVendor Vendor { get; private set; }

		public EarthAspect BossEarth { get; private set; }
		public FireAspect BossFire { get; private set; }
		public FrostAspect BossFrost { get; private set; }
		public PoisonAspect BossPoison { get; private set; }
		public EnergyAspect BossEnergy { get; private set; }

		public ElementalAspect[] Bosses { get; private set; }

		public InternalTeleporter[] TeleportersTo { get; private set; }
		public InternalTeleporter[] TeleportersFrom { get; private set; }

		public Rectangle3D[] Circles { get; private set; }
		public List<Static>[] Floors { get; private set; }

		public Pair<string, int>[] Infos { get; private set; }

        public Rectangle3D StagingArea { get; private set; }

        public List<Static> Stage { get; private set; }

		public TrialOfElements()
		{
			Options.Rules.CanFly = false;
			Options.Rules.CanMount = false;
			Options.Rules.AllowPets = false;
			Options.Rules.CanMountEthereal = false;
			Options.Rules.CanMoveThrough = true;
		}

		public TrialOfElements(DungeonSerial serial)
			: base(serial)
		{ }

		protected override void EnsureConstructDefaults()
		{
			base.EnsureConstructDefaults();

			Bosses = new ElementalAspect[5];

			TeleportersTo = new InternalTeleporter[5];
			TeleportersFrom = new InternalTeleporter[5];

			Circles = new Rectangle3D[5];
			Floors = new List<Static>[5];

			Infos = new Pair<string, int>[5];

            Stage = new List<Static>();
        }

		protected override void OnGenerate()
		{
			base.OnGenerate();

			CreateZone("Trial of Elements", new Rectangle2D(70, 1990, 180, 250));

            //CreateZone("Trial of Elements Staging Area", new Rectangle2D(135, 2183, 50, 50));

            CreateZone("Trial of Earth", Circles[0] = new Rectangle3D(135, 2055, 0, 50, 50, 100));
			CreateZone("Trial of Fire", Circles[1] = new Rectangle3D(71, 1991, 0, 50, 50, 100));
			CreateZone("Trial of Frost", Circles[2] = new Rectangle3D(199, 1991, 0, 50, 50, 100));
			CreateZone("Trial of Poison", Circles[3] = new Rectangle3D(71, 2119, 0, 50, 50, 100));
			CreateZone("Trial of Energy", Circles[4] = new Rectangle3D(199, 2119, 0, 50, 50, 100));
            
			Altar = CreateItem<VoltaicAltar>(new Point3D(160, 2188, 0), false, new object[] {this});

            StagingArea = new Rectangle3D(135, 2183, -20, 50, 50, 60);

            var center = StagingArea.Start.Clone2D(25, 25).ToPoint3D();

            Stage.AddRange(center.GetAllPointsInRange(Map, 25, false).Select(p => CreateStatic(11846, p, false)));

            foreach (var s in Stage)
            {
                s.Hue = 2999;
            }

            foreach (var p in center.GetAllPointsInRange(Map, 25, 25, false))
            {
                CreateItem<LOSBlocker>(p.ToPoint3D(20), false);
            }

            for (var i = 0; i < Floors.Length; i++)
			{
				center = Circles[i].Start.Clone2D(25, 25).ToPoint3D();

				Floors[i] = center.GetAllPointsInRange(Map, 25, false).Select(p => CreateStatic(11846, p, false)).ToList();

				foreach (var p in center.GetAllPointsInRange(Map, 25, 25, false))
				{
					CreateItem<LOSBlocker>(p.ToPoint3D(20), false);
				}
			}

			Bosses[0] = BossEarth = CreateMobile<EarthAspect>(new Point3D(160, 2060, 0), true, true, 1.20);
			Bosses[1] = BossFire = CreateMobile<FireAspect>(new Point3D(96, 1996, 0), true, true, 1.40);
			Bosses[2] = BossFrost = CreateMobile<FrostAspect>(new Point3D(224, 1996, 0), true, true, 1.60);
			Bosses[3] = BossPoison = CreateMobile<PoisonAspect>(new Point3D(96, 2124, 0), true, true, 1.80);
			Bosses[4] = BossEnergy = CreateMobile<EnergyAspect>(new Point3D(224, 2124, 0), true, true, 2.00);

			for (var i = 0; i < Bosses.Length; i++)
			{
				if (Bosses[i] != null && !Bosses[i].Deleted)
				{
					Infos[i] = Pair.Create(Bosses[i].RawName, Bosses[i].Hue);
				}
				else
				{
					Infos[i] = Pair.Create("Elemental Aspect", 0);
				}
			}

			/*for (var i = 0; i < Bosses.Length; i++)
			{
				Bosses[i].Home = Circles[i].Start.Clone2D(25, 25).ToPoint3D();
				Bosses[i].RangeHome = 25;
			}*/

			for (var i = 0; i < Floors.Length; i++)
            {
                var hue = Bosses[i].Hue;

				Floors[i].ForEachReverse(s => s.Hue = hue);
			}

			var r = Utility.Random(360);

			for (var i = 0; i < TeleportersTo.Length; i++)
			{
				var a = Angle.FromDegrees(r + (i * 72));
				var p = a.GetPoint3D(160, 2208, 0, 10);

				TeleportersTo[i] = CreateItem<InternalTeleporter>(p, false);
			}

			TeleportersFrom[0] = CreateItem<InternalTeleporter>(new Point3D(160, 2100, 0), false);
			TeleportersFrom[1] = CreateItem<InternalTeleporter>(new Point3D(96, 2036, 0), false);
			TeleportersFrom[2] = CreateItem<InternalTeleporter>(new Point3D(224, 2036, 0), false);
			TeleportersFrom[3] = CreateItem<InternalTeleporter>(new Point3D(96, 2164, 0), false);
			TeleportersFrom[4] = CreateItem<InternalTeleporter>(new Point3D(224, 2164, 0), false);

			for (var i = 0; i < TeleportersTo.Length && i < TeleportersFrom.Length; i++)
			{
				var t1 = TeleportersTo[i];
				var t2 = TeleportersFrom[i];

				t1.PointDest = t2.Location;
				t2.PointDest = t1.Location;

				t1.ItemID = t2.ItemID = 19403;
				t1.Movable = t2.Movable = false;
				t1.Visible = t2.Visible = true;
				t1.Active = t2.Active = true;
				t1.MapDest = t2.MapDest = Map;

				switch (i)
				{
					case 0: // Earth
					{
						t1.Name = "Trial of Earth [Tier 1]";
						t2.Name = "Exit Trial of Earth";
						t1.Hue = t2.Hue = BossEarth.Hue;
					}
						break;
					case 1: // Fire
					{
						t1.Name = "Trial of Fire [Tier 2]";
						t2.Name = "Exit Trial of Fire";
						t1.Hue = t2.Hue = BossFire.Hue;
					}
						break;
					case 2: // Frost
					{
						t1.Name = "Trial of Frost [Tier 3]";
						t2.Name = "Exit Trial of Frost";
						t1.Hue = t2.Hue = BossFrost.Hue;
					}
						break;
					case 3: // Poison
					{
						t1.Name = "Trial of Poison [Tier 4]";
						t2.Name = "Exit Trial of Poison";
						t1.Hue = t2.Hue = BossPoison.Hue;
					}
						break;
					case 4: // Energy
					{
						t1.Name = "Trial of Energy [Tier 5]";
						t2.Name = "Exit Trial of Energy";
						t1.Hue = t2.Hue = BossEnergy.Hue;
					}
						break;
				}
			}
		}

		protected override void OnSlice()
		{
			base.OnSlice();

			for (var i = 0; i < Bosses.Length; i++)
			{
				var o = Bosses[i];

				if (o != null && !o.Deleted && o.Alive)
				{
					var players = 0;

					foreach (var m in Circles[i].FindEntities<PlayerMobile>(Map))
					{
						if (ActiveGroup.Contains(m))
						{
							++players;
						}

						if (m.Z < 0)
						{
							Teleport(m, TeleportersFrom[i].PointDest, Map);
						}
					}

					if (players <= 0)
					{
						o.Combatant = null;
					}

					if (o.InCombat(TimeSpan.FromSeconds(5)))
					{
						DropFloor(i, false);
					}
				}
			}
		}

		protected override void OnSpawnActivate(Mobile m)
		{
			base.OnSpawnActivate(m);

			if (m != null)
			{
				var index = Bosses.IndexOf(m);

				if (index > -1)
				{
					TeleporterState(index, false);
				}

				if (!m.Deleted && m.Alive && m.Z < 0)
				{
					Teleport(m, m.ToPoint3D(), Map);
				}
			}
		}

		protected override void OnSpawnDeactivate(Mobile m)
		{
			base.OnSpawnDeactivate(m);

			if (m != null)
			{
				var index = Bosses.IndexOf(m);

				if (index > -1)
				{
					TeleporterState(index, true);

					if (!m.Deleted && m.Alive && m.Z < 0)
					{
						Teleport(m, Circles[index].Start.Clone2D(25, 5).ToPoint3D(), Map);
					}
				}
			}
		}

		public override void OnDeath(DungeonZone zone, Mobile m)
		{
			base.OnDeath(zone, m);

			var index = Bosses.IndexOf(m);

			if (index > -1)
			{
				if (m.Corpse != null)
				{
					m.Corpse.Z = -20;
				}

				while (Floors[index].Count > 0)
				{
					DropFloor(index, true);
				}

				TeleportersTo[index].Hue = 2999;
				TeleportersTo[index].ItemID = 19343;

				TeleporterMove(index, Circles[index].Start.Clone2D(25, 25).ToPoint3D(-20));
			}
		}

		protected override void OnDelete()
		{
			base.OnDelete();

			if (Altar != null)
			{
				Altar.Delete();
			}

			if (Vendor != null)
			{
				Vendor.Delete();
			}

			foreach (var b in Bosses.Where(b => b != null))
			{
				b.Delete();
			}

			foreach (var t in TeleportersTo.Union(TeleportersFrom).Where(t => t != null))
			{
				t.Delete();
			}

			foreach (var s in Floors.Where(l => l != null).SelectMany(l => l.Where(s => s != null)))
			{
				s.Delete();
			}

            foreach (var s in Stage.Where(s => s != null))
            {
                s.Delete();
            }
		}

		protected override void OnAfterDelete()
		{
			base.OnAfterDelete();

			Altar = null;
			Vendor = null;

			BossEarth = null;
			BossFire = null;
			BossFrost = null;
			BossPoison = null;
			BossEnergy = null;

			Bosses.SetAll(i => null);
			Bosses = null;

			TeleportersTo.SetAll(i => null);
			TeleportersTo = null;

			TeleportersFrom.SetAll(i => null);
			TeleportersFrom = null;

			Circles = null;

			Floors.Free(true);
			Floors.SetAll(i => null);
			Floors = null;

            Stage.Free(true);
            Stage = null;
        }

		private static bool CanDropFloor(Static o)
		{
			const int pathWidth = 3; // odd numbers for proper symmetry

			var path = Math.Max(1, pathWidth / 2);

			return (o.X < 96 - path || o.X > 96 + path) && //
				   (o.X < 224 - path || o.X > 224 + path) && //
				   (o.X < 160 - path || o.X > 160 + path);
		}

		private void DropFloor(int index, bool any)
		{
			var i = Floors[index].Count;

			if (!any && i <= 75)
			{
				return;
			}

			Static o;

			do
			{
				o = Floors[index].GetRandom();
			}
			while (!any && !CanDropFloor(o) && --i >= 0);

			if (!any && !CanDropFloor(o))
			{
				return;
			}

			if (Floors[index].Remove(o))
			{
				var fx = new MovingEffectInfo(
					o.Location,
					o.Location.ToPoint3D(-20),
					Map,
					o.ItemID,
					o.Hue,
					7,
					EffectRender.SemiTransparent);

				o.Delete();

				fx.Send();

				foreach (var m in fx.Source.FindMobilesAt(fx.Map).OfType<PlayerMobile>().Where(ActiveGroup.Contains))
				{
					m.Z = -20;
				}
			}
		}

		private void TeleporterState(int index, bool value)
		{
			TeleportersTo[index].Active = TeleportersFrom[index].Active = value;
		}

		private void TeleporterMove(int index, Point3D value)
		{
			TeleportersTo[index].PointDest = TeleportersFrom[index].Location = value;
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			var version = writer.SetVersion(2);

			writer.Write(Altar);
			writer.Write(Vendor);

			writer.Write(BossEarth);
			writer.Write(BossFire);
			writer.Write(BossFrost);
			writer.Write(BossPoison);
			writer.Write(BossEnergy);

			writer.WriteArray(Bosses, (w, o) => w.Write(o));

			writer.WriteArray(TeleportersTo, (w, o) => w.Write(o));
			writer.WriteArray(TeleportersFrom, (w, o) => w.Write(o));

			writer.WriteArray(Circles, (w, o) => w.Write(o));

			writer.WriteArray(Floors, (w, o) => w.WriteList(o, (w1, s) => w1.Write(s)));

			if (version > 0)
			{
				writer.WriteArray(
					Infos,
					(w, o) =>
					{
						w.Write(o.Left);
						w.Write(o.Right);
					});
			}

            if (version > 1)
            {
                writer.Write(StagingArea);
                writer.WriteItemList(Stage);
            }
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			var version = reader.GetVersion();

			Altar = reader.ReadItem<VoltaicAltar>();
			Vendor = reader.ReadMobile<VoltaicVendor>();

			BossEarth = reader.ReadMobile<EarthAspect>();
			BossFire = reader.ReadMobile<FireAspect>();
			BossFrost = reader.ReadMobile<FrostAspect>();
			BossPoison = reader.ReadMobile<PoisonAspect>();
			BossEnergy = reader.ReadMobile<EnergyAspect>();

			Bosses = reader.ReadArray(r => r.ReadMobile<ElementalAspect>(), Bosses);

			TeleportersTo = reader.ReadArray(r => r.ReadItem<InternalTeleporter>(), TeleportersTo);
			TeleportersFrom = reader.ReadArray(r => r.ReadItem<InternalTeleporter>(), TeleportersFrom);

			Circles = reader.ReadArray(r => r.ReadRect3D(), Circles);

			Floors = reader.ReadArray(r => r.ReadList(r1 => r1.ReadItem<Static>()), Floors);

			if (version > 0)
			{
				Infos = reader.ReadArray(r => Pair.Create(r.ReadString(), r.ReadInt()));
			}
			else
			{
				for (var i = 0; i < Infos.Length; i++)
				{
					switch (i)
					{
						case 0:
							Infos[i] = Pair.Create("Terra", 147);
							break;
						case 1:
							Infos[i] = Pair.Create("Ignis", 1255);
							break;
						case 2:
							Infos[i] = Pair.Create("Glacies", 1261);
							break;
						case 3:
							Infos[i] = Pair.Create("Venenom", 1267);
							break;
						case 4:
							Infos[i] = Pair.Create("Industria", 1273);
							break;
					}
				}
			}

            if (version > 1)
            {
                StagingArea = reader.ReadRect3D();
                Stage = reader.ReadStrongItemList<Static>();
            }
        }

		public sealed class InternalTeleporter : Teleporter
		{
			public override bool ForceShowProperties { get { return true; } }

			public InternalTeleporter()
			{
				Name = "Trial of Elements";

				Creatures = false;
				CombatCheck = true;
			}

			public InternalTeleporter(Serial serial)
				: base(serial)
			{ }

			public override void GetProperties(ObjectPropertyList list)
			{
				AddNameProperties(list);

				list.Add(Active ? "[Active]" : "[Inert]");
			}

			public override void OnSingleClick(Mobile m)
			{
				m.Send(
					new UnicodeMessage(
						Serial,
						ItemID,
						MessageType.Label,
						0x3B2,
						3,
						"ENU",
						"",
						Name + (Amount > 1 ? " : " + Amount : "")));

				LabelTo(m, Active ? "[Active]" : "[Inert]");
			}

			public override void Serialize(GenericWriter writer)
			{
				base.Serialize(writer);

				writer.SetVersion(0);
			}

			public override void Deserialize(GenericReader reader)
			{
				base.Deserialize(reader);

				reader.GetVersion();
			}
		}

		public sealed class VoltaicVendor : AdvancedVendor
		{
			private const int RandomCount = 5;

			private static readonly Dictionary<Type[], int> _Buyables = new Dictionary<Type[], int>
			{
				{new[] {typeof(PendantOfTheMagi)}, 1500000},
				{new[] {typeof(StitchersMittens)}, 750000},
				{new[] {typeof(RobeOfTheEclipse)}, 225000},
				{new[] {typeof(RobeOfTheEquinox)}, 225000},
				{new[] {typeof(OssianGrimoire)}, 325000},
				//{new[] {typeof(DragonLampSouth), typeof(DragonLampEast)}, 125000},
				//{new[] {typeof(DragonEggLampSouth), typeof(DragonEggLampEast)}, 235000},
				//{new[] {typeof(DragonTrophySouthDeed), typeof(DragonTrophyEastDeed)}, 1500000}
			};

            [CommandProperty(AccessLevel.GameMaster)]
			public int Sold { get; set; }

			[Constructable]
			public VoltaicVendor()
				: base("the Guardian Sentinel", typeof(Gold), "Gold", "GP")
			{
				Name = "Voltaic";

				Body = 826;
				BaseSoundID = 362;

				Female = false;
				Hue = 2076;

				CanRestock = false;
			}

			public VoltaicVendor(Serial serial)
				: base(serial)
			{ }

			public override void InitBody()
			{ }

			public override void InitOutfit()
			{ }

			protected override void InitBuyInfo()
			{
				var random = RandomCount - Sold;

				if (random <= 0)
				{
					return;
				}

				var buyables = _Buyables;
				
				if (buyables.Count == 0)
				{
					return;
				}

				do
				{
					Type type;

					foreach (var kv in buyables.TakeRandom(random))
					{
						type = kv.Key.GetRandom();

						AddStock(type, kv.Value, null, 1);
						
						--random;
					}
				}
				while (random > 0);
			}

			protected override void OnItemReceived(Mobile buyer, Item item, IBuyItemInfo buy)
			{
				base.OnItemReceived(buyer, item, buy);

				if (buyer != null && item != null && !item.Deleted)
				{
					++Sold;
				}
			}

			public override void Serialize(GenericWriter writer)
			{
				base.Serialize(writer);

				writer.SetVersion(1);

				// 1
				writer.Write(Sold);
			}

			public override void Deserialize(GenericReader reader)
			{
				base.Deserialize(reader);

				var version = reader.GetVersion();

				if (version > 0)
				{
					Sold = reader.ReadInt();
				}

				if (AdvancedStock.StockCount > 0)
				{
					var trim = AdvancedStock.StockCount - Sold;

					if (trim <= 0)
					{
						ClearBuyInfo();
					}
					else
					{
						AdvancedStock.BuyInfo.TrimEndTo(trim);
					}
				}
			}
		}

		public sealed class VoltaicAltar : Item
		{
			private readonly BitArray _Heads = new BitArray(5, false);

			public BitArray Heads { get { return _Heads; } }

			[CommandProperty(AccessLevel.GameMaster)]
			public int HeadCount { get { return _Heads.OfType<bool>().Count(val => val); } }

			[CommandProperty(AccessLevel.GameMaster)]
			public int HeadMax { get { return _Heads.Length; } }

			[CommandProperty(AccessLevel.GameMaster)]
			public TrialOfElements Dungeon { get; private set; }

			public VoltaicAltar(TrialOfElements toe)
				: base(19724)
			{
				Dungeon = toe;

				Name = "Trophy Case";
				Hue = 2076;

				Movable = false;
			}

			public VoltaicAltar(Serial serial)
				: base(serial)
			{ }

			public override void GetProperties(ObjectPropertyList list)
			{
				base.GetProperties(list);

				list.Add("Heads Collected {0:#,0}/{1:#,0}", HeadCount, HeadMax);
			}

			public override void OnDoubleClick(Mobile m)
			{
				if (!this.CheckDoubleClick(m, true, false, 3) || !(m is PlayerMobile))
				{
					return;
				}

				if (Dungeon != null && Dungeon.Bosses.Any(b => b != null && b.Alive))
				{
					m.SendMessage("The {0} seems to be magically locked.", this.ResolveName(m));
					return;
				}

				new VoltaicGump((PlayerMobile)m, this).Send();
			}

			public void DoSummon()
			{
				if (Deleted || Map == null || Map == Map.Internal)
				{
					Heads.SetAll(false);
					return;
				}

				var mp = new MapPoint(Map, GetWorldLocation());

				new TornadoEffect(mp, mp, Direction.North, 3)
				{
					CanMove = false
				}.Send();

				Visible = false;

				new MovingEffectInfo(mp, mp.Clone3D(0, 0, 80), mp, ItemID, Hue, 5).MovingImpact(
					() => Timer.DelayCall(TimeSpan.FromSeconds(1.5), EndSummon, mp));
			}

			private void EndSummon(MapPoint mp)
			{
				if (Dungeon != null && !Dungeon.Deleted)
				{
					foreach (var m in mp.FindPlayersInRange(mp, GetMaxUpdateRange()))
					{
						ScreenFX.LightFlash.Send(m);
					}

					var v = Dungeon.Vendor ?? (Dungeon.Vendor = Dungeon.CreateMobile<VoltaicVendor>(mp, false, false));

					if (v != null)
					{
						v.Home = mp;
						v.RangeHome = 0;
						v.CanMove = false;
						v.Direction = Direction.South;
						v.Trading = false;

						var speech = new[]
						{
							"The Aspects' challenge was finally met?  ...I see...",
							"Had there been no challenge to meet, you'd be dead where you stand.", "I will honor my fallen kin.",
							"In return for your trophies, I can offer you the treasures I have found on my way here."
						};

						Timer.DelayCall(TimeSpan.FromSeconds(1), v.Say, speech[0]);
						Timer.DelayCall(TimeSpan.FromSeconds(6), v.Say, speech[1]);
						Timer.DelayCall(TimeSpan.FromSeconds(12), v.Say, speech[2]);
						Timer.DelayCall(TimeSpan.FromSeconds(15), v.Say, speech[3]);
						Timer.DelayCall(TimeSpan.FromSeconds(20), () => v.Trading = true);
					}
				}

				Delete();
			}

			public override void OnDelete()
			{
				base.OnDelete();

				Dungeon = null;
			}

			public override void OnAfterDelete()
			{
				base.OnAfterDelete();

				Dungeon = null;
			}

			public override void Serialize(GenericWriter writer)
			{
				base.Serialize(writer);

				writer.SetVersion(0);

				writer.WriteBitArray(Heads);

				writer.WriteDungeon(Dungeon);
			}

			public override void Deserialize(GenericReader reader)
			{
				base.Deserialize(reader);

				reader.GetVersion();

				reader.ReadBitArray(Heads);

				Dungeon = reader.ReadDungeon<TrialOfElements>();
			}
		}

		public class VoltaicGump : SuperGump
		{
			public VoltaicAltar Altar { get; private set; }

			public VoltaicGump(PlayerMobile user, VoltaicAltar altar)
				: base(user)
			{
				Altar = altar;

				CanClose = true;
				CanDispose = true;
				CanMove = true;
				CanResize = false;
			}

			protected override void CompileLayout(SuperGumpLayout layout)
			{
				base.CompileLayout(layout);

				layout.Add("bg", () => AddImage(0, 0, 1422));

				for (var i = 0; i < Altar.HeadMax; i++)
				{
					CompileEntryLayout(layout, 65 + (54 * i), 160, i);
				}
			}

			protected virtual void CompileEntryLayout(SuperGumpLayout layout, int x, int y, int index)
			{
				layout.Add(
					"heads/" + index,
					() =>
					{
						var n = Altar.Dungeon.Infos[index].Left;
						var h = Altar.Dungeon.Infos[index].Right;

						var o = ArtExtUtility.GetImageOffset(0x2DB4);
						var s = ArtExtUtility.GetImageSize(0x2DB4);

						var c = Color.White;

						switch (index)
						{
							case 0:
								c = Color.RosyBrown;
								break;
							case 1:
								c = Color.OrangeRed;
								break;
							case 2:
								c = Color.SkyBlue;
								break;
							case 3:
								c = Color.GreenYellow;
								break;
							case 4:
								c = Color.MediumPurple;
								break;
						}

						if (Altar.Heads[index])
						{
							AddRectangle(x, y, 54, 54, Color.Empty, c, 2);
							AddItem(x + 5 + o.X, y + (30 - (s.Height / 2)), 11700, h);
						}
						else
						{
							AddButton(
								x,
								y,
								2183,
								2183,
								b =>
								{
									if (!Altar.Heads[index])
									{
										var head = User.Backpack.FindItemByType(ElementalAspectHead.Types[index]);

										if (head == null && User.AccessLevel >= AccessLevel.Administrator)
										{
											head = ElementalAspectHead.CreateInstance(index, n, h);

											User.GiveItem(head, GiveFlags.Pack, false);
										}

										if (head != null && !head.Deleted && head.Amount > 0)
										{
											User.SendMessage(85, "You place {0} in to {1}.", head.ResolveName(User), Altar.ResolveName(User));

											Altar.Heads[index] = true;

											head.Consume();

											if (Altar.HeadCount >= Altar.HeadMax)
											{
												Altar.DoSummon();

												Close(true);
												return;
											}
										}
										else
										{
											User.SendMessage(
												34,
												"This slot is engraved with '{0}' and it seems the head of a dragon would fit in it.",
												n);
										}
									}

									Refresh(true);
								});
							AddRectangle(x, y, 54, 54, Color.Empty, c, 2);
						}

						var name = n.WrapUOHtmlCenter().WrapUOHtmlColor(c, false);

						AddHtml(x, index % 2 == 0 ? y + 55 : y - 20, 54, 40, name, false, false);
					});
			}

			protected override void OnMovement(MovementEventArgs e)
			{
				base.OnMovement(e);

				if (!User.InRange(Altar, 3))
				{
					Close(true);
				}
			}

			protected override void OnClosed(bool all)
			{
				base.OnClosed(all);

				if (all)
				{
					Altar = null;
				}
			}

			protected override void OnDisposed()
			{
				base.OnDisposed();

				Altar = null;
			}
		}
	}
}
