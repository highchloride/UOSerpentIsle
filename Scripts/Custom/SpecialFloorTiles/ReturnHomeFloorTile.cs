using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class ReturnHomeFloorTile : Item
	{
		[Constructable]
		public ReturnHomeFloorTile()
			: base(1286)
		{
			Movable = false;
			Hue = 88;
			Name = "sends monsters home";
			Visible = false;
		}

		public ReturnHomeFloorTile(Serial serial)
			: base(serial)
		{ }

		public override bool OnMoveOver(Mobile m)
		{
			BaseCreature bc = m as BaseCreature;

			if (bc != null && bc.Summoned != true && bc.Controlled != true && bc.Home != new Point3D(0, 0, 0))
			{
				Point3D from = bc.Location;
				Point3D to = bc.Home;
				Effects.SendLocationParticles(EffectItem.Create(from, bc.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023);
				Effects.SendLocationParticles(EffectItem.Create(to, bc.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 5023);
				bc.PlaySound(510);
				bc.Location = bc.Home;
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
