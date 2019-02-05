using System;
using Server.Targeting;
using Server.Network;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientDouseSpell : AncientSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Douse", "An Flam",
                                                        212,
                                                        9001,
                                                        Reagent.Bloodmoss
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 1.0; } }
        public override double RequiredSkill { get { return 0.0; } }
        public override int RequiredMana { get { return 5; } }

        public AncientDouseSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.Target = new InternalTarget(this);
                Caster.SendMessage("What do you wish to douse?");
            }
        }

        public void Target(NaturalFire item)
        {
            if (!Caster.CanSee(item))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }

            else if (CheckSequence())
            {
                SpellHelper.Turn(Caster, item);

                Point3D loc = item.GetWorldLocation();


                Effects.PlaySound(loc, item.Map, 0x11);
                IEntity to = new Entity(Serial.Zero, new Point3D(Caster.X, Caster.Y, Caster.Z), Caster.Map);
                IEntity from = new Entity(Serial.Zero, new Point3D(Caster.X, Caster.Y, Caster.Z + 50), Caster.Map);
                Effects.SendMovingParticles(from, to, 0x376A, 1, 0, false, false, 33, 3, 1263, 1, 0, EffectLayer.Head, 0x100);


                item.Delete();
                Caster.SendMessage("You douse the flames!");
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private AncientDouseSpell m_Owner;

            public InternalTarget(AncientDouseSpell owner)
                : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is NaturalFire)
                {
                    m_Owner.Target((NaturalFire)o);
                }
                else if (o is GreaterNaturalFire)
                {
                    from.SendMessage("Your spell is not powerful enough to douse that!");
                }
                else
                {
                    from.SendMessage("You can't douse that!");
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
