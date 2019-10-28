using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Items
{
    class DreamCrystal : Item
    {
        [Constructable]
        public DreamCrystal() : base(0x1F1C)
        {
            Name = "Dream Crystal";
            Hue = 0xC2;
            
        }        

        public override bool IsArtifact
        {
            get
            {
                return true;
            }
        }


        public DreamCrystal(Serial serial) : base(serial)
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
