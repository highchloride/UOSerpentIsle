using System;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientGlimmerScroll : CSpellScroll
    {
        [Constructable]
        public AncientGlimmerScroll()
            : this(1)
        {
        }

        [Constructable]
        public AncientGlimmerScroll(int amount)
            : base(typeof(AncientGlimmerSpell), 0x1F32, amount)
        {
            Name = "Glimmer Scroll";
            Hue = 1355;
        }

        public AncientGlimmerScroll(Serial serial)
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
