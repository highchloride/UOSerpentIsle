using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Bard
{
	public class BardKnightsMinneScroll : CSpellScroll
	{
		[Constructable]
		public BardKnightsMinneScroll() : this( 1 )
		{
		}

		[Constructable]
		public BardKnightsMinneScroll( int amount ) : base( typeof( BardKnightsMinneSpell ), 0x14ED, amount )
		{
			Name = "Knights Minne Sheet Music";
			Hue = 0x96;
		}

		public BardKnightsMinneScroll( Serial serial ) : base( serial )
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
