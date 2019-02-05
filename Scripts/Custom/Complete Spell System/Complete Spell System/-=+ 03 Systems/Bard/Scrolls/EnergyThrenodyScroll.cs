using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Bard
{
	public class BardEnergyThrenodyScroll : CSpellScroll
	{
		[Constructable]
		public BardEnergyThrenodyScroll() : this( 1 )
		{
		}

		[Constructable]
		public BardEnergyThrenodyScroll( int amount ) : base( typeof( BardEnergyThrenodySpell ), 0x14ED, amount )
		{
			Name = "Energy Threnody Sheet Music";
			Hue = 0x96;
		}

		public BardEnergyThrenodyScroll( Serial serial ) : base( serial )
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
