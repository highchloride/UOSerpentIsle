using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using System;
using Server.Items;
using Server.Multis;

namespace Server.Gumps
{
        public class OutpostGump : Gump
        {
            private readonly Mobile m_From;
	    private readonly OutpostCamp m_Camp;
	    private bool HasTent;

            public OutpostGump(Mobile from, OutpostCamp camp)
                : base(100, 0)
            {
                m_From = from;
		m_Camp = camp;

                AddBackground(0, 0, 380, 380, 0xA28);

		AddHtml(140, 20, 300, 35, "Secure Outpost", false, false);
	        AddItem(80, 15, 0xFAC);
	        AddItem(80, 20, 0xDE3);


		AddHtml(50, 55, 300, 140, "Outposts are upgradable encampments that can be outfitted with various utilities that benefit all passing adventurers.<br><br>Any adventurer with enough skill in Camping can establish or upgrade these outposts.", false, false);

                AddButton(75, 208, 0xFA5, 0xFA7, 1, GumpButtonType.Reply, 0);
                AddHtml(110, 210, 110, 70, "MARK LOCATION", false, false);

                AddButton(75, 233, 0xFA5, 0xFA7, 2, GumpButtonType.Reply, 0);
                AddHtml(110, 235, 110, 70, "BUILD TENT", false, false);

                AddButton(75, 258, 0xFA5, 0xFA7, 3, GumpButtonType.Reply, 0);
                AddHtml(110, 260, 110, 70, "BUILD SHRINE", false, false);

                AddButton(75, 283, 0xFA5, 0xFA7, 4, GumpButtonType.Reply, 0);
                AddHtml(110, 285, 110, 70, "BUILD STASH", false, false);
            }

            public override void OnResponse(NetState sender, RelayInfo info)
            {

                PlayerMobile pm = m_From as PlayerMobile;
		OutpostCamp camp = m_Camp;
		int button = info.ButtonID; 

                if(camp.m_Tent != null)
		    HasTent = true;
		else
		    HasTent = false;


		if(button == 1 && m_Camp.Active)
		{
              	    CampersMap map = FindMap(pm);
		    if(map == null || map.Deleted)
		    {
			pm.SendMessage("A camper's map must be in your backpack.");
			return;
		    }
		    else if(pm.Skills[SkillName.Camping].Value < 50.0)
		    {
			pm.SendMessage("You do not have sufficient skill in camping to do that.");
			return;
		    }
		    else
		    {
		    	map.Target = pm.Location;
		    	map.TargetMap = pm.Map;
		    	map.Marked = true;
			pm.PlaySound(0x249);
		    	pm.SendMessage("Your camper's map has been updated.");
		    }

		    pm.SendGump(new OutpostGump(pm, m_Camp));
		}

		if(button == 2 && m_Camp.Active)
		{
		    if(m_Camp.m_Tent == null && (pm.Skills[SkillName.Camping].Value >= 70))
		    {
			m_Camp.AddTent();
			pm.SendMessage("You upgrade the outpost with a tent.");
		        pm.PlaySound(0x23D);
		    }
		    else if(m_Camp.m_Tent != null)
			pm.SendMessage("The outpost already has that upgrade.");
		    else
			pm.SendMessage("Doing that would require greater skill in Camping.");

		    pm.SendGump(new OutpostGump(pm, m_Camp));
		}

		if(button == 3 && m_Camp.Active)
		{
		    if(m_Camp.m_Ankh == null && (pm.Skills[SkillName.Camping].Value >= 90))
		    {
			m_Camp.AddAnkh();
			pm.SendMessage("You upgrade the outpost with a shrine.");
		        pm.PlaySound(0x1E7);
		    }
		    else if(m_Camp.m_Ankh != null)
			pm.SendMessage("The outpost already has that upgrade.");
		    else
			pm.SendMessage("Doing that would require greater skill in Camping.");

		   pm.SendGump(new OutpostGump(pm, m_Camp));
		}

		if(button == 4 && m_Camp.Active)
		{
		    if(m_Camp.m_Stash == null && (pm.Skills[SkillName.Camping].Value >= 100))
		    {
			m_Camp.AddBank();
			pm.SendMessage("You upgrade the outpost with a stash.");
		        pm.PlaySound(0x2A); //or 0x3BA
		    }
		    else if(m_Camp.m_Stash != null)
			pm.SendMessage("The outpost already has that upgrade.");
		    else
			pm.SendMessage("Doing that would require greater skill in Camping.");

		   pm.SendGump(new OutpostGump(pm, m_Camp));
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
                //Campfire.RemoveEntry(m_Entry);
                m_From.CloseGump(typeof(OutpostGump));
            }
        }
}