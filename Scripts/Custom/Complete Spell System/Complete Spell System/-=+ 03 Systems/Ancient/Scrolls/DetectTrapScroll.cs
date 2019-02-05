using System;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientDetectTrapScroll : CSpellScroll
    {
        [Constructable]
        public AncientDetectTrapScroll()
            : this(1)
        {
        }

        [Constructable]
        public AncientDetectTrapScroll(int amount)
            : base(typeof(AncientDetectTrapSpell), 0x1F2E, amount)
        {
            Name = "Detect Trap Scroll";
            Hue = 1355;
        }

        public AncientDetectTrapScroll(Serial serial)
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
