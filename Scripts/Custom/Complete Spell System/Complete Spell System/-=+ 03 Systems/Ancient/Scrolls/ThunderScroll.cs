using System;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientThunderScroll : CSpellScroll
    {
        [Constructable]
        public AncientThunderScroll()
            : this(1)
        {
        }

        [Constructable]
        public AncientThunderScroll(int amount)
            : base(typeof(AncientThunderSpell), 0x1F32, amount)
        {
            Name = "Thunder Scroll";
            Hue = 1355;
        }

        public AncientThunderScroll(Serial serial)
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
