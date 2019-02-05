using System;
using System.Collections;
using Server.Network;
using Server.Items;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientInvisibilityAllSpell : AncientSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Invisibility All", "Vas Sanct Lor",
                                                        215,
                                                        9061,
                                                        Reagent.Nightshade,
                                                        Reagent.Bloodmoss,
                                                        Reagent.BlackPearl,
                                                        Reagent.MandrakeRoot
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Eighth; }
        }

        public AncientInvisibilityAllSpell(Mobile caster, Item scroll)
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
                        if (Caster.CanBeBeneficial(m, false))
                            targets.Add(m);
                    }

                    eable.Free();
                }

                if (targets.Count > 0)
                {
                    for (int i = 0; i < targets.Count; ++i)
                    {
                        Mobile m = (Mobile)targets[i];

                        Caster.DoBeneficial(m);

                        Effects.SendLocationParticles(EffectItem.Create(new Point3D(m.X, m.Y, m.Z + 16), Caster.Map, EffectItem.DefaultDuration), 0x376A, 10, 15, 5045);
                        m.PlaySound(0x3C4);

                        m.Hidden = true;

                        RemoveTimer(m);

                        TimeSpan duration = TimeSpan.FromSeconds(Caster.Skills[SkillName.Magery].Value * 1.2); // 120% of magery

                        Timer t = new InternalTimer(m, duration);

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

            public InternalTimer(Mobile m, TimeSpan duration)
                : base(duration)
            {
                m_Mobile = m;
            }

            protected override void OnTick()
            {
                m_Mobile.RevealingAction();
                RemoveTimer(m_Mobile);
            }
        }

        private class InternalTarget : Target
        {
            private AncientInvisibilityAllSpell m_Owner;

            public InternalTarget(AncientInvisibilityAllSpell owner)
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
