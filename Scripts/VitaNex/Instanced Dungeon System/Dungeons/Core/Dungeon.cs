#region Header
//   Vorspire    _,-'/-'/  Dungeon.cs
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
using Server.Mobiles;

using VitaNex.InstanceMaps;
#endregion

namespace VitaNex.Dungeons
{
	[PropertyObject]
	public abstract partial class Dungeon : IDungeon
	{
		[CommandProperty(Instances.Access)]
		public abstract DungeonID ID { get; }

		[CommandProperty(Instances.Access)]
		public abstract Map MapParent { get; }

		[CommandProperty(Instances.Access)]
		public abstract Point3D Entrance { get; }

		[CommandProperty(Instances.Access)]
		public abstract Point3D Exit { get; }

		[CommandProperty(Instances.Access)]
		public abstract TimeSpan Duration { get; }

		[CommandProperty(Instances.Access)]
		public abstract TimeSpan Lockout { get; }

		[CommandProperty(Instances.Access)]
		public abstract int GroupMax { get; }

		[CommandProperty(Instances.Access)]
		public abstract string Name { get; }

		[CommandProperty(Instances.Access)]
		public abstract string Desc { get; }

		[CommandProperty(Instances.Access)]
		public virtual Expansion Expansion { get { return Expansion.None; } }

		[CommandProperty(Instances.Access, true)]
		public DungeonSerial Serial { get; private set; }

		protected bool Deserializing { get; private set; }
		protected bool Deserialized { get; private set; }

		public bool Initializing { get; private set; }
		public bool Initialized { get; private set; }

		public bool Generating { get; private set; }
		public bool Generated { get; private set; }

		[CommandProperty(Instances.Access, true)]
		public bool Deleted { get; private set; }

		[CommandProperty(Instances.Access, true)]
		public PollTimer CoreTimer { get; private set; }

		[CommandProperty(Instances.Access, true)]
		public DateTime Created { get; private set; }

		[CommandProperty(Instances.Access, true)]
		public InstanceMap Map { get; private set; }

		[CommandProperty(Instances.Access)]
		public DungeonOptions Options { get; set; }

		[CommandProperty(Instances.Access)]
		public DateTime Deadline { get; set; }

		[CommandProperty(Instances.Access, true)]
		public PlayerMobile ActiveLeader { get; set; }

		[CommandProperty(Instances.Access)]
		public DungeonPortal ExitPortal1 { get; set; }

		[CommandProperty(Instances.Access)]
		public DungeonPortal ExitPortal2 { get; set; }

		public List<DungeonZone> Zones { get; private set; }
		public List<PlayerMobile> Group { get; private set; }
		public List<PlayerMobile> ActiveGroup { get; private set; }

		public List<Mobile> MobileSpawns { get; private set; }
		public List<Item> ItemSpawns { get; private set; }

		public List<DungeonLootEntry> Loot { get; private set; }

		public Dictionary<string, List<string>> Logs { get; private set; }

		public Dungeon()
		{
			Serial = new DungeonSerial();

			Created = DateTime.UtcNow;
			Deadline = Created.Add(Duration);

			LootMode = DungeonLootMode.Advanced;

			EnsureConstructDefaults();
		}

		public Dungeon(DungeonSerial serial)
		{
			Serial = serial;

			EnsureConstructDefaults();
		}

		protected virtual void EnsureConstructDefaults()
		{
			Options = new DungeonOptions();

			Group = new List<PlayerMobile>();
			ActiveGroup = new List<PlayerMobile>();
			Zones = new List<DungeonZone>();

			MobileSpawns = new List<Mobile>();
			ItemSpawns = new List<Item>();

			Loot = new List<DungeonLootEntry>();

			SubCommands = new Dictionary<string, DungeonCommandInfo>();
			Logs = new Dictionary<string, List<string>>();
		}

		private long _NextDefragment;

