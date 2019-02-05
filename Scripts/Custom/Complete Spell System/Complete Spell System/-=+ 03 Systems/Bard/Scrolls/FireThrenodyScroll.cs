using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Bard
{
	public class BardFireThrenodyScroll : CSpellScroll
	{
		[Constructable]
		public BardFireThrenodyScroll() : this( 1 )
		{
		}

		[Constructable]
		public BardFireThrenodyScroll( int amount ) : base( typeof( BardFireThrenodySpell ), 0x14ED, amount )
		{
			Name = "Fire Threnody Sheet Music";
			Hue = 0x96;
		}

		public BardFireThrenodyScroll( Serial serial ) : base( serial )
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
