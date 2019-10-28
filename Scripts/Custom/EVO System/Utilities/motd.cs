#region AuthorHeader
//
//	Motd.cs version 2.0, by Xanthos
//
//	This is a RunUO assembly to display a message of the day (motd) gump
//	at login.  It supports archiving and display of previous motd content.
//	You may use this code as you please as long as you leave this AuthorHeader
//	in place.
//
#endregion AuthorHeader
#region Header
//
//	Using this is pretty simple.  Place this file in your custom directory.
//	It will create a Motd subdirectory in your data directory and an Archive
//	directory beneath that.  Place a file named motd.txt in the Motd sub-
//	directory, enter the game as an admin and type [motd.  Select the reload
//	button and all players will see the new motd on login. Once the motd is
//	presented to a player, their account is tagged so they will not see it
//	again on login unless the Reload button is used as described above.
//	Players may, at any time, see the motd by typing motd. The Reload
//	option allows an admin to make small changes to the motd and then get
//	them brought into the motd cache without a restart of the server.
//
//	The [motd admin command also allows archiving of the current motd.txt
//	into the Archive directory and replacing the existing motd file with one 
//	named new.txt.  This action will also tag all accounts so that the motd
//	will be displayed at login.  The archives are given names in sequence
//	starting at 1.txt.  The current motd, when archived is always given the
//	name 1.txt after any older files are propagated to higher numbered names.
//	By default, a maximum of nine archives will be shown in the gump.  The
//	number of archives maintained in the Archive directory is limited only
//	by the file system.  Archiving may take longer as the number of files
//	increases however.  If an motd.txt file is not present when the Reload
//	or Archive functions are used, all accounts are tagged so that the motd
//	will not be displayed.
//
//	There are a number of static variables that can be used to customize
//	aspects of this program.  In most cases no modifications are needed.
//
//	This work was done by Xanthos, based on code written by Viago and others, 
//	see the credits page for more details.
//	- Xanthos.
//
//	editor tabs = 4
//
#endregion Header
using System;
using System.IO;
using System.Text;
using System.Threading;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Mobiles;
using Server.Accounting;
using Server.Commands;

namespace Xanthos.Utilities
{
	public class NewsGump : Gump
	{
		// Modify these constants to suit your needs
		private const string kNewFile		= "new";
		private const string kMotdFile		= "motd";
		private const string kFileSuffix	= ".txt";
		private const string kPathMotd		= "Data/Motd/";
		private const string kPathArchive	= kPathMotd + "Archive/";
		private const string kGreeting		= "Greetings And Welcome To ShardName";
		private const string kDefaultBody	= "There is no current news.";
		private const string kDefaultTitle	= "Current News";
		private const string kNewMotdMessage= "The message of the day has been updated. To see it, type motd.";
		private static int   kMaximumArchives			= 9;	// should be const, used static to get avoid compiler warning
		private	static bool	 kAlwaysShowMotdOnLogin		= false;// should be const, used static to get avoid compiler warning
		private	static bool  kTextInsteadOfGumpOnLogin	= true;	// should be const, used static to get avoid compiler warning
		
		internal const int    kDataColor = 50;
		internal const int    kLabelColor = 88;
		internal const string kLabelHtmlColor = "66CCFF";

		//----- Edting below this line is not recommended -----

		#region Static Methods

		private static volatile string [] s_ArchiveFilenameCache;
		private static volatile string s_MotdCache;
		private static volatile Mutex s_Mutex = new Mutex(); 

		public static void Initialize()
		{
			try
			{
				// Make sure we have a message store
				if ( !Directory.Exists( kPathArchive ) )
					Directory.CreateDirectory( kPathArchive );

				LoadMotd();
			}
			catch
			{
				World.Broadcast( kLabelColor, true, "Motd: no motd file found");
			}

			try { GetArchiveList(); }
			catch { World.Broadcast( kLabelColor, true, "Motd: no archive files found"); }

			EventSink.Login += new LoginEventHandler( MOTD_Login );
			EventSink.Speech += new SpeechEventHandler( EventSink_Speech );
			CommandHandlers.Register( "MOTD", AccessLevel.Administrator, new CommandEventHandler( MOTD_OnCommand ) );
		}

