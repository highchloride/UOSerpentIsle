using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Druid
{
	public class DruidRestorativeSoilScroll : CSpellScroll
	{
		[Constructable]
		public DruidRestorativeSoilScroll() : this( 1 )
		{
		}

		[Constructable]
		public DruidRestorativeSoilScroll( int amount ) : base( typeof( DruidRestorativeSoilSpell ), 0xE39, amount )
		{
			Name = "Restorative Soil";
			Hue = 0x58B;
		}

		public DruidRestorativeSoilScroll( Serial serial ) : base( serial )
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
