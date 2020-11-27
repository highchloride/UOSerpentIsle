/**************************** OpenPropertyList.cs **************************
 *  
 *  Allows you to use Item Properties as localized strings
 *  
/***************************************************************************
 *
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation; either version 2 of the License, or
 *   (at your option) any later version.
 *
 ***************************************************************************/
using System;
using System.Collections.Generic;
using Server.Spells;
using Server.Spells.Ninjitsu;
using Server.Spells.Spellweaving;
using Server.Items;

namespace Server
{
    public sealed class OpenProperty
    {
        private int m_Number;
        private string m_Arguments;

        public int Number { get { return m_Number; } set { m_Number = value; } }
        public string Arguments { get { return m_Arguments; } set { m_Arguments = value; } }

        public OpenProperty(int number, string arguments)
        {
            m_Number = number;
            m_Arguments = arguments;
        }
    }

    public sealed class OpenPropertyList
    {
        private List<OpenProperty> m_PropList;
        public List<OpenProperty> PropList { get { return m_PropList; } set { m_PropList = value; } }

        public OpenPropertyList()
        {
            m_PropList = new List<OpenProperty>();
        }

        public void Add(int number)
        {
            if (number == 0)
            {
                return;
            }
            Add(number, "");
        }

        public void Add(int number, string format, object arg0)
        {
            Add(number, String.Format(format, arg0));
        }

        public void Add(int number, string format, object arg0, object arg1)
        {
            Add(number, String.Format(format, arg0, arg1));
        }

        public void Add(int number, string format, object arg0, object arg1, object arg2)
        {
            Add(number, String.Format(format, arg0, arg1, arg2));
        }

        public void Add(int number, string format, params object[] args)
        {
            Add(number, String.Format(format, args));
        }

        public void Add(int number, string arguments)
        {
            if (number == 0)
            {
                return;
            }

            if (arguments == null)
            {
                arguments = "";
            }

            m_PropList.Add(new OpenProperty(number, arguments));
        }
    }

    public sealed class AllPropertyList
    {
        public AllPropertyList()
        {
        }

        public OpenPropertyList GetOpenProperties(Item item)
        {
            if (item is BaseWeapon)
                return GetOpenProperties(item as BaseWeapon);
            if (item is BaseClothing)
                return GetOpenProperties(item as BaseClothing);
            if (item is BaseJewel)
                return GetOpenProperties(item as BaseJewel);
            if (item is Spellbook)
                return GetOpenProperties(item as Spellbook);
            if (item is BaseQuiver)
                return GetOpenProperties(item as BaseQuiver);
            if (item is BaseArmor)
                return GetOpenProperties(item as BaseArmor);
            if (item is BaseInstrument)
                return GetOpenProperties(item as BaseInstrument);
            if (item is BaseTalisman)
                return GetOpenProperties(item as BaseTalisman);

            return AddNameProperties(item);
        }

        public OpenPropertyList AddNameProperties(Item item)
        {
            OpenPropertyList list = new OpenPropertyList();

            if (item.IsSecure)
            {
                list.Add(501644); // locked down & secure
            }
            else if (item.IsLockedDown)
            {
                list.Add(501643); // locked down
            }

            Mobile blessedFor = item.BlessedFor;

            if (blessedFor != null && !blessedFor.Deleted)
            {
                list.Add(1062203, "{0}", blessedFor.Name); // Blessed for ~1_NAME~
            }

            if (item.DisplayLootType)
            {
                if (item.LootType == LootType.Blessed)
                {
                    list.Add(1038021); // blessed
                }
                else if (item.LootType == LootType.Cursed)
                {
                    list.Add(1049643); // cursed
                }
                else if (item.Insured)
                {
                    list.Add(1061682); // <b>insured</b>
                }
            }

            if (item.DisplayWeight)
            {
                int weight = item.PileWeight + item.TotalWeight;

                if (weight == 1)
                {
                    list.Add(1072788, weight.ToString()); //Weight: ~1_WEIGHT~ stone
                }
                else
                {
                    list.Add(1072789, weight.ToString()); //Weight: ~1_WEIGHT~ stones
                }
            }

            if (item.QuestItem)
            {
                list.Add(1072351); // Quest Item
            }

            return list;
        }

        public OpenPropertyList GetOpenProperties(BaseArmor armor)
        {
            OpenPropertyList list = new OpenPropertyList();

            #region Stygian Abyss
            if (armor.IsImbued == true)
                list.Add(1080418); // (Imbued)

            if (armor.GorgonLenseCharges > 0)
                list.Add(1112590, armor.GorgonLenseCharges.ToString()); //Gorgon Lens Charges: ~1_val~
            #endregion

            if (armor.Crafter != null)
                list.Add(1050043, armor.Crafter.TitleName); // crafted by ~1_NAME~

            #region Factions
            if (armor.FactionItemState != null)
                list.Add(1041350); // faction item
            #endregion

            #region Mondain's Legacy Sets
            if (armor.IsSetItem)
            {
                if (armor.MixedSet)
                    list.Add(1073491, armor.Pieces.ToString()); // Part of a Weapon/Armor Set (~1_val~ pieces)
                else
                    list.Add(1072376, armor.Pieces.ToString()); // Part of an Armor Set (~1_val~ pieces)

                if (armor.SetEquipped)
                {
                    if (armor.MixedSet)
                        list.Add(1073492); // Full Weapon/Armor Set Present
                    else
                        list.Add(1072377); // Full Armor Set Present

                    GetSetProperties(list, armor);
                }
            }
            #endregion

            if (armor.RequiredRace == Race.Elf)
                list.Add(1075086); // Elves Only
            else if (armor.RequiredRace == Race.Gargoyle)
                list.Add(1111709); // Gargoyles Only

            if (armor is SurgeShield && ((SurgeShield)armor).Surge > SurgeType.None)
                list.Add(1116176 + ((int)((SurgeShield)armor).Surge));

            if (armor.NegativeAttributes != null)
                GetNegativeProperties(list, armor.NegativeAttributes);

            if (armor.SkillBonuses != null)
            {
                for (int i = 0; i < 5; ++i)
                {
                    SkillName skill;
                    double skillbonus;

                    if (!armor.SkillBonuses.GetValues(i, out skill, out skillbonus))
                        continue;

                    list.Add(1060451 + i, "#{0}\t{1}", AosSkillBonuses.GetLabel(skill), skillbonus);
                }
            }

            GetSAAbsorptionProperties(list, armor.AbsorptionAttributes);
            GetAosAttributeProperties(list, armor.Attributes);

            int prop;

            if ((prop = armor.ArtifactRarity) > 0)
                list.Add(1061078, prop.ToString()); // artifact rarity ~1_val~

            if ((prop = armor.GetLowerStatReq()) != 0)
                list.Add(1060435, prop.ToString()); // lower requirements ~1_val~%

            if ((prop = (armor.GetLuckBonus() + armor.Attributes.Luck)) != 0)
                list.Add(1060436, prop.ToString()); // luck ~1_val~

            if ((prop = armor.ArmorAttributes.MageArmor) != 0)
                list.Add(1060437); // mage armor

            if ((prop = armor.ArmorAttributes.SelfRepair) != 0)
                list.Add(1060450, prop.ToString()); // self repair ~1_val~

            if (armor is SurgeShield && ((SurgeShield)armor).Surge > SurgeType.None)
                list.Add(1153098, ((SurgeShield)armor).Charges.ToString());

            GetResistanceProperties(list, armor);

            if ((prop = armor.GetDurabilityBonus()) > 0)
                list.Add(1060410, prop.ToString()); // durability ~1_val~%

            if ((prop = armor.ComputeStatReq(StatType.Str)) > 0)
                list.Add(1061170, prop.ToString()); // strength requirement ~1_val~

            if (armor.HitPoints >= 0 && armor.MaxHitPoints > 0)
                list.Add(1060639, "{0}\t{1}", armor.HitPoints, armor.MaxHitPoints); // durability ~1_val~ / ~2_val~

            if (armor.IsSetItem && !armor.SetEquipped)
            {
                list.Add(1072378); // <br>Only when full set is present:				
                GetSetProperties(list, armor);
            }

            if (armor.ItemPower != ItemPower.None)
            {
                if (armor.ItemPower <= ItemPower.LegendaryArtifact)
                    list.Add(1151488 + ((int)armor.ItemPower - 1));
                else
                    list.Add(1152281 + ((int)armor.ItemPower - 9));
            }

            return list;
        }

