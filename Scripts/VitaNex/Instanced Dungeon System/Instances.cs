#region Header
//   Vorspire    _,-'/-'/  Instances.cs
//   .      __,-; ,'( '/
//    \.    `-.__`-._`:_,-._       _ , . ``
//     `:-._,------' ` _,`--` -: `_ , ` ,' :
//        `---..__,,--'  (C) 2015  ` -'. -'
//        #  Vita-Nex [http://core.vita-nex.com]  #
//  {o)xxx|===============-   #   -===============|xxx(o}
//        #        The MIT License (MIT)          #
#endregion

#region References
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

using Server;
using Server.Engines.PartySystem;
using Server.Items;
using Server.Mobiles;

using VitaNex.Dungeons;
using VitaNex.IO;
using VitaNex.Network;
using VitaNex.SuperGumps.UI;
using VitaNex.TimeBoosts;
#endregion

namespace VitaNex.InstanceMaps
{
	public static partial class Instances
	{
		public const AccessLevel Access = AccessLevel.Administrator;

		public static InstanceMapsOptions CSOptions { get; private set; }

		public static FileInfo IndexFile { get; private set; }
		public static FileInfo RestoreFile { get; private set; }

		public static BinaryDirectoryDataStore<InstanceMapSerial, InstanceMap> Maps { get; private set; }
		public static BinaryDirectoryDataStore<DungeonSerial, Dungeon> Dungeons { get; private set; }

		public static BinaryDataStore<PlayerMobile, LockoutState> Lockouts { get; private set; }

		public static Type[] DungeonTypes { get; private set; }
		public static DungeonInfo[] DungeonInfo { get; private set; }

		public static PollTimer DefragmentTimer { get; private set; }
		
		private static readonly Dictionary<PlayerMobile, MapPoint> _BounceRestore;
		private static readonly Dictionary<Item, IEntity> _ItemRestore;

		private static CreateCorpseHandler _CreateCorpseSuccessor;

		private static bool _Defragmenting;

		public static void Defragment()
		{
			if (_Defragmenting)
			{
				return;
			}

			_Defragmenting = true;

			var now = DateTime.UtcNow;

			Dungeons.Values.Where(d => d != null && !d.Deleted).ForEach(d =>
			{
				d.Defragment();

				if (d.Zones.Count == 0 || d.Deadline < now || (d.Group.Count == 0 && (now - d.Created).TotalHours > 1))
				{
					d.Delete();
				}
			});

			Dungeons.RemoveValueRange(d => d == null || d.Deleted);

			Maps.Values.Where(map => map != null && !map.Deleted).ForEach(map =>
			{
				map.Defragment();

				var del = map.InstanceRegions.Count == 0;

				if (!del)
				{
					del = map.InstanceRegions.All(r =>
					{
						if (r == null || r.Deleted)
						{
							return true;
						}

						if (r is DungeonZone)
						{
							var z = (DungeonZone)r;

							return z.Dungeon == null || z.Dungeon.Deleted;
						}

						return false;
					});
				}

				if (del)
				{
					map.Delete();
				}
			});

			Maps.RemoveValueRange(m => m == null || m.Deleted);

			_Defragmenting = false;
		}

		public static void Restore()
		{
			RestoreFile.SetHidden(true);
			RestoreFile.Deserialize(LoadRestore);

			var bounce = new MapPoint(CSOptions.BounceMap, CSOptions.BounceLocation);

			if (bounce.InternalOrZero)
			{
				// Britain Peninsula (West of the Bank)
				bounce.Map = Map.Felucca;
				bounce.Location = new Point3D(1383, 1713, 20);
			}

			foreach (var kv in _BounceRestore)
			{
				var m = kv.Key;
				var p = kv.Value;

				if (m == null || m.Deleted)
				{
					continue;
				}

				if (p == null || p.InternalOrZero)
				{
					p = bounce;
				}

				m.FixMap(p);
			}

			_BounceRestore.Clear();

			foreach (var kv in _ItemRestore)
			{
				var i = kv.Key;
				var p = kv.Value;

				if (i == null || i.Deleted || i.Map != null || p == null || p.Deleted)
				{
					continue;
				}

#if NEWPARENT
				var cp = i.Parent;
#else
				var cp = i.ParentEntity;
#endif

				if (cp == p)
				{
					continue;
				}

				if (p is Item)
				{
					((Item)p).AddItem(i);
				}
				else if (p is Mobile)
				{
					((Mobile)p).AddItem(i);
				}
				else
				{
					i.Parent = p;
				}
			}

			_ItemRestore.Clear();
		}

