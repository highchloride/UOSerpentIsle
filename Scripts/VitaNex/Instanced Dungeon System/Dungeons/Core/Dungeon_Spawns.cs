#region Header
//   Vorspire    _,-'/-'/  Dungeon_Spawns.cs
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

using VitaNex.InstanceMaps;
#endregion

namespace VitaNex.Dungeons
{
	public abstract partial class Dungeon
	{
		public virtual double SpawnFactor { get { return 1.0; } }

		public bool IsSpawn(Mobile m)
		{
			return m != null && MobileSpawns.Contains(m);
		}

		public bool IsSpawn(Item i)
		{
			return i != null && ItemSpawns.Contains(i);
		}

		public T CreateMobile<T>(Point3D p, bool replacePack, bool scale) where T : Mobile
		{
			return CreateMobile<T>(p, replacePack, scale, null);
		}

		public T CreateMobile<T>(Point3D p, bool replacePack, bool scale, object[] args) where T : Mobile
		{
			return (T)CreateMobile(typeof(T), p, replacePack, scale, args);
		}

		public Mobile CreateMobile(Type type, Point3D p, bool replacePack, bool scale)
		{
			return CreateMobile(type, p, replacePack, scale, null);
		}

		public Mobile CreateMobile(Type type, Point3D p, bool replacePack, bool scale, object[] args)
		{
			return CreateMobile(type, p, replacePack, scale, SpawnFactor, args);
		}

		public T CreateMobile<T>(Point3D p, bool replacePack, bool scale, double factor) where T : Mobile
		{
			return CreateMobile<T>(p, replacePack, scale, factor, null);
		}

		public T CreateMobile<T>(Point3D p, bool replacePack, bool scale, double factor, object[] args) where T : Mobile
		{
			return (T)CreateMobile(typeof(T), p, replacePack, scale, factor, args);
		}

		public Mobile CreateMobile(Type type, Point3D p, bool replacePack, bool scale, double factor)
		{
			return CreateMobile(type, p, replacePack, scale, factor, null);
		}

		public virtual Mobile CreateMobile(Type type, Point3D p, bool replacePack, bool scale, double factor, object[] args)
		{
			var m = type.CreateInstanceSafe<Mobile>(args);

			if (m == null)
			{
				return null;
			}

			MobileSpawns.Add(m);

			m.BeginAction(InstanceMap.KickPreventionLock);

			if (replacePack)
			{
				var dp = new DungeonPack(m);
				var cp = m.Backpack;

				if (cp != null && !cp.Deleted)
				{
					cp.Items.ForEachReverse(
						i =>
						{
							var l = i.Location;
							dp.DropItem(i);
							i.Location = l;
						});

					cp.Delete();
				}

				m.AddItem(dp);
			}

			m.OnBeforeSpawn(p, Map);

			if (scale)
			{
				ScaleMobile(m, factor);
			}

			if (m is BaseCreature)
			{
				var c = (BaseCreature)m;

				c.Home = p;
				c.RangeHome = 0;
				c.Team = (int)ID;
				c.ApproachWait = true;
				c.ApproachRange = 8;
			}

			m.Direction = Entrance != Point3D.Zero ? p.GetDirection(Entrance) : (Direction)Utility.Random(8);

			if (Map != null)
			{
				m.MoveToWorld(p, Map);
			}

			m.OnAfterSpawn();

			return m;
		}

		public IEnumerable<Static> TileStatic(int itemID, Point3D start, int w, int h, bool checkExist)
		{
			var z = start.Z;
			var end = start.Clone2D(w - 1, h - 1);

			var b = new Rectangle2D(
				new Point2D(Math.Min(start.X, end.X), Math.Min(start.Y, end.Y)),
				new Point2D(Math.Max(start.X, end.X), Math.Max(start.Y, end.Y)));

			return
				b.EnumeratePoints()
				 .Select(p => CreateStatic(itemID, p.ToPoint3D(z), checkExist))
				 .Where(i => i != null && !i.Deleted);
		}

		public Static CreateStatic(int itemID, Point3D p, bool checkExist)
		{
			Static s = null;

			if (checkExist)
			{
				s = ItemSpawns.OfType<Static>().FirstOrDefault(i => i.ItemID == itemID && i.GetWorldLocation() == p);
			}

			if (s == null || s.Deleted)
			{
				s = CreateItem<Static>(p, false);

				if (s != null)
				{
					s.ItemID = itemID;
				}
			}

			return s;
		}

		public IEnumerable<T> TileItem<T>(Point3D start, int w, int h, bool scale) where T : Item
		{
			return TileItem<T>(start, w, h, scale, null);
		}

		public IEnumerable<T> TileItem<T>(Point3D start, int w, int h, bool scale, object[] args) where T : Item
		{
			return TileItem(typeof(T), start, w, h, scale, args).OfType<T>();
		}

		public IEnumerable<Item> TileItem(Type type, Point3D start, int w, int h, bool scale)
		{
			return TileItem(type, start, w, h, scale, null);
		}

		public IEnumerable<Item> TileItem(Type type, Point3D start, int w, int h, bool scale, object[] args)
		{
			var z = start.Z;
			var end = start.Clone2D(w, h);

			var b = new Rectangle2D(
				new Point2D(Math.Min(start.X, end.X), Math.Min(start.Y, end.Y)),
				new Point2D(Math.Max(start.X, end.X), Math.Max(start.Y, end.Y)));

			return
				b.EnumeratePoints().Select(p => CreateItem(type, p.ToPoint3D(z), scale, args)).Where(i => i != null && !i.Deleted);
		}

