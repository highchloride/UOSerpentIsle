using System;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientDanceScroll : CSpellScroll
    {
        [Constructable]
        public AncientDanceScroll()
            : this(1)
        {
        }

        [Constructable]
        public AncientDanceScroll(int amount)
            : base(typeof(AncientDanceSpell), 0x1F51, amount)
        {
            Name = "Dance Scroll";
            Hue = 1355;
        }

        public AncientDanceScroll(Serial serial)
            : base(serial)
        {
        }

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
