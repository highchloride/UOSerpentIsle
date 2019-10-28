using System;
using System.Collections.Generic;
using Server;
using Server.Accounting;
using Server.Mobiles;
using Server.Multis;
using Server.Items;

namespace Server.PoisonousSwamps
{
    //UOSI: All swamps now cause deadly poison. This...might be overkill.
    //All swamps now dismount the rider
	public class EffectLogin
	{
		public static void Initialize()
		{
            EventSink.Movement += new MovementEventHandler(EventSink_MovementSwampCheck);
		}

        private static void EventSink_MovementSwampCheck(MovementEventArgs args)
        {
            Mobile mobile = args.Mobile;
            PlayerMobile pm = mobile as PlayerMobile;

            if (pm != null)
                SwampCheck(pm);
        }
        private static void SwampCheck(PlayerMobile from)
        {
            // basic sanity checks

            if (from == null)
                return;  // wtf? O.o;

            // don't poison staff
            if ( from.AccessLevel >= AccessLevel.GameMaster )
            	 return;

            // if the player is dead, don't poison them. Thanks GrayStar! :>
            if (!from.Alive)
                return;

            // don't poison a poisoned player.
            if (from.Poisoned)// || from.Mounted)
                return;

            // no swamps on the internal map.
            if (from.Map == Map.Internal)
                return;

            // is the player wearing swamp boots and not mounted?
            Item shoes = from.FindItemOnLayer(Layer.Shoes);
            if (shoes != null && shoes is SwampBoots)
                return;

            // find out if the player is moving over a land/terrain (ground), static/frozen (dungeon) swamp, or static/unfrozen/added swamp.

            // initialize common variables.
            Map map = from.Map;

            // is it a land/terrain swamp?

            LandTile lt = map.Tiles.GetLandTile(from.X, from.Y);

            if (IsDeepLandSwamp(lt.ID) && lt.Z == from.Z)
            {
                DismountRoutine(from);

                if (Utility.RandomDouble() < 0.15)
                {
                    // poison player
                    from.Poison = Poison.Deadly;

                    from.SendMessage("You were poisoned by the swamp!");

                    return;
                }
            }
            else if (IsLightLandSwamp(lt.ID) && lt.Z == from.Z)
            {
                DismountRoutine(from);

                if (Utility.RandomDouble() < 0.05)
                {
                    // poison player
                    from.Poison = Poison.Deadly;

                    from.SendMessage("You were poisoned by the swamp!");

                    return;
                }
            }

            //FOOTSTEP SFX MOD
            else if (IsDirtLand(lt.ID) && lt.Z == from.Z)
            {
                from.PlaySound(Utility.RandomList(0x33B, 0x33C));

                return;
            }
            else if (IsSandLand(lt.ID) && lt.Z == from.Z)
            {
                from.PlaySound(Utility.RandomList(0x33F, 0x340));

                return;
            }
            else if (IsSnowLand(lt.ID) && lt.Z == from.Z)
            {
                from.PlaySound(Utility.RandomList(0x341, 0x342));

                return;
            }
            else if (IsTileLightWoodenFloor(lt.ID) && lt.Z == from.Z)
            {
                from.PlaySound(Utility.RandomList(0x121, 0x122));

                return;
            }
            else if (IsTileDarkWoodenFloor(lt.ID) && lt.Z == from.Z)
            {
                from.PlaySound(Utility.RandomList(0x123, 0x124));

                return;
            }
            else if (IsTileHardStoneFloor(lt.ID) && lt.Z == from.Z)
            {
                from.PlaySound(Utility.RandomList(0x33D, 0x33E));

                return;
            }

            // is it a static swamp?

            StaticTile[] tiles = map.Tiles.GetStaticTiles(from.X, from.Y);

            for (int i = 0; i < tiles.Length; ++i)
            {
                StaticTile t = tiles[i];
                ItemData id = TileData.ItemTable[t.ID & 0x3FFF];

                int tand = t.ID & 0x3FFF;

                if (t.Z != from.Z)
                {
                    continue;
                }
                else if (IsDeepStaticSwamp(tand))
                {
                    DismountRoutine(from);

                    if (Utility.RandomDouble() < 0.15) // 0.15% chance
                    {
                        // poison player
                        from.Poison = Poison.Deadly;

                        from.SendMessage("You were poisoned by the swamp!");

                        return;
                    }
                }
                else if (IsLightStaticSwamp(tand))
                {
                    DismountRoutine(from);

                    if (Utility.RandomDouble() < 0.05) // 0.05% chance
                    {
                        // poison player
                        from.Poison = Poison.Deadly;

                        from.SendMessage("You were poisoned by the swamp!");

                        return;
                    }
                }

                //FOOTSTEP SFX MOD
                else if (IsStaticLightWoodenFloor(tand))
                {
                    from.PlaySound(Utility.RandomList(0x121, 0x122));
                    return;
                }
                else if (IsStaticDarkWoodenFloor(tand))
                {
                    from.PlaySound(Utility.RandomList(0x123, 0x124));
                    return;
                }
                else if (IsStaticBambooFloorAndSteps(tand))
                {
                    from.PlaySound(Utility.RandomList(0x127, 0x128));
                    return;
                }
                else if (IsStaticHardStoneFloor(tand))
                {
                    from.PlaySound(Utility.RandomList(0x12F, 0x130));
                    return;
                }
                else if (IsStaticBushes(tand))
                {
                    from.PlaySound(Utility.RandomList(0x12D, 0x12E));
                    return;
                }
                else if (IsShallowWaterSurface(tand))
                {
                    from.PlaySound(Utility.RandomList(0x131, 0x132));
                    return;
                }
                else if (IsDeepWaterSurface(tand))
                {
                    from.PlaySound(Utility.RandomList(0x133, 0x134));
                    return;
                }
            }

            // is it an added swamp?

            IPooledEnumerable eable = map.GetItemsInRange(new Point3D(from.X, from.Y, from.Z), 0);

            foreach (Item item in eable)
            {
                if (item == null || item.Z != from.Z)
                {
                    continue;
                }
                else if (IsDeepStaticSwamp(item.ItemID))
                {
                    DismountRoutine(from);

                    if (Utility.RandomDouble() < 0.15) // 0.15% chance
                    {
                        // poison player
                        from.Poison = Poison.Deadly;

                        from.SendMessage("You were poisoned by the swamp!");

                        return;
                    }
                }
                else if (IsLightStaticSwamp(item.ItemID))
                {
                    DismountRoutine(from);

                    if (Utility.RandomDouble() < 0.05) // 0.05% chance
                    {
                        // poison player
                        from.Poison = Poison.Deadly;

                        from.SendMessage("You were poisoned by the swamp!");

                        return;
                    }
                }

                //FOOTSTEP SFX MOD
                else if (IsStaticLightWoodenFloor(item.ItemID))
                {
                    from.PlaySound(Utility.RandomList(0x121, 0x122));
                    return;
                }
                else if (IsStaticDarkWoodenFloor(item.ItemID))
                {
                    from.PlaySound(Utility.RandomList(0x123, 0x124));
                    return;
                }
                else if (IsStaticBambooFloorAndSteps(item.ItemID))
                {
                    from.PlaySound(Utility.RandomList(0x127, 0x128));
                    return;
                }
                else if (IsStaticHardStoneFloor(item.ItemID))
                {
                    from.PlaySound(Utility.RandomList(0x12F, 0x130));
                    return;
                }
                else if (IsStaticBushes(item.ItemID))
                {
                    from.PlaySound(Utility.RandomList(0x12D, 0x12E));
                    return;
                }
                else if (IsShallowWaterSurface(item.ItemID))
                {
                    from.PlaySound(Utility.RandomList(0x131, 0x132));
                    return;
                }
                else if (IsDeepWaterSurface(item.ItemID))
                {
                    from.PlaySound(Utility.RandomList(0x133, 0x134));
                    return;
                }
            }

            from.PlaySound(Utility.RandomList(0x12B, 0x12C));

            eable.Free();
        }

