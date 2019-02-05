using System;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientCharmScroll : CSpellScroll
    {
        [Constructable]
        public AncientCharmScroll()
            : this(1)
        {
        }

        [Constructable]
        public AncientCharmScroll(int amount)
            : base(typeof(AncientCharmSpell), 0x1F51, amount)
        {
            Name = "Charm Scroll";
            Hue = 1355;
        }

        public AncientCharmScroll(Serial serial)
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
