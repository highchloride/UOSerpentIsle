using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Mobiles
{
    class ShipTicketVendor : BaseVendor
    {
        private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();
        private int m_Loc;

        [CommandProperty(AccessLevel.Seer)]
        public int CityLocation
        {
            get { return m_Loc; }
            set
            {
                m_Loc = value;
                InvalidateProperties();
            }
        }

        [Constructable]
        public ShipTicketVendor(int loc)
            : base("the ship ticketer")
        {
            CityLocation = loc;
        }

        public ShipTicketVendor(Serial serial)
            : base(serial)
        {
        }

        protected override List<SBInfo> SBInfos
        {
            get
            {
                return this.m_SBInfos;
            }
        }
        public override void InitSBInfo()
        {
            switch(CityLocation)
            {
                case 0:
                    this.m_SBInfos.Add(new SBShipTransitMonitor()); break;
                case 1:
                    this.m_SBInfos.Add(new SBShipTransitFawn()); break;
                case 2:
                    this.m_SBInfos.Add(new SBShipTransitSleepingBull()); break;
                case 3:
                    this.m_SBInfos.Add(new SBShipTransitMoonshade()); break;
            }
            
        }

        public override void InitOutfit()
        {
            base.InitOutfit();
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
