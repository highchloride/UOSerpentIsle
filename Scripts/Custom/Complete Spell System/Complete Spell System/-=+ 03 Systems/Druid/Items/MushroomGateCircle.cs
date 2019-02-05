using System;
using Server.Items;
using Server.Mobiles;
using Server.Misc;

namespace Server.ACC.CSS.Systems.Druid
{
	[DispellableField]
	public class MushroomGateCircle : Moongate
	{
		private int m_ItemID;

		public MushroomGateCircle (Point3D target, Map map, int item)
		{
			m_ItemID=item;

			if ( ShowFeluccaWarning && map == Map.Felucca )
			{
				Hue = 1175;
				ItemID=m_ItemID;
			}

			InternalTimer t = new InternalTimer( this );
			t.Start();
		}

		public MushroomGateCircle( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			Delete();
		}

		private class InternalTimer : Timer
		{
			private Item m_Item;

			public InternalTimer( Item item ) : base( TimeSpan.FromSeconds( 30.0 ) )
			{
				m_Item = item;
			}

			protected override void OnTick()
			{
				m_Item.Delete();
			}
		}
	}
}
