/*
 created by:
     /\            888                   888     .d8888b.   .d8888b.  
____/_ \____       888                   888    d88P  Y88b d88P  Y88b 
\  ___\ \  /       888                   888    888    888 888    888 
 \/ /  \/ /    .d88888  8888b.   8888b.  888888 Y88b. d888 Y88b. d888 
 / /\__/_/\   d88" 888     "88b     "88b 888     "Y888P888  "Y888P888 
/__\ \_____\  888  888 .d888888 .d888888 888           888        888 
    \  /      Y88b 888 888  888 888  888 Y88b.  Y88b  d88P Y88b  d88P 
     \/        "Y88888 "Y888888 "Y888888  "Y888  "Y8888P"   "Y8888P"  
*/
using System;
using System.Collections.Generic;
using Server.ContextMenus;
using Server.Gumps;
using Server.Items;
using Server.Network;
using Server.Commands;

namespace Server.Mobiles
{
	[CorpseName( "Lady Luck's Corpse" )]
	public class LadyLuck : Mobile
	{
		public static void Initialize() 
		{ 
			CommandSystem.Register( "Lottery", AccessLevel.Player, new CommandEventHandler( Lottery_OnCommand ) ); 
		}    
      
		[Usage( "Lottery" )] 
		[Description( "Check if the next drawing need to be hold and anounce the next drawing if it isn't due yet." )] 

		public static void Lottery_OnCommand( CommandEventArgs e ) 
		{ 
			if ( DateTime.Now >= dt_NextDrawing )
				DrawingTime();
			
			if (e.Mobile.AccessLevel >= AccessLevel.GameMaster)
				World.Broadcast( 66, true, "Current reward for drawing number {0} that will take place at {1} stands on {2} Tokens.", i_Drawing, dt_NextDrawing, i_Reward );
			else
				e.Mobile.SendMessage(32, "Current reward for drawing number {0} that will take place at {1} stands on {2} Tokens.", i_Drawing, dt_NextDrawing, i_Reward );
			
			if ( i_Drawing > 0 && i_Winner > 0 )
			{
				if (s_WinnerName == "Not claimed")
					e.Mobile.SendMessage(1073, "Ticket number {0} won {1} tokens in drawing number {2}.", i_Winner, i_WinnerReward, (i_Drawing - 1));
				else
					e.Mobile.SendMessage(1073, "{0} won {1} tokens at drawing number {2}.", s_WinnerName, i_WinnerReward, (i_Drawing - 1));
			}
		}    

		private static bool bEnableLottery = true;
		[CommandProperty(AccessLevel.Administrator)]
		public static bool EnableLottery { get { return bEnableLottery; } set { bEnableLottery = value; } }
		
		private static bool bEnableExchange = true;
		[CommandProperty(AccessLevel.Administrator)]
		public static bool EnableExchange { get { return bEnableExchange; } set { bEnableExchange = value; } }

		private static int iTokenCostInGold = 500;
		[CommandProperty(AccessLevel.Administrator)]
		public static int TokenCostInGold { get { return iTokenCostInGold; } set { iTokenCostInGold = value; } }

		private static DateTime dt_NextDrawing;

		[CommandProperty(AccessLevel.Administrator)]
		public DateTime NextDrawing { get { return dt_NextDrawing; } set { dt_NextDrawing = value; InvalidateProperties(); } }

		private static string s_WinnerName = "Not claimed";

		[CommandProperty(AccessLevel.GameMaster)]
		public string WinnerName { get { return s_WinnerName; } }
		//public string WinnerName { get { return s_WinnerName; } set { s_WinnerName = value; InvalidateProperties(); } }

		private static int i_Drawing, i_Reward, i_Ticket = 1, i_Winner = 0, i_TicketCost = 1000, i_DrawingDelay = 168, i_WinnerReward = 0;
		
