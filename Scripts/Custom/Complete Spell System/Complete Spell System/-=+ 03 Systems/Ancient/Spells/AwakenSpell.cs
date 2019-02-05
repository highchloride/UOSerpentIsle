using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Gumps;
using Server.Spells;
using Server.Targeting;
using Server.Network;
using Server.Regions;


namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientAwakenSpell : AncientSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Awaken", "An Zu",
                                                        218,
                                                        9002,
                                                        Reagent.SulfurousAsh
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 1.0; } }
        public override double RequiredSkill { get { return 0.0; } }
        public override int RequiredMana { get { return 5; } }

        public AncientAwakenSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if(CheckSequence())
                Caster.Target = new InternalTarget(this);
        }

        public void Target(SleepingBody slumber)
        {
            if (!Caster.CanSee(slumber))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }

            else if (CheckSequence())
            {

                if (slumber != null)
                {
                    if (slumber.Owner != null)
                    {
                        slumber.Owner.RevealingAction();
                        slumber.Owner.Frozen = false;
                        slumber.Owner.Squelched = false;
                        slumber.Owner.Map = slumber.Map;
                        slumber.Owner.Location = slumber.Location;
                        slumber.Owner.Animate(21, 6, 1, false, false, 0); ;


                        slumber.Delete();
                        slumber.Owner.SendMessage("You wake up!");
                        Caster.SendMessage("You awaken them!");
                    }
                    else
                        Caster.SendMessage("They are beyond your power to awaken...");
                }
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private AncientAwakenSpell m_Owner;

            public InternalTarget(AncientAwakenSpell owner)
                : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is SleepingBody)
                {
                    m_Owner.Target((SleepingBody)o);
                }
                else
                {
                    from.SendMessage("That cannot be awoken."); // I cannot mark that object.
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();

            }
        }
    }
}
