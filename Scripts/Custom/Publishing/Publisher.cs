using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Gumps;
using Server.Commands;
using Engale.BookPublishing;

namespace Server.Mobiles
{
	public class Publisher : Merchant
	{
		private static List<BookContent> m_Books;
		
		public static List<BookContent> Books
		{
			get{
				if(m_Books == null)
					m_Books = new List<BookContent>();
				return m_Books;
			}
			set{
				m_Books = value;
			}
		}
		
		[Constructable]
		public Publisher()
		{
			Title = "the publisher";
		}
		
		public override bool OnDragDrop(Mobile from, Item dropped)
        {
            if(dropped is BaseBook)
			{
				object[] arg = new object[1];
				arg[0] = dropped;
				YesNo.SimpleConfirmMsg( new YesNoCallbackState( PublishConfirm ), from, "Publish This Book?", true, arg );
			}
            return base.OnDragDrop(from, dropped);
        }
        public override void OnDoubleClick(Mobile from)
        {
            from.SendGump(new BookPubGump(from));
        }
		public void PublishConfirm(Mobile from, bool yesNo, object[] arg)
		{
			if(!yesNo)
				return;
			BaseBook book = arg[0] as BaseBook;
			if(book == null)
				return;
			
			BookContent bc = new BookContent(book.Title, book.Author, book.Pages);
			
			if(m_Books == null)
				m_Books = new List<BookContent>();
			
			if(!m_Books.Contains(bc))
				m_Books.Add(bc);
		}
		
		public Publisher( Serial serial ) : base( serial )
		{}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}