		[CommandProperty(AccessLevel.GameMaster)]
		public int Drawing { get { return i_Drawing; } }
		//public int Drawing { get { return i_Drawing; } set { i_Drawing = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.GameMaster)]
		public int Reward { get { return i_Reward; } }
		//public int Reward { get { return i_Reward; } set { i_Reward = value; InvalidateProperties(); } }
		
		[CommandProperty(AccessLevel.Administrator)]
		public int TicketCost { get { return i_TicketCost; } set { i_TicketCost = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Administrator)]
		public int Ticket { get { return i_Ticket; } set { i_Ticket = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.GameMaster)]
		public int Winner { get { return i_Winner; } }
		//public int Winner { get { return i_Winner; } set { i_Winner = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Administrator)]
		public int DrawingDelay { get { return i_DrawingDelay; } set { i_DrawingDelay = value; InvalidateProperties(); } }
		
		[CommandProperty(AccessLevel.Administrator)]
		public int WinnerReward { get { return i_WinnerReward; } }
		//public int WinnerReward { get { return i_WinnerReward; } set { i_WinnerReward = value; InvalidateProperties(); } }
		
		[Constructable]
		public LadyLuck()
		{
			Name = "Lady Luck";
			Title = @"Tokens Lottery & Exchange";
			Body = 0x191;
			Frozen = true;
			CantWalk = true;
			Hue = 0x83F8;
			Direction = Direction.East;

			AddItem( new Server.Items.Sandals(2213) );
			AddItem( new Server.Items.FloppyHat( 2213 ) );
			AddItem( new Server.Items.FancyDress( 2213 ) );
			
			AddItem( new LongHair( 1153 ) );
			Blessed = true;

			if (i_Drawing == 0)
			{
				dt_NextDrawing = DateTime.Now + TimeSpan.FromHours( i_DrawingDelay );
				i_Drawing = 1;
			}
		}

		public LadyLuck( Serial serial ) : base( serial )
		{
		}

		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list )
		{ 
			base.GetContextMenuEntries( from, list ); 
			if (bEnableLottery)
				list.Add( new SellTicketEntry( from ) ); 
			if (bEnableExchange)
				list.Add( new ExchangeEntry( from ) );
		} 

		public virtual bool IsInvulnerable{ get{ return true; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 2 );

			//version 2
			writer.Write( (bool) bEnableLottery );
			writer.Write( (bool) bEnableExchange );
			writer.Write( (int) iTokenCostInGold );
			//version 1
			writer.Write( (int) i_WinnerReward );
			writer.Write( (string) s_WinnerName );
			//version 0
			writer.Write( (int) i_Drawing );
			writer.Write( (int) i_Reward );
			writer.Write( (int) i_Ticket );
			writer.Write( (int) i_Winner );
			writer.Write( (int) i_TicketCost );
			writer.Write( (DateTime)dt_NextDrawing );

			if (i_Reward == 0)
				i_Reward = 4500 + (i_Ticket*500);

			CheckDrawingTime();
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

			switch (version)
			{
				case 2:
				{
					bEnableLottery = reader.ReadBool();
					bEnableExchange = reader.ReadBool();
					iTokenCostInGold = reader.ReadInt();
					goto case 1;
				}
				case 1:
				{
					i_WinnerReward = reader.ReadInt();
					s_WinnerName = reader.ReadString();
					goto case 0;
				}
				case 0:
				{
					i_Drawing = reader.ReadInt();
					i_Reward = reader.ReadInt();
					i_Ticket = reader.ReadInt();
					i_Winner = reader.ReadInt();
					i_TicketCost = reader.ReadInt();
					dt_NextDrawing = reader.ReadDateTime();
					break;
				}
			}
			Frozen = true;
			CheckDrawingTime();
		}

		public static void CheckDrawingTime()
		{
			if ( DateTime.Now >= dt_NextDrawing )
				DrawingTime();
		}
		
