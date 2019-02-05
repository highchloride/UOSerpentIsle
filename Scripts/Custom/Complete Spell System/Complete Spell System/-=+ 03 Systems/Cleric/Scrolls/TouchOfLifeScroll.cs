using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Cleric
{
	public class ClericTouchOfLifeScroll : CSpellScroll
	{
		[Constructable]
		public ClericTouchOfLifeScroll() : this( 1 )
		{
		}

		[Constructable]
		public ClericTouchOfLifeScroll( int amount ) : base( typeof( ClericTouchOfLifeSpell ), 0xE39, amount )
		{
			Name = "Touch Of Life";
			Hue = 0x1F0;
		}

		public ClericTouchOfLifeScroll( Serial serial ) : base( serial )
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
