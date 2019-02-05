using System;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientInvisibilityAllScroll : CSpellScroll
    {
        [Constructable]
        public AncientInvisibilityAllScroll()
            : this(1)
        {
        }

        [Constructable]
        public AncientInvisibilityAllScroll(int amount)
            : base(typeof(AncientInvisibilityAllSpell), 0x1F65, amount)
        {
            Name = "Invisibility All Scroll";
            Hue = 1355;
        }

        public AncientInvisibilityAllScroll(Serial serial)
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