        //UOSI - Horses throw riders in swamps
        //This can be extended to make it so some mounts don't give a shit about the swamp
        private static void DismountRoutine(Mobile from)
        {
            if(from.Mounted)
            {
                from.Dismount();
                from.SendMessage("Thy mount rebuffs the poisoned waters!");
            }            
        }

        private static bool IsLightLandSwamp(int itemID)
        {
            if (itemID >= 15808 && itemID <= 15833)
                return true;

            if (itemID >= 15835 && itemID <= 15836)
                return true;

            if (itemID >= 15838 && itemID <= 15848)
                return true;

            if (itemID >= 15853 && itemID <= 15857)
                return true;

            return false;
        }

        private static bool IsDeepLandSwamp(int itemID)
        {
            if (itemID >= 15849 && itemID <= 15852)
                return true;

            return false;
        }

        //FOOTSTEP SFX MOD
        private static bool IsDirtLand(int itemID)
        {
            if (itemID >= 113 && itemID <= 140)
                return true;

            if (itemID >= 332 && itemID <= 348)
                return true;

            if (itemID >= 353 && itemID <= 372)
                return true;

            if (itemID >= 476 && itemID <= 495)
                return true;

            if (itemID >= 581 && itemID <= 601)
                return true;

            if (itemID >= 887 && itemID <= 890)
                return true;

            if (itemID >= 1351 && itemID <= 1378)
                return true;

            if (itemID >= 1571 && itemID <= 1598)
                return true;

            return false;
        }

