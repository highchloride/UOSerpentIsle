using System;
using Server;
using Server.Spells.Necromancy;

namespace Server.ACC.CSS.Systems.Necromancy
{
	public class NecroInitializer : BaseInitializer
	{
		public static void Configure()
		{
			Register( typeof( AnimateDeadSpell ), "Animate Dead",
						"Animates the Targeted corpse, creating a mindless, wandering undead.  The strength of the risen undead is greatly modified by the Fame of the original creature.",
						"Grave Dust; Daemon Blood",
						"Mana: 23; Skill: 40",
						20480, 3600, School.Necro );
			Register( typeof( BloodOathSpell ), "Blood Oath",
						"Temporarily creates a dark pact between the Caster and the Target.  Any damage dealt by the Target to the Caster is increased, but the Target receives the same amount of damage.",
						"Daemon Blood",
						"Mana: 13; Skill: 20",
						20481, 3600, School.Necro );
			Register( typeof( CorpseSkinSpell ), "Corpse Skin",
						"Transmogrifies the flesh of the Target creature or player to resemble rotted corpse flesh, making them more vulnerable to Fire and Poison damage, but increasing their Resistance to Physical and Cold damage.",
						"Bat Wing; Grave Dust",
						"Mana: 11; Skill: 20",
						20482, 3600, School.Necro );
			Register( typeof( CurseWeaponSpell ), "Curse Weapon",
						"Temporarily imbues a weapon with a life draining effect.",
						"Pig Iron",
						"Mana: 7; Skill: 0",
						20483, 3600, School.Necro );
			Register( typeof( EvilOmenSpell ), "Evil Omen",
						"Curses the Target so that the next harmful event that affects them is magnified.",
						"Bat Wing; Nox Crystal",
						"Mana: 11; Skill: 20",
						20484, 3600, School.Necro );
			Register( typeof( HorrificBeastSpell ), "Horrific Beast",
						"Transforms the Caster into a horrific demonic beast, wich deals more damage, and recovers hit points faster, but can no longer cast any spells except for Necromancer Transformation spells.  Caster remains in this form until they recast the Horrific Beast spell.",
						"Bat Wing; Daemon Blood",
						"Mana: 11; Skill: 40",
						20485, 3600, School.Necro );
			Register( typeof( LichFormSpell ), "Lich Form",
						"Transforms the Caster into a lich, increasing their mana regeneration and some Resistances, while lowering their Fire Resist and slowly sapping their life.  Caster remains in this form until they recast the Lich Form spell.",
						"Grave Dust; Daemon Blood; Nox Crystal",
						"Mana: 23; Skill: 70",
						20486, 3600, School.Necro );
			Register( typeof( MindRotSpell ), "Mind Rot",
						"Attempts to place a curse on the Target that increases the mana cost of any spells they cast, for a duration bassed off a comparison between the Caster's Spirit Speak skill and the Target's Resisting Spells skill.",
						"Bat Wing; Daemon Blood; Pig Iron",
						"Mana: 17; Skill: 30",
						20487, 3600, School.Necro );
			Register( typeof( PainSpikeSpell ), "Pain Spike",
						"Temporarily causes intense physical pain to the Target, dealing Direct damage.  Once the spell wears off, if the Target is still alive, some of the Hit Points lost through the Pain Spike are restored.",
						"Grave Dust; Pig Iron",
						"Mana: 5; Skill: 20",
						20488, 3600, School.Necro );
			Register( typeof( PoisonStrikeSpell ), "Poison Strike",
						"Creates a blast of poisonous energy centered on the Target.  The main Target is inflicted with a large amount of Poison damage, and all valid Targets in a radius around the main Target are inflicted with a lesser effect.",
						"Nox Crystal",
						"Mana: 17; Skill: 50",
						20489, 3600, School.Necro );
			Register( typeof( StrangleSpell ), "Strangle",
						"Temporarily chokes off the air supply of the Target with poisonous fumes.  The Target is inflicted with Poison damage over time.  The amount of damage dealt each 'hit' is based off of the Caster's Spirit Speak skill and the Target's current Stamina.",
						"Daemon Blood; Nox Crystal",
						"Mana: 29; Skill: 65",
						20490, 3600, School.Necro );
			Register( typeof( SummonFamiliarSpell ), "Summon Familiar",
						"Allows the Caster to summon a Familiar from a selecetd list.  A Familiar will follow and fight with its owner, in addition to granting unique bonuses to the Caster, dependent upon the type of Familiar summoned.",
						"Bat Wing; Grave Dust; Daemon Blood",
						"Mana: 17; Skill: 30",
						20491, 3600, School.Necro );
			Register( typeof( VampiricEmbraceSpell ), "Vampiric Embrace",
						"Transforms the Caster into a powerful Vampire, wich increases his Stamina and Mana regeration while lowering his Fire Resistance.  Vampires also perform Life Drain when striking their enemies.  Caster remains in this form untill they recast the Vampiric Embrace spell.",
						"Bat Wing; Nox Crystal; Pig Iron",
						"Mana: 23; Skill: 99",
						20492, 3600, School.Necro );
			Register( typeof( VengefulSpiritSpell ), "Vengeful Spirit",
						"Summons a vile Spirit wich haunts the Target untill either the Target or the Spirit is dead.  Vengeful Spirits have the ability to track down their Target wherever they may travel.  A Spirit's strength is determined by the Necromancy and Spirit Speak skills of the Caster.",
						"Bat Wing; Grave Dust; Pig Iron",
						"Mana: 41; Skill: 80",
						20493, 3600, School.Necro );
			Register( typeof( WitherSpell ), "Wither",
						"Creates a withering frost around the Caster, wich deals Cold Damage to all valid targets in a radius.",
						"Grave Dust; Nox Crystal; Pig Iron",
						"Mana: 23; Skill: 60",
						20494, 3600, School.Necro );
			Register( typeof( WraithFormSpell ), "Wraith Form",
						"Transforms the Caster into an etheral Wraith, lowering some Elemental Resists, while increasing their physical resists.  Wraith Form also allows the caster to always succeed when using the Recall spell, and causes a Mana Drain effect when hitting enemies.  Caster remains in this form until they recast the Wraith Form spell.",
						"Nox Crystal; Pig Iron",
						"Mana: 17; Skill: 20",
						20495, 3600, School.Necro );
		}
	}
}