using System;
using Server;
using Server.Mobiles;
using Engale.BookPublishing;
using Server.SkillHandlers;

namespace Server.Items
{
	public class PublishedBook : BaseBook
	{
		[Constructable]
		public PublishedBook() : this(Utility.Random(Publisher.Books.Count))
		{}
		
		[Constructable]
		public PublishedBook(int index) : base(Utility.RandomList(0xFF2,0xFEF,0xFF1,0xFF0))
		{
			Writable = false;
			if(index >= Publisher.Books.Count)
				return;
			
			BookContent bc = Publisher.Books[index];
			
			Title = bc.Title;
			Author = bc.Author;

			BookPageInfo[] pagesSrc = bc.Pages;
			for ( int i = 0; i < pagesSrc.Length && i < Pages.Length; i++ )
			{
				BookPageInfo pageSrc = pagesSrc[i];
				BookPageInfo pageDst = Pages[i];

				int length = pageSrc.Lines.Length;
				pageDst.Lines = new string[length];

				for ( int j = 0; j < length; j++ )
					pageDst.Lines[j] = pageSrc.Lines[j];
			}
		}
		
		public PublishedBook( Serial serial ) : base( serial )
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