		public void Defragment()
		{
			if (MobileSpawns != null)
			{
				MobileSpawns.RemoveAll(m => m == null || m.Deleted || m.Map != Map);
			}

			if (ItemSpawns != null)
			{
				ItemSpawns.RemoveAll(i => i == null || i.Deleted || i.Map != Map);
			}

			if (Zones != null)
			{
				Zones.RemoveAll(z => z == null || z.Deleted || z.Map != Map);
				Zones.ForEachReverse(z => z.Defragment());
			}

			if (Logs != null && Logs.Count > 0)
			{
				Logs.RemoveRange(
					(c, l) =>
					{
						if (l == null)
						{
							return true;
						}
	
						l.RemoveAll(String.IsNullOrWhiteSpace);
	
						if (l.Count == 0)
						{
							l.Free(false);
							return true;
						}
	
						if (String.IsNullOrWhiteSpace(c))
						{
							l.Free(true);
							return true;
						}
	
						return false;
					});
			}
		}

		public void Init()
		{
			if (Initialized || Initializing)
			{
				return;
			}

			Initialized = true;

			if (Deleted)
			{
				return;
			}

			Initializing = true;

			EventSink.Shutdown += InternalServerShutdown;
			EventSink.Logout += InternalLogout;
			EventSink.Login += InternalLogin;

			CoreTimer = PollTimer.FromSeconds(1.0, Slice, () => !Deleted && !Initializing && Initialized);

			if (Map == null || Map.Deleted)
			{
				Map = Instances.ResolveMap(this, true);
			}

			if (Map == null || Map.Deleted)
			{
				Initializing = false;

				Delete();
				return;
			}

			_NextDefragment = Core.TickCount + 60000;

			RegisterSubCommands();

			Generate();

			OnInit();

			foreach (var z in Zones.Where(z => z != null && !z.Deleted && z.Dungeon != this))
			{
				z.Dungeon = this;
			}

			Defragment();

			Initializing = false;
		}

		protected virtual void OnInit()
		{ }

		private void Generate()
		{
			if (Generated || Generating)
			{
				return;
			}

			Generated = true;

			if (Deleted)
			{
				return;
			}

			Generating = true;

			OnGenerate();

			Generating = false;
		}

		protected virtual void OnGenerate()
		{ }

		private void Slice()
		{
			if (Deleted || Initializing || Generating)
			{
				return;
			}

			ActiveGroup.ForEachReverse(
				m =>
				{
					if (m == null)
					{
						return;
					}

					if (m.Map == Map && Zones.Any(m.InRegion))
					{
						CheckDismount(m);

						if (!Options.Rules.AllowPets)
						{
							StablePets(m);
						}
					}
					else
					{
						OnExitDungeon(m);
					}
				});

			OnSlice();

			if (DateTime.UtcNow > Deadline)
			{
				OnDeadline();
			}

			if (Core.TickCount < _NextDefragment)
			{
				return;
			}

			_NextDefragment = Core.TickCount + 60000;

			Defragment();
		}

		protected virtual void OnSlice()
		{
			GenerateExitPortal();
			CheckTimeWarning();
			ProcessLoot();
		}

		protected virtual void OnDeadline()
		{
			if (Group != null && Group.Count > 0)
			{
				Group.ForEachReverse(pm => Instances.SetLockout(pm, ID, Lockout));
			}

			Delete();
		}

		protected virtual void GenerateExitPortal()
		{
			if (Deleted)
			{
				return;
			}

			if (Entrance == Exit)
			{
				if ((ExitPortal1 == null || ExitPortal1.Deleted) && Map != null && Entrance != Point3D.Zero)
				{
					ExitPortal1 = CreateItem<DungeonPortal>(Entrance, false);
					ExitPortal1.Name = "Exit Portal";
				}
			}
			else
			{
				if ((ExitPortal1 == null || ExitPortal1.Deleted) && Map != null && Entrance != Point3D.Zero)
				{
					ExitPortal1 = CreateItem<DungeonPortal>(Entrance, false);
					ExitPortal1.Name = "Exit Portal";
				}

				if ((ExitPortal2 == null || ExitPortal2.Deleted) && Map != null && Exit != Point3D.Zero)
				{
					ExitPortal2 = CreateItem<DungeonPortal>(Exit, false);
					ExitPortal2.Name = "Exit Portal";
				}
			}
		}

		private long _NextTimeWarning = Core.TickCount;