        public OpenPropertyList GetOpenProperties(BaseWeapon weapon)
        {
            OpenPropertyList list = new OpenPropertyList();

            if (weapon.TimesImbued > 0)
            {
                list.Add(1080418); // (Imbued)
            }

            if (weapon.Crafter != null)
            {
                list.Add(1050043, weapon.Crafter.TitleName); // crafted by ~1_NAME~
            }

            #region Factions
            if (weapon.FactionItemState != null)
            {
                list.Add(1041350); // faction item
            }
            #endregion

            #region Mondain's Legacy Sets
            if (weapon.IsSetItem)
            {
                list.Add(1073491, weapon.Pieces.ToString()); // Part of a Weapon/Armor Set (~1_val~ pieces)

                if (weapon.SetEquipped)
                {
                    list.Add(1073492); // Full Weapon/Armor Set Present			
                    GetSetProperties(list, weapon);
                }
            }
            #endregion

            if (weapon.Attributes.Brittle != 0)
            {
                list.Add(1116209); // Brittle
            }

            if (weapon.SkillBonuses != null)
            {
                for (int i = 0; i < 5; ++i)
                {
                    SkillName skill;
                    double skillbonus;

                    if (!weapon.SkillBonuses.GetValues(i, out skill, out skillbonus))
                        continue;

                    list.Add(1060451 + i, "#{0}\t{1}", AosSkillBonuses.GetLabel(skill), skillbonus);
                }
            }

            GetSAAbsorptionProperties(list, weapon.AbsorptionAttributes);
            GetAosAttributeProperties(list, weapon.Attributes);

            if (weapon.Quality == ItemQuality.Exceptional)
            {
                list.Add(1060636); // exceptional
            }

            if (weapon.RequiredRace == Race.Elf)
            {
                list.Add(1075086); // Elves Only
            }

            #region Stygian Abyss
            else if (weapon.RequiredRace == Race.Gargoyle)
            {
                list.Add(1111709); // Gargoyles Only
            }
            #endregion

            if (weapon.ArtifactRarity > 0)
            {
                list.Add(1061078, weapon.ArtifactRarity.ToString()); // artifact rarity ~1_val~
            }

            if (weapon is IUsesRemaining && ((IUsesRemaining)weapon).ShowUsesRemaining)
            {
                list.Add(1060584, ((IUsesRemaining)weapon).UsesRemaining.ToString()); // uses remaining: ~1_val~
            }

            if (weapon.Poison != null && weapon.PoisonCharges > 0)
            {
                #region Mondain's Legacy mod
                list.Add(weapon.Poison.LabelNumber, weapon.PoisonCharges.ToString());
                #endregion
            }

            if (weapon.Slayer != SlayerName.None)
            {
                SlayerEntry entry = SlayerGroup.GetEntryByName(weapon.Slayer);
                if (entry != null)
                {
                    list.Add(entry.Title);
                }
            }

            if (weapon.Slayer2 != SlayerName.None)
            {
                SlayerEntry entry = SlayerGroup.GetEntryByName(weapon.Slayer2);
                if (entry != null)
                {
                    list.Add(entry.Title);
                }
            }

            #region Mondain's Legacy
            if (weapon.Slayer3 != TalismanSlayerName.None)
            {
                if (weapon.Slayer3 == TalismanSlayerName.Wolf)
                {
                    list.Add(1075462);
                }
                else if (weapon.Slayer3 == TalismanSlayerName.Goblin)
                {
                    list.Add(1095010);
                }
                else if (weapon.Slayer3 == TalismanSlayerName.Undead)
                {
                    list.Add(1060479);
                }
                else
                {
                    list.Add(1072503 + (int)weapon.Slayer3);
                }
            }
            #endregion

            GetResistanceProperties(list, weapon);

            double focusBonus = 1;
            int enchantBonus = 0;
            bool fcMalus = false;
            int damBonus = 0;
            SpecialMove move = null;
            AosWeaponAttribute bonus = AosWeaponAttribute.HitColdArea;

            #region Focus Attack
            if (weapon.FocusWeilder != null)
            {
                move = SpecialMove.GetCurrentMove(weapon.FocusWeilder);

                if (move is FocusAttack)
                {
                    focusBonus = move.GetPropertyBonus(weapon.FocusWeilder);
                    damBonus = (int)(move.GetDamageScalar(weapon.FocusWeilder, null) * 100) - 100;
                }
            }
            #endregion

            #region Stygian Abyss
            var wielder = weapon.EnchantedWeilder;
            if (wielder != null)
            {
                if (Server.Spells.Mysticism.EnchantSpell.IsUnderSpellEffects(wielder, weapon))
                {
                    bonus = Server.Spells.Mysticism.EnchantSpell.BonusAttribute(wielder);
                    enchantBonus = Server.Spells.Mysticism.EnchantSpell.BonusValue(wielder);
                    fcMalus = Server.Spells.Mysticism.EnchantSpell.CastingMalus(wielder, weapon);
                }
            }
            #endregion

            int prop;
            double fprop;

            if (Core.ML && weapon is BaseRanged && ((BaseRanged)weapon).Balanced)
            {
                list.Add(1072792); // Balanced
            }

            #region Stygian Abyss
            if ((prop = weapon.WeaponAttributes.BloodDrinker) != 0)
            {
                list.Add(1113591, prop.ToString()); // Blood Drinker
            }

            if ((prop = weapon.WeaponAttributes.BattleLust) != 0)
            {
                list.Add(1113710, prop.ToString()); // Battle Lust
            }

            if ((prop = weapon.WeaponAttributes.ReactiveParalyze) != 0)
            {
                list.Add(1112364); // reactive paralyze
            }

            if ((prop = weapon.WeaponAttributes.SplinteringWeapon) != 0)
            {
                list.Add(1112857, prop.ToString()); //splintering weapon ~1_val~%
            }
            #endregion

            if ((prop = weapon.WeaponAttributes.UseBestSkill) != 0)
            {
                list.Add(1060400); // use best weapon skill
            }

            if ((prop = (weapon.GetDamageBonus() + weapon.Attributes.WeaponDamage + damBonus)) != 0)
            {
                list.Add(1060401, prop.ToString()); // damage increase ~1_val~%
            }

            if ((prop = (weapon.GetHitChanceBonus() + weapon.Attributes.AttackChance)) != 0)
            {
                list.Add(1060415, prop.ToString()); // hit chance increase ~1_val~%
            }

            if ((fprop = (double)weapon.WeaponAttributes.HitColdArea * focusBonus) != 0)
            {
                list.Add(1060416, ((int)fprop).ToString()); // hit cold area ~1_val~%
            }

            if ((fprop = (double)weapon.WeaponAttributes.HitDispel * focusBonus) != 0)
            {
                list.Add(1060417, ((int)fprop).ToString()); // hit dispel ~1_val~%
            }
            else if (bonus == AosWeaponAttribute.HitDispel && enchantBonus != 0)
            {
                list.Add(1060417, ((int)(enchantBonus * focusBonus)).ToString()); // hit dispel ~1_val~%
            }

            if ((fprop = (double)weapon.WeaponAttributes.HitEnergyArea * focusBonus) != 0)
            {
                list.Add(1060418, ((int)fprop).ToString()); // hit energy area ~1_val~%
            }

            if ((fprop = (double)weapon.WeaponAttributes.HitFireArea * focusBonus) != 0)
            {
                list.Add(1060419, ((int)fprop).ToString()); // hit fire area ~1_val~%
            }

            if ((fprop = (double)weapon.WeaponAttributes.HitFireball * focusBonus) != 0)
            {
                list.Add(1060420, ((int)fprop).ToString()); // hit fireball ~1_val~%
            }
            else if (bonus == AosWeaponAttribute.HitFireball && enchantBonus != 0)
            {
                list.Add(1060420, ((int)((double)enchantBonus * focusBonus)).ToString()); // hit fireball ~1_val~%
            }

            if ((fprop = (double)weapon.WeaponAttributes.HitHarm * focusBonus) != 0)
            {
                list.Add(1060421, ((int)fprop).ToString()); // hit harm ~1_val~%
            }
            else if (bonus == AosWeaponAttribute.HitHarm && enchantBonus != 0)
            {
                list.Add(1060421, ((int)(enchantBonus * focusBonus)).ToString()); // hit harm ~1_val~%
            }

            if ((fprop = (double)weapon.WeaponAttributes.HitLeechHits * focusBonus) != 0)
            {
                list.Add(1060422, ((int)fprop).ToString()); // hit life leech ~1_val~%
            }

            if ((fprop = (double)weapon.WeaponAttributes.HitLightning * focusBonus) != 0)
            {
                list.Add(1060423, ((int)fprop).ToString()); // hit lightning ~1_val~%
            }
            else if (bonus == AosWeaponAttribute.HitLightning && enchantBonus != 0)
            {
                list.Add(1060423, ((int)(enchantBonus * focusBonus)).ToString()); // hit lightning ~1_val~%
            }

            #region Stygian Abyss
            if ((fprop = (double)weapon.WeaponAttributes.HitCurse * focusBonus) != 0)
            {
                list.Add(1113712, ((int)fprop).ToString()); // Hit Curse ~1_val~%
            }

            if ((fprop = (double)weapon.WeaponAttributes.HitFatigue * focusBonus) != 0)
            {
                list.Add(1113700, ((int)fprop).ToString()); // Hit Fatigue ~1_val~%
            }

            if ((fprop = (double)weapon.WeaponAttributes.HitManaDrain * focusBonus) != 0)
            {
                list.Add(1113699, ((int)fprop).ToString()); // Hit Mana Drain ~1_val~%
            }

            if (weapon.SearingWeapon)
            {
                list.Add(1151183); // Searing Weapon
            }
            #endregion

            if ((fprop = (double)weapon.WeaponAttributes.HitLowerAttack * focusBonus) != 0)
            {
                list.Add(1060424, ((int)fprop).ToString()); // hit lower attack ~1_val~%
            }

            if ((fprop = (double)weapon.WeaponAttributes.HitLowerDefend * focusBonus) != 0)
            {
                list.Add(1060425, ((int)fprop).ToString()); // hit lower defense ~1_val~%
            }

            if ((fprop = (double)weapon.WeaponAttributes.HitMagicArrow * focusBonus) != 0)
            {
                list.Add(1060426, ((int)fprop).ToString()); // hit magic arrow ~1_val~%
            }
            else if (bonus == AosWeaponAttribute.HitMagicArrow && enchantBonus != 0)
            {
                list.Add(1060426, ((int)(enchantBonus * focusBonus)).ToString()); // hit magic arrow ~1_val~%
            }

            if ((fprop = (double)weapon.WeaponAttributes.HitLeechMana * focusBonus) != 0)
            {
                list.Add(1060427, ((int)fprop).ToString()); // hit mana leech ~1_val~%
            }

            if ((fprop = (double)weapon.WeaponAttributes.HitPhysicalArea * focusBonus) != 0)
            {
                list.Add(1060428, ((int)fprop).ToString()); // hit physical area ~1_val~%
            }

            if ((fprop = (double)weapon.WeaponAttributes.HitPoisonArea * focusBonus) != 0)
            {
                list.Add(1060429, ((int)fprop).ToString()); // hit poison area ~1_val~%
            }

            if ((fprop = (double)weapon.WeaponAttributes.HitLeechStam * focusBonus) != 0)
            {
                list.Add(1060430, ((int)fprop).ToString()); // hit stamina leech ~1_val~%
            }

            if (ImmolatingWeaponSpell.IsImmolating(weapon.Owner, weapon))
            {
                list.Add(1111917); // Immolated
            }

            if (Core.ML && weapon is BaseRanged && (prop = ((BaseRanged)weapon).Velocity) != 0)
            {
                list.Add(1072793, prop.ToString()); // Velocity ~1_val~%
            }

            if ((prop = weapon.GetLowerStatReq()) != 0)
            {
                list.Add(1060435, prop.ToString()); // lower requirements ~1_val~%
            }

            if ((prop = (weapon.GetLuckBonus() + weapon.Attributes.Luck)) != 0)
            {
                list.Add(1060436, prop.ToString()); // luck ~1_val~
            }

            if ((prop = weapon.WeaponAttributes.MageWeapon) != 0)
            {
                list.Add(1060438, (30 - prop).ToString()); // mage weapon -~1_val~ skill
            }

            if ((prop = weapon.WeaponAttributes.SelfRepair) != 0)
            {
                list.Add(1060450, prop.ToString()); // self repair ~1_val~
            }

            #region Stygian Abyss
            if ((prop = weapon.AbsorptionAttributes.CastingFocus) != 0)
            {
                list.Add(1113696, prop.ToString()); // Casting Focus ~1_val~%
            }

            if ((prop = weapon.AbsorptionAttributes.EaterFire) != 0)
            {
                list.Add(1113593, prop.ToString()); // Fire Eater ~1_Val~%
            }

            if ((prop = weapon.AbsorptionAttributes.EaterCold) != 0)
            {
                list.Add(1113594, prop.ToString()); // Cold Eater ~1_Val~%
            }

            if ((prop = weapon.AbsorptionAttributes.EaterPoison) != 0)
            {
                list.Add(1113595, prop.ToString()); // Poison Eater ~1_Val~%
            }

            if ((prop = weapon.AbsorptionAttributes.EaterEnergy) != 0)
            {
                list.Add(1113596, prop.ToString()); // Energy Eater ~1_Val~%
            }

            if ((prop = weapon.AbsorptionAttributes.EaterKinetic) != 0)
            {
                list.Add(1113597, prop.ToString()); // Kinetic Eater ~1_Val~%
            }

            if ((prop = weapon.AbsorptionAttributes.EaterDamage) != 0)
            {
                list.Add(1113598, prop.ToString()); // Damage Eater ~1_Val~%
            }

            if ((prop = weapon.AbsorptionAttributes.ResonanceFire) != 0)
            {
                list.Add(1113691, prop.ToString()); // Fire Resonance ~1_val~%
            }

            if ((prop = weapon.AbsorptionAttributes.ResonanceCold) != 0)
            {
                list.Add(1113692, prop.ToString()); // Cold Resonance ~1_val~%
            }

            if ((prop = weapon.AbsorptionAttributes.ResonancePoison) != 0)
            {
                list.Add(1113693, prop.ToString()); // Poison Resonance ~1_val~%
            }

            if ((prop = weapon.AbsorptionAttributes.ResonanceEnergy) != 0)
            {
                list.Add(1113694, prop.ToString()); // Energy Resonance ~1_val~%
            }

            if ((prop = weapon.AbsorptionAttributes.ResonanceKinetic) != 0)
            {
                list.Add(1113695, prop.ToString()); // Kinetic Resonance ~1_val~%
            }
            #endregion

            int phys, fire, cold, pois, nrgy, chaos, direct;

            weapon.GetDamageTypes(null, out phys, out fire, out cold,
                out pois, out nrgy, out chaos, out direct);

            #region Mondain's Legacy
            if (chaos != 0)
            {
                list.Add(1072846, chaos.ToString()); // chaos damage ~1_val~%
            }

            if (direct != 0)
            {
                list.Add(1079978, direct.ToString()); // Direct Damage: ~1_PERCENT~%
            }
            #endregion

            if (phys != 0)
            {
                list.Add(1060403, phys.ToString()); // physical damage ~1_val~%
            }

            if (fire != 0)
            {
                list.Add(1060405, fire.ToString()); // fire damage ~1_val~%
            }

            if (cold != 0)
            {
                list.Add(1060404, cold.ToString()); // cold damage ~1_val~%
            }

            if (pois != 0)
            {
                list.Add(1060406, pois.ToString()); // poison damage ~1_val~%
            }

            if (nrgy != 0)
            {
                list.Add(1060407, nrgy.ToString()); // energy damage ~1_val
            }

            if (Core.ML && chaos != 0)
            {
                list.Add(1072846, chaos.ToString()); // chaos damage ~1_val~%
            }

            if (Core.ML && direct != 0)
            {
                list.Add(1079978, direct.ToString()); // Direct Damage: ~1_PERCENT~%
            }

            list.Add(1061168, "{0}\t{1}", weapon.MinDamage.ToString(), weapon.MaxDamage.ToString()); // weapon damage ~1_val~ - ~2_val~

            if (Core.ML)
            {
                list.Add(1061167, String.Format("{0}s", weapon.Speed)); // weapon speed ~1_val~
            }
            else
            {
                list.Add(1061167, weapon.Speed.ToString());
            }

            if (weapon.MaxRange > 1)
            {
                list.Add(1061169, weapon.MaxRange.ToString()); // range ~1_val~
            }

            int strReq = AOS.Scale(weapon.StrRequirement, 100 - weapon.GetLowerStatReq());

            if (strReq > 0)
            {
                list.Add(1061170, strReq.ToString()); // strength requirement ~1_val~
            }

            if (weapon.Layer == Layer.TwoHanded)
            {
                list.Add(1061171); // two-handed weapon
            }
            else
            {
                list.Add(1061824); // one-handed weapon
            }

            if (Core.SE || weapon.WeaponAttributes.UseBestSkill == 0)
            {
                switch (weapon.Skill)
                {
                    case SkillName.Swords:
                        list.Add(1061172);
                        break; // skill required: swordsmanship
                    case SkillName.Macing:
                        list.Add(1061173);
                        break; // skill required: mace fighting
                    case SkillName.Fencing:
                        list.Add(1061174);
                        break; // skill required: fencing
                    case SkillName.Archery:
                        list.Add(1061175);
                        break; // skill required: archery
                }
            }

            if (weapon.HitPoints >= 0 && weapon.MaxHitPoints > 0)
            {
                list.Add(1060639, "{0}\t{1}", weapon.HitPoints, weapon.MaxHitPoints); // durability ~1_val~ / ~2_val~
            }

            if (weapon.IsSetItem && !weapon.SetEquipped)
            {
                list.Add(1072378); // <br>Only when full set is present:				
                GetSetProperties(list, weapon);
            }

            if (weapon.ItemPower != ItemPower.None)
            {
                if (weapon.ItemPower <= ItemPower.LegendaryArtifact)
                    list.Add(1151488 + ((int)weapon.ItemPower - 1));
                else
                    list.Add(1152281 + ((int)weapon.ItemPower - 9));
            }

            return list;
        }

