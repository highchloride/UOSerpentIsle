using System;
using System.Collections;
using Server.Network;
using Server.Items;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Druid
{
    public class DruidSpringOfLifeSpell : DruidSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
              "Spring Of Life", "En Sepa Aete",
            //SpellCircle.Fourth,
              204,
              9061,
              false,
              CReagent.SpringWater
           );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 1.0; } }
        public override double RequiredSkill { get { return 40.0; } }
        public override int RequiredMana { get { return 40; } }

        public DruidSpringOfLifeSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
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
                SpellHelper.Turn(Caster, p);

                SpellHelper.GetSurfaceTop(ref p);

                ArrayList targets = new ArrayList();

                IPooledEnumerable eable = Caster.Map.GetMobilesInRange(new Point3D(p), 3);

                foreach (Mobile m in eable)
                {
                    if (Caster.CanBeBeneficial(m, false))
                        targets.Add(m);
                }

                eable.Free();

                Effects.PlaySound(p, Caster.Map, 0x11);

                int val = (int)(Caster.Skills[CastSkill].Value / 20.0 + 5);

                if (targets.Count > 0)
                {
                    for (int i = 0; i < targets.Count; ++i)
                    {
                        Mobile m = (Mobile)targets[i];

                        if (m.BeginAction(typeof(DruidSpringOfLifeSpell)))
                        {
                            Caster.DoBeneficial(m);
                            m.FixedParticles(0x375A, 9, 20, 5027, EffectLayer.Head);

                            int toHeal = (int)(Caster.Skills[DamageSkill].Value * 0.6);
                            toHeal += Utility.Random(1, 10);

                            m.Heal(toHeal);

                            new InternalTimer(m, Caster, val).Start();
                            m.FixedParticles(0x375A, 9, 20, 5027, EffectLayer.Waist);
                            m.PlaySound(0xAF);
                        }
                    }
                }
            }

            FinishSequence();
        }

        private class InternalTimer : Timer
        {
            private Mobile m_Owner;
            private int m_Val;

            public InternalTimer(Mobile target, Mobile caster, int val)
                : base(TimeSpan.FromSeconds(0))
            {
                double time = caster.Skills[SkillName.AnimalLore].Value * 1.2;
                if (time > 300)
                    time = 300;
                Delay = TimeSpan.FromSeconds(time);
                Priority = TimerPriority.TwoFiftyMS;

                m_Owner = target;
                m_Val = val;
            }

            protected override void OnTick()
            {
                m_Owner.EndAction(typeof(DruidSpringOfLifeSpell));
            }
        }

        private class InternalTarget : Target
        {
            private DruidSpringOfLifeSpell m_Owner;

            public InternalTarget(DruidSpringOfLifeSpell owner)
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
