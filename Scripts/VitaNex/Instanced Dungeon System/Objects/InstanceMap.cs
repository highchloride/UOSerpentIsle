#region Header
//   Vorspire    _,-'/-'/  InstanceMap.cs
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
using System.Reflection;
using System.Linq;

using Server;
using Server.Mobiles;

using VitaNex.Network;
#endregion

namespace VitaNex.InstanceMaps
{
    public sealed class InstanceMap : Map, IEquatable<InstanceMap>
    {
        public static object KickPreventionLock = typeof(InstanceMap);

        private static readonly FieldInfo _InternalMapNames =
            typeof(Map).GetField("_MapNames", BindingFlags.Static | BindingFlags.NonPublic) ??
            typeof(Map).GetField("m_MapNames", BindingFlags.Static | BindingFlags.NonPublic);

        /// <summary>
        ///     Forces the update of inaccessible private static fields of the Map class.
        /// </summary>
        public static void UpdateMaps()
        {
            try
            {
                AllMaps.Sort((m1, m2) =>
                {
                    var res = 0;

                    if (m1.CompareNull(m2, ref res))
                    {
                        return res;
                    }

                    return m1.MapIndex.CompareTo(m2.MapIndex);
                });

                if (_InternalMapNames == null)
                {
                    return;
                }

                _InternalMapNames.SetValue(null, null);
            }
            catch (Exception e)
            {
                Instances.CSOptions.ToConsole("Update of Map Values and Names array failed:");
                Instances.CSOptions.ToConsole(e);
            }
        }

        private static int UnusedIndex()
        {
            var index = Maps.Length;

            while (--index >= 0)
            {
                if (index != 255 && index != 127 && Maps[index] == null)
                {
                    return index;
                }
            }

            return -1;
        }

        public InstanceMapSerial Serial { get; private set; }

        public Map Parent { get; private set; }

        public bool Deleted { get; private set; }

        public List<InstanceRegion> InstanceRegions { get; private set; }

        public List<Mobile> Mobiles { get; private set; }
        public List<Item> Items { get; private set; }

        public Dictionary<PlayerMobile, MapPoint> BounceInfo { get; private set; }

        public InstanceMap(Map parent, string name, Season season, MapRules rules)
            : base(parent.MapID, UnusedIndex(), parent.MapID, parent.Width, parent.Height, (int)season, name, rules)
        {
            InstanceRegions = new List<InstanceRegion>();

            Mobiles = new List<Mobile>();
            Items = new List<Item>();

            BounceInfo = new Dictionary<PlayerMobile, MapPoint>();

            if (!Maps.InBounds(MapIndex) || MapIndex == 255 || MapIndex == 127)
            {
                Serial = new InstanceMapSerial(-MapIndex);
                Deleted = true;
                return;
            }

            Serial = new InstanceMapSerial(MapIndex);
            Parent = parent;

            Maps[MapIndex] = this;

            AllMaps.Add(this);

            UpdateMaps();
        }

        public InstanceMap(int index, Map parent, string name, Season season, MapRules rules)
            : base(parent.MapID, index, parent.MapID, parent.Width, parent.Height, (int)season, name, rules)
        {
            InstanceRegions = new List<InstanceRegion>();

            Mobiles = new List<Mobile>();
            Items = new List<Item>();

            BounceInfo = new Dictionary<PlayerMobile, MapPoint>();

            if (!Maps.InBounds(MapIndex) || MapIndex == 255 || MapIndex == 127)
            {
                Serial = new InstanceMapSerial(-MapIndex);
                Deleted = true;
                return;
            }

            Serial = new InstanceMapSerial(MapIndex);
            Parent = parent;

            Maps[MapIndex] = this;

            AllMaps.Add(this);

            UpdateMaps();
        }

        public void Delete()
        {
            if (Deleted)
            {
                return;
            }

            Wipe();

            Deleted = true;

            var dr = DefaultRegion;

            if (dr != null)
            {
                dr.Unregister();

                DefaultRegion = null;
            }

            if (Maps.InBounds(MapIndex) && Maps[MapIndex] == this)
            {
                Maps[MapIndex] = null;
            }

            if (AllMaps.Remove(this))
            {
                UpdateMaps();
            }

            Instances.Maps.Remove(Serial);
        }

        public void AddRegion(InstanceRegion region, bool register)
        {
            if (region == null || region.Deleted || InstanceRegions == null)
            {
                return;
            }

            InstanceRegions.AddOrReplace(region);

            if (register)
            {
                region.Register();
            }
        }

        public bool RemoveRegion(InstanceRegion region, bool unregister)
        {
            if (region == null || InstanceRegions == null || !InstanceRegions.Remove(region))
            {
                return false;
            }

            if (unregister)
            {
                region.Unregister();
            }

            return true;
        }

        public void Kick(Mobile m)
        {
            if (m != null && !m.Deleted && (m.Player || m.CanBeginAction(KickPreventionLock)))
            {
                Bounce(m);
            }
        }

