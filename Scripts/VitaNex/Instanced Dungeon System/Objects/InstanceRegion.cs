#region Header
//   Vorspire    _,-'/-'/  InstanceRegion.cs
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
using Server.Regions;
#endregion

namespace VitaNex.InstanceMaps
{
	public class InstanceRegion : BaseRegion, IEquatable<InstanceRegion>
	{
		public static List<InstanceRegion> AllRegions { get; private set; }

		static InstanceRegion()
		{
			AllRegions = new List<InstanceRegion>();
		}

		public InstanceRegionSerial Serial { get; private set; }

		public bool Deleted { get; private set; }

		public List<Mobile> Mobiles { get; private set; }
		public List<Item> Items { get; private set; }

		private InstanceMap _InstanceMap;

		public InstanceMap InstanceMap
		{
			get
			{
				if (_InstanceMap == null || _InstanceMap != Map)
				{
					_InstanceMap = Map as InstanceMap;
				}

				return _InstanceMap;
			}
		}

		private InstanceRegion _InstanceParent;

		public InstanceRegion InstanceParent
		{
			get
			{
				if (_InstanceParent == null || _InstanceParent != Parent)
				{
					_InstanceParent = Parent as InstanceRegion;
				}

				return _InstanceParent;
			}
		}

		public InstanceRegion(string name, InstanceMap map, int priority, params Rectangle3D[] area)
			: base(name, map, priority, area)
		{
			Mobiles = new List<Mobile>();
			Items = new List<Item>();

			Serial = new InstanceRegionSerial(Name, Map.MapIndex);

			OnCreated();
		}

		public InstanceRegion(string name, InstanceMap map, InstanceRegion parent, params Rectangle3D[] area)
			: base(name, map, parent, area)
		{
			Mobiles = new List<Mobile>();
			Items = new List<Item>();

			Serial = new InstanceRegionSerial(Name, Map.MapIndex);

			OnCreated();
		}

		public InstanceRegion(string name, InstanceMap map, Rectangle3D[] area, GenericReader reader)
			: base(name, map, null, area)
		{
			Mobiles = new List<Mobile>();
			Items = new List<Item>();

			Serial = new InstanceRegionSerial(Name, Map.MapIndex);

			Deserialize(reader);

			OnCreated();
		}

		protected virtual void OnCreated()
		{
			if (Deleted)
			{
				Unregister();
			}
			else
			{
				Register();
			}
		}

		public override void OnRegister()
		{
			base.OnRegister();

			if (InstanceMap != null && !InstanceMap.Deleted)
			{
				InstanceMap.AddRegion(this, false);
			}

			AllRegions.AddOrReplace(this);
		}

		public override void OnUnregister()
		{
			base.OnUnregister();

			if (InstanceMap != null)
			{
				InstanceMap.RemoveRegion(this, false);
			}

			AllRegions.Remove(this);
			AllRegions.Free(false);
		}

		public virtual void Delete()
		{
			if (Deleted)
			{
				return;
			}

			Deleted = true;

			OnDelete();
			Wipe();
			Unregister();
			OnAfterDelete();
		}

		protected virtual void OnDelete()
		{ }

		protected virtual void OnAfterDelete()
		{ }

		public void Kick(Mobile m)
		{
			if (m != null && InstanceMap != null)
			{
				InstanceMap.Kick(m);
			}
		}

		public void EjectMobiles()
		{
			if (Mobiles != null && Mobiles.Count > 0)
			{
				Mobiles.RemoveAll(o => o == null || o.Deleted || o.Map != Map || !o.InRegion(this));
				Mobiles.ForEachReverse(Kick);
				Mobiles.RemoveAll(o => o == null || o.Deleted || o.Map != Map || !o.InRegion(this));
			}
		}

		private void Wipe()
		{
			EjectMobiles();

			if (Mobiles != null && Mobiles.Count > 0)
			{
				Mobiles.RemoveAll(o => o == null || o.Deleted || o.Map != Map || o.Player || !o.InRegion(this));
				Mobiles.ForEachReverse(o => o.Delete());
			}

			if (Items != null && Items.Count > 0)
			{
				Items.RemoveAll(o => o == null || o.Deleted || o.Map != Map || o.RootParent != null || !o.InRegion(this));
				Items.ForEachReverse(o => o.Delete());
			}

			Defragment();
		}

		public void Defragment()
		{
			if (Mobiles != null && Mobiles.Count > 0)
			{
				Mobiles.RemoveAll(o => o == null || o.Deleted || o.Map != Map || !o.InRegion(this));
			}

			if (Items != null && Items.Count > 0)
			{
				Items.RemoveAll(o => o == null || o.Deleted || o.Map != Map || !o.InRegion(this));
			}
		}

		public virtual void OnEnter(Item item)
		{
			if (item != null && !item.Deleted)
			{
				Items.AddOrReplace(item);
			}
		}

		public virtual void OnExit(Item item)
		{
			if (item != null)
			{
				Items.Remove(item);
			}
		}

		public override void OnEnter(Mobile m)
		{
			base.OnEnter(m);

			if (m != null && !m.Deleted)
			{
				Mobiles.AddOrReplace(m);
			}
		}

		public override void OnExit(Mobile m)
		{
			base.OnExit(m);

			if (m != null)
			{
				Mobiles.Remove(m);
			}
		}

		public virtual void OnMove(Point3D oldLocation, Item i)
		{ }

		public virtual void OnMove(Point3D oldLocation, Mobile m)
		{ }

		public override int GetHashCode()
		{
			return Serial.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return obj is InstanceRegion && Equals((InstanceRegion)obj);
		}

		public virtual bool Equals(InstanceRegion other)
		{
			return !ReferenceEquals(other, null) && Serial == other.Serial;
		}

		public virtual void Serialize(GenericWriter writer)
		{
			var version = writer.SetVersion(0);

			switch (version)
			{
				case 0:
					writer.Write(Deleted);
					break;
			}
		}

		public virtual void Deserialize(GenericReader reader)
		{
			var version = reader.GetVersion();

			switch (version)
			{
				case 0:
					Deleted = reader.ReadBool();
					break;
			}
		}
	}
}