		public T CreateItem<T>(Point3D p, bool scale) where T : Item
		{
			return CreateItem<T>(p, scale, null);
		}

		public T CreateItem<T>(Point3D p, bool scale, object[] args) where T : Item
		{
			return CreateItem(typeof(T), p, scale, args) as T;
		}

		public Item CreateItem(Type type, Point3D p, bool scale)
		{
			return CreateItem(type, p, scale, null);
		}

		public Item CreateItem(Type type, Point3D p, bool scale, object[] args)
		{
			return CreateItem(type, p, scale, SpawnFactor, args);
		}

		public T CreateItem<T>(Point3D p, bool scale, double factor) where T : Item
		{
			return CreateItem<T>(p, scale, factor, null);
		}

		public T CreateItem<T>(Point3D p, bool scale, double factor, object[] args) where T : Item
		{
			return CreateItem(typeof(T), p, scale, factor, args) as T;
		}

		public Item CreateItem(Type type, Point3D p, bool scale, double factor)
		{
			return CreateItem(type, p, scale, factor, null);
		}

		public virtual Item CreateItem(Type type, Point3D p, bool scale, double factor, object[] args)
		{
			var i = type.CreateInstanceSafe<Item>(args);

			if (i == null)
			{
				return null;
			}

			ItemSpawns.Add(i);

			i.OnBeforeSpawn(p, Map);

			if (scale)
			{
				ScaleItem(i, factor);
			}

			if (Map != null)
			{
				i.MoveToWorld(p, Map);
			}

			i.OnAfterSpawn();

			return i;
		}

		public virtual void ScaleMobile(Mobile m, double factor)
		{
			if (m == null || m.Deleted || m.Player || factor <= 0)
			{
				return;
			}

			Mobile cm;

			if (m.IsControlled(out cm) && cm != null && cm.Player)
			{
				return;
			}

			if (m is BaseCreature)
			{
				var c = (BaseCreature)m;

				c.SetStr(ScaleValue(c.RawStr, factor));
				c.SetDex(ScaleValue(c.RawDex, factor));
				c.SetInt(ScaleValue(c.RawInt, factor));

				c.SetHits(ScaleValue(c.HitsMaxSeed, factor));
				c.SetStam(ScaleValue(c.StamMaxSeed, factor));
				c.SetMana(ScaleValue(c.ManaMaxSeed, factor));

				c.SetDamage(ScaleValue(c.DamageMin, factor), ScaleValue(c.DamageMax, factor));
			}
			else
			{
				m.RawStr = ScaleValue(m.RawStr, factor);
				m.RawDex = ScaleValue(m.RawDex, factor);
				m.RawInt = ScaleValue(m.RawInt, factor);
			}

			foreach (var s in m.Skills.Where(s => s.Base > 0 && s.Cap > 0))
			{
				s.SetCap(ScaleValue(s.Cap, factor));
				s.SetBase(ScaleValue(s.Base, factor));
			}

			foreach (var i in m.FindEquippedItems<Item>())
			{
				ScaleItem(i, factor);
			}
		}

		public virtual void ScaleItem(Item item, double factor)
		{
			if (item == null || item.Deleted || factor <= 0)
			{
				return;
			}

			AosAttributes attrs;

			if (item.SupportsAttribute(out attrs))
			{
				foreach (var attr in AttributesExtUtility.AttrFactors)
				{
					var val = attrs[attr.Key];

					if (val != 0)
					{
						var min = attr.Value.Min;
						var max = attr.Value.Max;

						//min += (max - min) / 2;

						attrs[attr.Key] = Math.Max(min, Math.Min(max, ScaleValue(val, factor)));
					}
				}
			}

			AosArmorAttributes armorAttrs;

			if (item.SupportsAttribute(out armorAttrs))
			{
				foreach (var attr in AttributesExtUtility.ArmorAttrFactors)
				{
					var val = armorAttrs[attr.Key];

					if (val != 0)
					{
						var min = attr.Value.Min;
						var max = attr.Value.Max;

						//min += (max - min) / 2;

						armorAttrs[attr.Key] = Math.Max(min, Math.Min(max, ScaleValue(val, factor)));
					}
				}
			}

			AosWeaponAttributes weaponAttrs;

			if (item.SupportsAttribute(out weaponAttrs))
			{
				foreach (var attr in AttributesExtUtility.WeaponAttrFactors)
				{
					var val = weaponAttrs[attr.Key];

					if (val != 0)
					{
						var min = attr.Value.Min;
						var max = attr.Value.Max;

						//min += (max - min) / 2;

						weaponAttrs[attr.Key] = Math.Max(min, Math.Min(max, ScaleValue(val, factor)));
					}
				}
			}
		}

		protected virtual void OnSpawnActivate(Mobile m)
		{ }

		protected virtual void OnSpawnDeactivate(Mobile m)
		{
			if (Map != null && !Map.Deleted && m != null && !m.Deleted && IsSpawn(m))
			{
				Timer.DelayCall(TimeSpan.FromSeconds(3.0), SendHome, m);
			}
		}

		public void SendHome(Mobile m)
		{
			if (Map == null || Map.Deleted || m == null || m.Deleted || m.InCombat() || !IsSpawn(m))
			{
				return;
			}

			if (m is BaseCreature)
			{
				var c = (BaseCreature)m;

				if (Zones.Any(z => z.Contains(c.Home)))
				{
					Teleport(c, c.Home, Map);
				}
			}
		}
	}
}