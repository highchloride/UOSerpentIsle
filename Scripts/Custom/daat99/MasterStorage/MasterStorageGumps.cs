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
#define USE_TOKENS
using System;
using Server;
using Server.Items;
using Server.Gumps;
using Server.Network;
using Server.Mobiles;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace daat99
{
	public class MasterStorageStorageGump : Gump
	{
		private static readonly BaseStorage NoFilter = ItemStorage.Storage;
		public class StoredItemGumpObject
		{
			public ulong Amount;
			public Type ItemType;
			public StoredItemGumpObject(Type type, ulong amount)
			{
				ItemType = type;
				Amount = amount;
			}

			public string GetVisualName()
			{
				StringBuilder sb = new StringBuilder();
				sb.Append(Amount.ToString());
				sb.Append(":  ");
				sb.Append(Regex.Replace(ItemType.Name, "([A-Z])", " $1").Trim());
				return sb.ToString();
			}
		}
		public static void SendGump(Mobile from, MasterStorage backpack) { SendGump(from, backpack, -1); }
		public static void SendGump(Mobile from, MasterStorage backpack, int page)  { SendGump(from, backpack, page, null); }
		public static void SendGump(Mobile from, MasterStorage backpack, int page, BaseStorage filter)
		{
			CloseGump(from);
			if (from == null )
				return;
			if (backpack == null || backpack.Deleted || !backpack.IsOwner(from as PlayerMobile))
			{
				from.SendMessage(1173, "You don't have your Master Storage in your backpack.");
				return;
			}
			from.SendGump(new MasterStorageStorageGump(from as PlayerMobile, backpack, page, filter));
		}
		public static void CloseGump(Mobile from)
		{
			if (from != null && from.HasGump(typeof(MasterStorageStorageGump)))
				from.CloseGump(typeof(MasterStorageStorageGump));
		}

		enum BUTTONS_ENUM
		{
			OK = 0, WEBSITE, REFILL, AMOUNT_TEXT, RESET_FILTER, PREVIOUS_PAGE, NEXT_PAGE, FILTER_START = 100, WITHDRAW_ITEM_START = 200
		};

		PlayerMobile player;
		MasterStorage backpack;
		int page;
		BaseStorage filter;
		StoredItemGumpObject[] items;
		public MasterStorageStorageGump(PlayerMobile player, MasterStorage backpack) : this(player, backpack, -1, null) { }
		public MasterStorageStorageGump(PlayerMobile player, MasterStorage backpack, int page, BaseStorage filter) : base(20, 20)
		{
			this.backpack = backpack;
			this.page = page;
			this.player = player;
			this.filter = filter;
			items = null;
			if (backpack != null)
			{
				if (page == -1)
					page = backpack.StoragePage;
				if (filter == null)
					filter = backpack.StorageFilter;
			}
			init();
			if (player == null || backpack == null)
				return;
			drawFilters();
			drawItems();
			addPageControls();
		}


		private void init()
		{
			AddPage(0);
			AddBackground(0, 0, 660, 500, 3500); //white  500 was 480

			if (filter == null)
				filter = ItemStorage.Storage;
			AddHtml(250, 15, 450, 20, "<basefont color=#ff0000>Daat99's Master Storage </basefont> Showing: " + filter.Name, false, false);

			AddLabel(15, 15, 2, @"====================");
			AddLabel(58, 30, 2, @" /\");
			AddLabel(19, 40, 2, @" -----/---\-----");
			AddLabel(23, 50, 2, @" \ /     \ /");
			AddLabel(31, 60, 2, @" /       \");
			AddLabel(33, 60, 2, @" \       /");
			AddLabel(22, 70, 2, @" / \     / \");
			AddLabel(20, 80, 2, @" -----\---/-----");
			AddLabel(60, 90, 2, @" \/");
			AddLabel(15, 100, 2, @"====================");
			
			AddLabel(15, 120, 20, "Extract Amount:");
			AddTextEntry(25, 140, 85, 20, 32, (int)BUTTONS_ENUM.AMOUNT_TEXT, 100.ToString());
		}
		private void drawFilters()
		{
			int filterIndex = 0;
			int filterHeight = 20;
			int topBuffer = 190;

			AddButton(15, topBuffer, 4033, 4033, (int)BUTTONS_ENUM.REFILL, GumpButtonType.Reply, 1);
			AddLabel(35, topBuffer-2, 1263, "Refill");
			topBuffer += (int)(filterHeight*1.5);

			AddButton(15, topBuffer, 4033, 4033, (int)BUTTONS_ENUM.RESET_FILTER, GumpButtonType.Reply, 0);
			AddLabel(35, topBuffer - 2, 70, @"Clear filter");
			topBuffer += filterHeight;

			foreach (BaseStorage storage in backpack.StorageList)
			{
				AddButton(15, topBuffer + filterIndex * filterHeight, 4033, 4033, (int)BUTTONS_ENUM.FILTER_START + filterIndex, GumpButtonType.Reply, 0);
				AddLabel(35, topBuffer - 2 + filterIndex * filterHeight, 70, storage.Name);
				++filterIndex;
			}
		}

		private void drawItems()
		{
			int leftBuffer = 170;
			int topBuffer = 40;
			int itemsPerRow = 2;
			int itemsPerColumn = 20;
			int itemHeight = 20;
			int itemWidth = 240;
			items = backpack.GetStoredItemsForPage(itemsPerRow * itemsPerColumn, page, filter);

			for (int itemIndex = 0; itemIndex < items.Length && items[itemIndex] != null; ++itemIndex)
			{
				if ((int)(itemIndex / itemsPerColumn) >= itemsPerRow)
					break;
				int x = (int)(itemIndex / itemsPerColumn)*itemWidth + leftBuffer;
				int y = (itemIndex % itemsPerColumn)*itemHeight + topBuffer;
				AddButton( x, y, 2511, 2510, (int)BUTTONS_ENUM.WITHDRAW_ITEM_START + itemIndex, GumpButtonType.Reply, 0 );
				AddHtml(x + 20, y, itemWidth - 20, itemHeight, items[itemIndex].GetVisualName(), false, false);
			}
		}

		private void addPageControls()
		{
			if (page > 0)
				AddButton(15, 160, 4014, 4016, (int)BUTTONS_ENUM.PREVIOUS_PAGE, GumpButtonType.Reply, 0);

			AddButton(45, 159, 5526, 5527, (int)BUTTONS_ENUM.WEBSITE, GumpButtonType.Reply, 0);

			if (items[items.Length - 1] != null)
				AddButton(110, 160, 4005, 4007, (int)BUTTONS_ENUM.NEXT_PAGE, GumpButtonType.Reply, 0);
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			if (player == null)
				return;
			if (info.ButtonID == (int)BUTTONS_ENUM.OK)
				CloseGump(player);
			else if (info.ButtonID == (int)BUTTONS_ENUM.WEBSITE)
				player.LaunchBrowser("http://daat99.home.dyndns.org/index.html");
			else if (info.ButtonID == (int)BUTTONS_ENUM.RESET_FILTER)
				SendGump(player, backpack, page, NoFilter);
			else if (info.ButtonID == (int)BUTTONS_ENUM.PREVIOUS_PAGE)
				SendGump(player, backpack, page - 1, filter);
			else if (info.ButtonID == (int)BUTTONS_ENUM.NEXT_PAGE)
				SendGump(player, backpack, page + 1, filter);
			//extracting items
			else if (info.ButtonID >= (int)BUTTONS_ENUM.WITHDRAW_ITEM_START && info.ButtonID < (int)BUTTONS_ENUM.WITHDRAW_ITEM_START + items.Length)
			{
				//calculate extract amount
				TextRelay trText = info.GetTextEntry((int)BUTTONS_ENUM.AMOUNT_TEXT);
				int amount = 0;
				if (trText != null)
				{
					try { amount = Convert.ToInt32(trText.Text, 10); }
					catch { player.SendMessage(1173, "Unable to parse the extract amount into a number."); }
				}
				int itemIndex = info.ButtonID - (int)BUTTONS_ENUM.WITHDRAW_ITEM_START;
				if (amount < 0)
					player.SendMessage(1173, "You must enter a positive number to extract.");
				else if (itemIndex < 0 || itemIndex > items.Length)
					player.SendMessage(1173, "Unrecognized item.");
				else
				{
					StoredItemGumpObject storedItem = items[itemIndex];
					ulong available = storedItem.Amount;
					if ( available <= 0 )
					{
						player.SendMessage(1173, "You don't have that items.");
						return;
					}
					if (available < (ulong)amount)
					{
						amount = (int)available;
						player.SendMessage(1173, "You don't have enough of that item, extracting only " + amount + " instead.");
					}
					List<Item> extractedItems = backpack.TryExtractType(storedItem.ItemType, amount);
					if (extractedItems == null || extractedItems.Count <= 0)
						player.SendMessage(1173, "Unable to extract the items.");
					else
					{
						foreach (Item item in extractedItems)
							MasterStorageUtils.DropItemInBagOrFeet(player, backpack, item);
						player.SendMessage(1173, "You successfully extracted " + amount + " items.");
						SendGump(player, backpack, page, filter);
					}
				}
			}
			//changing filter
			else if (info.ButtonID >= (int)BUTTONS_ENUM.FILTER_START && info.ButtonID < (int)BUTTONS_ENUM.WITHDRAW_ITEM_START)
			{
				int filterIndex = info.ButtonID - (int)BUTTONS_ENUM.FILTER_START;
				if (filterIndex < 0 || filterIndex > backpack.StorageList.Count)
					player.SendMessage(1173, "Unrecognized filter.");
				else
					SendGump(player, backpack, page, backpack.StorageList[filterIndex]);
			}
			else if (info.ButtonID == (int)BUTTONS_ENUM.REFILL)
			{
				backpack.RefillStorage(player);
				SendGump(player, backpack, page, filter);
			}
		}
	}

    public class MasterStorageSetupGump : Gump
    {
		public static void SendGump(Mobile from, MasterStorage backpack, int page)
		{
			CloseGump(from);
			if (from == null ||  backpack == null || backpack.Deleted || !backpack.IsOwner(from as PlayerMobile))
				return;
			from.SendGump(new MasterStorageSetupGump(backpack, page));
		}
		public static void CloseGump(Mobile from)
		{
			if (from != null && from.HasGump(typeof(MasterStorageSetupGump)))
				from.CloseGump(typeof(MasterStorageSetupGump));
		}
		private static readonly int LINE_HEIGHT = 25;
		private static readonly int ITEMS_PER_PAGE = 10;

		int lineIndex;
		MasterStorage backpack;
		int page;
		Type[] types;
		enum BUTTONS_ENUM 
		{ 
			OK = 0, 			PREVIOUS_PAGE, 		NEXT_PAGE, 				ADD_ITEM, 
			ADD_TYPE, 			LOOT_SETTINGS, 		DELETE_ALL_CORPSES, 	RESTORE_DEFAULTS,
			SWITCH_ACTIVE_LIST,
			REMOVE_TYPE_START=100 
		};

		public MasterStorageSetupGump(MasterStorage backpack) : this( backpack, 0) { }
		public MasterStorageSetupGump(MasterStorage backpack, int page) : base(20, 20)
        {
			this.backpack = backpack;
			this.page = page;
			int typesCount = backpack.LootableTypesCount;
			int lastPage = typesCount / ITEMS_PER_PAGE + (typesCount % ITEMS_PER_PAGE  == 0 ? 0 : 1)-1;
			if ( this.page == -1 || this.page > lastPage ) //last page
				this.page = lastPage;
			if ( this.page < 0 )
				this.page = 0;
			
			initialize(); 
			addTop(typesCount);
			addAddSettingsSection();
			addTypesPage();
			addPageControls();
        }

		private void initialize()
		{
			Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;
			lineIndex = 1;
            AddPage(0);
			types = backpack.GetLootableTypesForPage(page);
		}
		
		private void addTop( int typesCount )
		{
			int itemsCount = typesCount - page * ITEMS_PER_PAGE;
			if (itemsCount > ITEMS_PER_PAGE)
				itemsCount = ITEMS_PER_PAGE;
			AddBackground(0, 0, 220, (11+itemsCount)*LINE_HEIGHT, 5120);
		}
		private void addTypesPage()
		{
			AddImage( 25, (lineIndex*LINE_HEIGHT), 57);
            AddLabel( 70, (lineIndex*LINE_HEIGHT), 48, @"Loot Selector");
            AddImage(175, (lineIndex*LINE_HEIGHT), 59);
			++lineIndex;
			for ( int i = 0; i < types.Length; ++i )
			{
				if ( types[i] == null )
					return;
				AddButton(25, (lineIndex*LINE_HEIGHT), 5409, 5408, (int)BUTTONS_ENUM.REMOVE_TYPE_START+i, GumpButtonType.Reply, 0);
				string typeName = types[i].ToString();
				int dotIndex = typeName.LastIndexOf(".");
				if ( dotIndex >= 0 )
					typeName = typeName.Substring(dotIndex+1);
				AddLabel(104, (lineIndex*LINE_HEIGHT), 60, typeName);
				++lineIndex;
			}
		}
		private void addAddSettingsSection()
		{
		    AddImage( 23, (lineIndex*LINE_HEIGHT), 57);
            AddLabel( 70, (lineIndex*LINE_HEIGHT), 48, "Add More Items");
            AddImage(175, (lineIndex*LINE_HEIGHT), 59);
			++lineIndex;
            AddButton(23, (lineIndex*LINE_HEIGHT), 1210, 1209, (int)BUTTONS_ENUM.ADD_ITEM, GumpButtonType.Reply, 0);
            AddLabel( 50, (lineIndex*LINE_HEIGHT), 60, "Add Single Item");
			++lineIndex;
            AddButton(23, (lineIndex*LINE_HEIGHT), 1210, 1209, (int)BUTTONS_ENUM.ADD_TYPE, GumpButtonType.Reply, 0);
            AddLabel( 50, (lineIndex*LINE_HEIGHT), 60, "Add Items Common Type");
			++lineIndex;
			AddButton(23, (lineIndex*LINE_HEIGHT), 1210, 1209, (int)BUTTONS_ENUM.LOOT_SETTINGS, GumpButtonType.Reply, 0);
			AddLabel( 50, (lineIndex*LINE_HEIGHT), 60, "Looting: "  + backpack.LootSettingsString);
			++lineIndex;
			AddButton(23, (lineIndex*LINE_HEIGHT), 1210, 1209, (int)BUTTONS_ENUM.DELETE_ALL_CORPSES, GumpButtonType.Reply, 0);
			AddLabel( 50, (lineIndex*LINE_HEIGHT), 60, "Deleting: "  + (backpack.DeleteAllCorpses? "All Corpses": "Empty Corpses"));
			++lineIndex;
			AddButton(23, (lineIndex*LINE_HEIGHT), 1210, 1209, (int)BUTTONS_ENUM.SWITCH_ACTIVE_LIST, GumpButtonType.Reply, 0);
			AddLabel( 50, (lineIndex*LINE_HEIGHT), 60, "Active List: "  + backpack.ActiveListName );
			++lineIndex;
			AddButton(80, (lineIndex*LINE_HEIGHT), 0xF5, 0xF4, (int)BUTTONS_ENUM.RESTORE_DEFAULTS, GumpButtonType.Reply, 0);
			++lineIndex;
		}
		private void addPageControls()
		{
			if ( page > 0 )
				AddButton( 23, (lineIndex*LINE_HEIGHT), 4014, 4016, (int)BUTTONS_ENUM.PREVIOUS_PAGE, GumpButtonType.Reply, 0 );
		
			AddLabel( 90, (lineIndex*LINE_HEIGHT), 48, "Page: " + (page+1));

			if (backpack.LootableTypesCount > page * ITEMS_PER_PAGE + ITEMS_PER_PAGE)
				AddButton( 175, (lineIndex*LINE_HEIGHT), 4005, 4007, (int)BUTTONS_ENUM.NEXT_PAGE, GumpButtonType.Reply, 0 );
			++lineIndex;
		}
        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
			CloseGump(from);
            PlayerMobile player = from as PlayerMobile;
			if ( player == null )
				return;
			switch (info.ButtonID)
            {
				case (int)BUTTONS_ENUM.OK:
					//don't send new gump
					break;
				case (int)BUTTONS_ENUM.ADD_ITEM:
					player.Target = new MasterStorageAddTypeTarget(backpack, player, MasterStorageAddTypeTarget.SELECTION.ITEM_SELECTION);
					break;
				case (int)BUTTONS_ENUM.ADD_TYPE:
					player.Target = new MasterStorageAddTypeTarget(backpack, player, MasterStorageAddTypeTarget.SELECTION.TYPE_SELECTION);
					break;
				case (int)BUTTONS_ENUM.PREVIOUS_PAGE:
					SendGump(from, backpack, page-1);
					break;
				case (int)BUTTONS_ENUM.NEXT_PAGE:
					SendGump(from, backpack, page+1);
					break;
				case (int)BUTTONS_ENUM.LOOT_SETTINGS:
					backpack.NextLootSettings();
					SendGump(from, backpack, page);
					break;
				case (int)BUTTONS_ENUM.DELETE_ALL_CORPSES:
					backpack.DeleteAllCorpses = !backpack.DeleteAllCorpses;
					SendGump(from, backpack, page);
					break;
				case (int)BUTTONS_ENUM.RESTORE_DEFAULTS:
					backpack.RestoreDefaultList();
					SendGump(from, backpack, page);
					break;
				case (int)BUTTONS_ENUM.SWITCH_ACTIVE_LIST:
					backpack.SwitchActiveList();
					SendGump(from, backpack, page);
					break;
				default:
					if ( info.ButtonID < (int)BUTTONS_ENUM.REMOVE_TYPE_START 
						|| info.ButtonID >= (int)BUTTONS_ENUM.REMOVE_TYPE_START+types.Length )
						return;
					backpack.RemoveLootableType(types[info.ButtonID-(int)BUTTONS_ENUM.REMOVE_TYPE_START]);
					SendGump(from, backpack, page);
					break;
            }
        }
    }

	public class MasterStorageLedgerGump : Gump
    {
		public static void SendGump(Mobile from, MasterStorage backpack)
		{
			CloseGump(from);
			if ( from == null || backpack == null || backpack.Deleted || (!backpack.GoldLedger && !backpack.TokenLedger) || !backpack.IsOwner(from as PlayerMobile))
				return;
			from.SendGump(new MasterStorageLedgerGump(backpack));
		}
		public static void CloseGump(Mobile from)
		{
			if (from != null && from.HasGump(typeof(MasterStorageLedgerGump)))
				from.CloseGump(typeof(MasterStorageLedgerGump));
		}
		
		MasterStorage backpack;
		enum BUTTONS { OK = 0, AMOUNT, ADD, FILL, TOKENS, TOKENS_CHECK, GOLD, GOLD_CHECK };

		public MasterStorageLedgerGump(MasterStorage backpack)
			: base(50, 50)
		{
			this.backpack  = backpack;
			this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			bool gold = backpack.GoldLedger;
			bool token = backpack.TokenLedger;
			int totalLines = 7 + (gold?2:0) + (token?2:0);
			int offset = 0;
			this.AddBackground(0, 0, 350, 75+25*totalLines, 5170);
			
			this.AddLabel(120, 25, 69, "Account information");
			
			if ( gold )
			{
				this.AddLabel(25, 50, 69, "You have " + backpack.GoldAmount + " gold.");
				offset += 25;
			}
			if ( token )
			{
				this.AddLabel(25, 50+offset, 88, "You have " + backpack.TokensAmount + " tokens.");
				offset += 25;
			}
			
			this.AddLabel(  25, 50+offset, 32, "Add Currency:");
			this.AddButton(180, 50+offset, 2443, 2444, (int)BUTTONS.ADD, GumpButtonType.Reply, 0);
			this.AddLabel( 190, 50+offset, 0, "Add Pile");
			this.AddButton(255, 50+offset, 2443, 2444, (int)BUTTONS.FILL, GumpButtonType.Reply, 0);
			this.AddLabel( 265, 50+offset, 0, "Add All");

			this.AddLabel(25, 98+offset, 0, @"How much to withdraw?");
			this.AddBackground(170, 85+offset, 150, 50, 9270);
			this.AddTextEntry(180, 98+offset, 130, 25, 39, (int)BUTTONS.AMOUNT, "");
			
			if ( gold )
			{
				this.AddButton( 25, 145+offset, 4027, 4028, (int)BUTTONS.GOLD, GumpButtonType.Reply, 0);
				this.AddLabel(  65, 145+offset, 69, "Gold Pile");
				this.AddButton(195, 145+offset, 4012, 4013, (int)BUTTONS.GOLD_CHECK, GumpButtonType.Reply, 0);
				this.AddLabel( 235, 145+offset, 69, "Gold Check");
				offset += 25;
			}
			if ( token )
			{
				this.AddButton( 25, 145+offset, 4027, 4028, (int)BUTTONS.TOKENS, GumpButtonType.Reply, 0);
				this.AddLabel(  65, 145+offset, 69, "Tokens Pile");
				this.AddButton(195, 145+offset, 4012, 4013, (int)BUTTONS.TOKENS_CHECK, GumpButtonType.Reply, 0);
				this.AddLabel( 235, 145+offset, 69, "Tokens Check");
				offset += 25;
			}
			
			this.AddImage(25, 150+offset, 7012);
			this.AddImage(250, 150+offset, 7012);
			this.AddLabel(150, 160+offset, 38, "Daat99's");
			this.AddLabel(130, 185+offset, 38, "Master Storage");
		}
		public override void OnResponse( NetState state, RelayInfo info )
		{
			Mobile from = state.Mobile;
			if ( info.ButtonID == (int)BUTTONS.OK || backpack.Deleted || !(from is PlayerMobile) )
			{
				CloseGump(from);
				return;
			}
			if ( info.ButtonID == (int)BUTTONS.ADD )
			{
				from.Target = new MasterStorageAddCurrencyTarget(from, backpack);
				return;
			}
			else if ( info.ButtonID == (int)BUTTONS.FILL )
				backpack.AddCurrencyFromBackpack(from as PlayerMobile);
			else
			{
				TextRelay amountRelay = info.GetTextEntry( (int)BUTTONS.AMOUNT );
				if ( amountRelay != null )
				{
					ulong amount = 0;
					try
					{
						int iAmount = Convert.ToInt32(amountRelay.Text, 10);
						if ( iAmount > 0 )
							amount = (ulong)iAmount;
					}
					catch { }
					if ( amount > 0 )
					{
						if ( info.ButtonID == (int)BUTTONS.GOLD && amount <= 60000 && amount <= backpack.GoldAmount )
						{
							backpack.GoldAmount -= amount;
							from.AddToBackpack( new Gold((int)amount) );
							from.SendMessage(1173, "You extracted {0} gold from your Master Storage Backpack.", amount);
						}
						else if ( info.ButtonID == (int)BUTTONS.GOLD_CHECK && amount <= 1000000 && amount <= backpack.GoldAmount )
						{
							backpack.GoldAmount -= amount;
							from.AddToBackpack( new BankCheck((int)amount) );
							from.SendMessage(1173, "You extracted {0} gold from your Master Storage Backpack.", amount);
						}
#if USE_TOKENS
						else if ( info.ButtonID == (int)BUTTONS.TOKENS && amount <= 60000 && amount <= backpack.TokensAmount )
						{
							backpack.TokensAmount -= amount;
							from.AddToBackpack( new Daat99Tokens((int)amount) );
							from.SendMessage(1173, "You extracted {0} tokens from your Master Storage Backpack.", amount);
						}
						else if ( info.ButtonID == (int)BUTTONS.TOKENS_CHECK && amount <= 1000000 && amount <= backpack.TokensAmount )
						{
							backpack.TokensAmount -= amount;
							from.AddToBackpack( new TokenCheck((int)amount) );
							from.SendMessage(1173, "You extracted {0} tokens from your Master Storage Backpack.", amount);
						}
#endif
						else
							from.SendMessage(1173, "You can't extract that.");
					}
				}
			}
			SendGump(from, backpack);
		}
    }	
}