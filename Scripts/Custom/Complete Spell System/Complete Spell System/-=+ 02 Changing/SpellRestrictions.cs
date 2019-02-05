using System;
using Server.Mobiles;
using Server.Items;

namespace Server.ACC.CSS
{
	public class SpellRestrictions
	{
		//Set Restricted to true if you want to check restrictions.
		public static bool UseRestrictions = true;

		/* All checks should be written in this method.  Return true if they can cast. */
		public static bool CheckRestrictions( Mobile caster, School school )
		{
			if( caster.AccessLevel >= AccessLevel.GameMaster )
				return true;

			return !(school == School.Invalid);
		}

		/* This method should be left alone */
		public static bool CheckRestrictions( Mobile caster, Type type )
		{
			Item item = caster.FindItemOnLayer( Layer.OneHanded );
			if( item is CSpellbook && CheckRestrictions( caster, ((CSpellbook)item).School ) )
				return true;

			Container pack = caster.Backpack;
			if( pack == null )
				return false;

			for( int i = 0; i < pack.Items.Count; i++ )
			{
				item = (Item)pack.Items[i];
				if( item is CSpellbook && CheckRestrictions( caster, ((CSpellbook)item).School ) )
					return true;
			}

			return false;
		}
	}
}