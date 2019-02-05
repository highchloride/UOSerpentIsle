using System;
using Server.Items;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class FakeGold : Item
    {
        public int m_Amount;
     
        [Constructable]
        public FakeGold()
            : base(0xEEF)
        {
            Weight = 0.0;
            Name = "" + m_Amount + " Gold Coins";

        }

        public FakeGold(Serial serial)
            : base(serial)
        {
        }

        public override int GetDropSound()
        {
            if (m_Amount <= 1)
                return 0x2E4;
            else if (m_Amount <= 5)
                return 0x2E5;
            else
                return 0x2E6;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
            writer.Write(m_Amount);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            m_Amount = reader.ReadInt();
        }
    }
}
