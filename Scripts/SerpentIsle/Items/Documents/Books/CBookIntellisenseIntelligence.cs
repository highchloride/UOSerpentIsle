using System;
using Server;

namespace Server.Items
{
    public class CBookIntellisenseIntelligence : BaseBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Intellisense Intelligence", "Lord Salt",

            new BookPageInfo
            (
                "This baffling tome",
                "discusses nonsense terms",
                "like 'serialization' and",
                "seems to advocate",
                "a philosophy called",
                "'break it til you make it.'"
            )
        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public CBookIntellisenseIntelligence() : base(0xFF4, false)
        {
            Hue = 129;
        }

        public CBookIntellisenseIntelligence(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
        }
    }
}
