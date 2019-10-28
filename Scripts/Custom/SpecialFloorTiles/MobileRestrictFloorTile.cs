using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class MobileRestrictFloorTile : Item
	{
		[Constructable]
		public MobileRestrictFloorTile()
			: base(0x50d)
		{
			Movable = false;
			Hue = 55;
			Name = "monsters may NOT pass this";
			Visible = false;
		}

		public MobileRestrictFloorTile(Serial serial)
			: base(serial)
		{ }

		public override bool OnMoveOver(Mobile m)
		{
			if (m != null && m is Server.Mobiles.BaseCreature)
			{
				return false;
			}

			return true;
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
