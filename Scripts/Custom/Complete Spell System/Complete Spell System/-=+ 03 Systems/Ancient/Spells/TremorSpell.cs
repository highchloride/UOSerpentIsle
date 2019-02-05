using System;
using System.Collections;
using Server.Network;
using Server.Items;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientTremorSpell : AncientSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Tremor", "Vas Por Ylem",
                                                        233,
                                                        9012,
                                                        false,
                                                        Reagent.Bloodmoss,
                                                        Reagent.MandrakeRoot,
                                                        Reagent.SulfurousAsh
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public AncientTremorSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override bool DelayedDamage { get { return !Core.AOS; } }

        public override void OnCast()
        {
            if (SpellHelper.CheckTown(Caster, Caster) && CheckSequence())
            {
                ArrayList targets = new ArrayList();

                Map map = Caster.Map;

                if (map != null)
                {
                    foreach (Mobile m in Caster.GetMobilesInRange(1 + (int)(Caster.Skills[SkillName.Magery].Value / 15.0)))
                    {
                        if (Caster != m && m.AccessLevel == AccessLevel.Player && SpellHelper.ValidIndirectTarget(Caster, m) && Caster.CanBeHarmful(m, false) && (!Core.AOS || Caster.InLOS(m)))
                            targets.Add(m);
                    }
                }

                Caster.PlaySound(0x2F3);

                for (int i = 0; i < targets.Count; ++i)
                {
                    Mobile m = (Mobile)targets[i];

                    m.Stam = 0;
                    m.Mana = 0;
                    m.SendMessage("A powerful tremor in the ground makes you stumble and tremble!");
                    int damage = (Utility.Random(10, 15));
                    Caster.DoHarmful(m);
                    SpellHelper.Damage(TimeSpan.Zero, m, Caster, damage, 100, 0, 0, 0, 0);
                }
            }

            FinishSequence();
        }
    }
}
