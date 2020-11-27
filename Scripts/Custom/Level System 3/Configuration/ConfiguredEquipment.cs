using System;
using System.Collections;
using Server;

namespace Server
{
	/*	Tried only using skills that are not passive or used via weapon
		default gain amount is 10 */
		/* usage: ConfiguredEquipment cfe = new ConfiguredEquipment(); */
    public class ConfiguredEquipment
	{

		#region Equipment - USE ONE OR THE OTHER! USING BOTH AT THE SAME TIME CAN CAUSE PROBLEMS!  YOU HAVE BEEN WARNED!
		public bool AttachOnEquipCreate = false;	/* Attach Level Requirement Item to all weapons and armor!
														Edit LevelEquipXML.cs to configure */
		/* The above system allows you to specify equipment or groups of equipment instead of every peice of equipment like
		   the below system.*/
														
		/*	USE ONE OR THE OTHER! USING BOTH AT THE SAME TIME CAN CAUSE PROBLEMS!  YOU HAVE BEEN WARNED! 
			BY USING THE BELOW OPTION, CREATING UNIQUE EQUIPMENT TO REQUIRE SPECIFIV LEVELS MUST BE MARKED TO BE INGORED WHEN BEING ADDED */
		
		/* The rest of the toggles in this region are for the secondary type of equipment levels.  This is for dynaic
		usage.  This means that the script compares the intensity of the equipment and decides if a player can equip it
		based on their level! */
		
		public bool AttachOnEquipCreateDynamicSystem	=	false;				/*	This will place the attachment on ALL equipment. 
																					Clothing/jewels/armor/weapons/ EVEN RARES */
		public bool ActivateDynamicLevelSystem			=	true;
		/* Even if the attachments are on the equipment, if this is set to false, the attachments do nothing! 
		------------>  TURN THIS ON TO USE THE SYSTEM!*/
																					
		public string NameOfBattleRatingStat			=	"Battle Rating:";		
		public string RequiredLevelMouseOver			=	"Required Level:";		
		/*	Change this if you do not want it call it Battle Rating. This applies to all equipment! */
		
		/* Each Equipment Variable will have 10 thresholds of configuration, you can add more if you like however I felt 10 is a decent balance. */
		
		/* In the below default settings anything below level intensity of Level 1 (100 in this case) doesn't require a level to equip! */
		
		/* Explanation: So lets say you have a helmet with a battle rating of 101, then that means the player Must have a level of '5'
		   or higher to equip the helmet.  Be sure to choose levels that make sense for the intesity you are picking! */
		
		/* BaseArmor - Armor Properties */
		/* 									Levels		Threshhold											intensity */
		public int ArmorRequiredLevel1		=	5;		/*1*/		public int ArmorRequiredLevel1Intensity = 100;
		public int ArmorRequiredLevel2		=	10;		/*2*/		public int ArmorRequiredLevel2Intensity = 250;
		public int ArmorRequiredLevel3		=	20;		/*3*/		public int ArmorRequiredLevel3Intensity = 450;
		public int ArmorRequiredLevel4		=	25;		/*4*/		public int ArmorRequiredLevel4Intensity = 650;
		public int ArmorRequiredLevel5		=	35;		/*5*/		public int ArmorRequiredLevel5Intensity = 750;
		public int ArmorRequiredLevel6		=	40;		/*6*/		public int ArmorRequiredLevel6Intensity = 850;
		public int ArmorRequiredLevel7		=	45;		/*7*/		public int ArmorRequiredLevel7Intensity = 950;
		public int ArmorRequiredLevel8		=	50;		/*8*/		public int ArmorRequiredLevel8Intensity = 1050;
		public int ArmorRequiredLevel9		=	55;		/*9*/		public int ArmorRequiredLevel9Intensity = 1500;
		public int ArmorRequiredLevel10		=	60;		/*10*/		public int ArmorRequiredLevel10Intensity = 1800;
		
