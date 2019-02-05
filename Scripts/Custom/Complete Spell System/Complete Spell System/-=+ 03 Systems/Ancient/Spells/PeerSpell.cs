using System;
using System.Collections;
using Server;
using Server.Targeting;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Spells;
using Server.Spells.Fifth;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientPeerSpell : AncientSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Peer", "Vas Wis",
                                                        221,
                                                        9002,
                                                        Reagent.MandrakeRoot,
                                                        Reagent.Nightshade
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        private int m_OldBody;
        private Souless m_Fake;
        public ArrayList m_PeerMod;

        public AncientPeerSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (!CheckSequence())
                return;
            else if (Caster.Mounted)
            {
                Caster.SendLocalizedMessage(1042561); //Please dismount first.
            }
            else if (!Caster.CanBeginAction(typeof(AncientPeerSpell)))
            {
                Caster.SendLocalizedMessage(1005559); // This spell is already in effect.
            }
            else if (Server.Spells.TransformationSpellHelper.UnderTransformation(Caster))
            {
                Caster.SendMessage("Your eyes stay firmly inside your body.");
            }
            else if (DisguiseTimers.IsDisguised(Caster)) //
            {                
                Caster.SendMessage("Your eyes stay firmly inside your body.");
            }
            else if (Caster.BodyMod == 183 || Caster.BodyMod == 184)
            {
                Caster.SendMessage("Your eyes stay firmly inside your body.");
            }
            else if (!Caster.CanBeginAction(typeof(IncognitoSpell)) || Caster.IsBodyMod)
            {
                DoFizzle();
            }
            else if (CheckSequence())
            {
                if (Caster.BeginAction(typeof(AncientPeerSpell)))
                {
                    if (this.Scroll != null)
                        Scroll.Consume();
                    Caster.PlaySound(0x2DF);

                    Caster.SendMessage("Your sight leaves your body.");

                    Souless dg = new Souless(this);

                    dg.Body = Caster.Body;

                    dg.Hue = Caster.Hue;
                    dg.Name = Caster.Name;
                    dg.SpeechHue = Caster.SpeechHue;
                    dg.Fame = Caster.Fame;
                    dg.Karma = Caster.Karma;
                    dg.EmoteHue = Caster.EmoteHue;
                    dg.Title = Caster.Title;
                    dg.Criminal = (Caster.Criminal);
                    dg.AccessLevel = Caster.AccessLevel;
                    dg.Str = Caster.Str;
                    dg.Int = Caster.Int;
                    dg.Hits = Caster.Hits;
                    dg.Dex = Caster.Dex;
                    dg.Mana = Caster.Mana;
                    dg.Stam = Caster.Stam;

                    dg.VirtualArmor = (Caster.VirtualArmor);
                    dg.SetSkill(SkillName.Wrestling, Caster.Skills[SkillName.Wrestling].Value);
                    dg.SetSkill(SkillName.Tactics, Caster.Skills[SkillName.Tactics].Value);
                    dg.SetSkill(SkillName.Anatomy, Caster.Skills[SkillName.Anatomy].Value);

                    dg.SetSkill(SkillName.Magery, Caster.Skills[SkillName.Magery].Value);
                    dg.SetSkill(SkillName.MagicResist, Caster.Skills[SkillName.MagicResist].Value);
                    dg.SetSkill(SkillName.Meditation, Caster.Skills[SkillName.Meditation].Value);
                    dg.SetSkill(SkillName.EvalInt, Caster.Skills[SkillName.EvalInt].Value);

                    dg.SetSkill(SkillName.Archery, Caster.Skills[SkillName.Archery].Value);
                    dg.SetSkill(SkillName.Macing, Caster.Skills[SkillName.Macing].Value);
                    dg.SetSkill(SkillName.Swords, Caster.Skills[SkillName.Swords].Value);
                    dg.SetSkill(SkillName.Fencing, Caster.Skills[SkillName.Fencing].Value);
                    dg.SetSkill(SkillName.Lumberjacking, Caster.Skills[SkillName.Lumberjacking].Value);
                    dg.SetSkill(SkillName.Alchemy, Caster.Skills[SkillName.Alchemy].Value);
                    dg.SetSkill(SkillName.Parry, Caster.Skills[SkillName.Parry].Value);
                    dg.SetSkill(SkillName.Focus, Caster.Skills[SkillName.Focus].Value);
                    dg.SetSkill(SkillName.Necromancy, Caster.Skills[SkillName.Necromancy].Value);
                    dg.SetSkill(SkillName.Chivalry, Caster.Skills[SkillName.Chivalry].Value);
                    dg.SetSkill(SkillName.ArmsLore, Caster.Skills[SkillName.ArmsLore].Value);
                    dg.SetSkill(SkillName.Poisoning, Caster.Skills[SkillName.Poisoning].Value);
                    dg.SetSkill(SkillName.SpiritSpeak, Caster.Skills[SkillName.SpiritSpeak].Value);
                    dg.SetSkill(SkillName.Stealing, Caster.Skills[SkillName.Stealing].Value);
                    dg.SetSkill(SkillName.Inscribe, Caster.Skills[SkillName.Inscribe].Value);
                    dg.Kills = (Caster.Kills);

                    m_PeerMod = new ArrayList();
                    double loss = (0 - Caster.Skills[SkillName.AnimalTaming].Base);
                    SkillMod sk = new DefaultSkillMod(SkillName.AnimalTaming, true, loss);
                    Caster.AddSkillMod(sk);
                    m_PeerMod.Add(sk);
                    double loss1 = (0 - Caster.Skills[SkillName.AnimalLore].Base);
                    SkillMod sk1 = new DefaultSkillMod(SkillName.AnimalLore, true, loss1);// Druidry
                    Caster.AddSkillMod(sk1);
                    m_PeerMod.Add(sk1);

                    double loss3 = (0 - Caster.Skills[SkillName.Necromancy].Base);
                    SkillMod sk3 = new DefaultSkillMod(SkillName.Necromancy, true, loss3);
                    Caster.AddSkillMod(sk3);
                    m_PeerMod.Add(sk3);
                    double loss4 = (0 - Caster.Skills[SkillName.TasteID].Base);
                    SkillMod sk4 = new DefaultSkillMod(SkillName.TasteID, true, loss4);
                    Caster.AddSkillMod(sk4);
                    m_PeerMod.Add(sk4);
                    // Clear Items
                    RemoveFromAllLayers(dg);

                    // Then copy
                    CopyFromLayer(Caster, dg, Layer.FirstValid);
                    CopyFromLayer(Caster, dg, Layer.OneHanded);
                    CopyFromLayer(Caster, dg, Layer.TwoHanded);
                    CopyFromLayer(Caster, dg, Layer.Shoes);
                    CopyFromLayer(Caster, dg, Layer.Pants);
                    CopyFromLayer(Caster, dg, Layer.Shirt);
                    CopyFromLayer(Caster, dg, Layer.Helm);
                    CopyFromLayer(Caster, dg, Layer.Gloves);
                    CopyFromLayer(Caster, dg, Layer.Ring);
                    CopyFromLayer(Caster, dg, Layer.Talisman);
                    CopyFromLayer(Caster, dg, Layer.Neck);
                    CopyFromLayer(Caster, dg, Layer.Hair);
                    CopyFromLayer(Caster, dg, Layer.Waist);
                    CopyFromLayer(Caster, dg, Layer.InnerTorso);
                    CopyFromLayer(Caster, dg, Layer.Bracelet);
                    //CopyFromLayer(Caster, dg, Layer.Unused_xF);
                    CopyFromLayer(Caster, dg, Layer.FacialHair);
                    CopyFromLayer(Caster, dg, Layer.MiddleTorso);
                    CopyFromLayer(Caster, dg, Layer.Earrings);
                    CopyFromLayer(Caster, dg, Layer.Arms);
                    CopyFromLayer(Caster, dg, Layer.Cloak);
                    CopyFromLayer(Caster, dg, Layer.Backpack);
                    CopyFromLayer(Caster, dg, Layer.OuterTorso);
                    CopyFromLayer(Caster, dg, Layer.OuterLegs);
                    CopyFromLayer(Caster, dg, Layer.InnerLegs);
                    CopyFromLayer(Caster, dg, Layer.LastUserValid);
                    CopyFromLayer(Caster, dg, Layer.Mount);

                    dg.Owner = Caster;
                    dg.OldBody = m_OldBody;
                    m_Fake = dg;
                    dg.Map = Caster.Map;
                    dg.Location = Caster.Location;
                    BaseArmor.ValidateMobile(Caster);
                    m_OldBody = Caster.Body;
                    Caster.BodyValue = 903;
                    Caster.Blessed = true;
                    StopTimer(Caster);

                    Timer t = new InternalTimer(Caster, m_OldBody, m_Fake, this);

                    m_Timers[Caster] = t;

                    t.Start();
                }
                else
                {
                    Caster.SendLocalizedMessage(1005559); // This spell is already in effect.
                }
            }

            FinishSequence();
        }

        private void CopyFromLayer(Mobile from, Mobile mimic, Layer layer)
        {
            if (mimic.FindItemOnLayer(layer) != null && mimic.FindItemOnLayer(layer).LootType != LootType.Blessed)
                mimic.FindItemOnLayer(layer).LootType = LootType.Newbied;
        }

        private void DeleteFromLayer(Mobile from, Layer layer)
        {
            if (from.FindItemOnLayer(layer) != null)
                from.RemoveItem(from.FindItemOnLayer(layer));
        }

        private void RemoveFromAllLayers(Mobile from)
        {
            DeleteFromLayer(from, Layer.FirstValid);
            DeleteFromLayer(from, Layer.OneHanded);
            DeleteFromLayer(from, Layer.TwoHanded);
            DeleteFromLayer(from, Layer.Shoes);
            DeleteFromLayer(from, Layer.Pants);
            DeleteFromLayer(from, Layer.Shirt);
            DeleteFromLayer(from, Layer.Helm);
            DeleteFromLayer(from, Layer.Gloves);
            DeleteFromLayer(from, Layer.Ring);
            DeleteFromLayer(from, Layer.Talisman);
            DeleteFromLayer(from, Layer.Neck);
            DeleteFromLayer(from, Layer.Hair);
            DeleteFromLayer(from, Layer.Waist);
            DeleteFromLayer(from, Layer.InnerTorso);
            DeleteFromLayer(from, Layer.Bracelet);
            //DeleteFromLayer(from, Layer.Unused_xF);
            DeleteFromLayer(from, Layer.FacialHair);
            DeleteFromLayer(from, Layer.MiddleTorso);
            DeleteFromLayer(from, Layer.Earrings);
            DeleteFromLayer(from, Layer.Arms);
            DeleteFromLayer(from, Layer.Cloak);
            DeleteFromLayer(from, Layer.Backpack);
            DeleteFromLayer(from, Layer.OuterTorso);
            DeleteFromLayer(from, Layer.OuterLegs);
            DeleteFromLayer(from, Layer.InnerLegs);
            DeleteFromLayer(from, Layer.LastUserValid);
            DeleteFromLayer(from, Layer.Mount);
        }

        public void RemovePeerMod()
        {
            if (m_PeerMod == null)
                return;

            for (int i = 0; i < m_PeerMod.Count; ++i)
                ((SkillMod)m_PeerMod[i]).Remove();
            m_PeerMod = null;
        }
        private static Hashtable m_Timers = new Hashtable();

        public static bool StopTimer(Mobile m)
        {
            Timer t = (Timer)m_Timers[m];

            if (t != null)
            {
                t.Stop();
                m_Timers.Remove(m);
            }

            return (t != null);
        }

        private class InternalTimer : Timer
        {
            private Mobile m_Owner;
            private int m_OldBody;
            private Souless fake;
            private Point3D loc;
            private AncientPeerSpell m_spell;
            public InternalTimer(Mobile owner, int body, Souless m_Fake, AncientPeerSpell spell)
                : base(TimeSpan.FromSeconds(0))
            {
                m_Owner = owner;
                m_OldBody = body;
                fake = m_Fake;
                m_spell = spell;

                int val = (int)owner.Skills[SkillName.Magery].Value;

                if (val > 100)
                    val = 100;
                double loss2 = (0 - m_Owner.Skills[SkillName.Magery].Base);
                SkillMod sk2 = new DefaultSkillMod(SkillName.Magery, true, loss2);
                m_Owner.AddSkillMod(sk2);
                m_spell.m_PeerMod.Add(sk2);

                Delay = TimeSpan.FromSeconds(val);
                Priority = TimerPriority.TwoFiftyMS;
            }

            protected override void OnTick()
            {
                if (!m_Owner.CanBeginAction(typeof(AncientPeerSpell)))
                {
                    if (fake != null && !fake.Deleted)
                    {
                        loc = new Point3D(fake.X, fake.Y, fake.Z);
                        m_Owner.Location = loc;
                        m_Owner.Blessed = fake.Blessed;
                        fake.Delete();

                    }
                    m_Owner.BodyValue = m_OldBody;
                    m_spell.RemovePeerMod();
                    m_Owner.EndAction(typeof(AncientPeerSpell));

                    BaseArmor.ValidateMobile(m_Owner);
                }
            }
        }
    }
}
