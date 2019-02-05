using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Bard
{
	public class BardIceThrenodyScroll : CSpellScroll
	{
		[Constructable]
		public BardIceThrenodyScroll() : this( 1 )
		{
		}

		[Constructable]
		public BardIceThrenodyScroll( int amount ) : base( typeof( BardIceThrenodySpell ), 0x14ED, amount )
		{
			Name = "Ice Threnody Sheet Music";
			Hue = 0x96;
		}

		public BardIceThrenodyScroll( Serial serial ) : base( serial )
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