		public static void DrawingTime()
		{
			if (i_Ticket == 1) //nobody bought a ticket
			{
				dt_NextDrawing = DateTime.Now + TimeSpan.FromHours( i_DrawingDelay );
				World.Broadcast( 38, true, "Since nobody bought any ticket for drawing {0} the drawing will continue.", i_Drawing);
			}
			else
			{
				s_WinnerName = "Not claimed";
				i_Winner = Utility.RandomMinMax(1, i_Ticket);

				i_WinnerReward = i_Reward;
				dt_NextDrawing = DateTime.Now + TimeSpan.FromHours( i_DrawingDelay );
				World.Broadcast( 38, true, "And the lucky ticket that won {0} tokens for drawing number {1} is {2}.", i_WinnerReward, i_Drawing, i_Winner);				
				i_Reward = 5000;
				i_Drawing++;
				i_Ticket = 1;
				World.Broadcast( 89, true, "The person that hold this ticket please hand it over to Lady Luck in order to get the reward, Thank You." );
				World.Broadcast( 66, true, "The next drawing will take place at {0}.", dt_NextDrawing );
			}
		}

		public class SellTicketEntry : ContextMenuEntry
		{
			private Mobile m_Mobile;
			
			public SellTicketEntry( Mobile from ) : base( 6103, 3 )
			{
				m_Mobile = from;
			}

			public override void OnClick()
			{
				if( !( m_Mobile is PlayerMobile ) )
					return;
				
				PlayerMobile from = (PlayerMobile) m_Mobile;
				from.CloseGump( typeof( LadyLuckSellingGump ) ); 
				from.SendGump( new LadyLuckSellingGump( from ) );
			}
		}

		public class ExchangeEntry : ContextMenuEntry
		{
			private Mobile m_Mobile;
			//3000143
			//3006113 transfer
			public ExchangeEntry( Mobile from ) : base( 6113, 3 )
			{
				m_Mobile = from;
			}

			public override void OnClick()
			{
				if( !( m_Mobile is PlayerMobile ) )
					return;
				
				PlayerMobile from = (PlayerMobile) m_Mobile;
				from.CloseGump( typeof( LadyLuckExchangeGump ) ); 
				from.SendGump( new LadyLuckExchangeGump( from ) );
			}
		}

