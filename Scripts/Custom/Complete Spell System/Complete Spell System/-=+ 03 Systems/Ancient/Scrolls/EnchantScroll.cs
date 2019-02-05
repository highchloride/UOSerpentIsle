using System;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientEnchantScroll : CSpellScroll
    {
        [Constructable]
        public AncientEnchantScroll()
            : this(1)
        {
        }

        [Constructable]
        public AncientEnchantScroll(int amount)
            : base(typeof(AncientEnchantSpell), 0x1F35, amount)
        {
            Name = "Enchant Scroll";
            Hue = 1355;
        }

        public AncientEnchantScroll(Serial serial)
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
