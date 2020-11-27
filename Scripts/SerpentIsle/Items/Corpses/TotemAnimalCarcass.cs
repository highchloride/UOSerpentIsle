using Server.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.SerpentIsle.Items.Corpses
{
    public class TotemAnimalCarcass : Item
    {
        public TotemAnimalCarcass()
        {
            LootType = LootType.Blessed;
        }

        public TotemAnimalCarcass(Serial serial) : base(serial)
        { }
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

    
    public class TotemWolfCarcass : TotemAnimalCarcass
    {
        [Constructable]
        public TotemWolfCarcass()
        {
            ItemID = 0x2122;
            Hue = 0x157;
            Name = "Totem Wolf Carcass";
            Weight = 18;
        }

        public TotemWolfCarcass(Serial serial) : base(serial)
        { }

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

    
    public class TotemLeopardCarcass : TotemAnimalCarcass
    {
        [Constructable]
        public TotemLeopardCarcass()
        {
            ItemID = 0x25A3;
            Hue = 0x55c;
            Name = "Totem Leopard Carcass";
            Weight = 18;
        }

        public TotemLeopardCarcass(Serial serial) : base(serial)
        { }

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

    
    public class TotemBearCarcass : TotemAnimalCarcass
    {
        [Constructable]
        public TotemBearCarcass()
        {
            ItemID = 0x20CF;
            Hue = 0x562;
            Name = "Totem Bear Carcass";
            Weight = 18;
        }

        public TotemBearCarcass(Serial serial) : base(serial)
        { }

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

