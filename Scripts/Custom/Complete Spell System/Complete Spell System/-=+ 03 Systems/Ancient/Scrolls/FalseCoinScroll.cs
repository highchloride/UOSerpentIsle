using System;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientFalseCoinScroll : CSpellScroll
    {
        [Constructable]
        public AncientFalseCoinScroll()
            : this(1)
        {
        }

        [Constructable]
        public AncientFalseCoinScroll(int amount)
            : base(typeof(AncientFalseCoinSpell), 0x1F35, amount)
        {
            Name = "False Coin Scroll";
            Hue = 1355;
        }

        public AncientFalseCoinScroll(Serial serial)
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
