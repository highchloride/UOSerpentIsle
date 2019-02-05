using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Bard
{
	public class BardMagesBalladScroll : CSpellScroll
	{
		[Constructable]
		public BardMagesBalladScroll() : this( 1 )
		{
		}

		[Constructable]
		public BardMagesBalladScroll( int amount ) : base( typeof( BardMagesBalladSpell ), 0x14ED, amount )
		{
			Name = "Mages Ballad Sheet Music";
			Hue = 0x96;
		}

		public BardMagesBalladScroll( Serial serial ) : base( serial )
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
