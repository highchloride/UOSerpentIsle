/*
Purpose of this attachment.  
Certian things would not work out nicely with normal usage.  Example would be,
on movement for PM doesn't search area and add attachments to pets. For now
This attachment serves a purpose of adding the pet level attachment to all pets
that serve the player.  A fail safe exist to add the pet att anyways when the pet
kills something, however this helps ensure the attachment exist. If you rather not use this,
remove it and create an entry in OnTame in basecreature to add the attachment. Distro Edit
*/

using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using System.Collections;
using System.Collections.Generic;
using Server.Engines.XmlSpawner2;

namespace Server.Engines.XmlSpawner2
{
    public class PlayerAttloop : XmlAttachment
    {		
        public PlayerAttloop(ASerial serial) : base(serial)
        {
        }

        [Attachable]
        public PlayerAttloop()
        {
			Expiration = TimeSpan.FromSeconds(5);
        }

        [Attachable]
        public PlayerAttloop(double seconds, double duration)
        {
			Expiration = TimeSpan.FromSeconds(duration);
        }

		public override void OnAttach()
		{
			base.OnAttach();
			ConfiguredPetXML cp = new ConfiguredPetXML();
			if(AttachedTo is PlayerMobile)
			{
				PlayerMobile master = (PlayerMobile)((PlayerMobile)this.AttachedTo);
				List<Mobile> pets = master.AllFollowers;
				if (pets.Count > 0)
				{
				   for (int i = 0; i < pets.Count; ++i)
					{
						Mobile pet = (Mobile)pets[i];
						
						if (cp.EnabledLevelPets == true)
						GetAttPetSet(pet, ((PlayerMobile)this.AttachedTo));
					}
				}
			}
			else
				Delete();
		}
		
		public static void GetAttPetSet(Mobile pet, Mobile master)
		{
			Configured cpc = new Configured();
			BaseCreature bc = (BaseCreature)pet;
			XMLPetLevelAtt petxml = (XMLPetLevelAtt)XmlAttach.FindAttachment(bc, typeof(XMLPetLevelAtt));
			if (petxml == null)
			{
				XmlAttach.AttachTo(bc, new XMLPetLevelAtt());
				if (cpc.TamingGivesExp == true)
                LevelCore.Taming(master);
			}
			else
				return;
		}
		
		public override void OnDelete()
		{
			XmlAttach.AttachTo(((PlayerMobile)AttachedTo), new PlayerAttloop());
		}
		public override void OnReattach()
		{
			base.OnReattach();
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
