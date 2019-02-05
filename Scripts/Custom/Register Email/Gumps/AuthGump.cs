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
	public class AuthGump : Gump
	{
        public AuthGump()
            : base(0, 0)
		{
			Closable = true;
			Dragable = true;
			Resizable = false;

			AddBackground( 145, 150, 365, 223, 9300 );
			AddLabel(220, 157, 0, "Confirm Email Registration");

			AddHtml(155, 184, 347, 119, "Enter the 10 character long registration code that was sent to your email after you submited it. However if you have logged out since submitting your email, your 10 character registration code has expired and you will need to submit your request again.", true, false);

			AddLabel(158, 310, 0, "Enter Here:");
			AddAlphaRegion( 247, 308, 255, 25 );
            AddTextEntry(247, 308, 255, 25, 40, 2, "");

			AddButton( 223, 340, 4005, 4006, 1, GumpButtonType.Reply, 0);
			AddLabel(258, 342, 0, "Submit");
		}

        public bool CanValidate(string s, string key, Mobile from)
        {
            if (EmailHolder.Codes == null)
                EmailHolder.Codes = new Dictionary<string, string>();
            if (EmailHolder.Confirm == null)
                EmailHolder.Confirm = new Dictionary<string, string>();

            if (s == "")
                return false;
            if (s.Length != 10)
                return false;
            if (!EmailHolder.Codes.ContainsKey(key) || !EmailHolder.Codes.ContainsValue(s))
                return false;
            if (!EmailHolder.Confirm.ContainsKey(key))
                return false;

            return true;
        }

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			Mobile from = sender.Mobile;

            switch (info.ButtonID)
            {
                case 0:
                    {
                        from.SendMessage("Please be sure to register your email address at some point in time.");
                        break;
                    }
                case 1:
                    {
                        TextRelay txt = (TextRelay)info.GetTextEntry(2);
                        string s = (string)txt.Text.Trim();

                        Account acct = (Account)from.Account;

                        string key = (string)acct.Username;

                        if (CanValidate(s, key, from))
                        {
                            string email = (string)EmailHolder.Confirm[key];

                            if (!EmailHolder.Emails.ContainsKey(key))
                            {
                                EmailHolder.Emails.Add(key, email);
                            }
                            else
                            {
                                EmailHolder.Emails.Remove(key);
                                EmailHolder.Emails.Add(key, email);
                            }

                            if (EmailHolder.Confirm.ContainsKey(key))
                            {
                                EmailHolder.Confirm.Remove(key);
                                EmailHolder.Codes.Remove(key);
                            }

                            string msg = "Congragulation on sucessfully registering your email address, You will now be able to recieve special announcment s via email and use the in game commands \"[account\" to manage your password and email address and \"[email\" to send emails to other players. \n \n" + "Thank You";

                            EmailEventArgs eea = new EmailEventArgs(true, null, email, "Email Registered", msg);
                            
                            RegisterEmailClient.SendMail(eea);
                            from.SendMessage("Thank you for registering your email.");
                        }
                        else
                        {
                            from.SendMessage("The code you entered is either not correct, or no longer valid.");
                        }

                        break;
                    }
            }
		}
	}
}
