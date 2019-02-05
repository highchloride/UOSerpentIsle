using System;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientMassMightScroll : CSpellScroll
    {
        [Constructable]
        public AncientMassMightScroll()
            : this(1)
        {
        }

        [Constructable]
        public AncientMassMightScroll(int amount)
            : base(typeof(AncientMassMightSpell), 0x1F62, amount)
        {
            Name = "Mass Might Scroll";
            Hue = 1355;
        }

        public AncientMassMightScroll(Serial serial)
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
