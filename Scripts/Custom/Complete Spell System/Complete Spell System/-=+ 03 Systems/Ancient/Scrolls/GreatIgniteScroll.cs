using System;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientGreatIgniteScroll : CSpellScroll
    {
        [Constructable]
        public AncientGreatIgniteScroll()
            : this(1)
        {
        }

        [Constructable]
        public AncientGreatIgniteScroll(int amount)
            : base(typeof(AncientGreatIgniteSpell), 0x1F43, amount)
        {
            Name = "Great Ignite Scroll";
            Hue = 1355;
        }

        public AncientGreatIgniteScroll(Serial serial)
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