        public OpenPropertyList GetOpenProperties(BaseClothing clothing)
        {
            OpenPropertyList list = new OpenPropertyList();

            #region Stygian Abyss
            if (clothing.IsImbued == true)
                list.Add(1080418); // (Imbued)

            if (clothing.GorgonLenseCharges > 0)
                list.Add(1112590, clothing.GorgonLenseCharges.ToString()); //Gorgon Lens Charges: ~1_val~
            #endregion

            if (clothing.Crafter != null)
                list.Add(1050043, clothing.Crafter.TitleName); // crafted by ~1_NAME~

            #region Factions
            if (clothing.FactionItemState != null)
                list.Add(1041350); // faction item
            #endregion

            #region Mondain's Legacy Sets
            if (clothing.IsSetItem)
            {
                if (clothing.MixedSet)
                    list.Add(1073491, clothing.Pieces.ToString()); // Part of a Weapon/Armor Set (~1_val~ pieces)
                else
                    list.Add(1072376, clothing.Pieces.ToString()); // Part of an Armor Set (~1_val~ pieces)

                if (clothing.SetEquipped)
                {
                    if (clothing.MixedSet)
                        list.Add(1073492); // Full Weapon/Armor Set Present
                    else
                        list.Add(1072377); // Full Armor Set Present

                    GetSetProperties(list, clothing);
                }
            }
            #endregion

            if (clothing.Quality == ItemQuality.Exceptional)
                list.Add(1060636); // exceptional

            if (clothing.RequiredRace == Race.Elf)
                list.Add(1075086); // Elves Only
            #region Stygian Abyss
            else if (clothing.RequiredRace == Race.Gargoyle)
                list.Add(1111709); // Gargoyles Only
            #endregion

            if (clothing.NegativeAttributes != null)
                GetNegativeProperties(list, clothing.NegativeAttributes);

            for (int i = 0; i < 5; ++i)
            {
                SkillName skill;
                double bonus;

                if (!clothing.SkillBonuses.GetValues(i, out skill, out bonus))
                    continue;

                list.Add(1060451 + i, "#{0}\t{1}", AosSkillBonuses.GetLabel(skill), bonus);
            }

            GetAosAttributeProperties(list, clothing.Attributes);

            int prop;

            if ((prop = clothing.ArtifactRarity) > 0)
                list.Add(1061078, prop.ToString()); // artifact rarity ~1_val~

            if ((prop = clothing.ClothingAttributes.LowerStatReq) != 0)
                list.Add(1060435, prop.ToString()); // lower requirements ~1_val~%

            if ((prop = clothing.ClothingAttributes.MageArmor) != 0)

                if ((prop = clothing.ClothingAttributes.SelfRepair) != 0)
                    list.Add(1060450, prop.ToString()); // self repair ~1_val~

            #region SA
            GetSAAbsorptionProperties(list, clothing.SAAbsorptionAttributes);
            #endregion

            GetResistanceProperties(list, clothing);

            if ((prop = clothing.ClothingAttributes.DurabilityBonus) > 0)
                list.Add(1060410, prop.ToString()); // durability ~1_val~%

            if ((prop = clothing.ComputeStatReq(StatType.Str)) > 0)
                list.Add(1061170, prop.ToString()); // strength requirement ~1_val~

            if (clothing.HitPoints >= 0 && clothing.MaxHitPoints > 0)
                list.Add(1060639, "{0}\t{1}", clothing.HitPoints, clothing.MaxHitPoints); // durability ~1_val~ / ~2_val~

            #region Mondain's Legacy Sets
            if (clothing.IsSetItem && !clothing.SetEquipped)
            {
                list.Add(1072378); // <br>Only when full set is present:				
                GetSetProperties(list, clothing);
            }
            #endregion

            if (clothing.ItemPower != ItemPower.None)
            {
                if (clothing.ItemPower <= ItemPower.LegendaryArtifact)
                    list.Add(1151488 + ((int)clothing.ItemPower - 1));
                else
                    list.Add(1152281 + ((int)clothing.ItemPower - 9));
            }

            return list;
        }