		protected virtual void CheckTimeWarning()
		{
			if (Core.TickCount < _NextTimeWarning)
			{
				return;
			}

			var lifespan = Deadline - DateTime.UtcNow;

			if (lifespan <= TimeSpan.Zero)
			{
				return;
			}

			if (lifespan.TotalMinutes > 60)
			{
				_NextTimeWarning = Core.TickCount + 1800000; //30m
			}
			else if (lifespan.TotalMinutes > 30)
			{
				_NextTimeWarning = Core.TickCount + 600000; //10m
			}
			else if (lifespan.TotalMinutes > 5)
			{
				_NextTimeWarning = Core.TickCount + 300000; //5m
			}
			else if (lifespan.TotalSeconds > 60)
			{
				_NextTimeWarning = Core.TickCount + 30000; //30s
			}
			else if (lifespan.TotalSeconds > 10)
			{
				_NextTimeWarning = Core.TickCount + 10000; //10s
			}
			else
			{
				_NextTimeWarning = Core.TickCount + 1000; //1s
			}

			string time;

			if (lifespan.TotalDays > 1.0)
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
			}

			ZoneMessage(0x22, "Reality will caese to exist in {0}!", time);
		}

		private bool _Deleting;

		public void Delete()
		{
			if (Deleted || _Deleting)
			{
				return;
			}
			
			_Deleting = true;

			OnBeforeDelete();

			KickAll();

			EventSink.Shutdown -= InternalServerShutdown;
			EventSink.Logout -= InternalLogout;
			EventSink.Login -= InternalLogin;

			if (CoreTimer != null)
			{
				CoreTimer.Dispose();
				CoreTimer = null;
			}

			Instances.Dungeons.Remove(Serial);

			OnDelete();

			if (ExitPortal1 != null)
			{
				ExitPortal1.Delete();
				ExitPortal1 = null;
			}

			if (ExitPortal2 != null)
			{
				ExitPortal2.Delete();
				ExitPortal2 = null;
			}

			if (Loot != null)
			{
				Loot.ForEach(l => l.Free());
				Loot.Free(true);
			}

			if (Zones != null)
			{
				Zones.ForEachReverse(
					z =>
					{
						if(z != null)
						{
							z.Dungeon = null;
							z.Delete();
						}
					});
				Zones.Free(true);
			}

			if (MobileSpawns != null)
			{
				MobileSpawns.Where(m => m != null && !m.Deleted && !m.Player && m.Map == Map).ForEach(m => m.Delete());
				MobileSpawns.Free(true);
			}

			if (ItemSpawns != null)
			{
				ItemSpawns.Where(m => m != null && !m.Deleted && m.Map == Map && m.Parent == null).ForEach(i => i.Delete());
				ItemSpawns.Free(true);
			}

			if (Map != null)
			{
				if (Map.InstanceRegions.All(z => z == null || z.Deleted))
				{
					Map.Delete();
				}

				Map = null;
			}

			if (Logs != null)
			{
				Logs.Values.Free(true);
				Logs.Clear();
			}

			if (ActiveGroup != null)
			{
				ActiveGroup.Free(true);
			}

			if (Group != null)
			{
				Group.Free(true);
			}

			if (SubCommands != null)
			{
				SubCommands.Clear();
			}

			if (Options != null)
			{
				Options.Clear();
			}

			Deleted = true;

			OnAfterDelete();

			_Deleting = false;
			/*
			Loot = null;
			Zones = null;
			MobileSpawns = null;
			ItemSpawns = null;
			Map = null;
			Logs = null;
			ActiveGroup = null;
			Group = null;
			SubCommands = null;
			Options = null;
			*/
		}

		protected virtual void OnBeforeDelete()
		{
			ZoneMessage(0x22, "Reality breaks down and ceases to exist...");
		}

		protected virtual void OnDelete()
		{ }

		protected virtual void OnAfterDelete()
		{ }

		private void InternalServerShutdown(ShutdownEventArgs e)
		{
			OnServerShutdown();
		}

		protected virtual void OnServerShutdown()
		{
			/*foreach (var m in Group)
			{
				Kick(m);
			}*/
		}

		private void InternalLogout(LogoutEventArgs e)
		{
			if (e != null && e.Mobile is PlayerMobile && !e.Mobile.Deleted && Instances.GetDungeon(e.Mobile) == this)
			{
				OnLogout((PlayerMobile)e.Mobile);
			}
		}

