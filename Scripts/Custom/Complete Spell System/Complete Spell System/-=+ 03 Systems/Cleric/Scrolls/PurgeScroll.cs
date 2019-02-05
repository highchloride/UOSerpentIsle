using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Cleric
{
	public class ClericPurgeScroll : CSpellScroll
	{
		[Constructable]
		public ClericPurgeScroll() : this( 1 )
		{
		}

		[Constructable]
		public ClericPurgeScroll( int amount ) : base( typeof( ClericPurgeSpell ), 0xE39, amount )
		{
			Name = "Purge";
			Hue = 0x1F0;
		}

		public ClericPurgeScroll( Serial serial ) : base( serial )
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
