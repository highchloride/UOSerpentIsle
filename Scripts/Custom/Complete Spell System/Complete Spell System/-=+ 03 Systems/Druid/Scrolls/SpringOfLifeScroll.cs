using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Druid
{
	public class DruidSpringOfLifeScroll : CSpellScroll
	{
		[Constructable]
		public DruidSpringOfLifeScroll() : this( 1 )
		{
		}

		[Constructable]
		public DruidSpringOfLifeScroll( int amount ) : base( typeof( DruidSpringOfLifeSpell ), 0xE39, amount )
		{
			Name = "Spring Of Life";
			Hue = 0x58B;
		}

		public DruidSpringOfLifeScroll( Serial serial ) : base( serial )
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

