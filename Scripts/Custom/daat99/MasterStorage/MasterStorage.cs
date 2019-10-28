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
//comment the USE_TOKENS line if you don't have daat99 tokens system (1.0) or daat99 OWLTR system
#define USE_TOKENS
//comment the USE_OWLTR3 line if you don't have OWLTR 2.0+
#define USE_OWLTR3

using Server;
using Server.Mobiles;
using Server.Items;
using System;
using Server.ContextMenus;
using System.Collections.Generic;
using System.Collections;
using daat99;
namespace Daat99MasterLooterSystem
{
	public sealed class MasterLooterBackpack : MasterStorage
    {
		public MasterLooterBackpack(Serial serial)
			: base(serial)
		{
		}
		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
		}
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
		}
    }
}
namespace daat99
{
    public class MasterStorage : Backpack
    {
		//Change this to set the default loot settings for your players
		public void RestoreDefaultSettings()
		{
			RestoreDefaultList();
			lootSettings = LootSettingsEnum.From_List; //players loot items from their list only
			DeleteAllCorpses = false; //deletes all the corpses or just empty ones
#if USE_OWLTR3
			LootType = OWLTROptionsManager.IsEnabled(OWLTROptionsManager.OPTIONS_ENUM.BLESSED_STORAGE) ? LootType.Blessed : Server.LootType.Regular;
#else
            LootType = LootType.Blessed;
#endif
			GoldLedger = true; //enables the gold ledger
			TokenLedger = true; //enables the token ledger
			KeepItemsOnDeath = false;
		}
		
		public void RestoreDefaultList()
		{
			activeBaseTypes = new List<Type>(MasterStorageUtils.DefaultBaseTypes); //see and/or change list in Utils file
			activeLootTypes = new List<Type>(MasterStorageUtils.DefaultItemTypes); //see and/or change list in Utils file
		}

		public enum LootSettingsEnum { Everything, From_List, Currency_Only };
		private LootSettingsEnum lootsettings = LootSettingsEnum.From_List;
		public LootSettingsEnum lootSettings { get { return lootsettings; } set { lootsettings = value; InvalidateProperties(); } }

		public string LootSettingsString 
		{ 
			get 
			{
				return lootSettings.ToString().Replace("_", " ");
			} 
		}
		
		private static readonly string DefaultLooterName = "Master Storage";

		private PlayerMobile owner;
		[CommandProperty(AccessLevel.GameMaster)]
		public PlayerMobile Owner { get { return owner; } set { owner = value; InvalidateProperties(); } }
		
		private bool goldLedger;
		[CommandProperty(AccessLevel.GameMaster)]
		public bool GoldLedger 
		{ 
			get 
			{
#if USE_OWLTR3
				return goldLedger && OWLTROptionsManager.Manager.Options[OWLTROptionsManager.OPTIONS_ENUM.GOLD_STORAGE].Setting;
#else
				return goldLedger;
#endif
			} 
			set 
			{ 
				goldLedger = value; 
				InvalidateProperties(); 
			} 
		}
		
		private bool tokenLedger;
		[CommandProperty(AccessLevel.GameMaster)]
		public bool TokenLedger 
		{ 
			get 
			{ 
#if USE_OWLTR3
				return tokenLedger && OWLTROptionsManager.Manager.Options[OWLTROptionsManager.OPTIONS_ENUM.TOKEN_STORAGE].Setting;
#elif USE_TOKENS
				return tokenLedger; 
#else
				return false;
#endif
			} 
			set 
			{ 
				tokenLedger = value; InvalidateProperties(); 
			} 
		}
		
		private bool deleteAllCorpses;
		[CommandProperty(AccessLevel.GameMaster)]
		public bool DeleteAllCorpses { get { return deleteAllCorpses; } set { deleteAllCorpses = value; InvalidateProperties(); } }

		private ulong goldAmount;
		[CommandProperty(AccessLevel.GameMaster)]
		public ulong GoldAmount
		{
			get { return goldAmount; }
			set 
			{
				if (value > ulong.MaxValue || value < ulong.MinValue)
					return;
				goldAmount = value;
				InvalidateProperties();
			}
		}

		private ulong tokensAmount;
		[CommandProperty(AccessLevel.GameMaster)]
		public ulong TokensAmount
		{
			get { return tokensAmount; }
			set
			{
				if (value > ulong.MaxValue || value < ulong.MinValue)
					return;
				tokensAmount = value;
				InvalidateProperties();
			}
		}

		private List<Type> baseTypesA, baseTypesB;
		private List<Type> lootTypesA, lootTypesB;
		private List<Type> activeBaseTypes 
		{
			get
			{
				return (activeListA ? baseTypesA : baseTypesB);
			}
			set 
			{
				if ( activeListA )
					baseTypesA = value;
				else
					baseTypesB = value;
			}
		}
		private List<Type> activeLootTypes
		{
			get
			{
				return (activeListA ? lootTypesA : lootTypesB);
			}
			set 
			{
				if ( activeListA )
					lootTypesA = value;
				else
					lootTypesB = value;
			}
		}

		private bool keepItemsOnDeath;
		[CommandProperty(AccessLevel.GameMaster)]
		public bool KeepItemsOnDeath 
		{ 
			get 
			{ 
#if USE_OWLTR3
				return keepItemsOnDeath && OWLTROptionsManager.Manager.Options[OWLTROptionsManager.OPTIONS_ENUM.STORAGE_KEEP_ITEMS_DEATH].Setting;
#else
				return keepItemsOnDeath; 
#endif
			} 
			set 
			{ 
				keepItemsOnDeath = value; 
				InvalidateProperties();  
			} 
		}
		
