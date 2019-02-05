using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Misc;
using Server.Items;
using Server.Mobiles;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientIgniteSpell : AncientSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Ignite", "In Flam",
                                                        233,
                                                        9012,
                                                        false,
                                                        Reagent.BlackPearl
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 3.0; } }
        public override double RequiredSkill { get { return 0.0; } }
        public override int RequiredMana { get { return 10; } }

        public AncientIgniteSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
                Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private AncientIgniteSpell m_Owner;

            public InternalTarget(AncientIgniteSpell owner)
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

        public void Target(IPoint3D p)
        {
            if (!Caster.CanSee(p))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {

                SpellHelper.Turn(Caster, p);

                SpellHelper.GetSurfaceTop(ref p);

                Effects.PlaySound(p, Caster.Map, 0x1DD);

                IEntity to = new Entity(Serial.Zero, new Point3D(p), Caster.Map);
                Effects.SendMovingParticles(Caster, to, 0xf53, 1, 0, false, false, 33, 3, 1260, 1, 0, EffectLayer.Head, 0x100);

                Point3D loc = new Point3D(p.X, p.Y, p.Z);

                NaturalFire fire = new NaturalFire(Caster.Location, Caster.Map, Caster);
                fire.MoveToWorld(loc, Caster.Map);
            }

            FinishSequence();
        }
    }
}
