using System;
using Server;

namespace Server.ACC.CSS.Systems.Druid
{
    public class DruidInitializer : BaseInitializer
    {
		public static void Configure()
		{
            Register( typeof( DruidLeafWhirlwindSpell ),    "Leaf Whirlwind",    "A gust of wind blows picking up magic leaves that memorize where they have come from, marking a rune for the caster.", "Spring Water; Petrafied Wood; Destroying Angel", "Mana: 25; Skill: 50", 2271,  5120, School.Druid );
            Register( typeof( DruidHollowReedSpell ),       "Hollow Reed",       "Increases both the Strength and the Intelligence of the Druid.",                                                       "Bloodmoss; Mandrake Root; Nightshade",           "Mana: 30; Skill: 30", 2255,  5120, School.Druid );
            Register( typeof( DruidPackOfBeastSpell ),      "Pack of Beasts",    "Summons a pack of beasts to fight for the Druid.  Spell length increases with skill.",                                 "Bloodmoss; Black Pearl; Petrafied Wood",         "Mana: 45; Skill: 40", 20491, 5120, School.Druid );
            Register( typeof( DruidSpringOfLifeSpell ),     "Spring of Life",    "Creates a magical spring that heals the Druid and their party.",                                                       "Spring Water",                                   "Mana: 40; Skill: 40", 2268,  5120, School.Druid );
            Register( typeof( DruidGraspingRootsSpell ),    "Grasping Roots",    "Summons roots from the ground to entangle a single target.",                                                           "Spring Water; Bloodmoss; Spider's Silk",         "Mana: 40; Skill: 40", 2293,  5120, School.Druid );
            Register( typeof( DruidBlendWithForestSpell ),  "Blend With Forest", "The Druid blends seamlessly with the background, becoming invisible to their foes.",                                   "Bloodmoss; Nightshade",                          "Mana: 60; Skill: 75", 2249,  5120, School.Druid );
            Register( typeof( DruidSwarmOfInsectsSpell ),   "Swarm of Instects", "Summons a swarm of insects that bite and sting the Druid's enemies.",                                                  "Garlic; Nightshade; DestroyingAngel",            "Mana: 70; Skill: 85", 2272,  5120, School.Druid );
            Register( typeof( DruidVolcanicEruptionSpell ), "Volcanic Eruption", "A blast of molten lava bursts from the ground, hitting every enemy nearby.",                                           "Sulfurous Ash; Destroying Angel",                "Mana: 85; Skill: 98", 2296,  5120, School.Druid );
            Register( typeof( DruidFamiliarSpell ),         "Summon Familiar",   "Summons a choice of diffrent familiars that can aid the druid.",                                                       "Mandrake Root; Spring Water; Petrafied Wood",    "Mana: 17; Skill: 30", 2295,  5120, School.Druid );
            Register( typeof( DruidStoneCircleSpell ),      "Stone Circle",      "Forms an impassable circle of stones, ideal for trapping enemies.",                                                    "Black Pearl; Ginseng; Spring Water",             "Mana: 45; Skill: 60", 2263,  5120, School.Druid );
            Register( typeof( DruidEnchantedGroveSpell ),   "Enchanted Grove",   "Creates a grove of trees to grow around the Druid, restoring vitality.",                                               "Mandrake Root; Petrafied Wood; Spring Water",    "Mana: 60; Skill: 95", 2280,  5120, School.Druid );
            Register( typeof( DruidLureStoneSpell ),        "Lure Stone",        "Creates a magical stone that calls all nearby animals to it.",                                                         "Black Pearl; Spring Water",                      "Mana: 30; Skill: 15", 2294,  5120, School.Druid );
            Register( typeof( DruidNaturesPassageSpell ),   "Nature's Passage",  "The Druid is turned into flower petals and carried on the wind to their destination.",                                 "Black Pearl; Bloodmoss; Mandrake Root",          "Mana: 10; Skill: 15", 2297,  5120, School.Druid );
            Register( typeof( DruidMushroomGatewaySpell ),  "Mushroom Gateway",  "A magical circle of mushrooms opens, allowing the Druid to step through it to another location.",                      "Black Pearl; Spring Water; Mandrake Root",       "Mana: 40; Skill: 70", 2291,  5120, School.Druid );
            Register( typeof( DruidRestorativeSoilSpell ),  "Restorative Soil",  "Saturates a patch of land with power, causing life giving mud to seep through.",                                       "Garlic; Ginseng; Spring Water",                  "Mana: 55; Skill: 89", 2298,  5120, School.Druid );
            Register( typeof( DruidShieldOfEarthSpell ),    "Shield of Earth",   "A quick-growning wall of foliage springs up at the bidding of the Druid.",                                             "Ginseng; Spring Water",                          "Mana: 15; Skill: 20", 2254,  5120, School.Druid );
		}
	}
}
