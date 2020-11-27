#region Header
//   Vorspire    _,-'/-'/  DungeonGenerator.cs
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
using System.IO;
using System.Linq;
using System.Text;

using Server;
using Server.Commands;
using Server.Gumps;
using Server.Items;
using Server.Network;

using VitaNex.IO;
#endregion

namespace VitaNex.Dungeons
{
	public static class DungeonGenerator
	{
		public const string OutputDirectory = "Scripts/__GEN/Dungeons";

		#region Template
		private const string _Template = @"#region Header
/*
 * Name: ~NAME~
 */
#endregion

#region References
~USING~
#endregion

namespace ~NAMESPACE~
{
	public class ~NAME~Dungeon : Dungeon
	{
		public override DungeonID ID { get { return ~DUNGEONID~; } }

		public override Map MapParent { get { return Server.Map.~MAPPARENT~; } }

		public override TimeSpan Duration { get { return TimeSpan.FromHours(~DURATION~); } }
		public override TimeSpan Lockout { get { return TimeSpan.FromHours(~LOCKOUT~); } }

		public override Point3D Entrance { get { return new Point3D~ENTRANCE~; } }
		public override Point3D Exit { get { return new Point3D~EXIT~; } }

		public override int GroupMax { get { return ~GROUPMAX~; } }

		public override string Name { get { return ""~NAMESPLIT~""; } }

		public override string Desc { get { return ""~DESC~""; } }

		public override double LootFactor { get { return ~LOOTFACTOR~; } }
		public override double SpawnFactor { get { return ~SPAWNFACTOR~; } }

		[Constructable]
		public ~NAME~Dungeon()
		{ }

        public ~NAME~Dungeon(DungeonSerial serial) 
			: base(serial)
        { }

		protected override void OnGenerate()
		{
			base.OnGenerate();
			
			CreateZone(""~REGION~"", new Rectangle2D~BOUNDS~);

			~NAME~DungeonComponents.Instance.Generate(this);
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			reader.ReadInt();
		}
	}

	public sealed class ~NAME~DungeonComponents : DungeonComponents<~NAME~Dungeon>
	{
		public static ~NAME~DungeonComponents Instance { get; private set; }

		static ~NAME~DungeonComponents()
		{
			Instance = new ~NAME~DungeonComponents();
		}

		private ~NAME~DungeonComponents()
		{ }

		protected override void OnGenerate(~NAME~Dungeon dungeon)
		{
			~COMPONENTS~
		}
	}
}";
		#endregion

		public static void Initialize()
		{
			CommandSystem.Register("DungeonGen", AccessLevel.GameMaster, OnDungeonGen);
		}

		[Usage("DungeonGen [<name>]"), Description("Brings up the Dungeon script generator gump.")]
		private static void OnDungeonGen(CommandEventArgs e)
		{
			var states = new object[]
			{
				//
				"", // Name
				"VitaNex.Dungeons", // Namespace
				true, // Get Statics
				false, // Get Items
				false, // Get Tiles
				true, // Static ID Range Limited
				true, // Item ID Range Limited
				true, // Tile ID Range Limited
				true, // Z Rang Limited
				Region.MinZ, // Min Z
				Region.MaxZ, // Max Z
				2, // Min Static ID
				(int)UInt16.MaxValue, // Max Static ID
				2, // Min Item ID
				(int)UInt16.MaxValue, // Max Item ID
				2, // Min Tile ID
				(int)UInt16.MaxValue, // Max Tile ID
				"Dungeon", // Region Name
				TimeSpan.Zero, // Duration
				TimeSpan.Zero, // Lockout
				Point3D.Zero, // Entrance
				Point3D.Zero, // Exit
				10, // Group Max
				1.00, // Loot factor
				1.00, // Spawn Factor
				"" // Description
			};

			if (e.Arguments.Length > 0)
			{
				states[0] = e.Arguments[0];
			}

			e.Mobile.SendGump(new InternalGump(e.Mobile, states));
		}

