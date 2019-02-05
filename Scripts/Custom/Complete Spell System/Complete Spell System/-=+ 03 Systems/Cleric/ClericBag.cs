using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Cleric
{
	public class ClericBag : ScrollBag
	{
		[Constructable]
		public ClericBag()
		{
			Hue = 0x1F0;
			PlaceItemIn( 30, 35, new ClericAngelicFaithScroll() );
			PlaceItemIn( 50, 35, new ClericBanishEvilScroll() );
			PlaceItemIn( 70, 35, new ClericDampenSpiritScroll() );
			PlaceItemIn( 90, 35, new ClericDivineFocusScroll() );
			PlaceItemIn( 30, 55, new ClericHammerOfFaithScroll() );
			PlaceItemIn( 50, 55, new ClericPurgeScroll() );
			PlaceItemIn( 70, 55, new ClericRestorationScroll() );
			PlaceItemIn( 90, 55, new ClericSacredBoonScroll() );
			PlaceItemIn( 30, 75, new ClericSacrificeScroll() );
			PlaceItemIn( 50, 75, new ClericSmiteScroll() );
			PlaceItemIn( 70, 75, new ClericTouchOfLifeScroll() );
			PlaceItemIn( 90, 75, new ClericTrialByFireScroll() );
		}

		public ClericBag( Serial serial ) : base( serial )
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