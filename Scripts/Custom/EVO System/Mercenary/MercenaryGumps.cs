#region AuthorHeader
//
//	EvoSystem version 2.1, by Xanthos
//
//
#endregion AuthorHeader
using System;
using Server;
using Server.Items;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Xanthos.Evo
{
	// This is a paired down dupe of the HairstylistBuyGump. For all of
	// the hair changes the distro gumps are used.

	public class HairstylistBuyGump : Gump
	{
		private static readonly object From = new object();
		private static readonly object Merc = new object();
		private static readonly object Price = new object();
		private static HairstylistBuyInfo[] m_SellList = new HairstylistBuyInfo[]
		{
			new HairstylistBuyInfo( "New Hair", 0, false, typeof( ChangeHairstyleGump ), new object[]
				{ From, Merc, Price, false, ChangeHairstyleEntry.HairEntries } ),
			new HairstylistBuyInfo( "New Beard", 0, true, typeof( ChangeHairstyleGump ), new object[]
				{ From, Merc, Price, true, ChangeHairstyleEntry.BeardEntries } ),
			new HairstylistBuyInfo( "Normal Hair Dye", 0, false, typeof( ChangeHairHueGump ), new object[]
				{ From, Merc, Price, true, true, ChangeHairHueEntry.RegularEntries } ),
			new HairstylistBuyInfo( "Bright Hair Dye", 0, false, typeof( ChangeHairHueGump ), new object[]
				{ From, Merc, Price, true, true, ChangeHairHueEntry.BrightEntries } ),
			new HairstylistBuyInfo( "Hair Only Dye", 0, false, typeof(ChangeHairHueGump ), new object[]
				{ From, Merc, Price, true, false, ChangeHairHueEntry.RegularEntries } ),
			new HairstylistBuyInfo( "Beard Only Dye", 0, true, typeof( ChangeHairHueGump ), new object[]
				{ From, Merc, Price, false, true, ChangeHairHueEntry.RegularEntries } ),
			new HairstylistBuyInfo( "Bright Hair Only Dye", 0, false, typeof( ChangeHairHueGump ), new object[]
				{ From, Merc, Price, true, false, ChangeHairHueEntry.BrightEntries } ),
			new HairstylistBuyInfo( "Bright Beard Only Dye", 0, true, typeof( ChangeHairHueGump ), new object[]
				{ From, Merc, Price, false, true, ChangeHairHueEntry.BrightEntries } )
		};

		private Mobile m_From, m_Merc;

		public HairstylistBuyGump( Mobile from, Mobile mercenary ) : base( 50, 50 )
		{
			m_From = from;
			m_Merc = mercenary;

			from.CloseGump( typeof( Xanthos.Evo.HairstylistBuyGump ) );
			from.CloseGump( typeof( ChangeHairHueGump ) );
			from.CloseGump( typeof( ChangeHairstyleGump ) );

			bool isFemale = ( m_Merc.Female || m_Merc.Body.IsFemale );

			int rows = 0;
			for ( int i = 0; i < m_SellList.Length; ++i )
			{
				if ( m_SellList[ i ].FacialHair != true || !isFemale )
					++rows;
			}

			AddPage( 0 );
			AddBackground( 50, 10, 450, 100 + (rows * 25), 2600 );
			AddHtmlLocalized( 100, 40, 350, 20, 1018356, false, false ); // Choose your hairstyle change:

			for ( int i = 0, index = 0; i < m_SellList.Length; ++i )
			{
				if ( m_SellList[ i ].FacialHair != true || !isFemale )
				{
					AddHtml( 140, 75 + (index * 25), 300, 20, m_SellList[i].TitleString, false, false );
					AddButton( 100, 75 + (index++ * 25), 4005, 4007, 1 + i, GumpButtonType.Reply, 0 );
				}
			}
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			int index = info.ButtonID - 1;

			if ( index >= 0 && index < m_SellList.Length )
			{
				HairstylistBuyInfo buyInfo = m_SellList[index];

				try
				{
					object[] origArgs = buyInfo.GumpArgs;
					object[] args = new object[origArgs.Length];

					for ( int i = 0; i < args.Length; ++i )
					{
						if ( origArgs[i] == Price )
							args[i] = 0;
						else if ( origArgs[i] == From )
							args[i] = m_Merc;
						else if ( origArgs[i] == Merc )
							args[i] = m_From;
						else
							args[i] = origArgs[i];
					}

					m_From.SendGump( Activator.CreateInstance( buyInfo.GumpType, args ) as Gump );
				}
				catch {}
			}
		}
	}
}