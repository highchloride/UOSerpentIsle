using System;
using Server;
using Server.Spells.Chivalry;

namespace Server.ACC.CSS.Systems.Chivalry
{
	public class ChivalryInitializer : BaseInitializer
	{
		public static void Configure()
		{
			Register( typeof( CleanseByFireSpell ), "Cleanse By Fire",
						"Cures the target of poisons, but causes the caster to be burned by fire damage.  The amount of fire damage is lessened if the caster has high Karma.",
						null,
						"Tithe: 10; Mana: 10; Skill: 5",
						20736, 9300, School.Chivalry );
			Register( typeof( CloseWoundsSpell ), "Close Wounds",
						"Heals the target of damage.  The caster's Karma affects the amount of damage healed.",
						null,
						"Tithe: 10; Mana: 10; Skill: 0",
						20737, 9300, School.Chivalry );
			Register( typeof( ConsecrateWeaponSpell ), "Consecrate Weapon",
						"Temporarily enchants the weapon the caster is currently wielding.  The type of damage the weapon inflicts when hitting a target will be converted to the target's worst Resistance type.  Duration of the effect is affected by the caster's Karma.",
						null,
						"Tithe: 10; Mana: 10; Skill: 15",
						20738, 9300, School.Chivalry );
			Register( typeof( DispelEvilSpell ), "Dispel Evil",
						"Attempts to dispel evil summoned creatures and cause other evil creatures to flee from combat.  Transformed Necromancers may also take Stamina and Mana Damage.  Caster's Karma and Chivalry, and Target's Fame or Necromancy affect Dispel Chance.",
						null,
						"Tithe: 10; Mana: 15; Skill: 35",
						20739, 9300, School.Chivalry );
			Register( typeof( DivineFurySpell ), "Divine Fury",
						"Temporarily increases the Paladin's swing speed, chance to hit, and damage dealt, while lowering the Paladin's defenses.  Upon casting, the Paladin's Stamina is also refreshed.  Duration of the spell is affected by the Caster's Karma.",
						null,
						"Tithe: 10; Mana: 15; Skill: 25",
						20740, 9300, School.Chivalry );
			Register( typeof( EnemyOfOneSpell ), "Enemy Of One",
						"The next target hit becomes the Paladin's Mortal Enemy.  All damage dealt to that creature type is increased, but the Paladin takes extra damage from all other creature types.  Mortal Enemy creature types will highlight Orange to the Paladin.  Duration of the spell is affected by the Caster's Karma.",
						null,
						"Tithe: 10; Mana: 20; Skill: 45",
						20741, 9300, School.Chivalry );
			Register( typeof( HolyLightSpell ), "Holy Light",
						"Deals energy damage to all valid targets in a radius around the caster.  Amount of damage dealt is affected by Caster's Karma.",
						null,
						"Tithe: 10; Mana: 15; Skill: 55",
						20742, 9300, School.Chivalry );
			Register( typeof( NobleSacrificeSpell ), "Noble Sacrifice",
						"Attempts to Ressurect, Cure, and Heal all targets in a radius around the caster.  If any target is successfully assisted, the Paladin's current Hit points, Mana, and Stamina are greatly reduced.  Amount of damage healed is affected by the Caster's Karma.",
						null,
						"Tithe: 30; Mana: 20; Skill: 65",
						20743, 9300, School.Chivalry );
			Register( typeof( RemoveCurseSpell ), "Remove Curse",
						"Attempts to remove all Curse effects from target.  Curses include Mage spells such as Clumsy, Weaken, Feebleming, and Paralyze, as well as all Necromancer curses.  Chance of removing curse is affected by the Caster's Karma.",
						null,
						"Tithe: 10; Mana: 20; Skill: 5",
						20744, 9300, School.Chivalry );
			Register( typeof( SacredJourneySpell ), "Sacred Journey",
						"Targeting a rune or ship key allows the caster to teleport to the marked location.  Caster may not flee from combat in this manner.",
						null,
						"Tithe: 15; Mana: 20; Skill: 15",
						20745, 9300, School.Chivalry );
        }
   }
}