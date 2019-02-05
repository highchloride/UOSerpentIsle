using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientWeatherSpell : AncientSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Weather", "Rel Hur",
                                                        236,
                                                        9011,
                                                        Reagent.SulfurousAsh
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.5; } }
        public override double RequiredSkill { get { return 0.0; } }
        public override int RequiredMana { get { return 7; } }

        public AncientWeatherSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                if (this.Scroll != null)
                    Scroll.Consume();

                Timer t = (Timer)m_UnderEffect[Caster];

                if (Caster != null && t == null)
                {
                    TimeSpan duration = SpellHelper.GetDuration(Caster, Caster);
                    m_UnderEffect[Caster] = t = Timer.DelayCall(duration, new TimerStateCallback(RemoveEffect), Caster);
                    Caster.SendMessage("Clouds swirl and boil above your head!");
                    Caster.FixedParticles(0xC4A, 10, 15, 5028, EffectLayer.Head);
                    Caster.PlaySound(0x16);
                }
            }

            FinishSequence();
        }

        private static Hashtable m_UnderEffect = new Hashtable();

        private static void RemoveEffect(object state)
        {
            Mobile m = (Mobile)state;

            m_UnderEffect.Remove(m);

            m.SendMessage("The magical weather fades away.");
        }

        public static bool UnderEffect(Mobile m)
        {
            return m_UnderEffect.Contains(m);
        }
    }
}
