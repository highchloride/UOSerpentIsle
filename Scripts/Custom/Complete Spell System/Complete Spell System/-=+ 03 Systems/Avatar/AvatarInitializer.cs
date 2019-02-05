using System;
using Server;

namespace Server.ACC.CSS.Systems.Avatar
{
	public class AvatarInitializer : BaseInitializer
	{
		public static void Configure()
		{
			Register( typeof( AvatarHeavensGateSpell ),   "Heaven's Gate",  "Allows the Paladin to open a heavenly portal to another location.",      null, "Tithe: 30; Skill: 80; Mana: 40", 2258, 9300, School.Avatar );
			Register( typeof( AvatarMarkOfGodsSpell ),    "Mark Of Gods",   "Casts down a powerful bolt of lightning to mark a rune for the Palaidn", null, "Tithe: 10; Skill: 20; Mana: 10", 2269, 9300, School.Avatar );
			Register( typeof( AvatarHeavenlyLightSpell ), "Heavenly Light", "Heaven lights the Paladin's way.",                                       null, "Tithe: 10; Skill: 20; Mana: 10", 2245, 9300, School.Avatar );
		}
	}
}