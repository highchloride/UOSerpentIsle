using System;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientSeanceScroll : CSpellScroll
    {
        [Constructable]
        public AncientSeanceScroll()
            : this(1)
        {
        }

        [Constructable]
        public AncientSeanceScroll(int amount)
            : base(typeof(AncientSeanceSpell), 0x1F47, amount)
        {
            Name = "Seance Scroll";
            Hue = 1355;
        }

        public AncientSeanceScroll(Serial serial)
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
