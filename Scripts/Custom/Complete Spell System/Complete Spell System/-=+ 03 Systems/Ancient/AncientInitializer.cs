using System;
using Server;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientInitializer : BaseInitializer
    {
        public static void Configure()
        {
            Register(typeof(AncientFireworksSpell), "Fireworks", "Creates an impressive display of multi-colored moving lights.", "Sulfurous Ash", null, 2282, 9270, School.Ancient);
            Register(typeof(AncientGlimmerSpell), "Glimmer", "Creates a small light source that lasts for a short period of time.", "Bloodmoss", null, 21280, 9270, School.Ancient);
            Register(typeof(AncientAwakenSpell), "Awaken", "Awakens one sleeping or unconscious creature.", "Sulfurous Ash", null, 2242, 9270, School.Ancient);
            Register(typeof(AncientThunderSpell), "Thunder", "Causes a single thunderclap to be heard, as if a terrible storm is imminent.", "Sulfurous Ash", null, 21540, 9270, School.Ancient);
            Register(typeof(AncientWeatherSpell), "Weather", "Can create a storm or cause an existing storm to stop.", "Sulfurous Ash", null, 23004, 9270, School.Ancient);
            Register(typeof(AncientIgniteSpell), "Ignite", "Generates a tiny missile of sparks that can ignite flammable material.", "Black Pearl", null, 2257, 9270, School.Ancient);
            Register(typeof(AncientDouseSpell), "Douse", "Extinguishes any small, non-magical fire.", "Bloodmoss", null, 2256, 9270, School.Ancient);
            Register(typeof(AncientLocateSpell), "Locate", "Reveals the position of the mage, even when underground.", "Nightshade", null, 2260, 9270, School.Ancient);
            Register(typeof(AncientAwakenAllSpell), "Awaken All", "Awakens all unconscious members of the mage's party.", "Garlic, Ginseng", null, 2292, 9270, School.Ancient);
            Register(typeof(AncientDetectTrapSpell), "Detect Trap", "Allows the mage to see any nearby traps.", "Bloodmoss, Sulfurous Ash", null, 20496, 9270, School.Ancient);
            Register(typeof(AncientGreatDouseSpell), "Great Douse", "A more potent version of the spell Douse.", "Garlic, Spider's Silk", null, 20999, 9270, School.Ancient);
            Register(typeof(AncientGreatIgniteSpell), "Great Ignite", "A more potent version of the spell Ignite.", "Sulfurous Ash, Spider's Silk", null, 21256, 9270, School.Ancient);
            Register(typeof(AncientEnchantSpell), "Enchant", "Causes a ranged weapon to become enchanted and glow blue. Enchanted weapons will always hit their target.", "Black Pearl, Mandrake Root", null, 23003, 9270, School.Ancient);
            Register(typeof(AncientFalseCoinSpell), "False Coin", "When cast upon any coin, this spell creates five duplicate coins.", "Nightshade, Sulfurous Ash", null, 20481, 9270, School.Ancient);
            Register(typeof(AncientGreatLightSpell), "Great Light", "A more potent version of Nightsight, and has a substantially longer duration.", "Sulfurous Ash, Mandrake Root", null, 2245, 9270, School.Ancient);
            Register(typeof(AncientDestroyTrapSpell), "Destroy Trap", "Destroys any one specific trap upon which it is cast.", "Bloodmoss, Sulfurous Ash", null, 20994, 9270, School.Ancient);
            Register(typeof(AncientSleepSpell), "Sleep", "Causes the enchanted person to fall asleep.", "Spider's Silk, Nightshade, Black Pearl", null, 2277, 9270, School.Ancient);
            Register(typeof(AncientSwarmSpell), "Swarm", "Summons swarms of insects to attack the enemies of the mage from all directions.", "Nightshade, Mandrake Root, Bloodmoss", null, 20740, 9270, School.Ancient);
            Register(typeof(AncientPeerSpell), "Peer", "Enables the mage's vision to leave his body and scout the area.", "Mandrake Root, Nightshade", null, 2270, 9270, School.Ancient);
            Register(typeof(AncientSeanceSpell), "Seance", "Enables the mage to move as a ghost.", "Bloodmoss, Spider's Silk, Mandrake Root, Nightshade, Sulfurous Ash", null, 23014, 9270, School.Ancient);
            Register(typeof(AncientCharmSpell), "Charm", "Can be used either to control an enemy or creature, or to free a charmed one.", "Black Pearl, Nightshade, Spider's Silk", null, 23015, 9270, School.Ancient);
            Register(typeof(AncientDanceSpell), "Dance", "Makes everyone in sight (except the mage and his party) start to dance.", "Garlic, Bloodmoss, Mandrake Root", null, 23005, 9270, School.Ancient);
            Register(typeof(AncientMassSleepSpell), "Mass Sleep", "Puts to sleep a group of targets.", "Nightshade, Ginseng, Spider's Silk", null, 2276, 9270, School.Ancient);
            Register(typeof(AncientCloneSpell), "Clone", "Creates an exact duplicate of any mortal creature, who will then fight for the mage.", "Sulfurous Ash, Spider's Silk, Bloodmoss, Ginseng, Nightshade,Mandrake Rook", null, 2261, 9270, School.Ancient);
            Register(typeof(AncientCauseFearSpell), "Cause Fear", "Nothing is known about this spell.", "Garlic, Nightshade, Mandrake Root", null, 2286, 9270, School.Ancient);
            Register(typeof(AncientFireRingSpell), "Fire Ring", "Creates a ring of fire that will encircle the mage's target.", "Black Pearl, Spider's Silk, Sulfurous Ash, Mandrake Root", null, 2302, 9270, School.Ancient);
            Register(typeof(AncientTremorSpell), "Tremor", "Creates violent tremors in the earth that will cause the mage's enemies to tremble frantically.", "Bloodmoss, Mandrake Root, Sulfurous Ash", null, 2296, 9270, School.Ancient);
            Register(typeof(AncientSleepFieldSpell), "Sleep Field", "Creates a thick wall of energy field where the mage desires. All who enter this energy field will fall asleep.", "Black Pearl, Ginseng, Spider's Silk", null, 2283, 9270, School.Ancient);
            Register(typeof(AncientMassMightSpell), "Mass Might", "Casts Bless on everyone in the mage's party.", "Black Pearl, Mandrake Root, Ginseng", null, 23001, 9270, School.Ancient);
            Register(typeof(AncientMassCharmSpell), "Mass Charm", "Similar to Charm, but it affects more powerful monsters, based on the mage's intellect.", "Black Pearl, Nightshade, Spider's Silk, Mandrake Root", null, 21000, 9270, School.Ancient);
            Register(typeof(AncientInvisibilityAllSpell), "Invisibility All", "Casts Invisibility upon the mage and everyone in his party.", "Nightshade, Bloodmoss, Black Pearl, Mandrake Root", null, 23012, 9270, School.Ancient);
            Register(typeof(AncientDeathVortexSpell), "Death Vortex", "Creates a swirling black vortex at the point the mage designates, which will thereafter move at random.", "Bloodmoss, Sulfurous Ash, Mandrake Root, Nightshade", null, 21541, 9270, School.Ancient);
            Register(typeof(AncientMassDeathSpell), "Mass Death", "Kills everything within the mage's sight", "Bloodmoss, Ginseng, Garlic, Mandrake Root, Nightshade", null, 2285, 9270, School.Ancient);
            Register(typeof(AncientArmageddonSpell), "Armageddon", "Believed to be so powerful that it may be capable of destroying all known living things in the entire world.", "Bloodmoss, Black Pearl, Mandrake Root, Nightshade, Garlic, Ginseng, Sulfurous Ash, Spider's Silk", null, 2300, 9270, School.Ancient);
        }
    }
}
