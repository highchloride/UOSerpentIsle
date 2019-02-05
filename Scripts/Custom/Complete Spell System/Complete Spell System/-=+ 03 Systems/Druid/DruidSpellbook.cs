using System;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Druid
{
	public class DruidSpellbook : CSpellbook
	{
		public override School School{ get{ return School.Druid; } }

		[Constructable]
		public DruidSpellbook() : this( (ulong)0, CSSettings.FullSpellbooks )
		{
		}

		[Constructable]
		public DruidSpellbook( bool full ) : this( (ulong)0, full )
		{
		}

		[Constructable]
		public DruidSpellbook( ulong content, bool full ) : base( content, 0xEFA, full )
		{
			Hue = 0x48C;
			Name = "Tome of Nature";
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

			from.CloseGump( typeof( DruidSpellbookGump ) );
			from.SendGump( new DruidSpellbookGump( this ) );
		}

		public DruidSpellbook( Serial serial ) : base( serial )
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
