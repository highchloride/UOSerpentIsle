using System;
using Server.Guilds;
using Server.Engines.Craft;
using Server.Engines.XmlSpawner2; /* added for level check */
using Server.Mobiles; /* added for level check */

namespace Server.Items
{
    public class ChaosShieldLevel : BaseShield
    {
		private int m_RequiredLevel = 5;
        [CommandProperty(AccessLevel.GameMaster)]
        public int RequiredLevel
        {
            get { return m_RequiredLevel; }
            set { m_RequiredLevel = value; }
        }
		
        [Constructable]
        public ChaosShieldLevel()
            : base(0x1BC3)
        {
            if (!Core.AOS)
                this.LootType = LootType.Newbied;

            this.Weight = 5.0;
        }

        public ChaosShieldLevel(Serial serial)
            : base(serial)
        {
        }
		
		public override bool OnEquip(Mobile from)
		{
			XMLPlayerLevelAtt weap1 = (XMLPlayerLevelAtt)XmlAttach.FindAttachment(from, typeof(XMLPlayerLevelAtt));
			if (weap1 != null && weap1.Levell >= RequiredLevel && from is PlayerMobile)
			{		
				return true;
			}
			else
			{
				if (from is PlayerMobile)
				{
					from.SendMessage( "You do not meet the level requirement for this Shield." );
					return false; 
				}
			}
			return true;
		}

        public override int BasePhysicalResistance
        {
            get
            {
                return 1;
            }
        }
        public override int BaseFireResistance
        {
            get
            {
                return 0;
            }
        }
        public override int BaseColdResistance
        {
            get
            {
                return 0;
            }
        }
        public override int BasePoisonResistance
        {
            get
            {
                return 0;
            }
        }
        public override int BaseEnergyResistance
        {
            get
            {
                return 0;
            }
        }
        public override int InitMinHits
        {
            get
            {
                return 100;
            }
        }
        public override int InitMaxHits
        {
            get
            {
                return 125;
            }
        }
        public override int AosStrReq
        {
            get
            {
                return 95;
            }
        }
        public override int ArmorBase
        {
            get
            {
                return 32;
            }
        }
        public override void GetProperties( ObjectPropertyList list )
        {
            base.GetProperties( list );
           
            list.Add( "<BASEFONT COLOR=#7FCAE7>Required Level: <BASEFONT COLOR=#7FCAE7>{0}<BASEFONT COLOR=#FFFFFF>", m_RequiredLevel);
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
			writer.Write((int) m_RequiredLevel);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
			m_RequiredLevel = reader.ReadInt();
            int version = reader.ReadInt();
        }
    }
}