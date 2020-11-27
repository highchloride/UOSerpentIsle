/*////////////////////////////////////////////////////////////////////////////
 * 
 * Speaker Mobile by Arya
 * Version 1
 * 
 * This NPC responds to keywords with specified text lines. Configuration
 * is done through in game gumps. Each NPC supports an unlimited number
 * of keywords/speech combinations.
 * 
 *//////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections;

using Server;
using Server.Mobiles;
using Server.Items;
using Server.Gumps;

namespace Arya.Mobiles
{
	public class Speaker : BaseCreature
	{
		// Version 1
		private bool m_SpeechStartDelay = true; // Specifies if there should be a delay before starting the speech
		private bool m_Sequence = false; // Specifies if the entries must be matched in sequence
		private TimeSpan m_SequenceTimeOut = TimeSpan.FromMinutes( 5 ); // The amount of time before the NPC resets the sequence to entry one
		private int m_SpeechIndex = 0; // The index of the next speech in the sequence.
		private DateTime m_SpeechStart = DateTime.Now;
		// Version 0
		private int m_SpeechInterval = 3; // Seconds between each line
		private int m_ResponseRange = 5;
		private ArrayList m_Speech;
		private bool m_Speaking = false;
		private bool m_Updating = false;
		private SpeechTimer m_Timer = null;
		private SpeechEntry m_SelectedEntry = null;
		private int m_NextLine;
		private Mobile m_User;

		[ CommandProperty( AccessLevel.GameMaster ) ]
		public bool Updating
		{
			get { return m_Updating; }
		}

		[ CommandProperty( AccessLevel.GameMaster ) ]
		public bool Speaking
		{
			get { return m_Speaking; }
		}

		[ CommandProperty( AccessLevel.GameMaster ) ]
		public int SpeechIndex // This is 1-Based for users
		{
			get 
			{
				if ( m_SpeechStart + m_SequenceTimeOut <= DateTime.Now )
				{
					m_SpeechIndex = 0;
				}

				return m_SpeechIndex + 1;
			}
			set
			{
				if ( m_Speech.Count == 0 )
				{
					m_SpeechIndex = 0;
				}
				else if ( value > 0 && value <= m_Speech.Count )
				{
					m_SpeechIndex = value - 1;
					m_SpeechStart = DateTime.Now;
				}
			}
		}

		[ CommandProperty( AccessLevel.GameMaster ) ]
		public bool Sequence
		{
			get { return m_Sequence; }
			set { m_Sequence = value; }
		}

		[ CommandProperty( AccessLevel.GameMaster ) ]
		public TimeSpan SequenceTimeOut
		{
			get { return m_SequenceTimeOut; }
			set { m_SequenceTimeOut = value; }
		}

