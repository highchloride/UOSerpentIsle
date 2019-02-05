using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Rogue
{
	public class RogueBag : ScrollBag
	{
		[Constructable]
		public RogueBag()
		{
			Hue = 0x20;
			PlaceItemIn( 30, 35, new RogueCharmScroll() );
			PlaceItemIn( 50, 35, new RogueFalseCoinScroll() );
			PlaceItemIn( 70, 35, new RogueIntimidationScroll() );
			PlaceItemIn( 90, 35, new RogueShadowScroll() );
			PlaceItemIn( 30, 55, new RogueSlyFoxScroll() );
		}

		public RogueBag( Serial serial ) : base( serial )
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