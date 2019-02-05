using System;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientMassSleepScroll : CSpellScroll
    {
        [Constructable]
        public AncientMassSleepScroll()
            : this(1)
        {
        }

        [Constructable]
        public AncientMassSleepScroll(int amount)
            : base(typeof(AncientMassSleepSpell), 0x1F51, amount)
        {
            Name = "Mass Sleep Scroll";
            Hue = 1355;
        }

        public AncientMassSleepScroll(Serial serial)
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
