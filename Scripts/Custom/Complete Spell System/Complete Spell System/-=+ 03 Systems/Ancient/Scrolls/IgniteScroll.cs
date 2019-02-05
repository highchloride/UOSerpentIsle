using System;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientIgniteScroll : CSpellScroll
    {
        [Constructable]
        public AncientIgniteScroll()
            : this(1)
        {
        }

        [Constructable]
        public AncientIgniteScroll(int amount)
            : base(typeof(AncientIgniteSpell), 0x1F32, amount)
        {
            Name = "Ignite Scroll";
            Hue = 1355;
        }

        public AncientIgniteScroll(Serial serial)
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
