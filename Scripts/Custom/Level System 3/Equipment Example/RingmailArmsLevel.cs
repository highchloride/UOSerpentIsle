using System;
using Server.Engines.XmlSpawner2; /* added for level check */
using Server.Mobiles; /* added for level check */

namespace Server.Items
{
    [FlipableAttribute(0x13ee, 0x13ef)]
    public class RingmailArmsLevel : BaseArmor
    {
		private int m_RequiredLevel = 5;
        [CommandProperty(AccessLevel.GameMaster)]
        public int RequiredLevel
        {
            get { return m_RequiredLevel; }
            set { m_RequiredLevel = value; }
        }
		
        [Constructable]
        public RingmailArmsLevel()
            : base(0x13EE)
        {
            this.Weight = 15.0;
        }

        public RingmailArmsLevel(Serial serial)
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
					from.SendMessage( "You do not meet the level requirement for this Armor." );
					return false; 
				}
			}
			return true;
		}

        public override int BasePhysicalResistance
        {
            get
            {
                return 3;
            }
        }
        public override int BaseFireResistance
        {
            get
            {
                return 3;
            }
        }
        public override int BaseColdResistance
        {
            get
            {
                return 1;
            }
        }
        public override int BasePoisonResistance
        {
            get
            {
                return 5;
            }
        }
        public override int BaseEnergyResistance
        {
            get
            {
                return 3;
            }
        }
        public override int InitMinHits
        {
            get
            {
                return 40;
            }
        }
        public override int InitMaxHits
        {
            get
            {
                return 50;
            }
        }
        public override int AosStrReq
        {
            get
            {
                return 40;
            }
        }
        public override int OldStrReq
        {
            get
            {
                return 20;
            }
        }
        public override int OldDexBonus
        {
            get
            {
                return -1;
            }
        }
        public override int ArmorBase
        {
            get
            {
                return 22;
            }
        }
        public override ArmorMaterialType MaterialType
        {
            get
            {
                return ArmorMaterialType.Ringmail;
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
            if (this.Weight == 1.0)
                this.Weight = 15.0;
        }
    }
}