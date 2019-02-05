using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Cleric
{
	public class ClericSmiteScroll : CSpellScroll
	{
		[Constructable]
		public ClericSmiteScroll() : this( 1 )
		{
		}

		[Constructable]
		public ClericSmiteScroll( int amount ) : base( typeof( ClericSmiteSpell ), 0xE39, amount )
		{
			Name = "Smite";
			Hue = 0x1F0;
		}

		public ClericSmiteScroll( Serial serial ) : base( serial )
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