		private bool activeListA;
		public string ActiveListName { get { return ( activeListA ? "Primary" : "Secondary"); } }
		public void SwitchActiveList()
		{
			activeListA = !activeListA; 
			InvalidateProperties();
		}

		public int LootableTypesCount 
		{ 
			get 
			{ 
				return (activeBaseTypes == null ? 0 : activeBaseTypes.Count) + (activeLootTypes == null ? 0 : activeLootTypes.Count);
			}
		}

		[Constructable]
		public MasterStorage()
			: base()
		{
			Weight = 0.0;
			Hue = 1169;
			ItemID = 0x9b2;
			Name = DefaultLooterName;
			Owner = null;

			//resets list B to default
			activeListA = false;
			RestoreDefaultList();
			
			//resets list A and loot settings to default
			SwitchActiveList();
			RestoreDefaultSettings();
			StoredItems = new Dictionary<Type, ulong>();
			StorageList = new List<BaseStorage>();
		}

		public MasterStorage( Serial serial ) : base( serial ) { }

		public override void GetProperties(ObjectPropertyList list)
        {
			base.GetProperties(list);
			//if (!KeepItemsOnDeath)
			//    list.Add("<basefont color=#FF0000><center>Dropping Items on Death");
			//else
			//    list.Add("<basefont color=#00FF00><center>Keeping Items on Death");
			list.Add(1060659, "Gold\t" + (GoldLedger?GoldAmount.ToString():"Inactive"));
			list.Add(1060660, "Tokens\t" + (TokenLedger?TokensAmount.ToString():"Inactive"));
			list.Add(1060661, "Looting\t" + LootSettingsString); //value: ~1_val~
			list.Add(1060662, "Deleting\t" + (DeleteAllCorpses?"All Corpses":"Empty Corpses")); //value: ~1_val~
			list.Add(1060658, "<center>On Death\t<basefont color=#" + (KeepItemsOnDeath ? "00ff00>Keeping Items" : "ff0000>Dropping Items") + "</basefont></center>"); //value: ~1_val~
		}
		
		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list )
		{
			base.GetContextMenuEntries( from, list );
			PlayerMobile player = from as PlayerMobile;
			if ( !IsOwner(player) )
				return;
			if (StorageList.Count > 0)
				list.Add(new MasterStorageStorageContextMenu(player, this));
			if (GoldLedger || TokenLedger)
				list.Add( new MasterStorageLedgerContextMenu( player, this ) );
			list.Add( new MasterStorageSetupContextMenu( player, this ) );
			if ( GoldLedger || TokenLedger )
				list.Add( new MasterStorageFillContextMenu( player, this ) );
				
		}
		
		public void Loot( PlayerMobile player )
		{
			if ( !IsOwner(player) )
				return;
			List<Item> items = new List<Item>();
			List<Corpse> corpses = new List<Corpse>();

			foreach ( Item item in player.GetItemsInRange(3) )
			{
				if ( item is Corpse )
				{
					Corpse corpse = item as Corpse;
					if ( isCorpseLootable(player, corpse) )
						corpses.Add(corpse);
				}
				else if ( item.Movable && item.IsAccessibleTo(player) && isItemLootable(item) )
					items.Add(item);
 			}

			foreach ( Item item in items )
				TryDropItem(player, item, false);

				
			bool lootedAll = true;
			int totalTokens = 0;
			int retries = 3;
			foreach ( Corpse corpse in corpses )
			{
				if ( lootContainer(player, corpse) )
				{

					if ( DeleteAllCorpses || corpse.GetAmount(typeof(Item), false) == 0 )
					{

						int reward = getCorpseReward(corpse);

						if ( reward > 0 )
						{

							totalTokens += reward;
							AddTokensAmount((ulong)reward);
							AddGoldAmount((ulong)(reward*2));
						}

						corpse.Delete();
					}

				}
				else
				{
					lootedAll = false;
					if ( --retries == 0 )
						break;
				}
			}

			if ( totalTokens > 0 )
			{
				player.SendMessage(1173, "You gained " + totalTokens + " tokens for cleaning the shard.");
			}
			else
				player.SendMessage(1173, "You didn't gain a single token...");
			if ( !lootedAll )
				player.SendMessage(1173, "You can't loot all the items.");
		}
		
		private bool lootContainer( PlayerMobile player, Container container )
		{
			if ( !IsOwner(player) )
				return false;
			List<Item> items = new List<Item>( container.Items.Count );
			foreach ( Item item in container.Items )
				if ( item != null && isItemLootable( item ) )
					items.Add( item );

			foreach ( Item item in items )
				if ( !this.TryDropItem(player, item, false) )
					return false;
			return true;
		}
		
		private int getCorpseReward( Corpse c )
		{

			if (c == null || c.Owner == null)
				return 0;

			BaseCreature owner = c.Owner as BaseCreature;
			if ( owner == null )
				return 0;

			double resists = ((owner.PhysicalResistance + owner.FireResistance + owner.ColdResistance + owner.PoisonResistance + owner.EnergyResistance)/50); //set the amount of resists the monster have
			if (resists < 1.0) //if it have less then total on 100 resists set to 1
				resists = 1.0;
			int hits = (owner.HitsMax/10); //set the amount of max hp the creature had.
			double tempReward = (hits + ((hits * resists)/10) ); //set the temp reward
						
			int fameKarma = Math.Abs(owner.Fame) + Math.Abs(owner.Karma); //set fame\karma reward bonus
			fameKarma /= 250;
			tempReward += fameKarma; //add the fame\karma reward to the temp reward

			if (owner.AI == AIType.AI_Mage) //if it's mage add some tokens, it have spells
			{
				double mage = ((owner.Skills[SkillName.Meditation].Value + owner.Skills[SkillName.Magery].Value + owner.Skills[SkillName.EvalInt].Value)/8);
				tempReward += mage;
			}
							
			if (owner.HasAbility(SpecialAbility.DragonBreath)) //give bonus for creatures that have fire breath
			{
				double fireBreath = (owner.HitsMax/25);
				tempReward += fireBreath; //add the firebreath bonus to temp reward
			}	
							
			int reward = ((int)tempReward);
			reward = Utility.RandomMinMax((int)(reward*0.3), (int)(reward*0.6));

			if (reward < 1)
				reward = 1; //set minimum reward to 1

			return reward;
		}
		
