using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Bard
{
	public class BardIceCarolScroll : CSpellScroll
	{
		[Constructable]
		public BardIceCarolScroll() : this( 1 )
		{
		}

		[Constructable]
		public BardIceCarolScroll( int amount ) : base( typeof( BardIceCarolSpell ), 0x14ED, amount )
		{
			Name = "Ice Carol Sheet Music";
			Hue = 0x96;
		}

		public BardIceCarolScroll( Serial serial ) : base( serial )
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
