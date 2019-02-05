using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Cleric
{
    public class ClericBanishEvilSpell : ClericSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Banish Evil", "Abigo Malus",
            //SpellCircle.Sixth,
                                                        212,
                                                        9041
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override int RequiredTithing { get { return 30; } }
        public override double RequiredSkill { get { return 60.0; } }

        public ClericBanishEvilSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile m)
        {
            SlayerEntry undead = SlayerGroup.GetEntryByName(SlayerName.Silver);
            SlayerEntry demon = SlayerGroup.GetEntryByName(SlayerName.DaemonDismissal);

            if (!Caster.CanSee(m))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (m is PlayerMobile)
            {
                Caster.SendMessage("You cannot banish another player!");
            }
            else if ((undead != null && !undead.Slays(m)) || (demon != null && !demon.Slays(m)))
            {
                Caster.SendMessage("This spell cannot be used on this type of creature.");
            }
            else if (CheckHSequence(m))
            {
                SpellHelper.Turn(Caster, m);

                m.FixedParticles(0x3709, 10, 30, 5052, 0x480, 0, EffectLayer.LeftFoot);
                m.PlaySound(0x208);

                m.Say("No! I musn't be banished!");
                new InternalTimer(m).Start();
            }

            FinishSequence();
        }

        private class InternalTimer : Timer
        {
            Mobile m_Owner;

            public InternalTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(1.5))
            {
                m_Owner = owner;
            }

            protected override void OnTick()
            {
                if (m_Owner != null && m_Owner.CheckAlive())
                {
                    m_Owner.Delete();
                }
            }
        }

        private class InternalTarget : Target
        {
            private ClericBanishEvilSpell m_Owner;

            public InternalTarget(ClericBanishEvilSpell owner)
                : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                {
                    m_Owner.Target((Mobile)o);
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
