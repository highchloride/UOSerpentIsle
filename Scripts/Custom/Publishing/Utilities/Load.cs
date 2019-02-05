using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Engale.BookPublishing
{
	public class BP_Load
	{
		[CallPriority( 101 )]
		public static void Initialize()
		{
			ReadData(Path.Combine( BP_Save.SavePath, BP_Save.FileName ));
		}
		
		public static void ReadData( string filePath )
		{
			if ( !File.Exists( filePath ) )
				return;

			Console.WriteLine();
			Console.WriteLine("Book Publishing: Loading...");

			XmlDocument doc = new XmlDocument();
			doc.Load( filePath );

			XmlElement root = doc["BookPublisher"];
			int version = Utility.ToInt32(root.GetAttribute("Version"));

			if( root.HasChildNodes )
			{
				foreach ( XmlElement book in root.GetElementsByTagName("PublishedBook") )
				{
					try{ ReadBookNode( book ); }
					catch{ Console.WriteLine( "Warning: Book Publisher load failed." ); }
				}
			}
		}

		public static void ReadBookNode( XmlElement parent )
		{
			try
			{
				string title = parent.GetAttribute("Title");
				string author = parent.GetAttribute("Author");
				int pgcnt = Utility.ToInt32(parent.GetAttribute("PagesCount"));
				BookPageInfo[] pages = new BookPageInfo[pgcnt];
				
				if( parent.HasChildNodes )
				{
					int i = 0;
					XmlElement child = parent.FirstChild as XmlElement;
					pages[i++] = ReadPageNode(child);
					while( child.NextSibling != null && i < pages.Length)
					{
						child = child.NextSibling as XmlElement;
						pages[i++] = ReadPageNode(child);
					}
				}
				Publisher.Books.Add(new BookContent(title, author, pages) );
			}
			catch
			{
				Console.WriteLine( "failed." );
			}
			Console.WriteLine( "done." );
		}

		public static BookPageInfo ReadPageNode( XmlElement parent )
		{
			int lncnt = Utility.ToInt32(parent.GetAttribute("Lines"));
			string[] lines = new string[lncnt];
			
			for(int i = 0; i < lncnt; i++)
				lines[i] = parent.GetAttribute("Line" + i.ToString());
			
			return new BookPageInfo(lines);
		}
	}
}