		private static bool LoadMotd()	// Not thread safe, call only from a locked block
		{
			string Filename = kPathMotd + kMotdFile + kFileSuffix;
			string str = "";

			try
			{
				StreamReader reader = new StreamReader( Filename );
				str = reader.ReadToEnd();
				reader.Close();
			}
			finally
			{
				s_MotdCache = ( str == "" ? kDefaultBody : str );
			}

			return !( s_MotdCache == kDefaultBody );
		}

		private static void ArchiveMotd( PlayerMobile mobile ) // Not thread safe, call only from a locked block
		{
			string [] list = Directory.GetFiles( kPathArchive, "*" + kFileSuffix );
			int totalFiles = list.Length;
			string source;

			// Rename all files to make room for the new #1
			for ( int i = totalFiles, j = i + 1; i > 0; i--, j-- )
			{
				if ( File.Exists( source = kPathArchive + i + kFileSuffix ) )
					File.Move( source, kPathArchive + j + kFileSuffix );
			}

			// Copy the motd file to the #1 slot in the archive, and replace old motd with the new motd file
			if ( File.Exists( source = kPathMotd + kMotdFile + kFileSuffix ) )
				File.Move( source, kPathArchive + "1" + kFileSuffix );

			if ( File.Exists( source = kPathMotd + kNewFile + kFileSuffix ) )
				File.Move( kPathMotd + kNewFile + kFileSuffix, kPathMotd + kMotdFile + kFileSuffix );
		}

		private static void GetArchiveList()	// Not thread safe, call only from a locked block
		{
			s_ArchiveFilenameCache = null;

			// This method may be set to not get all of the files so don't use when archiving.
			// One of these will be unreachable - I chose to live with it rather than use defines.
			// Also Directory.GetFiles has really poor regular expression support
			// so this is about the best you can do without more work.
			
			if ( kMaximumArchives < 10 )
				s_ArchiveFilenameCache = Directory.GetFiles( kPathArchive, "?" + kFileSuffix );
			else
				s_ArchiveFilenameCache = Directory.GetFiles( kPathArchive, "*" + kFileSuffix );
		}
		
		//
		// All thread unsafe methods are bottlenecked in the next three methods.
		//

		private static void PreformArchive( PlayerMobile mobile )
		{
			bool showMotd;

			s_Mutex.WaitOne();
			try
			{
				ArchiveMotd( mobile );
				showMotd = LoadMotd();
				GetArchiveList( );
			}
			catch (Exception exc)
			{
				s_Mutex.ReleaseMutex();
				throw exc;
			}
			s_Mutex.ReleaseMutex();
			SetAllTags( mobile, showMotd );	// no need to block for this
		}
	
		private static void PerformReload( PlayerMobile mobile )
		{
			bool showMotd;

			s_Mutex.WaitOne();
			try
			{
				showMotd = LoadMotd();
				GetArchiveList( );
			}
			catch (Exception exc)
			{
				s_Mutex.ReleaseMutex();
				throw exc;
			}
			s_Mutex.ReleaseMutex();
			SetAllTags( mobile, showMotd );	// no need to block for this
		}

		internal static void SafelySendGumpTo( Mobile mobile, bool admin )
		{
			NewsGump gump;

			s_Mutex.WaitOne();
			try
			{
				int length = s_ArchiveFilenameCache.Length >= kMaximumArchives
					? kMaximumArchives : s_ArchiveFilenameCache.Length;

				// Get the caches copied onto the stack of the gump thread.
				gump = new NewsGump( mobile, s_MotdCache, s_ArchiveFilenameCache, length, admin );
			}
			finally
			{
				s_Mutex.ReleaseMutex();
			}
			
			gump.DrawGump( mobile, admin );
			mobile.SendGump( gump );
			((Account)(((PlayerMobile)mobile).Account)).SetTag( "motd", "false" );
		}

		private static void SetAllTags( Mobile from, bool toTrue )
		{
			if ( kAlwaysShowMotdOnLogin )	// No need to tag if we are always showing
				return;

			string boolValue = toTrue.ToString();
			Account account;

			foreach ( Mobile mobile in World.Mobiles.Values )
			{
				if ( null == mobile || !( mobile is PlayerMobile ) )
					continue;

				if( null == ( account = (Account)from.Account ) )
					continue;

				account.SetTag( "motd", boolValue );
			}
			from.SendMessage( kLabelColor, "All players" + ( true == toTrue ? " will " : " will not ") + "see the MOTD Message at next login." );
		}

