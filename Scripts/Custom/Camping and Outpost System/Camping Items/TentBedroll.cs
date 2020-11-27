using Server.Mobiles;
using Server.Network;
using System;

namespace Server.Items
{
    [FlipableAttribute(0xA57, 0xA58, 0xA59)]
    public class TentBedroll : Item
    {
        [Constructable]
        public TentBedroll()
            : base(0xA57) //0xA576 is rolled
        {
            Weight = 5.0;
        }

        public TentBedroll(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
	    //PlayerMobile pm = from as PlayerMobile;

            if (Parent != null || !VerifyMove(from))
                return;
/*
            if (!from.InRange(this, 3))
            {
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
                return;
            }
	    else
	    {
                pm.BedrollLogout = true;
		pm.SendMessage("This seems like a safe place to rest.");
	    }
*/
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();
        }
    }
}
