using System;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientAwakenScroll : CSpellScroll
    {
        [Constructable]
        public AncientAwakenScroll()
            : this(1)
        {
        }

        [Constructable]
        public AncientAwakenScroll(int amount)
            : base(typeof(AncientAwakenSpell), 0x1F32, amount)
        {
            Name = "Awaken Scroll";
            Hue = 1355;
        }

        public AncientAwakenScroll(Serial serial)
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