		#region In-game commands and events

		[Usage( "MOTD" )]
		[Description( "Show MOTD admin menu." )]
		private static void MOTD_OnCommand( CommandEventArgs args )
		{
			Mobile mobile = args.Mobile;
			mobile.CloseGump( typeof(NewsGump) );
			SafelySendGumpTo( mobile, true );
		}

		private static void EventSink_Speech( SpeechEventArgs args )
		{
			Mobile mobile = args.Mobile;
			if ( args.Speech.ToLower().IndexOf( "motd" ) >= 0 )
			{
				SafelySendGumpTo( mobile, false );
			}
		}

		private static void MOTD_Login( LoginEventArgs args )
		{
			Mobile mobile = args.Mobile;
			Account account = (Account)mobile.Account;

			if ( kAlwaysShowMotdOnLogin || Convert.ToBoolean( account.GetTag( "motd" ) ) )
			{
				// One of these will be unreachable - I chose to live with it rather than use defines.
				if ( kTextInsteadOfGumpOnLogin )
					mobile.SendMessage( kLabelColor, kNewMotdMessage );
				else
					SafelySendGumpTo( mobile, false );
			}
		}

		#endregion In-game commands and events
		#endregion Static Methods
		#region News Gump Methods

		private PlayerMobile m_Player;
		private string m_UserCount;
		private string m_ItemCount;
		private string m_MobileCount;
		private string m_Body;
		private string [] m_ArchiveList;
		private int    m_TotalPages;
		private int    m_ArchiveCount;
		private int    m_ArchivesLoaded;

		private string LoadNextArchive()
		{
			string Filename;
			string text = "";

			if ( m_ArchiveList != null && m_ArchivesLoaded < m_ArchiveCount
				&& (( Filename = m_ArchiveList[ m_ArchivesLoaded ].ToString() ) != null )
				&& File.Exists( Filename ) )
			{
				StreamReader reader = new StreamReader( Filename );

				text = reader.ReadToEnd();
				reader.Close();

				m_ArchivesLoaded++;
			}
			return text == "" ? kDefaultBody : text;
		}

		private NewsGump( Mobile mobile, string body, string [] archiveList, int archiveCount, bool admin ) : base( 100, 100 )
		{
			mobile.CloseGump( typeof(NewsGump) );
			m_Body			= body;
			m_Player		= (PlayerMobile)mobile;
			m_UserCount		= NetState.Instances.Count.ToString();
			m_ItemCount		= World.Items.Count.ToString();
			m_MobileCount	= World.Mobiles.Count.ToString();
			m_ArchiveList	= archiveList;
			m_ArchiveCount	= ( null == archiveList ? 0 : archiveCount );
			m_ArchivesLoaded = 0;
			m_TotalPages	= 0;
		}

		private void DrawGump( Mobile mobile, bool admin )
		{
			if ( admin )
			{
				int colOne = 10;
				int colTwo = colOne + 35;
				int rowOne = 10;
				int rowHeight = 25;
				int row = rowOne;

				AddPage( 1 );
				AddBackground( 0, 0, 205, 180, 5054 );
				AddButton( colOne, rowOne, 0xFB7, 0xFB9, 100, GumpButtonType.Reply, 0 );				// Archive
				AddLabel( colTwo, rowOne, kLabelColor, "Archive motd File" );
				AddButton( colOne, (row += rowHeight), 0xFB7, 0xFB9, 101, GumpButtonType.Reply, 0 );	// Reload
				AddLabel( colTwo, row, kLabelColor, "Reload motd File" );
				AddButton( colOne, (row += rowHeight + 10), 0xFB7, 0xFB9, 0, GumpButtonType.Reply, 0 ); // Cancel
				AddLabel( colTwo, row, kLabelColor, "Cancel" );
				AddLabel( colOne, (row += rowHeight + 10), kDataColor, "Warning, admins have both" );
				AddLabel( colOne, (row += rowHeight - 10), kDataColor, "the [motd and motd commands." );
				AddLabel( colOne, (row += rowHeight - 10), kDataColor, "Press Cancel unless you know" );
				AddLabel( colOne, (row += rowHeight - 10), kDataColor, "what these options do." );
			}
			else
			{
				AddNewPage( m_Body );

				while ( m_ArchivesLoaded < m_ArchiveCount )
				{
					AddNewPage( LoadNextArchive() );
				}
			}
		}

