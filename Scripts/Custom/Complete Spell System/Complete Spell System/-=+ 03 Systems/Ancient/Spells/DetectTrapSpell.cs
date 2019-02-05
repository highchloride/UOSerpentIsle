using System;
using System.Collections;
using Server.Misc;
using Server.Targeting;
using Server.Network;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientDetectTrapSpell : AncientSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Detect Trap", "Wis Jux",
                                                        206,
                                                        9002,
                                                        Reagent.SpidersSilk,
                                                        Reagent.Nightshade
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public AncientDetectTrapSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
                Caster.Target = new InternalTarget(this);
        }

        private static Hashtable m_UnderEffect = new Hashtable();

        private static void RemoveEffect(object state)
        {
            Mobile m = (Mobile)state;

            m_UnderEffect.Remove(m);

            m.SendMessage("You can no longer see hidden traps");
        }

        public static bool UnderEffect(Mobile m)
        {
            return m_UnderEffect.Contains(m);
        }

        public void Target(Mobile m)
        {
            if (!Caster.CanSee(m))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (CheckSequence())
            {
                SpellHelper.Turn(Caster, m);

                Timer t = (Timer)m_UnderEffect[m];

                if (Caster.Player && m.Player && t == null)
                {
                    TimeSpan duration = SpellHelper.GetDuration(Caster, m);
                    m_UnderEffect[m] = t = Timer.DelayCall(duration, new TimerStateCallback(RemoveEffect), m);
                    m.SendMessage("You can now see hidden traps!");
                    Map duck = m.Map;
                    if (duck == Map.Felucca)
                        m.Map = Map.Malas;
                    else
                        m.Map = Map.Felucca;
                    m.Map = duck;
                    m.FixedParticles(0x3818, 10, 15, 5028, EffectLayer.Head);
                    m.PlaySound(0x104);
                }
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private AncientDetectTrapSpell m_Owner;

            public InternalTarget(AncientDetectTrapSpell owner)
                : base(12, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                    m_Owner.Target((Mobile)o);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