		private static void PickerCallback(Mobile m, Map map, Point3D start, Point3D end, object state)
		{
			var args = state as object[];

			if (args == null)
			{
				return;
			}

			if (start.X > end.X)
			{
				var x = start.X;

				start.X = end.X;
				end.X = x;
			}

			if (start.Y > end.Y)
			{
				var y = start.Y;

				start.Y = end.Y;
				end.Y = y;
			}

			var bounds = new Rectangle2D(start, end);

			var name = args[0] as string;
			var namesplit = name;

			if (name != null)
			{
				namesplit = name.SpaceWords().ToUpperWords();
				name = namesplit.Replace(" ", String.Empty);
			}

			var ns = args[1] as string;

			var getStatics = (bool)args[2];
			var getItems = (bool)args[3];
			var getTiles = (bool)args[4];
			var includeStaticRange = (bool)args[5];
			var includeItemRange = (bool)args[6];
			var includeTileRange = (bool)args[7];
			var includeZRange = (bool)args[8];

			int minZ, maxZ, minStaticID, maxStaticID, minItemID, maxItemID, minTileID, maxTileID;

			if (!Int32.TryParse(args[9] as string, out minZ) || minZ < Region.MinZ)
			{
				minZ = Region.MinZ;
			}

			if (!Int32.TryParse(args[10] as string, out maxZ) || maxZ > Region.MaxZ)
			{
				maxZ = Region.MaxZ;
			}

			if (!Int32.TryParse(args[11] as string, out minStaticID) || minStaticID < 0)
			{
				minStaticID = 2;
			}

			if (!Int32.TryParse(args[12] as string, out maxStaticID) || maxStaticID > UInt16.MaxValue)
			{
				maxStaticID = UInt16.MaxValue;
			}

			if (!Int32.TryParse(args[13] as string, out minItemID) || minItemID < 0)
			{
				minItemID = 2;
			}

			if (!Int32.TryParse(args[14] as string, out maxItemID) || maxItemID > UInt16.MaxValue)
			{
				maxItemID = UInt16.MaxValue;
			}

			if (!Int32.TryParse(args[15] as string, out minTileID) || minTileID < 0)
			{
				minTileID = 2;
			}

			if (!Int32.TryParse(args[16] as string, out maxTileID) || maxTileID > UInt16.MaxValue)
			{
				maxTileID = UInt16.MaxValue;
			}

			var region = args[17] as string;

			if (String.IsNullOrWhiteSpace(region))
			{
				region = "Dungeon";
			}

			var duration = (TimeSpan)args[18];
			var lockout = (TimeSpan)args[19];

			var entrance = (Point3D)args[20];
			var exit = (Point3D)args[21];

			var groupMax = (int)args[22];

			var lootFactor = (double)args[23];
			var spawnFactor = (double)args[24];

			var desc = args[25] as string ?? String.Empty;

			var cList = GetComponents(
				bounds,
				map,
				getTiles,
				getStatics,
				getItems,
				includeZRange,
				minZ,
				maxZ,
				includeTileRange,
				minTileID,
				maxTileID,
				includeStaticRange,
				minStaticID,
				maxStaticID,
				includeItemRange,
				minItemID,
				maxItemID);

			if (cList == null || cList.Count == 0)
			{
				m.SendMessage(0x40, "No components have been selected.");
			}

			var list = String.Empty;

			if (cList != null && cList.Count > 0)
			{
				if (cList.Count > 100)
				{
					var dir = String.Format(@"Data\Dungeons\{0}", name);
					var cfg = String.Format(@"{0}\components.cfg", dir);

					if (!Directory.Exists(dir))
					{
						Directory.CreateDirectory(dir);
					}

					var lines = new string[cList.Count + 2];

					lines[0] = "#Type\tItemID\tLocation\tAmount\tHue\tLightID\tName";
					lines[1] = String.Empty;

					for (var i = 0; i < cList.Count; i++)
					{
						lines[i + 2] = cList[i].ToString(true);
					}

					File.WriteAllLines(cfg, lines);

					list = String.Format("LoadFile(dungeon, @\"{0}\");", cfg);

					m.SendMessage(0x40, "Components file generated: {0}", cfg);
				}
				else
				{
					list = String.Join("\n\t\t\t", cList);
				}
			}

			var area = String.Format("({0}, {1}, {2}, {3})", bounds.X, bounds.Y, bounds.Width, bounds.Height);
			var dungeonID = String.Join(String.Empty, namesplit.Split(' ').Select(w => w.Substring(0, 1)));
			var mapParent = map.Name.Replace(" ", String.Empty);

			var fileOut = new StringBuilder(_Template);

			var useref = "using System;";

			if (!ns.StartsWith("Server"))
			{
				useref += "\nusing Server;";
				useref += "\nusing Server.Items;";
			}
			else if (!ns.StartsWith("Server.Items"))
			{
				useref += "\nusing Server.Items;";
			}
			else if (!ns.StartsWith("VitaNex"))
			{
				useref += "\nusing VitaNex;";
				useref += "\nusing VitaNex.Dungeons;";
			}
			
			if (!Enum.IsDefined(typeof(DungeonID), dungeonID))
			{
				dungeonID = "/*" + dungeonID + "*//* TODO: Add to DungeonID enum */ ";

				for (var index = 1000000; index < Int32.MaxValue; index++)
				{
					if (!Enum.IsDefined(typeof(DungeonID), index))
					{
						dungeonID += "(DungeonID)" + index;
						break;
					}
				}
			}
			else
			{
				dungeonID = "DungeonID." + dungeonID;
			}

			fileOut.Replace("~USING~", useref);
			fileOut.Replace("~NAMESPACE~", ns);
			fileOut.Replace("~NAME~", name);
			fileOut.Replace("~NAMESPLIT~", namesplit);

			fileOut.Replace("~REGION~", region);
			fileOut.Replace("~BOUNDS~", area);
			fileOut.Replace("~COMPONENTS~", list);

			fileOut.Replace("~DUNGEONID~", dungeonID);
			fileOut.Replace("~MAPPARENT~", mapParent);

			fileOut.Replace("~DURATION~", duration.TotalHours.ToString("F"));
			fileOut.Replace("~LOCKOUT~", lockout.TotalHours.ToString("F"));

			fileOut.Replace("~ENTRANCE~", entrance.ToString());
			fileOut.Replace("~EXIT~", exit.ToString());

			fileOut.Replace("~GROUPMAX~", groupMax.ToString("D"));

			fileOut.Replace("~LOOTFACTOR~", lootFactor.ToString("F"));
			fileOut.Replace("~SPAWNFACTOR~", spawnFactor.ToString("F"));

			fileOut.Replace("~DESC~", desc);

			var path = Path.IsPathRooted(OutputDirectory) ? OutputDirectory : Path.Combine(Core.BaseDirectory, OutputDirectory);

			var file = IOUtility.EnsureFile(path + "/" + name + "Dungeon.cs", true);

			try
			{
				file.AppendText(true, fileOut.ToString());
			}
			catch (Exception ex)
			{
				ex.ToConsole(true, true);

				m.SendMessage(0x40, "An error occurred while writing the Dungeon file.");
				return;
			}

			m.SendMessage(0x40, "Dungeon saved to {0}", file);
			m.SendMessage(0x40, "Total components in Dungeon: {0}", cList.Count);
		}

