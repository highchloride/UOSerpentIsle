/*	System Configuration, changes to the system should be 
	made here instead of changes to the scripts
*/
using System;
using System.Collections;
using Server;

namespace Server
{
    public class Configured
    {		
		#region Enable or Disable System
		public bool ExpGainFromKill		= true;		
		/* PLAYERS Gain EXP Per Kill. Set to false to disable.  */
		/* Setting to false doesn't disable the pet gain system, however for any kills to register for the
		pet gain system, the playermobile MUST have the XMLPlayerLevelAtt attached, otherwise the kills are
		not registered.  This could be done different however this is done to avoid unneeded distro edits.*/

		#endregion

		#region Int Variable Options
        public int StartMaxLvl			= 150;		//What is the highest level reachable without scrolls?
        public int EndMaxLvl			= 200;		//What is the highest level reachable with scrolls?
		public int SkillCoinCap			= 700;		//Change this to match the servers skill cap
		
		public int ExpCoinValue = 100;
		/* Example of specific value */ 
		//	public int ExpCoinValue = 100; 
		/* Random beteen 50-70, can be set to a specific number to adjusted in game with [props */
		#endregion
		
		#region LowLevel XML attachment
		public bool LowLevelBonus		= true;	/* This is a bonus to players under a specific level  */
		public int WhatLevelToDelete	= 21;		/* For LowLevelBonus, what level should the bonus be deleted and kept
													deleted after that level? default 21 and up, LowLevelBonus must be true
													for this to apply! */
		public int StatBonusStr			= 12;		/* LowLevelBonus must be enabled for this to work */
		public int StatBonusDex			= 14;		/* LowLevelBonus must be enabled for this to work */
		public int StatBonusInt			= 16;		/* LowLevelBonus must be enabled for this to work */
		#endregion
		  
		#region General Bool Off and On
		public bool GainExpFromBods 	= false;	/* gain exp from turning in bods, based on points */
		public int ExpPowerAmount		= 75;		/* How much bonus exp for power hour? */
		public bool DisableSkillGain	= true;	/*This will Disable Normal Skill Gain Mechanics, only 
													leveling will allow skill gain. False by default*/
		public bool LevelBelowToon		= false;	/*Default False: In current Servuo Distro, when titles activate this disappears.
													In Older Distro or servers that do not use skill titles over toons head
													this should work. */
		public bool AttachonLogon		= true;		//Attach Level Attachment on Toon Login / This does affect ALL playermobiles. 
		public bool PaperdollLevel		= true;		//Show Players Level In Paperdoll?
        public bool PetKillGivesExp		= true;     //When players pet kills something player gets exp
        public bool CraftGivesExp		= true;		//A sucessful craft gives players exp. (EXPTables.cs for changes) 
        public bool AdvancedSkillExp	= false;	//Only fighting skills give exp?
		public bool TamingGivesExp		= true;		/* Taming Creatures gives EXP, Exp based on attempt, not creature stats */
        public bool AdvancedExp			= false;	/* Use tables to give exp off of killed. 
													set to false to gain exp based on RawStats and Hits*/
		
		
        public bool StaffHasLevel		= false;		//Do Staff Display A Level?
        public bool MaxLevel			= true;	//Show the max level? ex: 86/100
        public bool BonusStatOnLvl		= false;		//On Level Give A Chance To Get A Bonous Stat?
        public bool RefreshOnLevel		= true;		//Sets players hits, stam, and mana to max on level.
		public bool LevelSheetPerma		= true;	/*Shold Player be allowed to drop or move the level sheet?
													Set True to prevent player from dropping, set false to allow drop.*/
		public bool VendorLevels		= false;	//When CreatureLvls is true, do vendors display a level also?
        public bool CreatureLevels		= true;	//Do creatures have levels?
		public bool DiscountsForLevels	= true;		/* Do Level Venders give discounts? */
		#endregion 
		
		#region Party EXP Sharing
        public int PartyRange			= 15;		//When parties share exp, how close do they need to be to get it?
        public bool PartyExpShare		= false;	//False by Default: Do parties share exp?
		
		/* MUST ENABLE PartyExpShare for this to work!!! */
        public bool PartySplitExp		= true;	/*if parties share exp do they split it evenly? */
		
		#endregion
		
