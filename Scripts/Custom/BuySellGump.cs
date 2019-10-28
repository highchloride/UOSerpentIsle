/**
In order for basevendors to use this gump to provide UI, you must add the following to your BaseVendor.cs:
///
Add this at the top of of the file, with the other references.

using Server.Gumps;
///
This override should be added to any existing OnDoubleClick override, if you're unsure, this snip can be placed directly below BardImune.

public override void OnDoubleClick( Mobile from )
		{
			bool enabled;
			enabled = CheckVendorAccess(from);

			if (enabled) {
				if (from.InRange (this.Location, 2)) {
					from.CloseGump (typeof(BuySellGump));
					from.SendGump (new BuySellGump (from, this));
				}
			}
		}
///
**/

using System;
using Server.Factions;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Targeting;

namespace Server.Gumps
{
    public class BuySellGump : Gump
    {
		private BaseVendor m_Vendor;

		public BuySellGump(Mobile from, BaseVendor vendor)
			: base(400, 100)
		{
			PlayerMobile pm = (PlayerMobile)from;
			m_Vendor = vendor;

			this.AddBackground(0, 0, 98, 71, 9270);

			this.AddButton (15, 13, 0x867, 0x869, 1, GumpButtonType.Reply, 0);
			AddLabel( 18, 40, 0x903, "Buy" );

			this.AddButton (55, 13, 0x867, 0x869, 2, GumpButtonType.Reply, 0);
			AddLabel( 57, 40, 0x903, "Sell" );
		}

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            base.OnResponse(sender, info);
			Mobile from = sender.Mobile;

            switch (info.ButtonID)
            {
			case 1:
				{
					m_Vendor.VendorBuy(from);
					break;
				}
			case 2:
				{
					m_Vendor.VendorSell(from);
					break;
				}
            }
        }
    }
}
//Sidle