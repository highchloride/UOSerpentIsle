using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Ranger
{
	public class RangerFamiliarScroll : CSpellScroll
	{
		[Constructable]
		public RangerFamiliarScroll() : this( 1 )
		{
		}

		[Constructable]
		public RangerFamiliarScroll( int amount ) : base( typeof( RangerFamiliarSpell ), 3828, amount )
		{
			Name = "Animal Companion";
			Hue = 2001;
		}

		public RangerFamiliarScroll( Serial serial ) : base( serial )
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
