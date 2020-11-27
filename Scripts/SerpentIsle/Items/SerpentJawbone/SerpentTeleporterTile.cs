using System;
using Server;
using Server.Gumps;

namespace Server.Items
{
    public class SerpentTeleporterTileAddon : BaseAddon
    {
        public override BaseAddonDeed Deed { get { return new SerpentTeleporterTileDeed(); } }

        private Point3D m_Destination;
        private SerpentTooth m_RequiredTooth;

        [CommandProperty(AccessLevel.Seer)]
        public Point3D Destination
        {
            get { return m_Destination; }
            set
            {
                m_Destination = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.Seer)]
        public SerpentTooth RequiredTooth
        {
            get { return m_RequiredTooth; }
            set
            {
                m_RequiredTooth = value;
                InvalidateProperties();
            }
        }

        [Constructable]
        public SerpentTeleporterTileAddon()
        {
            AddComponent(new AddonComponent(16109), 0, 0, 0);
            AddComponent(new AddonComponent(16110), 1, 0, 0);
            AddComponent(new AddonComponent(16111), 0, 1, 0);
            AddComponent(new AddonComponent(16112), 1, 1, 0);
            foreach(AddonComponent component in Components)
            {
                component.Name = "Serpent Gate";                
            }
        }

        public SerpentTeleporterTileAddon(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version

            //Version 1
            writer.Write(m_Destination);

            //Version 0
            if (m_RequiredTooth != null)
                writer.WriteType(m_RequiredTooth.GetType());
            else
                writer.WriteType(null);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch(version)
            {
                case 1:
                    m_Destination = reader.ReadPoint3D();
                    goto case 0;
                case 0:
                    Type type = reader.ReadType();
                    if (type != null)
                    {
                        if (type == typeof(SerpentToothMonitor))
                            m_RequiredTooth = new SerpentToothMonitor();
                        else if (type == typeof(SerpentToothMoonshade))
                            m_RequiredTooth = new SerpentToothMoonshade();
                    }
                    break;
            }                
        }
    }

    public class SerpentTeleporterTileDeed : BaseAddonDeed
    {
        public override BaseAddon Addon { get { return new SerpentTeleporterTileAddon(); } }
        //public override int LabelNumber { get { return 1080486; } } // Valor Virtue Tile Deed

        [Constructable]
        public SerpentTeleporterTileDeed()
        {
            Name = "Serpent Gate Tile Deed";
            LootType = LootType.Blessed;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
            {
                base.OnDoubleClick(from);
            }
            else
                from.SendLocalizedMessage(1062334); // This item must be in your backpack to be used.       	
        }

        public SerpentTeleporterTileDeed(Serial serial)
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