		#region Level Up Multpilier
		/* Math = Current Level * Multipier = AmountNeededForNextLevelUp */
		/* Reduce these numbers to lower the amount of EXP needed per level */
		public int L2to20Multipier		= 100;		/* Leve 2 to Level 20 */
		public int L21to40Multiplier	= 200;		/* Leve 21 to Level 40 */
		public int L41to60Multiplier	= 300;		/* Leve 41 to Level 60 */
		public int L61to70Multiplier	= 500;		/* Leve 61 to Level 70 */
		public int L71to80Multiplier	= 700;		/* Leve 71 to Level 80 */
		public int L81to90Multipier		= 900;		/* Leve 81 to Level 90 */
		public int L91to100Multipier	= 1200;		/* Leve 91 to Level 100 */
		public int L101to110Multiplier	= 1500;		/* Leve 101 to Level 110 */
		public int L111to120Multiplier	= 1800;		/* Leve 110 to Level 120 */
		public int L121to130Multiplier	= 2200;		/* Leve 121 to Level 130 */
		public int L131to140Multiplier	= 2600;		/* Leve 131 to Level 140 */
		public int L141to150Multiplier	= 3000;		/* Leve 141 to Level 150 */
		public int L151to160Multiplier	= 3500;		/* Leve 151 to Level 160 */
		public int L161to170Multiplier	= 4000;		/* Leve 161 to Level 170 */
		public int L171to180Multiplier	= 4500;		/* Leve 171 to Level 180 */
		public int L181to190Multiplier	= 5100;		/* Leve 181 to Level 190 */
		public int L191to200Multiplier	= 5700;		/* Leve 191 to Level 200 */
		
		#endregion
		
		#region Player Level Gump Config
		public bool LevelSheetStatResetButton	= false;	/* Master Stat Reset button - adds a button to playerlevelgump */
		public bool LevelSheetSkillResetButton	= false;	/* Master Skill Reset button - adds a button to playerlevelgump */
		
		/* Categories - set false to hide category */
		public bool Miscelaneous		= true; 		/* Enabled by Default */
		public bool Combat				= true; 		/* Enabled by Default */
		public bool TradeSkills			= true; 		/* Enabled by Default */
		public bool Magic				= true; 		/* Enabled by Default */
		public bool Wilderness			= true; 		/* Enabled by Default */
		public bool Thieving			= true; 		/* Enabled by Default */
		public bool Bard				= true; 		/* Enabled by Default */

        /* AOS & ML Skills */
        public bool Focus = false;
        public bool Necromancy = false;
        public bool Chivalry = false;
        public bool Bushido = false;
        public bool Ninjitsu = false;

        /* Stygian Abyss Skills */
        public bool Imbuing				= false;
		public bool Throwing			= false;
		public bool Mysticism			= false;
		
		/* Mondain's Legacy */
		public bool Spellweaving		= false;
		

		#endregion
		
		/* Being phased out */
        public const Active Curr = Active.Enviroment;
        // -<  Off | Classic | Enviroment | PvP >-

        public const View Cnfg = View.InName;
        // -< None | InName | BelowName >-
		/* Being phased out */
    }

    #region  !!! DO NOT CHANGE ANY THING BELOW THIS POINT !!!
	// levelHandler Raise Max Level and Levelcore scripts use these still, need to phase out.
    public class Off
    {
        public static bool Enabled { get { return Configured.Curr == Active.Off; } }
    }
    public class Cl
    {
        public static bool Enabled { get { return Configured.Curr == Active.Classic; } }
    }
    public class En
    {
        public static bool Enabled { get { return Configured.Curr == Active.Enviroment; } }
    }
    public class PvP
    {
        public static bool Enabled { get { return Configured.Curr == Active.PvP; } }
    }
    public class None
    {
        public static bool Enabled { get { return Configured.Cnfg == View.None; } }
    }
    public class InName
    {
        public static bool Enabled { get { return Configured.Cnfg == View.InName; } }
    }
    public class BelowName
    {
        public static bool Enabled { get { return Configured.Cnfg == View.BelowName; } }
    }

    public enum Active
    {
        Off,
        Classic,
        Enviroment,
        PvP
    }

    public enum View
    {
        None,
        InName,
        BelowName
    }
    #endregion
}
