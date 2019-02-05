using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Cleric
{
	public class ClericDampenSpiritScroll : CSpellScroll
	{
		[Constructable]
		public ClericDampenSpiritScroll() : this( 1 )
		{
		}

		[Constructable]
		public ClericDampenSpiritScroll( int amount ) : base( typeof( ClericDampenSpiritSpell ), 0xE39, amount )
		{
			Name = "Dampen Spirit";
			Hue = 0x1F0;
		}

		public ClericDampenSpiritScroll( Serial serial ) : base( serial )
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
