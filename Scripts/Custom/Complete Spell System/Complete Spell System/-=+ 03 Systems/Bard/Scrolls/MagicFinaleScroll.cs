using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Bard
{
	public class BardMagicFinaleScroll : CSpellScroll
	{
		[Constructable]
		public BardMagicFinaleScroll() : this( 1 )
		{
		}

		[Constructable]
		public BardMagicFinaleScroll( int amount ) : base( typeof( BardMagicFinaleSpell ), 0x14ED, amount )
		{
			Name = "Magic Finale Sheet Music";
			Hue = 0x96;
		}

		public BardMagicFinaleScroll( Serial serial ) : base( serial )
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
