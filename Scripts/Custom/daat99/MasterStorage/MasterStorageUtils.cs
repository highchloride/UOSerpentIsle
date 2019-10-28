/*
 created by:
     /\            888                   888     .d8888b.   .d8888b.  
____/_ \____       888                   888    d88P  Y88b d88P  Y88b 
\  ___\ \  /       888                   888    888    888 888    888 
 \/ /  \/ /    .d88888  8888b.   8888b.  888888 Y88b. d888 Y88b. d888 
 / /\__/_/\   d88" 888     "88b     "88b 888     "Y888P888  "Y888P888 
/__\ \_____\  888  888 .d888888 .d888888 888           888        888 
    \  /      Y88b 888 888  888 888  888 Y88b.  Y88b  d88P Y88b  d88P 
     \/        "Y88888 "Y888888 "Y888888  "Y888  "Y8888P"   "Y8888P"  
*/
#define RunUO2_2
#define USE_TOKENS

using System;
using Server;
using Server.Mobiles;
using Server.Items;
using System.Collections.Generic;
using Server.Commands;
using Server.Engines.Craft;

namespace daat99
{
    public static class MasterStorageUtils
    {
		//defaults types to loot
		//When editing this make sure you don't include types that have inheritance links
		public static Type[] DefaultBaseTypes = 
		{ 
			typeof(BaseReagent),		typeof(BaseWeapon), 		typeof(BaseArmor), 			typeof(BaseJewel),
			typeof(BaseContainer),		typeof(SpellScroll),		typeof(BaseInstrument), 	typeof(BasePotion),
			typeof(BaseTool),			typeof(BaseHarvestTool), 	typeof(ICommodity),			typeof(MapItem),
			typeof(BaseAddon),			typeof(BaseGranite)
		};
		
		//default items to loot
		//When editing this make sure you don't add items that inherits from one of the types in the DefaultBaseTypes list
		public static Type[] DefaultItemTypes = 
		{ 
			typeof(Amber), 			typeof(Amethyst), 	typeof(Citrine), 	typeof(Diamond), 
			typeof(Emerald), 		typeof(Ruby), 		typeof(Sapphire), 	typeof(StarSapphire), 
			typeof(Tourmaline), 	typeof(Board),		typeof(PowerScroll)
		};

		//This is the currency items types
		private static Dictionary<Type, Type> currencyTypes;
		private static Dictionary<Type, Type> CurrencyTypes
		{
			get
			{
				if (currencyTypes == null)
				{
					currencyTypes = new Dictionary<Type, Type>();
					currencyTypes.Add(typeof(Gold), typeof(Gold));
					currencyTypes.Add(typeof(BankCheck), typeof(Gold));
#if USE_TOKENS
					currencyTypes.Add(typeof(Daat99Tokens), typeof(Daat99Tokens));
					currencyTypes.Add(typeof(TokenCheck), typeof(Daat99Tokens));
#endif
				}
				return currencyTypes;
			}
		}

		public static bool IsCurrencyType( Type itemType )
		{
			return CurrencyTypes.ContainsKey(itemType);
		}

		public static Type GetCurrencyType(Type itemType)
		{
			if (CurrencyTypes.ContainsKey(itemType))
				return CurrencyTypes[itemType];
			return null;
		}
				
		private static Dictionary<Serial, MasterStorage> PlayersMasterStorageList = new Dictionary<Serial, MasterStorage>();

		private static void addToMasterStoragesList(PlayerMobile player, MasterStorage bag)
		{
			if (bag == null)
				return;
			if (!MasterStorageUtils.PlayersMasterStorageList.ContainsKey(player.Serial))
				MasterStorageUtils.PlayersMasterStorageList.Add(player.Serial, bag);
		}

		public static void Initialize()
		{
#if RunUO2_2
            CommandSystem.Register("Loot", AccessLevel.Player, new CommandEventHandler(Loot_OnCommand));
#else
            CommandHandlers.Register( "Loot", AccessLevel.Player, new CommandEventHandler( Loot_OnCommand ) );
#endif
		}
		
		public static void Loot_OnCommand( CommandEventArgs e )
		{
			PlayerMobile player = e.Mobile as PlayerMobile;
			if (  player == null || !CanPlayerLoot(player) )
				return;
			MasterStorage backpack = GetMasterStorage(player);
			if ( backpack == null )
			{
				player.SendMessage("You must have a Master Storage in your backpack!");
				return;
			}
			if ( backpack.IsOwner(player) )
				backpack.Loot( player );
		}

		///WARNING!!! may return null!!!
		public static MasterStorage GetMasterStorage(PlayerMobile player)
		{
			if (player == null || player.Backpack == null)
				return null;
			Serial serial = player.Serial;
			MasterStorage bag = null;
			if (MasterStorageUtils.PlayersMasterStorageList.ContainsKey(serial))
			{
				bag = MasterStorageUtils.PlayersMasterStorageList[serial];
				if (bag.IsChildOf(player.Backpack) && !bag.Deleted)
					return bag;
				else
					MasterStorageUtils.PlayersMasterStorageList.Remove(serial);
			}
			bag = player.Backpack.FindItemByType(typeof(MasterStorage), false) as MasterStorage;
			if (bag != null && bag.Deleted)
				bag = null;
			if (bag != null && bag.IsOwner(player))
			{
				MasterStorageUtils.PlayersMasterStorageList.Add(serial, bag);
				return bag;
			}
			else
				return null;
		}
		
