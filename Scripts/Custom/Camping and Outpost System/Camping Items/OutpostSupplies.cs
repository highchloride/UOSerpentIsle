using Server.ContextMenus;
using Server.Gumps;
using Server.Mobiles;
using System.Collections.Generic;
using Server.Multis;
using System;

namespace Server.Items
{
    public class OutpostSupplies : Item
    {

	public OutpostCamp LinkedOutpost;

        [Constructable]
        public OutpostSupplies()
            : base(0xE3D)
        {
	    Name = "Ouutpost Supplies";
            Movable = false;
        }

        public OutpostSupplies(Serial serial)
            : base(serial)
        {
        }

	public override void OnDoubleClick(Mobile m)
	{

	    if( m.Skills[SkillName.Camping].Value >= 50 && LinkedOutpost != null)
	    {
		if (!m.HasGump(typeof(OutpostGump)))
		{
		    Timer.DelayCall(TimeSpan.FromSeconds(1), () =>
		    {
			m.SendGump(new OutpostGump(m, LinkedOutpost));
		    });
		}
	    }
	    else
		m.SendMessage("A skilled camper could use these supplies to upgrade the outpost.");

	    if( LinkedOutpost == null)
		m.SendMessage("Broken Link");	//supplies do not have a linked outpost

	}

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0);

        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

        }
    }
}