        private void Bounce(Mobile m)
        {
            if (m == null || m.Deleted)
            {
                return;
            }

            var bounce = new MapPoint(Instances.CSOptions.BounceMap, Instances.CSOptions.BounceLocation);

            if (bounce.InternalOrZero)
            {
                bounce = new MapPoint(Felucca, new Point3D(1383, 1713, 20)); // Brit Peninsula
            }

            if (m is PlayerMobile pm)
            {
                var mp = ClearBounce(pm);

                if (mp != null && !mp.InternalOrZero)
                {
                    bounce = mp;
                }

                if (!pm.Alive)
                {
                    pm.Resurrect();
                }

                if (pm.Map == this)
                {
                    ScreenFX.FadeOut.Send(pm);

                    bounce.MoveToWorld(pm);

                    SummonPets(pm);
                }
                else if (pm.Map == Internal && pm.LogoutMap == this)
                {
                    pm.LogoutMap = bounce;
                    pm.LogoutLocation = bounce;

                    StablePets(pm);
                }
                else if (pm.Map == null)
                {
                    pm.Map = Internal;
                    pm.Location = Point3D.Zero;

                    pm.LogoutMap = bounce;
                    pm.LogoutLocation = bounce;

                    StablePets(pm);
                }

                return;
            }

            if (m is BaseCreature bc)
            {
                if (bc.IsControlled(out PlayerMobile master) && bc.Stable(false))
                {
                    master.AllFollowers.Remove(bc);
                    return;
                }
            }

            if (m.Map == this)
            {
                bounce.MoveToWorld(m);
            }
            else if (m.Map == Internal && m.LogoutMap == this)
            {
                m.LogoutMap = bounce;
                m.LogoutLocation = bounce;
            }
            else if (m.Map == null)
            {
                m.Map = Internal;
                m.Location = Point3D.Zero;

                m.LogoutMap = bounce;
                m.LogoutLocation = bounce;
            }
        }

        public MapPoint RecordBounce(PlayerMobile pm, Point3D loc, Map map)
        {
            if (BounceInfo == null || pm == null || map == null || map == Internal || map is InstanceMap)
            {
                return MapPoint.Empty;
            }

            var mp = BounceInfo.GetValue(pm);

            if (mp == null)
            {
                BounceInfo[pm] = mp = new MapPoint(map, loc);
            }
            else
            {
                mp.Location = loc;
                mp.Map = map;
            }

            return mp;
        }

        public MapPoint ClearBounce(PlayerMobile pm)
        {
            if (BounceInfo == null || pm == null)
            {
                return MapPoint.Empty;
            }

            var mp = BounceInfo.GetValue(pm);

            BounceInfo.Remove(pm);

            return mp;
        }

        public void StablePets(PlayerMobile pm)
        {
            if (pm == null || pm.Deleted || pm.AllFollowers.IsNullOrEmpty())
            {
                return;
            }

            var count = 0;

            pm.AllFollowers.ForEachReverse(o =>
            {
                if (o is BaseCreature && ((BaseCreature)o).Stable(false))
                {
                    pm.AllFollowers.Remove(o);

                    ++count;
                }
            });

            if (count > 0)
            {
                pm.SendMessage(0x55, "Your {0} been stabled.", count > 1 ? "pets have" : "pet has");
            }
        }

        public void SummonPets(PlayerMobile pm)
        {
            if (pm == null || pm.Deleted || pm.Map == null || pm.Map == Internal || pm.AllFollowers.IsNullOrEmpty())
            {
                return;
            }

            var count = 0;

            pm.AllFollowers.ForEachReverse(o =>
            {
                if ((o.Map == null || o.Map == Internal || o.Map == this) && (!(o is IMount) || ((IMount)o).Rider == null))
                {
                    o.MoveToWorld(pm.Location, pm.Map);

                    ++count;
                }
            });

            if (count > 0)
            {
                pm.SendMessage(0x55, "Your {0} been summoned.", count > 1 ? "pets have" : "pet has");
            }
        }

        public void EjectMobiles()
        {
            if (Mobiles != null && Mobiles.Count > 0)
            {
                Mobiles.RemoveAll(o => o == null || o.Deleted || o.Map != this);
                Mobiles.ForEachReverse(Kick);
                Mobiles.RemoveAll(o => o == null || o.Deleted || o.Map != this);
            }

            if (BounceInfo != null && BounceInfo.Count > 0)
            {
                BounceInfo.Keys.ForEachReverse(Kick);
            }
        }

        private void Wipe()
        {
            EjectMobiles();

            if (Mobiles != null && Mobiles.Count != 0)
            {
                Mobiles.RemoveAll(o => o == null || o.Deleted || o.Map != this || o.Player);
                Mobiles.ForEachReverse(o => o.Delete());
            }

            if (Items != null && Items.Count != 0)
            {
                Items.RemoveAll(o => o == null || o.Deleted || o.Map != this || o.RootParent != null);
                Items.ForEachReverse(o => o.Delete());
            }

            if (InstanceRegions != null && InstanceRegions.Count != 0)
            {
                InstanceRegions.RemoveAll(o => o == null || o.Deleted || o.Map != this);
                InstanceRegions.ForEachReverse(o => o.Delete());
            }

            Defragment();
        }

