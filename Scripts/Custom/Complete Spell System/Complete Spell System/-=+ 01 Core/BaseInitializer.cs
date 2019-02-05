using System;
using Server;

namespace Server.ACC.CSS
{
	public abstract class BaseInitializer
	{
		public static void Register( Type type, string name, string desc, string regs, string info, int icon, int back, School flag )
		{
			SpellInfoRegistry.Register( type, name, desc, regs, info, icon, back, flag );
		}
	}
}