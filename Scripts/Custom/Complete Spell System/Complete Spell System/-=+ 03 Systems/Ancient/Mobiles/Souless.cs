using System;
using System.Collections.Generic;
using Server.Items;
using Server.Targeting;
using Server.Misc;
using Server.Mobiles;

namespace Server.ACC.CSS.Systems.Ancient
{
    [CorpseName("a corpse")]
    public class Souless : BaseCreature
    {
        private Mobile m_Owner;
        private int m_OldBody;

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Owner
        {
            get { return m_Owner; }
            set { m_Owner = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int OldBody
        {
            get { return m_OldBody; }
            set { m_OldBody = value; }
        }

        private AncientPeerSpell spell;

        [Constructable]
        public Souless(AncientPeerSpell m_spell)
            : base(AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Body = 777;
            Title = " The Mystic Lama Herder";
            CantWalk = true;
            spell = m_spell;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (m_Owner != null && m_Owner == from)
            {
                m_Owner.Map = this.Map;
                m_Owner.Location = this.Location;
                m_Owner.BodyValue = m_OldBody;
                m_Owner.Blessed = this.Blessed;
                m_Owner.Direction = this.Direction;
                this.Delete();
                m_Owner.SendMessage("You return to your body");
                if (spell != null)
                {
                    spell.RemovePeerMod();
                }
                if (!m_Owner.CanBeginAction(typeof(AncientPeerSpell)))
                    m_Owner.EndAction(typeof(AncientPeerSpell));
            }
        }

        public override bool OnBeforeDeath()
        {
            if (m_Owner != null)
                m_Owner.Map = this.Map;
            m_Owner.Location = this.Location;
            m_Owner.Blessed = this.Blessed;
            m_Owner.Direction = this.Direction;
            AFKKiller();
            m_Owner.Kill();
            m_Owner.BodyValue = 402;
            Delete();
            return false;
        }

        public void AFKKiller()
        {
            List<Mobile> toGive = new List<Mobile>();

            List<AggressorInfo> list = Aggressors;

            for (int i = 0; i < list.Count; ++i)
            {
                AggressorInfo info = (AggressorInfo)list[i];

                if (info.Attacker.Player && (DateTime.Now - info.LastCombatTime) < TimeSpan.FromSeconds(30.0) && !toGive.Contains(info.Attacker))
                    toGive.Add(info.Attacker);
            }

            list = Aggressed;
            for (int i = 0; i < list.Count; ++i)
            {
                AggressorInfo info = (AggressorInfo)list[i];

                if (info.Defender.Player && (DateTime.Now - info.LastCombatTime) < TimeSpan.FromSeconds(30.0) && !toGive.Contains(info.Defender))
                    toGive.Add(info.Defender);
            }

            if (toGive.Count == 0)
                return;

            for (int i = 0; i < toGive.Count; ++i)
            {
                Mobile m = (Mobile)toGive[i % toGive.Count];

                if (m != null)
                {
                    m.DoHarmful(m_Owner);
                }
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public override bool ClickTitle { get { return false; } }
        public Souless(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_Owner);
            writer.Write(m_OldBody);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Owner = reader.ReadMobile();
            m_OldBody = reader.ReadInt();
        }
    }
}
