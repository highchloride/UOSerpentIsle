using System;
using System.Collections;
using System.IO;
using Server.Commands;
using Server.Gumps;
using Server.Network;
using Server.Spells;
using Server.ACC.CSS.Modules;
using Server.ACC.CM;

namespace Server.ACC.CSS
{
    public class CSS : ACCSystem
    {
        private static int Version = 100;
        private static int PrevVersion;

        public override string Name() { return "Complete Spell System"; }

        private static Hashtable m_Loaded = new Hashtable();

        public static void Configure()
        {
            ACC.RegisterSystem("Server.ACC.CSS.CSS");
        }

        public static void Initialize()
        {
            CommandSystem.Register("CSSCast", AccessLevel.Player, new CommandEventHandler(CSSCast_OnCommand));
        }

        [Usage("CSSCast <text>")]
        [Description("Casts the spell assigned to the given text.")]
        private static void CSSCast_OnCommand(CommandEventArgs e)
        {
            if (e.Length == 1)
            {
                if (!CentralMemory.Running || !CSS.Running)
                {
                    e.Mobile.SendMessage("The Central Memory is not running.  This command is disabled.");
                    return;
                }

                if (!Multis.DesignContext.Check(e.Mobile))
                {
                    e.Mobile.SendMessage("You cannot cast while customizing!");
                    return;
                }

                CastCommandsModule module = (CastCommandsModule)CentralMemory.GetModule(e.Mobile.Serial, typeof(CastCommandsModule));
                if (module == null)
                {
                    e.Mobile.SendMessage("You do not have any commands to cast stored.");
                    return;
                }

                CastInfo info = module.Get(e.GetString(0));
                if (info == null)
                {
                    e.Mobile.SendMessage("You have not assigned that command to any spells.");
                    return;
                }

                if (SpellRestrictions.UseRestrictions && !SpellRestrictions.CheckRestrictions(e.Mobile, info.SpellType))
                {
                    e.Mobile.SendMessage("You are not allowed to cast this spell.");
                    return;
                }

                if (!CSpellbook.MobileHasSpell(e.Mobile, info.School, info.SpellType))
                {
                    e.Mobile.SendMessage("You do not have this spell.");
                    return;
                }

                Spell spell = SpellInfoRegistry.NewSpell(info.SpellType, info.School, e.Mobile, null);
                if (spell != null)
                    spell.Cast();
                else
                    e.Mobile.SendMessage("This spell has been disabled.");
            }
            else
            {
                e.Mobile.SendMessage("Format: Cast <text>");
            }
        }

        public static bool Running
        {
            get { return ACC.SysEnabled("Server.ACC.CSS.CSS"); }
        }

        public override void Enable()
        {
            Console.WriteLine("{0} has just been enabled!", Name());
        }

        public override void Disable()
        {
            Console.WriteLine("{0} has just been disabled!", Name());
        }

        public override void Gump(Mobile from, Gump gump, ACCGumpParams subParams)
        {
        }

        public override void Help(Mobile from, Gump gump)
        {
        }

        public override void OnResponse(NetState state, RelayInfo info, ACCGumpParams subParams)
        {
        }

        private static void Refresh()
        {
            if (m_Loaded == null || m_Loaded.Count == 0)
                return;

            Console.WriteLine("\n    - Checking Spell Registry:");

            ArrayList books;
            ArrayList oldsr;
            ArrayList newsr;
            bool changed;

            foreach (DictionaryEntry de in m_Loaded)
            {
                changed = false;

                oldsr = (ArrayList)de.Value;

                newsr = SpellInfoRegistry.GetSpellsForSchool((School)de.Key);
                if (newsr == null)
                    continue;

                if (oldsr.Count != newsr.Count)
                    changed = true;

                for (int i = 0; i < oldsr.Count && !changed; i++)
                {
                    if (((CSpellInfo)oldsr[i]).Type != (Type)newsr[i])
                        changed = true;
                }

                for (int i = 0; i < oldsr.Count; i++)
                {
                    if (!((CSpellInfo)oldsr[i]).Enabled)
                        SpellInfoRegistry.DisEnable((School)de.Key, ((CSpellInfo)oldsr[i]).Type, false);
                }

                if (!changed)
                    continue;

                Console.WriteLine("      - Changes in {0} detected - refreshing all books.", (School)de.Key);

                books = new ArrayList();
                foreach (Item i in World.Items.Values)
                {
                    if ((i is CSpellbook))
                    {
                        CSpellbook book = i as CSpellbook;
                        if (book == null || book.Deleted || book.School != (School)de.Key)
                            continue;
                        books.Add(book);
                    }
                }

                for (int i = 0; i < books.Count; i++)
                {
                    if (((CSpellbook)books[i]).Full)
                    {
                        ((CSpellbook)books[i]).Fill();
                        continue;
                    }

                    ulong oldcontent = ((CSpellbook)books[i]).Content;
                    ((CSpellbook)books[i]).Content = (ulong)0;

                    for (int j = 0; j < oldsr.Count; j++)
                    {
                        if ((oldcontent & ((ulong)1 << j)) != 0)
                            ((CSpellbook)books[i]).AddSpell(((CSpellInfo)oldsr[j]).Type);
                    }

                    ((CSpellbook)books[i]).InvalidateProperties();
                }
            }
        }

        public static void Update()
        {
            if (Version == 100)
                Console.WriteLine("    - Base Install");

            else if (PrevVersion < Version)
            {
            }
        }

        public override void Save(GenericWriter idx, GenericWriter tdb, GenericWriter writer)
        {
            writer.Write((int)1); // version

            writer.Write(SpellInfoRegistry.Registry.Count);
            foreach (DictionaryEntry de in SpellInfoRegistry.Registry)
            {
                writer.Write((int)de.Key);
                writer.Write((int)((ArrayList)de.Value).Count);
                foreach (CSpellInfo info in (ArrayList)de.Value)
                {
                    info.Serialize(writer);
                }
            }

            writer.Write((int)Version);
        }

        public override void Load(BinaryReader idx, BinaryReader tdb, BinaryFileReader reader)
        {
            int version = reader.ReadInt();

            m_Loaded = new Hashtable();

            switch (version)
            {
                case 1:
                    {
                        int keys = reader.ReadInt();
                        for (int i = 0; i < keys; i++)
                        {
                            School school = (School)reader.ReadInt();
                            ArrayList valuelist = new ArrayList();

                            int values = reader.ReadInt();
                            for (int j = 0; j < values; j++)
                            {
                                valuelist.Add(new CSpellInfo(reader));
                            }

                            m_Loaded.Add(school, valuelist);
                        }

                        PrevVersion = reader.ReadInt();

                        Refresh();
                        Update();
                        break;
                    }
                case 0:
                    {
                        int keys = reader.ReadInt();
                        for (int i = 0; i < keys; i++)
                        {
                            School school = (School)reader.ReadInt();
                            ArrayList valuelist = new ArrayList();

                            int values = reader.ReadInt();
                            for (int j = 0; j < values; j++)
                            {
                                valuelist.Add(new CSpellInfo(reader));
                            }

                            m_Loaded.Add(school, valuelist);
                        }

                        reader.ReadBool();
                        PrevVersion = reader.ReadInt();

                        Refresh();
                        Update();
                        break;
                    }
            }
        }
    }
}