using System;
using System.Collections;
using Server.Network;
using Server.Items;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientGreatLightSpell : AncientSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Great Light", "Vas Lor",
                                                        215,
                                                        9061,
                                                        Reagent.SulfurousAsh,
                                                        Reagent.MandrakeRoot
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public AncientGreatLightSpell(Mobile caster, Item scroll)
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
                SpellHelper.Turn(Caster, p);

                SpellHelper.GetSurfaceTop(ref p);

                ArrayList targets = new ArrayList();
                if (this.Scroll != null)
                    Scroll.Consume();
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

                Effects.PlaySound(p, Caster.Map, 0x299);

                if (targets.Count > 0)
                {
                    for (int i = 0; i < targets.Count; ++i)
                    {
                        Mobile targ = (Mobile)targets[i];

                        if (targ.BeginAction(typeof(LightCycle)))
                        {
                            new LightCycle.NightSightTimer(targ).Start();
                            int level = (int)Math.Abs(LightCycle.DungeonLevel * (Caster.Skills[SkillName.Magery].Base / 100));

                            if (level > 25 || level < 0)
                                level = 25;

                            targ.LightLevel = level;

                            targ.FixedParticles(0x376A, 9, 32, 5007, EffectLayer.Waist);
                            targ.PlaySound(0x1E3);
                        }
                    }
                }
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private AncientGreatLightSpell m_Owner;

            public InternalTarget(AncientGreatLightSpell owner)
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
