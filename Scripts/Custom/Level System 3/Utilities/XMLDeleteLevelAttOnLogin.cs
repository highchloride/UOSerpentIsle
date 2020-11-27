using System;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Engines.XmlSpawner2;

/*	
-- This is FALSE by default, you must turn this to TRUE for this script to do
-- it's job.  This script will continue to remove the attachment on login
-- if it's detected on players until you either delete this script or set
-- the below option back to False 
*/
namespace Server.Misc
{
    public class XMLDeleteLevelAttOnLogin
    {
		private bool ActivateDeleteOnLogin = false;
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
				XMLDeleteLevelAttOnLogin cr = new XMLDeleteLevelAttOnLogin();
				XMLPlayerLevelAtt xmlplayer = (XMLPlayerLevelAtt)XmlAttach.FindAttachment(m, typeof(XMLPlayerLevelAtt));
				if (xmlplayer != null && cr.ActivateDeleteOnLogin == true)
				{
					xmlplayer.Delete();
				}
				else 
				{
					return;
				}
			}			
        }
    }
}