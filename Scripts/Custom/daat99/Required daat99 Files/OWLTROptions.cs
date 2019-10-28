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

using System;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Commands;

namespace daat99
{
	public class OWLTROptionsManager
	{
		//NOTE make sure the first actual setting starts with 0 and there are no "jumps"!!!
		public enum OPTIONS_ENUM
		{
			//OWLTR
			UNKNOWN = -1, UBBER_RESOURCES = 0, DAAT99_MINING, DAAT99_LUMBERJACKING, A_LI_N_CLEAN_CHAMP,
			SAVE_CLEAN_CHAMP_GOLD, RECIPE_CRAFT, CRAFTING_BANK_HIVE, CRAFTING_STORAGE_DEEDS, CRAFTING_MOBILE_FORGE,
			MULE_FORGE, CRAFT_RUNIC_JEWELRY, USE_STORAGE_RESOURCES, MONSTER_GIVE_TOKENS, BOD_GIVE_TOKENS,
			HARVEST_GIVE_TOKENS, CRAFT_GIVE_TOKENS, ALCHEMY_RECIPES, BLACKSMITH_RECIPES, BOWFLETCH_RECIPES,
			CARPENTRY_RECIPES, COOKING_RECIPES, GLASSBLOWING_RECIPES, INSCRIPTION_RECIPES, MASONRY_RECIPES,
			TAILORING_RECIPES, TINKERING_RECIPES, RECIPE_HARVESTING, DEDICATION_RECIPE, RESOURCE_COST,
			//tokens
			TOKENS_LOTTERY, TOKENS_EXCHANGE,
			//storage
			BLESSED_STORAGE, GOLD_STORAGE, TOKEN_STORAGE, STORAGE_KEEP_ITEMS_DEATH
		}
		private Dictionary<OPTIONS_ENUM, OWLTROption> options;
		public Dictionary<OPTIONS_ENUM, OWLTROption> Options { get { if (options == null) InitOwltrOptions(); return options; } private set { options = value; } }
		private bool[] settings;
		public bool[] Settings { get { if (settings == null) InitOwltrOptions(); return settings; } private set { settings = value; } }
		private static OWLTROptionsManager manager;
		public static OWLTROptionsManager Manager { get { if (manager == null) manager = new OWLTROptionsManager(); return manager; } private set { manager = value; } }

