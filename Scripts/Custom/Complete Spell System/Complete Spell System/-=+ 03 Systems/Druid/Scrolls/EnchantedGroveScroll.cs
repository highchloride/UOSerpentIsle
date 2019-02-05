using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Druid
{
	public class DruidEnchantedGroveScroll : CSpellScroll
	{
		[Constructable]
		public DruidEnchantedGroveScroll() : this( 1 )
		{
		}

		[Constructable]
		public DruidEnchantedGroveScroll( int amount ) : base( typeof( DruidEnchantedGroveSpell ), 0xE39, amount )
		{
			Name = "Enchanted Grove";
			Hue = 0x58B;
		}

		public DruidEnchantedGroveScroll( Serial serial ) : base( serial )
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