        public OpenPropertyList GetOpenProperties(BaseQuiver quiver)
        {
            OpenPropertyList list = new OpenPropertyList();

            if (quiver.Crafter != null)
                list.Add(1050043, quiver.Crafter.TitleName); // crafted by ~1_NAME~

            if (quiver.Quality == ItemQuality.Exceptional)
                list.Add(1063341); // exceptional

            for (int i = 0; i < 5; ++i)
            {
                SkillName skill;
                double bonus;

                if (!quiver.SkillBonuses.GetValues(i, out skill, out bonus))
                    continue;

                list.Add(1060451 + i, "#{0}\t{1}", AosSkillBonuses.GetLabel(skill), bonus);
            }

            Item ammo = quiver.Ammo;

            if (ammo != null)
            {
                if (ammo is Arrow)
                    list.Add(1075265, "{0}\t{1}", ammo.Amount, quiver.Capacity); // Ammo: ~1_QUANTITY~/~2_CAPACITY~ arrows
                else if (ammo is Bolt)
                    list.Add(1075266, "{0}\t{1}", ammo.Amount, quiver.Capacity); // Ammo: ~1_QUANTITY~/~2_CAPACITY~ bolts
            }
            else
                list.Add(1075265, "{0}\t{1}", 0, quiver.Capacity); // Ammo: ~1_QUANTITY~/~2_CAPACITY~ arrows

            int prop;

            if ((prop = quiver.DamageIncrease) != 0)
                list.Add(1074762, prop.ToString()); // Damage modifier: ~1_PERCENT~%

            int phys, fire, cold, pois, nrgy, chaos, direct;
            phys = fire = cold = pois = nrgy = chaos = direct = 0;

            quiver.AlterBowDamage(ref phys, ref fire, ref cold, ref pois, ref nrgy, ref chaos, ref direct);

            if (phys != 0)
                list.Add(1060403, phys.ToString()); // physical damage ~1_val~%

            if (fire != 0)
                list.Add(1060405, fire.ToString()); // fire damage ~1_val~%

            if (cold != 0)
                list.Add(1060404, cold.ToString()); // cold damage ~1_val~%

            if (pois != 0)
                list.Add(1060406, pois.ToString()); // poison damage ~1_val~%

            if (nrgy != 0)
                list.Add(1060407, nrgy.ToString()); // energy damage ~1_val

            if (chaos != 0)
                list.Add(1072846, chaos.ToString()); // chaos damage ~1_val~%

            if (direct != 0)
                list.Add(1079978, direct.ToString()); // Direct Damage: ~1_PERCENT~%

            GetAosAttributeProperties(list, quiver.Attributes);

            #region Mondain's Legacy Sets
            if (quiver.IsSetItem)
            {
                list.Add(1073491, quiver.Pieces.ToString()); // Part of a Weapon/Armor Set (~1_val~ pieces)

                if (quiver.SetEquipped)
                {
                    list.Add(1073492); // Full Weapon/Armor Set Present					
                    GetSetProperties(list, quiver);
                }
            }
            #endregion

            GetResistanceProperties(list, quiver);

            double weight = 0;

            if (ammo != null)
                weight = ammo.Weight * ammo.Amount;

            list.Add(1072241, "{0}\t{1}\t{2}\t{3}", quiver.Items.Count, quiver.DefaultMaxItems, (int)weight, quiver.DefaultMaxWeight); // Contents: ~1_COUNT~/~2_MAXCOUNT items, ~3_WEIGHT~/~4_MAXWEIGHT~ stones

            if ((prop = quiver.WeightReduction) != 0)
                list.Add(1072210, prop.ToString()); // Weight reduction: ~1_PERCENTAGE~%	

            #region Mondain's Legacy Sets
            if (quiver.IsSetItem && !quiver.SetEquipped)
            {
                list.Add(1072378); // <br>Only when full set is present:				
                GetSetProperties(list, quiver);
            }
            #endregion

            return list;
        }

