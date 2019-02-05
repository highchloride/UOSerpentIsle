using System;
using Server;
using Server.Items;
using Server.Mobiles;
using System.Collections;
using Server.Accounting;
using System.Collections.Generic;
using System.IO;
using System.Xml;

public class EmailHolder
{
    public static Dictionary<string, string> Emails;
    public static Dictionary<string, string> Confirm;
    public static Dictionary<string, string> Codes;

    public static void Initialize()
    {
        if (Emails == null)
            Emails = new Dictionary<string, string>();
        if (Confirm == null)
            Confirm = new Dictionary<string, string>();
        if (Codes == null)
            Codes = new Dictionary<string, string>();

        On_Load();

        EventSink.Crashed += new CrashedEventHandler(On_Crash);
        EventSink.Shutdown += new ShutdownEventHandler(On_Shut);
        EventSink.WorldSave += new WorldSaveEventHandler(On_Save);
    }

    public static void On_Crash(CrashedEventArgs e)
    {
        StreamWriter writer = new StreamWriter("Emails.xml");
        writer.Write(XmlString());
        writer.Close();
        writer.Dispose();
    }

    public static void On_Shut(ShutdownEventArgs e)
    {
        StreamWriter writer = new StreamWriter("Emails.xml");
        writer.Write(XmlString());
        writer.Close();
        writer.Dispose();
    }

    public static void On_Load()
    {
        try
        {
            XmlDocument doc = new XmlDocument();
            XmlNode node;
            XmlNodeReader reader;
            doc.Load("Emails.xml");

            int count = (int)LoadCount();

            ArrayList keys = new ArrayList();
            for (int i = 0; i < count; ++i)
            {
                node = doc.DocumentElement.SelectSingleNode("Account" + i.ToString());
                reader = new XmlNodeReader(node);

                while (reader.Read())
                {
                    switch (reader.Name)
                    {
                        case "Name":
                            {
                                string s = (string)reader.ReadString();
                                keys.Add(s);
                                break;
                            }
                    }
                }
            }

            ArrayList vals = new ArrayList();
            for (int i = 0; i < count; ++i)
            {
                node = doc.DocumentElement.SelectSingleNode("Email" + i.ToString());
                reader = new XmlNodeReader(node);

                while (reader.Read())
                {
                    switch (reader.Name)
                    {
                        case "E":
                            {
                                string s = (string)reader.ReadString();
                                vals.Add(s);
                                break;
                            }
                    }
                }
            }

            for (int i = 0; i < count; ++i)
            {
                string key = (string)keys[i];
                string val = (string)vals[i];

                Emails.Add(key, val);
            }
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine("Loading of emails.xml has failed!!!");
        }
    }

    public static void On_Save(WorldSaveEventArgs e)
    {
        StreamWriter writer = new StreamWriter("Emails.xml");
        writer.Write(XmlString());
        writer.Close();
        writer.Dispose();
    }

    public static int LoadCount()
    {
        try
        {
            XmlDocument doc = new XmlDocument();
            XmlNode node;
            XmlNodeReader reader;
            doc.Load("Emails.xml");

            node = doc.DocumentElement.SelectSingleNode("Count");
            reader = new XmlNodeReader(node);

            while (reader.Read())
            {
                switch (reader.Name)
                {
                    case "I":
                        {
                            string s = (string)reader.ReadString();
                            return (int)Convert.ToInt64(s);
                            break;
                        }
                }
            }
        }
        catch
        {
            Console.WriteLine("Loading of emails.xml has failed!!!");
        }

        return 0;
    }

    public static string XmlString()
    {
        string local = "";
        local += "<Emails>";

        IEnumerator key = Emails.Keys.GetEnumerator();

        for (int i = 0; i < Emails.Count; ++i)
        {
            key.MoveNext();

            string s = (string)key.Current;
            local += "  <Account" + i.ToString() + ">";
            local += "    <Name>" + s + "</Name>";
            local += "  </Account" + i.ToString() + ">"; 
        }

        IEnumerator val = Emails.Values.GetEnumerator();

        for (int i = 0; i < Emails.Count; ++i)
        {
            val.MoveNext();

            string s = (string)val.Current;
            local += "  <Email" + i.ToString() + ">";
            local += "    <E>" + s + "</E>";
            local += "  </Email" + i.ToString() + ">";
        }

        local += "  <Count>";
        local += "    <I>" + Emails.Count.ToString() +"</I>";
        local += "  </Count>";

        local += "</Emails>";
        return local;
    }
}