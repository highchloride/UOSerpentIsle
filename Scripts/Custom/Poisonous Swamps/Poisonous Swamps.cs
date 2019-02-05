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
            }

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
	}
}
