using System;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Network;
using Server.Spells;
using Server.ACC.CM;
using Server.ACC.CSS.Modules;

namespace Server.ACC.CSS
{
	public class IconPlacementGump : Gump
	{
		public CSpellbook m_Book;
		public Mobile     m_From;

		public int        m_X;
		public int        m_Y;
		public int        m_I;
		public int        m_Icon;
		public Type       m_Spell;
		public int        m_Background;
		public School     m_School;

		public IconPlacementGump( CSpellbook book, Mobile from, int x, int y, int i, int icon, Type spellType, int background, School school ) : base( 0, 0 )
		{
			m_Book = book;
			m_From = from;

			m_X = x;
			m_Y = y;
			m_I = i;
			m_Icon = icon;
			m_Spell = spellType;
			m_Background = background;
			m_School = school;

			string xtext = m_X.ToString();
			string ytext = m_Y.ToString();
			string itext = m_I.ToString();

			Closable=true;
			Disposable=true;
			Dragable=false;
			Resizable=false;

			AddPage(0);

			AddImage( 260, 160, 5011 );
			AddLabel( 353, 230, 1153, "Icon Placement" );

			CSpellInfo si = SpellInfoRegistry.GetInfo( m_School, m_Spell );
			if( si == null )
				return;

			AddLabel( 338, 350, 1153, si.Name );

			AddButton( 412, 288, 2444, 2443, 1, GumpButtonType.Reply, 0 );
			AddButton( 325, 288, 2444, 2443, 2, GumpButtonType.Reply, 0 );
			AddLabel( 425, 290, 1153, "Apply" );
			AddLabel( 339, 290, 1153, "Move" );

			AddButton( 377, 178, 4500, 4500, 3, GumpButtonType.Reply, 0 ); //Up
			AddButton( 474, 276, 4502, 4502, 4, GumpButtonType.Reply, 0 ); //Right
			AddButton( 377, 375, 4504, 4504, 5, GumpButtonType.Reply, 0 ); //Down
			AddButton( 278, 276, 4506, 4506, 6, GumpButtonType.Reply, 0 ); //Left

			AddBackground( 348, 260, 105, 20, 9300 );
			AddBackground( 348, 318, 105, 20, 9300 );
			AddBackground( 388, 290, 25, 20, 9300 );

			AddTextEntry( 348, 260, 105, 20, 1153, 7, "" + xtext );
			AddTextEntry( 348, 318, 105, 20, 1153, 8, "" + ytext );
			AddTextEntry( 388, 290, 25, 20, 1153, 9, "" + itext );

			AddBackground( x, y, 54, 56, background );
			AddImage( x+45, y, 9008 );
			AddImage( x+5, y+5, icon );
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			Mobile from = state.Mobile;
			switch( info.ButtonID )
			{
				//Apply
				case 1:
				{
					if( !CentralMemory.Running )
					{
						from.SendMessage( "There is no Central Memory!  Please page an Admin to assist." );
						return;
					}

					IconInfo ii = new IconInfo( m_Spell, m_Icon, m_X, m_Y, m_Background, m_School );

					if( !CentralMemory.ContainsModule( from.Serial, typeof( IconsModule ) ) )
						CentralMemory.AddModule( from.Serial, typeof( IconsModule ) );

					IconsModule im = new IconsModule( from.Serial );
					im.Add( ii );

					CentralMemory.AppendModule( from.Serial, (Module)im, false );

					from.SendGump( new SpellIconGump( ii ) );
					break;
				}

				//Move
				case 2:
				{
					TextRelay xrelay = info.GetTextEntry( 7 );
					TextRelay yrelay = info.GetTextEntry( 8 );
					string xstring = ( xrelay == null ? null : xrelay.Text.Trim() );
					string ystring = ( yrelay == null ? null : yrelay.Text.Trim() );
					if( xstring == null || xstring.Length == 0 || ystring == null || ystring.Length == 0 )
					{
						from.SendMessage( "Please enter a X coordinate in the top box and a Y coordinate in the bottom box" );
					}
					else
					{
						int x = m_X;
						int y = m_Y;
						try
						{
							x = Int32.Parse(xstring);
							y = Int32.Parse(ystring);
							m_X = x;
							m_Y = y;
						}
						catch
						{
							from.SendMessage( "Please enter a X coordinate in the top box and a Y coordinate in the bottom box" );
						}
					}
					if( m_X < 0 )
					{
						m_X = 0;
						from.SendMessage( "You cannot go any farther left" );
					}
					if( m_Y < 0 )
					{
						m_Y = 0;
						from.SendMessage( "You cannot go any farther up" );
					}

					from.CloseGump( typeof( IconPlacementGump ) );
					from.SendGump(new IconPlacementGump( m_Book, from, m_X, m_Y, m_I, m_Icon, m_Spell, m_Background, m_School ) );
					break;
				}

				//Up
				case 3:
				{
					MakeI(info);
					from.CloseGump( typeof( IconPlacementGump ) );
					if( (m_Y-m_I) < 0 )
					{
						m_Y = 0;
						from.SendMessage( "You cannot go any farther up" );
						from.SendGump( new IconPlacementGump( m_Book, from, m_X, m_Y, m_I, m_Icon, m_Spell, m_Background, m_School ) );
					}
					else
					{
						from.SendGump( new IconPlacementGump( m_Book, from, m_X, (m_Y-m_I), m_I, m_Icon, m_Spell, m_Background, m_School ) );
					}
					break;
				}

				//Right
				case 4:
				{
					MakeI(info);
					from.CloseGump( typeof( IconPlacementGump ) );
					from.SendGump( new IconPlacementGump( m_Book, from, (m_X+m_I), m_Y, m_I, m_Icon, m_Spell, m_Background, m_School ) );
					break;
				}

				//Down
				case 5:
				{
					MakeI(info);
					from.CloseGump( typeof( IconPlacementGump ) );
					from.SendGump( new IconPlacementGump( m_Book, from, m_X, (m_Y+m_I), m_I, m_Icon, m_Spell, m_Background, m_School ) );
					break;
				}

				//Left
				case 6:
				{
					MakeI(info);
					from.CloseGump( typeof( IconPlacementGump ) );
					if( (m_X-m_I) < 0 )
					{
						m_X = 0;
						from.SendMessage( "You cannot go any farther left" );
						from.SendGump( new IconPlacementGump( m_Book, from, m_X, m_Y, m_I, m_Icon, m_Spell, m_Background, m_School ) );
					}
					else
					{
						from.SendGump( new IconPlacementGump( m_Book, from, (m_X-m_I), m_Y, m_I, m_Icon, m_Spell, m_Background, m_School ) );
					}
					break;
				}
			}
		}
		private void MakeI(RelayInfo info)
		{
			TextRelay irelay = info.GetTextEntry( 9 );
			string istring = ( irelay == null ? null : irelay.Text.Trim() );
			if( istring == null || istring.Length == 0 )
			{
				m_From.SendMessage( "Please enter an integer in the middle text field." );
			}
			else
			{
				int i = m_I;
				try
				{
					i = Int32.Parse(istring);
					m_I = i;
				}
				catch
				{
					m_From.SendMessage( "Please enter an integer in the middle text field." );
				}
			}
		}
	}
}