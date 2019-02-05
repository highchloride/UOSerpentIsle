using System;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientDouseScroll : CSpellScroll
    {
        [Constructable]
        public AncientDouseScroll()
            : this(1)
        {
        }

        [Constructable]
        public AncientDouseScroll(int amount)
            : base(typeof(AncientDouseSpell), 0x1F32, amount)
        {
            Name = "Douse Scroll";
            Hue = 1355;
        }

        public AncientDouseScroll(Serial serial)
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
