using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Bard
{
	public class BardFireCarolScroll : CSpellScroll
	{
		[Constructable]
		public BardFireCarolScroll() : this( 1 )
		{
		}

		[Constructable]
		public BardFireCarolScroll( int amount ) : base( typeof( BardFireCarolSpell ), 0x14ED, amount )
		{
			Name = "Fire Carol Sheet Music";
			Hue = 0x96;
		}

		public BardFireCarolScroll( Serial serial ) : base( serial )
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
