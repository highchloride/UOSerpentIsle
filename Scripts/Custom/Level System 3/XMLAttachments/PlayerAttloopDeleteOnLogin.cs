using System;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Misc
{
    public class PlayerAttloopDeleteOnLogin
    {
        public static void Initialize()
        {
            EventSink.Login += new LoginEventHandler(EventSink_Login);
        }

        private static void EventSink_Login(LoginEventArgs args)
        {
            Mobile m = args.Mobile;
			
            if (args.Mobile.NetState == null)
            {
                return;
            }

			if (m is PlayerMobile)
			{
				PlayerMobile pm = (PlayerMobile)m;
				PlayerAttloop looping = (PlayerAttloop)XmlAttach.FindAttachment(m, typeof(PlayerAttloop));

				if (looping != null)
				{
					looping.Delete();
				}
			}			
        }
    }
}