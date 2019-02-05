using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.Spells.Seventh;
using Server.Gumps;

namespace Server.ACC.CSS.Systems.Rogue
{
    public class RogueIntimidationSpell : RogueSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Intimidation", " ",
            //SpellCircle.Fourth,
                                                        212,
                                                        9041
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0; } }
        public override double RequiredSkill { get { return 0; } }
        public override int RequiredMana { get { return 0; } }

        private static Dictionary<Mobile, object[]> m_Table = new Dictionary<Mobile, object[]>();

        public RogueIntimidationSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override bool CheckCast()
        {
            if (!TransformationSpellHelper.CheckCast(Caster, this))
                return false;

            return base.CheckCast();
        }

        public override void OnCast()
        {
            if (Caster.CanBeginAction(typeof(RogueSlyFoxSpell)) && CheckSequence())
            {
                Caster.BeginAction(typeof(RogueIntimidationScroll));
                new InternalTimer(Caster, TimeSpan.FromMinutes(1)).Start();

                object[] mods = new object[]
				{
					new StatMod( StatType.Dex, "RogueIntimidationSpellDexMod", -20, TimeSpan.Zero ),
					new StatMod( StatType.Str, "RogueIntimidationSpellStrMod", 20, TimeSpan.Zero ),
					new DefaultSkillMod( SkillName.Hiding, true, -20 ),
					new DefaultSkillMod( SkillName.Stealth, true, -20 ),
					new DefaultSkillMod( SkillName.Swords, true, 20 ),
					new DefaultSkillMod( SkillName.Macing, true, 20 ),
					new DefaultSkillMod( SkillName.Fencing, true, -20 )

				};

                m_Table[Caster] = mods;

                Caster.AddStatMod((StatMod)mods[0]);
                Caster.AddStatMod((StatMod)mods[1]);
                Caster.AddSkillMod((SkillMod)mods[2]);
                Caster.AddSkillMod((SkillMod)mods[3]);
                Caster.AddSkillMod((SkillMod)mods[4]);
                Caster.AddSkillMod((SkillMod)mods[5]);
                Caster.AddSkillMod((SkillMod)mods[6]);
            }
            else
                Caster.SendMessage("You cannot intimidate someone in that state!");
        }
        public static void RemoveEffect(Mobile m)
        {
            if (m_Table.ContainsKey(m))
            {
                object[] mods = (object[])m_Table[m];

                if (mods != null)
                {
                    m.RemoveStatMod(((StatMod)mods[0]).Name);
                    m.RemoveStatMod(((StatMod)mods[1]).Name);
                    m.RemoveSkillMod((SkillMod)mods[2]);
                    m.RemoveSkillMod((SkillMod)mods[3]);
                    m.RemoveSkillMod((SkillMod)mods[4]);
                    m.RemoveSkillMod((SkillMod)mods[5]);
                    m.RemoveSkillMod((SkillMod)mods[6]);
                }

                m_Table.Remove(m);

                m.EndAction(typeof(RogueIntimidationSpell));
            }
        }

        private class InternalTimer : Timer
        {
            private Mobile m_Owner;
            private DateTime m_Expire;

            public InternalTimer(Mobile owner, TimeSpan duration)
                : base(TimeSpan.Zero, TimeSpan.FromSeconds(1))
            {
                m_Owner = owner;
                m_Expire = DateTime.Now + duration;

            }

            protected override void OnTick()
            {
                if (DateTime.Now >= m_Expire)
                {
                    RogueIntimidationSpell.RemoveEffect(m_Owner);
                    Stop();
                }
            }
        }
    }
}
