using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Commands;
using System.Collections;
using System.Net.Mail;
using Server.Accounting;
using Server.Misc;

namespace Server.Gumps
{
	public class SendSingleEmailGump : Gump
	{
        private Mobile Targ;

		public SendSingleEmailGump(Mobile target) : base(0, 0)
		{
            Targ = target;

			Closable = true;
			Dragable = false;
			Resizable = false;

			AddBackground( 141, 90, 425, 235, 9250 );
			AddLabel(278, 106, 0, "Send Email Message");

			AddLabel(156, 132, 0, "Subject:");
            AddAlphaRegion(220, 128, 212, 25);
            AddTextEntry(220, 128, 212, 25, 0, 2, "");

			AddLabel(159, 162, 0, "Message:");
			AddAlphaRegion( 156, 183, 396, 96 );
			AddTextEntry(156, 183, 396, 96, 0, 3, "");

			AddButton( 159, 285, 4005, 4006, 1, GumpButtonType.Reply, 0);
			AddLabel(192, 287, 0, "Send Email");
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			Mobile from = sender.Mobile;

            TextRelay txt_subject = (TextRelay)info.GetTextEntry(2);
            string subject = txt_subject.Text;

            TextRelay txt_message = (TextRelay)info.GetTextEntry(3);
            string message = txt_message.Text;

            if (info.ButtonID == 1)
            {
                Account acct = (Account)Targ.Account;
                string key = acct.Username;

                string to = (string)EmailHolder.Emails[key];

                message += "\n\n This email was sent by " + from.Name + " at the time of " + DateTime.Now.ToString() + " using the email system on the " + RegisterEmailClient.ServerName + " shard.";

                EmailEventArgs eea = new EmailEventArgs(true, null, to, subject, message);

                RegisterEmailClient.SendMail(eea);

                from.SendMessage("The email is being sent.");
            }
            else
            {
                from.SendMessage("You decide not to send the email.");
            }
		}
	}
}