		protected virtual void OnLogout(PlayerMobile m)
		{
			//Kick(m);
		}

		private void InternalLogin(LoginEventArgs e)
		{
			if (e != null && e.Mobile is PlayerMobile && !e.Mobile.Deleted && Zones.Any(e.Mobile.InRegion))
			{
				OnLogin((PlayerMobile)e.Mobile);
			}
		}

		protected virtual void OnLogin(PlayerMobile m)
		{
			//Kick(m);
		}

		public virtual TimeSpan GetLogoutDelay(DungeonZone zone, Mobile m)
		{
			return TimeSpan.Zero;
		}

		public virtual bool CanEnterDungeon(Mobile m)
		{
			return m != null && !m.Deleted;
		}

		public virtual int GetNotoriety(Mobile source, Mobile target)
		{
			if (source == target || !source.Player || !target.Player)
			{
				return NotoUtility.Bubble;
			}

			if (Group.Exists(m => m == source) && Group.Exists(m => m == target))
			{
				return Notoriety.Ally;
			}

			return NotoUtility.Bubble;
		}

		public void KickAll()
		{
			Zones.SelectMany(z => z.Mobiles).ForEach(Kick);
		}

		public void Kick(Mobile m)
		{
			if (m == null || m.Deleted || m.Map != Map || MobileSpawns.Contains(m) ||
				!m.CanBeginAction(InstanceMap.KickPreventionLock))
			{
				return;
			}

			if ((m.Player || m.IsDeadBondedPet) && !m.Alive)
			{
				m.Resurrect();
			}

			var z = Zones.Find(m.InRegion);

			if (z != null && !z.Deleted)
			{
				z.Kick(m);
			}
			else if (Map != null && Instances.GetDungeon(m) == null)
			{
				Map.Kick(m);
			}

			OnExitDungeon(m);
		}

		public IEnumerable<Item> FindItems()
		{
			return FindItems<Item>();
		}

		public IEnumerable<T> FindItems<T>() where T : Item
		{
			return FindItems<T>(null);
		}

		public IEnumerable<Item> FindItems(Func<Item, bool> predicate)
		{
			return FindItems<Item>(predicate);
		}

		public IEnumerable<T> FindItems<T>(Func<T, bool> predicate) where T : Item
		{
			if (predicate != null)
			{
				return Zones.SelectMany(z => z.Items.OfType<T>().Where(predicate));
			}

			return Zones.SelectMany(z => z.Items.OfType<T>());
		}

		public IEnumerable<Mobile> FindMobiles()
		{
			return FindMobiles<Mobile>();
		}

		public IEnumerable<T> FindMobiles<T>() where T : Mobile
		{
			return FindMobiles<T>(null);
		}

		public IEnumerable<Mobile> FindMobiles(Func<Mobile, bool> predicate)
		{
			return FindMobiles<Mobile>(predicate);
		}

		public IEnumerable<T> FindMobiles<T>(Func<T, bool> predicate) where T : Mobile
		{
			if (predicate != null)
			{
				return Zones.SelectMany(z => z.Mobiles.OfType<T>().Where(predicate));
			}

			return Zones.SelectMany(z => z.Mobiles.OfType<T>());
		}

		public virtual TimeSpan ScaleValue(TimeSpan value, double factor)
		{
			return TimeSpan.FromMilliseconds(ScaleValue(value.TotalMilliseconds, factor));
		}

		public virtual sbyte ScaleValue(sbyte value, double factor)
		{
			return unchecked((sbyte)ScaleValue((double)value, factor));
		}

		public virtual byte ScaleValue(byte value, double factor)
		{
			return unchecked((byte)ScaleValue((double)value, factor));
		}

		public virtual short ScaleValue(short value, double factor)
		{
			return unchecked((short)ScaleValue((double)value, factor));
		}

		public virtual ushort ScaleValue(ushort value, double factor)
		{
			return unchecked((ushort)ScaleValue((double)value, factor));
		}

		public virtual int ScaleValue(int value, double factor)
		{
			return unchecked((int)ScaleValue((double)value, factor));
		}

		public virtual uint ScaleValue(uint value, double factor)
		{
			return unchecked((uint)ScaleValue((double)value, factor));
		}

