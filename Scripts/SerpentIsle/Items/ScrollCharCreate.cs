using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Items;
using Server.Gumps;

namespace Server.Items
{
    class ScrollCharCreate : Item
    {

        [Constructable]
        public ScrollCharCreate() : base()
        {
            ItemID = 0x1F23;
            Name = "Character Creation Deed";            
        }

        public ScrollCharCreate(Serial serial) : base(serial)
        {

        }

        public override void OnDoubleClick(Mobile from)
        {
            if(from.Region.IsPartOf("the Serpent Pillar Expedition Launch"))
            {
                from.SendGump(new GumpCharCreate(from, 0));
            }
            else
            {
                from.SendMessage("Thou must be at the Expedition Launch to use this!");
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
