using System;
using System.Collections;
using Server.Multis;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
	public class GraveTreasureChest5 : LockableContainer
	{
	        public override bool Decays{ get{ return true; } } 

	        public override TimeSpan DecayTime{ get{ return TimeSpan.FromMinutes( Utility.Random( 2, 5 ) ); } }

		public override int DefaultGumpID{ get{ return 9; } }

		public override int DefaultDropSound{ get{ return 66; } }

		public override Rectangle2D Bounds{ get{ return new Rectangle2D( 20, 85, 104, 111 ); } }

		[Constructable]
		public GraveTreasureChest5() : base( 3712 )
		{
			ItemID = Utility.RandomList( 2472, 3712 );
			Weight = 10.0;
		        Movable = false;
		        Locked = true;
			Name = "A Mysterious Chest";
			LiftOverride = false; // force stealing

			int reward = Utility.RandomMinMax( 2, 4 );;
			RequiredSkill = 88;
			
			switch ( Utility.Random( 2 ) )
			{
				default:
				case 0: TrapType = TrapType.DartTrap; break;
				case 1: TrapType = TrapType.PoisonTrap; break;
				case 2: TrapType = TrapType.ExplosionTrap; break;
			}
		        TrapPower = Utility.Random( 50, 125 );
			TrapOnLockpick = true;

            		LockLevel = RequiredSkill - Utility.Random( 50, 125 );
            		MaxLockLevel = RequiredSkill;

		        DropItem( new Gold( Utility.RandomMinMax( RequiredSkill * 5, RequiredSkill * 10 ) ) );

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
					                    weapon.DamageLevel = (WeaponDamageLevel)Utility.Random( reward * 2 );
					                    weapon.AccuracyLevel = (WeaponAccuracyLevel)Utility.Random( reward * 2 );
					                    weapon.DurabilityLevel = (WeaponDurabilityLevel)Utility.Random( reward * 2 );
					                    weapon.Quality = ItemQuality.Normal;
					                }
						        else if( item is BaseArmor )
					                {
						            BaseArmor armor = (BaseArmor)item;
					                    armor.ProtectionLevel = (ArmorProtectionLevel)Utility.Random( reward * 2 );
					                    armor.Durability = (ArmorDurabilityLevel)Utility.Random( reward * 2 );
					                    armor.Quality = ItemQuality.Normal;
					                }
						}
                				DropItem( item );
						break;
					}
					case 3:
					{
						//arties :D
						break;
					}
				}
			}			
		}

		public GraveTreasureChest5( Serial serial ) : base( serial )
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