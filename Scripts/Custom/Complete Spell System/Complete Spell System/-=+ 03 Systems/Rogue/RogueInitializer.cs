using System;
using Server;

namespace Server.ACC.CSS.Systems.Rogue
{
    public class RogueList : BaseInitializer
    {
		public static void Configure()
		{
			Register( typeof( RogueFalseCoinSpell ),    "False Coin",   "The Rogue produces false gold with the trick of the hand.",          "Sulfurous Ash; Nightshade",                null, 20481, 5100, School.Rogue );
			Register( typeof( RogueCharmSpell ),        "Charm",        "The Rogue mesmerize's a target with his evil eyes.",                 "Black Pearl; Nightshade; Spider's Silk",   null, 21282, 5100, School.Rogue );
			Register( typeof( RogueSlyFoxSpell ),       "Sly Fox",      "The Rogue changes shape into a stealthly Sly Fox.",                  "Petrafied Wood; Nox Crystal; Nightshade",  null, 20491, 5100, School.Rogue );
			Register( typeof( RogueShadowSpell ),       "Shadow",       "The Rogue slips into the shadows.",                                  "Spider's Silk; Daemon Blood; Black Pearl", null, 21003, 5100, School.Rogue );
			Register( typeof( RogueIntimidationSpell ), "Intimidation", "The Rogue begins to look angry and mean at the loss of his skills.", null,                                       null, 20485, 5100, School.Rogue );
		}
	}
}