        private static bool IsSandLand(int itemID)
        {
            if (itemID >= 22 && itemID <= 25)
                return true;

            if (itemID >= 51 && itemID <= 62)
                return true;

            if (itemID >= 286 && itemID <= 301)
                return true;

            if (itemID >= 642 && itemID <= 657)
                return true;

            if (itemID >= 821 && itemID <= 860)
                return true;

            if (itemID >= 951 && itemID <= 970)
                return true;

            if (itemID >= 1447 && itemID <= 1466)
                return true;

            if (itemID >= 1611 && itemID <= 1650)
                return true;

            return false;
        }

        private static bool IsSnowLand(int itemID)
        {
            if (itemID >= 268 && itemID <= 285)
                return true;

            if (itemID >= 302 && itemID <= 305)
                return true;

            if (itemID >= 901 && itemID <= 940)
                return true;

            if (itemID >= 1471 && itemID <= 1506)
                return true;

            return false;
        }

        private static bool IsTileLightWoodenFloor(int itemID)
        {
            if (itemID >= 1038 && itemID <= 1045)
                return true;

            if (itemID >= 1227 && itemID <= 1249)
                return true;

            return false;
        }

        private static bool IsTileDarkWoodenFloor(int itemID)
        {
            if (itemID >= 1030 && itemID <= 1037)
                return true;

            if (itemID >= 1203 && itemID <= 1222)
                return true;

            return false;
        }

        private static bool IsTileHardStoneFloor(int itemID)
        {
            if (itemID >= 513 && itemID <= 536)
                return true;

            if (itemID >= 1001 && itemID <= 1029)
                return true;

            if (itemID >= 1046 && itemID <= 1198)
                return true;

            if (itemID >= 1255 && itemID <= 1308)
                return true;

            return false;
        }

        private static bool IsLightStaticSwamp(int itemID)
        {
            if (itemID >= 12809 && itemID <= 12810)
                return true;

            if (itemID >= 12882 && itemID <= 12905)
                return true;

            if (itemID >= 12912 && itemID <= 12933)
                return true;

            return false;
        }

        private static bool IsDeepStaticSwamp(int itemID)
        {
            if (itemID >= 12813 && itemID <= 12881)
                return true;

            if (itemID >= 12906 && itemID <= 12911)
                return true;

            return false;
        }

        //FOOTSTEP SFX MOD
        private static bool IsStaticLightWoodenFloor(int itemID)
        {
            if (itemID >= 1185 && itemID <= 1188)
                return true;

            if (itemID >= 1222 && itemID <= 1249)
                return true;

            if (itemID >= 10042 && itemID <= 10065)
                return true;

            if (itemID >= 10350 && itemID <= 10373)
                return true;

            if (itemID >= 11576 && itemID <= 11579)
                return true;

            if (itemID >= 11723 && itemID <= 11726)
                return true;

            return false;
        }

