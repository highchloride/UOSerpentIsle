using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Engines.XmlSpawner2
{
    public class ExpPowerHour : XmlAttachment
    {		
        public ExpPowerHour(ASerial serial) : base(serial)
        {
        }

        [Attachable]
        public ExpPowerHour()
        {
			Expiration = TimeSpan.FromMinutes(60);
        }

        [Attachable]
        public ExpPowerHour(double seconds, double duration)
        {
			Expiration = TimeSpan.FromMinutes(duration);
        }
		


		public override void OnAttach()
		{
			base.OnAttach();
			if(AttachedTo is PlayerMobile)
			{
				((PlayerMobile)AttachedTo).SendMessage("You have boosted Exp for 1 hour starting now!");
			}
			else
				Delete();
		}
		public override void OnDelete()
		{
			((PlayerMobile)AttachedTo).SendMessage("Your power hour has ended!");
		}
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize(writer);
			writer.Write( (int) 0 );
			// version 
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			// version 0
		}
		
    }
}
