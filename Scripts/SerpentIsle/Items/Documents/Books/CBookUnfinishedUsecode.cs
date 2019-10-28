using System;
using Server;

namespace Server.Items
{
    public class CBookUnfinishedUsecode : BaseBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Unfinished Usecode", "Phil",

            new BookPageInfo
            (
                "An enlightening",
                "discourse on the enigma",
                "of usecode. Beginning",
                "with the heretofore",
                "unresolved mysteries of",
                "global variables..."
            )
        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public CBookUnfinishedUsecode() : base(0xFF0, false)
        {
            Hue = 124;
        }

        public CBookUnfinishedUsecode(Serial serial) : base(serial)
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
