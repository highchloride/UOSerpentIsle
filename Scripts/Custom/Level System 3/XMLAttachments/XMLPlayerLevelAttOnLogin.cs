using System;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Misc
{
    public class XMLPlayerLevelAttOnLogin
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
				Configured c = new Configured();
				XMLPlayerLevelAtt xmlplayer = (XMLPlayerLevelAtt)XmlAttach.FindAttachment(m, typeof(XMLPlayerLevelAtt));
				if (xmlplayer != null)
				{
					return;
				}
				else
				{
					if (m is PlayerMobile && c.AttachonLogon)
					{
						XmlAttach.AttachTo(m, new XMLPlayerLevelAtt());
					}
					else
						return;
				}
			}			
        }
    }
}