		public bool isItemLootable( Item item )
		{
			if ( item == null || item.Deleted || !item.Movable )
				return false;
			return isTypeLootable(item.GetType());
		}
		
		internal bool isTypeLootable(Type itemType)
		{
			if ( itemType == null )
				return false;

			if ( lootSettings == LootSettingsEnum.Everything )
				return true;

			if ( MasterStorageUtils.IsCurrencyType(itemType) )
				return true;
			
			if ( lootSettings == LootSettingsEnum.Currency_Only )
				return false;

			if ( activeLootTypes != null && activeLootTypes.Contains(itemType) )
				return true;

			if ( activeBaseTypes != null )
				foreach ( Type type in activeBaseTypes )
					if ((type.IsInterface && type.IsAssignableFrom(itemType)) ||
						itemType.IsSubclassOf(type) )
						return true;
			return false;
		}

		internal bool AddLootableItemType(Type type)
		{
			if ( type == null )
				return false;
			if ( isTypeLootable(type) )
				return false;
			activeLootTypes.Add(type);
			return true;
		}
		
		internal bool AddLootableBaseType(Type type)
		{
			if ( type == null )
				return false;
			if ( isTypeLootable(type) )
				return false;
			
			List<Type> contained = new List<Type>();
			foreach ( Type t in activeBaseTypes )
				if ( t.IsSubclassOf(type) )
					contained.Add(t);
			foreach ( Type t in contained )
				activeBaseTypes.Remove(t);
			foreach ( Type t in activeLootTypes )
				if ( t.IsSubclassOf(type) )
					contained.Add(t);
			foreach ( Type t in contained )
				activeLootTypes.Remove(t);
			activeBaseTypes.Add(type);
			return true;
		}

		internal void RemoveLootableType(Type type)
		{
			if ( type == null )
				return;

			if ( activeLootTypes.Contains(type) )
				activeLootTypes.Remove(type);
			else if ( activeBaseTypes.Contains(type) )
				activeBaseTypes.Remove(type);
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 8 ); // version
			
			//version 7
			writer.Write(StoragePage);
			writer.Write(StorageFilter != null);
			if ( StorageFilter != null )
				StorageFilter.Serialize(writer);

			//version 6
			writer.Write(StoredItems.Count);
            
			foreach (Type key in StoredItems.Keys)
			{
				writer.Write(key.FullName);
				writer.Write(StoredItems[key]);

               // Vii edit
                if (key.IsSubclassOf(typeof(BaseInstrument)))
                {
                    if (ItemInfoByType.ContainsKey(key) && ItemInfoByType[key].Count == (int)StoredItems[key])
                    {

                        writer.Write((bool)true); // This is to stop a possible loading crash

                        foreach (ItemInformation info in ItemInfoByType[key])
                        {
                            info.Serialize(writer);
                        }
                    }
                    else
                        writer.Write((bool)false);  // Fix possible loading crash
                }
                // end Vii edit

			}
            
			//version 5
			writer.Write(KeepItemsOnDeath);
			
			//version 4
			writer.Write(activeListA);
			
			//version 6 change start
			SerializeTypeList(writer, baseTypesA);
			SerializeTypeList(writer, lootTypesA);
			SerializeTypeList(writer, baseTypesB);
			SerializeTypeList(writer, lootTypesB);
			//version 6 change end

			writer.Write( GoldLedger );
			writer.Write( TokenLedger );
			writer.Write( (int)lootSettings );
			writer.Write( GoldAmount );
			writer.Write( TokensAmount );
			writer.Write( Owner ); //version 8 changed from int to PlayerMobile
			// Version 2
			writer.Write( DeleteAllCorpses );

