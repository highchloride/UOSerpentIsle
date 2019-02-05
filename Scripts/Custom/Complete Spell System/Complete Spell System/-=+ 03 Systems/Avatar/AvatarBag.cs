using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Avatar
{
	public class AvatarBag : ScrollBag
	{
		[Constructable]
		public AvatarBag()
		{
			Hue = 1174;
			PlaceItemIn( 30, 35, new AvatarHeavenlyLightScroll() );
			PlaceItemIn( 50, 35, new AvatarHeavensGateScroll() );
			PlaceItemIn( 70, 35, new AvatarMarkOfGodsScroll() );
		}

		public AvatarBag( Serial serial ) : base( serial )
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