		private void AddNewPage( string bodyText )
		{
			AddPage( ++m_TotalPages );

			AddBackground( 50, 0, 514, 280, 9270 );
			AddBackground( 50, 80, 514, 400, 9270 );

			AddLabel( 170, 15, kDataColor, kGreeting );
			AddLabel( 80, 33, kDataColor, m_Player.Name );
			AddLabel( 205, 33, kLabelColor, "Online Users" );
			AddLabel( 205, 47, kDataColor, m_UserCount );
			AddLabel( 315, 33, kLabelColor, "Total Items" );
			AddLabel( 315, 47, kDataColor, m_ItemCount );
			AddLabel( 425, 33, kLabelColor, "Total Mobiles" );
			AddLabel( 425, 47, kDataColor, m_MobileCount);

			AddHtml( 65, 95, 485, 25, CenterAndColor( 1 == m_TotalPages ? kDefaultTitle : "Previous News" ), false, false );
			AddHtml( 65, 120, 485, 330, bodyText, true, true );
			AddHtml( 65, 450, 485, 25, CenterAndColor( "Page " + ( m_TotalPages ) + " of " + ( m_ArchiveCount + 1 ) ), false, false );
			
			AddButton( 525, 15, 25, 26, 102, GumpButtonType.Reply, 0 );		// Close button
			AddButton( 60, 55, 5522, 5523, 103, GumpButtonType.Reply, 0 );	// Account button
			AddButton( 65, 12, 22153, 22154, 104, GumpButtonType.Reply, 0 );// About button
			AddButton( 500, 95, 5537, 5538, 0, GumpButtonType.Page, m_TotalPages - 1 );	// Navigation left button
			AddButton( 525, 95, 5540, 5541, 0, GumpButtonType.Page, m_TotalPages + 1 );	// Navigation right button
		}

		private static string CenterAndColor( string text )
		{
			return String.Format( "<BASEFONT COLOR=#"+ kLabelHtmlColor + "><CENTER>{0}</CENTER></BASEFONT>", text );
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			Mobile from = sender.Mobile;

			switch ( info.ButtonID )
			{
				case 100:	// Archive
					try
					{
						PreformArchive( (PlayerMobile)from );
						SafelySendGumpTo( from, false );
						World.Broadcast( kLabelColor, true, kNewMotdMessage );
					}
					catch (Exception exc) { from.SendMessage( kLabelColor, "Archive: exception encountered - " + exc.Message); }
					break;
				case 101:	// Reload
					try
					{
						PerformReload( (PlayerMobile)from );
						SafelySendGumpTo( from, false );
						World.Broadcast( kLabelColor, true, kNewMotdMessage );
					}
					catch (Exception exc) { from.SendMessage( kLabelColor, "Archive: exception encountered - " + exc.Message); }
					break;
				case 102:	// Close Account Gump
					from.CloseGump( typeof(AccountGump) );
					break;
				case 103:	// Account Gump
					from.SendGump( new AccountGump( from, sender, false ) );
					break;
				case 104:	// Credits
					from.SendGump( new AccountGump( from, sender, true ) );
					break;
				case 105:	// Motd from credits only page - $$$ not tested, may not work
					from.CloseGump( typeof(AccountGump) );
					SafelySendGumpTo( from, false );
					break;
			}
		}

		#endregion News Gump Methods
		#region AccountGump

