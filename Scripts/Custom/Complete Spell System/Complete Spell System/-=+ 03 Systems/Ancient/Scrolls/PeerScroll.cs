using System;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientPeerScroll : CSpellScroll
    {
        [Constructable]
        public AncientPeerScroll()
            : this(1)
        {
        }

        [Constructable]
        public AncientPeerScroll(int amount)
            : base(typeof(AncientPeerSpell), 0x1F43, amount)
        {
            Name = "Peer Scroll";
            Hue = 1355;
        }

        public AncientPeerScroll(Serial serial)
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