		public static List<DungeonObject> GetComponents(
			Rectangle2D bounds,
			Map map,
			bool tiles,
			bool statics,
			bool items,
			bool incZRange,
			int minZ,
			int maxZ,
			bool incTileRange,
			int minTileID,
			int maxTileID,
			bool incStaticRange,
			int minStaticID,
			int maxStaticID,
			bool incItemRange,
			int minItemID,
			int maxItemID)
		{
			var list = new List<DungeonObject>(Math.Min(4096, bounds.Width * bounds.Height));

			if (tiles)
			{
				foreach (var p in bounds.EnumeratePoints())
				{
					list.AddRange(
						map.GetStaticTiles(p, !items)
						   .Where(t => !incZRange || (t.Z >= minZ && t.Z <= maxZ))
						   .Where(t => !incTileRange || (t.ID >= minTileID && t.ID <= maxTileID))
						   .Select(t => new DungeonObject(t.GetType(), t.ID, t.ToPoint3D(), 1, t.Hue, -1, null)));
				}
			}

			IEnumerable<Item> check = null;

			if (items)
			{
				check = bounds.FindEntities<Item>(map);
			}

			if (statics && check == null)
			{
				check = bounds.FindEntities<Static>(map);
			}
			else if (!statics && check != null)
			{
				check = check.Not(i => i is Static);
			}

			if (check != null)
			{
				foreach (var i in check.Where(t => !incZRange || (t.Z >= minZ && t.Z <= maxZ)))
				{
					if (i is Static)
					{
						if (incStaticRange && (i.ItemID < minStaticID || i.ItemID > maxStaticID))
						{
							continue;
						}
					}
					else if (i is IAddon)
					{
						continue;
					}
					else if (incItemRange && (i.ItemID < minItemID || i.ItemID > maxItemID))
					{
						continue;
					}

					var t = i.GetType();

					if (i is AddonComponent || i is AddonContainerComponent)
					{
						t = typeof(Static);
					}
					else if (i is BaseMulti)
					{
						var mcl = ((BaseMulti)i).Components;

						if (mcl != null && mcl.List != null && mcl.List.Length > 0)
						{
							t = typeof(Static);

							Point3D loc;

							foreach (var e in mcl.List)
							{
								loc = i.Location.Clone3D(e.m_OffsetX, e.m_OffsetY, e.m_OffsetZ);

								list.Add(new DungeonObject(t, e.m_ItemID, loc, 1, 0, -1, i.Name));
							}
						}

						continue;
					}

					list.Add(new DungeonObject(t, i.ItemID, i.Location, i.Amount, i.Hue, (int)i.Light, i.Name));
				}
			}

			list.Free(false);

			list.Sort(
				(l, r) =>
				{
					var o = l.Type.CompareTo(r.Type);

					if (o == 0)
					{
						o = Insensitive.Compare(l.Name, r.Name);
					}

					return o;
				});

			return list;
		}

