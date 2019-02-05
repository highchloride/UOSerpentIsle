using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Druid
{
	public class DruidVolcanicEruptionScroll : CSpellScroll
	{
		[Constructable]
		public DruidVolcanicEruptionScroll() : this( 1 )
		{
		}

		[Constructable]
		public DruidVolcanicEruptionScroll( int amount ) : base( typeof( DruidVolcanicEruptionSpell ), 0xE39, amount )
		{
			Name = "Volcanic Eruption";
			Hue = 0x58B;
		}

		public DruidVolcanicEruptionScroll( Serial serial ) : base( serial )
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
