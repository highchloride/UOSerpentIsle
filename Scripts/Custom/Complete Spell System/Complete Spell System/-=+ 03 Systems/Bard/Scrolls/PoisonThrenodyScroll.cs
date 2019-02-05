using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Bard
{
	public class BardPoisonThrenodyScroll : CSpellScroll
	{
		[Constructable]
		public BardPoisonThrenodyScroll() : this( 1 )
		{
		}

		[Constructable]
		public BardPoisonThrenodyScroll( int amount ) : base( typeof( BardPoisonThrenodySpell ), 0x14ED, amount )
		{
			Name = "Poison Threnody Sheet Music";
			Hue = 0x96;
		}

		public BardPoisonThrenodyScroll( Serial serial ) : base( serial )
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
