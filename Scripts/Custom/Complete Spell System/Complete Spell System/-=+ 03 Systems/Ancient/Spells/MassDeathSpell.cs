using System;
using System.Collections;
using Server.Network;
using Server.Items;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientMassDeathSpell : AncientSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Mass Death", "Vas Corp",
                                                        233,
                                                        9012,
                                                        false,
                                                        Reagent.Bloodmoss,
                                                        Reagent.Ginseng,
                                                        Reagent.Garlic,
                                                        Reagent.MandrakeRoot,
                                                        Reagent.Nightshade
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Eighth; }
        }

        public AncientMassDeathSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override bool DelayedDamage { get { return !Core.AOS; } }

        public override void OnCast()
        {
            if (SpellHelper.CheckTown(Caster, Caster) && CheckSequence())
            {
                if (this.Scroll != null)
                    Scroll.Consume();
                ArrayList targets = new ArrayList();

                Map map = Caster.Map;

                if (map != null)
                {
                    foreach (Mobile m in Caster.GetMobilesInRange(1 + (int)(Caster.Skills[SkillName.Magery].Value / 15.0)))
                    {
                        if (Caster != m && SpellHelper.ValidIndirectTarget(Caster, m) && Caster.CanBeHarmful(m, false) && (!Core.AOS || Caster.InLOS(m)))
                            targets.Add(m);
                    }
                }

                Caster.PlaySound(0x309);

                for (int i = 0; i < targets.Count; ++i)
                {
                    Mobile m = (Mobile)targets[i];

                    double damage = Core.AOS ? m.Hits - (m.Hits / 3.0) : m.Hits * 0.6;

                    if (!m.Player && damage < 10.0)
                        damage = 10.0;
                    else if (damage > (Core.AOS ? 100.0 : 75.0))
                        damage = Core.AOS ? 100.0 : 75.0;

                    Caster.DoHarmful(m);
                    SpellHelper.Damage(TimeSpan.Zero, m, Caster, damage, 100, 0, 0, 0, 0);
                    m.Kill();
                }
            }

            FinishSequence();
        }
    }
}