		public override bool OnDragDrop( Mobile from, Item dropped )
		{          		
			CheckDrawingTime();
			Mobile m = from;
			PlayerMobile mobile = m as PlayerMobile;

			if ( mobile != null)
			{
				if( dropped is LotteryTicket )
				{
					LotteryTicket ticket = dropped as LotteryTicket;
					if ( ((LotteryTicket)ticket).DrawingNumber == ( i_Drawing - 1 ) ) //last drawing
					{
						if ( ((LotteryTicket)ticket).StartTicketNumber <= i_Winner && i_Winner <= ((LotteryTicket)ticket).EndTicketNumber) //winner
						{
							switch (Utility.Random(3))
							{
								case 0: this.PrivateOverheadMessage( MessageType.Regular, 1153, false, "I am very Pleased to Inform you that you have won!!!", mobile.NetState ); break;
								case 1: this.PrivateOverheadMessage( MessageType.Regular, 1153, false, "Congratulations!!! You have won!!!", mobile.NetState ); break;
								case 2: this.PrivateOverheadMessage( MessageType.Regular, 1153, false, "YAY! We have a winner!", mobile.NetState ); break;
							}
							m.AddToBackpack(new TokenCheck(i_WinnerReward));
							m.AddToBackpack(new FireworksWand());
							if (m.Name == null)
								s_WinnerName = "John Doe";
							else
								s_WinnerName = m.Name;
							World.Broadcast( 88, true, "{0} claimed {1} tokens from drawing {2},  Congratulations.", s_WinnerName, i_WinnerReward, (i_Drawing - 1) );
							World.Broadcast( 66, true, "Drawing number {0} will take place at {1}.", i_Drawing, dt_NextDrawing );
							new Celebration( m.Map, m ).Start();
						}
						else //looser
						{
							switch (Utility.Random(3))
							{
								case 0: this.PrivateOverheadMessage( MessageType.Regular, 1153, false, "Sorry, The Gods are not with you today. Try again next time.", mobile.NetState ); break;
								case 1: this.PrivateOverheadMessage( MessageType.Regular, 1153, false, "You are not a winner.", mobile.NetState ); break;
								case 2: this.PrivateOverheadMessage( MessageType.Regular, 1153, false, "You are a big Loser!", mobile.NetState ); break;
							}
						}
						dropped.Delete();
						return true;
					}
					else if ( ((LotteryTicket)ticket).DrawingNumber == i_Drawing ) //drowing isn't over
					{
						switch (Utility.Random(3))
						{
							case 0: this.PrivateOverheadMessage( MessageType.Regular, 1153, false, "I am sorry, the drawing is not yet closed.", mobile.NetState ); break;
							case 1: this.PrivateOverheadMessage( MessageType.Regular, 1153, false, "This drawing is not over yet.", mobile.NetState ); break;
							case 2: this.PrivateOverheadMessage( MessageType.Regular, 1153, false, "Please come back at the end of drawing to see if you have won.", mobile.NetState ); break;
						}
					}
					else //old drawings
					{
						switch (Utility.Random(3))
						{
							case 0: this.PrivateOverheadMessage( MessageType.Regular, 1153, false, "I am very sorry, That ticket has expired.", mobile.NetState ); break;
							case 1: this.PrivateOverheadMessage( MessageType.Regular, 1153, false, "That Ticket is no longer any good.", mobile.NetState ); break;
							case 2: this.PrivateOverheadMessage( MessageType.Regular, 1153, false, "Do not try to fool me With outdated Tickets!!!", mobile.NetState ); break;
						}
						dropped.Delete();
					}
					return false;
				}
				else
				{
					this.PrivateOverheadMessage( MessageType.Regular, 1153, false, "I have no need of this!", mobile.NetState );
				}
			}
			return false;
		}

		private class Celebration : Timer
		{
			private Map m_Map;
			private int m_Count;
			Mobile m_From;

			public Celebration( Map map, Mobile from ) : base( TimeSpan.FromSeconds( 1 ), TimeSpan.FromSeconds( 1 ) )
			{
				m_Map = map;
				m_Count = 30;
				m_From = from;
			}

			protected override void OnTick()
			{
				if (m_Count > 0)
				{
					Point3D ourLoc = m_From.Location;
					Point3D startLoc = new Point3D( ourLoc.X, ourLoc.Y, ourLoc.Z + 10 );
					Point3D endLoc = new Point3D( startLoc.X + Utility.RandomMinMax( -2, 2 ), startLoc.Y + Utility.RandomMinMax( -2, 2 ), startLoc.Z + 20 );
					Effects.SendMovingEffect( new Entity( Serial.Zero, startLoc, m_Map ), new Entity( Serial.Zero, endLoc, m_Map ), 0x36E4, 5, 0, false, false );
					Timer.DelayCall( TimeSpan.FromSeconds( 1.0 ), new TimerStateCallback( FinishCelebration ), new object[]{ m_From, endLoc, m_Map } );
					m_Count--;
				}
				else Stop();
			}
		}
		private static void FinishCelebration( object state )
		{
			object[] states = (object[])state;

			Mobile from = (Mobile)states[0];
			Point3D endLoc = (Point3D)states[1];
			Map map = (Map)states[2];

			int hue = Utility.Random( 40 );

			if ( hue < 8 )
				hue = 0x66D;
			else if ( hue < 10 )
				hue = 0x482;
			else if ( hue < 12 )
				hue = 0x47E;
			else if ( hue < 16 )
				hue = 0x480;
			else if ( hue < 20 )
				hue = 0x47F;
			else
				hue = 0;

			if ( Utility.RandomBool() )
				hue = Utility.RandomList( 0x47E, 0x47F, 0x480, 0x482, 0x66D );

			int renderMode = Utility.RandomList( 0, 2, 3, 4, 5, 7 );

			Effects.PlaySound( endLoc, map, Utility.Random( 0x11B, 4 ) );
			Effects.SendLocationEffect( endLoc, map, 0x373A + (0x10 * Utility.Random( 4 )), 16, 10, hue, renderMode );
		}


