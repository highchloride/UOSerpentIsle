using System;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientDestroyTrapScroll : CSpellScroll
    {
        [Constructable]
        public AncientDestroyTrapScroll()
            : this(1)
        {
        }

        [Constructable]
        public AncientDestroyTrapScroll(int amount)
            : base(typeof(AncientDestroyTrapSpell), 0x1F35, amount)
        {
            Name = "Destroy Trap Scroll";
            Hue = 1355;
        }

        public AncientDestroyTrapScroll(Serial serial)
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
