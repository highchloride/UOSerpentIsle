using System;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientSleepFieldScroll : CSpellScroll
    {
        [Constructable]
        public AncientSleepFieldScroll()
            : this(1)
        {
        }

        [Constructable]
        public AncientSleepFieldScroll(int amount)
            : base(typeof(AncientSleepFieldSpell), 0x1F56, amount)
        {
            Name = "Sleep Field Scroll";
            Hue = 1355;
        }

        public AncientSleepFieldScroll(Serial serial)
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