		public class LadyLuckExchangeGump : Gump
		{
			private Mobile m_From;
			private int iCost;
			
			public LadyLuckExchangeGump( Mobile from ) : base(0, 0)
			{
				m_From = from;
				if (!(m_From is PlayerMobile))
					return;
				
				Resizable=false;
				AddPage(0);
				AddImage(5, 5, 1140);
				AddLabel(80, 65, 2213, "1 Token = " + iTokenCostInGold + " Gold");
				AddLabel(88, 90, 89, "How many you want to exchange?");
				AddLabel(75, 115, 67, "Buy:");
				AddTextEntry(135, 131, 90, 20, 2113, 1, "1");
				AddLabel(75, 145, 67, "Sell:");
				AddLabel(235, 130, 67, "Tokens.");
				AddRadio(100, 110, 9727, 9730, true, 1 );
				AddRadio(100, 140, 9727, 9730, false, 0 );
				AddButton(290, 125, 2642, 2643, 3, GumpButtonType.Reply, 0);

			}
			public override void OnResponse( NetState state, RelayInfo info ) 
			{
				if ( info.ButtonID == 3 )
				{
					TextRelay tr_Exchange = info.GetTextEntry( 1 );
					if(tr_Exchange != null)
					{
						int iTokensExchange = 0;
						try
						{
							iTokensExchange = Convert.ToInt32(tr_Exchange.Text,10);
						} 
						catch
						{
							m_From.SendMessage("Please make sure you wrote only numbers.");
							m_From.SendGump( new LadyLuckSellingGump( m_From ) );
						}
						if (iTokensExchange <= 0)
							return;
						iCost = iTokensExchange*iTokenCostInGold;
						if ( iCost > 1000000 )
						{
							m_From.SendMessage("There's a 1 mil gold limit per transfer, please try a smaller exchange.");
							return;
						}
						if (info.IsSwitched(1)) //buy tokens
						{
							if (m_From.BankBox.ConsumeTotal(typeof(Gold), iCost))
							{
								m_From.AddToBackpack( new TokenCheck(iTokensExchange) );
								m_From.CloseGump( typeof( LadyLuckExchangeGump ) );
								m_From.SendMessage(1173, "You bought {0} tokens for {1} gold.", iTokensExchange, iCost );
							}
							else
							{
								m_From.PlaySound(1069); //play Hey!! sound
								m_From.SendMessage(32, "Hey, don't try to rob the bank!!! You don't have enough gold in your bank box!!!");
								m_From.CloseGump( typeof( LadyLuckExchangeGump ) );
							}
						}
						else
						{
							if (daat99.MasterStorageUtils.TakeTokensFromPlayer(m_From as PlayerMobile, iTokensExchange))
							{
								m_From.AddToBackpack( new BankCheck(iCost) );
								m_From.CloseGump( typeof( LadyLuckExchangeGump ) );
								m_From.SendMessage(1173, "You sold {0} tokens for {1} gold.", iTokensExchange, iCost );
							}
						}
					}
				}
			}
		}

		public class LadyLuckSellingGump : Gump
		{
			private Mobile m_From;
			
