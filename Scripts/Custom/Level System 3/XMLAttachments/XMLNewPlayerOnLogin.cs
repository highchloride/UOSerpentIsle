using System;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Misc
{
    public class XMLNewPlayerOnLogin
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
				XMLNewPlayer xmlnewplayers = (XMLNewPlayer)XmlAttach.FindAttachment(m, typeof(XMLNewPlayer));

				if (xmlnewplayers != null)
				{
					if (c.LowLevelBonus == false)
					{
						XMLNewPlayer xmldel = (XMLNewPlayer)XmlAttach.FindAttachment(m, typeof(XMLNewPlayer));
						xmldel.Delete();
					}
					else
						return;
				}
				else
				{
					if (m is PlayerMobile && c.LowLevelBonus)
					{
						XmlAttach.AttachTo(m, new XMLNewPlayer());
					}
					else
						return;
				}
			}			
        }
    }
}