		public virtual long ScaleValue(long value, double factor)
		{
			return unchecked((long)ScaleValue((double)value, factor));
		}

		public virtual ulong ScaleValue(ulong value, double factor)
		{
			return unchecked((ulong)ScaleValue((double)value, factor));
		}

		public virtual float ScaleValue(float value, double factor)
		{
			return (float)ScaleValue((double)value, factor);
		}

		public virtual double ScaleValue(double value, double factor)
		{
			return value * factor;
		}

		public override string ToString()
		{
			return Name ?? ID.ToString();
		}

		public override int GetHashCode()
		{
			return Serial.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return obj is Dungeon && Equals((Dungeon)obj);
		}

		public bool Equals(Dungeon other)
		{
			return !ReferenceEquals(null, other) && Serial.Equals(other.Serial);
		}

		private void SerializeDefaults(GenericWriter writer)
		{
			writer.SetVersion(0);

			writer.Write(Serial);

			Options.Serialize(writer);

			writer.Write(Deleted);
			writer.Write(Generated);

			writer.Write(Created);

			writer.WriteInstanceMap(Map);

			writer.WriteBlockList(Zones, (w, z) => w.WriteInstanceRegion(z));

			writer.WriteMobileList(Group);
			writer.WriteMobileList(ActiveGroup);

			writer.WriteMobileList(MobileSpawns);
			writer.WriteItemList(ItemSpawns);

			writer.WriteBlockList(Loot, (w, e) => w.WriteType(e, t => e.Serialize(w)));
		}

		private void DeserializeDefaults(GenericReader reader)
		{
			reader.GetVersion();

			Serial = reader.ReadHashCode<DungeonSerial>();

			Options.Deserialize(reader);

			Deleted = reader.ReadBool();
			Generated = reader.ReadBool();

			Created = reader.ReadDateTime();

			Map = reader.ReadInstanceMap();

			reader.ReadBlockList(r => r.ReadInstanceRegion<DungeonZone>(), Zones);

			Zones.ForEach(
				z =>
				{
					z.Dungeon = this;
	
					if ((Map == null || Map.Deleted) && z.InstanceMap != null && !z.InstanceMap.Deleted)
					{
						Map = z.InstanceMap;
					}
				});

			Group = reader.ReadStrongMobileList<PlayerMobile>();
			ActiveGroup = reader.ReadStrongMobileList<PlayerMobile>();

			MobileSpawns = reader.ReadStrongMobileList();
			ItemSpawns = reader.ReadStrongItemList();

			reader.ReadBlockList(r => r.ReadTypeCreate<DungeonLootEntry>(this, r), Loot);
		}

		public virtual void Serialize(GenericWriter writer)
		{
			SerializeDefaults(writer);

			var version = writer.SetVersion(1);

			switch (version)
			{
				case 1:
				{
					writer.Write(ActiveLeader);
					writer.WriteFlag(LootMode);
				}
					goto case 0;
				case 0:
				{
					writer.WriteDeltaTime(Deadline);
					writer.Write(SubCommandPrefix);
					writer.Write(ExitPortal2);
				}
					break;
			}
		}

		public virtual void Deserialize(GenericReader reader)
		{
			DeserializeDefaults(reader);

			MobileSpawns.ForEach(m => m.BeginAction(InstanceMap.KickPreventionLock));

			var version = reader.GetVersion();

			switch (version)
			{
				case 1:
				{
					ActiveLeader = reader.ReadMobile<PlayerMobile>();
					LootMode = reader.ReadFlag<DungeonLootMode>();
				}
					goto case 0;
				case 0:
				{
					Deadline = reader.ReadDeltaTime();
					SubCommandPrefix = reader.ReadChar();
					ExitPortal2 = reader.ReadItem<DungeonPortal>();
				}
					break;
			}
		}

		public static bool operator ==(Dungeon left, Dungeon right)
		{
			return ReferenceEquals(null, left) ? ReferenceEquals(null, right) : left.Equals(right);
		}

		public static bool operator !=(Dungeon left, Dungeon right)
		{
			return ReferenceEquals(null, left) ? !ReferenceEquals(null, right) : !left.Equals(right);
		}
	}
}