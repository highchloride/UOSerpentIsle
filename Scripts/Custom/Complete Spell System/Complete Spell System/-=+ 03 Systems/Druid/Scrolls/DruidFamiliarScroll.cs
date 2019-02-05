using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Druid
{
	public class DruidFamiliarScroll : CSpellScroll
	{
		[Constructable]
		public DruidFamiliarScroll() : this( 1 )
		{
		}

		[Constructable]
		public DruidFamiliarScroll( int amount ) : base( typeof( DruidFamiliarSpell ), 0xE39, amount )
		{
			Name = "Druid Familiar";
			Hue = 0x58B;
		}

		public DruidFamiliarScroll( Serial serial ) : base( serial )
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
