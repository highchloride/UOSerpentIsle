using System;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientFireworksScroll : CSpellScroll
    {
        [Constructable]
        public AncientFireworksScroll()
            : this(1)
        {
        }

        [Constructable]
        public AncientFireworksScroll(int amount)
            : base(typeof(AncientFireworksSpell), 0x1F32, amount)
        {
            Name = "Fireworks Scroll";
            Hue = 1355;
        }

        public AncientFireworksScroll(Serial serial)
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
