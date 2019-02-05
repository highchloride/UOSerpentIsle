using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Druid
{
	public class DruidStoneCircleScroll : CSpellScroll
	{
		[Constructable]
		public DruidStoneCircleScroll() : this( 1 )
		{
		}

		[Constructable]
		public DruidStoneCircleScroll( int amount ) : base( typeof( DruidStoneCircleSpell ), 0xE39, amount )
		{
			Name = "Stone Circle";
			Hue = 0x58B;
		}

		public DruidStoneCircleScroll( Serial serial ) : base( serial )
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
