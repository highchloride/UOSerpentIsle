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

namespace Server.Misc
{
    public class RegisterEmailClient
    {
        public static bool Enabled = false; // Is this system enabled?

        public static string ServerName = "SIOP"; // Your server name here.

        public static string EmailServer = "smtp.gmail.com"; // Your mail server here
        public static string User = "highchloride@gmail.com"; // Your username here
        public static string Pass = "fanta5!a1A"; // Your password here

        public static int ServerPort = 587; //SMTP Server Port

        public static bool UseSSL = true; //Is SSL encryption used?

        public static string YourAddress = "highchloride@gmail.com"; // Your email address here, Or Shard name
        // Server will crash on start up if the adress is incorrectly formatted.

        public static SmtpClient client;
        public static MailMessage mm;

        public static void Initialize()
        {
            if (Enabled)
            {
                client = new SmtpClient(EmailServer);
                client.Credentials = new NetworkCredential(User, Pass);
                client.Port = ServerPort;
                client.EnableSsl = UseSSL;

                mm = new MailMessage();
                mm.Subject = ServerName;
                mm.From = new MailAddress(YourAddress);
                
            }
        }

        public static void SendMail(EmailEventArgs e)
        {
            bool single = e.Single;

            if (single)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(SendSingal), e);
            }
            else
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(SendMultiple), e);
            }

            return;
        }

        private static void SendMultiple(object e)
        {
            EmailEventArgs eea = (EmailEventArgs)e;

            List<MailAddress> emails = (List<MailAddress>)eea.Emails;
            string sub = (string)eea.Subject;
            string msg = (string)eea.Message;

            for (int i = 0; i < emails.Count; ++i)
            {
                MailAddress ma = (MailAddress)emails[i];

                mm.To.Add(ma);
            }

            mm.Subject += " - " + sub;
            mm.Body = msg;

            try
            {
                client.Send(mm);
            }
            catch { }
            mm.To.Clear();
            mm.Body = "";
            mm.Subject = ServerName;

            return;
        }

        private static void SendSingal(object e)
        {
            EmailEventArgs eea = (EmailEventArgs)e;

            string to = (string)eea.To;
            string sub = (string)eea.Subject;
            string msg = (string)eea.Message;

            mm.To.Add(to);
            mm.Subject += " " + sub;
            mm.Body = msg;

            try
            {
                client.Send(mm);
            }
            catch { }
            mm.To.Clear();
            mm.Body = "";
            mm.Subject = ServerName;

            return;
        }
    }

    public class EmailEventArgs
    {
        public bool Single;
        public List<MailAddress> Emails;
        public string To;
        public string Subject;
        public string Message;

        public EmailEventArgs(bool single, List<MailAddress> list, string to, string sub, string msg)
        {
            Single = single;
            Emails = list;
            To = to;
            Subject = sub;
            Message = msg;
        }
    }
}
