using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Bard
{
	public class BardSinewyEtudeScroll : CSpellScroll
	{
		[Constructable]
		public BardSinewyEtudeScroll() : this( 1 )
		{
		}

		[Constructable]
		public BardSinewyEtudeScroll( int amount ) : base( typeof( BardSinewyEtudeSpell ), 0x14ED, amount )
		{
			Name = "Sinewy Etude Sheet Music";
			Hue = 0x96;
		}

		public BardSinewyEtudeScroll( Serial serial ) : base( serial )
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
