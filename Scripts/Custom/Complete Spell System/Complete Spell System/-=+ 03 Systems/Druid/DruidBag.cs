using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Druid
{
	public class DruidBag : ScrollBag
	{
		[Constructable]
		public DruidBag()
		{
			Hue = 0x48C;
			PlaceItemIn( 30, 35, new DruidBlendWithForestScroll() );
			PlaceItemIn( 50, 35, new DruidEnchantedGroveScroll() );
			PlaceItemIn( 70, 35, new DruidFamiliarScroll() );
			PlaceItemIn( 90, 35, new DruidGraspingRootsScroll() );
			PlaceItemIn( 30, 55, new DruidHollowReedScroll() );
			PlaceItemIn( 50, 55, new DruidLureStoneScroll() );
			PlaceItemIn( 70, 55, new DruidMushroomGatewayScroll() );
			PlaceItemIn( 90, 55, new DruidNaturesPassageScroll() );
			PlaceItemIn( 30, 75, new DruidPackOfBeastScroll() );
			PlaceItemIn( 50, 75, new DruidRestorativeSoilScroll() );
			PlaceItemIn( 70, 75, new DruidShieldOfEarthScroll() );
			PlaceItemIn( 90, 75, new DruidSpringOfLifeScroll() );
			PlaceItemIn( 30, 95, new DruidStoneCircleScroll() );
			PlaceItemIn( 50, 95, new DruidSwarmOfInsectsScroll() );
			PlaceItemIn( 70, 95, new DruidLeafWhirlwindScroll() );
			PlaceItemIn( 90, 95, new DruidVolcanicEruptionScroll() );
		}

		public DruidBag( Serial serial ) : base( serial )
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