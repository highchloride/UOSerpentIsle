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
    public class ExpCoin : Item
    {
		Configured c = new Configured();
		private int m_SCV = 100;
		
		public static void Initialize()
		{
			CommandHandlers.Register( "expaward", AccessLevel.GameMaster, new CommandEventHandler( expaward_OnCommand ) );
        }

		[Usage( "expaward" )]
		[Description( "Gives ExpAward." )]
		public static void expaward_OnCommand( CommandEventArgs e )
		{
			PlayerMobile from = e.Mobile as PlayerMobile;
			if ( null != from )
				from.Target = new AwardExpTarget( from, true );
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int SCV
        {
            get { return m_SCV; }
            set { m_SCV = value; InvalidateProperties(); }
        }
        
        [Constructable]
        public ExpCoin()
            : base(0x1869)
        {
            Name = "A Exp Coin";
            Weight = 1.0;
            LootType = LootType.Blessed;
            m_SCV = c.ExpCoinValue;
			ItemID = 10922;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add("+{0}", m_SCV.ToString(), "Exp Points"); // value: ~1_val~
        }

        public override void OnDoubleClick(Mobile from)
        {
			ConfiguredPetXML cp = new ConfiguredPetXML();
			XMLPlayerLevelAtt xmlplayer = (XMLPlayerLevelAtt)XmlAttach.FindAttachment(from, typeof(XMLPlayerLevelAtt));
            PlayerMobile pm = from as PlayerMobile;

            if (IsChildOf(pm.Backpack))
            {
                if (xmlplayer.Levell >= c.EndMaxLvl)  /* Max Level per System */
				{
		            pm.SendMessage("You have reached the max level, this doesn't work for you!");
					return;
				}
                else
                    xmlplayer.kxp += m_SCV;
                    pm.SendMessage("You have been awarded {0} EXP points", m_SCV);
					if (xmlplayer.Expp >= xmlplayer.ToLevell && xmlplayer.Levell < xmlplayer.MaxLevel)
                        LevelHandler.DoLevel(pm, new Configured());
                    this.Delete();               
            }
            else
                pm.SendMessage("This must be in your pack!");

        }

        public ExpCoin(Serial serial)
            : base(serial)
        {
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            
            int version = reader.ReadInt();
            switch (version)
            {
                case 0:
				{
					m_SCV = reader.ReadInt();
					break;
				}
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
            writer.Write((int)SCV);
        }
    }
	public class AwardExpTarget : Target
	{
		Configured c = new Configured();
		ExpCoin xp = new ExpCoin();
//		private int m_SCV = 100;
		private bool m_StaffCommand;

		public AwardExpTarget( Mobile from, bool staffCommand ) : base( 10, false, TargetFlags.None )
		{
			m_StaffCommand = staffCommand;
			from.SendMessage( "Who gets the Exp Award?." );
		}
		
		protected override void OnTarget( Mobile from, object target )
		{
			XMLPlayerLevelAtt xmlplayer = (XMLPlayerLevelAtt)XmlAttach.FindAttachment(from, typeof(XMLPlayerLevelAtt));
            PlayerMobile pm = from as PlayerMobile;
			BaseCreature pet = target as BaseCreature;

			if ( target == pet )
			{
				from.SendMessage( "This only works on Players!" );
			}
			else if (xmlplayer.Levell >= c.EndMaxLvl)  /* Max Level per System */
			{
				pm.SendMessage("Target has reached the max level, this doesn't work for them!");
			}
			else
			{
				if (target is Mobile)
				{
					Mobile mt = (Mobile)target;
					mt.SendMessage("You have been awarded {0} EXP points", xp.SCV);
					xmlplayer.kxp += xp.SCV;
					if (xmlplayer.Expp >= xmlplayer.ToLevell && xmlplayer.Levell < xmlplayer.MaxLevel)
                        LevelHandler.DoLevel(pm, new Configured());
				}         
			}		
		}
	}
}