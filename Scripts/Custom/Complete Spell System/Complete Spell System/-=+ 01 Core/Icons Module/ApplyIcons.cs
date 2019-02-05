using System;
using System.Collections;
using System.Collections.Generic;
using Server.Gumps;
using Server.Network;
using Server.Spells;
using Server.ACC.CM;

namespace Server.ACC.CSS.Modules
{
    public class ApplyIcons
    {
        public static void Initialize()
        {
            EventSink.Login += new LoginEventHandler(EventSink_Login);
        }

        private static void EventSink_Login(LoginEventArgs args)
        {
            if (!CentralMemory.Running || !CSS.Running)
                return;

            Mobile m = args.Mobile;

            IconsModule im = (IconsModule)CentralMemory.GetModule(m.Serial, typeof(IconsModule));
            if (im == null)
                return;

            IconsModule imRemove = new IconsModule(m.Serial);

            foreach (KeyValuePair<Type, IconInfo> kvp in im.Icons)
            {
                IconInfo ii = kvp.Value;
                if (ii == null)
                {
                    imRemove.Add(ii);
                    continue;
                }

                if (ii.SpellType == null || ii.School == School.Invalid)
                {
                    imRemove.Add(ii);
                    continue;
                }

                if (SpellRestrictions.UseRestrictions && !SpellRestrictions.CheckRestrictions(m, ii.School))
                {
                    imRemove.Add(ii);
                    continue;
                }

                m.SendGump(new SpellIconGump(ii));
            }

            if (im.Icons.Count > 0)
                CentralMemory.AppendModule(m.Serial, (Module)imRemove, true);
        }
    }
}