using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Ranger
{
	public class RangerLightningBowScroll : CSpellScroll
	{
		[Constructable]
		public RangerLightningBowScroll() : this( 1 )
		{
		}

		[Constructable]
		public RangerLightningBowScroll( int amount ) : base( typeof( RangerLightningBowSpell ), 3828, amount )
		{
			Name = "Lightning Bow";
			Hue = 2001;
		}

		public RangerLightningBowScroll( Serial serial ) : base( serial )
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
