//===============================================================================
//                      This script was created by Gizmo's UoDevPro
//                      This script was created on 1/11/2019 6:03:56 PM
//===============================================================================


using System;
using Server.Items;
using Server.Engines.Quests;

namespace Server.Mobiles
{
	public class Brendan : MondainQuester
	{
		[Constructable]
		public Brendan() : base("Brendan","")
		{
		}

		public Brendan(Serial serial) : base(serial)
		{
		}

		public override Type[] Quests { get { return new Type[] { typeof(GoblinSlayer) }; } }

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
