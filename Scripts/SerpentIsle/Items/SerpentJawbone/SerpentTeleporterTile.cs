using System;
using Server;
using Server.Gumps;

namespace Server.Items
{
    public class SerpentTeleporterTileAddon : BaseAddon
    {
        public override BaseAddonDeed Deed { get { return new SerpentTeleporterTileDeed(); } }

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

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
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
