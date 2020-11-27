using System;
using Server.Engines.Craft;
using Server.Engines.XmlSpawner2; /* added for level check */
using Server.Mobiles; /* added for level check */

namespace Server.Items
{
    [FlipableAttribute(0x13FF, 0x13FE)]
    public class KatanaLevel : BaseSword
    {	
		private int m_RequiredLevel = 5;
        [CommandProperty(AccessLevel.GameMaster)]
        public int RequiredLevel
        {
            get { return m_RequiredLevel; }
            set { m_RequiredLevel = value; }
        }
		
        [Constructable]
        public KatanaLevel()
            : base(0x13FF)
        {
            this.Weight = 6.0;
        }

        public KatanaLevel(Serial serial)
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
					from.SendMessage( "You do not meet the level requirement for this weapon." );
					return false; 
				}
			}
			return true;
		}

        public override WeaponAbility PrimaryAbility
        {
            get
            {
                return WeaponAbility.DoubleStrike;
            }
        }
        public override WeaponAbility SecondaryAbility
        {
            get
            {
                return WeaponAbility.ArmorIgnore;
            }
        }
        public override int AosStrengthReq
        {
            get
            {
                return 25;
            }
        }
        public override int AosMinDamage
        {
            get
            {
                return 10;
            }
        }
        public override int AosMaxDamage
        {
            get
            {
                return 14;
            }
        }
        public override int AosSpeed
        {
            get
            {
                return 46;
            }
        }
        public override float MlSpeed
        {
            get
            {
                return 2.50f;
            }
        }
        public override int OldStrengthReq
        {
            get
            {
                return 10;
            }
        }
        public override int OldMinDamage
        {
            get
            {
                return 5;
            }
        }
        public override int OldMaxDamage
        {
            get
            {
                return 26;
            }
        }
        public override int OldSpeed
        {
            get
            {
                return 58;
            }
        }
        public override int DefHitSound
        {
            get
            {
                return 0x23B;
            }
        }
        public override int DefMissSound
        {
            get
            {
                return 0x23A;
            }
        }
        public override int InitMinHits
        {
            get
            {
                return 31;
            }
        }
        public override int InitMaxHits
        {
            get
            {
                return 90;
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