using System;
using System.Collections;
using Server.Network;
using Server.Items;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientMassSleepSpell : AncientSpell
    {
        private SleepingBody m_Body;

        private static SpellInfo m_Info = new SpellInfo(
                                                        "Mass Sleep", "Vas Zu",
                                                        215,
                                                        9061,
                                                        Reagent.Nightshade,
                                                        Reagent.Ginseng,
                                                        Reagent.SpidersSilk
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fifth; }
        }

        public AncientMassSleepSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
                Caster.Target = new InternalTarget(this);
        }

        public void Target(IPoint3D p)
        {
            if (!Caster.CanSee(p))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (CheckSequence())
            {
                if (this.Scroll != null)
                    Scroll.Consume();
                SpellHelper.Turn(Caster, p);

                SpellHelper.GetSurfaceTop(ref p);

                ArrayList targets = new ArrayList();

                Map map = Caster.Map;

                if (map != null)
                {
                    IPooledEnumerable eable = map.GetMobilesInRange(new Point3D(p), 3);

                    foreach (Mobile m in eable)
                    {
                        if (m != Caster && m.AccessLevel <= Caster.AccessLevel)
                            targets.Add(m);
                    }

                    eable.Free();
                }

                if (targets.Count > 0)
                {
                    for (int i = 0; i < targets.Count; ++i)
                    {
                        Mobile m = (Mobile)targets[i];

                        Effects.SendLocationParticles(EffectItem.Create(new Point3D(m.X, m.Y, m.Z + 16), Caster.Map, EffectItem.DefaultDuration), 0x376A, 10, 15, 5045);
                        m.PlaySound(0x3C4);

                        m.Hidden = true;
                        m.Frozen = true;
                        m.Squelched = true;

                        SleepingBody body = new SleepingBody(m, m.Blessed);
                        body.Map = m.Map;
                        body.Location = m.Location;
                        m_Body = body;
                        m.Z -= 100;

                        m.SendMessage("You fall asleep");

                        RemoveTimer(m);

                        TimeSpan duration = TimeSpan.FromSeconds(Caster.Skills[SkillName.Magery].Value * 1.2); // 120% of magery

                        Timer t = new InternalTimer(m, duration, m_Body);

                        m_Table[m] = t;

                        t.Start();
                    }
                }
            }

            FinishSequence();
        }

        private static Hashtable m_Table = new Hashtable();

        public static void RemoveTimer(Mobile m)
        {
            Timer t = (Timer)m_Table[m];

            if (t != null)
            {
                t.Stop();
                m_Table.Remove(m);
            }
        }

        private class InternalTimer : Timer
        {
            private Mobile m_Mobile;
            private SleepingBody m_Body;

            public InternalTimer(Mobile m, TimeSpan duration, SleepingBody body)
                : base(duration)
            {
                m_Mobile = m;
                m_Body = body;
            }

            protected override void OnTick()
            {
                m_Mobile.RevealingAction();
                m_Mobile.Frozen = false;
                m_Mobile.Squelched = false;
                m_Mobile.Z = m_Body.Z;
                m_Mobile.Map = m_Body.Map;
                if (m_Body != null)
                {
                    m_Body.Delete();
                    m_Mobile.SendMessage("You wake up!");
                }
                RemoveTimer(m_Mobile);
            }
        }
        private class InternalTarget : Target
        {
            private AncientMassSleepSpell m_Owner;

            public InternalTarget(AncientMassSleepSpell owner)
                : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                IPoint3D p = o as IPoint3D;

                if (p != null)
                    m_Owner.Target(p);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
