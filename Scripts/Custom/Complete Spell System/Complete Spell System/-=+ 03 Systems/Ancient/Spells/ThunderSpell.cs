using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientThunderSpell : AncientSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Thunder", "Vas Kal",
                                                        236,
                                                        9011,
                                                        Reagent.SulfurousAsh
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.5; } }
        public override double RequiredSkill { get { return 0.0; } }
        public override int RequiredMana { get { return 2; } }

        public AncientThunderSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override bool CheckCast()
        {
            return true;
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                switch (Utility.Random(3))
                {
                    case 0: Caster.PlaySound(0x28); break;
                    case 1: Caster.PlaySound(0x29); break;
                    case 2: Caster.PlaySound(0x206); break;
                }

                if (Caster.Skills[SkillName.Magery].Base >= 50)
                {
                    foreach (Mobile ma in Caster.GetMobilesInRange(5))
                    {
                        BaseCreature m = ma as BaseCreature;
                        if (m != null)
                        {
                            if (!m.Blessed && !m.Controlled && !m.Summoned && m.Alive && m.Int <= (Caster.Int + Caster.Str))
                            {
                                m.FocusMob = Caster;
                                m.AIObject.DoActionFlee();
                            }
                        }
                    }
                }
            }

            FinishSequence();
        }
    }
}
