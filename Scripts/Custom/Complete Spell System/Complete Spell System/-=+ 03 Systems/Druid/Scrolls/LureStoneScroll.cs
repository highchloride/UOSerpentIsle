using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Druid
{
	public class DruidLureStoneScroll : CSpellScroll
	{
		[Constructable]
		public DruidLureStoneScroll() : this( 1 )
		{
		}

		[Constructable]
		public DruidLureStoneScroll( int amount ) : base( typeof( DruidLureStoneSpell ), 0xE39, amount )
		{
			Name = "Lure Stone";
			Hue = 0x58B;
		}

		public DruidLureStoneScroll( Serial serial ) : base( serial )
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