        public OpenPropertyList GetOpenProperties(BaseInstrument instrument)
        {
            OpenPropertyList list = new OpenPropertyList();

            int oldUses = instrument.UsesRemaining;
            instrument.CheckReplenishUses(false);

            if (instrument.Crafter != null)
                list.Add(1050043, instrument.Crafter.TitleName); // crafted by ~1_NAME~

            if (instrument.Quality == ItemQuality.Exceptional)
                list.Add(1060636); // exceptional

            list.Add(1060584, instrument.UsesRemaining.ToString()); // uses remaining: ~1_val~

            if (instrument.ReplenishesCharges)
                list.Add(1070928); // Replenish Charges

            if (instrument.Slayer != SlayerName.None)
            {
                SlayerEntry entry = SlayerGroup.GetEntryByName(instrument.Slayer);
                if (entry != null)
                    list.Add(entry.Title);
            }

            if (instrument.Slayer2 != SlayerName.None)
            {
                SlayerEntry entry = SlayerGroup.GetEntryByName(instrument.Slayer2);
                if (entry != null)
                    list.Add(entry.Title);
            }

            if (instrument.UsesRemaining != oldUses)
                Timer.DelayCall(TimeSpan.Zero, new TimerCallback(instrument.InvalidateProperties));

            return list;
        }

