using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Items
{
    class SerpentToothMoonshade : SerpentTooth
    {
        [Constructable]
        public SerpentToothMoonshade()
        {
            Name = "Serpent Tooth";
            Hue = 0x490;
        }

        public SerpentToothMoonshade(Serial serial) : base(serial)
        { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();
        }
    }
}
