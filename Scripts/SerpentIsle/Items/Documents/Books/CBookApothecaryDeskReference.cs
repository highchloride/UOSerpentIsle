using System;
using Server;

namespace Server.Items
{
    public class CBookApothecaryDeskReference : BaseBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Apothecary Desk Reference", "Fetoau",

            new BookPageInfo
            (
                "It is the author's",
                "expectation that thou",
                "art reading this to",
                "familiarize thyself with",
                "the effects of various",
                "potions based on their",
                "color. The first part of",
                "this work will discuss"
            ),
            new BookPageInfo
            (
                "such aspects, with the",
                "remaining pages covering",
                "the materials and steps",
                "required to make such",
                "alchemical creations.",
                "Definitions: Black",
                "potion: Drinking this",
                "will allow the"
            ),
            new BookPageInfo
            (
                "individual to see in the",
                "dark for a time. Blue",
                "potion: This mixture",
                "will grant the imbiber a",
                "boost to their reflexes.",
                "Orange potion: This",
                "potion will counter the",
                "effects of poisons,"
            ),
            new BookPageInfo
            (
                "including those gained",
                "from drinking a GREEN",
                "potion. Purple potion:",
                "WARNING: This concoction",
                "is dangerous and will",
                "explode shortly after",
                "being shaken. White",
                "potion: This potion will"
            ),
            new BookPageInfo
            (
                "provide a burst of",
                "strength. Yellow potion:",
                "This powerful mixture",
                "will give healing aid to",
                "the imbiber's wounds.",
                "WARNING: Green potion:",
                "This potion is a",
                "dangerous toxin, and"
            ),
            new BookPageInfo
            (
                "will poison the imbiber,",
                "possibly killing the",
                "individual. Red potion:",
                "This fabulous drink will",
                "treat exhaustion by",
                "restoring lost stamina.",
                "This next section",
                "details how one can best"
            ),
            new BookPageInfo
            (
                "recreate these uncanny",
                "concoctions..."
            )
        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public CBookApothecaryDeskReference() : base(0xFF1, false)
        {
            Hue = 360;
        }

        public CBookApothecaryDeskReference(Serial serial) : base(serial)
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
