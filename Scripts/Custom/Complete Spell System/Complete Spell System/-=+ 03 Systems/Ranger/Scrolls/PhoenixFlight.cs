using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Ranger
{
	public class RangerPhoenixFlightScroll : CSpellScroll
	{
		[Constructable]
		public RangerPhoenixFlightScroll() : this( 1 )
		{
		}

		[Constructable]
		public RangerPhoenixFlightScroll( int amount ) : base( typeof( RangerPhoenixFlightSpell ), 3828, amount )
		{
			Name = "Flight of the Phoenix";
			Hue = 2001;
		}

		public RangerPhoenixFlightScroll( Serial serial ) : base( serial )
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
