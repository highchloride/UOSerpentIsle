using System;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientWeatherScroll : CSpellScroll
    {
        [Constructable]
        public AncientWeatherScroll()
            : this(1)
        {
        }

        [Constructable]
        public AncientWeatherScroll(int amount)
            : base(typeof(AncientWeatherSpell), 0x1F2E, amount)
        {
            Name = "Weather Scroll";
            Hue = 1355;
        }

        public AncientWeatherScroll(Serial serial)
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
