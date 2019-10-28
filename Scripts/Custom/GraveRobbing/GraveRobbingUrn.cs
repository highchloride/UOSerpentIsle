using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Engines.Harvest;

namespace Server.Items
{
    public class GraveRobbingUrn : Item
    {
        [Constructable]
        public GraveRobbingUrn() : base(0x241D)
        {
            Name = "Funeral Urn";
        }

        public override void OnDoubleClick(Mobile from)
        {
            
        }

        public GraveRobbingUrn(Serial serial) : base(serial)
        { }

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
