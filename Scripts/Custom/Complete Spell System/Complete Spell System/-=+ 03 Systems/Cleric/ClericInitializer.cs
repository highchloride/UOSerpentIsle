using System;
using Server;

namespace Server.ACC.CSS.Systems.Cleric
{
	public class ClericInitializer : BaseInitializer
	{
		public static void Configure()
		{
			Register( typeof( ClericAngelicFaithSpell ),  "Angelic Faith",   "The caster calls upon the divine powers of the heavens to transform himself into a holy angel.  The caster gains better regeneration rates and increased stats and skills.", null, "Mana: 50; Skill: 80; Tithing: 100", 2295,  3500, School.Cleric );
			Register( typeof( ClericBanishEvilSpell ),    "Banish Evil",     "The caster calls forth a divine fire to banish his undead or demonic foe from the earth.",                                                                                   null, "Mana: 40; Skill: 60; Tithing: 30",  20739, 3500, School.Cleric );
			Register( typeof( ClericDampenSpiritSpell ),  "Dampen Spirit",   "The caster's enemy is slowly drained of his stamina, greatly hindering their ability to fight in combat or flee.",                                                           null, "Mana: 11; Skill: 35; Tithing: 15",  2270,  3500, School.Cleric );
			Register( typeof( ClericDivineFocusSpell ),   "Divine Focus",    "The caster's mind focuses on his divine faith increasing the effect of his prayers.  However, the caster becomes mentally fatigued much faster.",                            null, "Mana: 4;  Skill: 35; Tithing: 15",  2276,  3500, School.Cleric );
			Register( typeof( ClericHammerOfFaithSpell ), "Hammer Of Faith", "Summons forth a divine hammer of pure energy blessed with the ability to vanquish undead foes with greater efficiency.",                                                     null, "Mana: 14; Skill: 40; Tithing: 20",  20741, 3500, School.Cleric );
			Register( typeof( ClericPurgeSpell ),         "Purge",           "The target is cured of all poisons and has all negative stat curses removed.",                                                                                               null, "Mana: 6;  Skill: 10; Tithing: 5",   20744, 3500, School.Cleric );
			Register( typeof( ClericRestorationSpell ),   "Restoration",     "The caster's target is resurrected and fully healed and refreshed.",                                                                                                         null, "Mana: 50; Skill: 50; Tithing: 40",  2298,  3500, School.Cleric );
			Register( typeof( ClericSacredBoonSpell ),    "Sacred Boon",     "The caster's target is surrounded by a divine wind that heals small amounts of lost life over time.",                                                                        null, "Mana: 11; Skill: 25; Tithing: 15",  20742, 3500, School.Cleric );
			Register( typeof( ClericSacrificeSpell ),     "Sacrifice",       "The caster sacrifices himself for his allies. Whenever damaged, all party members are healed a small percent of the damage dealt. The caster still takes damage.",           null, "Mana: 4;  Skill: 5;  Tithing: 5",   20743, 3500, School.Cleric );
			Register( typeof( ClericSmiteSpell ),         "Smite",           "The caster calls to the heavens to send a deadly bolt of lightning to strike down his opponent.",                                                                            null, "Mana: 50; Skill: 80; Tithing: 60",  2269,  3500, School.Cleric );
			Register( typeof( ClericTouchOfLifeSpell ),   "Touch Of Life",   "The caster's target is healed by the heavens for a significant amount.",                                                                                                     null, "Mana: 9;  Skill: 30; Tithing: 10",  2243,  3500, School.Cleric );
			Register( typeof( ClericTrialByFireSpell ),   "Trial By Fire",   "The caster is surrounded by a divine flame that damages the caster's enemy when hit by a weapon.",                                                                           null, "Mana: 9;  Skill: 45; Tithing: 25",  20736, 3500, School.Cleric );
		}
	}
}