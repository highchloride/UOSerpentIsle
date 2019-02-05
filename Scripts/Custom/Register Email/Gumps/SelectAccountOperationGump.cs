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
    public class AccountPropertiesGump : Gump
    {
        public AccountPropertiesGump()
            : base(0, 0)
        {
            Closable = true;
            Dragable = true;
            Resizable = false;

            AddBackground(212, 221, 246, 120, 9300);
            AddLabel(288, 230, 0, "Your Account");

            AddLabel(256, 259, 0, "Change Email");
            AddButton(222, 258, 4005, 4006, 1, GumpButtonType.Reply, 0);

            AddLabel(255, 291, 0, "Change Password");
            AddButton(222, 289, 4005, 4006, 2, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            Account acct = (Account)from.Account;

            string key = (string)acct.Username;

            switch (info.ButtonID)
            {
                case 0:
                    {
                        from.SendMessage("Account operations cancelled.");
                        break;
                    }
                case 1:
                    {
                        if (!EmailHolder.Confirm.ContainsKey(key))
                        {
                            from.CloseGump(typeof(RegisterEmailGump));
                            from.SendGump(new RegisterEmailGump());
                        }
                        else
                        {
                            from.SendMessage("You are currently in the process of registering a new email address.");
                        }

                        break;
                    }
                case 2:
                    {
                        from.CloseGump(typeof(ChangePasswordGump));
                        from.SendGump(new ChangePasswordGump());

                        break;
                    }
            }
        }
    }
}