		[ CommandProperty( AccessLevel.GameMaster ) ]
		public bool SpeechStartDelay
		{
			get { return m_SpeechStartDelay; }
			set { m_SpeechStartDelay = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int SpeechInterval
		{
			get { return m_SpeechInterval; }
			set { m_SpeechInterval = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int ResponseRange
		{
			get { return m_ResponseRange; }
			set { m_ResponseRange = value; }
		}

		public ArrayList Speech
		{
			get { return m_Speech; }
			set { m_Speech = value; }
		}

		[Constructable]
		public Speaker() : base ( AIType.AI_Thief, FightMode.None, 10, 1, 0.8, 1.6 )
		{
			InitStats( 100, 25, 100 );

			if ( Female == Utility.RandomBool() )
			{
				// Female
				Name = NameList.RandomName( "female" );
				Body = 0x191;
			}
			else
			{
				Name = NameList.RandomName( "male" );
				Body = 0x190;
			}

			Blessed = true;
			Hidden = true;
			CantWalk = true;
			RangeHome = 5;

			InitOutfit();

			m_Speech = new ArrayList();

			EventSink.Disconnected += new DisconnectedEventHandler(EventSink_Disconnected);
		}

		public Speaker( Serial serial ) : base( serial )
		{
			EventSink.Disconnected += new DisconnectedEventHandler(EventSink_Disconnected);
		}

		private void EventSink_Disconnected(DisconnectedEventArgs e)
		{
			if ( ! m_Updating )
				return;

			if ( e.Mobile == m_User )
				EndUpdateDisconnected();
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize (writer);

			writer.Write( (int) 1 ); // Version

			// Version 1 Starts here
			writer.Write( m_SpeechStartDelay );
			writer.Write( m_Sequence );
			writer.Write( m_SequenceTimeOut );
			writer.Write( m_SpeechIndex );


			// Version 0 Starts here
			writer.Write( m_SpeechInterval );
			writer.Write( m_ResponseRange );

			writer.Write( m_Speech.Count );

			foreach( SpeechEntry entry in m_Speech )
			{
				entry.Serialize( writer );
			}
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize (reader);

			int version = reader.ReadInt();

			switch ( version )
			{
				case 1:

					m_SpeechStartDelay = reader.ReadBool();
					m_Sequence = reader.ReadBool();
					m_SequenceTimeOut = reader.ReadTimeSpan();
					m_SpeechIndex = reader.ReadInt();
					goto case 0;

				case 0:

					m_SpeechInterval = reader.ReadInt();
					m_ResponseRange = reader.ReadInt();

					int NumOfEntries = reader.ReadInt();

					m_Speech = new ArrayList();

					for ( int i = 0; i < NumOfEntries; i++ )
					{
						m_Speech.Add( SpeechEntry.Deserialize( reader, version ) );
					}
					break;

			}

			

			EndUpdate();
		}

		#region Outfit

		private void InitOutfit()
		{
			switch ( Utility.Random( 3 ) )
			{
				case 0: AddItem( new FancyShirt( GetRandomHue() ) ); break;
				case 1: AddItem( new Doublet( GetRandomHue() ) ); break;
				case 2: AddItem( new Shirt( GetRandomHue() ) ); break;
			}

			switch ( ShoeType )
			{
				case VendorShoeType.Shoes: AddItem( new Shoes( GetShoeHue() ) ); break;
				case VendorShoeType.Boots: AddItem( new Boots( GetShoeHue() ) ); break;
				case VendorShoeType.Sandals: AddItem( new Sandals( GetShoeHue() ) ); break;
				case VendorShoeType.ThighBoots: AddItem( new ThighBoots( GetShoeHue() ) ); break;
			}

			int hairHue = Utility.RandomHairHue();

			if ( Female )
			{
				switch ( Utility.Random( 6 ) )
				{
					case 0: AddItem( new ShortPants( GetRandomHue() ) ); break;
					case 1:
					case 2: AddItem( new Kilt( GetRandomHue() ) ); break;
					case 3:
					case 4:
					case 5: AddItem( new Skirt( GetRandomHue() ) ); break;
				}

				switch ( Utility.Random( 9 ) )
				{
					case 0: AddItem( new Afro( hairHue ) ); break;
					case 1: AddItem( new KrisnaHair( hairHue ) ); break;
					case 2: AddItem( new PageboyHair( hairHue ) ); break;
					case 3: AddItem( new PonyTail( hairHue ) ); break;
					case 4: AddItem( new ReceedingHair( hairHue ) ); break;
					case 5: AddItem( new TwoPigTails( hairHue ) ); break;
					case 6: AddItem( new ShortHair( hairHue ) ); break;
					case 7: AddItem( new LongHair( hairHue ) ); break;
					case 8: AddItem( new BunsHair( hairHue ) ); break;
				}
			}
			else
			{
				switch ( Utility.Random( 2 ) )
				{
					case 0: AddItem( new LongPants( GetRandomHue() ) ); break;
					case 1: AddItem( new ShortPants( GetRandomHue() ) ); break;
				}

				switch ( Utility.Random( 8 ) )
				{
					case 0: AddItem( new Afro( hairHue ) ); break;
					case 1: AddItem( new KrisnaHair( hairHue ) ); break;
					case 2: AddItem( new PageboyHair( hairHue ) ); break;
					case 3: AddItem( new PonyTail( hairHue ) ); break;
					case 4: AddItem( new ReceedingHair( hairHue ) ); break;
					case 5: AddItem( new TwoPigTails( hairHue ) ); break;
					case 6: AddItem( new ShortHair( hairHue ) ); break;
					case 7: AddItem( new LongHair( hairHue ) ); break;
				}

				switch ( Utility.Random( 5 ) )
				{
					case 0: AddItem( new LongBeard( hairHue ) ); break;
					case 1: AddItem( new MediumLongBeard( hairHue ) ); break;
					case 2: AddItem( new Vandyke( hairHue ) ); break;
					case 3: AddItem( new Mustache( hairHue ) ); break;
					case 4: AddItem( new Goatee( hairHue ) ); break;
				}
			}
		}

		private int GetRandomHue()
		{
			switch ( Utility.Random( 5 ) )
			{
				default:
				case 0: return Utility.RandomBlueHue();
				case 1: return Utility.RandomGreenHue();
				case 2: return Utility.RandomRedHue();
				case 3: return Utility.RandomYellowHue();
				case 4: return Utility.RandomNeutralHue();
			}
		}

		private int GetShoeHue()
		{
			if ( 0.1 > Utility.RandomDouble() )
				return 0;

			return Utility.RandomNeutralHue();
		}

		private VendorShoeType ShoeType
		{
			get{ return VendorShoeType.Shoes; }
		}

		#endregion

		public override void OnDoubleClick(Mobile from)
		{
			if ( from.AccessLevel >= AccessLevel.GameMaster )
			{
				if ( m_Speaking )
				{
					from.SendMessage( "The NPC is currently performing a task. Please wait for it to end before configuring it." );
					return;
				}

				if ( m_Updating )
				{
					from.SendMessage( "Someone else is currently configuring this NPC. Please try again in a short while." );
					return;
				}

				BeginUpdate( from );
				from.SendGump( new SpeakerGump( from, this, 0 ) );
			}
		}

		public override bool HandlesOnSpeech(Mobile from)
		{
			return true;
		}

		public override void OnSpeech(SpeechEventArgs e)
		{
			if ( m_Speaking ) // Ignore anything while speaking
				return;

			if ( m_Updating )
			{
				Say( "I apologize, but I am very busy organizing my thoughts. Please come back shortly..." );
				return;
			}

			if ( e.Mobile.InRange( this, m_ResponseRange ) )
			{
			m_SelectedEntry = SelectEntry( e.Speech );

				if ( m_SelectedEntry != null )
				{
					Speak();
				}
			}
		}

		private SpeechEntry SelectEntry( string speech )
		{
			if ( m_Speech.Count == 0 )
				return null;

			if ( m_Sequence )
			{
				if ( m_SpeechIndex < 0 || m_SpeechIndex >= m_Speech.Count )
				{
					m_SpeechIndex = 0; // Reset when out of bounds
				}

				// Reset sequence if timed out (players left the NPC or whatever)
				if ( m_SpeechStart + m_SequenceTimeOut <= DateTime.Now )
				{
					m_SpeechIndex = 0;
				}

				SpeechEntry next = m_Speech[ m_SpeechIndex ] as SpeechEntry;

				if ( next.Match( speech ) )
				{
					m_SpeechStart = DateTime.Now;
					m_SpeechIndex++;

					if ( m_SpeechIndex == m_Speech.Count )
					{
						// Last one, reset to 0
						m_SpeechIndex = 0;
					}

					return next;
				}
			}
			else
			{
				// Not in sequence, just select appropriate entry
				foreach ( SpeechEntry entry in m_Speech )
				{
					if ( entry.Match( speech ) )
						return entry;
				}
			}
			return null;
		}

		private void Speak()
		{
			if ( m_SpeechStartDelay )
			{
				// First run timer then talk
				m_Speaking = true;

				if ( m_Timer != null )
				{
					m_Timer.Stop();
					m_Timer = null;
				}

				m_NextLine = 0;
				m_Timer = new SpeechTimer( this, m_SpeechInterval );
				m_Timer.Start();
			}
			else
			{
				// Start talking right away
				Say( (string) m_SelectedEntry.Speech[ 0 ] );
				m_Speaking = true;

				if ( m_SelectedEntry.Speech.Count > 1 )
				{
					if ( m_Timer != null )
					{
						m_Timer.Stop();
						m_Timer = null;
					}

					m_NextLine = 1;
					m_Timer = new SpeechTimer( this, m_SpeechInterval );
					m_Timer.Start();
				}
				else
				{
					m_SelectedEntry = null;
					m_Speaking = false;
				}
			}
		}

		public void SayNext()
		{
			Say( (string) m_SelectedEntry.Speech[ m_NextLine ] );

			m_NextLine++;

			if ( m_NextLine >= m_SelectedEntry.Speech.Count )
			{
				// End here
				m_Timer.Stop();
				m_Timer = null;
				m_SelectedEntry = null;
				m_Speaking = false;
			}
		}

		private void BeginUpdate( Mobile m )
		{
			CantWalk = true;
			Hidden = true;
			m_Updating = true;
			m_User = m;
		}

		public void EndUpdate()
		{
			if ( m_Speech.Count > 0 )
			{
				CantWalk = false;
				Hidden = false;
			}

			if ( Home == Point3D.Zero )
				Home = Location;

			m_Updating = false;
		}

		private void EndUpdateDisconnected()
		{
			CantWalk = true;
			Hidden = true;

			if ( Home == Point3D.Zero )
				Home = Location;

			m_Updating = false;
		}

		public override void OnDelete()
		{
			if ( m_Timer != null )
			{
				m_Timer.Stop();
			}

			base.OnDelete ();
		}

		#region Speech Entry

		private class SpeechEntry
		{
			private bool m_MatchAll = false; // Means all keywords must be matched for the NPC to respond
			private ArrayList m_Keywords;
			private ArrayList m_Speech;

			public ArrayList Keywords
			{
				get { return m_Keywords; }
				set { m_Keywords = value; }
			}

			public ArrayList Speech
			{
				get { return m_Speech; }
				set { m_Speech = value; }
			}

			public bool MatchAll
			{
				get { return m_MatchAll; }
				set { m_MatchAll = value; }
			}

			public SpeechEntry()
			{
				m_Keywords = new ArrayList();
				m_Speech = new ArrayList();
			}

			public SpeechEntry( ICollection keywords, ICollection speech ) : this ()
			{
				m_Keywords.AddRange( keywords );
				m_Speech.AddRange( speech );
			}

			public void Serialize( GenericWriter writer )
			{
				writer.Write( m_MatchAll );

				writer.Write( (int) m_Keywords.Count );
				foreach( string s in m_Keywords )
					writer.Write( s );

				writer.Write( (int) m_Speech.Count );
				foreach( string s in m_Speech )
					writer.Write( s );
			}

			public static SpeechEntry Deserialize( GenericReader reader, int version )
			{
				SpeechEntry entry = new SpeechEntry();

				entry.m_MatchAll = reader.ReadBool();

				int NumOfKeywords = reader.ReadInt();

				for ( int i = 0; i < NumOfKeywords; i++ )
					entry.m_Keywords.Add( reader.ReadString() );

				int NumOfSpeech = reader.ReadInt();
				
				for ( int i = 0; i < NumOfSpeech; i++ )
					entry.m_Speech.Add( reader.ReadString() );

				return entry;
			}

			public string GetHtml()
			{
				string html = "<p>Keywords :";

				if ( m_Keywords.Count > 0 )
				{
					html += (string) m_Keywords[ 0 ];

					if ( m_Keywords.Count > 1 )
					{
						for ( int i = 1; i < m_Keywords.Count; i++ )
						{
							html += string.Format( ", {0}", (string) m_Keywords[ i ] );
						}
					}
				}

				if ( m_Speech.Count > 0 )
				{
					html += "</p><p>Speech:";
					foreach ( string s in m_Speech )
					{
						html += string.Format( "<br>- {0}", s );
					}
					html += "</p>";
				}

				return html;
			}

			public bool Match( string speech )
			{
				speech = speech.ToLower();

				if ( m_MatchAll )
				{
					foreach ( string s in m_Keywords )
					{
						if ( speech.IndexOf( s.ToLower() ) < 0 )
							return false;
					}
					return true;
				}
				else
				{
					foreach ( string s in m_Keywords )
					{
						if ( speech.IndexOf( s.ToLower() ) >= 0 )
							return true;
					}
					return false;
				}
			}

			public bool IsValid()
			{
				return m_Keywords.Count > 0 && m_Speech.Count > 0;
			}

			public override string ToString()
			{
				if ( m_Keywords.Count == 0 )
					return "Not a valid entry";

				string str = "<p><basefont color=\"#CCCCCC\">";
				str += (string) m_Keywords[ 0 ];

				if ( m_Keywords.Count > 1 )
				{
					for ( int i = 1; i < m_Keywords.Count; i++ )
						str += string.Format( " - {0}", (string) m_Keywords[ i ] );
				}

				str += "</basefont></p>";

				return str;
			}

		}

		#endregion

		#region Speech Timer

		private class SpeechTimer : Timer
		{
			private Speaker m_Speaker;

			public SpeechTimer( Speaker speaker, int interval ) : base ( new TimeSpan( 0, 0, 0, interval, 0 ), new TimeSpan( 0, 0, 0, interval, 0 ) )
			{
				m_Speaker = speaker;
			}

			protected override void OnTick()
			{
				if ( m_Speaker != null )
				{
					m_Speaker.SayNext();
				}
			}
		}

		#endregion

		#region Speech Entry Gump

		private class SpeechEntryGump : Gump
		{
			private Mobile m_User;
			private Speaker m_Speaker;
			private SpeechEntry m_Entry;
			private int m_KeyIndex = 0;
			private int m_SpeechIndex = 0;
			private bool m_EditMode = false;

			public SpeechEntryGump( Mobile user, Speaker speaker ) : this( user, speaker, null, 0, 0, false )
			{
			}

			public SpeechEntryGump( Mobile user, Speaker speaker, SpeechEntry entry ) : this( user, speaker, entry, entry.Keywords.Count, entry.Speech.Count, true )
			{
			}

			public SpeechEntryGump( Mobile user, Speaker speaker, SpeechEntry entry, int keyIndex, int speechIndex, bool editMode ) : base( 50, 50 )
			{
				m_User = user;
				m_Speaker = speaker;
				m_KeyIndex = keyIndex;
				m_SpeechIndex = speechIndex;
				m_EditMode = editMode;

				m_User.CloseGump( typeof( SpeechEntryGump ) );

				if ( entry != null )
					m_Entry = entry;
				else
					m_Entry = new SpeechEntry();

				MakeGump();
			}

			private void MakeGump()
			{
				AddPage( 0 );

				AddBackground( 0, 0, 600, 450, 9270 );
				AddImageTiled( 1, 1, 598, 448, 2624 );
				AddAlphaRegion( 2, 2, 596, 446 );

				AddLabel( 10, 10, 37, "Add keyword" );

				// Keyword up and down buttons
				if ( m_Entry.Keywords.Count > 0 )
				{
					int prevKey = m_KeyIndex - 1;
					int nextKey = m_KeyIndex + 1;

					if ( prevKey >= 0 )
					{
						// Previous key exists - BUTTON 10
						AddButton( 40, 30, 5600, 5604, 10, GumpButtonType.Reply, 0 );
						AddLabel( 95, 10, 631, (string) m_Entry.Keywords[ prevKey ] );
					}

					if ( nextKey < m_Entry.Keywords.Count )
					{
						// Next key exists - BUTTON 11
						AddButton( 40, 50, 5602, 5606, 11, GumpButtonType.Reply, 0 );
						AddLabel( 95, 70, 631, (string) m_Entry.Keywords[ nextKey ] );
					}
				}

				AddImageTiled( 95, 40, 150, 20, 3004 );
				AddImageTiled( 96, 41, 148, 18, 2624 );
				AddAlphaRegion( 97, 42, 146, 16 );

				string baseKeyword = "";

				if ( m_KeyIndex < m_Entry.Keywords.Count )
				{
					// Display a keyword. Enable modify/delete buttons
					baseKeyword = (string) m_Entry.Keywords[ m_KeyIndex ];

					// Modify keyword: BUTTON 12
					AddButton( 250, 10, 4029, 4031, 12, GumpButtonType.Reply, 0 );
					AddLabel( 280, 13, 86, "MODIFY CURRENT" );

					// Delete keyword: BUTTON 13
					AddButton( 400, 10, 4017, 4019, 13, GumpButtonType.Reply, 0 );
					AddLabel( 430, 13, 86, "DELETE" );
				}

				// KEYWORD: TEXT ENTRY 0
				AddTextEntry( 98, 40, 144, 20, 151, 0, baseKeyword );

				// Add keyword: BUTTON 2
				AddButton( 250, 39, 4011, 4013, 2, GumpButtonType.Reply, 0 );

				AddLabel( 10, 90, 37, "Add speech" );

				if ( m_Entry.Speech.Count > 0 )
				{
					int prevSpeech = m_SpeechIndex - 1;
					int nextSpeech = m_SpeechIndex + 1;

					if ( prevSpeech >= 0 )
					{
						// Prev Speech: BUTTON 20
						AddButton( 40, 120, 5600, 5604, 20, GumpButtonType.Reply, 0 );
						AddLabelCropped( 95, 90, 150, 20, 631, (string) m_Entry.Speech[ prevSpeech ] );
					}

					if ( nextSpeech < m_Entry.Speech.Count )
					{
						// Next Speech: BUTTON 21
						AddButton( 40, 140, 5602, 5606, 21, GumpButtonType.Reply, 0 );
						AddLabelCropped( 95, 150, 460, 20, 631, (string) m_Entry.Speech[ nextSpeech ] );
					}
				}

				string baseSpeech = "";

				AddImageTiled( 95, 120, 460, 20, 3004 );
				AddImageTiled( 96, 121, 458, 18, 2624 );
				AddAlphaRegion( 97, 122, 456, 16 );

				if ( m_SpeechIndex < m_Entry.Speech.Count )
				{
					// Display speech line, modify and delete buttons
					baseSpeech = (string) m_Entry.Speech[ m_SpeechIndex ];

					// Modify Current. BUTTON 22
					AddButton( 250, 90, 4029, 4031, 22, GumpButtonType.Reply, 0 );
					AddLabel( 280, 93, 86, "MODIFY CURRENT" );

					// Delete. BUTTON 23
					AddButton( 400, 90, 4017, 4019, 23, GumpButtonType.Reply, 0 );
					AddLabel( 430, 93, 86, "DELETE" );
				}

				AddTextEntry( 98, 120, 454, 20, 151, 1, baseSpeech );

				AddButton( 560, 119, 4011, 4013, 3, GumpButtonType.Reply, 0 );

				AddImageTiled( 10, 180, 580, 230, 3004 );
				AddImageTiled( 11, 181, 578, 228, 2624 );
				AddAlphaRegion( 12, 182, 576, 226 );

				Add( new GumpHtml( 12, 182, 576, 226, m_Entry.GetHtml(), false, true ) );

				AddButton( 275, 420, 4023, 4025, 1, GumpButtonType.Reply, 0 );

				// Match all checkbox
				AddCheck( 10, 420, 9721, 9724, m_Entry.MatchAll, 100 );
				AddLabel( 45, 423, 37, "MATCH ALL KEYWORDS" );
			}

			public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
			{
				string keyword = "";
				string speech = "";

				foreach( TextRelay tr in info.TextEntries )
				{
					switch ( tr.EntryID )
					{
						case 0 : 
							if ( tr.Text != null )
								keyword = tr.Text;
							break;

						case 1:
							if ( tr.Text != null )
								speech = tr.Text;
							break;
					}
				}
				

				m_Entry.MatchAll = ( info.Switches.Length == 1 ); // Only check possible

				switch ( info.ButtonID )
				{
					case 1: // Ok finalize
						
						if ( !m_EditMode )
						{
							if ( m_Entry.IsValid() )
								m_Speaker.Speech.Add( m_Entry );
							else
							{
								m_User.SendMessage( "The specified entry is not complete to be used by the NPC. It will not be added." );
							}
						}
						else
						{
							if ( ! m_Entry.IsValid() )
							{
								m_User.SendMessage( "The modifications didn't result in a valid speech script, so the entry has been removed." );
								m_Speaker.Speech.Remove( m_Entry );
							}
						}
						
						m_User.SendGump( new SpeakerGump( m_User, m_Speaker, 0 ) );

						base.OnResponse( sender, info );

						return;

					case 2: // Add keyword

						if ( keyword.Length > 0 )
						{
							if ( m_KeyIndex >= m_Entry.Keywords.Count - 1 )
							{
								// Add at end
								m_Entry.Keywords.Add( keyword );
								m_KeyIndex = m_Entry.Keywords.Count;
							}
							else
							{
								try
								{
									m_Entry.Keywords.Insert( m_KeyIndex++, keyword );
								}
								catch
								{
									m_User.SendMessage( "A bug has been encountered, operation aborted." );
								}
							}
						}
						else
						{
							m_User.SendMessage( "Cannot add an empty keyword." );
						}

						break;

					case 3: // Add speech line

						if ( speech.Length > 0 )
						{
							if ( m_SpeechIndex >= m_Entry.Speech.Count - 1 )
							{
								// Add at end
								m_Entry.Speech.Add( speech );
								m_SpeechIndex = m_Entry.Speech.Count;
							}
							else
							{
								try
								{
									m_Entry.Speech.Insert( m_SpeechIndex++, speech );
								}
								catch
								{
									m_User.SendMessage( "A bug has been encountered, operation aborted." );
								}
							}
						}
						else
						{
							m_User.SendMessage( "Cannot add empty text" );
						}

						break;

					case 10:		// Previous keyword
						
						if ( m_KeyIndex > 0 )
							m_KeyIndex--;
						else
						{
							m_User.SendMessage( "A bug has been encountered, operation aborted." );
						}

						break;

					case 11:		// Next keyword

						if ( m_KeyIndex < m_Entry.Keywords.Count - 1 )
							m_KeyIndex++;
						else
						{
							m_User.SendMessage( "A bug has been encountered, operation aborted." );
						}

						break;

					case 12:		// Modify keyword

						if ( keyword.Length > 0 )
						{
							try
							{
								m_Entry.Keywords[ m_KeyIndex ] = keyword;
							}
							catch
							{
								m_User.SendMessage( "A bug has been encountered, operation aborted." );
							}
						}
						else
							m_User.SendMessage( "The keyword cannot be empty, use the delete button instead." );

						break;

					case 13:		// Delete keyword

						try
						{
							m_Entry.Keywords.RemoveAt( m_KeyIndex );
						}
						catch
						{
							m_User.SendMessage( "A bug has been encountered, operation aborted." );
						}

						break;

					case 20:		// Previous speech line

						if ( m_SpeechIndex > 0 )
							m_SpeechIndex--;
						else
						{
							m_User.SendMessage( "A bug has been encountered, operation aborted." );
						}

						break;

					case 21:		// Next speech line

						if ( m_SpeechIndex < m_Entry.Speech.Count - 1 )
							m_SpeechIndex++;
						else
						{
							m_User.SendMessage( "A bug has been encountered, operation aborted." );
						}

						break;

					case 22:		// Modify speech

							if ( speech.Length > 0 )
							{
								try
								{
									m_Entry.Speech[ m_SpeechIndex ] = speech;
								}
								catch
								{
									m_User.SendMessage( "A bug has been encountered, operation aborted." );
								}
							}
							else
								m_User.SendMessage( "The text can't be empty. Use the delete button instead" );

							break;

					case 23:		// Delete speech

						try
						{
							m_Entry.Speech.RemoveAt( m_SpeechIndex );
						}
						catch
						{
							m_User.SendMessage( "A bug has been encountered, operation aborted." );
						}

						break;
				}

				m_User.SendGump( new SpeechEntryGump( m_User, m_Speaker, m_Entry, m_KeyIndex, m_SpeechIndex, m_EditMode ) );
			}
		}
		#endregion

		#region Speaker Gump

		private class SpeakerGump : Gump
		{
			private Speaker m_Speaker;
			private Mobile m_User;
			private int m_Page;

			public SpeakerGump( Mobile user, Speaker speaker, int page ) : base( 50, 50 )
			{
				m_Speaker = speaker;
				m_User = user;
				m_Page = page;

				MakeGump();
			}

			private void MakeGump()
			{
				AddPage( 0 );

				AddImageTiled( 0, 0, 300, 330, 2624 );
				AddAlphaRegion( 1, 1, 298, 328 );

				AddButton( 5, 10, 4029, 4031, 2, GumpButtonType.Reply, 0 );
				AddLabel( 40, 10, 37, "Add a new speech entry" );

				AddButton( 215, 10, 4005, 4006, 5, GumpButtonType.Reply, 0 );
				AddLabel( 250, 10, 37, "Props" );

				int NumOfPages = m_Speaker.Speech.Count / 4;
				if ( NumOfPages > 0 && m_Speaker.Speech.Count % 4 == 0 )
					NumOfPages--;

				// BUTTONS
				//
				// 2: New speech entry
				// 3: Previous page
				// 4: Next page
				// 1: OK

				if ( NumOfPages > 0 )
				{
					if ( m_Page > 0 )
					{
						// Prev page
						AddButton( 10, 290, 4014, 4016, 3, GumpButtonType.Reply, 0 );
						AddLabel( 45, 294, 37, "Previous Page" );
					}
					if ( m_Page < NumOfPages )
					{
						// Next page
						AddButton( 150, 290, 4005, 4007, 4, GumpButtonType.Reply, 0 );
						AddLabel( 185, 294, 37, "Next Page" );
					}
				}

				for ( int i = 0; i < 4 && m_Page * 4 + i < m_Speaker.Speech.Count ; i++ )
				{
					// 10, 11 - 12, 13...
					AddButton( 5, 50 + i * 60, 4005, 4007, 10 + 2 * i, GumpButtonType.Reply, 0 );
					AddButton( 5, 80 + i * 60, 4017, 4019, 11 + 2 * i, GumpButtonType.Reply, 0 );

					AddImageTiled( 35, 50 + i * 60, 230, 50, 3004 );
					AddImageTiled( 36, 51 + i * 60, 228, 48, 2624 );
					AddAlphaRegion( 37, 52 + i * 60, 226, 46 );
					Add( new GumpHtml( 54, 52 + i * 60, 222, 46, m_Speaker.Speech[ m_Page * 4 + i ].ToString(), false, false ) );

					AddLabel( 267, 63 + i * 60, 0x480, ( m_Page * 4 + i + 1 ).ToString() );
				}

				AddButton( 10, 290, 4023, 4025, 1, GumpButtonType.Reply, 0 );
			}

			public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
			{
				switch ( info.ButtonID )
				{
					case 0:
					case 1:
						m_Speaker.EndUpdate();
						break;

					case 2:
						m_User.SendGump( new SpeechEntryGump( m_User, m_Speaker ) );
						break;

					case 3:
						m_User.SendGump( new SpeakerGump( m_User, m_Speaker, m_Page - 1 ) );
						return;

					case 4:
						m_User.SendGump( new SpeakerGump( m_User, m_Speaker, m_Page + 1 ) );
						return;

					case 5:
						m_User.SendGump( this );

						PropertiesGump pGump = new PropertiesGump( m_User, m_Speaker );
						pGump.X = 350;
						pGump.Y = 50;
						m_User.SendGump( pGump );
						return;

					default:

						int index = ( info.ButtonID - 10 ) / 2;
						int action = ( info.ButtonID - 10 ) % 2; // 0 - Edit, 1 - Delete

						int speechIndex = m_Page * 4 + index;

						switch ( action )
						{
							case 0:
								m_User.SendGump( new SpeechEntryGump( m_User, m_Speaker, (SpeechEntry) m_Speaker.Speech[ speechIndex ] ) );
								break;
							case 1:
								m_Speaker.Speech.RemoveAt( speechIndex );
								m_User.SendGump( new SpeakerGump( m_User, m_Speaker, 0 ) );
								return;
						}
						break;
				}
				base.OnResponse( sender, info );
			}
		}

		#endregion
	}
}