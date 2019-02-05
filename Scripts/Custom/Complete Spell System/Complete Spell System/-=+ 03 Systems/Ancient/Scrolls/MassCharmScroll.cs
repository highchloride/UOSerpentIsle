using System;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientMassCharmScroll : CSpellScroll
    {
        [Constructable]
        public AncientMassCharmScroll()
            : this(1)
        {
        }

        [Constructable]
        public AncientMassCharmScroll(int amount)
            : base(typeof(AncientMassCharmSpell), 0x1F56, amount)
        {
            Name = "Mass Charm Scroll";
            Hue = 1355;
        }

        public AncientMassCharmScroll(Serial serial)
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
