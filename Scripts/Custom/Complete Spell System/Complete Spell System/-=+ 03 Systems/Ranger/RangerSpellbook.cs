using System;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Ranger
{
	public class RangerSpellbook : CSpellbook
	{
		public override School School{ get{ return School.Ranger; } }

		[Constructable]
		public RangerSpellbook() : this( (ulong)0, CSSettings.FullSpellbooks )
		{
		}

		[Constructable]
		public RangerSpellbook( bool full ) : this( (ulong)0, full )
		{
		}

		[Constructable]
		public RangerSpellbook( ulong content, bool full ) : base( content, 0xEFA, full )
		{
			Hue = 2001;
			Name = "Ranger Survival Guide";
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

			from.CloseGump( typeof( RangerSpellbookGump ) );
			from.SendGump( new RangerSpellbookGump( this ) );
		}

		public RangerSpellbook( Serial serial ) : base( serial )
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
