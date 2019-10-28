using System;
using Server;

namespace Server.Items
{
    public class CBookFightingisanArt : BaseBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Fighting is an Art", "Johnson",

            new BookPageInfo
            (
                "Lesson One: In fighting,",
                "thou must never lose, no",
                "matter what thou must",
                "do. Anyone fighting thee",
                "is an enemy. An enemy",
                "must be destroyed.",
                "During battle thou wilt",
                "be confronted by thine"
            ),
            new BookPageInfo
            (
                "enemy. Thou must defeat",
                "him, or thou wilt bring",
                "disgrace upon thyself,",
                "thy clan, and thy",
                "trainer. Battles are",
                "always on the horizon.",
                "Thou must train hard,",
                "and work thyself hard."
            ),
            new BookPageInfo
            (
                "Thou must think battle,",
                "eat battle, sleep battle",
                "if thou dost expect to",
                "be victorious. Those",
                "warriors who do not",
                "think they can do this",
                "should contemplate",
                "another line of work."
            ),
            new BookPageInfo
            (
                "Knowing thy weapon is",
                "the greatest value. Be",
                "it sword, mace, or bow,",
                "thou must train with it",
                "until mastery if thou",
                "dost want to be a true",
                "warrior. If thou hast",
                "not the time or the mind"
            ),
            new BookPageInfo
            (
                "to train in this fashion",
                "then thou shouldst",
                "perhaps train with the",
                "shovel and the carrying",
                "of animal wastes from",
                "the fields instead. It",
                "is better for thee to",
                "serve thy clan as best"
            ),
            new BookPageInfo
            (
                "as thou canst, though it",
                "be a less than",
                "knight-worthy post, than",
                "for thee to run in",
                "battle and endanger thy",
                "comrades. Anyone caught",
                "running away from a",
                "battle should be put to"
            ),
            new BookPageInfo
            (
                "the sword immediately,",
                "lest others feel that",
                "they may let their",
                "womanish fears prevail.",
                "The true knight is not",
                "he who fights battles,",
                "but he who relishes in",
                "the spilling of blood."
            ),
            new BookPageInfo
            (
                "He who is enthralled",
                "with the smell of a",
                "bowel split wide, or the",
                "sight of anothers' blood",
                "staining his sword and",
                "armour, should be",
                "praised above all. This",
                "is what a true knight"
            ),
            new BookPageInfo
            (
                "seeks. He is a true",
                "master in the arts of",
                "fighting."
            )
        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public CBookFightingisanArt() : base(0xFF2, false)
        {
            Hue = 680;
        }

        public CBookFightingisanArt(Serial serial) : base(serial)
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