        public OpenPropertyList GetOpenProperties(BaseTalisman talisman)
        {
            OpenPropertyList list = new OpenPropertyList();

            if (talisman.Attributes.Brittle > 0)
                list.Add(1116209); // Brittle

            if (talisman.Blessed)
            {
                if (talisman.BlessedFor != null)
                    list.Add(1072304, !String.IsNullOrEmpty(talisman.BlessedFor.Name) ? talisman.BlessedFor.Name : "Unnamed Warrior"); // Owned by ~1_name~
                else
                    list.Add(1072304, "Nobody"); // Owned by ~1_name~
            }

            if (talisman.Parent is Mobile && talisman.MaxChargeTime > 0)
            {
                if (talisman.ChargeTime > 0)
                    list.Add(1074884, talisman.ChargeTime.ToString()); // Charge time left: ~1_val~
                else
                    list.Add(1074883); // Fully Charged
            }

            if (talisman.Killer != null && !talisman.Killer.IsEmpty && talisman.Killer.Amount > 0)
                list.Add(1072388, "{0}\t{1}", talisman.Killer.Name != null ? talisman.Killer.Name.ToString() : "Unknown", talisman.Killer.Amount); // ~1_NAME~ Killer: +~2_val~%

            if (talisman.Protection != null && !talisman.Protection.IsEmpty && talisman.Protection.Amount > 0)
                list.Add(1072387, "{0}\t{1}", talisman.Protection.Name != null ? talisman.Protection.Name.ToString() : "Unknown", talisman.Protection.Amount); // ~1_NAME~ Protection: +~2_val~%

            if (talisman.ExceptionalBonus != 0)
                list.Add(1072395, "#{0}\t{1}", AosSkillBonuses.GetLabel(talisman.GetMainSkill()), talisman.ExceptionalBonus); // ~1_NAME~ Exceptional Bonus: ~2_val~%

            if (talisman.SuccessBonus != 0)
                list.Add(1072394, "#{0}\t{1}", AosSkillBonuses.GetLabel(talisman.GetMainSkill()), talisman.SuccessBonus); // ~1_NAME~ Bonus: ~2_val~%

            if (talisman.SkillBonuses != null)
            {
                for (int i = 0; i < 5; ++i)
                {
                    SkillName skill;
                    double skillbonus;

                    if (!talisman.SkillBonuses.GetValues(i, out skill, out skillbonus))
                        continue;

                    list.Add(1060451 + i, "#{0}\t{1}", AosSkillBonuses.GetLabel(skill), skillbonus);
                }
            }

            GetAosAttributeProperties(list, talisman.Attributes);

            if (talisman.MaxCharges > 0)
                list.Add(1060741, talisman.Charges.ToString()); // charges: ~1_val~

            if (talisman is ManaPhasingOrb)
                list.Add(1116158); //Mana Phase

            if (talisman.Slayer != TalismanSlayerName.None)
            {
                if (talisman.Slayer == TalismanSlayerName.Wolf)
                    list.Add(1075462);
                else if (talisman.Slayer == TalismanSlayerName.Goblin)
                    list.Add(1095010);
                else if (talisman.Slayer == TalismanSlayerName.Undead)
                    list.Add(1060479);
                else
                    list.Add(1072503 + (int)talisman.Slayer);
            }

            if (talisman.MaxHitPoints > 0)
                list.Add(1060639, "{0}\t{1}", talisman.HitPoints, talisman.MaxHitPoints); // durability ~1_val~ / ~2_val~

            return list;
        }

        public OpenPropertyList GetOpenProperties(Spellbook spellbook)
        {
            OpenPropertyList list = new OpenPropertyList();

            if (spellbook.Quality == BookQuality.Exceptional)
            {
                list.Add(1063341); // exceptional
            }

            if (spellbook.EngravedText != null)
            {
                list.Add(1072305, spellbook.EngravedText); // Engraved: ~1_INSCRIPTION~
            }

            if (spellbook.Crafter != null)
            {
                list.Add(1050043, spellbook.Crafter.TitleName); // crafted by ~1_NAME~
            }

            if (spellbook.SkillBonuses != null)
            {
                for (int i = 0; i < 5; ++i)
                {
                    SkillName skill;
                    double skillbonus;

                    if (!spellbook.SkillBonuses.GetValues(i, out skill, out skillbonus))
                        continue;

                    list.Add(1060451 + i, "#{0}\t{1}", AosSkillBonuses.GetLabel(skill), skillbonus);
                }
            }

            if (spellbook.Slayer != SlayerName.None)
            {
                SlayerEntry entry = SlayerGroup.GetEntryByName(spellbook.Slayer);
                if (entry != null)
                {
                    list.Add(entry.Title);
                }
            }

            if (spellbook.Slayer2 != SlayerName.None)
            {
                SlayerEntry entry = SlayerGroup.GetEntryByName(spellbook.Slayer2);
                if (entry != null)
                {
                    list.Add(entry.Title);
                }
            }

            GetAosAttributeProperties(list, spellbook.Attributes);

            list.Add(1042886, spellbook.SpellCount.ToString()); // ~1_NUMBERS_OF_SPELLS~ Spells

            return list;
        }