			//version 6 addition
			writer.Write(StorageList.Count);
			foreach (BaseStorage storage in StorageList)
			{
				//force delete in version 8 without upgrading to 9 intentionally!
				writer.Write(storage.GetType().FullName);
				storage.Serialize(writer);
			}
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( version >= 7 )
			{
				StoragePage = reader.ReadInt();
				bool hasStorageFilter = reader.ReadBool();
				if (hasStorageFilter)
					StorageFilter = new BaseStorage(reader);

			}
			if (version >= 6)
			{
				int count = reader.ReadInt();
				StoredItems = new Dictionary<Type, ulong>(count);
                
				for (int i = 0; i < count; ++i)
				{
					try
					{
						Type key = Type.GetType(reader.ReadString());
						ulong amount = reader.ReadULong();
                        if (key != null)
                        {
                            StoredItems.Add(key, amount);

                            // Vii edit
                            if (key.IsSubclassOf(typeof(BaseInstrument)))
                            {
                                if (reader.ReadBool()) // For stopping load crash
                                {
                                    m_ItemInfoByType = new Dictionary<Type, List<ItemInformation>>();
                                    List<ItemInformation> info = new List<ItemInformation>();
                                    for (int j = 0; j < (int)amount; j++)
                                    {
                                        ItemInformation iteminfo = new ItemInformation(reader);
                                        info.Add(iteminfo);
                                    }
                                    ItemInfoByType.Add(key, info);
                                }
                            }
                            // end Vii edit
                        }
					}
					catch { }
				}
                
				KeepItemsOnDeath = reader.ReadBool();
				activeListA = reader.ReadBool();
				baseTypesA = DeserializeTypeList(reader, true);
				lootTypesA = DeserializeTypeList(reader, true);
				baseTypesB = DeserializeTypeList(reader, true);
				lootTypesB = DeserializeTypeList(reader, true);
				GoldLedger = reader.ReadBool();
				TokenLedger = reader.ReadBool();
				try { lootSettings = (LootSettingsEnum)reader.ReadInt(); }
				catch { lootSettings = LootSettingsEnum.From_List; }
				GoldAmount = reader.ReadULong();
				TokensAmount = reader.ReadULong();
				if (version >= 8)
					Owner = reader.ReadMobile() as PlayerMobile;
				else
				{
					int serial = reader.ReadInt();
					Owner = World.FindMobile(serial) as PlayerMobile;
				}
				DeleteAllCorpses = reader.ReadBool();

				count = reader.ReadInt();
				StorageList = new List<BaseStorage>(count);
				while (count-- > 0)
				{
					try
					{
						BaseStorage storage;
						if (version >= 8)
						{
							Type storageType = Type.GetType(reader.ReadString());
							if ( storageType != typeof(BaseStorage) )
								storage = Activator.CreateInstance(storageType, new object[] { reader }) as BaseStorage;
							else
								storage = new BaseStorage(reader);
						}
						else
							storage = new BaseStorage(reader);
						if (!StorageList.Contains(storage))
							StorageList.Add(storage);
					} catch { }
				}
			}
			else
			{
				StorageList = new List<BaseStorage>();
				StoredItems = new Dictionary<Type, ulong>();
				if (version >= 5)
					KeepItemsOnDeath = reader.ReadBool();
				if (version >= 4)
				{
					activeListA = reader.ReadBool();
					DeserializeList(reader, true);
					DeserializeList(reader, false);
				}
				else
				{
					activeListA = false;
					RestoreDefaultList();
					activeListA = true;
					DeserializeList(reader, true);
				}

				GoldLedger = reader.ReadBool();
				TokenLedger = reader.ReadBool();
				if (version >= 3)
				{
					try { lootSettings = (LootSettingsEnum)reader.ReadInt(); }
					catch { lootSettings = LootSettingsEnum.From_List; }
				}
				else
					lootSettings = reader.ReadBool() ? LootSettingsEnum.Currency_Only : LootSettingsEnum.From_List;
				GoldAmount = reader.ReadULong();
				TokensAmount = reader.ReadULong();
				int serial = reader.ReadInt();
				Owner = World.FindMobile(serial) as PlayerMobile;
				if (version > 1)
					DeleteAllCorpses = reader.ReadBool();
			}
		}
		private List<Type> DeserializeTypeList(GenericReader reader, bool CheckLootable)
		{
			int count = reader.ReadInt();
			List<Type> list = new List<Type>(count);
			for (int i = 0; i < count; ++i)
			{
				try
				{
					Type type = Type.GetType(reader.ReadString());
					if (type != null && (!CheckLootable || !isTypeLootable(type)))
						list.Add(type);
				}
				catch { }
			}
			return list;
		}
		private void DeserializeList(GenericReader reader, bool deserActiveListA)
		{
			bool tempActiveListA = activeListA;
			activeListA = deserActiveListA;
			
			activeBaseTypes = new List<Type>();
			int count = reader.ReadInt();
			for ( int i=0; i<count; ++i )
			{
				try
				{
					Type type = Type.GetType(reader.ReadString());
					if ( type != null && !isTypeLootable(type) )
						activeBaseTypes.Add(type);
				}
				catch { }
			}
			
			activeLootTypes = new List<Type>();
			count = reader.ReadInt();
			for ( int i=0; i<count; ++i )
			{
				try
				{
					Type type = Type.GetType(reader.ReadString());
					
					if ( type != null && !isTypeLootable(type) )
						activeLootTypes.Add(type);
				}
				catch { }
			}
			
			activeListA = tempActiveListA;
		}

		public void SerializeTypeList(GenericWriter writer, List<Type> list)
		{
			writer.Write(list.Count);
			foreach (Type type in list)
				writer.Write(type.FullName);
		}
		public void SerializeList( GenericWriter writer, bool serActiveListA )
		{
			bool tempActiveListA = activeListA;
			activeListA = serActiveListA;
			
			writer.Write( activeBaseTypes.Count );
			foreach ( Type type in activeBaseTypes )
				writer.Write( type.FullName );
				
			writer.Write( activeLootTypes.Count );
			foreach ( Type type in activeLootTypes )
				writer.Write( type.FullName );

			activeListA = tempActiveListA;
		}
		public override void DropItem( Item dropped )
		{
			if ( AddCurrency(dropped) )
				return;
			base.DropItem(dropped);
		}
		public override bool OnDragDropInto( Mobile from, Item item, Point3D p )
		{
			if ( AddCurrency(item) )
				return true;
			return base.OnDragDropInto(from, item, p);
		}
		public override bool TryDropItem( Mobile from, Item dropped, bool sendFullMessage )
		{
			if ( AddCurrency(dropped) )
				return true;
			return base.TryDropItem(from, dropped, sendFullMessage);
		}
		public override bool OnDragDrop(Mobile from, Item dropped)
		{
			if ( AddCurrency(dropped) )
				return true;
			return base.OnDragDrop(from, dropped);
		}
		
		public void AddCurrencyFromBackpack( PlayerMobile player )
		{
			foreach ( Item item in player.Backpack.FindItemsByType(typeof(Item)) )
				AddCurrency(item);
		}
		public bool AddCurrency( Item currency )
		{
			if ( !MasterStorageUtils.IsCurrencyType(currency.GetType()) )
				return false;

			if (currency is Gold && GoldLedger)
				return AddGoldPile(currency as Gold);

			if (currency is BankCheck && GoldLedger)
				return AddGoldCheck(currency as BankCheck);

#if USE_TOKENS
			if (currency is Daat99Tokens && TokenLedger)
				return AddTokensPile(currency as Daat99Tokens);

			if (currency is TokenCheck && TokenLedger)
				return AddTokenCheck(currency as TokenCheck);
#endif
			return false;
		}

		public bool AddGoldPile(Gold gold)
		{
			if ( !GoldLedger )
				return false;
			gold.Amount = (int)AddGoldAmount((ulong)gold.Amount);
			if (gold.Amount == 0)
			{
				gold.Consume();
				return true;
			}
			return false;
		}
		public bool AddGoldCheck(BankCheck check)
		{
			if ( !GoldLedger )
				return false;
			check.Worth = (int)AddGoldAmount((ulong)check.Worth);
			if (check.Worth == 0)
			{
				check.Consume();
				return true;
			}
			return false;
		}

		public ulong AddGoldAmount(ulong amount)
		{
			if ( !GoldLedger )
				return amount;
			ulong max = ulong.MaxValue - GoldAmount;
			if (max > amount)
			{
				GoldAmount += amount;
				return (ulong)0;
			}
			GoldAmount += max;
			amount -= max;
			return amount;
		}

