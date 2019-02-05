using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Cleric
{
	public class ClericDivineFocusScroll : CSpellScroll
	{
		[Constructable]
		public ClericDivineFocusScroll() : this( 1 )
		{
		}

		[Constructable]
		public ClericDivineFocusScroll( int amount ) : base( typeof( ClericDivineFocusSpell ), 0xE39, amount )
		{
			Name = "Divine Focus";
			Hue = 0x1F0;
		}

		public ClericDivineFocusScroll( Serial serial ) : base( serial )
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
