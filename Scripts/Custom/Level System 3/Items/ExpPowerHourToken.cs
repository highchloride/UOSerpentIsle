using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Commands;
using Server.Engines.PartySystem;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class ExpPowerHourToken : Item
    {
		public static void Initialize()
		{
			CommandHandlers.Register( "exphour", AccessLevel.GameMaster, new CommandEventHandler( exphour_OnCommand ) );
        }

		[Usage( "exphour" )]
		[Description( "Gives 1 hour of Exp boost." )]
		public static void exphour_OnCommand( CommandEventArgs e )
		{
			PlayerMobile from = e.Mobile as PlayerMobile;
			if ( null != from )
				from.Target = new EPowerHourTarget( from, true );
        }
		
		Configured c = new Configured();
        
        [Constructable]
        public ExpPowerHourToken()
            : base(0x1869)
        {
            Name = "EXP Power Hour Coin";
            Weight = 1.0;
            LootType = LootType.Blessed;
			ItemID = 10922;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add("1 Hour Exp Boost Token"); // value: ~1_val~
        }

        public override void OnDoubleClick(Mobile from)
        {
			XMLPlayerLevelAtt xmlplayer = (XMLPlayerLevelAtt)XmlAttach.FindAttachment(from, typeof(XMLPlayerLevelAtt));
			ExpPowerHour powerlevel = (ExpPowerHour)XmlAttach.FindAttachment(from, typeof(ExpPowerHour));
			PlayerMobile pm = from as PlayerMobile;
			Configured c = new Configured();
			var p = Party.Get(from);
			int range = c.PartyRange;
			
            if (IsChildOf(pm.Backpack))
            {		
				if (xmlplayer == null)
				{
					pm.SendMessage("This wont work for you!");
					return;
				}
				else if (powerlevel != null)
				{
					pm.SendMessage("You must wait for your current Exp Power Hour to end!");
					return;
				}
				else
				{
					if (p != null)
					{
						foreach (PartyMemberInfo mi in p.Members)
						{
							pm = mi.Mobile as PlayerMobile;
							if (pm.Alive && pm.InRange(pm, range))
							{
								XMLPlayerLevelAtt xmlplayer2 = (XMLPlayerLevelAtt)XmlAttach.FindAttachment(pm, typeof(XMLPlayerLevelAtt));
								ExpPowerHour powerlevel2 = (ExpPowerHour)XmlAttach.FindAttachment(pm, typeof(ExpPowerHour));
								if (powerlevel2 != null)
								{
									pm.SendMessage("You already have a power hour, your party gains their bonus!");
									return;
								}
								if (xmlplayer2 == null)
								{
									pm.SendMessage("You lack level attachment, talk to your admin!");
									return;
								}
								else
								{
									XmlAttach.AttachTo(pm, new ExpPowerHour());
									this.Delete();
								}
							}
						}
					
					}
					XmlAttach.AttachTo(from, new ExpPowerHour());
					this.Delete();
				}   
            }
            else
                pm.SendMessage("This must be in your pack!");

        }

        public ExpPowerHourToken(Serial serial)
            : base(serial)
        {
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            
            int version = reader.ReadInt();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }
    }
	public class EPowerHourTarget : Target
	{
		Configured c = new Configured();
		private bool m_StaffCommand;

		public EPowerHourTarget( Mobile from, bool staffCommand ) : base( 10, false, TargetFlags.None )
		{
			m_StaffCommand = staffCommand;
			from.SendMessage( "Who gets the Power Hour?" );
		}
		
		protected override void OnTarget( Mobile from, object target )
		{
			XMLPlayerLevelAtt xmlplayer = (XMLPlayerLevelAtt)XmlAttach.FindAttachment(from, typeof(XMLPlayerLevelAtt));
			ExpPowerHour powerlevel = (ExpPowerHour)XmlAttach.FindAttachment(from, typeof(ExpPowerHour));
			PlayerMobile pm = from as PlayerMobile;
	
			if (xmlplayer == null)
			{
				pm.SendMessage("You cant give them power hour!");
				return;
			}
			else if (powerlevel != null)
			{
				pm.SendMessage("They already have a power hour!");
				return;
			}
			else
			{
				pm.SendMessage("They have been awarded Power Hour!");
				XmlAttach.AttachTo(from, new ExpPowerHour());
			}   

		}
	}
}