		public static InstanceMap CreateMap(Map parent, string name, Season season, MapRules rules)
		{
			// If the map is TerMur, use Spring or Summer to prevent static clouds from being hidden.
			if (season != Season.Spring && season != Season.Summer && parent.MapID == 5)
			{
				season = Utility.RandomList(Season.Spring, Season.Summer);
			}

			var map = new InstanceMap(parent, name, season, rules);

			if (!map.Deleted)
			{
				Maps.AddOrReplace(map.Serial, map);
			}

			return map;
		}

		public static InstanceMap ResolveMap(Dungeon d, bool create)
		{
			if (d == null || d.Deleted)
			{
				return null;
			}

			if (d.Map != null && !d.Map.Deleted)
			{
				return d.Map;
			}

			InstanceMap map = null;

			foreach (var m in Maps.Values.Where(m => m != null && !m.Deleted && m.Parent == d.MapParent))
			{
				foreach (var zone in
					m.InstanceRegions.OfType<DungeonZone>()
					 .Where(zone => !zone.Deleted && zone.Dungeon != null && !zone.Dungeon.Deleted))
				{
					if (zone.Dungeon == d)
					{
						map = m;
						break;
					}

					if (zone.Dungeon.ID == d.ID)
					{
						break;
					}
				}

				if (map != null && !map.Deleted)
				{
					break;
				}
			}

			if ((map == null || map.Deleted) && create)
			{
				map = CreateMap(d.MapParent, "Universe", (Season)Utility.Random(5), MapRules.FeluccaRules | MapRules.FreeMovement);

				if (map != null && !map.Deleted)
				{
					map.Name = "Universe-" + map.MapID + "-" + map.MapIndex;
				}
			}

			return map != null && !map.Deleted ? map : null;
		}

		public static InstanceMap FindMap(InstanceMapSerial serial)
		{
			return Maps.GetValue(serial);
		}

		public static InstanceRegion FindRegion(InstanceRegionSerial serial)
		{
			return InstanceRegion.AllRegions.Find(ir => ir != null && ir.Serial.Equals(serial));
		}

		public static Dungeon FindDungeon(DungeonSerial serial)
		{
			return Dungeons.GetValue(serial);
		}

		public static InstanceRegion GetRegion(Mobile m)
		{
			return m.GetRegion<InstanceRegion>();
		}

		public static Dungeon GetDungeon(Mobile m)
		{
			var region = m.GetRegion<DungeonZone>();

			return region != null ? region.Dungeon : null;
		}

		public static void SetLockout(PlayerMobile pm, DungeonID id, TimeSpan t)
		{
			if (pm == null || pm.Deleted || id == DungeonID.None)
			{
				return;
			}

			var state = Lockouts.GetValue(pm);

			if (state == null)
			{
				if (t <= TimeSpan.Zero)
				{
					return;
				}

				Lockouts[pm] = state = new LockoutState(pm);
			}

			state.SetLockout(id, t);

			if (state.IsEmpty)
			{
				Lockouts.Remove(pm);
			}
		}

		public static TimeSpan GetLockout(PlayerMobile pm, DungeonID id)
		{
			if (pm == null || pm.Deleted || id == DungeonID.None)
			{
				return TimeSpan.Zero;
			}

			var t = TimeSpan.Zero;

			var state = Lockouts.GetValue(pm);

			if (state != null)
			{
				t = state.GetLockout(id);
				
				if (state.IsEmpty)
				{
					Lockouts.Remove(pm);
				}
			}

			return t;
		}

		public static bool IsLockedOut(PlayerMobile pm, DungeonID id)
		{
			if (pm == null || pm.Deleted || id == DungeonID.None)
			{
				return false;
			}

			var s = false;

			var state = Lockouts.GetValue(pm);

			if (state != null)
			{
				s = state.IsLockedOut(id);

				if (state.IsEmpty)
				{
					Lockouts.Remove(pm);
				}
			}

			return s;
		}

		public static bool IsLockedOut(PlayerMobile pm, DungeonID id, out TimeSpan t)
		{
			t = TimeSpan.Zero;

			if (pm == null || pm.Deleted || id == DungeonID.None)
			{
				return false;
			}

			var s = false;

			var state = Lockouts.GetValue(pm);

			if (state != null)
			{
				s = state.IsLockedOut(id, out t);

				if (state.IsEmpty)
				{
					Lockouts.Remove(pm);
				}
			}

			return s;
		}