        private static bool IsStaticDarkWoodenFloor(int itemID)
        {
            if (itemID >= 1189 && itemID <= 1216)
                return true;

            if (itemID >= 1993 && itemID <= 2000)
                return true;

            if (itemID >= 10018 && itemID <= 10041)
                return true;

            if (itemID >= 10592 && itemID <= 10639)
                return true;

            return false;
        }

        private static bool IsStaticBambooFloorAndSteps(int itemID)
        {
            if (itemID >= 1217 && itemID <= 1221)
                return true;
            if (itemID >= 1284 && itemID <= 1292)
                return true;
            if (itemID >= 1801 && itemID <= 1821)
                return true;
            if (itemID >= 1822 && itemID <= 1823)
                return true;
            if (itemID >= 1824 && itemID <= 1845)
                return true;
            if (itemID >= 1846 && itemID <= 1847)
                return true;
            if (itemID >= 1848 && itemID <= 1864)
                return true;
            if (itemID >= 1865 && itemID <= 1871)
                return true;
            if (itemID >= 1872 && itemID <= 1896)
                return true;
            if (itemID >= 1897 && itemID <= 1921)
                return true;
            if (itemID >= 1922 && itemID <= 1923)
                return true;
            if (itemID >= 1924 && itemID <= 1948)
                return true;
            if (itemID >= 1949 && itemID <= 1951)
                return true;
            if (itemID >= 1952 && itemID <= 1967)
                return true;
            if (itemID >= 1968 && itemID <= 1970)
                return true;
            if (itemID >= 1971 && itemID <= 1974)
                return true;
            if (itemID >= 1975 && itemID <= 1977)
                return true;
            if (itemID >= 1978 && itemID <= 1980)
                return true;
            if (itemID >= 1981 && itemID <= 1990)
                return true;
            if (itemID >= 1991 && itemID <= 1992)
                return true;
            if (itemID >= 2010 && itemID <= 2010)
                return true;
            if (itemID >= 2015 && itemID <= 2016)
                return true;
            if (itemID >= 2170 && itemID <= 2173)
                return true;
            if (itemID >= 2201 && itemID <= 2214)
                return true;
            if (itemID >= 2325 && itemID <= 2328)
                return true;

            return false;
        }

        private static bool IsStaticHardStoneFloor(int itemID)
        {
            if (itemID >= 1168 && itemID <= 1176)
                return true;

            if (itemID >= 1179 && itemID <= 1184)
                return true;

            if (itemID >= 1250 && itemID <= 1283)
                return true;

            if (itemID >= 1293 && itemID <= 1324)
                return true;

            if (itemID >= 1327 && itemID <= 1338)
                return true;

            if (itemID >= 11189 && itemID <= 11192)
                return true;

            if (itemID >= 11500 && itemID <= 11502)
                return true;

            return false;
        }

        private static bool IsStaticBushes(int itemID)
        {
            if (itemID >= 3157 && itemID <= 3163)
                return true;

            if (itemID >= 3203 && itemID <= 3214)
                return true;

            if (itemID >= 3219 && itemID <= 3220)
                return true;

            if (itemID >= 3223 && itemID <= 3224)
                return true;

            if (itemID >= 3231 && itemID <= 3239)
                return true;

            if (itemID >= 3255 && itemID <= 3265)
                return true;

            if (itemID >= 3332 && itemID <= 3333)
                return true;

            if (itemID >= 3391 && itemID <= 3392)
                return true;

            if (itemID >= 6802 && itemID <= 6806)
                return true;

            if (itemID >= 14539 && itemID <= 14563)
                return true;

            if (itemID >= 14572 && itemID <= 14602)
                return true;

            return false;
        }

        private static bool IsShallowWaterSurface(int itemID)
        {
            if (itemID >= 6045 && itemID <= 6066) // playsound 0x131, 0x132
                return true;

            return false;
        }

        private static bool IsDeepWaterSurface(int itemID)
        {
            if (itemID >= 6039 && itemID <= 6044) // playsound 0x133, 0x134
                return true;

            return false;
        }
    }
}
