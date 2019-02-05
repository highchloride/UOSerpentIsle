using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Items;

namespace Server.Mobiles
{
    public class FawnGuardGuard : BaseGuard
    {
        private Timer m_AttackTimer, m_IdleTimer;
        private Mobile m_Focus;


        [Constructable]
        public FawnGuardGuard()
            : this(null)
        { }

        public FawnGuardGuard(Mobile target)
            : base(target)
        {
            InitStats(1000, 1000, 1000);
            Title = "the Fawn guard";

            SpeechHue = Utility.RandomDyedHue();

            SkullCap sc = new SkullCap();
            sc.Hue = 902;
            AddItem(sc);

            ChainChest cc = new ChainChest();
            cc.Hue = 902;
            AddItem(cc);

            Doublet doublet = new Doublet();
            doublet.Hue = 243;
            AddItem(doublet);

            AddItem(new ChainLegs());

            Shoes shoes = new Shoes();
            shoes.Hue = 902;
            AddItem(shoes);

            AddItem(new Broadsword());

            if (Female = Utility.RandomBool())
            {
                Body = 401;
                Name = NameList.RandomName("female");
            }
            else
            {
                Body = 400;
                Name = NameList.RandomName("male");
            }

            Utility.AssignRandomHair(this);

            Container pack = new Backpack();

            pack.Movable = false;

            pack.DropItem(new Gold(10, 25));

            AddItem(pack);

            Skills[SkillName.Anatomy].Base = 120.0;
            Skills[SkillName.Tactics].Base = 120.0;
            Skills[SkillName.Swords].Base = 120.0;
            Skills[SkillName.MagicResist].Base = 120.0;
            Skills[SkillName.DetectHidden].Base = 100.0;

            NextCombatTime = Core.TickCount + 500;
            Focus = target;
        }

        public FawnGuardGuard(Serial serial)
            : base(serial)
        { }

        [CommandProperty(AccessLevel.GameMaster)]
        public override Mobile Focus
        {
            get { return m_Focus; }
            set
            {
                if (Deleted)
                {
                    return;
                }

                Mobile oldFocus = m_Focus;

                if (oldFocus != value)
                {
                    m_Focus = value;

                    if (value != null)
                    {
                        AggressiveAction(value);
                    }

                    Combatant = value;

                    if (oldFocus != null && !oldFocus.Alive)
                    {
                        Say("Thou hast suffered thy punishment, scoundrel.");
                    }

                    if (value != null)
                    {
                        Say(500131); // Thou wilt regret thine actions, swine!
                    }

                    if (m_AttackTimer != null)
                    {
                        m_AttackTimer.Stop();
                        m_AttackTimer = null;
                    }

                    if (m_IdleTimer != null)
                    {
                        m_IdleTimer.Stop();
                        m_IdleTimer = null;
                    }

                    if (m_Focus != null)
                    {
                        m_AttackTimer = new AttackTimer(this);
                        m_AttackTimer.Start();
                        ((AttackTimer)m_AttackTimer).DoOnTick();
                    }
                    else
                    {
                        m_IdleTimer = new IdleTimer(this);
                        m_IdleTimer.Start();
                    }
                }
                else if (m_Focus == null && m_IdleTimer == null)
                {
                    m_IdleTimer = new IdleTimer(this);
                    m_IdleTimer.Start();
                }
            }
        }

