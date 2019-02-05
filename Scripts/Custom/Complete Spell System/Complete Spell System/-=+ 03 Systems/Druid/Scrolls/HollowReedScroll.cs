using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Druid
{
	public class DruidHollowReedScroll : CSpellScroll
	{
		[Constructable]
		public DruidHollowReedScroll() : this( 1 )
		{
		}

		[Constructable]
		public DruidHollowReedScroll( int amount ) : base( typeof( DruidHollowReedSpell ), 0xE39, amount )
		{
			Name = "Hollow Reed";
			Hue = 0x58B;
		}

		public DruidHollowReedScroll( Serial serial ) : base( serial )
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
