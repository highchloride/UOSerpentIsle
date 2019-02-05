using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Bard
{
	public class BardPoisonCarolScroll : CSpellScroll
	{
		[Constructable]
		public BardPoisonCarolScroll() : this( 1 )
		{
		}

		[Constructable]
		public BardPoisonCarolScroll( int amount ) : base( typeof( BardPoisonCarolSpell ), 0x14ED, amount )
		{
			Name = "Poison Carol Sheet Music";
			Hue = 0x96;
		}

		public BardPoisonCarolScroll( Serial serial ) : base( serial )
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
