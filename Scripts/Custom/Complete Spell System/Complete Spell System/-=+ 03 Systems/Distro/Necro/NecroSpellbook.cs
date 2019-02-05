using System;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.Necromancy
{
	public class NecroSpellbook : CSpellbook
	{
		public override School School{ get{ return School.Necro; } }
/*		public override Item Dupe( int amount )
		{
			CSpellbook book = new NecroSpellbook();
			book.Content = this.Content;
			return base.Dupe( book, amount );
		}
*/
		[Constructable]
		public NecroSpellbook() : this( (ulong)0, CSSettings.FullSpellbooks )
		{
		}

		[Constructable]
		public NecroSpellbook( bool full ) : this( (ulong)0, full )
		{
		}

		[Constructable]
		public NecroSpellbook( ulong content, bool full ) : base( content, 0xEFA, full )
		{
			ItemID = 8787;
			Name = "Necro Spellbook";
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( from.AccessLevel == AccessLevel.Player )
			{
				Container pack = from.Backpack;
				if( !(Parent == from || (pack != null && Parent == pack)) )
				{
					from.SendMessage( "The spellbook must be in your backpack [and not in a container within] to open." );
					return;
				}
				else if( SpellRestrictions.UseRestrictions && !SpellRestrictions.CheckRestrictions( from, this.School ) )
				{
					return;
				}
			}

			from.CloseGump( typeof( NecroSpellbookGump ) );
			from.SendGump( new NecroSpellbookGump( this ) );
		}

		public NecroSpellbook( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}
