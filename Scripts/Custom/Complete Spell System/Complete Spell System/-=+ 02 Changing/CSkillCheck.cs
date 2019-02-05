using System;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.ACC.CSS
{
	public class CSkillCheck
	{
		//Set this to false if you only want to use custom skills.
		public static bool UseDefaultSkills = true;

		//Return true if they cast.
		public static bool CheckSkill( Mobile from, Type type )
		{
			return true;
		}
	}
}