using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Bard
{
	public class BardArmysPaeonScroll : CSpellScroll
	{
		[Constructable]
		public BardArmysPaeonScroll() : this( 1 )
		{
		}

		[Constructable]
		public BardArmysPaeonScroll( int amount ) : base( typeof( BardArmysPaeonSpell ), 0x14ED, amount )
		{
			Name = "Armys Paeon Sheet Music";
			Hue = 0x96;
		}

		public BardArmysPaeonScroll( Serial serial ) : base( serial )
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
