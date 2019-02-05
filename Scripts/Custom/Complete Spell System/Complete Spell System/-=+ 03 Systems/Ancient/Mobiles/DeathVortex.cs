using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Spells;
using Server.Mobiles;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class DeathVortex : BaseCreature
    {
        private Timer m_Timer;

        [Constructable]
        public DeathVortex()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Death Vortex";
            Body = 573;

            m_Timer = new InternalTimer(this);
            m_Timer.Start();
            AddItem(new LightSource());

            SetStr(50);
            SetDex(200);
            SetInt(100);

            SetHits(70);
            SetStam(250);
            SetMana(0);

            SetDamage(14, 17);

            SetDamageType(ResistanceType.Physical, 0);
            SetDamageType(ResistanceType.Energy, 100);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 90, 100);

            SetSkill(SkillName.MagicResist, 99.9);
            SetSkill(SkillName.Tactics, 90.0);
            SetSkill(SkillName.Wrestling, 100.0);

            Fame = 0;
            Karma = 0;

            VirtualArmor = 40;
            ControlSlots = 1;
        }

        public override Poison PoisonImmune { get { return Poison.Lethal; } }

        public override int GetAngerSound()
        {
            return 0x15;
        }

        public override int GetAttackSound()
        {
            return 0x28;
        }
        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            attacker.BoltEffect(0);
            AOS.Damage(this, attacker, 20, 0, 0, 0, 0, 100);
        }
        public override void OnGaveMeleeAttack(Mobile attacker)
        {
            base.OnGaveMeleeAttack(attacker);

            attacker.BoltEffect(0);
            AOS.Damage(this, attacker, 20, 0, 0, 0, 0, 100);
        }

        public override void AlterDamageScalarFrom(Mobile caster, ref double scalar)
        {
            base.AlterDamageScalarFrom(caster, ref scalar);
            caster.BoltEffect(0);
            AOS.Damage(this, caster, 20, 0, 0, 0, 0, 100);

        }
        public DeathVortex(Serial serial)
            : base(serial)
        {
            m_Timer = new InternalTimer(this);
            m_Timer.Start();
        }

        public override void OnDelete()
        {
            m_Timer.Stop();

            base.OnDelete();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        private class InternalTimer : Timer
        {
            private DeathVortex m_Owner;
            private int m_Count = 0;

            public InternalTimer(DeathVortex owner)
                : base(TimeSpan.FromSeconds(0.1), TimeSpan.FromSeconds(0.1))
            {
                m_Owner = owner;
            }

            protected override void OnTick()
            {
                if ((m_Count++ & 0x3) == 0)
                {
                    m_Owner.Direction = (Direction)(Utility.Random(8) | 0x80);
                }

                m_Owner.Move(m_Owner.Direction);
            }
        }
    }
}
