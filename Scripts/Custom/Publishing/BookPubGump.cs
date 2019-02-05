using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Gumps;
using Server.Network;
using Server.Commands;
using Engale.BookPublishing;

namespace Server.Gumps
{
    public class BookPubGump : Gump
    {
        private List<BookContent> bc;

        private int m_Page;

        public BookPubGump(Mobile from) : this(from, 0) { }

        public BookPubGump(Mobile from, int page) : base( 0, 0 )
        {

            this.Closable=true;
            this.Disposable=true;
            this.Dragable=true;
            this.Resizable=false;

            AddPage(0);

            bc = Publisher.Books;
            
            m_Page = page;

            AddBackground(0, 1, 500, 550, 9200);
            AddLabel(190, 5, 32, "Book Publisher");
            if(page > 0)
                AddButton(10, 515, 4014, 4016, 1, GumpButtonType.Reply, 0);

            if((page + 1) * 10 < bc.Count)
                AddButton(460, 515, 4005, 4007, 2, GumpButtonType.Reply, 0);

            for (int i = page * 10; i < ((page + 1) * 10 < bc.Count ? (page + 1) * 10 : bc.Count); i++)
                AddBookDetail(i , from.AccessLevel > AccessLevel.Player);
        }

        private void AddBookDetail(int i, bool admin)
        {
            BookContent b = bc[i];

            if (i % 2 != 0)
                AddImageTiled(0, 25 + (45 * (i % 10)), 500, 45, 9274);

            AddItem(5, 35 + (45 * (i % 10)), 4079);

            AddLabel(45, 35 + (45 * (i % 10)), 0, b.Title);
            AddLabel(235, 35 + (45 * (i % 10)), 0, "By: " + b.Author);

            AddButton(admin ? 410 : 460, 35 + (45 * (i % 10)), 4011, 4013, (i * 2) + 3, GumpButtonType.Reply, 0);

            if(admin)
                AddButton(460, 35 + (45 * (i % 10)), 4020, 4022, (i * 2) + 4, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            switch(info.ButtonID)
            {
                case 0:
                {
                    break;
                }
                case 1:
                {
                    from.SendGump(new BookPubGump(from, m_Page - 1));
                    break;
                }
                case 2:
                {
                    from.SendGump(new BookPubGump(from, m_Page + 1));
                    break;
                }
                default:
                {
                    int i = info.ButtonID - 3;
                    switch (i % 2)
                    {
                        case 0: {
                            int cost = 300 - from.Backpack.ConsumeUpTo(typeof(Gold), 300);

                            if (cost > 0)
                            {
                                from.SendMessage("You can not afford this.");

                                if(300 - cost > 0)
                                    from.AddToBackpack(new Gold(300 - cost));

                                from.SendGump(new BookPubGump(from, m_Page));
                                return;
                            }

                            PublishedBook pb = new PublishedBook(i / 2);

                            if (pb != null)
                                from.AddToBackpack(pb);
                            break;
                        }
                        case 1: {
                            int x = i / 2;
                            if (x > -1 && x < bc.Count)
                                bc.RemoveAt(x);
                            break;
                        }
                    }
                    from.SendGump(new BookPubGump(from, m_Page));
                    break;
                }
            }
        }
    }
}