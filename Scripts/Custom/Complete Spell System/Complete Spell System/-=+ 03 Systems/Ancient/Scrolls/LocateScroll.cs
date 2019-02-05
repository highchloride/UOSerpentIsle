using System;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientLocateScroll : CSpellScroll
    {
        [Constructable]
        public AncientLocateScroll()
            : this(1)
        {
        }

        [Constructable]
        public AncientLocateScroll(int amount)
            : base(typeof(AncientLocateSpell), 0x1F31, amount)
        {
            Name = "Locate Scroll";
            Hue = 1355;
        }

        public AncientLocateScroll(Serial serial)
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
