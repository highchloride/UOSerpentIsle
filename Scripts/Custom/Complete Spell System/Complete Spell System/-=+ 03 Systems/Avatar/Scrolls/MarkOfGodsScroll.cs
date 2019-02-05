using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Avatar
{
	public class AvatarMarkOfGodsScroll : CSpellScroll
	{
		[Constructable]
		public AvatarMarkOfGodsScroll() : this( 1 )
		{
		}

		[Constructable]
		public AvatarMarkOfGodsScroll( int amount ) : base( typeof( AvatarMarkOfGodsSpell ), 0xE39, amount )
		{
			Name = "Mark of Gods";
			Hue = 1174;
		}

		public AvatarMarkOfGodsScroll( Serial serial ) : base( serial )
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
