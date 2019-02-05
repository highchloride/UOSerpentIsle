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
    public class AncientEnchantSpell : AncientSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Enchant", "Ort Ylem",
                                                        218,
                                                        9002,
                                                        Reagent.BlackPearl,
                                                        Reagent.MandrakeRoot
                                                       );

        private int m_Hue;
        private string m_Name;

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public AncientEnchantSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                m_Name = null;
                Caster.Target = new InternalTarget(this);
            }
        }

        public override bool CheckCast()
        {
            if (!base.CheckCast())
                return false;

            return SpellHelper.CheckTravel(Caster, TravelCheckType.Mark);
        }

        public void Target(BaseRanged weapon)
        {
            if (!Caster.CanSee(weapon))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (!Caster.CanBeginAction(typeof(AncientEnchantSpell)))
            {
                Caster.SendLocalizedMessage(1005559); // This spell is already in effect.

            }
            else if (CheckSequence())
            {
                if (Caster.BeginAction(typeof(AncientEnchantSpell)))
                {
                    if (this.Scroll != null)
                        Scroll.Consume();
                    m_Hue = weapon.Hue;
                    m_Name = weapon.Name;
                    weapon.Name = "" + m_Name + " [enchanted]";
                    weapon.Hue = 1366;
                    weapon.Attributes.WeaponDamage += 10;
                    weapon.Attributes.AttackChance += 1000;

                    Caster.PlaySound(0x20C);
                    Caster.PlaySound(0x145);
                    Caster.FixedParticles(0x3779, 1, 30, 9964, 3, 3, EffectLayer.Waist);

                    IEntity from = new Entity(Serial.Zero, new Point3D(Caster.X, Caster.Y, Caster.Z), Caster.Map);
                    IEntity to = new Entity(Serial.Zero, new Point3D(Caster.X, Caster.Y, Caster.Z + 50), Caster.Map);
                    Effects.SendMovingParticles(from, to, 0x13B1, 1, 0, false, false, 33, 3, 9501, 1, 0, EffectLayer.Head, 0x100);
                    StopTimer(Caster);

                    Timer t = new InternalTimer(Caster, weapon, m_Hue, m_Name);

                    m_Timers[Caster] = t;

                    t.Start();
                }
                else if (!Caster.CanBeginAction(typeof(AncientEnchantSpell)))
                {
                    DoFizzle();
                }
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private AncientEnchantSpell m_Owner;

            public InternalTarget(AncientEnchantSpell owner)
                : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is BaseRanged)
                {
                    m_Owner.Target((BaseRanged)o);
                }
                else
                {
                    from.SendMessage("That cannot be enchanted."); // I cannot mark that object.
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
            private BaseRanged m_Weapon;
            private int m_weaponhue;
            private string m_Name;

            public InternalTimer(Mobile owner, BaseRanged weapon, int m_Hue, string m_WeaponName)
                : base(TimeSpan.FromSeconds(0))
            {
                m_Owner = owner;
                m_Weapon = weapon;
                m_weaponhue = m_Hue;
                m_Name = m_WeaponName;

                int val = (int)owner.Skills[SkillName.Magery].Value;

                if (val > 100)
                    val = 100;

                Delay = TimeSpan.FromSeconds(val);
                Priority = TimerPriority.TwoFiftyMS;
            }

            protected override void OnTick()
            {
                if (!m_Owner.CanBeginAction(typeof(AncientEnchantSpell)))
                {
                    m_Weapon.Hue = m_weaponhue;
                    m_Weapon.Name = m_Name;
                    m_Weapon.Attributes.WeaponDamage -= 10;
                    m_Weapon.Attributes.AttackChance -= 1000;
                    m_Owner.EndAction(typeof(AncientEnchantSpell));
                }
            }
        }
    }
}
