using System;
using System.Collections;
using Server.Multis;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
	public class BonePile2 : LockableContainer
	{
	        public override bool Decays{ get{ return true; } } 

	        public override TimeSpan DecayTime{ get{ return TimeSpan.FromMinutes( Utility.Random( 2, 4 ) ); } }

		public override int DefaultGumpID{ get{ return 9; } }

		public override int DefaultDropSound{ get{ return 66; } }

		public override Rectangle2D Bounds{ get{ return new Rectangle2D( 20, 85, 104, 111 ); } }

		[Constructable]
		public BonePile2() : base( 3786 )
		{
			Name = "Pile of Tangled Bones";
			Weight = 1;
		        Movable = false;
			LiftOverride = false; // force stealing

			ItemID = Utility.RandomList( 3786, 3794 );

			int reward = Utility.RandomMinMax( 1, 3 );;

		        DropItem( new Gold( Utility.RandomMinMax( reward * 100, reward * 150 ) ) );

			for( int i = Utility.Random( 1, reward ); i > 1; i-- )
			{
				switch (Utility.Random( reward ))
				{
					default:
					case 0: DropItem( Loot.RandomClothing() ); break;
					case 1: DropItem( Loot.RandomJewelry() ); break;
					case 2: 
					{
                				Item item = Loot.RandomArmorOrShieldOrWeapon();
						if (!Core.AOS)
						{
                					if( item is BaseWeapon )
                					{
					                    BaseWeapon weapon = (BaseWeapon)item;
					                    weapon.DamageLevel = (WeaponDamageLevel)Utility.Random( reward );
					                    weapon.AccuracyLevel = (WeaponAccuracyLevel)Utility.Random( reward );
					                    weapon.DurabilityLevel = (WeaponDurabilityLevel)Utility.Random( reward );
					                    weapon.Quality = ItemQuality.Normal;
					                }
						        else if( item is BaseArmor )
					                {
						            BaseArmor armor = (BaseArmor)item;
					                    armor.ProtectionLevel = (ArmorProtectionLevel)Utility.Random( reward );
					                    armor.Durability = (ArmorDurabilityLevel)Utility.Random( reward );
					                    armor.Quality = ItemQuality.Normal;
					                }
						}
                				DropItem( item );
						break;
					}
				}
			}			
		}

		public BonePile2( Serial serial ) : base( serial )
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