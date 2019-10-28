//===============================================================================
//                      This script was created by Gizmo's UoDevPro
//                      This script was created on 9/10/2019 3:39:21 PM
//===============================================================================


using System;
using Server.Items;
using Server.Engines.Quests;

namespace Server.Mobiles
{
	public class LordMarsten : MondainQuester
	{
		[Constructable]
		public LordMarsten() : base("Lord Marsten"," of Monitor")
		{
		}

		public LordMarsten(Serial serial) : base(serial)
		{
		}

        public override Type[] Quests { get { return new Type[] { typeof(TestofKnighthood) }; } }

		public override void InitBody()
		{
			this.Race = Race.Human;
			this.Hue = Hue = Utility.RandomSkinHue();
			this.HairItemID = Race.RandomHair(Female);
			this.HairHue = Race.RandomHairHue();
			this.FacialHairItemID = Race.RandomFacialHair(Female);
			if (FacialHairItemID != 0)
				FacialHairHue = Race.RandomHairHue();
			this.Body = 0x190;
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
