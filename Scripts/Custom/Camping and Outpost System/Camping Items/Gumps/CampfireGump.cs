using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using System;
using Server.Items;
using Server.Multis;

namespace Server.Gumps
{
        public class CampfireGump : Gump
        {
            //private readonly Timer m_CloseTimer;
            private readonly CampfireEntry m_Entry;
	    private readonly Campfire m_Campfire;

            public CampfireGump(CampfireEntry entry, Campfire fire)
                : base(100, 0)
            {
                m_Entry = entry;
		m_Campfire = fire;

                AddBackground(0, 0, 380, 200, 0xA28);

		AddHtml(120, 20, 200, 35, "Secure Campfire", false, false);
	        AddItem(80, 20, 0xDE3);


		AddHtml(50, 55, 300, 140, "At a secure campsite, you can: <br>-Mark this location on your Camper's Map.<br>-Upgrade your campsite to provide yourself and all nearby allies with a boon.", false, false);

                AddButton(75, 133, 0xFA5, 0xFA7, 1, GumpButtonType.Reply, 0);
                AddHtml(110, 135, 110, 70, "MARK LOCATION", false, false);

                AddButton(75, 158, 0xFA5, 0xFA7, 2, GumpButtonType.Reply, 0);
                AddHtml(110, 160, 110, 70, "UPGRADE CAMP", false, false);

            }

            public override void OnResponse(NetState sender, RelayInfo info)
            {
                PlayerMobile pm = m_Entry.Player;
		Campfire fire = m_Campfire;
		int button = info.ButtonID;
            	BaseBoat boat = BaseBoat.FindBoatAt(pm.Location, pm.Map);

                if (Campfire.GetEntry(pm) != m_Entry)
                    return;

		if(button == 1)
		{
              	    CampersMap map = FindMap(pm);
		    if(map == null || map.Deleted)
		    {
			pm.SendMessage("A camper's map must be in your backpack.");
			return;
		    }
		    else if(pm.Skills[SkillName.Camping].Value < 60.0)
		    {
			pm.SendMessage("You do not have sufficient skill in camping to do that.");
			return;
		    }
            	    else if (boat != null && !(boat is BaseGalleon))
            	    {
                	pm.LocalOverheadMessage(MessageType.Regular, 0x3B2, 501800); // You cannot mark an object at that location.
            	    }
		    else
		    {
		    	map.Target = fire.Location;
		    	map.TargetMap = fire.Map;
		    	map.Marked = true;
			pm.PlaySound(0x249);
		    	pm.SendMessage("Your camper's map has been updated.");
		    }

		    pm.SendGump(new CampfireGump(m_Entry, fire));
		}

		if(button == 2)
		{
		    fire.IsUpgraded = true;
		    pm.PlaySound(0x208);
		}


            }

            private static CampersMap FindMap(Mobile from)
            {
            	if (from == null || from.Backpack == null)
            	{
                    return null;
            	}

            	if (from.Holding is CampersMap)
            	{
                    return (CampersMap)from.Holding;
            	}

            	return from.Backpack.FindItemByType<CampersMap>();
            }

            private void CloseGump()
            {
                Campfire.RemoveEntry(m_Entry);
                m_Entry.Player.CloseGump(typeof(CampfireGump));
            }
        }
}