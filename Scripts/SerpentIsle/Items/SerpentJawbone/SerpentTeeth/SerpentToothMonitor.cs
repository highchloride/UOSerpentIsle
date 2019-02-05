using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Items
{
    class SerpentToothMonitor : SerpentTooth
    {
        [Constructable]
        public SerpentToothMonitor()
        {
            Name = "Serpent Tooth";
            Hue = 0x492;
            Tooth = SerpentsTeeth.Monitor;
        }

        public SerpentToothMonitor(Serial serial) : base(serial)
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
