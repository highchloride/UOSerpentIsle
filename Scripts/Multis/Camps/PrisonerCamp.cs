using System;
 using Server;
 using Server.Items;
 using Server.Mobiles;

namespace Server.Multis
{
     public class PrisonerCamp : BaseCamp
     {
        private BaseDoor m_Gate;
        private int m_CampType = 0;

         [Constructable]
         public PrisonerCamp(int CampType = 4) : base( 0x1D4C )
         {
            m_CampType = CampType;
         }

         public override void AddComponents()
         {
             IronGate gate = new IronGate( DoorFacing.EastCCW );
             m_Gate = gate;

             gate.KeyValue = Key.RandomValue();
             gate.Locked = true;

             AddItem( gate, -2, 1, 0 );
			 AddCampChests();

            //UOSI x= -2 puts these guys in the cage. I removed the - from all the 2's in this switch statement. They were all -2.
            //Wander ranges increased.
            //Added camp type setter.
            if(m_CampType == 4)
            {
                m_CampType = Utility.Random(4);
            }

             switch (m_CampType)
             {
                 case 0:
                     {
                         AddMobile(new Orc(), 2, 2, 0, 0);
                         AddMobile(new OrcishMage(), 2, 1, 0, 0);
                         AddMobile(new OrcishLord(), 2, 2, 0, 0);
                         AddMobile(new OrcCaptain(), 2, 1, 0, 0);
                         AddMobile(new Orc(),  2, -1, 0, 0);
                         AddMobile(new OrcChopper(), 2, 2, 0, 0);
                        Camp = CampType.Orc;
                     } break;

                 case 1:
                     {
                         AddMobile(new Ratman(), 2, 2, 0, 0);
                         AddMobile(new Ratman(), 2, 1, 0, 0);
                         AddMobile(new RatmanMage(), 2, 2, 0, 0);
                         AddMobile(new Ratman(), 2, 1, 0, 0);
                         AddMobile(new RatmanArcher(), 2, -1, 0, 0);
                         AddMobile(new Ratman(), 2, 2, 0, 0);
                        Camp = CampType.Ratman;
                     } break;

                 case 2:
                     {
                         AddMobile(new Lizardman(), 2, 2, 0, 0);
                         AddMobile(new Lizardman(), 2, 1, 0, 0);
                         AddMobile(new Lizardman(), 2, 2, 0, 0);
                         AddMobile(new Lizardman(), 2, 1, 0, 0);
                         AddMobile(new Lizardman(), 2, -1, 0, 0);
                         AddMobile(new Lizardman(), 2, 2, 0, 0);
                        Camp = CampType.Lizardman;
                     } break;

                 case 3:
                     {
                         AddMobile(new Brigand(), 2, 2, 0, 0);
                         AddMobile(new Brigand(), 2, 1, 0, 0);
                         AddMobile(new Brigand(),  2, 2, 0, 0);
                         AddMobile(new Brigand(), 2, 1, 0, 0);
                         AddMobile(new Brigand(),  2, -1, 0, 0);
                         AddMobile(new Brigand(), 2, 2, 0, 0);
                        Camp = CampType.Brigand;
                     } break;
             }

            //UOSI - added 'this' to the constructor for these two, which SHOULD flag them as camp prisoners.
             switch ( Utility.Random( 2 ) )
             {
                 case 0: Prisoner = new Noble(this); break;
                 case 1: Prisoner = new SeekerOfAdventure(this); break;
             }

             //Prisoner.IsPrisoner = true;
             Prisoner.CantWalk = true;
             
             
             Prisoner.YellHue = Utility.RandomList( 0x57, 0x67, 0x77, 0x87, 0x117 );
            //UOSI - Switched his wander range for his xOffset so he'll appear in the cage
             AddMobile( Prisoner, 0, -2, 0, 0);
         }

         public override void OnEnter( Mobile m )
         {
             base.OnEnter( m );

             if ( m.Player && Prisoner != null && m_Gate != null && m_Gate.Locked )
             {
                 int number;

                 switch ( Utility.Random( 10 ) )
                 {
                     default:
                     case 0: number =  502264; break; // Help a poor prisoner!
                     case 1: number =  502266; break; // Aaah! Help me!
                     case 2: number = 1046000; break; // Help! These savages wish to end my life!
                     case 3: number = 1046003; break; // Quickly! Kill them for me! HELP!!
					 case 4: number = 502261;  break; // HELP!
					 case 5: number = 502262; break; // Help me!
					 case 6: number = 502263; break; // Canst thou aid me?!
                 	 case 7: number = 502265; break; // Help! Please!
					 case 8: number = 502267; break; // Go and get some help!
					 case 9: number = 502268; break; // Quickly, I beg thee! Unlock my chains! If thou dost look at me close thou canst see them.	 
					 }

                 Prisoner.Yell( number );
             }
         }

         public PrisonerCamp( Serial serial ) : base( serial )
         {
         }

        
        private void AddCampChests()
        {
            LockableContainer chest = null;

            switch (Utility.Random(3))
            {
                case 0:
                    chest = new MetalChest();
                    break;
                case 1:
                    chest = new MetalGoldenChest();
                    break;
                default:
                    chest = new WoodenChest();
                    break;
            }

            chest.LiftOverride = true;

            TreasureMapChest.Fill(chest, Utility.Random(10, 40), Utility.Random(1,2), false, Map.SerpentIsle); //UOSI draws random values for level and luck

            this.AddItem(chest, -2, 2, 0);

            LockableContainer crates = null;

            switch (Utility.Random(4))
            {
                case 0:
                    crates = new SmallCrate();
                    break;
                case 1:
                    crates = new MediumCrate();
                    break;
                case 2:
                    crates = new LargeCrate();
                    break;
                default:
                    crates = new LockableBarrel();
                    break;
            }

            crates.TrapType = TrapType.ExplosionTrap;
            crates.TrapPower = Utility.RandomMinMax(30, 40);
            crates.TrapLevel = 2;

            crates.RequiredSkill = 76;
            crates.LockLevel = 66;
            crates.MaxLockLevel = 116;
            crates.Locked = true;

            crates.DropItem(new Gold(Utility.RandomMinMax(100, 400)));
            crates.DropItem(new Arrow(10));
            crates.DropItem(new Bolt(10));

            crates.LiftOverride = true;

            if (Utility.RandomDouble() < 0.8)
            {
                switch (Utility.Random(4))
                {
                    case 0:
                        crates.DropItem(new LesserCurePotion());
                        break;
                    case 1:
                        crates.DropItem(new LesserExplosionPotion());
                        break;
                    case 2:
                        crates.DropItem(new LesserHealPotion());
                        break;
                    default:
                        crates.DropItem(new LesserPoisonPotion());
                        break;
                }
            }

            this.AddItem(crates, 2, -2, 0);
        }

        public override void Serialize( GenericWriter writer )
         {
             base.Serialize( writer );

             writer.Write( (int) 1 ); // version

             writer.Write( m_Gate );
         }

         public override void Deserialize( GenericReader reader )
         {
             base.Deserialize( reader );

             int version = reader.ReadInt();

             switch ( version )
             {
                 case 1:
                     m_Gate = reader.ReadItem() as BaseDoor;
                     break;
                 case 0:
                 {
                     Prisoner = reader.ReadMobile() as BaseCreature;
                     m_Gate = reader.ReadItem() as BaseDoor;
                     break;
                 }
             }
         }
     }
}
