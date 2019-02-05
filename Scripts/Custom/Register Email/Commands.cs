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

public class EmailCommands
{
    public static void Initialize()
    {
        EventSink.Login += new LoginEventHandler(On_Login);
        EventSink.Logout += new LogoutEventHandler(On_Logout);
        //Player Commands
        CommandSystem.Register("auth", AccessLevel.Player, new CommandEventHandler(On_Auth));
        CommandSystem.Register("account", AccessLevel.Player, new CommandEventHandler(On_Account));
        CommandSystem.Register("email", AccessLevel.Player, new CommandEventHandler(On_Email));

        //Admin Commands
        CommandSystem.Register("sendemail", AccessLevel.Seer, new CommandEventHandler(Send_Email));
        CommandSystem.Register("clearemails", AccessLevel.Seer, new CommandEventHandler(Clear_Emails));
    }

    public static void On_Email(CommandEventArgs e)
    {
        Mobile m = (Mobile)e.Mobile;

        Account acct = (Account)m.Account;
        string key = acct.Username;

        if (EmailHolder.Emails.ContainsKey(key))
        {
            m.CloseGump(typeof(FindPlayersGump));
            m.SendGump(new FindPlayersGump());
        }
        else
        {
            m.SendMessage("You must register a email address to gain the ability to send others emails.");
            return;
        }
    }

    public static void Clear_Emails(CommandEventArgs e)
    {
        if (!RegisterEmailClient.Enabled)
            return;

        if (EmailHolder.Emails == null)
            EmailHolder.Emails = new Dictionary<string, string>();
        if (EmailHolder.Confirm == null)
            EmailHolder.Confirm = new Dictionary<string, string>();
        if (EmailHolder.Codes == null)
            EmailHolder.Codes = new Dictionary<string, string>();

        Mobile from = (Mobile)e.Mobile;

        EmailHolder.Emails.Clear();
        EmailHolder.Confirm.Clear();
        EmailHolder.Codes.Clear();

        from.SendMessage("You have cleared all of your shards registered email addresses.");
    }

    public static void On_Account(CommandEventArgs e)
    {
        if (!RegisterEmailClient.Enabled)
            return;

        if (EmailHolder.Emails == null)
            EmailHolder.Emails = new Dictionary<string, string>();
        if (EmailHolder.Confirm == null)
            EmailHolder.Confirm = new Dictionary<string, string>();
        if (EmailHolder.Codes == null)
            EmailHolder.Codes = new Dictionary<string, string>();

        Mobile from = (Mobile)e.Mobile;

        Account acct = (Account)from.Account;

        string test = (string)acct.Username;

        if (!EmailHolder.Emails.ContainsKey(test))
        {
            from.SendMessage("You must first register your account to a email before you can access your account operations menu.");
            return;
        }
        else
        {
            from.CloseGump(typeof(AccountPropertiesGump));
            from.SendGump(new AccountPropertiesGump());
        }
    }

    public static void Send_Email(CommandEventArgs e)
    {
        if (!RegisterEmailClient.Enabled)
            return;

        if (EmailHolder.Emails == null)
            EmailHolder.Emails = new Dictionary<string, string>();
        if (EmailHolder.Confirm == null)
            EmailHolder.Confirm = new Dictionary<string, string>();
        if (EmailHolder.Codes == null)
            EmailHolder.Codes = new Dictionary<string, string>();

        Mobile from = (Mobile)e.Mobile;

        from.CloseGump(typeof(SendEmailGump));
        from.SendGump(new SendEmailGump());
    }

    public static void On_Auth(CommandEventArgs e)
    {
        if (!RegisterEmailClient.Enabled)
            return;

        if (EmailHolder.Emails == null)
            EmailHolder.Emails = new Dictionary<string, string>();
        if (EmailHolder.Confirm == null)
            EmailHolder.Confirm = new Dictionary<string, string>();
        if (EmailHolder.Codes == null)
            EmailHolder.Codes = new Dictionary<string, string>();

        Mobile from = (Mobile)e.Mobile;

        from.CloseGump(typeof(AuthGump));
        from.SendGump(new AuthGump());
    }

    public static void On_Logout(LogoutEventArgs e)
    {
        Mobile from = (Mobile)e.Mobile;

        if (!RegisterEmailClient.Enabled)
            return;

        if (EmailHolder.Emails == null)
            EmailHolder.Emails = new Dictionary<string, string>();
        if (EmailHolder.Confirm == null)
            EmailHolder.Confirm = new Dictionary<string, string>();
        if (EmailHolder.Codes == null)
            EmailHolder.Codes = new Dictionary<string, string>();

        Account acct = (Account)from.Account;

        string test = (string)acct.Username;

        if (EmailHolder.Confirm.ContainsKey(test))
        {
            EmailHolder.Confirm.Remove(test);
            EmailHolder.Codes.Remove(test);
        }
    }

    public static void On_Login(LoginEventArgs e)
    {
        Mobile from = (Mobile)e.Mobile;

        if (!RegisterEmailClient.Enabled)
            return;

        if (EmailHolder.Emails == null)
            EmailHolder.Emails = new Dictionary<string, string>();
        if (EmailHolder.Confirm == null)
            EmailHolder.Confirm = new Dictionary<string, string>();
        if (EmailHolder.Codes == null)
            EmailHolder.Codes = new Dictionary<string, string>();

        Account acct = (Account)from.Account;

        string test = (string)acct.Username;

        if (!EmailHolder.Emails.ContainsKey(test))
        {
            from.SendGump(new RegisterEmailGump());
        }
        else
        {
            string msg = "This account is registered to the email " + (string)EmailHolder.Emails[test];

            from.SendMessage(msg);
        }
    }
}