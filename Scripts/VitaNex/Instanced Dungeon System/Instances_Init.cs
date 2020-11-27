#region Header
//   Vorspire    _,-'/-'/  Instances_Init.cs
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
using System.IO;
using System.Linq;

using Server;
using Server.Mobiles;

using VitaNex.Dungeons;
using VitaNex.IO;
using VitaNex.Network;
#endregion

namespace VitaNex.InstanceMaps
{
    [CoreService("Instance Maps", "1.0.0.0")]
    public static partial class Instances
    {
        static Instances()
        {
            _BounceRestore = new Dictionary<PlayerMobile, MapPoint>(0x100);
            _ItemRestore = new Dictionary<Item, IEntity>(0x1000);

            CSOptions = new InstanceMapsOptions();

            IndexFile = IOUtility.EnsureFile(VitaNexCore.SavesDirectory + "/InstanceMaps/Maps.idx");

            RestoreFile = IOUtility.EnsureFile(VitaNexCore.CacheDirectory + "/InstanceMaps/Restore.bin");
            RestoreFile.SetHidden(true);

            Lockouts = new BinaryDataStore<PlayerMobile, LockoutState>(VitaNexCore.SavesDirectory + "/InstanceMaps", "Lockouts")
            {
                Async = true,
                OnSerialize = SerializeLockouts,
                OnDeserialize = DeserializeLockouts
            };

            Maps = new BinaryDirectoryDataStore<InstanceMapSerial, InstanceMap>(VitaNexCore.SavesDirectory + "/InstanceMaps", "Maps", "bin")
            {
                Async = true,
                OnSerialize = SerializeMap,
                OnDeserialize = DeserializeMap
            };

            Dungeons = new BinaryDirectoryDataStore<DungeonSerial, Dungeon>(VitaNexCore.SavesDirectory + "/InstanceMaps", "Dungeons", "bin")
            {
                Async = true,
                OnSerialize = SerializeDungeon,
                OnDeserialize = DeserializeDungeon
            };

            DungeonTypes = typeof(Dungeon).GetConstructableChildren();

            var instances = DungeonTypes
                           .Select(t => t.CreateInstanceSafe<Dungeon>())
                           .Where(d => d != null && d.ID != DungeonID.None && d.GroupMax > 0)
                           .Where(d => d.MapParent != null && !(d.MapParent is InstanceMap))
                           .ToList();

            DungeonInfo = instances.Select(d => new DungeonInfo(d)).ToArray();

            instances.ForEach(d => d.Delete());
            instances.Free(true);

            DefragmentTimer = PollTimer.FromMinutes(10.0, Defragment, () => !_Defragmenting, false);
        }

        private static void CSConfig()
        {
            NotoUtility.RegisterNameHandler(HandleNotoriety);

            EventSink.PlayerDeath += HandlePlayerDeath;
            EventSink.CreatureDeath += HandleCreatureDeath;

            EventSink.WorldLoad += FixPlayerMaps;

            IndexFile.Deserialize(LoadIndex);
        }

        private static void CSInvoke()
        {
            _CreateCorpseSuccessor = Mobile.CreateCorpseHandler;
            Mobile.CreateCorpseHandler = HandleCreateCorpse;

            Dungeons.Values.Where(d => d != null && !d.Deleted).ForEach(d => VitaNexCore.TryCatch(d.Init, CSOptions.ToConsole));
        }

        private static void CSSave()
        {
            IndexFile.Serialize(SaveIndex);

            Maps.Export();
            Dungeons.Export();
            Lockouts.Export();

            RestoreFile.Serialize(SaveRestore);
            RestoreFile.SetHidden(true);
        }

        private static void CSLoad()
        {
            Maps.Import();
            Dungeons.Import();
            Lockouts.Import();

            Restore();

            DefragmentTimer.Start();
        }

