using System;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientDeathVortexScroll : CSpellScroll
    {
        [Constructable]
        public AncientDeathVortexScroll()
            : this(1)
        {
        }

        [Constructable]
        public AncientDeathVortexScroll(int amount)
            : base(typeof(AncientDeathVortexSpell), 0x1F66, amount)
        {
            Name = "Death Vortex Scroll";
            Hue = 1355;
        }

        public AncientDeathVortexScroll(Serial serial)
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