		public static bool CanPlayerLoot(PlayerMobile player)
		{
			if ( player.AccessLevel > AccessLevel.Player )
				return true;
			if ( !player.Alive )
			{
				player.PlaySound( 1069 ); //hey
				player.SendMessage( "You are dead!" );
				return false;
			}
			if ( player.Blessed )
			{
				player.PlaySound( 1069 ); //hey
				player.SendMessage( "You can't loot while you are invulnerable!");
				return false;
			}
			foreach ( Mobile other in player.GetMobilesInRange( 2 ) )
			{
				if ( ! ( other is PlayerMobile ) )
					continue;
				if ( player != other && !other.Hidden && other.AccessLevel == AccessLevel.Player )
				{
					player.PlaySound(1069); //hey
					player.SendMessage("You are too close to another player to do that!");
					return false; //ignore self, staff and hidden
				}
			}
			return true;
		}
		
		public static bool GiveGoldToPlayer( PlayerMobile player, int amount )
		{
			return GiveGoldToPlayer(player, amount, true);
		}
		public static bool GiveGoldToPlayer( PlayerMobile player, int amount, bool informPlayer )
		{
			return GiveTypeToPlayer(player, typeof(Gold), amount, informPlayer);
		}

		public static bool TakeGoldFromPlayer(PlayerMobile player, int amount)
		{
			return TakeGoldFromPlayer(player, amount, true);
		}
		public static bool TakeGoldFromPlayer(PlayerMobile player, int amount, bool informPlayer)
		{
			return TakeTypeFromPlayer(player, typeof(Gold), amount, informPlayer);
		}


		public static bool GiveTypeToPlayer(PlayerMobile player, Type type, int amount)
		{
			return GiveTypeToPlayer(player, type, amount, true);
		}
		public static bool GiveTypeToPlayer(PlayerMobile player, Type type, int amount, bool informPlayer)
		{
			int amountLeft = amount;
			if (amount < 0)
				return false;
			MasterStorage backpack = GetMasterStorage(player);
			if (backpack == null)
			{
				if (informPlayer)
					player.SendMessage(1173, "Unable to find your Master Storage, please make sure it's in your backpack.");
				return false;
			}
			if ( !backpack.TryStoreItemType(type, amount) )
			{
				if (informPlayer)
					player.SendMessage(1173, "Unable to add " + amount + " " + type.Name + " to your Master Storage.");
				return false;
			}
			if (informPlayer)
				player.SendMessage(1173, "You added " + amount + " " + type.Name + " to your Master Storage.");
			return true;
		}

		public static bool TakeTypeFromPlayer(PlayerMobile player, Type type, int amount)
		{
			return TakeTypeFromPlayer(player, type, amount, true);
		}
		public static bool TakeTypeFromPlayer(PlayerMobile player, Type type, int amount, bool informPlayer)
		{
			if (amount < 0)
				return false;
			MasterStorage backpack = GetMasterStorage(player);
			if (backpack == null)
			{
				if (informPlayer)
					player.SendMessage(1173, "Unable to find your Master Storage, please make sure it's in your backpack.");
				return false;
			}
			List<Item> items = backpack.TryExtractType(type, amount);
			if (items.Count == 0)
			{
				if (informPlayer)
					player.SendMessage(1173, "You don't have enough " + type.Name + " in your Master Storage.");
				return false;
			}
			if (informPlayer)
				player.SendMessage(1173, amount + " " + type.Name + " were removed from your Master Storage.");
			return true;
		}

		public static bool GiveTokensToPlayer( PlayerMobile player, int amount )
		{
			return GiveTokensToPlayer(player, amount, true);
		}
		public static bool GiveTokensToPlayer( PlayerMobile player, int amount, bool informPlayer )
		{
#if USE_TOKENS
			return GiveTypeToPlayer(player, typeof(Daat99Tokens), amount, informPlayer);
#else
			return false;
#endif
		}

		public static bool TakeTokensFromPlayer(PlayerMobile player, int amount)
		{
			return TakeTokensFromPlayer(player, amount, false);
		}
		public static bool TakeTokensFromPlayer(PlayerMobile player, int amount, bool informPlayer)
		{
#if USE_TOKENS
			return TakeTypeFromPlayer(player, typeof(Daat99Tokens), amount, informPlayer);
#else
			return false;
#endif
		}
		public static bool DropItemInBagOrFeet(PlayerMobile player, MasterStorage backpack, List<Item> items)
		{
			foreach (Item item in items)
				if (!DropItemInBagOrFeet(player, backpack, item))
					return false;
			return true;
		}
		public static bool DropItemInBagOrFeet(PlayerMobile player, MasterStorage backpack, Item item)
		{
			if ( player == null || item == null )
				return false;
			if ( backpack == null )
				backpack = GetMasterStorage(player);
			if ( backpack != null && backpack.TryDropItem(player, item, false) )
				return true;
			if ( player.Backpack != null && player.Backpack.TryDropItem(player, item, false) )
				return true;
			
			Map map = player.Map;
            if (map == null)
                return false;

            List<Item> atFeet = new List<Item>();
            foreach (Item obj in player.GetItemsInRange(0))
                atFeet.Add(obj);
            for (int i = 0; i < atFeet.Count; ++i)
            {
                Item check = atFeet[i];

                if (check.StackWith(player, item, false))
                    break;
            }
            item.MoveToWorld(player.Location, map);
			return true;
		}