		public void InitOwltrOptions()
		{
			if (options == null)
				options = new Dictionary<OPTIONS_ENUM, OWLTROption>();
			AddOption(OPTIONS_ENUM.UBBER_RESOURCES, new OWLTROption("Ubber Resources", "Enabling Ubber Resources will make the armor hold the max resists and the runics will be more powerfull. Disabling it will make the armor take random resists and less powerfull runics.", false));
			AddOption(OPTIONS_ENUM.DAAT99_MINING, new OWLTROption("Daat99 Mining", "Enabling Daat99 Mining will Make the players mine random ores (acording to their skills) everywhere. Disabling it will use the standard OSI vein mining system (they could camp platinum vein 24/7).", true));
			AddOption(OPTIONS_ENUM.DAAT99_LUMBERJACKING, new OWLTROption("Daat99 Lumberjacking", "Enabling Daat99 Lumberjacking will Make the players lumberjack random woods (acording to their skills) everywhere. Disabling it will use the standard OSI vein lumberjacking system (they could camp petrified vein 24/7).", true));
			AddOption(OPTIONS_ENUM.A_LI_N_CLEAN_CHAMP, new OWLTROption("A_Li_N CleanChamp", "Enabling A_Li_N CleanChamp will delete all the corpses every time a new white skull will be spawned in the champ. It will not delete players corpses or bonded pets.", true));
			AddOption(OPTIONS_ENUM.SAVE_CLEAN_CHAMP_GOLD, new OWLTROption("Save CleanChamp Gold", "Enabling A_Li_N Save Clean Champ Gold will save the gold from the corpses it delete and spawn it when the champion spawns (need A_Li_N CleanChamp enabled to work).", true));
			AddOption(OPTIONS_ENUM.RECIPE_CRAFT, new OWLTROption("Recipe Craft", "Enabling Recipe Craft will make the players need to learn how to craft items and how to harvest ores from crafting recipes.", false));
			AddOption(OPTIONS_ENUM.CRAFTING_BANK_HIVE, new OWLTROption("Crafting BankHive", "Enabling Crafting Bank Hive will enable the bank hive to be craftable in the tinkering menu (need custom craftables to be enabled).", true));
			AddOption(OPTIONS_ENUM.CRAFTING_STORAGE_DEEDS, new OWLTROption("Crafting Storage Deeds", "Enabling Crafting of all the storage deeds.", true));
			AddOption(OPTIONS_ENUM.CRAFTING_MOBILE_FORGE, new OWLTROption("Crafting Mobile Forge", "Enabling Crafting Mobile Forge will enable the mobile forge to be craftable in the tinkering menu (need custom craftables to be enabled).", true));
			AddOption(OPTIONS_ENUM.MULE_FORGE, new OWLTROption("Mule Forge", "Enabling Mule Forge will enable the mules that spawned with master of the arts to smelt ores just like normal forge, dclick the ore and target the mule that you own.", true));
			AddOption(OPTIONS_ENUM.CRAFT_RUNIC_JEWELRY, new OWLTROption("Craft Runic Jewelry", "Enabling Craft Runic Jewelry will enable players to craft runic jewelry (rings, bracelets, earings) with runic tinker tools.", true));
			AddOption(OPTIONS_ENUM.USE_STORAGE_RESOURCES, new OWLTROption("Use Storage Resources", "Enabling Use Master Storage reagents and resources for crafting/casting.", true));
			AddOption(OPTIONS_ENUM.MONSTER_GIVE_TOKENS, new OWLTROption("Monster Give Tokens", "Enabling Monster Give Tokens add tokens to players token ledger (assuming they have one that belong to them in their pack) when they kill monsters.", true));
			AddOption(OPTIONS_ENUM.BOD_GIVE_TOKENS, new OWLTROption("Bod Give Tokens", "Enabling Bod Give Tokens add tokens to players token ledger (assuming they have one that belong to them in their pack) when they return a full bod to the appropriate vendor.", true));
			AddOption(OPTIONS_ENUM.HARVEST_GIVE_TOKENS, new OWLTROption("Harvest Give Tokens", "Enabling Harvest Give Tokens will add tokens to the players token ledger (assuming they have one that belong to them in their pack) when they successfully mine ores or lumberjack wood, Only work with random ores/woods. If ores set to vein you can't get tokens for mining and if lumberjacking is set to vein you won't get tokens for lumberjacking. Players will get 50% more tokens when harvesting in Felucca.", true));
			AddOption(OPTIONS_ENUM.CRAFT_GIVE_TOKENS, new OWLTROption("Craft Give Tokens", "Enabling Craft Give Tokens will add tokens to the players token ledger (assuming they have one that belong to them in their pack) when they successfully craft items. Tokens amount will be based on the amount of the main resource, the type of the main resource and a bonus if the item is exceptional. When the player repairs an item he'll get 0-4 tokens.", true));
			AddOption(OPTIONS_ENUM.ALCHEMY_RECIPES, new OWLTROption("Alchemy recipes", "Alchemy requires recipes", true));
			AddOption(OPTIONS_ENUM.BLACKSMITH_RECIPES, new OWLTROption("Blacksmithy recipes", "Blacksmithy requires recipes", true));
			AddOption(OPTIONS_ENUM.BOWFLETCH_RECIPES, new OWLTROption("BowFletching recipes", "BowFletching requires recipes", true));
			AddOption(OPTIONS_ENUM.CARPENTRY_RECIPES, new OWLTROption("Carpentry recipes", "Carpentry requires recipes", true));
			AddOption(OPTIONS_ENUM.COOKING_RECIPES, new OWLTROption("Cooking recipes", "Cooking requires recipes", true));
			AddOption(OPTIONS_ENUM.GLASSBLOWING_RECIPES, new OWLTROption("Glassblowing recipes", "Glassblowing requires recipes", true));
			AddOption(OPTIONS_ENUM.INSCRIPTION_RECIPES, new OWLTROption("Inscription recipes", "Inscription requires recipes", true));
			AddOption(OPTIONS_ENUM.MASONRY_RECIPES, new OWLTROption("Masonry recipes", "Masonry requires recipes", true));
			AddOption(OPTIONS_ENUM.TAILORING_RECIPES, new OWLTROption("Tailoring recipes", "Tailoring requires recipes", true));
			AddOption(OPTIONS_ENUM.TINKERING_RECIPES, new OWLTROption("Tinkering recipes", "Tinkering requires recipes", true));
			AddOption(OPTIONS_ENUM.RECIPE_HARVESTING, new OWLTROption("Recipe Harvesting", "Recipe Harvesting", true));
			AddOption(OPTIONS_ENUM.DEDICATION_RECIPE, new OWLTROption("Dedication Recipes", "Enabling Dedication Recipes will teach players one recipe every 24 hours and 100 crafted items, I suggest to leave it enabled.", true));
			AddOption(OPTIONS_ENUM.RESOURCE_COST, new OWLTROption("Resource Cost", "Enabling Resource Cost will make the players get more money when they sell armor and weapon crafted with higher level resources, for example iron plate gorget will get 70 gold and dull copper will get 77 gold.", true));

			AddOption(OPTIONS_ENUM.TOKENS_LOTTERY, new OWLTROption("Tokens Lottery", "When enabled it'll allow players to buy lottery tickets from lady luck. <br>It won't effect tickets that were sold already and Lady Luck will accept that last drawing tickets as usuall.", true));
			AddOption(OPTIONS_ENUM.TOKENS_EXCHANGE, new OWLTROption("Tokens Exchange", "When enabled it'll allow players to exchange gold for tokens and tokens for gold.", true));

			AddOption(OPTIONS_ENUM.BLESSED_STORAGE, new OWLTROption("Blessed Master Storage", "Enabling Blessed Master Storage will make all new MasterStorage items start as blessed. This will NOT effect existing items.", true));
			AddOption(OPTIONS_ENUM.GOLD_STORAGE, new OWLTROption("Gold Storage", "Enabling Gold Storage will allow all MasterStorage to act as Gold Storage (only if enabled in MasterStorage or after using a Gold Ledger deed).", true));
			AddOption(OPTIONS_ENUM.TOKEN_STORAGE, new OWLTROption("Token Storage", "Enabling Token Storage will allow all MasterStorage to act as Token Storage (only if enabled in MasterStorage or after using a Token Ledger deed).", true)); 
			AddOption(OPTIONS_ENUM.STORAGE_KEEP_ITEMS_DEATH, new OWLTROption("Keep Items On Death", "Enabling Keep Items On Death will make the MasterStorage act like a blessed bag and keep all the stored items on players death. Disabling it will move all movable non-blessed/newbies items inside it to the players corpse.", true));

		}

