using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Ranger
{
	public class RangerSummonMountScroll : CSpellScroll
	{
		[Constructable]
		public RangerSummonMountScroll() : this( 1 )
		{
		}

		[Constructable]
		public RangerSummonMountScroll( int amount ) : base( typeof( RangerSummonMountSpell ), 3828, amount )
		{
			Name = "Call Mount";
			Hue = 2001;
		}

		public RangerSummonMountScroll( Serial serial ) : base( serial )
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
