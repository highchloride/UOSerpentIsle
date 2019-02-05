using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Gumps;
using Server.Spells;
using Server.Targeting;
using Server.Network;
using Server.Regions;
using Server.Mobiles;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientFalseCoinSpell : AncientSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "False Coin", "Rel Ylem",
                                                        218,
                                                        9002,
                                                        Reagent.Nightshade,
                                                        Reagent.SulfurousAsh
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        private FakeGold m_Fake;

        public AncientFalseCoinSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
                Caster.Target = new InternalTarget(this);
        }

        public override bool CheckCast()
        {
            if (!base.CheckCast())
                return false;

            return SpellHelper.CheckTravel(Caster, TravelCheckType.Mark);
        }

        public void Target(Gold weapon)
        {
            if (!Caster.CanSee(weapon))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }

            else if (CheckSequence())
            {
                if (Caster.BeginAction(typeof(AncientFalseCoinSpell)))
                {
                    if (this.Scroll != null)
                        Scroll.Consume();
                    FakeGold fake = new FakeGold();
                    fake.m_Amount = weapon.Amount * 5;
                    fake.Name = "" + (weapon.Amount * 5) + " Gold Coins";
                    m_Fake = fake;
                    Caster.AddToBackpack(fake);
                    Caster.PlaySound(0x2E6);

                    IEntity from = new Entity(Serial.Zero, new Point3D(Caster.X, Caster.Y, Caster.Z + 50), Caster.Map);
                    IEntity to = new Entity(Serial.Zero, new Point3D(Caster.X, Caster.Y, Caster.Z), Caster.Map);
                    Effects.SendMovingParticles(from, to, 0x1EC6, 1, 0, false, false, 33, 3, 9501, 1, 0, EffectLayer.Head, 0x100);
                    StopTimer(Caster);


                    Timer t = new InternalTimer(Caster, m_Fake);

                    m_Timers[Caster] = t;

                    t.Start();
                }
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private AncientFalseCoinSpell m_Owner;

            public InternalTarget(AncientFalseCoinSpell owner)
                : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Gold)
                {
                    m_Owner.Target((Gold)o);
                }
                else
                {
                    from.SendMessage("That cannot be copied."); // I cannot mark that object.
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
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
            private FakeGold m_Weapon;


            public InternalTimer(Mobile owner, FakeGold weapon)
                : base(TimeSpan.FromSeconds(0))
            {
                m_Owner = owner;
                m_Weapon = weapon;

                int val = (int)owner.Skills[SkillName.Magery].Value;

                if (val > 100)
                    val = 100;

                Delay = TimeSpan.FromSeconds(val);
                Priority = TimerPriority.TwoFiftyMS;
            }

            protected override void OnTick()
            {
                if (!m_Owner.CanBeginAction(typeof(AncientFalseCoinSpell)))
                {
                    if (m_Weapon != null)
                    {
                        m_Weapon.Delete();
                        m_Owner.SendMessage("The forged coins fade away!");
                    }

                    m_Owner.EndAction(typeof(AncientFalseCoinSpell));
                }
            }
        }
    }
}
