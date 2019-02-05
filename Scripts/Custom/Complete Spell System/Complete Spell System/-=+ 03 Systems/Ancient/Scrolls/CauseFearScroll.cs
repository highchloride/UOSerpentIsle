using System;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientCauseFearScroll : CSpellScroll
    {
        [Constructable]
        public AncientCauseFearScroll()
            : this(1)
        {
        }

        [Constructable]
        public AncientCauseFearScroll(int amount)
            : base(typeof(AncientCauseFearSpell), 0x1F56, amount)
        {
            Name = "Cause Fear Scroll";
            Hue = 1355;
        }

        public AncientCauseFearScroll(Serial serial)
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
