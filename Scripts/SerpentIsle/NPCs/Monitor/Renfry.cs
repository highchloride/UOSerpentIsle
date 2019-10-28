//===============================================================================
//                      This script was created by Gizmo's UoDevPro
//                      This script was created on 9/18/2019 1:19:20 PM
//===============================================================================


using System;
using Server.Items;
using Server.Engines.Quests;

namespace Server.Mobiles
{
	public class Renfry : MondainQuester
	{
		[Constructable]
		public Renfry() : base("Renfry","of Monitor")
		{
		}

		public Renfry(Serial serial) : base(serial)
		{
		}

		public override Type[] Quests { get { return new Type[] { typeof(CurseintheCrypts), typeof(TheCryptsArise) }; } }

		public override void InitBody()
		{
			this.Race = Race.Human;
			this.Hue = Hue = 33770;
			this.HairItemID = 8264;
			this.HairHue = 2411;
			this.FacialHairItemID = 15178;
            this.FacialHairHue = 2411;
			this.Body = 0x190;
		}

        public override void InitOutfit()
        {
            SetWearable(new FancyShirt());
            SetWearable(new Doublet(348));
            SetWearable(new Skirt(348));
            SetWearable(new Boots());
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
