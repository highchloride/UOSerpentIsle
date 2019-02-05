using System;
using Server;

namespace Server.ACC.CSS.Systems.Ranger
{
    public class RangerList : BaseInitializer
    {
		public static void Configure()
		{
			Register( typeof( RangerHuntersAimSpell ),    "Hunter's Aim",     "Increases the Rangers archery, and tactics for a short period of time.",                                                             "Nightshade; Spring Water; Bloodmoss",            "Mana: 25; Skill: 50", 2244,  5054, School.Ranger );
			Register( typeof( RangerPhoenixFlightSpell ), "Phoenix Flight",   "Calls Forth a Phoenix who will carry you to the location of your choice.",                                                           "Sulfurous Ash; Petrafied Wood",                  "Mana: 10; Skill: 15", 20736, 5054, School.Ranger );
			Register( typeof( RangerFamiliarSpell ),      "Animal Companion", "The Ranger summons an animal companion (baised on skill level) to aid him in his quests.",                                           "Destroying Angel; Spring Water; Petrafied Wood", "Mana: 17; Skill: 30", 20491, 5054, School.Ranger );
			Register( typeof( RangerFireBowSpell ),       "Fire Bow",         "The Ranger uses his knowlage of archery and hunting, to craft a temparary fire elemental bow, that last for a short duration.",      "Kindling; Sulfurous Ash",                        "Mana: 30; Skill: 85", 2257,  5054, School.Ranger );
			Register( typeof( RangerIceBowSpell ),        "Ice Bow",          "The Ranger uses his knowlage of archery and hunting, to craft a temparary ice elemental bow, that last for a short duration.",       "Kindling; Spring Water",                         "Mana: 30; Skill: 85", 21001, 5054, School.Ranger );
			Register( typeof( RangerLightningBowSpell ),  "Lightning Bow",    "The Ranger uses his knowlage of archery and hunting, to craft a temparary lightning elemental bow, that last for a short duration.", "Kindling; Black Pearl",                          "Mana: 30; Skill: 90", 2281,  5054, School.Ranger );
			Register( typeof( RangerNoxBowSpell ),        "Nox Bow",          "The Ranger uses his knowlage of archery and hunting, to craft a temparary poison elemental bow, that last for a short duration.",    "Kindling; Nightshade",                           "Mana: 30; Skill: 95", 20488, 5054, School.Ranger );
			Register( typeof( RangerSummonMountSpell ),   "Call Mount",       "The Ranger calls to the Wilds, summoning a speedy mount to his side.",                                                               "Spring Water; Black Pearl; Sulfurous Ash",       "Mana: 15; Skill: 30", 20745, 5054, School.Ranger );
		}
	}
}