        public OpenPropertyList GetOpenProperties(BaseJewel jewel)
        {
            OpenPropertyList list = new OpenPropertyList();

            #region Stygian Abyss
            if (jewel.IsImbued == true)
                list.Add(1080418); // (Imbued)

            if (jewel.GorgonLenseCharges > 0)
                list.Add(1112590, jewel.GorgonLenseCharges.ToString()); //Gorgon Lens Charges: ~1_val~
            #endregion

            #region Mondain's Legacy
            if (jewel.Quality == ItemQuality.Exceptional)
                list.Add(1063341); // exceptional

            if (jewel.Crafter != null)
                list.Add(1050043, jewel.Crafter.TitleName); // crafted by ~1_NAME~
            #endregion

            #region Mondain's Legacy Sets
            if (jewel.IsSetItem)
            {
                list.Add(1080240, jewel.Pieces.ToString()); // Part of a Jewelry Set (~1_val~ pieces)

                if (jewel.SetEquipped)
                {
                    list.Add(1080241); // Full Jewelry Set Present					
                    GetSetProperties(list, jewel);
                }
            }
            #endregion

            if (jewel.NegativeAttributes != null)
                GetNegativeProperties(list, jewel.NegativeAttributes);

            if (jewel.SkillBonuses != null)
            {
                for (int i = 0; i < 5; ++i)
                {
                    SkillName skill;
                    double skillbonus;

                    if (!jewel.SkillBonuses.GetValues(i, out skill, out skillbonus))
                        continue;

                    list.Add(1060451 + i, "#{0}\t{1}", AosSkillBonuses.GetLabel(skill), skillbonus);
                }
            }

            int prop;

            #region Stygian Abyss
            if (jewel.RequiredRace == Race.Elf)
                list.Add(1075086); // Elves Only
            else if (jewel.RequiredRace == Race.Gargoyle)
                list.Add(1111709); // Gargoyles Only
            #endregion

            if ((prop = jewel.ArtifactRarity) > 0)
                list.Add(1061078, prop.ToString()); // artifact rarity ~1_val~

            GetAosAttributeProperties(list, jewel.Attributes);

            #region SA
            GetSAAbsorptionProperties(list, jewel.AbsorptionAttributes);
            #endregion

            GetResistanceProperties(list, jewel);

            if (jewel.HitPoints >= 0 && jewel.MaxHitPoints > 0)
                list.Add(1060639, "{0}\t{1}", jewel.HitPoints, jewel.MaxHitPoints); // durability ~1_val~ / ~2_val~

            if (jewel.ItemPower != ItemPower.None)
            {
                if (jewel.ItemPower <= ItemPower.LegendaryArtifact)
                    list.Add(1151488 + ((int)jewel.ItemPower - 1));
                else
                    list.Add(1152281 + ((int)jewel.ItemPower - 9));
            }

            return list;
        }

        private void GetNegativeProperties(OpenPropertyList list, NegativeAttributes neg)
        {

            if (neg.Brittle > 0) list.Add(1116209);

            if (neg.Prized > 0) list.Add(1154910);

            if (neg.Massive > 0) list.Add(1038003);

            if (neg.Unwieldly > 0) list.Add(1154909);

            if (neg.Antique > 0) list.Add(1076187);

            if (neg.NoRepair > 0) list.Add(1151782);
        }

        private void GetSAAbsorptionProperties(OpenPropertyList list, SAAbsorptionAttributes attributes)
        {
            int prop;

            if ((prop = attributes.EaterFire) != 0)
                list.Add(1113593, prop.ToString()); // Fire Eater ~1_Val~%

            if ((prop = attributes.EaterCold) != 0)
                list.Add(1113594, prop.ToString()); // Cold Eater ~1_Val~%

            if ((prop = attributes.EaterPoison) != 0)
                list.Add(1113595, prop.ToString()); // Poison Eater ~1_Val~%

            if ((prop = attributes.EaterEnergy) != 0)
                list.Add(1113596, prop.ToString()); // Energy Eater ~1_Val~%

            if ((prop = attributes.EaterKinetic) != 0)
                list.Add(1113597, prop.ToString()); // Kinetic Eater ~1_Val~%

            if ((prop = attributes.EaterDamage) != 0)
                list.Add(1113598, prop.ToString()); // Damage Eater ~1_Val~%

            if ((prop = attributes.ResonanceFire) != 0)
                list.Add(1113691, prop.ToString()); // Fire Resonance ~1_val~%

            if ((prop = attributes.ResonanceCold) != 0)
                list.Add(1113692, prop.ToString()); // Cold Resonance ~1_val~%

            if ((prop = attributes.ResonancePoison) != 0)
                list.Add(1113693, prop.ToString()); // Poison Resonance ~1_val~%

            if ((prop = attributes.ResonanceEnergy) != 0)
                list.Add(1113694, prop.ToString()); // Energy Resonance ~1_val~%

            if ((prop = attributes.ResonanceKinetic) != 0)
                list.Add(1113695, prop.ToString()); // Kinetic Resonance ~1_val~%

            if ((prop = attributes.CastingFocus) != 0)
                list.Add(1113696, prop.ToString()); // Casting Focus ~1_val~%
        }