		public static bool CheckLoot(Corpse corpse, PlayerMobile player)
		{
			#if RunUO2_2
				return corpse.CheckLoot(player, null);
			#else
				return corpse.CheckLoot(player);
			#endif
		}

		public static bool ConsumeCurrency( PlayerMobile player, Type currencyType, int amount )
		{ return ConsumeCurrency(player, currencyType, amount, false); }
		public static bool ConsumeCurrency( PlayerMobile player, Type currencyType, int amount, bool informPlayer )
		{
			if ( amount < 0 )
			{
				if ( player != null && informPlayer )
					player.SendMessage("You can't consume negative amounts.");
				return false;
			}
			return ConsumeCurrency(player, currencyType, (UInt64)amount, informPlayer);
		}
		public static bool ConsumeCurrency( PlayerMobile player, Type currencyType, UInt64 amount )
		{ return ConsumeCurrency( player, currencyType, amount, false ); }
		public static bool ConsumeCurrency( PlayerMobile player, Type currencyType, UInt64 amount, bool informPlayer )
		{
			if ( player == null )
				return false;
			if ( amount == 0 )
			{
				if ( informPlayer )
					player.SendMessage("You don't need to consume anything.");
				return true;
			}
			MasterStorage backpack = GetMasterStorage(player);
			if ( backpack == null )
			{
				if ( informPlayer )
					player.SendMessage("You don't have a Master Storage backpack on you.");
				return false;
			}
			return backpack.ConsumeCurrency(player, currencyType, amount, informPlayer);
		}

        // Vii added
        public static Item CreateItemFromType(Type type, ItemInformation info)
        {
            try
            {
                Item item = Activator.CreateInstance(type) as Item;

                if (item == null)
                    return null;

                if (item is BaseInstrument)
                {
                    ((BaseInstrument)item).Name = info.Name;
                    ((BaseInstrument)item).Slayer = info.Slayer;
                    ((BaseInstrument)item).Slayer2 = info.Slayer2;
                    ((BaseInstrument)item).Quality = info.Quality;
                    ((BaseInstrument)item).Crafter = info.Crafter;
                    ((BaseInstrument)item).UsesRemaining = info.UsesRemaining;
                }

                return item;
            }
            catch { }

            return null;
        }

		public static List<Item> CreateItemsFromType(Type type, int amount)
		{
			List<Item> items = new List<Item>();
			try
			{
				Item item = Activator.CreateInstance(type) as Item;

				if (item == null)
					return null;
				while (amount > 0)
				{
					item = Activator.CreateInstance(type) as Item;
					if (item == null)
						return null;
					if (item is SackFlour)
					{
						int add = Math.Min(amount, 20);
						//((SackFlour)item).Quantity = add; //UOSI - Quantity has been removed from SackFlour per Oct 2019 merge
						amount -= add;
					}
					else if (item is IUsesRemaining)
					{
						((IUsesRemaining)item).UsesRemaining = amount;
						amount = 0;
					}
					else if (item.Stackable)
					{
						int add = Math.Min(amount, 60000);
						item.Amount = add;
						amount -= add;
					}
					else
						--amount;
					items.Add(item);
				}
			}
			catch { }
			if (amount > 0) //we failed!!!
			{
				foreach (Item item in items.ToArray())
					item.Delete();
				return null;
			}
			return items;
		}

		public static void MoveItemsOnDeath(PlayerMobile player, Container corpse)
		{
			if ( player == null || corpse == null )
				return;
			MasterStorage backpack = GetMasterStorage(player);
			if ( backpack != null )
				backpack.MoveItemsOnDeath(corpse);
		}

		public static ulong GetPlayersStorageItemCount(PlayerMobile player, Type type)
		{
			MasterStorage backpack = GetMasterStorage(player);
			if (backpack != null)
				return backpack.GetStoredItemAmount(type);
			return 0;
		}
		public static bool ConsumePlayersStorageItem(PlayerMobile player, Type type, int amount)
		{
			MasterStorage backpack = GetMasterStorage(player);
			if (backpack != null)
				return backpack.TryConsume(type, amount);
			return false;
		}
		public static bool ConsumePlayersStorageItems(PlayerMobile player, Type[] types, int[] amounts)
		{
			MasterStorage backpack = GetMasterStorage(player);
			if (backpack != null)
				return backpack.TryConsume(types, amounts);
			return false;
		}
	}
}
