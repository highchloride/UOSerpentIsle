using System;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientMassDeathScroll : CSpellScroll
    {
        [Constructable]
        public AncientMassDeathScroll()
            : this(1)
        {
        }

        [Constructable]
        public AncientMassDeathScroll(int amount)
            : base(typeof(AncientMassDeathSpell), 0x1F51, amount)
        {
            Name = "Mass Death Scroll";
            Hue = 1355;
        }

        public AncientMassDeathScroll(Serial serial)
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
