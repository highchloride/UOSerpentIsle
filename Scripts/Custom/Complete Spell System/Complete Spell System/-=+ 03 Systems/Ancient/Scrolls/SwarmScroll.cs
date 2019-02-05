using System;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientSwarmScroll : CSpellScroll
    {
        [Constructable]
        public AncientSwarmScroll()
            : this(1)
        {
        }

        [Constructable]
        public AncientSwarmScroll(int amount)
            : base(typeof(AncientSwarmSpell), 0x1F43, amount)
        {
            Name = "Swarm Scroll";
            Hue = 1355;
        }

        public AncientSwarmScroll(Serial serial)
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
