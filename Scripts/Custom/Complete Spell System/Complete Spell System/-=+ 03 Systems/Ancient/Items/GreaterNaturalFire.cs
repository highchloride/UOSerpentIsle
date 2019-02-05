using System;
using Server.Targeting;
using Server.Network;
using Server.Misc;
using Server.Items;
using System.Collections;
using Server.Mobiles;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class GreaterNaturalFire : Item
    {
        private Timer m_Timer;
        private Timer m_Burn;
        private DateTime m_End;
        private Mobile m_Caster;

        public override bool BlocksFit { get { return true; } }

        public GreaterNaturalFire(Point3D loc, Map map, Mobile caster)
            : base(0x19AB)
        {
            Visible = false;
            Movable = false;
            Light = LightType.Circle150;
            MoveToWorld(loc, map);
            m_Caster = caster;

            if (caster.InLOS(this))
                Visible = true;
            else
                Delete();

            if (Deleted)
                return;

            m_Timer = new InternalTimer(this, TimeSpan.FromMinutes(5.0));
            m_Timer.Start();
            m_Burn = new BurnTimer(this, m_Caster);
            m_Burn.Start();

            m_End = DateTime.Now + TimeSpan.FromMinutes(5.0);
        }

        public GreaterNaturalFire(Serial serial)
            : base(serial)
        {
        }

        public override bool OnMoveOver(Mobile m)
        {
            if (Visible && m_Caster != null && SpellHelper.ValidIndirectTarget(m_Caster, m) && m_Caster.CanBeHarmful(m, false))
            {
                m_Caster.DoHarmful(m);

                int damage = Utility.Random(5, 10);

                if (!Core.AOS && m.CheckSkill(SkillName.MagicResist, 0.0, 30.0))
                {
                    damage = Utility.Random(2, 5);

                    m.SendLocalizedMessage(501783); // You feel yourself resisting magical energy.
                }

                AOS.Damage(m, m_Caster, damage, 0, 100, 0, 0, 0);
                m.PlaySound(0x1DD);
            }

            return true;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version

            writer.Write(m_End - DateTime.Now);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    {
                        TimeSpan duration = reader.ReadTimeSpan();

                        m_Timer = new InternalTimer(this, duration);
                        m_Timer.Start();

                        m_End = DateTime.Now + duration;

                        break;
                    }
                case 0:
                    {
                        TimeSpan duration = TimeSpan.FromSeconds(10.0);

                        m_Timer = new InternalTimer(this, duration);
                        m_Timer.Start();

                        m_End = DateTime.Now + duration;

                        break;
                    }
            }
        }

        public override void OnAfterDelete()
        {
            base.OnAfterDelete();

            if (m_Timer != null)
                m_Timer.Stop();
        }

        private class InternalTimer : Timer
        {
            private GreaterNaturalFire m_Item;

            public InternalTimer(GreaterNaturalFire item, TimeSpan duration)
                : base(duration)
            {
                m_Item = item;
            }

            protected override void OnTick()
            {
                m_Item.Delete();
            }
        }

        private class BurnTimer : Timer
        {
            private Item m_FireRing;
            private Mobile m_Caster;
            private DateTime m_Duration;

            private static Queue m_Queue = new Queue();

            public BurnTimer(Item ap, Mobile ca)
                : base(TimeSpan.FromSeconds(0.25), TimeSpan.FromSeconds(0.5))
            {
                Priority = TimerPriority.FiftyMS;

                m_FireRing = ap;
                m_Caster = ca;
                m_Duration = DateTime.Now + TimeSpan.FromSeconds(15.0 + (Utility.RandomDouble() * 15.0));
            }

            protected override void OnTick()
            {
                if (m_FireRing.Deleted)
                    return;

                if (DateTime.Now > m_Duration)
                {

                    Stop();
                }
                else
                {
                    Map map = m_FireRing.Map;

                    if (map != null)
                    {
                        foreach (Mobile m in m_FireRing.GetMobilesInRange(1))
                        {
                            if ((m.Z + 16) > m_FireRing.Z && (m_FireRing.Z + 12) > m.Z)
                                m_Queue.Enqueue(m);
                        }

                        while (m_Queue.Count > 0)
                        {
                            Mobile m = (Mobile)m_Queue.Dequeue();

                            if (m_FireRing.Visible && m_Caster != null && SpellHelper.ValidIndirectTarget(m_Caster, m) && m_Caster.CanBeHarmful(m, false))
                            {
                                m_Caster.DoHarmful(m);

                                int damage = Utility.Random(5, 10);

                                if (!Core.AOS && m.CheckSkill(SkillName.MagicResist, 0.0, 30.0))
                                {
                                    damage = Utility.Random(2, 5);

                                    m.SendLocalizedMessage(501783); // You feel yourself resisting magical energy.
                                }

                                AOS.Damage(m, m_Caster, damage, 0, 100, 0, 0, 0);
                                m.PlaySound(0x1DD);
                                m.SendLocalizedMessage(503000);
                            }
                        }
                    }
                }
            }
        }
    }
}
