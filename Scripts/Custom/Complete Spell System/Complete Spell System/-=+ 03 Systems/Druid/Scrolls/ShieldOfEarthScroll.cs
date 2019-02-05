using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Druid
{
	public class DruidShieldOfEarthScroll : CSpellScroll
	{
		[Constructable]
		public DruidShieldOfEarthScroll() : this( 1 )
		{
		}

		[Constructable]
		public DruidShieldOfEarthScroll( int amount ) : base( typeof( DruidShieldOfEarthSpell ), 0xE39, amount )
		{
			Name = "Shield Of Earth";
			Hue = 0x58B;
		}

		public DruidShieldOfEarthScroll( Serial serial ) : base( serial )
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
