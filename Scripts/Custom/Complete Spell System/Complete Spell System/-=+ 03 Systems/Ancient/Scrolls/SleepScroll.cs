using System;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientSleepScroll : CSpellScroll
    {
        [Constructable]
        public AncientSleepScroll()
            : this(1)
        {
        }

        [Constructable]
        public AncientSleepScroll(int amount)
            : base(typeof(AncientSleepSpell), 0x1F43, amount)
        {
            Name = "Sleep Scroll";
            Hue = 1355;
        }

        public AncientSleepScroll(Serial serial)
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
