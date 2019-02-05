using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Bard
{
	public class BardSheepfoeMamboScroll : CSpellScroll
	{
		[Constructable]
		public BardSheepfoeMamboScroll() : this( 1 )
		{
		}

		[Constructable]
		public BardSheepfoeMamboScroll( int amount ) : base( typeof( BardSheepfoeMamboSpell ), 0x14ED, amount )
		{
			Name = "Sheep foe Mambo sheet music";
			Hue = 0x96;
		}

		public BardSheepfoeMamboScroll( Serial serial ) : base( serial )
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
