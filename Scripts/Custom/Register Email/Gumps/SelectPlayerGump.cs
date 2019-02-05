using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Commands;
using System.Collections;
using Server.Gumps;
using Server.Accounting;

namespace Server.Gumps
{
	public class SelectPlayerGump : Gump
	{
        private ArrayList List;

		public SelectPlayerGump(ArrayList list) : base(0, 0)
		{
            List = list;

			Closable = true;
			Dragable = true;
			Resizable = false;

            int bgh = 0;

            for (int i = 0; i < list.Count; ++i)
            {
                bgh += 32;
            }

			AddBackground( 189, 207, 250, bgh, 9250 );

            int by = 220;
            int bi = 1;

            int ly = 220;

            for (int i = 0; i < list.Count; ++i)
            {
                Mobile m = (Mobile)list[i];

                AddButton(205, by, 1153, 1154, bi, GumpButtonType.Reply, 0);
                by += 25;
                bi += 1;
                AddLabel(235, ly, 999, m.Name);
                ly += 25;
            }
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			Mobile from = sender.Mobile;

            if (info.ButtonID != 0)
            {
                int i = info.ButtonID;
                i -= 1;
                Mobile m = (Mobile)List[i];

                Account acct = (Account)m.Account;
                string key = (string)acct.Username;

                if (!EmailHolder.Emails.ContainsKey(key))
                {
                    from.SendMessage("The player has not registered a email address.");
                    return;
                }

                from.CloseGump(typeof(SendSingleEmailGump));
                from.SendGump(new SendSingleEmailGump(m));
            }
		}
	}
}
