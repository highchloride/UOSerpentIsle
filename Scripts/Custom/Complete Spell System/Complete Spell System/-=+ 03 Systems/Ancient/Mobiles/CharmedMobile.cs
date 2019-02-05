using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Misc;
using Server.Mobiles;

namespace Server.ACC.CSS.Systems.Ancient
{
    [CorpseName("a corpse")]
    public class CharmedMobile : BaseCreature
    {
        private BaseCreature m_Owner;
        
        [Constructable]
        public CharmedMobile(BaseCreature owner)
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            owner = m_Owner;
            Body = 777;
            Title = " The Mystic Lama Herder";
        }

        public CharmedMobile(Serial serial)
            : base(serial)
        {
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public BaseCreature Owner
        {
            get { return m_Owner; }
            set { m_Owner = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public override bool ClickTitle { get { return false; } }

        public override void GetProperties(ObjectPropertyList list)
        {
            list.Add(1042971, this.Name);

            list.Add(1049644, "charmed");

        }

        public override bool OnBeforeDeath()
        {
            BaseCreature m_Own = this.m_Owner;

            if (m_Own != null)
            {
                m_Own.Location = this.Location;
                m_Own.Blessed = false;
                m_Own.RevealingAction();
            }
            Delete();
            return false;

        }

        public override void OnAfterDelete()
        {
            BaseCreature m_Own = this.m_Owner;

            if (m_Own != null)
            {
                m_Own.Location = this.Location;
                m_Own.Blessed = false;
                m_Own.RevealingAction();
            }

            base.OnAfterDelete();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_Owner);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Owner = reader.ReadMobile() as BaseCreature;

            Delete();
        }
    }
}
