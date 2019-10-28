using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Items
{
    class CrystalRoseOfLove : DecoFlower2
    {
        [Constructable]
        public CrystalRoseOfLove() : base()
        {
            this.Hue = 0x13;
            this.Name = "Crystal Rose of Love";
        }

        public CrystalRoseOfLove(Serial serial) : base(serial)
        { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            switch(version)
            {
                case 0:
                    break;
            }
        }
    }
}
