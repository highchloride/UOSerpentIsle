using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Cleric
{
	public class ClericRestorationScroll : CSpellScroll
	{
		[Constructable]
		public ClericRestorationScroll() : this( 1 )
		{
		}

		[Constructable]
		public ClericRestorationScroll( int amount ) : base( typeof( ClericRestorationSpell ), 0xE39, amount )
		{
			Name = "Restoration";
			Hue = 0x1F0;
		}

		public ClericRestorationScroll( Serial serial ) : base( serial )
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
