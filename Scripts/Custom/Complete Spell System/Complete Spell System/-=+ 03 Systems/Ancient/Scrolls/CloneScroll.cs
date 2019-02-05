using System;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientCloneScroll : CSpellScroll
    {
        [Constructable]
        public AncientCloneScroll()
            : this(1)
        {
        }

        [Constructable]
        public AncientCloneScroll(int amount)
            : base(typeof(AncientCloneSpell), 0x1F56, amount)
        {
            Name = "Clone Scroll";
            Hue = 1355;
        }

        public AncientCloneScroll(Serial serial)
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
