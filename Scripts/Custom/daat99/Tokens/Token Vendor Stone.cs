/*
 Created by:
	Raelis
 Modified by:
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
using System.Collections;
using Server.Items; 
using Server.Network; 
using Server.Gumps; 
using Server.Mobiles; 
using Server.Targeting;

namespace Server.Items
{
	public class VSItem
	{
		private string m_Item = "";
		private string m_Name = "";
		private int m_Price = 0;
		private int m_Amount = 1;
		private bool m_BlessBond;
		private int m_BBPrice;
		private string m_Description = "";

		public string Item{ get{ return m_Item; } set{ m_Item = value; } }
		public string Name{ get{ return m_Name; } set{ m_Name = value; } }
		public int Price{ get{ return m_Price; } set{ m_Price = value; } }
		public int Amount{ get{ return m_Amount; } set{ m_Amount = value; if ( m_Amount < 1 ) m_Amount = 1; } }
		public bool BlessBond{ get{ return m_BlessBond; } set{ m_BlessBond = value; } }
		public int BBPrice{ get{ return m_BBPrice; } set{ m_BBPrice = value; } }
		public string Description{ get{ return m_Description; } set{ m_Description = value; } }

		public VSItem()
		{
		}

		public VSItem( string item, string name, int price, int amount, bool blessbond, int bbprice, string description )
		{
			Item = item;
			Name = name;
			Price = price;
			Amount = amount;
			BlessBond = blessbond;
			BBPrice = bbprice;
			Description = description;
		}

		public Type GetItemType()
		{
			Type type = ScriptCompiler.FindTypeByName( m_Item );
			return type;
		}

		public void Serialize( GenericWriter writer )
		{
			writer.Write( (string) m_Description );
			writer.Write( (bool) m_BlessBond );
			writer.Write( (int) m_BBPrice );
			writer.Write( (string) m_Item );
			writer.Write( (string) m_Name );
			writer.Write( (int) m_Price );
			writer.Write( (int) m_Amount );
		}

		public void Deserialize( GenericReader reader, int version )
		{
			switch( version )
			{
				case 3:
				{
					m_Description = reader.ReadString();

					goto case 2;
				}
				case 2:
				case 1:
				{
					m_BlessBond = reader.ReadBool();
					m_BBPrice = reader.ReadInt();

					goto case 0;
				}
				case 0:
				{
					m_Item = reader.ReadString();
					m_Name = reader.ReadString();
					m_Price = reader.ReadInt();
					m_Amount = reader.ReadInt();

					break;
				}
			}
		}
	}
	public class VSShopper
	{
		private ArrayList m_ItemList = new ArrayList();
		private Mobile m_Owner;
		private TokenVendorStone m_Stone;

		public ArrayList ItemList{ get{ return m_ItemList; } set{ m_ItemList = value; } }
		public Mobile Owner{ get{ return m_Owner; } set{ m_Owner = value; } }
		public TokenVendorStone Stone{ get{ return m_Stone; } set{ m_Stone = value; } }

		public VSShopper( Mobile owner, TokenVendorStone stone )
		{
			m_Owner = owner;
			m_Stone = stone;
		}

		public void AddItem( int itemnumber )
		{
			m_ItemList.Add( itemnumber );
		}

		public int TotalPrice()
		{
			int tp = 0;

			for( int i = 0; i < m_ItemList.Count; ++i )
			{
				int n = (int)ItemList[i];

				VSItem vsi = (VSItem)m_Stone.ItemList[n];
				tp += vsi.Price;
			}

			return tp;
		}
	}
	public class VendorBall : Item
	{
		private TokenVendorStone m_Stone;

		//[CommandProperty( AccessLevel.GameMaster )]
		public TokenVendorStone Stone{ get{ return m_Stone; } set{ m_Stone = value; } }

		[Constructable]
		public VendorBall() : base( 0x1869 )
		{
			Weight = 0.1;
			Hue = 89; 
			Name = "Vendor Ball";
		}

		public VendorBall( Serial serial ) : base( serial )
		{
		}

		private void ConnectTarget_Callback( Mobile from, object obj ) 
		{ 
			if ( obj is TokenVendorStone && obj is Item ) 
			{ 
				Item item = (Item)obj;
				TokenVendorStone ps = (TokenVendorStone)obj;

				Stone = ps;
				from.SendMessage( "You have connected the ball to the stone." );
			} 
			else 
			{ 
				from.SendMessage( "That is an invalid target!" );
			}
		}

		public override void OnDoubleClick( Mobile from ) 
		{ 
			if ( !IsChildOf( from ) && !Utility.InRange( from.Location, Location, 3 ) )
				from.SendMessage( "You are too far away to use that." );
			else if ( Stone != null && !Stone.Deleted )
			{
				if ( from.AccessLevel >= Stone.AccessLevel )
				{
					from.SendGump( new StaffVendorGump( from, Stone ) );
				}
				else if ( !Stone.EditMode )
				{
					if ( !Stone.EditMode )
						from.SendGump( new VendorGump( new VSShopper( from, m_Stone ), Stone ) );
					else
						from.SendMessage( "This vendor stone is currently in edit mode." );
				}
				else
					from.SendMessage( "This stone is in edit mode." );
			}
			else
			{
				from.BeginTarget( -1, false, TargetFlags.Beneficial, new TargetCallback( ConnectTarget_Callback ) );
				from.SendMessage( "Please target a stone to connect this ball to." );
			}
		}

		public override void OnSingleClick( Mobile from )
		{
			base.OnSingleClick( from );

			if ( Stone != null && !Stone.Deleted )
			{
				if ( Stone.Name != null )
					LabelTo( from, "Stone Ball, Connected to: "+ Stone.Name );
				else
					LabelTo( from, "Stone Ball, Connected to: !SET THE STONE'S NAME!" );
			}
			else
			{
				LabelTo( from, "Stone Ball, Connected to: None" );
			}
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( Stone != null && !Stone.Deleted )
			{
				if ( Stone.Name != null )
					list.Add( "Stone Ball, Connected to: "+ Stone.Name );
				else
					list.Add( "Stone Ball, Connected to: !SET THE STONE'S NAME!" );
			}
			else
			{
				list.Add( "Stone Ball, Connected to: None" );
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
			writer.Write( m_Stone );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			m_Stone = reader.ReadItem() as TokenVendorStone;
		}
	}
	public class TokenVendorStone : Item 
	{ 
		private AccessLevel m_AccessLevel = AccessLevel.Administrator;
		private bool m_EditMode;
		private string m_Currency;
		private ArrayList m_ItemList = new ArrayList();

		public string Currency{get{return m_Currency;}set{m_Currency = value;}}
		public ArrayList ItemList{get{return m_ItemList;}set{m_ItemList = value;}}
		public AccessLevel AccessLevel{get{return m_AccessLevel;}set{m_AccessLevel = value;}}
		public bool EditMode{get{return m_EditMode;}set{m_EditMode = value;}}

		[Constructable] 
		public TokenVendorStone() : base( 3806 ) 
		{ 
			Movable = false; 
			Hue = 1173; 
			Name = "Vendor Stone";
			Currency = "daat99tokens";
		} 

		public override void OnDoubleClick( Mobile from ) 
		{ 
			if ( !Utility.InRange( from.Location, Location, 3 ) )
				from.SendMessage( "You are too far away to use that." );
			else if ( from.AccessLevel >= AccessLevel )
				from.SendGump( new StaffVendorGump( from, this ) );
			else if ( !EditMode )
				from.SendGump( new VendorGump( new VSShopper( from, this ), this ) );
			else
				from.SendMessage( "This stone is in edit mode." );
		}

		public void CloseGumps( Mobile from )
		{
			from.CloseGump( typeof( VendorGump ) );
			from.CloseGump( typeof( TokenVendorStoneBuyGump ) );
			from.CloseGump( typeof( TokenVendorStoneEditGump ) );
			from.CloseGump( typeof( TokenVendorStoneAddItemGump ) );
			from.CloseGump( typeof( StaffVendorGump ) );
		}

		public Type GetCurrency()
		{
			Type type = ScriptCompiler.FindTypeByName( Currency );
			return type;
		}

		public TokenVendorStone( Serial serial ) : base( serial ) 
		{ 
		} 

		public override void Serialize( GenericWriter writer ) 
		{ 
			base.Serialize( writer ); 

			writer.Write( (int) 3 ); // version 
			writer.Write( (int)m_AccessLevel );
			writer.Write( (string)m_Currency );
			writer.Write( (bool)m_EditMode );

			writer.Write( m_ItemList.Count );
			for ( int i = 0; i < m_ItemList.Count; ++i )
				((VSItem)m_ItemList[i]).Serialize( writer );
		}

		public override void Deserialize( GenericReader reader ) 
		{ 
			base.Deserialize( reader ); 

			int version = reader.ReadInt(); 

			if (version == 0)
			{
				bool bNull;
				int iNull;
				bNull = reader.ReadBool(); //custom hues
				bNull = reader.ReadBool(); //m_Blessed
				bNull = reader.ReadBool(); //m_Bonded
				bNull = reader.ReadBool(); //m_Hued
				iNull = reader.ReadInt(); //m_BlessedPrice
				iNull = reader.ReadInt(); //m_BondedPrice
				iNull = reader.ReadInt(); //m_HuedPrice
				m_AccessLevel = (AccessLevel)reader.ReadInt(); 
				m_Currency = reader.ReadString();
				m_EditMode = reader.ReadBool(); 

				int size1 = reader.ReadInt();
				ArrayList alPrice = new ArrayList( size1 );
				for ( int i = 0; i < size1; ++i )
				{
					int price = reader.ReadInt();
					alPrice.Add( price );
				}

				int size4 = reader.ReadInt();
				ArrayList alNull = new ArrayList( size4 );
				for ( int i = 0; i < size4; ++i )
				{
					int hue = reader.ReadInt();
					alNull.Add( hue );
				}

				int size5 = reader.ReadInt();
				ArrayList alAmount = new ArrayList( size5 );
				for ( int i = 0; i < size5; ++i )
				{
					int itemamount = reader.ReadInt();
					alAmount.Add( itemamount );
				}

				int size2 = reader.ReadInt();
				ArrayList alItem = new ArrayList( size2 );
				for ( int i = 0; i < size2; ++i )
				{
					string item = reader.ReadString();
					alItem.Add( item );
				}

				int size3 = reader.ReadInt();
				ArrayList alName = new ArrayList( size3 );
				for ( int i = 0; i < size3; ++i )
				{
					string gumpname = reader.ReadString();
					alName.Add( gumpname );
				}

				int size6 = reader.ReadInt();
				alNull = new ArrayList( size6 );
				for ( int i = 0; i < size6; ++i )
				{
					int hueprices = reader.ReadInt();
					alNull.Add( hueprices );
				}
				//dispose of the not used arrays (bless.. bond...)
				alNull.Clear();
				for( int i = 0; i < alName.Count; i++ )
				{
					VSItem v = new VSItem( alItem[i].ToString(), alName[i].ToString(), (int)alPrice[i], (int)alAmount[i], false, 0, "" );
					ItemList.Add( v );
				}
				//dispose of the "old" arrays
				alItem.Clear();
				alName.Clear();
				alPrice.Clear();
				alAmount.Clear();
			}
			else switch( version )
			{
				case 3:
				case 2:goto case 0;
				case 1:
				{
					bool blah = reader.ReadBool(); //for the usesledger

					goto case 0;
				}
				case 0:
				{
					m_AccessLevel = (AccessLevel)reader.ReadInt(); 
					m_Currency = reader.ReadString();
					m_EditMode = reader.ReadBool(); 

					int size = reader.ReadInt();
					m_ItemList = new ArrayList( size );
					for ( int i = 0; i < size; ++i )
					{
						VSItem vsi = new VSItem();
						vsi.Deserialize( reader, version );
						m_ItemList.Add( vsi );
					}

					break;
				} 
			} 
		} 
	} 
} 

namespace Server.Gumps
{ 
	public class VendorGump : Gump
	{
                private TokenVendorStone m_Stone;
                private VSShopper m_Shopper;

		private const int AmountPerPage = 20; 

		public string Center( string text )
		{
			return String.Format( "<CENTER>{0}</CENTER>", text );
		}

		public string Color( string text, int color )
		{
			return String.Format( "<BASEFONT COLOR=#{0:X6}>{1}</BASEFONT>", color, text );
		}

                public VendorGump( VSShopper shopper, TokenVendorStone stone ) : base( 25, 25 )
                {
			m_Stone = stone;
			m_Shopper = shopper;

			m_Stone.CloseGumps( m_Shopper.Owner );

			AddPage( 0 ); 

			AddBackground( 0, 0, 550, 480, 0x2436 );
			AddAlphaRegion( 10, 10, 530, 460 );

			AddImageTiled( 10, 40, 530, 5, 2624 );
			AddImageTiled( 10, 58, 390, 5, 2624 );

			AddImageTiled( 90, 40, 5, 430, 2624 );
			AddImageTiled( 125, 40, 5, 430, 2624 );
			AddImageTiled( 160, 40, 5, 430, 2624 );
			AddImageTiled( 310, 40, 5, 430, 2624 );
			AddImageTiled( 400, 40, 5, 430, 2624 );

			if ( m_Stone.Name != null && m_Stone.Name != "" )
				AddHtml( 10, 10, 510, 20, Color( Center( m_Stone.Name ), 0xFFFFFF ), false, false );
			else
				AddHtml( 10, 10, 510, 20, Color( Center( "Vendor Stone" ), 0xFFFFFF ), false, false );

			AddLabel( 420, 60, 5, "Stone Currency:" );
			if ( m_Stone.Currency.ToLower() == "daat99tokens"  )
				AddLabel( 420, 80, 5, "Token Ledger" );
			else if ( m_Stone.Currency != null  )
				AddLabel( 420, 80, 5, m_Stone.Currency );
			else
				AddLabel( 420, 80, 33, "None" );

			AddHtml( 10, 42, 85, 20, Color( Center( "Item Amount" ), 0xFFFFFF ), false, false );
			AddHtml( 95, 42, 30, 20, Color( Center( ((m_Stone.EditMode && shopper.Owner.AccessLevel >= m_Stone.AccessLevel) ? "Edit" : "Buy") ), 0xFFFFFF ), false, false );
			AddHtml( 130, 42, 30, 20, Color( Center( "Des" ), 0xFFFFFF ), false, false );
			AddHtml( 165, 42, 145, 20, Color( Center( "Item" ), 0xFFFFFF ), false, false );
			AddHtml( 315, 42, 85, 20, Color( Center( "Price" ), 0xFFFFFF ), false, false );

			if ( !m_Stone.EditMode )
			{
				AddButton( 420, 120, 1209, 1210, 1, GumpButtonType.Reply, 0 );
				AddLabel( 440, 120, 1152, "Buy Items" );
			}

			AddLabel( 410, 160, 1152, "The (B) beside" );
			AddLabel( 410, 175, 1152, "items and creatures" );
			AddLabel( 410, 190, 1152, "stands for blessing" );
			AddLabel( 410, 205, 1152, "or bonding." );


			AddLabel( 420, 400, 906, "Created By" );
			AddLabel( 420, 420, 906, "~Raelis~" );

			AddButton( 420, 440, 4005, 4007, 0, GumpButtonType.Reply, 0 ); 
			AddLabel( 450, 440, 33, "Close" );

			int index = 0;
			int page = 1;

			AddPage( 1 );

			for ( int i = 0; i < m_Stone.ItemList.Count; ++i )
			{
				if ( index >= AmountPerPage )
				{ 
					AddButton( 420, 365, 0x1196, 0x1196, 1152, GumpButtonType.Page, page + 1 );
					AddLabel( 420, 350, 1152, "Next page" ); 

					++page; 
					index = 0; 

					AddPage( page );

					AddButton( 420, 315, 0x119a, 0x119a, 1152, GumpButtonType.Page, page - 1 );
					AddLabel( 420, 300, 1152, "Previous page" ); 
				}

				int price = ((VSItem)m_Stone.ItemList[i]).Price;
				int amount = ((VSItem)m_Stone.ItemList[i]).Amount;
				string gumpname = ((VSItem)m_Stone.ItemList[i]).Name;

				AddLabel( 165, 60 + (index * 20), 1152, gumpname +( ((VSItem)m_Stone.ItemList[i]).BlessBond ? " (B)" : "") );
				AddLabel( 25, 60 + (index * 20), 1152, ""+ amount );
				AddLabel( 320, 60 + (index * 20), 1152, ""+ price );

				AddButton( 100, 65 + (index * 20), 1209, 1210, i+2, GumpButtonType.Reply, 0 );
				if ( ((VSItem)m_Stone.ItemList[i]).Description != null && ((VSItem)m_Stone.ItemList[i]).Description != "" )
					AddButton( 135, 65 + (index * 20), 0x1523, 0x1523, i+m_Stone.ItemList.Count+3, GumpButtonType.Reply, 0 );

				AddImageTiled( 10, 80 + (index * 20), 390, 3, 2624 );

				index++;
			}
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{ 
			Mobile from = state.Mobile;

			if ( m_Stone.Deleted )
				return;

			if ( info.ButtonID == 0 )
			{
				from.SendMessage( "You decide not to buy anything." );
			}

			if ( info.ButtonID == 1 )
			{
				from.SendGump( new TokenVendorStoneBuyGump( m_Shopper, m_Stone ) );
			}

			if ( info.ButtonID-m_Stone.ItemList.Count-2 <= m_Stone.ItemList.Count && info.ButtonID >= 3 && info.ButtonID-2 > m_Stone.ItemList.Count )
			{
				from.SendGump( new VendorGump( m_Shopper, m_Stone ) );
				if ( ((VSItem)m_Stone.ItemList[info.ButtonID-m_Stone.ItemList.Count-3]).Description != null && ((VSItem)m_Stone.ItemList[info.ButtonID-m_Stone.ItemList.Count-3]).Description != "" )
					from.SendGump( new TokenVendorStoneDescriptionGump( from, ((VSItem)m_Stone.ItemList[info.ButtonID-m_Stone.ItemList.Count-3]).Description, ((VSItem)m_Stone.ItemList[info.ButtonID-m_Stone.ItemList.Count-3]).Name ) );
			}
			else if ( info.ButtonID-1 <= m_Stone.ItemList.Count && info.ButtonID >= 2 )
			{
				if ( !m_Stone.EditMode )
				{
					m_Shopper.AddItem( info.ButtonID-2 );
					from.SendMessage( "You add it to your list." );
					from.SendGump( new VendorGump( m_Shopper, m_Stone ) );
				}
				else
				{
					if ( info.ButtonID-2 > m_Stone.ItemList.Count-1 || info.ButtonID-2 < 0 )
						return;

					m_Stone.CloseGumps( from );
					from.SendGump( new TokenVendorStoneEditGump( from, (VSItem)m_Stone.ItemList[info.ButtonID-2], m_Stone ) );
				}
			}
		}
	}
	public class TokenVendorStoneBuyGump : Gump
	{
		private VSShopper m_Shopper;
		private TokenVendorStone m_Stone;

                public TokenVendorStoneBuyGump( VSShopper shopper, TokenVendorStone stone ) : base( 125, 125 )
                {
			m_Shopper = shopper;
			m_Stone = stone;

			m_Stone.CloseGumps( m_Shopper.Owner );

			AddPage( 0 ); 

			AddBackground( 0, 0, 375, 210, 0x2436 );

			AddLabel( 12, 17, 5, "Your Buy List" );
			AddLabel( 110, 22, 5, "Total Purchase: "+m_Shopper.TotalPrice() );

			AddButton( 15, 160, 4005, 4007, 0, GumpButtonType.Reply, 0 ); 
			AddLabel( 45, 160, 33, "Back" );
			AddButton( 110, 160, 4005, 4007, 1, GumpButtonType.Reply, 0 ); 
			AddLabel( 140, 160, 1152, "Buy" );

			for ( int i = 0; i < m_Shopper.ItemList.Count; ++i )
			{
				int item = (int)m_Shopper.ItemList[i];

				if ( (i % 5) == 0 )
				{
					if ( i != 0 )
					{
						AddButton( 190, 235, 0x1196, 0x1196, 1152, GumpButtonType.Page, (i / 5) + 1 );
						AddLabel( 190, 220, 1152, "Next page" ); 
					}

					AddPage( (i / 5) + 1 );

					if ( i != 0 )
					{
						AddButton( 10, 235, 0x119a, 0x119a, 1152, GumpButtonType.Page, (i / 5) );
						AddLabel( 10, 220, 1152, "Previous page" ); 
					}
				}

				VSItem vsi = (VSItem)m_Stone.ItemList[item];

				string blessbond = (vsi.BlessBond ? "(B)" : "");

				AddButton( 13, 40 + ((i % 5) * 20), 0x26AF, 0x26B1, i+2, GumpButtonType.Reply, 0 ); 
				if ( m_Stone.Currency.ToLower() == "daat99tokens" )
					AddLabel( 40, 40 + ((i % 5) * 20), 1152, vsi.Amount+" "+vsi.Name+" "+blessbond+" "+vsi.Price+" Token Ledger Tokens" );
				else
					AddLabel( 40, 40 + ((i % 5) * 20), 1152, vsi.Amount+" "+vsi.Name+" "+blessbond+" "+vsi.Price+" "+m_Stone.Currency );
			}
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{ 
			Mobile from = state.Mobile;

			if ( from.Backpack == null || from.BankBox == null )
				return;

			if ( info.ButtonID == 0 )
			{
				from.SendGump( new VendorGump( m_Shopper, m_Stone ) );
			}
			if ( info.ButtonID == 1 )
			{
				if ( m_Stone.GetCurrency() == null && m_Shopper.TotalPrice() > 0 )
					from.SendMessage( "There was no set currency to this stone." );
				else if ( m_Shopper.ItemList.Count <= 0 )
				{
					from.SendMessage( "You did not select anything to buy." );
					from.SendGump( new TokenVendorStoneBuyGump( m_Shopper, m_Stone ) );
				}
				else if ( m_Stone.Currency.ToLower() == "daat99tokens" )
				{
					if (!TokenSystem.TakePlayerTokens(from as PlayerMobile, m_Shopper.TotalPrice(), true))
						return;

					for( int i = 0; i < m_Shopper.ItemList.Count; ++i )
					{
						int n = (int)m_Shopper.ItemList[i];

						VSItem vsi = (VSItem)m_Stone.ItemList[n];

						Type type = vsi.GetItemType();

						if ( type != null )
						{
							object o = Activator.CreateInstance( type );

							if ( o is Mobile )
							{
								Mobile m = (Mobile)o;

								ArrayList items = new ArrayList();

								m.MoveToWorld( from.Location, from.Map );
								if ( m is BaseCreature )
								{
									BaseCreature c = (BaseCreature)m;
									c.ControlMaster = from;
									c.Controlled = true;
									c.ControlTarget = from;
									c.ControlOrder = OrderType.Follow;

									items.Add( c );

									if ( vsi.BlessBond )
										from.SendGump( new TokenVendorStoneBlessBondGump( from, vsi, m_Stone, items ) );
								}
								int total = 0;
								while( vsi.Amount-1 > total )
								{
									object o2 = Activator.CreateInstance( type );
									Mobile m2 = (Mobile)o2;

									m2.MoveToWorld( from.Location, from.Map );
									if ( m2 is BaseCreature )
									{
										BaseCreature c = (BaseCreature)m2;
										c.ControlMaster = from;
										c.Controlled = true;
										c.ControlTarget = from;
										c.ControlOrder = OrderType.Follow;

										items.Add( c );
									}
									total++;
								}
							}
							if ( o is Item )
							{
								Item item = (Item)o;

								ArrayList items = new ArrayList();

								items.Add( item );

								if ( item.Stackable )
								{
									if ( vsi.Amount > 60000 )
									{
										item.Amount = 60000;

										int aleft = vsi.Amount-60000;

										while( aleft > 0 )
										{
											object o2 = Activator.CreateInstance( type );
											if ( o2 is Item )
											{
												Item item2 = (Item)o2;

												if ( aleft > 60000 )
												{
													item2.Amount = 60000;
													aleft -= 60000;
												}
												else
												{
													item2.Amount = aleft;
													aleft = 0;
												}
												from.AddToBackpack( item2 );
												items.Add( item2 );
											}
										}
									}
									else
										item.Amount = vsi.Amount;
								}
								else
								{
									int total = 0;
									while( vsi.Amount-1 > total )
									{
										object o2 = Activator.CreateInstance( type );
										if ( o2 is Item )
										{
											Item item2 = (Item)o2;
											from.AddToBackpack( item2 );
											items.Add( item2 );
										}
										total++;
									}
								}

								if ( vsi.BlessBond )
									from.SendGump( new TokenVendorStoneBlessBondGump( from, vsi, m_Stone, items ) );
								from.AddToBackpack( item );
							}
						}
					}
					if ( m_Stone.Currency != null && m_Stone.Currency != "" )
						from.SendMessage( "You buy the items with the "+m_Stone.Currency+" in your backpack." );
					else
						from.SendMessage( "You buy the items with the currency in your backpack." );
				}
				else if ( m_Shopper.TotalPrice() == 0 || from.Backpack.ConsumeTotal( m_Stone.GetCurrency(), m_Shopper.TotalPrice(), true ) )
				{
					for( int i = 0; i < m_Shopper.ItemList.Count; ++i )
					{
						int n = (int)m_Shopper.ItemList[i];

						VSItem vsi = (VSItem)m_Stone.ItemList[n];

						Type type = vsi.GetItemType();

						if ( type != null )
						{
							object o = Activator.CreateInstance( type );

							if ( o is Mobile )
							{
								Mobile m = (Mobile)o;

								ArrayList items = new ArrayList();

								m.MoveToWorld( from.Location, from.Map );
								if ( m is BaseCreature )
								{
									BaseCreature c = (BaseCreature)m;
									c.ControlMaster = from;
									c.Controlled = true;
									c.ControlTarget = from;
									c.ControlOrder = OrderType.Follow;

									items.Add( c );
								}
								int total = 0;
								while( vsi.Amount-1 > total )
								{
									object o2 = Activator.CreateInstance( type );
									Mobile m2 = (Mobile)o2;

									m2.MoveToWorld( from.Location, from.Map );
									if ( m2 is BaseCreature )
									{
										BaseCreature c = (BaseCreature)m2;
										c.ControlMaster = from;
										c.Controlled = true;
										c.ControlTarget = from;
										c.ControlOrder = OrderType.Follow;

										items.Add( c );
									}
									total++;
								}

								if ( vsi.BlessBond )
									from.SendGump( new TokenVendorStoneBlessBondGump( from, vsi, m_Stone, items ) );
							}
							if ( o is Item )
							{
								Item item = (Item)o;

								ArrayList items = new ArrayList();

								items.Add( item );

								if ( item.Stackable )
								{
									if ( vsi.Amount > 60000 )
									{
										item.Amount = 60000;

										int aleft = vsi.Amount-60000;

										while( aleft > 0 )
										{
											object o2 = Activator.CreateInstance( type );
											if ( o2 is Item )
											{
												Item item2 = (Item)o2;

												if ( aleft > 60000 )
												{
													item2.Amount = 60000;
													aleft -= 60000;
												}
												else
												{
													item2.Amount = aleft;
													aleft = 0;
												}
												from.AddToBackpack( item2 );
												items.Add( item2 );
											}
										}
									}
									else
										item.Amount = vsi.Amount;
								}
								else
								{
									int total = 0;
									while( vsi.Amount-1 > total )
									{
										object o2 = Activator.CreateInstance( type );
										if ( o2 is Item )
										{
											Item item2 = (Item)o2;
											from.AddToBackpack( item2 );
											items.Add( item2 );
										}
										total++;
									}
								}

								if ( vsi.BlessBond )
									from.SendGump( new TokenVendorStoneBlessBondGump( from, vsi, m_Stone, items ) );
								from.AddToBackpack( item );
							}
						}
					}
					if ( m_Shopper.TotalPrice() == 0 )
						from.SendMessage( "You recieve the item for free." );
					else if ( m_Stone.Currency != null && m_Stone.Currency != "" )
						from.SendMessage( "You buy the items with the "+m_Stone.Currency+" in your backpack." );
					else
						from.SendMessage( "You buy the items with the currency in your backpack." );
				}
				else if ( from.BankBox.ConsumeTotal( m_Stone.GetCurrency(), m_Shopper.TotalPrice(), true ) )
				{
					for( int i = 0; i < m_Shopper.ItemList.Count; ++i )
					{
						int n = (int)m_Shopper.ItemList[i];

						VSItem vsi = (VSItem)m_Stone.ItemList[n];

						Type type = vsi.GetItemType();

						if ( type != null )
						{
							object o = Activator.CreateInstance( type );

							if ( o is Mobile )
							{
								Mobile m = (Mobile)o;

								ArrayList items = new ArrayList();

								m.MoveToWorld( from.Location, from.Map );
								if ( m is BaseCreature )
								{
									BaseCreature c = (BaseCreature)m;
									c.ControlMaster = from;
									c.Controlled = true;
									c.ControlTarget = from;
									c.ControlOrder = OrderType.Follow;

									items.Add( c );

									if ( vsi.BlessBond )
										from.SendGump( new TokenVendorStoneBlessBondGump( from, vsi, m_Stone, items ) );
								}
								int total = 0;
								while( vsi.Amount-1 > total )
								{
									object o2 = Activator.CreateInstance( type );
									Mobile m2 = (Mobile)o2;

									m2.MoveToWorld( from.Location, from.Map );
									if ( m2 is BaseCreature )
									{
										BaseCreature c = (BaseCreature)m2;
										c.ControlMaster = from;
										c.Controlled = true;
										c.ControlTarget = from;
										c.ControlOrder = OrderType.Follow;

										items.Add( c );
									}
									total++;
								}
							}
							if ( o is Item )
							{
								Item item = (Item)o;

								ArrayList items = new ArrayList();

								items.Add( item );

								if ( item.Stackable )
								{
									if ( vsi.Amount > 60000 )
									{
										item.Amount = 60000;

										int aleft = vsi.Amount-60000;

										while( aleft > 0 )
										{
											object o2 = Activator.CreateInstance( type );
											if ( o2 is Item )
											{
												Item item2 = (Item)o2;

												if ( aleft > 60000 )
												{
													item2.Amount = 60000;
													aleft -= 60000;
												}
												else
												{
													item2.Amount = aleft;
													aleft = 0;
												}
												from.AddToBackpack( item2 );
												items.Add( item2 );
											}
										}
									}
									else
										item.Amount = vsi.Amount;
								}
								else
								{
									int total = 0;
									while( vsi.Amount-1 > total )
									{
										object o2 = Activator.CreateInstance( type );
										if ( o2 is Item )
										{
											Item item2 = (Item)o2;
											from.AddToBackpack( item2 );
											items.Add( item2 );
										}
										total++;
									}
								}

								if ( vsi.BlessBond )
									from.SendGump( new TokenVendorStoneBlessBondGump( from, vsi, m_Stone, items ) );
								from.AddToBackpack( item );
							}
						}
					}
					if ( m_Stone.Currency != null && m_Stone.Currency != "" )
						from.SendMessage( "You buy the items with the "+m_Stone.Currency+" in your bank box." );
					else
						from.SendMessage( "You buy the items with the currency in your bank box." );
				}
				else
				{
					from.SendGump( new TokenVendorStoneBuyGump( m_Shopper, m_Stone ) );
					from.SendMessage( "You cannot not afford the items on your list." );
				}
			}
			if ( info.ButtonID >= 2 )
			{
				if ( info.ButtonID-2 > m_Shopper.ItemList.Count-1 || info.ButtonID-2 < 0 )
					return;
				m_Shopper.ItemList.Remove( m_Shopper.ItemList[info.ButtonID-2] );
				from.SendGump( new TokenVendorStoneBuyGump( m_Shopper, m_Stone ) );
			}
		}
	}
	public class StaffVendorGump : Gump
	{
                private TokenVendorStone m_Stone;

                public StaffVendorGump( Mobile from, TokenVendorStone stone ) : base( 125, 125 )
                {
			m_Stone = stone;

			m_Stone.CloseGumps( from );

			AddPage( 0 ); 

			AddBackground( 0, 0, 160, 140, 0x2436 );

			AddButton( 10, 10, 4005, 4007, 1, GumpButtonType.Reply, 0 ); 
			AddLabel( 40, 10, 1152, "Vendor Gump" );
			AddButton( 10, 30, 4005, 4007, 2, GumpButtonType.Reply, 0 ); 
			AddLabel( 40, 30, 1152, "Add Item" );

			if ( from.AccessLevel >= AccessLevel.Administrator )
				AddButton( 10, 50, 4005, 4007, 3, GumpButtonType.Reply, 0 ); 
			if ( m_Stone.AccessLevel >= AccessLevel.Administrator )
				AddLabel( 40, 50, 1302, "Administrator" );
			else if ( m_Stone.AccessLevel == AccessLevel.Seer )
				AddLabel( 40, 50, 324, "Seer" );
			else if ( m_Stone.AccessLevel == AccessLevel.GameMaster )
				AddLabel( 40, 50, 33, "Game Master" );
			else if ( m_Stone.AccessLevel == AccessLevel.Counselor )
				AddLabel( 40, 50, 2, "Counselor" );
			else if ( m_Stone.AccessLevel == AccessLevel.Player )
				AddLabel( 40, 50, 88, "Player" );

			AddButton( 10, 70, 4005, 4007, 4, GumpButtonType.Reply, 0 ); 
			if ( m_Stone.EditMode )
				AddLabel( 40, 70, 5, "Edit Mode" );
			else
				AddLabel( 40, 70, 33, "Edit Mode" );
			AddLabel( 10, 90, 1152, "Currency:" );
			AddTextEntry( 70, 90, 85, 15, 1152, 0, m_Stone.Currency );

			AddButton( 10, 110, 4005, 4007, 0, GumpButtonType.Reply, 0 ); 
			AddLabel( 40, 110, 33, "Close" );
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{ 
			Mobile from = state.Mobile;

			if ( m_Stone.Deleted )
				return;

			string currency = "";
			string[] tr = new string[ 1 ];
			foreach( TextRelay t in info.TextEntries )
				tr[ t.EntryID ] = t.Text;
			if ( tr[ 0 ] != null )
				currency = tr[ 0 ];
			m_Stone.Currency = currency;

			if ( info.ButtonID == 0 )
			{
				from.SendMessage( "Closed." );
			}
			if ( info.ButtonID == 1 )
			{
				from.SendGump( new VendorGump( new VSShopper( from, m_Stone ), m_Stone ) );
			}
			if ( info.ButtonID == 2 )
			{
				from.SendGump( new TokenVendorStoneAddItemGump( from, m_Stone ) );
			}
			if ( info.ButtonID == 3 )
			{
				if ( m_Stone.AccessLevel >= AccessLevel.Administrator )
					m_Stone.AccessLevel = AccessLevel.Player;
				else if ( m_Stone.AccessLevel == AccessLevel.Seer )
					m_Stone.AccessLevel = AccessLevel.Administrator;
				else if ( m_Stone.AccessLevel == AccessLevel.GameMaster )
					m_Stone.AccessLevel = AccessLevel.Seer;
				else if ( m_Stone.AccessLevel == AccessLevel.Counselor )
					m_Stone.AccessLevel = AccessLevel.GameMaster;
				else if ( m_Stone.AccessLevel == AccessLevel.Player )
					m_Stone.AccessLevel = AccessLevel.Counselor;

				from.SendGump( new StaffVendorGump( from, m_Stone ) );
			}
			if ( info.ButtonID == 4 )
			{
				if ( m_Stone.EditMode )
					m_Stone.EditMode = false;
				else
					m_Stone.EditMode = true;

				from.SendGump( new StaffVendorGump( from, m_Stone ) );
			}
		}
	}
	public class TokenVendorStoneAddItemGump : Gump
	{
		private Mobile m_Mobile;
		private TokenVendorStone m_Stone;

		public TokenVendorStoneAddItemGump( Mobile mobile, TokenVendorStone stone ) : base( 25, 50 )
		{
			m_Mobile = mobile;
			m_Stone = stone;

			m_Stone.CloseGumps( m_Mobile );

			AddPage( 0 );

			AddBackground( 25, 10, 420, 430, 0x2436 );

			AddImageTiled( 33, 20, 401, 411, 2624 );
			AddAlphaRegion( 33, 20, 401, 411 );

			AddLabel( 125, 40, 1152, "Vendor Stone" );

			AddLabel( 40, 60, 1152, "Add a Mobile or Item:" );
			AddTextEntry( 40, 80, 225, 15, 5, 0, "Item Here" );
			AddLabel( 40, 100, 1152, "Gump Name:" );
			AddTextEntry( 40, 120, 225, 15, 5, 1, "Name Here" );
			AddLabel( 40, 140, 1152, "Price:" );
			AddTextEntry( 40, 160, 225, 15, 5, 2, "0" );
			AddLabel( 40, 180, 1152, "Item Amount:" );
			AddTextEntry( 40, 200, 225, 15, 5, 3, "1" );

			AddCheck( 40, 220, 0x2342, 0x2343, false, 1 );
			AddLabel( 70, 220, 1152, "Bless/Bond:" );

			AddLabel( 40, 240, 1152, "Bless/Bond Price:" );
			AddTextEntry( 40, 260, 225, 15, 5, 4, "0" );

			AddLabel( 40, 280, 1152, "Description:" );
			AddTextEntry( 40, 300, 360, 75, 5, 5, "Des Here" );

			AddButton( 40, 390, 4005, 4007, 0, GumpButtonType.Reply, 0 );
			AddLabel( 70, 393, 1152, "Back" );
			AddButton( 120, 390, 4005, 4007, 1, GumpButtonType.Reply, 0 );
			AddLabel( 150, 393, 1152, "Apply" );
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			Mobile from = state.Mobile; 

			if ( from == null )
				return;

			if ( info.ButtonID == 0 )
			{
				from.SendGump( new StaffVendorGump( from, m_Stone ) );
			}
			if ( info.ButtonID == 1 )
			{
				string item = "";
				string gumpname = "";
				int price = 0;
				int amount = 0;
				int bbprice = 0;
				bool blessbond = info.IsSwitched( 1 );
				string description = "";

				string[] tr = new string[ 6 ];
				foreach( TextRelay t in info.TextEntries )
				{
					tr[ t.EntryID ] = t.Text;
				}

				try { price = (int) uint.Parse( tr[ 2 ] ); } 
				catch {}
				try { amount = (int) uint.Parse( tr[ 3 ] ); } 
				catch {}
				try { bbprice = (int) uint.Parse( tr[ 4 ] ); } 
				catch {}
				if ( tr[ 0 ] != null )
					item = tr[ 0 ];
				if ( tr[ 1 ] != null )
					gumpname = tr[ 1 ];
				if ( tr[ 5 ] != null )
					description = tr[ 5 ];

				if ( amount <= 0 )
					amount = 1;

				if ( item != "" && gumpname != "" )
				{
					VSItem vsi = new VSItem( item, gumpname, price, amount, blessbond, bbprice, description );
					m_Stone.ItemList.Add( vsi );

					from.SendMessage( "Item Added." );
				}
				else
				{
					from.SendMessage( "You must set a property for each one." );
				}

				from.SendGump( new TokenVendorStoneAddItemGump( from, m_Stone ) );
			}
		}
	}
	public class TokenVendorStoneEditGump : Gump
	{
                private TokenVendorStone m_Stone;
                private VSItem m_VSI;

                public TokenVendorStoneEditGump( Mobile from, VSItem vsi, TokenVendorStone stone ) : base( 125, 125 )
                {
			m_Stone = stone;
			m_VSI = vsi;

			m_Stone.CloseGumps( from );

			AddPage( 0 ); 

			AddBackground( 0, 0, 420, 300, 0x2436 );

			AddLabel( 13, 10, 1152, "Item Type:" );
			AddTextEntry( 83, 10, 100, 15, 1152, 0, m_VSI.Item );

			AddLabel( 13, 30, 1152, "Gump Name:" );
			AddTextEntry( 90, 30, 90, 15, 1152, 1, m_VSI.Name );

			AddLabel( 13, 50, 1152, "Price:" );
			AddTextEntry( 53, 50, 85, 15, 1152, 2, ""+m_VSI.Price );

			AddLabel( 13, 70, 1152, "Amount:" );
			AddTextEntry( 63, 70, 85, 15, 1152, 3, ""+m_VSI.Amount );

			AddCheck( 15, 90, 0x2342, 0x2343, m_VSI.BlessBond, 1 );
			AddLabel( 45, 90, 1152, "Bless/Bond:" );

			AddLabel( 13, 110, 1152, "Bless/Bond Price:" );
			AddTextEntry( 123, 110, 225, 15, 1152, 4, ""+m_VSI.BBPrice );

			AddLabel( 13, 130, 1152, "Description:" );
			AddTextEntry( 13, 150, 387, 75, 1152, 5, m_VSI.Description );

			AddButton( 15, 245, 4005, 4007, 2, GumpButtonType.Reply, 0 ); 
			AddLabel( 45, 245, 33, "Remove" );

			AddButton( 15, 265, 4005, 4007, 0, GumpButtonType.Reply, 0 ); 
			AddLabel( 45, 265, 33, "Back" );
			AddButton( 85, 265, 4005, 4007, 1, GumpButtonType.Reply, 0 ); 
			AddLabel( 115, 265, 33, "Apply" );
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{ 
			Mobile from = state.Mobile;

			if ( m_Stone.Deleted )
				return;

			if ( info.ButtonID == 0 )
			{
				from.SendMessage( "Back." );
				from.SendGump( new VendorGump( new VSShopper( from, m_Stone ), m_Stone ) );
			}
			if ( info.ButtonID == 1 )
			{
				string item = "";
				string gumpname = "";
				int price = 0;
				int amount = 0;
				int bbprice = 0;
				bool blessbond = info.IsSwitched( 1 );
				string description = "";

				string[] tr = new string[ 6 ];
				foreach( TextRelay t in info.TextEntries )
				{
					tr[ t.EntryID ] = t.Text;
				}

				try { price = (int) uint.Parse( tr[ 2 ] ); } 
				catch {}
				try { amount = (int) uint.Parse( tr[ 3 ] ); } 
				catch {}
				try { bbprice = (int) uint.Parse( tr[ 4 ] ); } 
				catch {}
				if ( tr[ 0 ] != null )
					item = tr[ 0 ];
				if ( tr[ 0 ] != null )
					gumpname = tr[ 1 ];
				if ( tr[ 5 ] != null )
					description = tr[ 5 ];

				m_VSI.Item = item;
				m_VSI.Name = gumpname;
				m_VSI.Price = price;
				m_VSI.Amount = amount;
				m_VSI.BBPrice = bbprice;
				m_VSI.BlessBond = blessbond;
				m_VSI.Description = description;

				from.SendGump( new VendorGump( new VSShopper( from, m_Stone ), m_Stone ) );
			}
			if ( info.ButtonID == 2 )
			{
				if ( m_Stone.ItemList.Contains( m_VSI ) )
					m_Stone.ItemList.Remove( m_VSI );

				from.SendGump( new VendorGump( new VSShopper( from, m_Stone ), m_Stone ) );
			}
		}
	}
	public class TokenVendorStoneBlessBondGump : Gump
	{
                private TokenVendorStone m_Stone;
                private VSItem m_VSI;
		private ArrayList m_Objects;

                public TokenVendorStoneBlessBondGump( Mobile from, VSItem vsi, TokenVendorStone stone, ArrayList objects ) : base( 125, 125 )
                {
			m_Stone = stone;
			m_VSI = vsi;
			m_Objects = objects;

			m_Stone.CloseGumps( from );

			AddPage( 0 ); 

			AddBackground( 0, 0, 375, 85, 0x2436 );

			if ( objects.Count > 0 && objects[0] is Item )
				AddLabel( 13, 10, 1152, "Would you like to bless this item: '"+ vsi.Name +"'?" );
			else if ( objects.Count > 0 && objects[0] is Mobile )
				AddLabel( 13, 10, 1152, "Would you like to bond this pet: '"+ vsi.Name +"'?" );
			AddLabel( 13, 25, 1152, "Price: "+ vsi.BBPrice );

			AddButton( 15, 50, 4005, 4007, 0, GumpButtonType.Reply, 0 ); 
			AddLabel( 45, 50, 33, "No" );
			AddButton( 85, 50, 4005, 4007, 1, GumpButtonType.Reply, 0 ); 
			AddLabel( 115, 50, 33, "Yes" );
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{ 
			Mobile from = state.Mobile;

			if ( m_Stone.Deleted )
				return;

			if ( info.ButtonID == 0 )
			{
				if ( m_Objects.Count > 0 && m_Objects[0] is Item )
					from.SendMessage( "You decide not to bless this item." );
				else if ( m_Objects.Count > 0 && m_Objects[0] is Mobile )
					from.SendMessage( "You decide not to bond this pet." );
			}
			if ( info.ButtonID == 1 )
			{
				if ( m_Stone.Currency.ToLower() == "daat99tokens" )
				{
					if (!TokenSystem.TakePlayerTokens(from as PlayerMobile, m_VSI.BBPrice, true))
						return;

					for( int i = 0; i < m_Objects.Count; i++ )
					{
						object o = m_Objects[i];

						if ( o is Item )
							((Item)o).LootType = LootType.Blessed;
						if ( o is Mobile && (Mobile)o is BaseCreature )
							((BaseCreature)o).IsBonded = true;
					}
				}
				else if ( from.Backpack.ConsumeTotal( m_Stone.GetCurrency(), m_VSI.BBPrice, true ) || m_VSI.BBPrice == 0 )
				{
					for( int i = 0; i < m_Objects.Count; i++ )
					{
						object o = m_Objects[i];

						if ( o is Item )
							((Item)o).LootType = LootType.Blessed;
						if ( o is Mobile && (Mobile)o is BaseCreature )
							((BaseCreature)o).IsBonded = true;
					}
				}
				else if ( from.BankBox.ConsumeTotal( m_Stone.GetCurrency(), m_VSI.BBPrice, true ) )
				{
					for( int i = 0; i < m_Objects.Count; i++ )
					{
						object o = m_Objects[i];

						if ( o is Item )
							((Item)o).LootType = LootType.Blessed;
						if ( o is Mobile && (Mobile)o is BaseCreature )
							((BaseCreature)o).IsBonded = true;
					}
				}
				else
					from.SendMessage( "You cannot not afford to do this." );
			}
		}
	}
	public class TokenVendorStoneDescriptionGump : Gump
	{
		public string Center( string text )
		{
			return String.Format( "<CENTER>{0}</CENTER>", text );
		}

		public string Color( string text, int color )
		{
			return String.Format( "<BASEFONT COLOR=#{0:X6}>{1}</BASEFONT>", color, text );
		}

                public TokenVendorStoneDescriptionGump( Mobile from, string description, string name ) : base( 125, 125 )
                {
			from.CloseGump( typeof( TokenVendorStoneDescriptionGump ) );

			AddPage( 0 ); 

			AddBackground( 0, 0, 200, 200, 0x2436 );

			AddHtml( 10, 10, 190, 20, Color( Center( "Description For: '"+name+"'" ), 0xFFFFFF ), false, false );

			AddHtml( 10, 30, 190, 145, Color( Center( description ), 0xFFFFFF ), false, true );
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{ 
			Mobile from = state.Mobile;

			if ( info.ButtonID == 0 )
				from.SendMessage( "Closed." );
		}
	}
}