using System;
using Server;

namespace Server.ACC.CSS.Systems.Bard
{
    public class BardInitializer : BaseInitializer
    {
		public static void Configure()
		{
			Register( typeof( BardArmysPaeonSpell ),      "Army's Paeon",     "Regenerates your partys health slowly. [Area Effect]",       null, "Mana: 15; Skill: 60", 2243,  3000, School.Bard );
			Register( typeof( BardEnchantingEtudeSpell ), "Enchanting Etude", "Raises the intelligence of your party. [Area Effect]",       null, "Mana: 12; Skill: 70", 2242,  3000, School.Bard );
			Register( typeof( BardEnergyCarolSpell ),     "Energy Carol",     "Raises the energy resistance of your party. [Area Effect]",  null, "Mana: 12; Skill: 30", 2289,  3000, School.Bard );
			Register( typeof( BardEnergyThrenodySpell ),  "Energy Threnody",  "Lowers the energy resistance of your target.",               null, "Mana: 7;  Skill: 35", 2281,  3000, School.Bard );
			Register( typeof( BardFireCarolSpell ),       "Fire Carol",       "Raises the fire resistance of your party. [Area Effect]",    null, "Mana: 12; Skill: 30", 2267,  3000, School.Bard );
			Register( typeof( BardFireThrenodySpell ),    "Fire Threnody",    "Lowers the fire resistance of your target.",                 null, "Mana: 7;  Skill: 35", 2257,  3000, School.Bard );
			Register( typeof( BardFoeRequiemSpell ),      "Foe Requiem",      "Damages your target with a burst of sonic energy.",          null, "Mana: 18; Skill: 55", 2270,  3000, School.Bard );
			Register( typeof( BardIceCarolSpell ),        "Ice Carol",        "Raises the cold resistance of your party. [Area Effect]",    null, "Mana: 12; Skill: 30", 2286,  3000, School.Bard );
			Register( typeof( BardIceThrenodySpell ),     "Ice Threnody",     "Lowers the ice resistance of your target.",                  null, "Mana: 7;  Skill: 35", 2269,  3000, School.Bard );
			Register( typeof( BardKnightsMinneSpell ),    "Knight's Minne",   "Raises the physical resist of your party. [Area Effect]",    null, "Mana: 12; Skill: 45", 2273,  3000, School.Bard );
			Register( typeof( BardMagesBalladSpell ),     "Mage's Ballad",    "Regenerates your party's mana slowly. [Area Effect]",        null, "Mana: 15; Skill: 65", 2292,  3000, School.Bard );
			Register( typeof( BardMagicFinaleSpell ),     "Magic Finale",     "Dispels all summoned creatures around you. [Area Effect]",   null, "Mana: 15; Skill: 80", 2280,  3000, School.Bard );
			Register( typeof( BardPoisonCarolSpell ),     "Poison Carol",     "Raises the poison resistance of your party.  [Area Effect]", null, "Mana: 12; Skill: 30", 2285,  3000, School.Bard );
			Register( typeof( BardPoisonThrenodySpell ),  "Poison Threnody",  "Lowers the poison resistance of your target.",               null, "Mana: 7;  Skill: 35", 20488, 3000, School.Bard );
			Register( typeof( BardSheepfoeMamboSpell ),   "Sheepfoe Mambo",   "Raises the dexterity of your party. [Area Effect]",          null, "Mana: 12; Skill: 70", 2248,  3000, School.Bard );
			Register( typeof( BardSinewyEtudeSpell ),     "Sinewy Etude",     "Raises the strength of your party. [Area Effect]",           null, "Mana: 12; Skill: 70", 20741, 3000, School.Bard );
		}
	}
}
