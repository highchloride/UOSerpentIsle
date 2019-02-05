using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Cleric
{
	public class ClericSacrificeScroll : CSpellScroll
	{
		[Constructable]
		public ClericSacrificeScroll() : this( 1 )
		{
		}

		[Constructable]
		public ClericSacrificeScroll( int amount ) : base( typeof( ClericSacrificeSpell ), 0xE39, amount )
		{
			Name = "Sacrifice";
			Hue = 0x1F0;
		}

		public ClericSacrificeScroll( Serial serial ) : base( serial )
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
