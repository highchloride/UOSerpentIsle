/*	System Configuration, changes to the system should be 
	made here instead of changes to the scripts
*/
using System;
using System.Collections;
using Server;

namespace Server
{
    public class ConfiguredPetXML
    {
		/* Advanced EXP is still handled by LevelCore, so changing it in the normal Configuration 
		File will affect pets! */
		/* ConfiguredPetXML cp = new ConfiguredPetXML(); */	
		/* XMLPetLevelAtt petxml = (XMLPetLevelAtt)XmlAttach.FindAttachment(pet, typeof(XMLPetLevelAtt)); */

		public bool EnabledLevelPets	= true;		/* Is the Pet Level System Activated? */	
		public bool MountedPetsGainEXP	= false;		/* If the pet is mounted does it still gain exp? */
		
		public bool PetAttackBonus		= true;		/* Attaches on logon - Needs XMLPetLevelAtt attached to pet for this to work! */
													/* Must enable attacks below! */
		public bool LevelBelowPet		= true;		/*	Pet level displayed below pet */
		public bool LoseExpLevelOnDeath	= false;	/*	False by default */
		public bool LoseStatOnDeath		= false;	/*	False by default - can disable to lose level only, not stat. */
		public int PetStatLossAmount	= 20;		/*	20 is 5%  - by default - LoseStatOnDeath must be true to apply */
		
		public bool PetLevelSheetPerma	= true;		/*	Pet Level Sheet unable to be dropped? */ 
		public bool PetExpGainFromKill 	= true;		/*	Does pets gain EXP from Kills, must have attachment!*/
		public int StartMaxLvl 			= 100;		/*	Max Level Without Scroll Boost */
		public int MaxLevelWithScroll	= 150;		/*	What is the highest level reachable without scrolls? */
		public int EndMaxLvl 			= 200;	
		public int SkillsTotal			= 1200000;	/* If pet is equal to or exceeds they will not
													gain skill points on level up */
		public int MaxStatPoints		= 15000;	/* If pet is equal to or exceeds they will not
													gain stat points on level up */
			
        public bool RefreshOnLevel		= true;		//Sets players hits, stam, and mana to max on level.
		
		public bool NotifyOnPetExpGain	= true;		/*	Will tell the pet owner if the pet gains exp */
		public bool NotifyOnPetlevelUp	= true;		/*	Will tell the pet owner if the pet gains a level */

		#region Attack Bonuses
		/* Chance Key:  0.01 is 1% 
						0.05 is 5%
						0.10 is 10%
						0.5	 is 50%  
		Using general add and subtraction and conversion from decimal to percent 
		you can choose a lot of possible chance variables. 
		*/
		/* THESE TOGGLES OVERRIDE THE TOGGLES ON THE ACTUAL PETS!! */
		public bool		PetSpeak			= false;	/*	Does the pet make random emotes? no sounds */
		public double	PetSpeakChance		= 0.05;		/*	Activation Chance on movement - default 0.05	*/
		
		public bool		SuperHeal			= true;	/*	Outside of the pets normal healing ability, this grants additional health periodically. */
		public bool		PetShouldBeBonded	= true;		/*	True by Default.  Bonded pets only benefit */
		public int		SuperHealReq		= 60;		/*	Must be this Level or Higher to use */
		public double	SuperHealChance		= 0.01;		/*	Activation Chance on movement - default 0.01	*/
		
		public bool		TelePortToTarget	= true;	/*	This teleports the pet to far away targets */
		public int		TelePortToTargetReq	= 60;		/*	Must be this Level or Higher to use */
		public double	TelePortToTarChance	= 0.10;		/*	Activation Chance on movement - default 0.10	*/
		
		public bool		MassProvokeToAtt	= true;	/*	This will allow the pet to provoke untamed creatures to its target */
		public int		MassProvokeToAttReq	= 60;		/*	Must be this Level or Higher to use */
		public double	MassProvokeChance	= 0.10;		/*	Activation Chance on movement - default 0.10	*/
		
		public bool		MassPeaceArea		= true;	/*	This will allow the pet to mass peace an area around it, stopping war */
		public int		MassPeaceReq		= 60;		/*	Must be this Level or Higher to use */
		public double	MassPeaceChance		= 0.10;		/*	Activation Chance on movement - default 0.10	*/
		
		public bool		BlessedPower		= true;	/*	This will allow the pet to get temporary stat boost */
		public int		BlessedPowerReq		= 60;		/*	Must be this Level or Higher to use */
		public double	BlessedPowerChance	= 0.10;		/*	Activation Chance on movement - default 0.10	*/
		
		public bool		AreaFireBlast		= true;		/*	This will allow the pet to blast area with fire */
		public int		AreaFireBlastReq	= 60;		/*	Must be this Level or Higher to use */
		public double	AreaFireBlastChance	= 0.3;		/*	Activation Chance on Attack - default 0.3	*/
		
