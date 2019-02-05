using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Ranger
{
	public class RangerBag : ScrollBag
	{
		[Constructable]
		public RangerBag()
		{
			Hue = 2001;
			PlaceItemIn( 30, 35, new RangerFireBowScroll() );
			PlaceItemIn( 50, 35, new RangerPhoenixFlightScroll() );
			PlaceItemIn( 70, 35, new RangerHuntersAimScroll() );
			PlaceItemIn( 90, 35, new RangerIceBowScroll() );
			PlaceItemIn( 30, 55, new RangerLightningBowScroll() );
			PlaceItemIn( 50, 55, new RangerFamiliarScroll() );
			PlaceItemIn( 70, 55, new RangerNoxBowScroll() );
			PlaceItemIn( 90, 55, new RangerSummonMountScroll() );
		}

		public RangerBag( Serial serial ) : base( serial )
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