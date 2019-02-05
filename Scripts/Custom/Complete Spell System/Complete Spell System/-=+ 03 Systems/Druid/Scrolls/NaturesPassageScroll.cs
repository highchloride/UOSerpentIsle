using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Druid
{
	public class DruidNaturesPassageScroll : CSpellScroll
	{
		[Constructable]
		public DruidNaturesPassageScroll() : this( 1 )
		{
		}

		[Constructable]
		public DruidNaturesPassageScroll( int amount ) : base( typeof( DruidNaturesPassageSpell ), 0xE39, amount )
		{
			Name = "Nature's Passage";
			Hue = 0x58B;
		}

		public DruidNaturesPassageScroll( Serial serial ) : base( serial )
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