		/* BaseWeapon - Weapon Properties */
		/* 									Levels		Threshhold											intensity */
		public int WeaponRequiredLevel1		=	5;		/*1*/		public int WeaponRequiredLevel1Intensity = 100;
		public int WeaponRequiredLevel2		=	10;		/*2*/		public int WeaponRequiredLevel2Intensity = 250;
		public int WeaponRequiredLevel3		=	20;		/*3*/		public int WeaponRequiredLevel3Intensity = 450;
		public int WeaponRequiredLevel4		=	25;		/*4*/		public int WeaponRequiredLevel4Intensity = 650;
		public int WeaponRequiredLevel5		=	35;		/*5*/		public int WeaponRequiredLevel5Intensity = 750;
		public int WeaponRequiredLevel6		=	40;		/*6*/		public int WeaponRequiredLevel6Intensity = 850;
		public int WeaponRequiredLevel7		=	45;		/*7*/		public int WeaponRequiredLevel7Intensity = 950;
		public int WeaponRequiredLevel8		=	50;		/*8*/		public int WeaponRequiredLevel8Intensity = 1050;
		public int WeaponRequiredLevel9		=	55;		/*9*/		public int WeaponRequiredLevel9Intensity = 1500;
		public int WeaponRequiredLevel10	=	60;		/*10*/		public int WeaponRequiredLevel10Intensity = 1800;
		#endregion
		
		/* BaseClothing - Clothing Properties */
		/* 									Levels		Threshhold											intensity */
		public int ClothRequiredLevel1		=	5;		/*1*/		public int ClothRequiredLevel1Intensity = 100;
		public int ClothRequiredLevel2		=	10;		/*2*/		public int ClothRequiredLevel2Intensity = 250;
		public int ClothRequiredLevel3		=	20;		/*3*/		public int ClothRequiredLevel3Intensity = 450;
		public int ClothRequiredLevel4		=	25;		/*4*/		public int ClothRequiredLevel4Intensity = 650;
		public int ClothRequiredLevel5		=	35;		/*5*/		public int ClothRequiredLevel5Intensity = 750;
		public int ClothRequiredLevel6		=	40;		/*6*/		public int ClothRequiredLevel6Intensity = 850;
		public int ClothRequiredLevel7		=	45;		/*7*/		public int ClothRequiredLevel7Intensity = 950;
		public int ClothRequiredLevel8		=	50;		/*8*/		public int ClothRequiredLevel8Intensity = 1050;
		public int ClothRequiredLevel9		=	55;		/*9*/		public int ClothRequiredLevel9Intensity = 1500;
		public int ClothRequiredLevel10		=	60;		/*10*/		public int ClothRequiredLevel10Intensity = 1800;
		
		/* BaseJewel - Jewel Properties */
		/* 									Levels		Threshhold											intensity */
		public int JewelRequiredLevel1		=	5;		/*1*/		public int JewelRequiredLevel1Intensity = 100;
		public int JewelRequiredLevel2		=	10;		/*2*/		public int JewelRequiredLevel2Intensity = 250;
		public int JewelRequiredLevel3		=	20;		/*3*/		public int JewelRequiredLevel3Intensity = 450;
		public int JewelRequiredLevel4		=	25;		/*4*/		public int JewelRequiredLevel4Intensity = 650;
		public int JewelRequiredLevel5		=	35;		/*5*/		public int JewelRequiredLevel5Intensity = 750;
		public int JewelRequiredLevel6		=	40;		/*6*/		public int JewelRequiredLevel6Intensity = 850;
		public int JewelRequiredLevel7		=	45;		/*7*/		public int JewelRequiredLevel7Intensity = 950;
		public int JewelRequiredLevel8		=	50;		/*8*/		public int JewelRequiredLevel8Intensity = 1050;
		public int JewelRequiredLevel9		=	55;		/*9*/		public int JewelRequiredLevel9Intensity = 1500;
		public int JewelRequiredLevel10		=	60;		/*10*/		public int JewelRequiredLevel10Intensity = 1800;
	}
 
}