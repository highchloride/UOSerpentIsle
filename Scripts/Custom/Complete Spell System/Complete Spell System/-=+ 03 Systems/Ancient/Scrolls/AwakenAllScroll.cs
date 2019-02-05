using System;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientAwakenAllScroll : CSpellScroll
    {
        [Constructable]
        public AncientAwakenAllScroll()
            : this(1)
        {
        }

        [Constructable]
        public AncientAwakenAllScroll(int amount)
            : base(typeof(AncientAwakenAllSpell), 0x1F2E, amount)
        {
            Name = "Awaken All Scroll";
            Hue = 1355;
        }

        public AncientAwakenAllScroll(Serial serial)
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
