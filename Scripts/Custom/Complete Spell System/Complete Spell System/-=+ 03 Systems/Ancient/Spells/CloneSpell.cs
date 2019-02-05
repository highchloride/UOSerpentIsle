using System;
using System.Reflection;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientCloneSpell : AncientSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Clone", "In Quas Xen",
            //SpellCircle.Sixth,
                                                        230,
                                                        9022,
                                                        Reagent.SulfurousAsh,
                                                        Reagent.SpidersSilk,
                                                        Reagent.Bloodmoss,
                                                        Reagent.Ginseng,
                                                        Reagent.Nightshade,
                                                        Reagent.MandrakeRoot
                                                       );
        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public AncientCloneSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if(CheckSequence())
                Caster.Target = new InternalTarget(this);
        }

        public override TimeSpan CastDelayBase
        {
            get { return base.CastDelayBase.Add(TimeSpan.FromSeconds(28)); }
        }

        public override double CastDelayFastScalar
        {
            get { return 5.0; }
        }

        public override TimeSpan GetCastDelay()
        {
            if (Core.AOS)
                return base.GetCastDelay();

            return base.GetCastDelay() + TimeSpan.FromSeconds(6.0);
        }

        public void Target(Mobile m)
        {
            if (!Caster.CanSee(m))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (CheckHSequence(m))
            {
                Mobile source = Caster;
                if (this.Scroll != null)
                    Scroll.Consume();
                SpellHelper.Turn(Caster, m);

                Clone dg = new Clone(m);

                dg.Body = m.Body;

                dg.Hue = m.Hue;
                dg.Name = m.Name;
                dg.SpeechHue = m.SpeechHue;
                dg.Fame = m.Fame;
                dg.Karma = (0 - m.Karma);
                dg.EmoteHue = m.EmoteHue;
                dg.Title = m.Title;
                dg.Criminal = (m.Criminal);
                dg.Str = m.Str;
                dg.Int = m.Int;
                dg.Hits = m.Hits;
                dg.Dex = m.Dex;
                dg.Mana = m.Mana;
                dg.Stam = m.Stam;
                dg.Female = m.Female;
                dg.AccessLevel = m.AccessLevel;

                dg.VirtualArmor = (m.VirtualArmor);
                dg.SetSkill(SkillName.Wrestling, m.Skills[SkillName.Wrestling].Value);
                dg.SetSkill(SkillName.Tactics, m.Skills[SkillName.Tactics].Value);
                dg.SetSkill(SkillName.Anatomy, m.Skills[SkillName.Anatomy].Value);

                dg.SetSkill(SkillName.Magery, m.Skills[SkillName.Magery].Value);
                dg.SetSkill(SkillName.MagicResist, m.Skills[SkillName.MagicResist].Value);
                dg.SetSkill(SkillName.Meditation, m.Skills[SkillName.Meditation].Value);
                dg.SetSkill(SkillName.EvalInt, m.Skills[SkillName.EvalInt].Value);

                dg.SetSkill(SkillName.Archery, m.Skills[SkillName.Archery].Value);
                dg.SetSkill(SkillName.Macing, m.Skills[SkillName.Macing].Value);
                dg.SetSkill(SkillName.Swords, m.Skills[SkillName.Swords].Value);
                dg.SetSkill(SkillName.Fencing, m.Skills[SkillName.Fencing].Value);
                dg.SetSkill(SkillName.Lumberjacking, m.Skills[SkillName.Lumberjacking].Value);
                dg.SetSkill(SkillName.Alchemy, m.Skills[SkillName.Alchemy].Value);
                dg.SetSkill(SkillName.Parry, m.Skills[SkillName.Parry].Value);
                dg.SetSkill(SkillName.Focus, m.Skills[SkillName.Focus].Value);
                dg.SetSkill(SkillName.Necromancy, m.Skills[SkillName.Necromancy].Value);
                dg.SetSkill(SkillName.Chivalry, m.Skills[SkillName.Chivalry].Value);
                dg.SetSkill(SkillName.ArmsLore, m.Skills[SkillName.ArmsLore].Value);
                dg.SetSkill(SkillName.Poisoning, m.Skills[SkillName.Poisoning].Value);
                dg.SetSkill(SkillName.SpiritSpeak, m.Skills[SkillName.SpiritSpeak].Value);
                dg.SetSkill(SkillName.Stealing, m.Skills[SkillName.Stealing].Value);
                dg.SetSkill(SkillName.Inscribe, m.Skills[SkillName.Inscribe].Value);
                dg.Kills = (m.Kills);


                // Clear Items
                RemoveFromAllLayers(dg);

                // Then copy
                CopyFromLayer(m, dg, Layer.FirstValid);
                CopyFromLayer(m, dg, Layer.TwoHanded);
                CopyFromLayer(m, dg, Layer.Shoes);
                CopyFromLayer(m, dg, Layer.Pants);
                CopyFromLayer(m, dg, Layer.Shirt);
                CopyFromLayer(m, dg, Layer.Helm);
                CopyFromLayer(m, dg, Layer.Gloves);
                CopyFromLayer(m, dg, Layer.Ring);
                CopyFromLayer(m, dg, Layer.Talisman);
                CopyFromLayer(m, dg, Layer.Neck);
                CopyFromLayer(m, dg, Layer.Hair);
                CopyFromLayer(m, dg, Layer.Waist);
                CopyFromLayer(m, dg, Layer.InnerTorso);
                CopyFromLayer(m, dg, Layer.Bracelet);
                //CopyFromLayer(m, dg, Layer.Unused_xF);
                CopyFromLayer(m, dg, Layer.FacialHair);
                CopyFromLayer(m, dg, Layer.MiddleTorso);
                CopyFromLayer(m, dg, Layer.Earrings);
                CopyFromLayer(m, dg, Layer.Arms);
                CopyFromLayer(m, dg, Layer.Cloak);
                CopyFromLayer(m, dg, Layer.OuterTorso);
                CopyFromLayer(m, dg, Layer.OuterLegs);
                CopyFromLayer(m, dg, Layer.LastUserValid);
                DupeFromLayer(m, dg, Layer.Mount);
                dg.ControlSlots = 5;
                SpellHelper.Summon(dg, Caster, 0x215, TimeSpan.FromSeconds(4.0 * Caster.Skills[SkillName.Magery].Value), false, false);

            }

            FinishSequence();
        }

        private void CopyFromLayer(Mobile from, Mobile mimic, Layer layer)
        {
            if (from.FindItemOnLayer(layer) != null)
            {
                Item copy = (Item)from.FindItemOnLayer(layer);
                Type t = copy.GetType();

                ConstructorInfo[] info = t.GetConstructors();

                foreach (ConstructorInfo c in info)
                {
                    //if ( !c.IsDefined( typeof( ConstructableAttribute ), false ) ) continue;

                    ParameterInfo[] paramInfo = c.GetParameters();

                    if (paramInfo.Length == 0)
                    {
                        object[] objParams = new object[0];

                        try
                        {
                            Item newItem = null;
                            object o = c.Invoke(objParams);

                            if (o != null && o is Item)
                            {
                                newItem = (Item)o;
                                CopyProperties(newItem, copy);//copy.Dupe( item, copy.Amount );
                                newItem.Parent = null;

                                mimic.EquipItem(newItem);
                            }

                            if (newItem != null)
                            {
                                if (newItem is BaseWeapon && o is BaseWeapon)
                                {
                                    BaseWeapon weapon = newItem as BaseWeapon;
                                    BaseWeapon oweapon = o as BaseWeapon;
                                    weapon.Attributes = oweapon.Attributes;
                                    weapon.WeaponAttributes = oweapon.WeaponAttributes;

                                }
                                if (newItem is BaseArmor && o is BaseArmor)
                                {
                                    BaseArmor armor = newItem as BaseArmor;
                                    BaseArmor oarmor = o as BaseArmor;
                                    armor.Attributes = oarmor.Attributes;
                                    armor.ArmorAttributes = oarmor.ArmorAttributes;
                                    armor.SkillBonuses = oarmor.SkillBonuses;

                                }

                                mimic.EquipItem(newItem);
                            }
                        }
                        catch
                        {
                            from.Say("Error!");
                            return;
                        }
                    }
                }
            }

            if (mimic.FindItemOnLayer(layer) != null && mimic.FindItemOnLayer(layer).LootType != LootType.Blessed)
                mimic.FindItemOnLayer(layer).LootType = LootType.Newbied;
        }

        private void DupeFromLayer(Mobile from, Mobile mimic, Layer layer)
        {
            //if (from.FindItemOnLayer(layer) != null)
            //	mimic.AddItem(from.FindItemOnLayer(layer).Dupe(1));

            if (mimic.FindItemOnLayer(layer) != null && mimic.FindItemOnLayer(layer).LootType != LootType.Blessed)
                mimic.FindItemOnLayer(layer).LootType = LootType.Newbied;
        }

        private static void CopyProperties(Item dest, Item src)
        {
            PropertyInfo[] props = src.GetType().GetProperties();

            for (int i = 0; i < props.Length; i++)
            {
                try
                {
                    if (props[i].CanRead && props[i].CanWrite)
                    {
                        //Console.WriteLine( "Setting {0} = {1}", props[i].Name, props[i].GetValue( src, null ) );
                        props[i].SetValue(dest, props[i].GetValue(src, null), null);
                    }
                }
                catch
                {
                    //Console.WriteLine( "Denied" );
                }
            }
        }

        private void DeleteFromLayer(Mobile from, Layer layer)
        {
            if (from.FindItemOnLayer(layer) != null)
                from.RemoveItem(from.FindItemOnLayer(layer));
        }

        private void RemoveFromAllLayers(Mobile from)
        {
            DeleteFromLayer(from, Layer.FirstValid);
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
            DeleteFromLayer(from, Layer.OuterTorso);
            DeleteFromLayer(from, Layer.OuterLegs);
            DeleteFromLayer(from, Layer.LastUserValid);
            DeleteFromLayer(from, Layer.Mount);
        }

        private class InternalTarget : Target
        {
            private AncientCloneSpell m_Owner;

            public InternalTarget(AncientCloneSpell owner)
                : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                    m_Owner.Target((Mobile)o);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
