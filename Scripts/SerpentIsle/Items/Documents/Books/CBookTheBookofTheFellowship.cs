using System;
using Server;

namespace Server.Items
{
    public class CBookTheBookofTheFellowship : BaseBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "The Book of The Fellowshi", "Batlin of Britain",

            new BookPageInfo
            (
                "Good morning to thee,",
                "gentle friend and",
                "traveller! No matter",
                "what time of day it",
                "might be when thou art",
                "reading this -- no",
                "matter what the hour of",
                "the clock -- I say good"
            ),
            new BookPageInfo
            (
                "morning to thee because",
                "this very moment brings",
                "to thee the coming of",
                "the dawn. The dawn, as",
                "everyone knows, is the",
                "moment when illumination",
                "comes. The dawn marks",
                "the end of the long dark"
            ),
            new BookPageInfo
            (
                "night. It is the moment",
                "that marks a new",
                "beginning. It is mine",
                "humble hope that these",
                "words may be for thee a",
                "dawning, or at least, a",
                "sort of awakening..."
            )
        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public CBookTheBookofTheFellowship() : base(0x23A0, false)
        {
            Hue = 683;
        }

        public CBookTheBookofTheFellowship(Serial serial) : base(serial)
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
