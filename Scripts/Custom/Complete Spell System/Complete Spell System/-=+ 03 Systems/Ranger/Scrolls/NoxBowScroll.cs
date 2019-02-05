using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Ranger
{
	public class RangerNoxBowScroll : CSpellScroll
	{
		[Constructable]
		public RangerNoxBowScroll() : this( 1 )
		{
		}

		[Constructable]
		public RangerNoxBowScroll( int amount ) : base( typeof( RangerNoxBowSpell ), 3828, amount )
		{
			Name = "Nox Bow";
			Hue = 2001;
		}

		public RangerNoxBowScroll( Serial serial ) : base( serial )
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