			public LadyLuckSellingGump( Mobile from ) : base(0, 0)
			{
				m_From = from;
				if (!(m_From is PlayerMobile))
					return;
				
				Resizable=false;
				AddPage(0);
				AddImage(5, 5, 1140);
				AddLabel(80, 65, 2213, @"Each lottery ticket cost " + i_TicketCost + " tokens.");
				AddLabel(88, 90, 89, @"How many tickets you want to buy ?");
				AddLabel(80, 130, 67, @"Buy:");
				AddLabel(215, 130, 67, @"tickets.");
				AddTextEntry(120, 132, 80, 20, 2113, 1, "1");
				AddButton(275, 130, 2644, 2645, 2, GumpButtonType.Reply, 0);
			}
			public override void OnResponse( NetState state, RelayInfo info ) 
			{
				if ( m_From.Deleted ) 
					return; 
				if ( info.ButtonID == 2 )
				{
					TextRelay tr_BuyTickets = info.GetTextEntry( 1 );
					if(tr_BuyTickets != null)
					{
						int i_BuyTickets = 0;
						try
						{
							i_BuyTickets = Convert.ToInt32(tr_BuyTickets.Text,10);
						} 
						catch
						{
							m_From.SendMessage("Please make sure you wrote only numbers.");
							m_From.SendGump( new LadyLuckSellingGump( m_From ) );
						}
						if ( i_BuyTickets <= 0 ) 
							return;
						else if (i_BuyTickets >= 1000000)
						{
							m_From.SendMessage(32, "you can't buy more then 999,999 tickets at the same time");
							return;
						}
						else
						{
							if (TokenSystem.GiveTokensToPlayer(m_From as PlayerMobile, (i_BuyTickets * i_TicketCost), true))
							{
								LotteryTicket lottery = new LotteryTicket();
								lottery.DrawingNumber = i_Drawing;
								lottery.StartTicketNumber = i_Ticket;
								lottery.EndTicketNumber = (i_Ticket + i_BuyTickets - 1);
								m_From.AddToBackpack(lottery);

								i_Ticket = (i_Ticket + i_BuyTickets);
								m_From.CloseGump(typeof(LadyLuckSellingGump));
								m_From.SendMessage("You bought {0} lottery tickets.", i_BuyTickets);
								i_Reward = (i_Reward + (i_BuyTickets * (i_TicketCost / 2)));
							}
						}
					}
				}
			}
		}
	}

	public class LotteryTicket : Item
	{
		private int i_DrawingNumber, i_StartTicketNumber, i_EndTicketNumber;
		
		//[CommandProperty(AccessLevel.Administrator)]
		//[CommandProperty(AccessLevel.Administrator)]
		public int DrawingNumber { get { return i_DrawingNumber; } set { i_DrawingNumber = value; InvalidateProperties(); } }
		
		//[CommandProperty(AccessLevel.Administrator)]
		//[CommandProperty(AccessLevel.Administrator)]
		public int StartTicketNumber { get { return i_StartTicketNumber; } set { i_StartTicketNumber = value; InvalidateProperties(); } }

		//[CommandProperty(AccessLevel.Administrator)]
		//[CommandProperty(AccessLevel.Administrator)]
		public int EndTicketNumber { get { return i_EndTicketNumber; } set { i_EndTicketNumber = value; InvalidateProperties(); } }
		
		public LotteryTicket() : base(5359)
		{
			Stackable = true;
			Name = "Lottery Ticket";
			Hue = 3;
			Weight = 1.0;
			Stackable = false;
			LootType = LootType.Blessed;
		}

		public LotteryTicket( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			
			writer.Write( (int) 0 ); // version
			//version 0
			writer.Write( i_DrawingNumber );
			writer.Write( i_StartTicketNumber );
			writer.Write( i_EndTicketNumber );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			//version 0
			i_DrawingNumber = reader.ReadInt();
			i_StartTicketNumber = reader.ReadInt();
			i_EndTicketNumber = reader.ReadInt();
			
			LootType = LootType.Blessed;
		}

		public override void AddNameProperties( ObjectPropertyList list )
		{
			base.AddNameProperties( list );
			if ( i_StartTicketNumber == i_EndTicketNumber )
				list.Add( 1060663, "Drawing\t{0}, Ticket: {1}", i_DrawingNumber, i_StartTicketNumber );
			else
				list.Add( 1060659, "Drawing\t{0}, Tickets: {1} through {2}", i_DrawingNumber, i_StartTicketNumber, i_EndTicketNumber );
		}
	}
}