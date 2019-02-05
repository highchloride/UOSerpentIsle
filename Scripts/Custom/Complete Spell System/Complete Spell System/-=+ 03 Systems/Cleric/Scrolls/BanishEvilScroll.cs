using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Cleric
{
	public class ClericBanishEvilScroll : CSpellScroll
	{
		[Constructable]
		public ClericBanishEvilScroll() : this( 1 )
		{
		}

		[Constructable]
		public ClericBanishEvilScroll( int amount ) : base( typeof( ClericBanishEvilSpell ), 0xE39, amount )
		{
			Name = "Banish Evil";
			Hue = 0x1F0;
		}

		public ClericBanishEvilScroll( Serial serial ) : base( serial )
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
