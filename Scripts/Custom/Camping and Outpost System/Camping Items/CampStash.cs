using Server.Accounting;
using Server.ContextMenus;
using Server.Items;
using Server.Network;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Items
{
    [DynamicFliping]
    [Flipable(0x9AB, 0xE7C)]
    public class CampStash : Item
    {
        [Constructable]
        public CampStash()
            : base(0x9AB)
        {
	    Movable = false;
        }

        public CampStash(Serial serial)
            : base(serial)
        {
        }

	public override void OnDoubleClick(Mobile from)
	{
	    from.BankBox.Open();
	}

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}