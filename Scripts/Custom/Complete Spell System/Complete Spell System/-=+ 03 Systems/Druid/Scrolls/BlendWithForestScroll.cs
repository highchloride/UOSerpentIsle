using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Druid
{
	public class DruidBlendWithForestScroll : CSpellScroll
	{
		[Constructable]
		public DruidBlendWithForestScroll() : this( 1 )
		{
		}

		[Constructable]
		public DruidBlendWithForestScroll( int amount ) : base( typeof( DruidBlendWithForestSpell ), 0xE39, amount )
		{
			Name = "Blend With Forest";
			Hue = 0x58B;
		}

		public DruidBlendWithForestScroll( Serial serial ) : base( serial )
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
