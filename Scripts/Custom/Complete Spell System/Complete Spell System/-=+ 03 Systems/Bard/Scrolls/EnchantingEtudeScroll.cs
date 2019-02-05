using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Bard
{
	public class BardEnchantingEtudeScroll : CSpellScroll
	{
		[Constructable]
		public BardEnchantingEtudeScroll() : this( 1 )
		{
		}

		[Constructable]
		public BardEnchantingEtudeScroll( int amount ) : base( typeof( BardEnchantingEtudeSpell ), 0x14ED, amount )
		{
			Name = "Enchanting Etude Sheet Music";
			Hue = 0x96;
		}

		public BardEnchantingEtudeScroll( Serial serial ) : base( serial )
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
