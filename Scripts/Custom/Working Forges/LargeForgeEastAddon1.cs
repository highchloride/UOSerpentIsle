
using System;
using Server;
using Server.Network;
using Server.Items;
using System.Collections;

namespace Server.Items
{
    public class LargeForgeEastAddon1 : BaseAddon
    {
        [Constructable]
        public LargeForgeEastAddon1()
        {
            this.AddComponent(new Bellows4(), 0, 0, 0);
            this.AddComponent(new AddonComponent(0x198A), 0, 1, 0);
            this.AddComponent(new AddonComponent(0x1996), 0, 2, 0);
            this.AddComponent(new Bellows3(), 0, 3, 0);
        }

        public LargeForgeEastAddon1(Serial serial)
            : base(serial)
        {
        }

        public override BaseAddonDeed Deed
        {
            get
            {
                return new LargeForgeEastDeed1();
            }
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

    public class LargeForgeEastDeed1 : BaseAddonDeed
    {
        [Constructable]
        public LargeForgeEastDeed1()
        {
        }

        public LargeForgeEastDeed1(Serial serial)
            : base(serial)
        {
        }

        public override BaseAddon Addon
        {
            get
            {
                return new LargeForgeEastAddon1();
            }
        }
        public override int LabelNumber
        {
            get
            {
                return 1044331;
            }
        }// large forge (east)
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