		public static bool EnterDungeon(PlayerMobile pm, DungeonInfo info, bool confirm)
		{
			if (pm == null || pm.Deleted || info == null || info.ID == DungeonID.None)
			{
				return false;
			}

			if (confirm)
			{
				var html = new StringBuilder();

				html.AppendLine(info.Desc.WrapUOHtmlColor(Color.PaleGoldenrod));
				html.AppendLine();
				html.AppendLine("You are about to enter {0}.", info.Name);
				html.AppendLine(
					"You have {0} to complete your raid...",
					info.Duration.ToSimpleString("!d# days #h# hours #m# minutes and #s# seconds#"));
				html.AppendLine();
				html.AppendLine("Click OK to enter {0}!", info.Name);

				new ConfirmDialogGump(pm)
				{
					Width = 500,
					Height = 400,
					Title = info.Name,
					Html = html.ToString(),
					HtmlColor = Color.White,
					AcceptHandler = b => EnterDungeon(pm, info, false)
				}.Send();

				return false;
			}

			if (info.Expansion != Expansion.None && pm.NetState != null && !pm.NetState.SupportsExpansion(info.Expansion, false))
			{
				pm.SendMessage(0x22, "The {0} expansion is required to enter {1}.", info.Expansion, info.Name);
				return false;
			}

			TimeSpan lockout;

			if (IsLockedOut(pm, info.ID, out lockout))
			{
				pm.SendMessage(
					0x22,
					"You must wait {0} before you can enter {1} again.",
					lockout.ToSimpleString("!d# days #h# hours #m# minutes and #s# seconds#"),
					info.Name);

				var ui = new TimeBoostsUI(pm, null, TimeBoosts.TimeBoosts.EnsureProfile(pm))
				{
					Title = info.Name,
					SubTitle = "Reduce Wait?",
					SummaryText = "Next Raid",
					GetTime = () => GetLockout(pm, info.ID),
					SetTime = t => SetLockout(pm, info.ID, t)
				};

				ui.BoostUsed = b =>
				{
					if (IsLockedOut(pm, info.ID, out lockout))
					{
						pm.SendMessage(
							0x22,
							"You must wait {0} before you can enter {1} again.",
							lockout.ToSimpleString("!d# days #h# hours #m# minutes and #s# seconds#"),
							info.Name);
					}
					else
					{
						ui.Close(true);

						EnterDungeon(pm, info, false);
					}
				};

				ui.Send();

				return false;
			}

			var party = Party.Get(pm);
			var isParty = false;

			Dungeon dungeon = null;

			if (party != null && party.Leader is PlayerMobile && party.Leader != pm)
			{
				isParty = true;

				var pl = (PlayerMobile)party.Leader;

				dungeon =
					Dungeons.Values.FirstOrDefault(
						d => d != null && !d.Deleted && d.ID == info.ID && (d.Group.Count == 0 || d.Group.Contains(pl)));
			}

			if (dungeon == null && isParty)
			{
				pm.SendMessage(0x22, "Your party leader must be the first to enter {0}.", info.Name);
				return false;
			}

			if (dungeon == null)
			{
				dungeon =
					Dungeons.Values.FirstOrDefault(
						d => d != null && !d.Deleted && d.ID == info.ID && (d.Group.Count == 0 || d.Group.Contains(pm)));
			}

			if (dungeon != null && dungeon.Group.Count(gp => gp != null && gp != pm && gp.Map == dungeon.Map) >= dungeon.GroupMax)
			{
				pm.SendMessage(0x22, "{0} is currently at maximum capacity.", info.Name);
				return false;
			}

			if (dungeon == null || dungeon.Deleted)
			{
				dungeon = info.Type.CreateInstanceSafe<Dungeon>();

				if (dungeon != null && !dungeon.Deleted)
				{
					Dungeons.Add(dungeon.Serial, dungeon);
				}
			}

			if (dungeon != null && !dungeon.Deleted)
			{
				dungeon.Init();

				var lifespan = dungeon.Deadline - DateTime.UtcNow;

				if (dungeon.Map == null || dungeon.Map.Deleted || lifespan <= TimeSpan.Zero)
				{
					pm.SendMessage(0x22, "A rip in the fabric of reality prevents you from entering {0}.", info.Name);

					if (dungeon.Group.Count == 0 || (dungeon.Group.Count == 1 && dungeon.Group.Contains(pm)))
					{
						dungeon.Delete();
					}

					return false;
				}

				if (dungeon.Entrance == Point3D.Zero || !dungeon.CanEnterDungeon(pm))
				{
					pm.SendMessage(0x22, "Mystical forces prevent you from entering {0}.", info.Name);
					
					if (dungeon.Group.Count == 0 || (dungeon.Group.Count == 1 && dungeon.Group.Contains(pm)))
					{
						dungeon.Delete();
					}

					return false;
				}

				string time = lifespan.ToSimpleString("!d# days #h# hours #m# minutes and #s# seconds#");

				/*if (lifespan.TotalDays > 1.0)
				{
					time = String.Format("{0:#,0} days", (int)lifespan.TotalDays);
				}
				else if (lifespan.TotalHours > 1.0)
				{
					time = String.Format("{0:#,0} hours", (int)lifespan.TotalHours);
				}
				else if (lifespan.TotalMinutes > 1.0)
				{
					time = String.Format("{0:#,0} minutes", (int)lifespan.TotalMinutes);
				}
				else
				{
					time = String.Format("{0:#,0} seconds", (int)lifespan.TotalSeconds);
				}*/

				pm.SendMessage(0x55, "You have {0} before reality breaks down and {1} ceases to exist.", time, info.Name);

				dungeon.Teleport(pm, dungeon.Entrance, dungeon.Map);
				dungeon.OnEnterDungeon(pm);
				return true;
			}

			pm.SendMessage(0x22, "{0} is currently unavailable.", info.Name);
			return false;
		}