        private static void CSDispose()
        {
            if (Mobile.CreateCorpseHandler == HandleCreateCorpse)
            {
                Mobile.CreateCorpseHandler = _CreateCorpseSuccessor;
            }

            NotoUtility.UnregisterNameHandler(HandleNotoriety);

            EventSink.PlayerDeath -= HandlePlayerDeath;
            EventSink.CreatureDeath -= HandleCreatureDeath;

            DefragmentTimer.Dispose();
            DefragmentTimer = null;
        }

        private static bool SerializeLockouts(GenericWriter writer)
        {
            var version = writer.SetVersion(0);

            switch (version)
            {
                case 0:
                    {
                        writer.WriteBlockDictionary(Lockouts, (w, k, v) =>
                        {
                            w.Write(k);
                            v.Serialize(w);
                        });
                    }
                    break;
            }

            return true;
        }

        private static bool DeserializeLockouts(GenericReader reader)
        {
            var version = reader.GetVersion();

            switch (version)
            {
                case 0:
                    {
                        reader.ReadBlockDictionary(r =>
                        {
                            var k = r.ReadMobile<PlayerMobile>();
                            var v = new LockoutState(r);

                            return new KeyValuePair<PlayerMobile, LockoutState>(k, v);
                        }, Lockouts);
                    }
                    break;
            }

            return true;
        }

        private static void SaveRestore(GenericWriter writer)
        {
            var version = writer.SetVersion(0);

            switch (version)
            {
                case 0:
                    {
                        writer.WriteBlock(w =>
                        {
                            w.Write(Maps.Count);

                            foreach (var map in Maps.Values)
                            {
                                w.WriteBlockDictionary(map.BounceInfo, (w1, m, p) =>
                                {
                                    w1.Write(m);
                                    w1.WriteLocation(p);
                                });

                                w.WriteBlockList(map.Items, (w1, i) =>
                                {
                                    w1.Write(i);
#if NEWPARENT
								    w1.WriteEntity(i.Parent);
#else
                                    w1.WriteEntity(i.ParentEntity);
#endif
                                });
                            }
                        });
                    }
                    break;
            }
        }

        private static void LoadRestore(GenericReader reader)
        {
            var version = reader.GetVersion();

            switch (version)
            {
                case 0:
                    {
                        reader.ReadBlock(r =>
                        {
                            var count = r.ReadInt();

                            while (--count >= 0)
                            {
                                r.ReadBlockDictionary(r1 =>
                                {
                                    var m = r1.ReadMobile<PlayerMobile>();
                                    var p = r1.ReadLocation();

                                    return new KeyValuePair<PlayerMobile, MapPoint>(m, p);
                                }, _BounceRestore);

                                r.ReadBlockDictionary(r1 =>
                                {
                                    var i = r1.ReadItem();
                                    var p = r1.ReadEntity();

                                    return new KeyValuePair<Item, IEntity>(i, p);
                                }, _ItemRestore);
                            }
                        });
                    }
                    break;
            }
        }

        private static void SaveIndex(GenericWriter writer)
        {
            var version = writer.SetVersion(0);

            switch (version)
            {
                case 0:
                    {
                        writer.WriteBlockDictionary(Maps, (w, k, v) =>
                        {
                            w.Write(v.MapIndex);
                            w.Write(v.Parent);
                        });

                        writer.WriteBlockDictionary(Dungeons, (w, k, v) =>
                        {
                            w.Write(k);
                            w.WriteType(v);
                        });
                    }
                    break;
            }
        }

        private static void LoadIndex(GenericReader reader)
        {
            var version = reader.GetVersion();

            switch (version)
            {
                case 0:
                    {
                        reader.ReadBlockDictionary(r =>
                        {
                            var i = r.ReadInt();
                            var p = r.ReadMap();

                            var m = new InstanceMap(i, p, p.Name + "-INDEX", Season.Desolation, MapRules.None);

                            return new KeyValuePair<InstanceMapSerial, InstanceMap>(m.Serial, m);
                        }, Maps);

                        reader.ReadBlockDictionary(r =>
                        {
                            var s = r.ReadHashCode<DungeonSerial>();
                            var d = r.ReadTypeCreate<Dungeon>(s);

                            return new KeyValuePair<DungeonSerial, Dungeon>(d.Serial, d);
                        }, Dungeons);
                    }
                    break;
            }
        }

