using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Rogue
{
	public class RogueFalseCoinScroll : CSpellScroll
	{
		[Constructable]
		public RogueFalseCoinScroll() : this( 1 )
		{
		}

		[Constructable]
		public RogueFalseCoinScroll( int amount ) : base( typeof( RogueFalseCoinSpell ), 0xE39, amount )
		{
			Name = "False Coin";
			Hue = 0x20;
		}

		public RogueFalseCoinScroll( Serial serial ) : base( serial )
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
