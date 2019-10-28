using System;
using Server;

namespace Server.Items
{
	public class UnhideFloorTile : Item
	{
		[Constructable]
		public UnhideFloorTile()
			: base(0x51e)
		{
			Movable = false;
			Hue = 0;
			Name = "a revealing floor tile";
			Light = LightType.Circle300;
			Visible = false;
		}

		public UnhideFloorTile(Serial serial)
			: base(serial)
		{ }

		public override bool OnMoveOver(Mobile m)
		{
			if (m.AccessLevel == AccessLevel.Player)
			{
				if (m.Hidden != false)
				{
					m.RevealingAction();
				}
				else
				{
					return true;
				}
			}
			else
			{ return true; }

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