        private static bool SerializeMap(GenericWriter writer, InstanceMapSerial serial, InstanceMap map)
        {
            var version = writer.SetVersion(0);

            switch (version)
            {
                case 0:
                    {
                        writer.WriteBlock(w =>
                        {
                            WriteInstanceMap(w, map);

                            map.Serialize(w);
                        });
                    }
                    break;
            }

            return true;
        }

        private static Tuple<InstanceMapSerial, InstanceMap> DeserializeMap(GenericReader reader)
        {
            Tuple<InstanceMapSerial, InstanceMap> value = null;

            var version = reader.GetVersion();

            switch (version)
            {
                case 0:
                    {
                        reader.ReadBlock(r =>
                        {
                            var map = ReadInstanceMap(r);

                            map.Deserialize(r);

                            value = Tuple.Create(map.Serial, map);
                        });
                    }
                    break;
            }

            return value;
        }

        private static bool SerializeDungeon(GenericWriter writer, DungeonSerial serial, Dungeon dungeon)
        {
            var version = writer.SetVersion(0);

            switch (version)
            {
                case 0:
                    {
                        writer.WriteBlock(w =>
                        {
                            WriteDungeon(w, dungeon);

                            dungeon.Serialize(w);
                        });
                    }
                    break;
            }

            return true;
        }

        private static Tuple<DungeonSerial, Dungeon> DeserializeDungeon(GenericReader reader)
        {
            Tuple<DungeonSerial, Dungeon> value = null;

            var version = reader.GetVersion();

            switch (version)
            {
                case 0:
                    {
                        reader.ReadBlock(r =>
                        {
                            var dungeon = ReadDungeon(r);

                            dungeon.Deserialize(r);

                            value = Tuple.Create(dungeon.Serial, dungeon);
                        });
                    }
                    break;
            }

            return value;
        }

        public static void WriteInstanceMap(this GenericWriter writer, InstanceMap map)
        {
            if (map != null)
            {
                writer.Write(true);
                writer.Write(map.Serial);
            }
            else
            {
                writer.Write(false);
            }
        }

        public static InstanceMap ReadInstanceMap(this GenericReader reader)
        {
            if (reader.ReadBool())
            {
                return FindMap(reader.ReadHashCode<InstanceMapSerial>());
            }

            return null;
        }

        public static void WriteInstanceRegion(this GenericWriter writer, InstanceRegion reg)
        {
            if (reg != null)
            {
                writer.Write(true);
                writer.Write(reg.Serial);
            }
            else
            {
                writer.Write(false);
            }
        }

        public static InstanceRegion ReadInstanceRegion(this GenericReader reader)
        {
            return ReadInstanceRegion<InstanceRegion>(reader);
        }

        public static T ReadInstanceRegion<T>(this GenericReader reader) where T : InstanceRegion
        {
            if (reader.ReadBool())
            {
                return FindRegion(reader.ReadHashCode<InstanceRegionSerial>()) as T;
            }

            return null;
        }

        public static void WriteDungeon(this GenericWriter writer, Dungeon d)
        {
            if (d != null)
            {
                writer.Write(true);
                writer.Write(d.Serial);
            }
            else
            {
                writer.Write(false);
            }
        }

        public static Dungeon ReadDungeon(this GenericReader reader)
        {
            return ReadDungeon<Dungeon>(reader);
        }

        public static T ReadDungeon<T>(this GenericReader reader) where T : Dungeon
        {
            if (reader.ReadBool())
            {
                return FindDungeon(reader.ReadHashCode<DungeonSerial>()) as T;
            }

            return null;
        }
    }
}