		internal class AccountGump : Gump
		{
			public AccountGump( Mobile from, NetState state, bool showOnlyCredits ) : base( 30, 20 )
			{
				if ( null == state )
					return;

				PlayerMobile mobile = (PlayerMobile)from;

				AddPage( 1 );

				if ( !showOnlyCredits )
				{
					AddBackground( 50, 0, 479, 309, 9270 );
					AddImage( 0, 0, 10400 );
					AddImage( 0, 225, 10402 );
					AddImage( 495, 0, 10410 );
					AddImage( 495, 225, 10412 );
					AddImage( 60, 15, 5536 );
					AddImage( 275, 15, 1025 );
					AddLabel( 205, 43, NewsGump.kLabelColor, "Account Name" );
					AddLabel( 205, 57, 0x480, mobile.Account.ToString() );
					AddLabel( 205, 80, NewsGump.kLabelColor, "Account Password" );
					AddLabel( 205, 98, NewsGump.kDataColor, "-(Protected)-" );
					AddLabel( 355, 43, NewsGump.kLabelColor, "Online Character" );
					AddLabel( 355, 57, NewsGump.kDataColor, mobile.Name );
					AddLabel( 355, 80, NewsGump.kLabelColor, "Character Age" );
					AddLabel( 355, 100, NewsGump.kDataColor, mobile.GameTime.Days.ToString() + " Days" );
					AddLabel( 355, 115, NewsGump.kDataColor, mobile.GameTime.Hours.ToString() + " Hours" );
					AddLabel( 355, 130, NewsGump.kDataColor, mobile.GameTime.Minutes.ToString() + " Minutes" );
					AddLabel( 355, 144, NewsGump.kDataColor, mobile.GameTime.Seconds.ToString() + " Seconds" );
					AddLabel( 205, 120, NewsGump.kLabelColor, "Account Access Level" );
					AddLabel( 205, 135, NewsGump.kDataColor, from.AccessLevel.ToString() );
					AddButton( 470, 15, 25, 26, 0, GumpButtonType.Reply, 0 );		// Close
					AddLabel( 355, 165, NewsGump.kLabelColor, "IP Address" );
					AddLabel( 355, 180, NewsGump.kDataColor, state.ToString());
					AddLabel( 205, 165, NewsGump.kLabelColor, "Client Version" );
//					AddLabel( 205, 180, NewsGump.kDataColor, (( state.Flags & 0x10 ) != 0 ) ? "Samurai Empire" : (( state.Flags & 0x08 ) != 0) ? "Age of Shadows" : (( state.Flags & 0x04 ) != 0) ? "Blackthorn's Revenge" : (( state.Flags & 0x02 ) != 0 ) ? "Third Dawn" : (( state.Flags & 0x01 ) != 0 ) ? "Renaissance" : "The Second Age" );
					AddLabel( 205, 200, NewsGump.kLabelColor, "Client Patch" );
					AddLabel( 205, 215, NewsGump.kDataColor, null == state.Version ? "(null)" : state.Version.ToString() );
					AddButton( 445, 15, 22153, 22154, 0, GumpButtonType.Page, 2 );	// About

					AddPage( 2 );
				}

				// Credits page
				AddBackground( 50, 0, 479, 309, 9270 );
				AddImage( 0, 0, 10400 );
				AddImage( 0, 225, 10402 );
				AddImage( 495, 0, 10410 );
				AddImage( 495, 225, 10412 );
				AddImage( 60, 15, 5536 );
				AddLabel( 205, 45, NewsGump.kLabelColor, "Code By" );
				AddLabel( 205, 60, NewsGump.kDataColor, "Xanthos" );
				AddLabel( 205, 80, NewsGump.kLabelColor, "Inspiration By" );
				AddLabel( 205, 95, NewsGump.kDataColor, "Princess Monika" );
				AddLabel( 205, 110, NewsGump.kLabelColor, "Gumps By" );
				AddLabel( 205, 125, NewsGump.kDataColor, "Viago" );
				AddLabel( 355, 45, NewsGump.kLabelColor, "Credit To" );
				AddLabel( 355, 60, NewsGump.kDataColor, "Xuse" );
				AddLabel( 355, 75, NewsGump.kDataColor, "The RunUO Comunity" );
				AddLabel( 355, 90, NewsGump.kDataColor, "Lady Rouge" );
				AddLabel( 355, 105, NewsGump.kDataColor, "RoninGT" );
				AddButton( 470, 15, 25, 26, 0, GumpButtonType.Reply, 0 );		// Close
			}
			#endregion AccountGump
		}
	}
}