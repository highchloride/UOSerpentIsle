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
    public class RogueSlyFoxSpell : RogueSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Sly Fox", " ",
            //SpellCircle.Fourth,
                                                        212,
                                                        9041,
                                                        CReagent.PetrafiedWood,
                                                        Reagent.NoxCrystal,
                                                        Reagent.Nightshade
                                                        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0; } }
        public override double RequiredSkill { get { return 0; } }
        public override int RequiredMana { get { return 0; } }

        private static Dictionary<Mobile, object[]> m_Table = new Dictionary<Mobile, object[]>();

        public RogueSlyFoxSpell(Mobile caster, Item scroll)
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
                Caster.BeginAction(typeof(RogueSlyFoxSpell));
                new InternalTimer(Caster, TimeSpan.FromMinutes(1)).Start();

                object[] mods = new object[]
				{
					new StatMod( StatType.Dex, "SlyFoxSpellStatMod", 20, TimeSpan.Zero ),
					new DefaultSkillMod( SkillName.Hiding, true, 20 ),
					new DefaultSkillMod( SkillName.Stealth, true, 20 )
				};

                m_Table[Caster] = mods;

                Caster.AddStatMod((StatMod)mods[0]);
                Caster.AddSkillMod((SkillMod)mods[1]);
                Caster.AddSkillMod((SkillMod)mods[2]);


                IMount mount = Caster.Mount;

                if (mount != null)
                    mount.Rider = null;

                Caster.BodyMod = 225;
                Caster.PlaySound(0xE5);
                Caster.FixedParticles(0x3728, 1, 13, 0x480, 92, 3, EffectLayer.Head);
            }
            else
                Caster.SendMessage("You cannot become a sly fox in that state!");
        }

        public static void RemoveEffect(Mobile m)
        {
            if (m_Table.ContainsKey(m))
            {
                object[] mods = m_Table[m];

                if (mods != null)
                {
                    m.RemoveStatMod(((StatMod)mods[0]).Name);
                    m.RemoveSkillMod((SkillMod)mods[1]);
                    m.RemoveSkillMod((SkillMod)mods[2]);
                }

                m_Table.Remove(m);

                m.BodyMod = 0;

                m.EndAction(typeof(RogueSlyFoxSpell));
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
                    RogueSlyFoxSpell.RemoveEffect(m_Owner);
                    Stop();
                }
            }
        }
    }
}