		public sealed class DungeonObject : IPoint3D
		{
			public Type Type { get; set; }
			public int ItemID { get; set; }

			public Point3D Location { get; set; }

			public int Amount { get; set; }
			public int Hue { get; set; }
			public int Light { get; set; }
			public string Name { get; set; }

			public int X { get { return Location.X; } }
			public int Y { get { return Location.Y; } }
			public int Z { get { return Location.Z; } }

			public bool IsComplex { get { return Amount > 1 || Hue > 0 || Light >= 0 || !String.IsNullOrWhiteSpace(Name); } }

			public DungeonObject(Type type, int itemID, Point3D loc, int amount, int hue, int light, string name)
			{
				Type = type;
				ItemID = itemID;
				Location = loc;
				Amount = amount;
				Hue = hue;
				Light = light;
				Name = name;
			}

			public string ToString(bool file)
			{
				if (file)
				{
					return String.Format(
						"{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}",
						Type.FullName,
						ItemID,
						Location,
						Amount,
						Hue,
						Light,
						Name);
				}

				return String.Format(
					"CreateObject(dungeon, typeof({0}), {1}, new Point3D{2}, {3}, {4}, {5}, \"{6}\");",
					Type.FullName,
					ItemID,
					Location,
					Amount,
					Hue,
					Light,
					Name);
			}

			public override string ToString()
			{
				return ToString(false);
			}
		}

		private sealed class InternalGump : Gump
		{
			private const int LabelHue = 0x480;
			private const int TitleHue = 0x35;

			private readonly object[] _State;

