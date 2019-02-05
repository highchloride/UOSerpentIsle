using System;
using Server.Targeting;
using Server.Network;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientDestroyTrapSpell : AncientSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Destroy Trap", "An Jux",
                                                        212,
                                                        9001,
                                                        Reagent.Bloodmoss,
                                                        Reagent.SulfurousAsh
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public AncientDestroyTrapSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.Target = new InternalTarget(this);
                Caster.SendMessage("Which trap do you wish to destroy?");
            }
        }

        public void Target(HouseTrap item)
        {
            if (!Caster.CanSee(item))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (CheckSequence())
            {
                SpellHelper.Turn(Caster, item);

                Point3D loc = item.GetWorldLocation();

                Blood shards = new Blood();
                shards.ItemID = 0xC2D;
                shards.Map = item.Map;
                shards.Location = loc;
                Effects.PlaySound(loc, item.Map, 0x305);
                if (item.Placer != null)
                    item.Placer.SendMessage("A trap you placed has been destroyed!");


                item.Delete();
                Caster.SendMessage("You destroy the trap!");
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private AncientDestroyTrapSpell m_Owner;

            public InternalTarget(AncientDestroyTrapSpell owner)
                : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is HouseTrap)
                {
                    m_Owner.Target((HouseTrap)o);
                }
                else
                {
                    from.SendMessage("You can't destroy that");
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
