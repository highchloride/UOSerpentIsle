using System;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
    public class SBShipTransitSleepingBull : SBInfo
    {
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();
        public SBShipTransitSleepingBull()
        {
        }

        public override IShopSellInfo SellInfo
        {
            get
            {
                return m_SellInfo;
            }
        }
        public override List<GenericBuyInfo> BuyInfo
        {
            get
            {
                return m_BuyInfo;
            }
        }

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new GenericBuyInfo(typeof(ShipTicketToMonitor), 100, 20, 0x14F0, 0x492, false));
                Add(new GenericBuyInfo(typeof(ShipTicketToMoonshade), 50, 20, 0x14F0, 0x494, false));
                Add(new GenericBuyInfo(typeof(ShipTicketToFawn), 300, 20, 0x14F0, 0x495, false));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
                Add(typeof(ShipTicketToFawn), 25);
                Add(typeof(ShipTicketToMoonshade), 25);
                Add(typeof(ShipTicketToSleepingBull), 25);
                Add(typeof(ShipTicketToMonitor), 25);
            }
        }
    }
}
