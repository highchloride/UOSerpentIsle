using System;
using Server;
using Server.Spells;
using Server.Spells.First;
using Server.Spells.Second;
using Server.Spells.Third;
using Server.Spells.Fourth;
using Server.Spells.Fifth;
using Server.Spells.Sixth;
using Server.Spells.Seventh;
using Server.Spells.Eighth;

namespace Server.ACC.CSS.Systems.Mage
{
    public class MageInitializer : BaseInitializer
    {
        public static void Configure()
        {
            //First Circle
            Register(typeof(ClumsySpell), "Clumsy",
                        "Temporarily reduces Target's Dexterity.",
                        "Bloodmoss; Nightshade",
                        "Mana: 4; Skill: .1",
                        2240, 9350, School.Magery);
            Register(typeof(CreateFoodSpell), "Create Food",
                        "Creates random food item in Caster's backpack.",
                        "Garlic; Ginseng; Mandrake Root",
                        "Mana: 4; Skill: .1",
                        2241, 9350, School.Magery);
            Register(typeof(FeeblemindSpell), "Feeblemind",
                        "Temporarily reduces Target's Intelligence.",
                        "Nightshade; Ginseng",
                        "Mana: 4; Skill: .1",
                        2242, 9350, School.Magery);
            Register(typeof(HealSpell), "Heal",
                        "Heals Target of a small amount of lost Hit Points.",
                        "Garlic; Ginseng; Spider's Silk",
                        "Mana: 4; Skill: .1",
                        2243, 9350, School.Magery);
            Register(typeof(MagicArrowSpell), "Magic Arrow",
                        "Shoots a magical arrow at Target, wich deals Fire damage.",
                        "Sulfurous Ash",
                        "Mana: 4; Skill: .1",
                        2244, 9350, School.Magery);
            Register(typeof(NightSightSpell), "Night Sight",
                        "Temporarily allows Target to see in darkness.",
                        "Spider's Silk; Sulfurous Ash",
                        "Mana: 4; Skill: .1",
                        2245, 9350, School.Magery);
            Register(typeof(ReactiveArmorSpell), "Reactive Armor",
                        "Increases the Caster's Physical Resistance while reducing their Elemental Resistances.  The Caster's Inscription skill adds a bonus to the amount of Physical Resist applied.  Active until spell is deactivated by re-casting the spell on the same Target.",
                        "Garlic; Spider's Silk; Sulfurous Ash",
                        "Mana: 4; Skill: .1",
                        2246, 9350, School.Magery);
            Register(typeof(WeakenSpell), "Weaken",
                        "Temporarily reduces Target's Strength.",
                        "Garlic; Nightshade",
                        "Mana: 4; Skill: .1",
                        2247, 9350, School.Magery);

            //Second Circle
            Register(typeof(AgilitySpell), "Agility",
                        "Temporarily increases Target's Dexterity.",
                        "Bloodmoss; Mandrake Root",
                        "Mana: 6; Skill: .1",
                        2248, 9350, School.Magery);
            Register(typeof(CunningSpell), "Cunning",
                        "Temporarily increases Target's Intelligence.",
                        "Nightshade; Mandrake Root",
                        "Mana: 6; Skill: .1",
                        2249, 9350, School.Magery);
            Register(typeof(CureSpell), "Cure",
                        "Attempts to neutralize poisons affecting the Target.",
                        "Garlic; Ginseng",
                        "Mana: 6; Skill: .1",
                        2250, 9350, School.Magery);
            Register(typeof(HarmSpell), "Harm",
                        "Affects the Target with a chilling effect, dealing Cold damage.  The closer the Target is to the Caster, the more damage is dealt.",
                        "Nightshade; Spider's Silk",
                        "Mana: 6; Skill: .1",
                        2251, 9350, School.Magery);
            Register(typeof(MagicTrapSpell), "Magic Trap",
                        "Places an explosive magic ward on a useable object that deals Fire damage to the next person to use the object.",
                        "Garlic; Spider's Silk; Sulfurous Ash",
                        "Mana: 6; Skill: .1",
                        2252, 9350, School.Magery);
            Register(typeof(RemoveTrapSpell), "Remove Trap",
                        "Deactivates a magical trap on a single object.",
                        "Bloodmoss; Sulfurous Ash",
                        "Mana: 6; Skill: .1",
                        2253, 9350, School.Magery);
            Register(typeof(ProtectionSpell), "Protection",
                        "Prevents the Target from having their spells disrupted, but lowers their Physical Resistances and ability to Resist Spells.  Active until the spell is deactivated by recasting on the same Target.",
                        "Garlic; Ginseng; Sulfurous Ash",
                        "Mana: 6; Skill: .1",
                        2254, 9350, School.Magery);
            Register(typeof(StrengthSpell), "Strength",
                        "Temporarily increases Target's Strength.",
                        "Mandrake Root; Nightshade",
                        "Mana: 6; Skill: .1",
                        2255, 9350, School.Magery);

            //Third Circle
            Register(typeof(BlessSpell), "Bless",
                        "Temporarily increases Target's Strength, Dexterity, and Intelligence.",
                        "Mandrake Root; Garlic",
                        "Mana: 9; Skill: 10.1",
                        2256, 9350, School.Magery);
            Register(typeof(FireballSpell), "Fireball",
                        "Shoots a ball of roiling flames at a Target, dealing Fire damage.",
                        "Black Pearl",
                        "Mana: 9; Skill: 10.1",
                        2257, 9350, School.Magery);
            Register(typeof(MagicLockSpell), "Magic Lock",
                        "Magically seals a container, blocking it from use until it is Magically Unlocked.",
                        "Bloodmoss; Garlic; Sulfurous Ash",
                        "Mana: 9; Skill: 10.1",
                        2258, 9350, School.Magery);
            Register(typeof(PoisonSpell), "Poison",
                        "The Target is afflicted by poison, of a strength determined by the Caster's Magery and Poison skills, and the distance from the Target.",
                        "Nightshade",
                        "Mana: 9; Skill: 10.1",
                        2259, 9350, School.Magery);
            Register(typeof(TelekinesisSpell), "Telekinesis",
                        "Allows the Caster to Use an item at a distance.",
                        "Bloodmoss; Mandrake Root",
                        "Mana: 9; Skill: 10.1",
                        2260, 9350, School.Magery);
            Register(typeof(TeleportSpell), "Teleport",
                        "Caster is transported to the Target Location.",
                        "Bloodmoss; Mandrake Root",
                        "Mana: 9; Skill: 10.1",
                        2261, 9350, School.Magery);
            Register(typeof(UnlockSpell), "Unlock",
                        "Unlocks a magical lock or low level normal lock.",
                        "Bloodmoss; Sulfurous Ash",
                        "Mana: 9; Skill: 10.1",
                        2262, 9350, School.Magery);
            Register(typeof(WallOfStoneSpell), "Wall of Stone",
                        "Creates a temporary wall of stone that blocks movement.",
                        "Bloodmoss; Garlic",
                        "Mana: 9; Skill: 10.1",
                        2263, 9350, School.Magery);

            //Fourth Circle
            Register(typeof(ArchCureSpell), "Arch Cure",
                        "Neutralizes poisons on all characters withing a small radius around the caster.",
                        "Garlic; Ginseng; Mandrake Root",
                        "Mana: 11; Skill: 24.1",
                        2264, 9350, School.Magery);
            Register(typeof(ArchProtectionSpell), "Arch Protection",
                        "Applies the Protection spell to all valid targets within a small radius around the Target Location.",
                        "Garlic; Ginseng; Mandrake Root; Sulfurous Ash",
                        "Mana: 11; Skill: 24.1",
                        2265, 9350, School.Magery);
            Register(typeof(CurseSpell), "Curse",
                        "Lowers the Strength, Dexterity, and Intelligence of the Target.  When cast during Player vs. Player combat, the spell also reduces the target's maximum resistance values.",
                        "Garlic; Nightshade; Sulfurous Ash",
                        "Mana: 11; Skill: 24.1",
                        2266, 9350, School.Magery);
            Register(typeof(FireFieldSpell), "Fire Field",
                        "Summons a wall of fire that deals Fire damage to all who walk through it.",
                        "Black Pearl; Spider's Silk; Sulfurous Ash",
                        "Mana: 11; Skill: 24.1",
                        2267, 9350, School.Magery);
            Register(typeof(GreaterHealSpell), "Greater Heal",
                        "Heals the target of a medium amount of lost Hit Points.",
                        "Garlic; Ginseng; Mandrake Root; Spider's Silk",
                        "Mana: 11; Skill: 24.1",
                        2268, 9350, School.Magery);
            Register(typeof(LightningSpell), "Lightning",
                        "Strikes the Target with a bolt of lightning, wich deals Energy damage.",
                        "Mandrake Root; Sulfurous Ash",
                        "Mana: 11; Skill: 24.1",
                        2269, 9350, School.Magery);
            Register(typeof(ManaDrainSpell), "Mana Drain",
                        "Temporarily removes an amount of mana from the Target, based on a comparison between the Caster's Evaluate Intelligence sill and the Target's Resist Spells skill.",
                        "Black Pearl; Mandrake Root; Spider's Silk",
                        "Mana: 11; Skill: 24.1",
                        2270, 9350, School.Magery);
            Register(typeof(RecallSpell), "Recall",
                        "Caster is transported to the location marked on the Target rune.  If a ship key is target, Caster is transported to the boat the key opens.",
                        "Black Pearl; Bloodmoss; Mandrake Root",
                        "Mana: 11; Skill: 24.1",
                        2271, 9350, School.Magery);

            //Fifth Circle
            Register(typeof(BladeSpiritsSpell), "Blade Spirits",
                        "Summons a whirling pillar of blades that selects a Target to attack based off of its combat strength and proximity.  The Blade Spirit disappears after a set amount of time. Requires 1 pet control slot.",
                        "Black Pearl; Mandrake Root; Nightshade",
                        "Mana: 14; Skill: 38.1",
                        2272, 9350, School.Magery);
            Register(typeof(DispelFieldSpell), "Dispel Field",
                        "Destroys one of the target Field spell.",
                        "Black Pearl; Garlic; Sulfurous Ash; Spider's Silk",
                        "Mana: 14; Skill: 38.1",
                        2273, 9350, School.Magery);
            Register(typeof(IncognitoSpell), "Incognito",
                        "Disuises the Caster with a randomly generated appearance and name.",
                        "Bloodmoss; Garlic; Nightshade",
                        "Mana: 14; Skill: 38.1",
                        2274, 9350, School.Magery);
            Register(typeof(MagicReflectSpell), "Magic Reflection",
                        "Lowers the caster's Physical resistances, while increasing their Elemental resistances.  Active until the spell is deactivated by recasting on the same Target.",
                        "Garlic; Mandrake Root; Spider's Silk",
                        "Mana: 14; Skill: 38.1",
                        2275, 9350, School.Magery);
            Register(typeof(MindBlastSpell), "Mind Blast",
                        "Deals Cold damage to the Target based off Caster's Magery and Intelligence.",
                        "Black Pearl; Mandrake Root; Nightshade; Sulfurous Ash",
                        "Mana: 14; Skill: 38.1",
                        2276, 9350, School.Magery);
            Register(typeof(ParalyzeSpell), "Paralyze",
                        "Immobilizes the Target for a brief amount of time.  The Target's Resisting Spells skill affects the Duration of the immobilization.",
                        "Garlic; Mandrake Root; Spider's Silk",
                        "Mana: 14; Skill: 38.1",
                        2277, 9350, School.Magery);
            Register(typeof(PoisonFieldSpell), "Poison Field",
                        "Conjures a wall of poisonous vapor that poisons anything that walks through it.",
                        "Black Pearl; Nightshade; Spider's Silk",
                        "Mana: 14; Skill: 38.1",
                        2278, 9350, School.Magery);
            Register(typeof(SummonCreatureSpell), "Summon Creature",
                        "Summons a random creature as a pet for a limited duration.  The strength of the summoned creature is based off of the Caster's Magery skill.",
                        "Bloodmoss; Mandrake Root; Spider's Silk",
                        "Mana: 14; Skill: 38.1",
                        2279, 9350, School.Magery);

            //Sixth Circle
            Register(typeof(DispelSpell), "Dispel",
                        "Attempts to Dispel a summoned creature, causing it to disapear from the world. The Dispel difficulty is affected by the Magery skill of the creature's owner.",
                        "Garlic; Mandrake Root; Sulfurous Ash",
                        "Mana: 20; Skill: 52.1",
                        2280, 9350, School.Magery);
            Register(typeof(EnergyBoltSpell), "Energy Bolt",
                        "Fires a bold of magical force at the Target dealing Energy damage.",
                        "Black Pearl; Nightshade",
                        "Mana: 20; Skill: 52.1",
                        2281, 9350, School.Magery);
            Register(typeof(ExplosionSpell), "Explosion",
                        "Strikes the Target with an explosive blast of energy, dealing Fire damage.",
                        "Bloodmoss; Mandrake Root",
                        "Mana: 20; Skill: 52.1",
                        2282, 9350, School.Magery);
            Register(typeof(InvisibilitySpell), "Invisibility",
                        "Temporarily causes the Target to become invisible.",
                        "Bloodmoss; Nightshade",
                        "Mana: 20; Skill: 52.1",
                        2283, 9350, School.Magery);
            Register(typeof(MarkSpell), "Mark",
                        "Binds a rune to the Caster's current Location.  The Mage's Recall spell and Paladin's Sacred Journey ability can both be used on the rune to teleport the Caster to the location of binding.",
                        "Black Pearl; Bloodmoss; Mandrake Root",
                        "Mana: 20; Skill: 52.1",
                        2284, 9350, School.Magery);
            Register(typeof(MassCurseSpell), "Mass Curse",
                        "Casts the Curse spell on a Target, and any creatures within a two tile radius.",
                        "Garlic; Mandrake Root; Nightshade; Sulfurous Ash",
                        "Mana: 20; Skill: 52.1",
                        2285, 9350, School.Magery);
            Register(typeof(ParalyzeFieldSpell), "Paralyze Field",
                        "Conjures a field of paralyzing energy that affects any creature that enters it with the effects of the Paralyze spell.",
                        "Black Pearl; Ginseng; Spider's Silk",
                        "Mana: 20; Skill: 52.1",
                        2286, 9350, School.Magery);
            Register(typeof(RevealSpell), "Reveal",
                        "Reveals the presence of any invisible or hiding creatures or players within a radius around the targeted tile.",
                        "Bloodmoss; Sulfurous Ash",
                        "Mana: 20; Skill: 52.1",
                        2287, 9350, School.Magery);

            //Seventh Circle
            Register(typeof(ChainLightningSpell), "Chain Lightning",
                        "Damages nearby targets with a series of lightning bolts that deal Energy damage.",
                        "Black Pearl; Bloodmoss; Mandrake Root; Sulfurous Ash",
                        "Mana: 40; Skill: 66.1",
                        2288, 9350, School.Magery);
            Register(typeof(EnergyFieldSpell), "Energy Field",
                        "Conjures a temporary field of energy on the ground at the Target Location that blocks all movement.",
                        "Black Pearl; Mandrake Root; Spider's Silk; Sulfurous Ash",
                        "Mana: 40; Skill: 66.1",
                        2289, 9350, School.Magery);
            Register(typeof(FlameStrikeSpell), "Flame Strike",
                        "Envelopes the target in a column of magical flame that deals Fire damage.",
                        "Spider's Silk; Sulfurous Ash",
                        "Mana: 40; Skill: 66.1",
                        2290, 9350, School.Magery);
            Register(typeof(GateTravelSpell), "Gate Travel",
                        "Targeting a rune marked with the Mark spell, opens a temporary portal to the rune's marked location.  The portal can be used by anyone to travel to that location.",
                        "Black Pearl; Mandrake Root; Sulfurous Ash",
                        "Mana: 40; Skill: 66.1",
                        2291, 9350, School.Magery);
            Register(typeof(ManaVampireSpell), "Mana Vampire",
                        "Drains mana from the Target and transfers it to the Caster.  The amount of mana drained is determined by a comparison between the Caster's Evaluate Intelligence skill and the Target's Resisting Spells skill.",
                        "Black Pearl; Bloodmoss; Mandrake Root; Spider's Silk",
                        "Mana: 40; Skill: 66.1",
                        2292, 9350, School.Magery);
            Register(typeof(MassDispelSpell), "Mass Dispel",
                        "Attempts to dispel any summoned creature within an eight tile radius.",
                        "Black Pearl; Garlic; Mandrake Root; Sulfurous Ash",
                        "Mana: 40; Skill: 66.1",
                        2293, 9350, School.Magery);
            Register(typeof(MeteorSwarmSpell), "Meteor Swarm",
                        "Summons a swarm of fiery meteors that strike all targets within a radius around the Target Location.  The total Fire damage dealt is split between all Targets of the spell.",
                        "Bloodmoss; Mandrake Root; Spider's Silk; Sulfurous Ash",
                        "Mana: 40; Skill: 66.1",
                        2294, 9350, School.Magery);
            Register(typeof(PolymorphSpell), "Polymorph",
                        "Temporarily transforms the Caster into a creature selected from a specified list. While polymorphed, other players will see the Caster as a criminal.",
                        "Bloodmoss; Mandrake Root; Spider's Silk",
                        "Mana: 40; Skill: 66.1",
                        2295, 9350, School.Magery);

            //Eighth Circle
            Register(typeof(EarthquakeSpell), "Earthquake",
                        "Causes a violent shaking of the earth that damages all nearby creatures and characters.",
                        "Bloodmoss; Ginseng; Mandrake Root; Sulfurous Ash",
                        "Mana: 50; Skill: 80.1",
                        2296, 9350, School.Magery);
            Register(typeof(EnergyVortexSpell), "Energy Vortex",
                        "Summons a spinning mass of energy that selects a Target to attack based off of its intelligence and proximity.  The Energy Vortex disappears after a set amount of time.  Requires 1 pet control slot.",
                        "Black Pearl; Bloodmoss; Mandrake Root; Nightshade",
                        "Mana: 50; Skill: 80.1",
                        2297, 9350, School.Magery);
            Register(typeof(ResurrectionSpell), "Resurrection",
                        "Resurrects a player's ghost.  Cannot be used on NPC's or monsters.",
                        "Bloodmoss; Garlic; Ginseng",
                        "Mana: 50; Skill: 80.1",
                        2298, 9350, School.Magery);
            Register(typeof(AirElementalSpell), "Air Elemental",
                        "An air elemental is summoned to serve the Caster.  Requires 2 pet control slots.",
                        "Bloodmoss; Mandrake Root; Spider's Silk",
                        "Mana: 50; Skill: 80.1",
                        2299, 9350, School.Magery);
            Register(typeof(SummonDaemonSpell), "Summon Daemon",
                        "A daemon is summoned to server the Caster.  Results in a large Karma loss for the Caster.  Requires 5 pet control slots.",
                        "Bloodmoss; Mandrake Root; Sulfurous Ash; Spider's Silk",
                        "Mana: 50; Skill: 80.1",
                        2300, 9350, School.Magery);
            Register(typeof(EarthElementalSpell), "Earth Elemental",
                        "An earth elemental is summoned to serve the Caster.  Requires 2 pet control slots.",
                        "Bloodmoss; Mandrake Root; Spider's Silk",
                        "Mana: 50; Skill: 80.1",
                        2301, 9350, School.Magery);
            Register(typeof(FireElementalSpell), "Fire Elemental",
                        "A fire elemental is summoned to serve the Caster.  Requires 4 pet control slots.",
                        "Bloodmoss; Mandrake Root; Spider's Silk; Sulfurous Ash",
                        "Mana: 50; Skill: 80.1",
                        2302, 9350, School.Magery);
            Register(typeof(WaterElementalSpell), "Water Elemental",
                        "A water elemental is summoned to serve the Caster.  Requires 3 pet control slots.",
                        "Bloodmoss; Mandrake Root; Spider's Silk",
                        "Mana: 50; Skill: 80.1",
                        2303, 9350, School.Magery);
        }
    }
}