		public static bool ExitDungeon(PlayerMobile pm, bool confirm)
		{
			if (pm == null || pm.Deleted || !(pm.Map is InstanceMap))
			{
				return false;
			}

			if (!pm.Alive)
			{
				if (pm.Corpse != null && pm.Corpse.Map == pm.Map)
				{
					pm.Corpse.MoveToWorld(pm.Location, pm.Map);
				}

				pm.Resurrect();
				return false;
			}

			var map = (InstanceMap)pm.Map;
			var dungeon = GetDungeon(pm);

			if (dungeon != null && !dungeon.Deleted)
			{
				if (confirm)
				{
					new ConfirmDialogGump(pm)
					{
						Width = 500,
						Height = 400,
						Title = dungeon.Name,
						Html = dungeon.Desc + "\n\nClick OK to exit " + dungeon.Name + "!",
						AcceptHandler = b => ExitDungeon(pm, false)
					}.Send();

					return false;
				}

				dungeon.Kick(pm);
			}
			else
			{
				map.Kick(pm);
			}

			return true;
		}

		public static Container HandleCreateCorpse(
			Mobile owner,
			HairInfo hair,
			FacialHairInfo facialhair,
			List<Item> initialContent,
			List<Item> equipItems)
		{
			Container c = null;

			if (_CreateCorpseSuccessor != null)
			{
				c = _CreateCorpseSuccessor(owner, hair, facialhair, initialContent, equipItems);
			}

			if (c == null || c.Deleted)
			{
				c = Corpse.Mobile_CreateCorpseHandler(owner, hair, facialhair, initialContent, equipItems);
			}

			if (c != null && !c.Deleted)
			{
				var d = GetDungeon(owner);

				if (d != null && !d.Deleted)
				{
					d.OnCorpseCreated(owner, c);
				}
			}

			return c;
		}

        public static int HandleNotoriety(Mobile source, IDamageable target, out bool handled)
        {
            return HandleNotoriety(source, target as Mobile, out handled);
        }

        public static int HandleNotoriety(Mobile source, Mobile target, out bool handled)
		{
			handled = false;

			if (source == null || source.Deleted || !(source.Map is InstanceMap) //
				|| target == null || target.Deleted || !(target.Map is InstanceMap) //
				|| source.Map != target.Map)
			{
				return NotoUtility.Bubble;
			}

			var dungeon =
				Dungeons.Values.FirstOrDefault(
					d => d != null && !d.Deleted && d.Zones.Any(source.InRegion) && d.Zones.Any(target.InRegion));

			if (dungeon != null)
			{
				var noto = dungeon.GetNotoriety(source, target);

				if (noto != NotoUtility.Bubble)
				{
					handled = true;
				}
			}

			return NotoUtility.Bubble;
		}

		private static void HandlePlayerDeath(PlayerDeathEventArgs e)
		{
			var d = GetDungeon(e.Mobile);

			if (d == null && e.Mobile.Corpse != null)
			{
				var z = e.Mobile.Corpse.GetRegion<DungeonZone>();

				if (z != null)
				{
					d = z.Dungeon;
				}
			}

			if (d != null)
			{
				d.HandlePlayerDeath(e);
			}
		}

		private static void HandleCreatureDeath(CreatureDeathEventArgs e)
		{
			var d = GetDungeon(e.Creature);

			if (d == null && e.Corpse != null)
			{
				var z = e.Corpse.GetRegion<DungeonZone>();

				if (z != null)
				{
					d = z.Dungeon;
				}
			}

			if (d != null)
			{
				d.HandleCreatureDeath(e);
			}
		}

		private static void FixPlayerMaps()
		{
			var mapDef = new MapPoint(CSOptions.BounceMap, CSOptions.BounceLocation);

			if (mapDef.InternalOrZero)
			{
				mapDef = new MapPoint(Map.Felucca, new Point3D(1383, 1713, 20)); // Brit Peninsula
			}

			foreach (var m in World.Mobiles.Values.OfType<PlayerMobile>())
			{
				m.FixMap(mapDef);
			}
		}
	}
}
