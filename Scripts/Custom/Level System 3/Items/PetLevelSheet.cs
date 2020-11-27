using System;
using System.Collections;
using Server;
using Server.Prompts;
using Server.Mobiles;
using Server.Network;
using Server.Gumps;
using Server.Items;
using Server.Commands;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
	public class PetLevelSheet : Item
	{
		int[] numArray = new int[1];
		ConfiguredPetXML cp = new ConfiguredPetXML();
		string str = CommandSystem.Prefix;
		
		[Constructable]
		public PetLevelSheet() : base( 7714 )
		{
			Movable = true;
			Weight = 1.0;
			Name = " Pet Level Sheet";
			LootType = LootType.Blessed;
			Hue = 1462;
		}
		public override void OnDoubleClick( Mobile m )
		{
			/* Using command instad of Gump as a work around*/
			CommandSystem.Handle(m, string.Format("{0}petlevel", str));
		}

		public PetLevelSheet( Serial serial ) : base( serial )
		{ 
		} 

        public virtual bool Accepted
        {
            get
            {
                return this.Deleted;
            }
        }
		
        public override bool DropToWorld(Mobile from, Point3D p)
        {
            bool ret = base.DropToWorld(from, p);
            if (ret && !this.Accepted && this.Parent != from.Backpack && cp.PetLevelSheetPerma)
            {
                if (from.IsStaff())
                {
                    return true;
                }
                else
                {
                    from.LocalOverheadMessage(MessageType.Emote, 0x22, true, "You feel silly for wanting to drop something so useful...");
                    return false;
                }
            }
            else
            {
                return ret;
            }
        }
        public override bool DropToMobile(Mobile from, Mobile target, Point3D p)
        {
            bool ret = base.DropToMobile(from, target, p);
            if (ret && !this.Accepted && this.Parent != from.Backpack && cp.PetLevelSheetPerma)
            {
                if (from.IsStaff())
                {
                    return true;
                }
                else
                {
                    from.LocalOverheadMessage(MessageType.Emote, 0x22, true, "This cannot be traded!");
                    return false;
                }
            }
            else
            {
                return ret;
            }
        }
        public override bool DropToItem(Mobile from, Item target, Point3D p)
        {
            bool ret = base.DropToItem(from, target, p);
            if (ret && !this.Accepted && this.Parent != from.Backpack && cp.PetLevelSheetPerma)
            {
                if (from.IsStaff())
                {
                    return true;
                }
                else
                {
                    from.LocalOverheadMessage(MessageType.Emote, 0x22, true, "This can only exist on the top level of the backpack!");
                    return false;
                }
            }
            else
            {
                return ret;
            }
        }
		
		public override void Serialize( GenericWriter writer ) 
		{ 
			base.Serialize( writer ); 

			writer.Write( (int) 0 ); // version 
		} 

		public override void Deserialize( GenericReader reader ) 
		{ 
			base.Deserialize( reader ); 

			int version = reader.ReadInt(); 
		}
	}
}