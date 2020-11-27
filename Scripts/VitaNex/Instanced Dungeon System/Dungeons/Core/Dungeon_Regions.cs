#region Header
//   Vorspire    _,-'/-'/  Dungeon_Regions.cs
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

using Server;
using Server.Mobiles;
#endregion

namespace VitaNex.Dungeons
{
	public abstract partial class Dungeon
	{
		public DungeonZone CreateZone(string name, params Rectangle2D[] area)
		{
			return CreateZone(name, area.Select(Region.ConvertTo3D).ToArray());
		}

		public virtual DungeonZone CreateZone(string name, params Rectangle3D[] area)
		{
			var zone = Zones.Find(z => z != null && z.Name == name);

			if (zone != null && !zone.Deleted)
			{
				zone.Dungeon = this;

				return zone;
			}

			Zones.AddOrReplace(
				zone = new DungeonZone(name, Map, null, area)
				{
					Dungeon = this
				});

			return zone;
		}

		public DungeonZone GetZone(Mobile m)
		{
			return m.GetRegion<DungeonZone>();
		}

		public virtual void OnLocationChanged(DungeonZone zone, Mobile m, Point3D oldLocation)
		{
			if (m == null || m.Deleted)
			{
				return;
			}

			CheckDismount(zone, m);

			if (m.Map != Map)
			{
				return;
			}

			var objs = Map.GetObjectsInRange(m.Location, 0);
			var allow = objs.All(e => CanMoveThrough(zone, m, e));

			objs.Free();

			if (!allow)
			{
				m.Location = oldLocation;
			}
		}

		public virtual void OnMove(Point3D oldLocation, Item i)
		{ }

		public virtual void OnMove(Point3D oldLocation, Mobile m)
		{ }

		public bool OnMoveInto(DungeonZone zone, Mobile m, Direction d, Point3D newLocation, Point3D oldLocation)
		{
			return CanMoveInto(zone, m, d, newLocation, oldLocation);
		}

		public virtual bool CanMoveInto(DungeonZone zone, Mobile m, Direction d, Point3D newLocation, Point3D oldLocation)
		{
			return true;
		}

		public virtual void OnEnter(DungeonZone zone, Mobile m)
		{
			if (m is PlayerMobile)
			{
				DungeonUI.DisplayTo((PlayerMobile)m, false, this);
			}
		}

		public virtual void OnExit(DungeonZone zone, Mobile m)
		{
			if (m is PlayerMobile)
			{
				m.CloseGump(typeof(DungeonUI));
			}
		}

		public virtual void OnEnter(DungeonZone zone, Item i)
		{ }

		public virtual void OnExit(DungeonZone zone, Item i)
		{ }
	}
}
