using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Ranger
{
	public class RangerFireBowScroll : CSpellScroll
	{
		[Constructable]
		public RangerFireBowScroll() : this( 1 )
		{
		}

		[Constructable]
		public RangerFireBowScroll( int amount ) : base( typeof( RangerFireBowSpell ), 3828, amount )
		{
			Name = "Fire Bow";
			Hue = 2001;
		}

		public RangerFireBowScroll( Serial serial ) : base( serial )
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