		public void AddOption(OPTIONS_ENUM optionEnum, OWLTROption newOption)
		{
			if (options.ContainsKey(optionEnum))
			{
				OWLTROption current = options[optionEnum];
				OWLTROption updated = new OWLTROption(newOption.Name, newOption.Description, current.Setting);
				options[optionEnum] = updated;
			}
			else
				options.Add(optionEnum, newOption);
		}

		public static void SwitchOption(OPTIONS_ENUM option)
		{
			try
			{
				if (Manager.Options.ContainsKey(option))
				{
					bool enabled = !Manager.Options[option].Setting;
					Manager.Options[option].Setting = enabled;
					Manager.Settings[(int)option] = enabled;
				}
			}
			catch { }
		}
		public static bool IsEnabled(OPTIONS_ENUM option)
		{
			try
			{
				return Manager.Settings[(int)option];
			}
			catch { }
			return false;
		}
		public static OWLTROption GetOption(OPTIONS_ENUM option)
		{
			if (Manager.Options.ContainsKey(option))
				return Manager.Options[option];
			return new OWLTROption("UNKNOWN", "UNKNOWN option at id: " + (int)option + ".", false);
		}
		public void Serialize(GenericWriter writer)
		{
			writer.Write(0); //version

			writer.Write(options.Values.Count);
			foreach (OPTIONS_ENUM id in Options.Keys)
			{
				writer.Write((int)id);
				Options[id].Serialize(writer);
			}

		}
		public void Deserialize(GenericReader reader)
		{
			int version = reader.ReadInt();

			int count = reader.ReadInt();
			Options = new Dictionary<OPTIONS_ENUM, OWLTROption>(count);
			while (count-- > 0)
			{
				OPTIONS_ENUM id = OPTIONS_ENUM.UNKNOWN;
				try
				{
					id = (OPTIONS_ENUM)reader.ReadInt();
				}
				catch { }
				OWLTROption option = new OWLTROption(reader);
				if (id != OPTIONS_ENUM.UNKNOWN && !Options.ContainsKey(id))
					AddOption(id, option);
			}
		}
	}

	public class OWLTROption
	{
		public readonly string Name;
		public readonly string Description;
		public bool setting;
		public bool Setting { get { return setting; } set { setting = value; } }
		internal OWLTROption(string name, string description) : this(name, description, false) { }
		internal OWLTROption(string name, string description, bool setting)
		{
			Name = name;
			Description = description;
			Setting = setting;
		}
		public void Serialize(GenericWriter writer)
		{
			writer.Write(0); //version

			writer.Write(Name);
			writer.Write(Description);
			writer.Write(Setting);
		}
		//deserr constructor
		public OWLTROption(GenericReader reader)
		{
			int version = reader.ReadInt();

			Name = reader.ReadString();
			Description = reader.ReadString();
			Setting = reader.ReadBool();
		}

		public void AddToGump(Gump gump, int x, int y, bool forStaff, int buttonID)
		{
			AddToGump(gump, x, y, forStaff, buttonID, false);
		}
		public void AddToGump(Gump gump, int x, int y, bool forStaff, int buttonID, bool addDescription)
		{
			AddToGump(gump, x, y, forStaff, buttonID, addDescription, false);
		}
		public void AddToGump(Gump gump, int x, int y, bool forStaff, int buttonID, bool addDescription, bool requiresRestart)
		{
			gump.AddHtml(x, y, 145, 20, String.Format("<basefont color=#111111>{0}:</font>", Name), false, false);
			if (forStaff) gump.AddButton(155 + x, y, 2511, 2510, buttonID, GumpButtonType.Reply, 0);
			if (Setting == true)
				gump.AddHtml(175 + x, y, 145, 20, String.Format("<basefont color=#008800>Currently enabled.</font>"), false, false);
			else
				gump.AddHtml(175 + x, y, 145, 20, String.Format("<basefont color=#ff0000>Currently disabled.</font>"), false, false);

			if (requiresRestart)
				gump.AddHtml(x, y + 20, 285, 20, String.Format("<basefont color=#ff0000>Require server restart.</font>"), false, false);
			if (addDescription)
				gump.AddHtml(x, y + 20 + (requiresRestart ? 20 : 0), 285, 200, String.Format("<basefont color=#0000ff>{0}</font>", Description), false, false);
		}
	}
}
