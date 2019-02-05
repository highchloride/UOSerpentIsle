//Maintenance Mode
//By Tresdni (aka DxMonkey)
//Last Update:  07/26/2015

using System;
using System.Collections.Generic;
using System.Linq;
using Server.Commands;
using Server.Gumps;
using Server.Network;

namespace Server.Maintenance
{
    public class MaintenanceMode
    {
        private static bool m_Enabled;

        public static void Initialize()
        {
            EventSink.Login += EventSink_Login;
            CommandSystem.Register("maintenancemode", AccessLevel.Administrator, new CommandEventHandler(MaintenanceMode_OnCommand));
        }

        private static void EventSink_Login(LoginEventArgs args)
        {
            Mobile m = args.Mobile;

            if (!m_Enabled || m.AccessLevel >= AccessLevel.Counselor)
            {
                return;
            }

            Timer.DelayCall(TimeSpan.FromSeconds(2.0), new TimerStateCallback(Disconnect), m);
        }

        private static void MaintenanceMode_OnCommand(CommandEventArgs e)
        {
            if (m_Enabled)
            {
                m_Enabled = false;
                e.Mobile.SendMessage("Maintenance mode disabled.  Players may now log in.");
            }
            else
            {
                m_Enabled = true;
                e.Mobile.SendMessage("Maintenance mode enabled.  Players can no longer log in during this time.");
                List<NetState> states = NetState.Instances;

                foreach (NetState t in states.Where(t => t != null && t.Running && t.Mobile.AccessLevel < AccessLevel.Counselor))
                {
                    t.Mobile.SendGump(new MaintenanceModeGump());
                    t.Dispose(true);
                }
            }
        }

        private static void Disconnect(object state)
        {
            Mobile m = (Mobile) state;

            if (m.NetState == null || !m.NetState.Running)
            {
                return;
            }

            m.SendGump(new MaintenanceModeGump());
            m.NetState.Dispose(true);
        }
    }
}