#if USE_TOKENS
		public bool AddTokensPile(Daat99Tokens tokens)
		{
			if ( !TokenLedger )
				return false;
			tokens.Amount = (int)AddTokensAmount((ulong)tokens.Amount);
			if (tokens.Amount == 0)
			{
				tokens.Consume();
				return true;
			}
			return false;
		}
		public bool AddTokenCheck(TokenCheck tokenCheck)
		{
			if ( !TokenLedger )
				return false;
			tokenCheck.tokensAmount = (int)AddTokensAmount((ulong)tokenCheck.tokensAmount);
			if (tokenCheck.tokensAmount == 0)
			{
				tokenCheck.Consume();
				return true;
			}
			return false;
		}
#else
		public bool AddTokensPile(object tokens) { return false; }
		public bool AddTokenCheck(object tokenCheck) { return false; }
#endif
		public ulong AddTokensAmount(ulong amount)
		{
			if ( !TokenLedger )
				return amount;
			ulong max = ulong.MaxValue - TokensAmount;
			if (max > amount)
			{
				TokensAmount += amount;
				return (ulong)0;
			}
			TokensAmount += max;
			amount -= max;
			return amount;
		}

		internal bool withdrawGold(PlayerMobile player, int amount, bool intoGoldCheck)
		{
			if (amount <= 0)
				return true;
			List<Item> gold;
			Type goldType = intoGoldCheck ? typeof(BankCheck) : typeof(Gold);
			gold = TryExtractType(goldType, (int)amount);
			if (gold.Count > 0)
			{
				foreach (Item item in gold)
					player.AddToBackpack(item);
				return true;
			}
			return false;
		}

		internal bool withdrawTokens(PlayerMobile player, int amount, bool intoTokensCheck)
		{
			if (amount <= 0)
				return true;
#if USE_TOKENS
			List<Item> tokens;
			Type tokensType = intoTokensCheck? typeof(TokenCheck) : typeof(Daat99Tokens);
			tokens = TryExtractType(tokensType, (int)amount);
			if (tokens.Count > 0)
			{
				foreach (Item item in tokens)
					player.AddToBackpack(item);
				return true;
			}
#endif
			return false;
		}
	
		private bool isContainerLootable(PlayerMobile player, Container container)
		{
			if (container.Movable || container.Deleted || container.IsAccessibleTo(player) )
				return false;
			if (container is Corpse)
				return isCorpseLootable(player, (Corpse)container);
			return true;
		}
		
		private bool isCorpseLootable(PlayerMobile player, Corpse corpse)
		{
			if ( corpse.Owner == null || corpse.Deleted || corpse.Owner is PlayerMobile 
				|| (corpse.Owner is BaseCreature && ((BaseCreature)corpse.Owner).IsBonded) 
				|| !MasterStorageUtils.CheckLoot(corpse, player) || corpse.IsCriminalAction(player)
				)
					return false;
			return true;
		}
		
		public Type[] GetLootableTypesForPage( int page )
		{
			Type[] types =  new Type[10];
			int index = page*10;
			if ( index < 0 || index > LootableTypesCount )
				return types;
			int added = 0;
			if ( index < activeBaseTypes.Count )
			{
				while ( added < 10 && index < activeBaseTypes.Count)
					types[added++] = activeBaseTypes[index++];
			}
			index -= activeBaseTypes.Count;
			while ( added < 10 && index < activeLootTypes.Count)
				types[added++] = activeLootTypes[index++];
			return types;
		}
		
		public bool IsOwner(PlayerMobile player)
		{
			if (player == null || this.Deleted || !IsChildOf( player.Backpack ))
				return false;
			if ( Owner == null )
			{
				Name = player.Name + "'s " + DefaultLooterName;
				Owner = player;
			}
			if ( Owner != player )
			{
				player.SendMessage("This isn't yours!");
				return false;
			}
			return true;
		}
		
		public void NextLootSettings()
		{
			switch ( lootSettings )
			{
				case LootSettingsEnum.Currency_Only:
					lootSettings = LootSettingsEnum.Everything;
					break;
				case LootSettingsEnum.Everything:
					lootSettings = LootSettingsEnum.From_List;
					break;
				default:
				case LootSettingsEnum.From_List:
					lootSettings = LootSettingsEnum.Currency_Only;
					break;
			}
		}
		
		public void MoveItemsOnDeath( Container corpse )
		{
			if ( !KeepItemsOnDeath && corpse != null && !corpse.Deleted )
			{
				List<Item> itemsToMove = new List<Item>();
				
				foreach ( Item item in Items )
					if ( item.LootType != LootType.Blessed && item.LootType != LootType.Newbied && item.Movable )
						itemsToMove.Add(item);

				foreach ( Item item in itemsToMove )
					corpse.DropItem(item);
			}
		}

		public bool ConsumeCurrency( PlayerMobile player, Type currencyType, UInt64 amount )
		{ return ConsumeCurrency( player, currencyType, amount, false ); }
		public bool ConsumeCurrency( PlayerMobile player, Type currencyType, UInt64 amount, bool informPlayer )
		{
			bool gold = false;
			if ( currencyType == typeof(Gold))
				gold = true;
			else 
#if USE_TOKENS
				if ( currencyType != typeof(Daat99Tokens) )
#endif
			{
				if ( informPlayer )
					player.SendMessage("Unable to consume currency type: " + currencyType.Name);
				return false;
			}
			
			if ( gold )
				return ConsumeGold(player, amount, informPlayer);
#if USE_TOKENS
			else
				return ConsumeTokens(player, amount, informPlayer);
#else
			return false;
#endif
		}
		
		public bool ConsumeGold( PlayerMobile player, UInt64 amount, bool informPlayer )
		{
			if ( player == null )
				return false;
			if ( !IsOwner(player) )
			{
				if ( informPlayer )
					player.SendMessage("This Master Storage doesn't belong to you!");
				return false;
			}
			if ( !GoldLedger )
			{
				if ( informPlayer )
					player.SendMessage("Your Master Storage doesn't contain Gold.");
				return false;
			}
			if ( GoldAmount < amount )
			{
				if ( informPlayer )
					player.SendMessage("Your Master Storage doesn't contain enough Gold.");
				return false;
			}
			GoldAmount -= amount;
			if ( informPlayer )
				player.SendMessage("You successfully consumed " + amount + " of Gold from your Master Storage.");
			return true;
		}
		public bool ConsumeTokens(PlayerMobile player, UInt64 amount, bool informPlayer)
		{
#if USE_TOKENS
			if (player == null)
				return false;
			if (!IsOwner(player))
			{
				if (informPlayer)
					player.SendMessage("This Master Storage doesn't belong to you!");
				return false;
			}
			if (!TokenLedger)
			{
				if (informPlayer)
					player.SendMessage("Your Master Storage doesn't contain Tokens.");
				return false;
			}
			if (TokensAmount < amount)
			{
				if (informPlayer)
					player.SendMessage("Your Master Storage doesn't contain enough Tokens.");
				return false;
			}
			TokensAmount -= amount;
			if (informPlayer)
				player.SendMessage("You successfully consumed " + amount + " of Tokens from your Master Storage.");
			return true;
#else
			return false;
#endif
		}

		public bool HasGold(int amount)
		{
			if ( amount <= 0 )
				return true;
			if (GoldLedger && GoldAmount >= (ulong)amount)
				return true;
			return false;
		}
		public bool HasTokens(int amount)
		{
#if USE_TOKENS
			if (amount <= 0)
				return true;
			if (TokenLedger && TokensAmount >= (ulong)amount)
				return true;
#endif
			return false;
		}

		public bool TryConsume(Type type, int amount)
		{
			if (amount <= 0)
				return true;
			Type currencyType = MasterStorageUtils.GetCurrencyType(type);
			if ( currencyType == typeof(Gold) )
			{
				if (HasGold(amount))
				{
					GoldAmount -= (ulong)amount;
					return true;
				}
				else
					return false;
			}
#if USE_TOKENS
			else if ( currencyType == typeof(Daat99Tokens) )
			{
				if (HasTokens(amount))
				{
					TokensAmount -= (ulong)amount;
					return true;
				}
				else
					return false;
			}
#endif
			if (StoredItems.ContainsKey(type) && StoredItems[type] >= (ulong)amount)
			{
				StoredItems[type] -= (ulong)amount;
				if (StoredItems[type] == 0)
					StoredItems.Remove(type);
				if (Owner.HasGump(typeof(MasterStorageStorageGump)))
					MasterStorageStorageGump.SendGump(Owner, this, StoragePage, StorageFilter);
				return true;
			}
			return false;
		}
		public bool TryConsume(Type[] types, int[] amounts)
		{
			if (types.Length != amounts.Length)
				return false;
			bool hasEnough = true;
			for (int i = 0; i < types.Length && hasEnough; ++i)
			{
				if (amounts[i] < 0) //we don't need anything
					continue;
				if ( StoredItems.ContainsKey(types[i]) ) //we have stored item
				{
					if ( StoredItems[types[i]] > (ulong)amounts[i] ) //we have enough
						continue;
					hasEnough = false; //we don't have enough
					break;
				}
				Type currencyType = MasterStorageUtils.GetCurrencyType(types[i]);
				if ( currencyType == typeof(Gold) && HasGold(amounts[i]) ) //we have enough gold
					continue;
#if USE_TOKENS
				if ( currencyType == typeof(Daat99Tokens) && HasTokens(amounts[i]) ) //we have enough tokens
					continue;
#endif
				hasEnough = false; //we don't have that item
			}
			if (!hasEnough)
				return false;
			for (int i = 0; i < types.Length; ++i)
				if (amounts[i] > 0)
				{
					StoredItems[types[i]] -= (ulong)amounts[i];
					if (StoredItems[types[i]] == 0)
						StoredItems.Remove(types[i]);
				}
			if (Owner != null && Owner.HasGump(typeof(MasterStorageStorageGump)))
				MasterStorageStorageGump.SendGump(Owner, this, StoragePage, StorageFilter);
			return true;
		}
