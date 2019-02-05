namespace Server.Items
{
	public class SwampBoots : ThighBoots
	{
		[Constructable]
		public SwampBoots() : base()
		{
			Name = "swamp boots";
			Hue = Utility.RandomGreenHue();
		}

		public SwampBoots(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}