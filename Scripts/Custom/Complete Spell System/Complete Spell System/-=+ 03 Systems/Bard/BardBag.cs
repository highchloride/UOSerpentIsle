using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Bard
{
	public class BardBag : ScrollBag
	{
		[Constructable]
		public BardBag()
		{
			Hue = 0x96;
			PlaceItemIn( 30, 35, new BardArmysPaeonScroll() );
			PlaceItemIn( 50, 35, new BardEnchantingEtudeScroll() );
			PlaceItemIn( 70, 35, new BardEnergyCarolScroll() );
			PlaceItemIn( 90, 35, new BardEnergyThrenodyScroll() );
			PlaceItemIn( 30, 55, new BardFireCarolScroll() );
			PlaceItemIn( 50, 55, new BardFireThrenodyScroll() );
			PlaceItemIn( 70, 55, new BardFoeRequiemScroll() );
			PlaceItemIn( 90, 55, new BardIceCarolScroll() );
			PlaceItemIn( 30, 75, new BardIceThrenodyScroll() );
			PlaceItemIn( 50, 75, new BardKnightsMinneScroll() );
			PlaceItemIn( 70, 75, new BardMagesBalladScroll() );
			PlaceItemIn( 90, 75, new BardMagicFinaleScroll() );
			PlaceItemIn( 30, 95, new BardPoisonCarolScroll() );
			PlaceItemIn( 50, 95, new BardPoisonThrenodyScroll() );
			PlaceItemIn( 70, 95, new BardSheepfoeMamboScroll() );
			PlaceItemIn( 90, 95, new BardSinewyEtudeScroll() );
		}

		public BardBag( Serial serial ) : base( serial )
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