        public override bool OnBeforeDeath()
        {
            if (m_Focus != null && m_Focus.Alive)
            {
                new AvengeTimer(m_Focus).Start(); // If a guard dies, three more guards will spawn
            }

            return base.OnBeforeDeath();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version

            writer.Write(m_Focus);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        m_Focus = reader.ReadMobile();

                        if (m_Focus != null)
                        {
                            m_AttackTimer = new AttackTimer(this);
                            m_AttackTimer.Start();
                        }
                        else
                        {
                            m_IdleTimer = new IdleTimer(this);
                            m_IdleTimer.Start();
                        }

                        break;
                    }
            }
        }

        public override void OnAfterDelete()
        {
            if (m_AttackTimer != null)
            {
                m_AttackTimer.Stop();
                m_AttackTimer = null;
            }

            if (m_IdleTimer != null)
            {
                m_IdleTimer.Stop();
                m_IdleTimer = null;
            }

            base.OnAfterDelete();
        }

        private class AvengeTimer : Timer
        {
            private readonly Mobile m_Focus;

            public AvengeTimer(Mobile focus)
                : base(TimeSpan.FromSeconds(2.5), TimeSpan.FromSeconds(1.0), 3)
            {
                m_Focus = focus;
            }

            protected override void OnTick()
            {
                Spawn(m_Focus, m_Focus, 1, true);
            }
        }

        private class AttackTimer : Timer
        {
            private readonly FawnGuardGuard m_Owner;

            public AttackTimer(FawnGuardGuard owner)
                : base(TimeSpan.FromSeconds(0.25), TimeSpan.FromSeconds(0.1))
            {
                m_Owner = owner;
            }

            public void DoOnTick()
            {
                OnTick();
            }

            protected override void OnTick()
            {
                if (m_Owner.Deleted)
                {
                    Stop();
                    return;
                }

                m_Owner.Criminal = false;
                m_Owner.Kills = 0;
                m_Owner.Stam = m_Owner.StamMax;

                Mobile target = m_Owner.Focus;

                if (target != null && (target.Deleted || !target.Alive || !m_Owner.CanBeHarmful(target)))
                {
                    m_Owner.Focus = null;
                    Stop();
                    return;
                }
                else if (m_Owner.Weapon is Fists)
                {
                    m_Owner.Kill();
                    Stop();
                    return;
                }

                if (target != null && m_Owner.Combatant != target)
                {
                    m_Owner.Combatant = target;
                }

                if (target == null)
                {
                    Stop();
                }
                else
                {
                    // <instakill>
                    TeleportTo(target);
                    target.BoltEffect(0);

                    if (target is BaseCreature)
                    {
                        ((BaseCreature)target).NoKillAwards = true;
                    }

                    target.Damage(target.HitsMax, m_Owner);
                    target.Kill(); // just in case, maybe Damage is overriden on some shard

                    if (target.Corpse != null && !target.Player)
                    {
                        target.Corpse.Delete();
                    }

                    m_Owner.Focus = null;
                    Stop();
                } // </instakill>
                  /*else if ( !m_Owner.InRange( target, 20 ) )
                  {
                  m_Owner.Focus = null;
                  }
                  else if ( !m_Owner.InRange( target, 10 ) || !m_Owner.InLOS( target ) )
                  {
                  TeleportTo( target );
                  }
                  else if ( !m_Owner.InRange( target, 1 ) )
                  {
                  if ( !m_Owner.Move( m_Owner.GetDirectionTo( target ) | Direction.Running ) )
                  TeleportTo( target );
                  }
                  else if ( !m_Owner.CanSee( target ) )
                  {
                  if ( !m_Owner.UseSkill( SkillName.DetectHidden ) && Utility.Random( 50 ) == 0 )
                  m_Owner.Say( "Reveal!" );
                  }*/
            }

            private void TeleportTo(Mobile target)
            {
                Point3D from = m_Owner.Location;
                Point3D to = target.Location;

                m_Owner.Location = to;

                Effects.SendLocationParticles(
                    EffectItem.Create(from, m_Owner.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023);
                Effects.SendLocationParticles(EffectItem.Create(to, m_Owner.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 5023);

                m_Owner.PlaySound(0x1FE);
            }
        }

        private class IdleTimer : Timer
        {
            private readonly FawnGuardGuard m_Owner;
            private int m_Stage;

            public IdleTimer(FawnGuardGuard owner)
                : base(TimeSpan.FromSeconds(2.0), TimeSpan.FromSeconds(2.5))
            {
                m_Owner = owner;
            }

            protected override void OnTick()
            {
                if (m_Owner.Deleted)
                {
                    Stop();
                    return;
                }

                if ((m_Stage++ % 4) == 0 || !m_Owner.Move(m_Owner.Direction))
                {
                    m_Owner.Direction = (Direction)Utility.Random(8);
                }

                if (m_Stage > 16)
                {
                    Effects.SendLocationParticles(
                        EffectItem.Create(m_Owner.Location, m_Owner.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023);
                    m_Owner.PlaySound(0x1FE);

                    m_Owner.Delete();
                }
            }
        }
    }
}
