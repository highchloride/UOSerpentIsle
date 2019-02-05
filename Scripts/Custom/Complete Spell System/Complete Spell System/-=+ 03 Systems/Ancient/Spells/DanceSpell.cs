using System;
using System.Collections;
using Server.Misc;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientDanceSpell : AncientSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Dance", "Por Xen",
                                                        218,
                                                        9031,
                                                        Reagent.Garlic,
                                                        Reagent.Bloodmoss,
                                                        Reagent.MandrakeRoot
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fifth; }
        }

        public AncientDanceSpell(Mobile caster, Item scroll)
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
            else
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
                        if (Core.AOS && m == Caster)
                            continue;

                        if (SpellHelper.ValidIndirectTarget(Caster, m) && Caster.CanSee(m))
                            targets.Add(m);
                    }

                    eable.Free();
                }

                for (int i = 0; i < targets.Count; ++i)
                {
                    Mobile m = (Mobile)targets[i];
                    if (Caster.CanBeHarmful(m, false))
                    {
                        Caster.DoHarmful(m);
                        m.Stam = 0;
                    }
                    m.Freeze(TimeSpan.FromSeconds(4)); //freeze for animation

                    m.Animate(111, 5, 1, true, false, 0); // Do a little dance...


                    m.FixedParticles(0x374A, 10, 15, 5028, EffectLayer.CenterFeet);
                    m.PlaySound(0x1FB);
                }
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private AncientDanceSpell m_Owner;

            public InternalTarget(AncientDanceSpell owner)
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
