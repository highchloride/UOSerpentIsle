using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Items
{
    public class MirrorOfTruth : MirrorOfPurification
    {
        [Constructable]
        public MirrorOfTruth() : base()
        {
            this.Name = "Mirror of Truth";
            this.Hue = 0x206;
        }

        public MirrorOfTruth(Serial serial) : base(serial)
        { }

        public override int LabelNumber
        {
            get
            {
                return 0;
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }    
}
