using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Rogue
{
	public class RogueSlyFoxScroll : CSpellScroll
	{
		[Constructable]
		public RogueSlyFoxScroll() : this( 1 )
		{
		}

		[Constructable]
		public RogueSlyFoxScroll( int amount ) : base( typeof( RogueSlyFoxSpell ), 0xE39, amount )
		{
			Name = "Sly Fox";
			Hue = 0x20;
		}

		public RogueSlyFoxScroll( Serial serial ) : base( serial )
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
