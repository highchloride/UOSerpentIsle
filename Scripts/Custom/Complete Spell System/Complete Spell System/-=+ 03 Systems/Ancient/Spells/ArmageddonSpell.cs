using System;
using System.Collections;
using Server.Network;
using Server.Items;
using Server.Spells;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientArmageddonSpell : AncientSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Armageddon", "Vas Kal An Mani In Corp Hur Tym",
            //SpellCircle.Eighth,
                                                        233,
                                                        9012,
                                                        false,
                                                        Reagent.Bloodmoss,
                                                        Reagent.BlackPearl,
                                                        Reagent.MandrakeRoot,
                                                        Reagent.Nightshade,
                                                        Reagent.Garlic,
                                                        Reagent.Ginseng,
                                                        Reagent.SulfurousAsh,
                                                        Reagent.SpidersSilk
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Eighth; }
        }

        public AncientArmageddonSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                if (this.Scroll != null)
                    Scroll.Consume();

                ArrayList targets = new ArrayList();

                IPooledEnumerable eable = base.Caster.Map.GetMobilesInRange(Caster.Location, 7000);

                foreach (Mobile m in eable)
                {
                    if (Caster != m)
                        targets.Add(m);
                }

                eable.Free();

                Caster.PlaySound(0x2F3);

                for (int i = 0; i < targets.Count; ++i)
                {
                    Mobile m = (Mobile)targets[i];
                    m.BoltEffect(0);
                    double damage = Core.AOS ? m.Hits - (m.Hits / 3.0) : m.Hits * 0.6;

                    damage = 20000.0;

                    Caster.DoHarmful(m);
                    SpellHelper.Damage(TimeSpan.Zero, m, Caster, (double)damage, 20, 20, 20, 20, 20);
                }
            }
        }
    }
}
