using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Engale.BookPublishing
{
	public class BP_Save
	{
		public static int Version = 1;
		public static string SavePath = "Saves\\BookPublisher";
		public static string FileName = "books.xml";
		
		[CallPriority( 101 )]
		public static void Initialize()
		{
			EventSink.WorldSave += new WorldSaveEventHandler( WriteData );
		}
		
		public static void WriteData(WorldSaveEventArgs e)
		{
			if ( !Directory.Exists( SavePath ) )
				Directory.CreateDirectory( SavePath );

			string filePath = Path.Combine( SavePath, FileName );

			using ( StreamWriter file = new StreamWriter( filePath ) )
			{
				XmlTextWriter xml = new XmlTextWriter( file );

				xml.Formatting = Formatting.Indented;
				xml.IndentChar = '\t';
				xml.Indentation = 1;

				xml.WriteStartDocument( true );

				xml.WriteStartElement( "BookPublisher" );

				xml.WriteAttributeString( "Version", Version.ToString() );

				for( int i = 0; i < Publisher.Books.Count; i++ )
					WriteBookNode( Publisher.Books[i], xml );

				xml.WriteEndElement();

				xml.Close();
			}
		}

		public static void WriteBookNode( BookContent bc, XmlTextWriter xml )
		{			
			xml.WriteStartElement( "PublishedBook" );

			xml.WriteAttributeString( "Title", bc.Title );
			xml.WriteAttributeString( "Author", bc.Author );
			xml.WriteAttributeString( "PagesCount", bc.Pages.Length.ToString() );
			for ( int i = 0; i < bc.Pages.Length; i++ )
					WritePageNode( bc.Pages[i], xml );

			xml.WriteEndElement();
		}

		public static void WritePageNode(BookPageInfo bpi, XmlTextWriter xml )
		{
			xml.WriteStartElement( "Page" );
			xml.WriteAttributeString( "Lines", bpi.Lines.Length.ToString() );
			for( int i = 0; i < bpi.Lines.Length; i++ )
				xml.WriteAttributeString( "Line" + i.ToString(), bpi.Lines[i] );

			xml.WriteEndElement();
		}
	}
}