using System;
using Server.Targeting;
using Server.Network;
using Server.Misc;
using Server.Items;
using Server.Mobiles;
using Server.Gumps;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Druid
{
    public class DruidRestorativeSoilSpell : DruidSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Restorative Soil", "Ohm Sepa Ante",
            //SpellCircle.Eighth,
                                                        269,
                                                        9020,
                                                        Reagent.Garlic,
                                                        Reagent.Ginseng,
                                                        CReagent.SpringWater
                                                       );
        public override SpellCircle Circle
        {
            get { return SpellCircle.Eighth; }
        }


        public override double CastDelay { get { return 2.0; } }
        public override double RequiredSkill { get { return 89.0; } }
        public override int RequiredMana { get { return 55; } }

        public DruidRestorativeSoilSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }
        
        public override bool CheckCast()
        {
            if (!base.CheckCast())
                return false;

            return true;
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(IPoint3D p)
        {
            if (!Caster.CanSee(p))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (CheckSequence())
            {
                SpellHelper.Turn(Caster, p);

                SpellHelper.GetSurfaceTop(ref p);

                Effects.PlaySound(p, Caster.Map, 0x382);

                Point3D loc = new Point3D(p.X, p.Y, p.Z);
                Item item = new InternalItem(loc, Caster.Map, Caster);
            }

            FinishSequence();
        }

        [DispellableField]
        private class InternalItem : Item
        {
            private Timer m_Timer;
            private DateTime m_End;
            private Mobile m_Owner;

            public override bool BlocksFit { get { return true; } }

            public InternalItem(Point3D loc, Map map, Mobile caster)
                : base(0x913)
            {
                m_Owner = caster;
                Visible = false;
                Movable = false;
                Name = "restorative soil";
                MoveToWorld(loc, map);

                if (caster.InLOS(this))
                    Visible = true;
                else
                    Delete();

                if (Deleted)
                    return;

                m_Timer = new InternalTimer(this, TimeSpan.FromSeconds(30.0));
                m_Timer.Start();

                m_End = DateTime.Now + TimeSpan.FromSeconds(30.0);
            }

            public InternalItem(Serial serial)
                : base(serial)
            {
            }

            public override bool HandlesOnMovement { get { return true; } }
            public override void Serialize(GenericWriter writer)
            {
                base.Serialize(writer);
                writer.Write((int)1); // version
                writer.Write(m_End - DateTime.Now);
            }

            public override void Deserialize(GenericReader reader)
            {
                base.Deserialize(reader);
                int version = reader.ReadInt();
                TimeSpan duration = reader.ReadTimeSpan();

                m_Timer = new InternalTimer(this, duration);
                m_Timer.Start();

                m_End = DateTime.Now + duration;
            }

            public override bool OnMoveOver(Mobile m)
            {
                if (m is PlayerMobile && !m.Alive)
                {
                    m.SendGump(new ResurrectGump(m));

                    m.SendMessage("The power of the soil surges through you!");
                }
                else
                    m.PlaySound(0x339);
                return true;
            }

            public override void OnAfterDelete()
            {
                base.OnAfterDelete();

                if (m_Timer != null)
                    m_Timer.Stop();
            }

            private class InternalTimer : Timer
            {
                private InternalItem m_Item;

                public InternalTimer(InternalItem item, TimeSpan duration)
                    : base(duration)
                {
                    m_Item = item;
                }

                protected override void OnTick()
                {
                    m_Item.Delete();
                }
            }
        }

        private class InternalTarget : Target
        {
            private DruidRestorativeSoilSpell m_Owner;

            public InternalTarget(DruidRestorativeSoilSpell owner)
                : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is IPoint3D)
                    m_Owner.Target((IPoint3D)o);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
