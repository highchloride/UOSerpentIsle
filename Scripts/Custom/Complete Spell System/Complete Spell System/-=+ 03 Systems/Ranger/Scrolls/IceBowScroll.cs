using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Ranger
{
	public class RangerIceBowScroll : CSpellScroll
	{
		[Constructable]
		public RangerIceBowScroll() : this( 1 )
		{
		}

		[Constructable]
		public RangerIceBowScroll( int amount ) : base( typeof( RangerIceBowSpell ), 3828, amount )
		{
			Name="Ice Bow";
			Hue = 2001;
		}

		public RangerIceBowScroll( Serial serial ) : base( serial )
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
