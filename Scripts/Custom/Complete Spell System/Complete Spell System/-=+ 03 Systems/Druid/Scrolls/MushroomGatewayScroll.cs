using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Druid
{
	public class DruidMushroomGatewayScroll : CSpellScroll
	{
		[Constructable]
		public DruidMushroomGatewayScroll() : this( 1 )
		{
		}

		[Constructable]
		public DruidMushroomGatewayScroll( int amount ) : base( typeof( DruidMushroomGatewaySpell ), 0xE39, amount )
		{
			Name = "Mushroom Gateway";
			Hue = 0x58B;
		}

		public DruidMushroomGatewayScroll( Serial serial ) : base( serial )
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
