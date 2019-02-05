using System;
using Server;
using Server.Commands;
using Server.Items;
using Server.Mobiles;
using System.Collections;
using System.Collections.Generic;
using Server.Accounting;
using Server.Network;
using Server.Misc;
using Server.Multis;
using Server.Targeting;
using Server.Gumps;
using System.Net.Mail;
using System.Threading;
using System.Net;

namespace Server.Gumps
{
	public class SendEmailGump : Gump
	{
		public SendEmailGump() : base(0, 0)
		{
			Closable = true;
			Dragable = true;
			Resizable = false;

			AddBackground( 158, 131, 509, 383, 9300 );
			AddLabel(373, 138, 0, "Send Email");

			AddLabel(167, 167, 0, "Subject:"); // Subject
            AddTextEntry(232, 165, 424, 25, 70, 2, "");
            AddAlphaRegion(232, 164, 424, 25);

			AddLabel(168, 198, 0, "Message Part 1:"); //Message Part 1
            AddTextEntry(171, 226, 482, 98, 70, 3, "");
            AddAlphaRegion(171, 225, 483, 100);

			AddLabel(166, 332, 0, "Message Part 2:"); // Message Part 2
            AddTextEntry(174, 359, 473, 99, 70, 4, "");
            AddAlphaRegion(171, 358, 483, 100);

			AddLabel(210, 477, 0, "Send Email"); // Send
            AddButton(176, 476, 4005, 4006, 1, GumpButtonType.Reply, 0);
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			Mobile from = sender.Mobile;

            switch (info.ButtonID)
            {
                case 0:
                    {
                        from.SendMessage("You decide not the send the email.");
                        break;
                    }
                case 1:
                    {
                        TextRelay txt1 = (TextRelay)info.GetTextEntry(2);
                        string subject = txt1.Text.Trim();

                        TextRelay txt2 = (TextRelay)info.GetTextEntry(3);
                        string msg1 = txt2.Text.Trim();

                        TextRelay txt3 = (TextRelay)info.GetTextEntry(4);
                        string msg2 = txt3.Text.Trim();

                        List<MailAddress> emails = new List<MailAddress>();

                        IEnumerator key = EmailHolder.Emails.Keys.GetEnumerator();

                        for(int i = 0; i < EmailHolder.Emails.Count; ++i)
                        {
                            key.MoveNext();

                            string k = (string)key.Current;

                            MailAddress ma = new MailAddress(EmailHolder.Emails[k]);

                            if(!emails.Contains(ma))
                                emails.Add(ma);
                        }

                        EmailEventArgs eea = new EmailEventArgs(false, emails, "", subject, (msg1 + msg2));

                        RegisterEmailClient.SendMail(eea);

                        break;
                    }
            }
		}
	}
}
