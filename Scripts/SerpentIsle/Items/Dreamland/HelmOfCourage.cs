using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Items
{
    [Flipable(0x2645, 0x2646)]
    public class HelmOfCourage : BaseArmor
    {
        [Constructable]
        public HelmOfCourage()
            : base(0x2645)
        {
            this.Name = "Helm of Courage";
            this.Weight = 5.0;
        }

        public HelmOfCourage(Serial serial)
            : base(serial)
        {
        }

        public override int BasePhysicalResistance
        {
            get
            {
                return 5;
            }
        }
        public override int BaseFireResistance
        {
            get
            {
                return 3;
            }
        }
        public override int BaseColdResistance
        {
            get
            {
                return 3;
            }
        }
        public override int BasePoisonResistance
        {
            get
            {
                return 3;
            }
        }
        public override int BaseEnergyResistance
        {
            get
            {
                return 3;
            }
        }
        public override int InitMinHits
        {
            get
            {
                return 255;
            }
        }
        public override int InitMaxHits
        {
            get
            {
                return 255;
            }
        }
        public override int AosStrReq
        {
            get
            {
                return 75;
            }
        }
        public override int OldStrReq
        {
            get
            {
                return 25;
            }
        }
        public override int ArmorBase
        {
            get
            {
                return 55;
            }
        }
        public override ArmorMaterialType MaterialType
        {
            get
            {
                return ArmorMaterialType.Plate;
            }
        }
        public override CraftResource DefaultResource
        {
            get
            {
                return CraftResource.None;
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (this.Weight == 1.0)
                this.Weight = 5.0;
        }
    }
}
