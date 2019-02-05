using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Ranger
{
	public class RangerHuntersAimScroll : CSpellScroll
	{
		[Constructable]
		public RangerHuntersAimScroll() : this( 1 )
		{
		}

		[Constructable]
		public RangerHuntersAimScroll( int amount ) : base( typeof( RangerHuntersAimSpell ), 3828, amount )
		{
			Name = "Hunter's Aim";
			Hue = 2001;
		}

		public RangerHuntersAimScroll( Serial serial ) : base( serial )
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