		public bool		AreaIceBlast		= true;	/*	This will allow the pet to blast area with Ice */
		public int		AreaIceBlastReq		= 60;		/*	Must be this Level or Higher to use */
		public double	AreaIceBlastChance	= 0.3;		/*	Activation Chance on Attack - default 0.3	*/
		
		public bool		AreaAirBlast		= true;	/*	This will allow the pet to blast area with Ice */
		public int		AreaAirBlastReq		= 60;		/*	Must be this Level or Higher to use */
		public double	AreaAirBlastChance	= 0.3;		/*	Activation Chance on Attack - default 0.3	*/

		#endregion
		
		#region Pet Aura effects
		/* AuraStatBoost may require you to increase the stat cap on server */ 
		/* AuraStatBoost increases benefits for higher levels - PlayerEXP Must be enabled! */
		public bool		AuraStatBoost		= true;			/*	This aura grants a stat boost to the pet master */
		public int		AuraStatBoostReq	= 60;			/*	Must be this Level or Higher to use */
		#endregion
		
        #region Mount Options
		public bool EnableLvLMountChkonPetAtt = true;
		/* Turn this to false to not have the Mounts levels checked. */
        //For Individual Levels:
        public int Beetle = 1;
        public int DesertOstard = 1;
        public int FireSteed = 1;
        public int ForestOstard = 1;
        public int FrenziedOstard = 1;
        public int HellSteed = 1;
        public int Hiryu = 1;
        public int Horse = 1;
        public int Kirin = 1;
        public int LesserHiryu = 1;
        public int NightMare = 1;
		public int Nightmare = 1;
        public int RidableLlama = 1;
        public int Ridgeback = 1;
        public int SavageRidgeback = 1;
        public int ScaledSwampDragon = 1;
        public int SeaHorse = 1;
        public int SilverSteed = 1;
        public int SkeletalMount = 1;
        public int SwampDragon = 1;
        public int Unicorn = 1;
		public int Reptalon = 1;
		public int WildTiger = 1;
		public int Windrunner = 1;
		public int Lasher = 1;
		public int Eowmu = 1;
		public int DreadWarhorse = 1;
		public int CuSidhe = 1;
        #endregion
		
		/* How many skill points awarded per level.
			Scenario: If turning level 18, below20 applies*/
		
		public int Below20	= 4;		/*	Below Level 20	*/
		public int Below40	= 4;		/*	Below Level 40	*/
		public int Below60	= 4;		/*	Below Level 60	*/
		public int Below70	= 4;		/*	Below Level 70	*/
		public int Below80	= 4;		/*	Below Level 80	*/
		public int Below90	= 4;		/*	Below Level 90	*/
		public int Below100	= 4;		/*	Below Level 100	*/
		public int Below110	= 4;		/*	Below Level 110	*/
		public int Below120	= 4;		/*	Below Level 120	*/
		public int Below130	= 4;		/*	Below Level 130	*/
		public int Below140	= 4;		/*	Below Level 140	*/
		public int Below150	= 4;		/*	Below Level 150	*/
		public int Below160	= 4;		/*	Below Level 160	*/
		public int Below170	= 4;		/*	Below Level 170	*/
		public int Below180	= 4;		/*	Below Level 180	*/
		public int Below190	= 4;		/*	Below Level 190	*/
		public int Below200	= 4;		/*	Below Level 200	*/

		/* How many stat points to be awarded per level */
		public int Below20Stat	= 3;		/*	Below Level 20	*/
		public int Below40Stat	= 3;		/*	Below Level 40	*/
		public int Below60Stat	= 3;		/*	Below Level 60	*/
		public int Below70Stat	= 3;		/*	Below Level 70	*/
		public int Below80Stat	= 3;		/*	Below Level 80	*/
		public int Below90Stat	= 3;		/*	Below Level 90	*/
		public int Below100Stat	= 3;		/*	Below Level 100	*/
		public int Below110Stat	= 3;		/*	Below Level 110	*/
		public int Below120Stat	= 3;		/*	Below Level 120	*/
		public int Below130Stat	= 3;		/*	Below Level 130	*/
		public int Below140Stat	= 3;		/*	Below Level 140	*/
		public int Below150Stat	= 3;		/*	Below Level 150	*/
		public int Below160Stat	= 3;		/*	Below Level 160	*/
		public int Below170Stat	= 3;		/*	Below Level 170	*/
		public int Below180Stat	= 3;		/*	Below Level 180	*/
		public int Below190Stat	= 3;		/*	Below Level 190	*/
		public int Below200Stat	= 3;		/*	Below Level 200	*/
	}
	
}
