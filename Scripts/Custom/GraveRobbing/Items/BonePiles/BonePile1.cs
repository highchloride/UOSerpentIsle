using System;
using System.Collections;
using Server.Multis;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
	public class BonePile1 : LockableContainer
	{
	        public override bool Decays{ get{ return true; } } 

	        public override TimeSpan DecayTime{ get{ return TimeSpan.FromMinutes( Utility.Random( 2, 4 ) ); } }

		public override int DefaultGumpID{ get{ return 9; } }

		public override int DefaultDropSound{ get{ return 66; } }

		public override Rectangle2D Bounds{ get{ return new Rectangle2D( 20, 85, 104, 111 ); } }

		[Constructable]
		public BonePile1() : base( 3786 )
		{
			Name = "Pile of Tangled Bones";
			Weight = 1;
		        Movable = false;
			LiftOverride = false; // force stealing

			ItemID = Utility.RandomList( 3786, 3794 );

			int reward = Utility.RandomMinMax( 0, 1 );;

		        DropItem( new Gold( Utility.RandomMinMax( 15, 125 )));

			for( int i = Utility.Random( 1, reward ); i > 1; i-- )
			{
				switch (Utility.Random( reward ))
				{
					default:
					case 0: DropItem( Loot.RandomClothing() ); break;
					case 1: DropItem( Loot.RandomJewelry() ); break;
				}
			}			
		}

		public BonePile1( Serial serial ) : base( serial )
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