#region Storage
		private List<BaseStorage> storageList;
		[CommandProperty(AccessLevel.GameMaster)]
		public List<BaseStorage> StorageList { get { return storageList; } set { storageList = value; } }
		private int storagePage = 0;
		public int StoragePage { get { return storagePage; } set { storagePage = value; } }
		private BaseStorage storageFilter = null;
		public BaseStorage StorageFilter { get { return storageFilter; } set { storageFilter = value; } }
		private Dictionary<Type, ulong> storedItems;
		public Dictionary<Type, ulong> StoredItems { get { return storedItems; } private set { storedItems = value; } }

		public bool HasStorage(BaseStorage storage)
		{
			return StorageList.Contains(storage);
		}
		public void AddStorage(BaseStorage storage)
		{
			if (!StorageList.Contains(storage))
				StorageList.Add(storage);
		}
		public bool CanStoreType(Type type)
		{
			Type currencyType = MasterStorageUtils.GetCurrencyType(type);
			if (currencyType == typeof(Gold))
				return GoldLedger;
#if USE_TOKENS
			if (currencyType == typeof(Daat99Tokens))
				return TokenLedger;
#endif
			foreach (BaseStorage storage in StorageList)
				if (storage.IsTypeStorable(type))
					return true;
			return false;
		}

		public Dictionary<Type, int> GetStoreableType(Item item)
		{
			Dictionary<Type, int> types = new Dictionary<Type, int>(1);
			Type currencyType = MasterStorageUtils.GetCurrencyType(item.GetType());
			if (currencyType == typeof(Gold))
			{
				if (!GoldLedger)
					return types;
				types.Add(currencyType, item.Amount);
			}
#if USE_TOKENS
			else if (currencyType == typeof(Daat99Tokens))
			{
				if (!TokenLedger)
					return types;
				types.Add(currencyType, item.Amount);
			}
#endif
			else
			{
				foreach (BaseStorage storage in StorageList)
				{
					types = storage.GetStorableTypesFromItem(item);
					if (types.Count > 0)
						return types;
				}
			}
			return types;
		}
		public List<Item> TryExtractType(Type type, int amount)
		{
			if (!TryConsume(type, amount))
				return new List<Item>(0);
            // Vii edit
            if ( type.IsSubclassOf(typeof(BaseInstrument)) )
            {
                
                List<Item> instruments = new List<Item>();
                if (ItemInfoByType.ContainsKey(type))
                {
                    if (amount <= ItemInfoByType[type].Count)
                    {
                        Console.WriteLine("Instrument confirmed.");
                        for (int i = 0; i < amount; i++)
                        {

                            instruments.Add(MasterStorageUtils.CreateItemFromType(type, ItemInfoByType[type][0]));
                            ItemInfoByType[type].RemoveAt(0);
                            if (ItemInfoByType[type].Count == 0)
                            {
                                ItemInfoByType.Remove(type);
                                break;
                            }
                        }
                        return instruments;
                    }
                }
            }
            // end Vii edit

			return MasterStorageUtils.CreateItemsFromType(type, amount);
		}


		public ulong GetStoredItemAmount(Type type)
		{
			if (StoredItems.ContainsKey(type))
				return StoredItems[type];
			return 0;
		}

		public bool TryStoreItem(Item item)
		{
			IUsesRemaining tool = item as IUsesRemaining;
			int amount = item.Amount;
			if (tool != null)
				amount = tool.UsesRemaining;
			return TryStoreItem(item, amount);
		}
		public bool TryStoreItem(Item item, int amount)
		{
			Dictionary<Type, int> types = GetStoreableType(item);
			if (types == null || types.Count == 0)
				return false;
			if (amount < 0)
				return false;
			//first make sure we can add them all
			//update the amount and make sure we don't have "negative" amounts
			foreach (Type type in types.Keys)
			{
				if ( types[type] <= 0 )
					types[type] = amount;
				ulong usedAmount = (ulong)types[type];
				
				if (usedAmount < 0)
					return false;

				if (usedAmount == 0)
					continue;
				
				if (type == typeof(Gold) && GoldAmount + usedAmount < GoldAmount)
					return false;
#if USE_TOKENS
				else if (type == typeof(Daat99Tokens) && TokensAmount + usedAmount < TokensAmount)
					return false;
#endif
				else if (StoredItems.ContainsKey(type) && usedAmount + StoredItems[type] < usedAmount)
					return false;
			}
			foreach (Type type in types.Keys)
			{
				int usedAmount = types[type];
				if (usedAmount == 0)
					continue;
				
				if (type == typeof(Gold))
					GoldAmount += (ulong)usedAmount;
#if USE_TOKENS
				else if (type == typeof(Daat99Tokens))
					TokensAmount += (ulong)usedAmount;
#endif
				else
				{
					if (!StoredItems.ContainsKey(type))
						StoredItems[type] = 0;
					StoredItems[type] += (ulong)usedAmount;
				}
			}
			if (Owner != null && Owner.HasGump(typeof(MasterStorageStorageGump)))
				MasterStorageStorageGump.SendGump(Owner, this, StoragePage, StorageFilter);
			return true;
		}
		public bool TryStoreItemType(Type type, int amount)
		{
			if (amount < 0)
				return false;
			if (amount == 0)
				return true;
			Type currencyType = MasterStorageUtils.GetCurrencyType(type);
			if (currencyType != null)
			{
				if (currencyType == typeof(Gold) && GoldLedger)
				{
					GoldAmount += (ulong)amount;
					return true;
				}
#if USE_TOKENS
				if (currencyType == typeof(Daat99Tokens) && TokenLedger)
				{
					TokensAmount += (ulong)amount;
					return true;
				}
#endif
				return false;
			}
			if (!CanStoreType(type))
				return false;
			if (!StoredItems.ContainsKey(type))
				StoredItems[type] = 0;
			if ((ulong)amount + StoredItems[type] >= (ulong)amount)
			{
				StoredItems[type] += (ulong)amount;
				if (Owner != null && Owner.HasGump(typeof(MasterStorageStorageGump)))
					MasterStorageStorageGump.SendGump(Owner, this, StoragePage, StorageFilter);
				return true;
			}
			return false;
		}

		public MasterStorageStorageGump.StoredItemGumpObject[] GetStoredItemsForPage(int itemsCount, int page) { return GetStoredItemsForPage(itemsCount, page, null); }
		public MasterStorageStorageGump.StoredItemGumpObject[] GetStoredItemsForPage(int itemsCount, int page, BaseStorage filter)
		{
			MasterStorageStorageGump.StoredItemGumpObject[] types = new MasterStorageStorageGump.StoredItemGumpObject[itemsCount+1];
			StoragePage = page;
			StorageFilter = filter;
			int startIndex = StoragePage * itemsCount;
			if (startIndex < 0)
				startIndex = 0;
			int endIndex = startIndex + itemsCount+1;
			int current = 0;
			foreach (Type key in StoredItems.Keys)
			{
				if (StorageFilter != null && !StorageFilter.IsTypeStorable(key))
					continue;
				if (current >= startIndex && current < endIndex)
					types[current - startIndex] = new MasterStorageStorageGump.StoredItemGumpObject(key, StoredItems[key]);
				++current;
			}
			return types;
		}

		public void RefillStorage( PlayerMobile player )
		{
			foreach (Item item in player.Backpack.FindItemsByType(typeof(Item)))
			{
				if (AddCurrency(item))
					continue;
                if (TryStoreItem(item))
                {
                    // Vii edit
                    if (item is BaseInstrument)
                    {
                        BaseInstrument inst = (BaseInstrument)item;
                        ItemInformation iteminfo = new ItemInformation(inst.Name, new SlayerName[] { inst.Slayer, inst.Slayer2 }, inst.Quality, inst.Crafter, inst.UsesRemaining);
                        if (!ItemInfoByType.ContainsKey(item.GetType()))
                            ItemInfoByType.Add(item.GetType(), new List<ItemInformation>());
                        ItemInfoByType[item.GetType()].Add(iteminfo);
                    }
                    // end Vii edit
                    item.Consume(item.Amount);
                }
			}
			if (player.HasGump(typeof(MasterStorageStorageGump)))
				MasterStorageStorageGump.SendGump(player, this, StoragePage, StorageFilter);
		}