			public InternalGump(Mobile m, object[] state)
				: base(100, 50)
			{
				m.CloseGump(typeof(InternalGump));

				_State = state;

				Closable = true;
				Disposable = true;
				Dragable = true;
				Resizable = false;

				AddPage(0);

				AddBackground(0, 0, 640, 460, 9260);
				//AddAlphaRegion(10, 10, 620, 440);

				AddHtml(0, 15, 440, 20, Center(Color("Dungeon Generator", 0x000080)), false, false);

				var y = 40;

				AddLabel(20, y, LabelHue, @"Name");
				AddImageTiled(95, y, 165, 18, 9274);
				AddTextEntry(95, y, 165, 20, LabelHue, 0, _State[0] as string); // Name

				y += 20;

				AddLabel(20, y, LabelHue, @"Namespace");
				AddImageTiled(95, y, 165, 18, 9274);
				AddTextEntry(95, y, 165, 20, LabelHue, 1, _State[1] as string); // Namespace
				AddLabel(340, y, TitleHue, @"ID Range");

				y += 20;

				AddLabel(20, y, TitleHue, @"Export");
				AddLabel(170, y, TitleHue, @"ID Range");
				AddLabel(320, y, TitleHue, @"Include/Exclude");

				y += 25;

				// Export Statics, Items, and Tiles
				var exportString = new[] {"Statics", "Items", "Tiles"};

				for (var i = 0; i < 3; i++)
				{
					DisplayExportLine(
						y,
						i,
						(bool)_State[i + 2],
						(bool)_State[i + 5],
						exportString[i],
						_State[11 + (i * 2)].ToString(),
						_State[12 + (i * 2)].ToString());
					y += (i < 2 ? 25 : 15);
				}

				AddImageTiled(15, y + 15, 620, 1, 9304);

				y += 25;

				// Z Range
				AddCheck(350, y, 9026, 9027, (bool)_State[8], 6);
				AddLabel(20, y - 2, LabelHue, @"Z Range");
				AddImageTiled(115, y + 15, 50, 1, 9274);
				AddTextEntry(115, y - 5, 50, 20, LabelHue, 2, _State[9].ToString());
				AddLabel(185, y - 2, LabelHue, @"to");
				AddImageTiled(225, y + 15, 50, 1, 9274);
				AddTextEntry(225, y - 5, 50, 20, LabelHue, 3, _State[10].ToString());

				AddImageTiled(15, y + 20, 620, 1, 9304);

				y += 25;

				AddLabel(20, y - 2, LabelHue, @"Region");
				AddImageTiled(95, y, 165, 18, 9274);
				AddTextEntry(95, y, 165, 20, LabelHue, 1004, _State[17] as string); // Region Name

				y += 25;

				AddLabel(20, y - 2, LabelHue, @"Duration");
				AddImageTiled(115, y + 15, 30, 1, 9274);
				AddTextEntry(115, y - 5, 30, 20, LabelHue, 1005, ((TimeSpan)_State[18]).Hours.ToString("D"));
				AddLabel(150, y - 2, LabelHue, "H");
				AddImageTiled(165, y + 15, 30, 1, 9274);
				AddTextEntry(165, y - 5, 30, 20, LabelHue, 1006, ((TimeSpan)_State[18]).Minutes.ToString("D"));
				AddLabel(200, y - 2, LabelHue, "M");

				AddLabel(335, y - 2, LabelHue, @"Lockout");
				AddImageTiled(430, y + 15, 30, 1, 9274);
				AddTextEntry(430, y - 5, 30, 20, LabelHue, 1007, ((TimeSpan)_State[19]).Hours.ToString("D"));
				AddLabel(465, y - 2, LabelHue, "H");
				AddImageTiled(480, y + 15, 30, 1, 9274);
				AddTextEntry(480, y - 5, 30, 20, LabelHue, 1008, ((TimeSpan)_State[19]).Hours.ToString("D"));
				AddLabel(515, y - 2, LabelHue, "M");

				y += 25;

				AddLabel(20, y - 2, LabelHue, @"Entrance");
				AddImageTiled(115, y + 15, 50, 1, 9274);
				AddTextEntry(115, y - 5, 50, 20, LabelHue, 1009, ((Point3D)_State[20]).X.ToString("D"));
				AddLabel(170, y - 2, LabelHue, "X");
				AddImageTiled(185, y + 15, 50, 1, 9274);
				AddTextEntry(185, y - 5, 50, 20, LabelHue, 1010, ((Point3D)_State[20]).Y.ToString("D"));
				AddLabel(240, y - 2, LabelHue, "Y");
				AddImageTiled(255, y + 15, 30, 1, 9274);
				AddTextEntry(255, y - 5, 30, 20, LabelHue, 1011, ((Point3D)_State[20]).Z.ToString("D"));
				AddLabel(290, y - 2, LabelHue, "Z");

				AddLabel(335, y - 2, LabelHue, @"Exit");
				AddImageTiled(430, y + 15, 50, 1, 9274);
				AddTextEntry(430, y - 5, 50, 20, LabelHue, 1012, ((Point3D)_State[21]).X.ToString("D"));
				AddLabel(485, y - 2, LabelHue, "X");
				AddImageTiled(500, y + 15, 50, 1, 9274);
				AddTextEntry(500, y - 5, 50, 20, LabelHue, 1013, ((Point3D)_State[21]).Y.ToString("D"));
				AddLabel(555, y - 2, LabelHue, "Y");
				AddImageTiled(570, y + 15, 30, 1, 9274);
				AddTextEntry(570, y - 5, 30, 20, LabelHue, 1014, ((Point3D)_State[21]).Z.ToString("D"));
				AddLabel(605, y - 2, LabelHue, "Z");

				y += 25;

				AddLabel(20, y - 2, LabelHue, @"Group Max");
				AddImageTiled(115, y + 15, 50, 1, 9274);
				AddTextEntry(115, y - 5, 50, 20, LabelHue, 1015, _State[22].ToString());

				y += 25;

				AddLabel(20, y - 2, LabelHue, @"Loot Factor");
				AddImageTiled(115, y + 15, 50, 1, 9274);
				AddTextEntry(115, y - 5, 50, 20, LabelHue, 1016, _State[23].ToString());

				AddLabel(335, y - 2, LabelHue, @"Spawn Factor");
				AddImageTiled(430, y + 15, 50, 1, 9274);
				AddTextEntry(430, y - 5, 50, 20, LabelHue, 1017, _State[24].ToString());

				y += 25;

				AddLabel(20, y - 2, LabelHue, @"Description");
				AddImageTiled(115, y + 55, 300, 1, 9274);
				AddTextEntry(115, y - 5, 300, 60, LabelHue, 1018, _State[25] as string);

				AddImageTiled(15, y + 60, 620, 1, 9304);

				y += 65;

				// Buttons
				AddButton(20, y, 4020, 4021, 0, GumpButtonType.Reply, 0);
				AddLabel(55, y, LabelHue, @"Cancel");
				AddButton(155, y, 4005, 4006, 1, GumpButtonType.Reply, 0);
				AddLabel(195, y, LabelHue, @"Generate");
			}

