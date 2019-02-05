using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Druid
{
	public class DruidPackOfBeastScroll : CSpellScroll
	{
		[Constructable]
		public DruidPackOfBeastScroll() : this( 1 )
		{
		}

		[Constructable]
		public DruidPackOfBeastScroll( int amount ) : base( typeof( DruidPackOfBeastSpell ), 0xE39, amount )
		{
			Name = "Pack Of Beast";
			Hue = 0x58B;
		}

		public DruidPackOfBeastScroll( Serial serial ) : base( serial )
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