#endregion Storage

        // Vii added
        private Dictionary<Type, List<ItemInformation>> m_ItemInfoByType;
        public Dictionary<Type, List<ItemInformation>> ItemInfoByType
        {
            get
            {
                if (m_ItemInfoByType == null)
                    m_ItemInfoByType = new Dictionary<Type, List<ItemInformation>>();
                return m_ItemInfoByType;
            }
            set { m_ItemInfoByType = value; }
        }
        // end Vii add

	}
    // Vii added for the saving of specific item information other than Type
    public class ItemInformation
    {
        private string m_name;
        public string Name { get { return m_name; } set { m_name = value; } }

        private SlayerName m_Slayer, m_Slayer2;
        public SlayerName Slayer { get { return m_Slayer; } set { m_Slayer = value; } }
        public SlayerName Slayer2 { get { return m_Slayer2; } set { m_Slayer2 = value; } }

        private ItemQuality m_Quality;
        public ItemQuality Quality { get { return m_Quality; } set { m_Quality = value; } }

        private Mobile m_Crafter;
        public Mobile Crafter { get { return m_Crafter; } set { m_Crafter = value; } }

        private int m_UsesRemaining;
        public int UsesRemaining { get { return m_UsesRemaining; } set { m_UsesRemaining = value; } }

        public ItemInformation(string name, SlayerName[] slayers, ItemQuality quality, Mobile crafter, int uses)
        {
            m_name = name;
            if (slayers != null)
            {
                m_Slayer = slayers[0];
                m_Slayer2 = slayers[1];
            }
            else
            {
                m_Slayer = SlayerName.None;
                m_Slayer2 = SlayerName.None;
            }
            m_Quality = quality;
            m_Crafter = crafter;
            m_UsesRemaining = uses;
        }

        public void Serialize(GenericWriter writer)
        {
            writer.Write(0); // version

            writer.Write(m_name);
            writer.Write((int)m_Slayer);
            writer.Write((int)m_Slayer2);
            writer.Write((int)m_Quality);
            writer.Write(m_Crafter);
            writer.Write((int)m_UsesRemaining);
        }

        public ItemInformation(GenericReader reader)
        {
            int version = reader.ReadInt();

            m_name = reader.ReadString();
            m_Slayer = (SlayerName)reader.ReadInt();
            m_Slayer2 = (SlayerName)reader.ReadInt();
            m_Quality = (ItemQuality)reader.ReadInt();
            m_Crafter = reader.ReadMobile();
            m_UsesRemaining = reader.ReadInt();
        }
    }
}

