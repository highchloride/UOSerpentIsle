using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Gumps;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientSeanceSpell : AncientSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Seance", "Kal Wis Corp",
                                                        221,
                                                        9002,
                                                        Reagent.Bloodmoss,
                                                        Reagent.SpidersSilk,
                                                        Reagent.MandrakeRoot,
                                                        Reagent.Nightshade,
                                                        Reagent.SulfurousAsh
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        private int m_NewBody;
        private int m_OldBody;

        public AncientSeanceSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override bool CheckCast()
        {
            if (Caster.Mounted)
            {
                Caster.SendLocalizedMessage(1042561); //Please dismount first.
                return false;
            }
            else if (TransformationSpellHelper.UnderTransformation(Caster))
            {
                Caster.SendMessage("You cannot enter the realm of the dead while in that form.");
                return false;
            }
            else if (DisguiseTimers.IsDisguised(Caster))
            {
                Caster.SendMessage("You cannot enter the realm of the dead while disguised.");
                return false;
            }
            else if (Caster.BodyMod == 183 || Caster.BodyMod == 184)
            {
                Caster.SendMessage("You cannot enter the realm of the dead without removing your paint.");
                return false;
            }
            else if (!Caster.CanBeginAction(typeof(AncientSeanceSpell)))
            {
                Caster.SendLocalizedMessage(1005559); // This spell is already in effect.
                return false;
            }
            else if (Caster.Female)
            {
                m_NewBody = 403;

            }
            else
            {
                m_NewBody = 402;


            }
            m_OldBody = Caster.Body;
            return true;
        }

        public override void OnCast()
        {
            if (!CheckSequence())
            {
                return;
            }
            else if (!Caster.CanBeginAction(typeof(AncientSeanceSpell)))
            {
                Caster.SendLocalizedMessage(1005559); // This spell is already in effect.
            }
            else if (TransformationSpellHelper.UnderTransformation(Caster))
            {
                Caster.SendMessage("You cannot enter the realm of the dead while in that form.");
            }
            else if (DisguiseTimers.IsDisguised(Caster))
            {
                Caster.SendMessage("You cannot enter the realm of the dead while disguised.");
            }
            else if (Caster.BodyMod == 183 || Caster.BodyMod == 184)
            {
                Caster.SendMessage("You cannot enter the realm of the dead without removing your paint.");
            }
            else if (!Caster.CanBeginAction(typeof(Server.Spells.Fifth.IncognitoSpell)) || Caster.IsBodyMod)
            {
                DoFizzle();
            }
            else if (CheckSequence())
            {

                if (Caster.BeginAction(typeof(AncientSeanceSpell)))
                {
                    if (m_NewBody != 0)
                    {
                        if (this.Scroll != null)
                            Scroll.Consume();
                        Caster.PlaySound(0x379);

                        Caster.BodyValue = m_NewBody;

                        Caster.SendMessage("You enter the realm of the dead.");
                        BaseArmor.ValidateMobile(Caster);

                        StopTimer(Caster);

                        Timer t = new InternalTimer(Caster, m_OldBody);

                        m_Timers[Caster] = t;

                        t.Start();
                    }
                }
                else
                {
                    Caster.SendLocalizedMessage(1005559); // This spell is already in effect.
                }
            }

            FinishSequence();
        }

        private static Hashtable m_Timers = new Hashtable();

        public static bool StopTimer(Mobile m)
        {
            Timer t = (Timer)m_Timers[m];

            if (t != null)
            {
                t.Stop();
                m_Timers.Remove(m);
            }

            return (t != null);
        }

        private class InternalTimer : Timer
        {
            private Mobile m_Owner;
            private int m_OldBody;

            public InternalTimer(Mobile owner, int body)
                : base(TimeSpan.FromSeconds(0))
            {
                m_Owner = owner;
                m_OldBody = body;

                int val = (int)owner.Skills[SkillName.Magery].Value;

                if (val > 100)
                    val = 100;

                Delay = TimeSpan.FromSeconds(val);
                Priority = TimerPriority.TwoFiftyMS;
            }

            protected override void OnTick()
            {
                if (!m_Owner.CanBeginAction(typeof(AncientSeanceSpell)))
                {
                    m_Owner.BodyValue = m_OldBody;
                    m_Owner.EndAction(typeof(AncientSeanceSpell));

                    BaseArmor.ValidateMobile(m_Owner);
                }
            }
        }
    }
}
