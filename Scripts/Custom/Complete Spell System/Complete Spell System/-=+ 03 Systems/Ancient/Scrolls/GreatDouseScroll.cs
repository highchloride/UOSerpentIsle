using System;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientGreatDouseScroll : CSpellScroll
    {
        [Constructable]
        public AncientGreatDouseScroll()
            : this(1)
        {
        }

        [Constructable]
        public AncientGreatDouseScroll(int amount)
            : base(typeof(AncientGreatDouseSpell), 0x1F43, amount)
        {
            Name = "Great Douse Scroll";
            Hue = 1355;
        }

        public AncientGreatDouseScroll(Serial serial)
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