        public void Defragment()
        {
            if (Mobiles != null && Mobiles.Count > 0)
            {
                Mobiles.RemoveAll(o => o == null || o.Deleted || o.Map != this);
            }

            if (Items != null && Items.Count > 0)
            {
                Items.RemoveAll(o => o == null || o.Deleted || o.Map != this);
            }

            if (InstanceRegions != null && InstanceRegions.Count > 0)
            {
                InstanceRegions.RemoveAll(o => o == null || o.Deleted || o.Map != this);
            }

            if (BounceInfo != null && BounceInfo.Count > 0)
            {
                BounceInfo.RemoveKeyRange(m => m == null || m.Deleted || (m.Map != null && m.Map != Internal && m.Map != this));
                BounceInfo.RemoveValueRange(p => p == null || p.InternalOrZero);
            }
        }

        public override void OnEnter(Item item)
        {
            base.OnEnter(item);

            if (item == null || item.Deleted)
            {
                return;
            }

            if (Items != null)
            {
                Items.AddOrReplace(item);
            }

            var reg = item.GetRegion<InstanceRegion>();

            if (reg != null)
            {
                reg.OnEnter(item);
            }
        }

        public override void OnLeave(Item item)
        {
            base.OnLeave(item);

            if (item == null)
            {
                return;
            }

            if (Items != null)
            {
                Items.Remove(item);
            }

            var reg = item.GetRegion<InstanceRegion>();

            if (reg != null)
            {
                reg.OnExit(item);
            }
        }

        public override void OnEnter(Mobile m)
        {
            base.OnEnter(m);

            if (Mobiles != null && m != null && !m.Deleted)
            {
                Mobiles.AddOrReplace(m);
            }
        }

        public override void OnLeave(Mobile m)
        {
            base.OnLeave(m);

            if (Mobiles != null && m != null)
            {
                Mobiles.Remove(m);
            }
        }

        public override void OnMove(Point3D oldLocation, Item i)
        {
            base.OnMove(oldLocation, i);

            var reg = i.GetRegion<InstanceRegion>();

            if (reg != null)
            {
                reg.OnMove(oldLocation, i);
            }
        }

        public override void OnMove(Point3D oldLocation, Mobile m)
        {
            base.OnMove(oldLocation, m);

            var reg = m.GetRegion<InstanceRegion>();

            if (reg != null)
            {
                reg.OnMove(oldLocation, m);
            }
        }

        public override int GetHashCode()
        {
            return Serial.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is InstanceMap && Equals((InstanceMap)obj);
        }

        public bool Equals(InstanceMap other)
        {
            return other != null && Serial == other.Serial;
        }

        public void Serialize(GenericWriter writer)
        {
            var version = writer.SetVersion(0);

            switch (version)
            {
                case 0:
                    {
                        writer.Write(Deleted);

                        writer.Write(Name);
                        writer.Write(Season);
                        writer.WriteFlag(Rules);

                        writer.WriteBlockList(InstanceRegions, (w, r) =>
                        {
                            w.WriteType(r);
                            w.Write(r.Name);
                            w.WriteArray(r.Area, (w2, b) => w2.Write(b));

                            r.Serialize(w);
                        });

                        writer.WriteBlockDictionary(BounceInfo, (w, k, v) =>
                        {
                            w.Write(k);
                            w.WriteLocation(v);
                        });
                    }
                    break;
            }
        }

        public void Deserialize(GenericReader reader)
        {
            var version = reader.GetVersion();

            switch (version)
            {
                case 0:
                    {
                        Deleted = reader.ReadBool();

                        Name = reader.ReadString();
                        Season = reader.ReadInt();
                        Rules = reader.ReadFlag<MapRules>();

                        InstanceRegions = reader.ReadBlockList(r =>
                        {
                            var t = r.ReadType();
                            var name = r.ReadString();
                            var area = r.ReadArray(r1 => r1.ReadRect3D());

                            if (Regions.ContainsKey(name))
                            {
                                var reg = Regions[name];

                                if (!(reg is InstanceRegion ir) || !reg.TypeEquals(t, false) || !reg.Area.ContentsEqual(area, true))
                                {
                                    reg.Unregister();

                                    Regions.Remove(name);
                                }
                                else
                                {
                                    ir.Deserialize(r);

                                    return ir;
                                }
                            }

                            return t.CreateInstanceSafe<InstanceRegion>(name, this, area, r);
                        }, InstanceRegions);

                        BounceInfo = reader.ReadBlockDictionary(r =>
                        {
                            var k = r.ReadMobile<PlayerMobile>();
                            var v = r.ReadLocation();

                            return new KeyValuePair<PlayerMobile, MapPoint>(k, v);
                        }, BounceInfo);
                    }
                    break;
            }
        }
    }
}
