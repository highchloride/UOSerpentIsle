using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class SkillCoin : Item
    {
        private int m_SKV;

        [CommandProperty(AccessLevel.GameMaster)]
        public int SKV
        {
            get { return m_SKV; }
            set { m_SKV = value; InvalidateProperties(); }
        }
        
        [Constructable]
        public SkillCoin()
            : base(0x1869)
        {
            Name = "A Skill Coin";
            Weight = 1.0;
            LootType = LootType.Blessed;
            m_SKV = Utility.RandomMinMax(1, 5);
			ItemID = 10922;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add("+{0}", m_SKV.ToString(), "Skill Points"); // value: ~1_val~
        }

        public override void OnDoubleClick(Mobile from)
        {
			XMLPlayerLevelAtt xmlplayer = (XMLPlayerLevelAtt)XmlAttach.FindAttachment(from, typeof(XMLPlayerLevelAtt));
            PlayerMobile pm = from as PlayerMobile;

            if (IsChildOf(pm.Backpack))
            {
                if (pm.SkillsTotal >= 700)  //Edit this value based on your servers skill cap
				{
		            pm.SendMessage("You have reached the skill cap, what do you need more skill points for");
					return;
				}
                else
                    xmlplayer.SKPoints += m_SKV;
                    pm.SendMessage("You have been awarded {0} skill points", m_SKV);
                    this.Delete();               
            }
            else
                pm.SendMessage("This must be in your pack!");

        }

        
        public SkillCoin(Serial serial)
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
                        m_SKV = reader.ReadInt();
                        break;
                    }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
            writer.Write((int)SKV);
        }
    }
}