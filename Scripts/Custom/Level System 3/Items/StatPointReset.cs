using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Commands;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class StatPointReset : Item
    {
		Configured c = new Configured();
		public static void Initialize()
		{
			CommandHandlers.Register( "resetstat", AccessLevel.GameMaster, new CommandEventHandler( resetstat_OnCommand ) );
        }

		[Usage( "resetstat" )]
		[Description( "Reset Stats." )]
		public static void resetstat_OnCommand( CommandEventArgs e )
		{
			PlayerMobile from = e.Mobile as PlayerMobile;
			if ( null != from )
				from.Target = new ResetStatTarget( from, true );
        }

        [Constructable]
        public StatPointReset()
            : base(0x1869)
        {
            Name = "Stat Reset Coin";
            Weight = 1.0;
            LootType = LootType.Blessed;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

        }

        public override void OnDoubleClick(Mobile from)
        {
			XMLPlayerLevelAtt xmlplayer = (XMLPlayerLevelAtt)XmlAttach.FindAttachment(from, typeof(XMLPlayerLevelAtt));
			int totalstatsreturned = (int)(xmlplayer.StrPointsUsed) + (xmlplayer.IntPointsUsed) + (xmlplayer.DexPointsUsed);
			int str = (int)(xmlplayer.StrPointsUsed);
			int dex = (int)(xmlplayer.DexPointsUsed);
			int intt = (int)(xmlplayer.IntPointsUsed);
			
			if (totalstatsreturned == 0)
			{
				from.SendMessage( "You need to have points to refund!" );
				return;
			}
			
			if (str == 0 || dex == 0 || intt == 0)
			{
				from.SendMessage( "You must have used at least 1 point in each category to use this! (STR, INT and DEX)" );
				return;
			}
			
			if (xmlplayer.StrPointsUsed != 0)
			{
				from.Str += -str;
				xmlplayer.StatPoints += str;
				xmlplayer.StrPointsUsed = 0;
			}
			if (xmlplayer.DexPointsUsed != 0)
			{
				from.Dex += -dex;
				xmlplayer.StatPoints += dex;
				xmlplayer.DexPointsUsed = 0;
			}
			if (xmlplayer.IntPointsUsed != 0)
			{
				from.Int += -intt;
				xmlplayer.StatPoints += intt;
				xmlplayer.IntPointsUsed = 0;
			}
			this.Delete();
        }

        public StatPointReset(Serial serial)
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
	public class ResetStatTarget : Target
	{
		Configured c = new Configured();
		private bool m_StaffCommand;

		public ResetStatTarget( Mobile from, bool staffCommand ) : base( 10, false, TargetFlags.None )
		{
			m_StaffCommand = staffCommand;
			from.SendMessage( "Reset stats for which player?" );
		}
		
		protected override void OnTarget( Mobile from, object target )
		{
			XMLPlayerLevelAtt xmlplayer = (XMLPlayerLevelAtt)XmlAttach.FindAttachment(from, typeof(XMLPlayerLevelAtt));
            PlayerMobile pm = from as PlayerMobile;
			BaseCreature pet = target as BaseCreature;
			int totalstatsreturned = (int)(xmlplayer.StrPointsUsed) + (xmlplayer.IntPointsUsed) + (xmlplayer.DexPointsUsed);
			int str = (int)(xmlplayer.StrPointsUsed);
			int dex = (int)(xmlplayer.DexPointsUsed);
			int intt = (int)(xmlplayer.IntPointsUsed);

			if ( target == pet )
			{
				from.SendMessage( "This only works on Players!" );
				return;
			}
			if (totalstatsreturned == 0)  /* Max Level per System */
			{
				pm.SendMessage("Target has no stats to reset!");
				return;
			}
			else
			{
				if (target is Mobile)
				{		
					if (xmlplayer.StrPointsUsed > 0)
					{
						from.Str += -str;
						xmlplayer.StatPoints += str;
						xmlplayer.StrPointsUsed = 0;
					}
					if (xmlplayer.DexPointsUsed > 0)
					{
						from.Dex += -dex;
						xmlplayer.StatPoints += dex;
						xmlplayer.DexPointsUsed = 0;
					}
					if (xmlplayer.IntPointsUsed > 0)
					{
						from.Int += -intt;
						xmlplayer.StatPoints += intt;
						xmlplayer.IntPointsUsed = 0;
					}
				}         
			}		
		}
	}
}
