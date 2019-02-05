using System;

namespace Server.Items
{
    public class LargeForgeSouthAddon1 : BaseAddon
    {
        [Constructable]
        public LargeForgeSouthAddon1()
        {
            this.AddComponent(new Bellows1(), 0, 0, 0);
            this.AddComponent(new AddonComponent(0x197E), 1, 0, 0);
            this.AddComponent(new AddonComponent(0x19A2), 2, 0, 0);
            this.AddComponent(new Bellows2(), 3, 0, 0);
        }

        public LargeForgeSouthAddon1(Serial serial)
            : base(serial)
        {
        }

        public override BaseAddonDeed Deed
        {
            get
            {
                return new LargeForgeSouthDeed1();
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

    public class LargeForgeSouthDeed1 : BaseAddonDeed
    {
        [Constructable]
        public LargeForgeSouthDeed1()
        {
        }

        public LargeForgeSouthDeed1(Serial serial)
            : base(serial)
        {
        }

        public override BaseAddon Addon
        {
            get
            {
                return new LargeForgeSouthAddon1();
            }
        }
        public override int LabelNumber
        {
            get
            {
                return 1044332;
            }
        }// large forge (south)
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