			private void DisplayExportLine(int y, int index, bool state, bool include, string heading, string min, string max)
			{
				AddCheck(20, y, 9026, 9027, state, index);
				AddLabel(40, y, LabelHue, heading);
				AddImageTiled(115, y + 15, 50, 1, 9274);
				AddTextEntry(115, y - 5, 50, 20, LabelHue, 4 + (index * 2), min); // Tile ID Min
				AddLabel(185, y, LabelHue, @"to");
				AddImageTiled(225, y + 15, 50, 1, 9274);
				AddTextEntry(225, y - 5, 50, 20, LabelHue, 5 + (index * 2), max); // Tile ID Max
				AddCheck(350, y, 9026, 9027, include, index + 3); // Include or Exclude compare?
			}

			private static string Center(string text)
			{
				return String.Format("<CENTER>{0}</CENTER>", text);
			}

			private static string Color(string text, int color)
			{
				return String.Format("<BASEFONT COLOR=#{0:X6}>{1}<BASEFONT COLOR=#FFFFFF>", color, text);
			}

			public override void OnResponse(NetState sender, RelayInfo info)
			{
				if (info.ButtonID == 0)
				{
					return;
				}

				foreach (var text in info.TextEntries)
				{
					int index;

					if (text.EntryID >= 1000)
					{
						continue;
					}

					if (text.EntryID > 1)
					{
						index = text.EntryID + 7;
					}
					else
					{
						index = text.EntryID;
					}

					_State[index] = text.Text ?? String.Empty;
				}

				// Reset checks
				for (var x = 2; x <= 8; x++)
				{
					_State[x] = false;
				}

				foreach (var check in info.Switches)
				{
					_State[check + 2] = true; // Offset by 2 in the state object
				}

				try
				{
					_State[17] = info.GetTextEntry(1004).Text ?? String.Empty;
				}
				catch
				{ }

				try
				{
					var duraH = info.GetTextEntry(1005).Text ?? String.Empty;
					var duraM = info.GetTextEntry(1006).Text ?? String.Empty;

					if (String.IsNullOrWhiteSpace(duraH))
					{
						duraH = "0";
					}

					if (String.IsNullOrWhiteSpace(duraM))
					{
						duraM = "0";
					}

					TimeSpan time;

					if (TimeSpan.TryParse("0:" + duraH + ":" + duraM + ":0", out time))
					{
						_State[18] = time;
					}
				}
				catch
				{ }

				try
				{
					var lockH = info.GetTextEntry(1007).Text ?? String.Empty;
					var lockM = info.GetTextEntry(1008).Text ?? String.Empty;

					if (String.IsNullOrWhiteSpace(lockH))
					{
						lockH = "0";
					}

					if (String.IsNullOrWhiteSpace(lockM))
					{
						lockM = "0";
					}

					TimeSpan time;

					if (TimeSpan.TryParse("0:" + lockH + ":" + lockM + ":0", out time))
					{
						_State[19] = time;
					}
				}
				catch
				{ }

				try
				{
					var enX = info.GetTextEntry(1009).Text ?? String.Empty;
					var enY = info.GetTextEntry(1010).Text ?? String.Empty;
					var enZ = info.GetTextEntry(1011).Text ?? String.Empty;

					if (String.IsNullOrWhiteSpace(enX))
					{
						enX = "0";
					}

					if (String.IsNullOrWhiteSpace(enY))
					{
						enY = "0";
					}

					if (String.IsNullOrWhiteSpace(enZ))
					{
						enZ = "0";
					}

					int px, py, pz;

					if (Int32.TryParse(enX, out px) | Int32.TryParse(enY, out py) | Int32.TryParse(enZ, out pz))
					{
						_State[20] = new Point3D(px, py, pz);
					}
				}
				catch
				{ }

				try
				{
					var exX = info.GetTextEntry(1012).Text ?? String.Empty;
					var exY = info.GetTextEntry(1013).Text ?? String.Empty;
					var exZ = info.GetTextEntry(1014).Text ?? String.Empty;

					if (String.IsNullOrWhiteSpace(exX))
					{
						exX = "0";
					}

					if (String.IsNullOrWhiteSpace(exY))
					{
						exY = "0";
					}

					if (String.IsNullOrWhiteSpace(exZ))
					{
						exZ = "0";
					}

					int px, py, pz;

					if (Int32.TryParse(exX, out px) | Int32.TryParse(exY, out py) | Int32.TryParse(exZ, out pz))
					{
						_State[21] = new Point3D(px, py, pz);
					}
				}
				catch
				{ }

				try
				{
					var grpMax = info.GetTextEntry(1015).Text ?? String.Empty;

					if (String.IsNullOrWhiteSpace(grpMax))
					{
						grpMax = "0";
					}

					int groupMax;

					if (Int32.TryParse(grpMax, out groupMax))
					{
						_State[22] = groupMax;
					}
				}
				catch
				{ }

				try
				{
					var lootF = info.GetTextEntry(1016).Text ?? String.Empty;

					if (String.IsNullOrWhiteSpace(lootF))
					{
						lootF = "0";
					}

					double lootFactor;

					if (Double.TryParse(lootF, out lootFactor))
					{
						_State[23] = lootFactor;
					}
				}
				catch
				{ }

				try
				{
					var spawnF = info.GetTextEntry(1017).Text ?? String.Empty;

					if (String.IsNullOrWhiteSpace(spawnF))
					{
						spawnF = "0";
					}

					double spawnFactor;

					if (Double.TryParse(spawnF, out spawnFactor))
					{
						_State[24] = spawnFactor;
					}
				}
				catch
				{ }

				try
				{
					_State[25] = info.GetTextEntry(1018).Text ?? String.Empty;
				}
				catch
				{ }

				if (Verify(sender.Mobile, _State))
				{
					BoundingBoxPicker.Begin(sender.Mobile, PickerCallback, _State);
				}
				else
				{
					sender.Mobile.SendMessage(0x40, "Please review the generation parameters, some are invalid.");
					sender.Mobile.SendGump(new InternalGump(sender.Mobile, _State));
				}
			}

			private static bool Verify(Mobile from, object[] state)
			{
				if (String.IsNullOrWhiteSpace(state[0] as string))
				{
					from.SendMessage(0x40, "Name field is invalid or missing.");
					return false;
				}

				if (String.IsNullOrWhiteSpace(state[1] as string))
				{
					from.SendMessage(0x40, "Namespace field is invalid or missing.");
					return false;
				}

				if (!((bool)state[2] || (bool)state[3] || (bool)state[4]))
				{
					from.SendMessage(0x40, "You must have least one Export button selected. (Static/Items/Tiles)");
					return false;
				}

				var errors = new[]
				{
					"Z Range Min", "Z Range Max", "Static Min ID", "Static Max ID", "Item Min ID", "Item Max ID", "Tile Min ID",
					"Tile Max ID"
				};

				for (var x = 0; x < 8; x++)
				{
					if (CheckNumber(state[x + 9] as string))
					{
						continue;
					}

					from.SendMessage(0x40, "There's a problem with the {0} field.", errors[x]);
					return false;
				}

				return true;
			}

			private static bool CheckNumber(string number)
			{
				int value;

				return Int32.TryParse(number, out value);
			}
		}
	}
}