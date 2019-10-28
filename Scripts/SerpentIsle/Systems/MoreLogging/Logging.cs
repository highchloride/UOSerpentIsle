using System;
using System.IO;
using Server;
using Server.Accounting;
using Server.Items;
using Server.Mobiles;
using System.Collections;
using System.Collections.Generic;

namespace Server.SerpentIsle.Systems.MoreLogging
{
    public class MoreLogging
    {
        private static StreamWriter m_Output;

        public static StreamWriter Output
        {
            get { return m_Output; }
        }

        public static void Initialize()
        {
            if (!Directory.Exists("Logs"))
                Directory.CreateDirectory("Logs");

            string directory = "Logs/MoreLogging";

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            try
            {
                m_Output = new StreamWriter(Path.Combine(directory, String.Format("{0}.log", DateTime.Now.ToLongDateString())), true);

                m_Output.AutoFlush = true;

                m_Output.WriteLine("##############################");
                m_Output.WriteLine("Log started on {0}", DateTime.Now);
                m_Output.WriteLine();
            }
            catch
            {
            }
            EventSink.PlayerDeath += new PlayerDeathEventHandler(LogDeath);
            Mobile.CreateCorpseHandler += LogCorpseCreated;
        }

        //Event Logging
        public static void LogDeath(PlayerDeathEventArgs e)
        {
            //UOSI Logging - Records the time of the player's death.
            WriteLine(e.Mobile, "Death has been recorded! Killed by: " + e.Killer.ToString());
        }

        public static Container LogCorpseCreated(Mobile owner, HairInfo hair, FacialHairInfo facialhair,
            List<Item> initialContent, List<Item> equipItems)
        {
            WriteLine(owner, "A corpse has been created!");

            return null;
        }

        public static object Format(object o)
        {
            if (o is Mobile)
            {
                Mobile m = (Mobile)o;

                if (m.Account == null)
                    return String.Format("{0} (no account)", m);
                else
                    return String.Format("{0} ('{1}')", m, ((Account)m.Account).Username);
            }
            else if (o is Item)
            {
                Item item = (Item)o;

                return String.Format("0x{0:X} ({1})", item.Serial.Value, item.GetType().Name);
            }

            return o;
        }

        public static void WriteLine(Mobile from, string format, params object[] args)
        {
            WriteLine(from, String.Format(format, args));
        }

        public static void WriteLine(Mobile from, string text)
        {
            try
            {
                m_Output.WriteLine("{0}: {1}: {2}", DateTime.Now.ToShortTimeString(), MoreLogging.Format(from), text);

                string path = Core.BaseDirectory;
                AppendPath(ref path, "Logs");
                AppendPath(ref path, "MoreLogging");
                path = Path.Combine(path, String.Format("{0}.log", DateTime.Now.ToLongDateString()));

                using (StreamWriter sw = new StreamWriter(path, true))
                    sw.WriteLine("{0}: {1}: {2}", DateTime.Now, MoreLogging.Format(from), text);
            }
            catch
            {
            }
        }

        public static void AppendPath(ref string path, string toAppend)
        {
            path = Path.Combine(path, toAppend);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
    }

    
}
