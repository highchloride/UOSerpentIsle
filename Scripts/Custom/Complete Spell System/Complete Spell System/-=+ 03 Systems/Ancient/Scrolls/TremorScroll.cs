using System;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientTremorScroll : CSpellScroll
    {
        [Constructable]
        public AncientTremorScroll()
            : this(1)
        {
        }

        [Constructable]
        public AncientTremorScroll(int amount)
            : base(typeof(AncientTremorSpell), 0x1F56, amount)
        {
            Name = "Tremor Scroll";
            Hue = 1355;
        }

        public AncientTremorScroll(Serial serial)
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