        private void GetAosAttributeProperties(OpenPropertyList list, AosAttributes attributes)
        {
            int prop;

            if ((prop = attributes.WeaponDamage) != 0)
                list.Add(1060401, prop.ToString()); // damage increase ~1_val~%

            if ((prop = attributes.DefendChance) != 0)
                list.Add(1060408, prop.ToString()); // defense chance increase ~1_val~%

            if ((prop = attributes.BonusDex) != 0)
                list.Add(1060409, prop.ToString()); // dexterity bonus ~1_val~

            if ((prop = attributes.EnhancePotions) != 0)
                list.Add(1060411, prop.ToString()); // enhance potions ~1_val~%

            if ((prop = attributes.CastRecovery) != 0)
                list.Add(1060412, prop.ToString()); // faster cast recovery ~1_val~

            if ((prop = attributes.CastSpeed) != 0)
                list.Add(1060413, prop.ToString()); // faster casting ~1_val~

            if ((prop = attributes.AttackChance) != 0)
                list.Add(1060415, prop.ToString()); // hit chance increase ~1_val~%

            if ((prop = attributes.BonusHits) != 0)
                list.Add(1060431, prop.ToString()); // hit point increase ~1_val~

            if ((prop = attributes.BonusInt) != 0)
                list.Add(1060432, prop.ToString()); // intelligence bonus ~1_val~

            if ((prop = attributes.LowerManaCost) != 0)
                list.Add(1060433, prop.ToString()); // lower mana cost ~1_val~%

            if ((prop = attributes.LowerRegCost) != 0)
                list.Add(1060434, prop.ToString()); // lower reagent cost ~1_val~%

            if ((prop = attributes.BonusMana) != 0)
                list.Add(1060439, prop.ToString()); // mana increase ~1_val~

            if ((prop = attributes.RegenMana) != 0)
                list.Add(1060440, prop.ToString()); // mana regeneration ~1_val~

            if ((prop = attributes.NightSight) != 0)
                list.Add(1060441); // night sight

            if ((prop = attributes.ReflectPhysical) != 0)
                list.Add(1060442, prop.ToString()); // reflect physical damage ~1_val~%

            if ((prop = attributes.RegenStam) != 0)
                list.Add(1060443, prop.ToString()); // stamina regeneration ~1_val~

            if ((prop = attributes.RegenHits) != 0)
                list.Add(1060444, prop.ToString()); // hit point regeneration ~1_val~

            if ((prop = attributes.SpellChanneling) != 0)
                list.Add(1060482); // spell channeling

            if ((prop = attributes.SpellDamage) != 0)
                list.Add(1060483, prop.ToString()); // spell damage increase ~1_val~%

            if ((prop = attributes.BonusStam) != 0)
                list.Add(1060484, prop.ToString()); // stamina increase ~1_val~

            if ((prop = attributes.BonusStr) != 0)
                list.Add(1060485, prop.ToString()); // strength bonus ~1_val~

            if ((prop = attributes.WeaponSpeed) != 0)
                list.Add(1060486, prop.ToString()); // swing speed increase ~1_val~%

            if (Core.ML && (prop = attributes.IncreasedKarmaLoss) != 0)
                list.Add(1075210, prop.ToString()); // Increased Karma Loss ~1val~%
        }

        private void GetResistanceProperties(OpenPropertyList list, Item item)
        {
            int v = item.PhysicalResistance;

            if (v != 0) list.Add(1060448, v.ToString()); // physical resist ~1_val~%

            v = item.FireResistance;

            if (v != 0) list.Add(1060447, v.ToString()); // fire resist ~1_val~%

            v = item.ColdResistance;

            if (v != 0) list.Add(1060445, v.ToString()); // cold resist ~1_val~%

            v = item.PoisonResistance;

            if (v != 0) list.Add(1060449, v.ToString()); // poison resist ~1_val~%

            v = item.EnergyResistance;

            if (v != 0) list.Add(1060446, v.ToString()); // energy resist ~1_val~%
        }

        private void GetSetProperties(OpenPropertyList list, ISetItem setItem)
        {
            int prop;

            if ((prop = setItem.SetAttributes.RegenHits) != 0)
                list.Add(1080244, prop.ToString()); // hit point regeneration ~1_val~ (total)

            if ((prop = setItem.SetAttributes.RegenStam) != 0)
                list.Add(1060443, prop.ToString()); // stamina regeneration ~1_val~ (total)

            if ((prop = setItem.SetAttributes.RegenMana) != 0)
                list.Add(1080245, prop.ToString()); // mana regeneration ~1_val~ (total)

            if ((prop = setItem.SetAttributes.DefendChance) != 0)
                list.Add(1073493, prop.ToString()); // defense chance increase ~1_val~% (total)

            if ((prop = setItem.SetAttributes.AttackChance) != 0)
                list.Add(1073490, prop.ToString()); // hit chance increase ~1_val~% (total)

            if ((prop = setItem.SetAttributes.BonusStr) != 0)
                list.Add(1072514, prop.ToString()); // strength bonus ~1_val~ (total)

            if ((prop = setItem.SetAttributes.BonusDex) != 0)
                list.Add(1072503, prop.ToString()); // dexterity bonus ~1_val~ (total)

            if ((prop = setItem.SetAttributes.BonusInt) != 0)
                list.Add(1072381, prop.ToString()); // intelligence bonus ~1_val~ (total)

            if ((prop = setItem.SetAttributes.BonusHits) != 0)
                list.Add(1080360, prop.ToString()); // hit point increase ~1_val~ (total)

            if ((prop = setItem.SetAttributes.BonusStam) != 0)
                list.Add(1060484, prop.ToString()); // stamina increase ~1_val~ (total)

            if ((prop = setItem.SetAttributes.BonusMana) != 0)
                list.Add(1060439, prop.ToString()); // mana increase ~1_val~ (total)

            if ((prop = setItem.SetAttributes.WeaponDamage) != 0)
                list.Add(1060401, prop.ToString()); // damage increase ~1_val~% (total)

            if ((prop = setItem.SetAttributes.WeaponSpeed) != 0)
                list.Add(1074323, prop.ToString()); // swing speed increase ~1_val~% (total)	

            if ((prop = setItem.SetAttributes.SpellDamage) != 0)
                list.Add(1072380, prop.ToString()); // spell damage increase ~1_val~% (total)

            if ((prop = setItem.SetAttributes.CastRecovery) != 0)
                list.Add(1080242, prop.ToString()); // faster cast recovery ~1_val~ (total)

            if ((prop = setItem.SetAttributes.CastSpeed) != 0)
                list.Add(1080243, prop.ToString()); // faster casting ~1_val~ (total)

            if ((prop = setItem.SetAttributes.LowerManaCost) != 0)
                list.Add(1073488, prop.ToString()); // lower mana cost ~1_val~% (total)

            if ((prop = setItem.SetAttributes.LowerRegCost) != 0)
                list.Add(1080441, prop.ToString()); // lower reagent cost ~1_val~% (total)

            if ((prop = setItem.SetAttributes.ReflectPhysical) != 0)
                list.Add(1072513, prop.ToString()); // reflect physical damage ~1_val~% (total)

            if ((prop = setItem.SetAttributes.EnhancePotions) != 0)
                list.Add(1060411, prop.ToString()); // enhance potions ~1_val~% (total)

            if ((prop = setItem.SetAttributes.Luck) != 0)
                list.Add(1073489, prop.ToString()); // luck ~1_val~% (total)

            if (!setItem.SetEquipped && setItem.SetAttributes.NightSight != 0)
                list.Add(1060441); // night sight

            if (setItem.SetSkillBonuses.Skill_1_Value != 0)
                list.Add(1072502, "{0}\t{1}", "#" + (1044060 + (int)setItem.SetSkillBonuses.Skill_1_Name), setItem.SetSkillBonuses.Skill_1_Value); // ~1_skill~ ~2_val~ (total)
        }
    }
}
