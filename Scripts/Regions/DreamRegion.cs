using System;
using System.Collections.Generic;
using System.Xml;
using Server.Mobiles;
using Server.Spells.Chivalry;
using Server.Spells.Fourth;
using Server.Spells.Necromancy;
using Server.Spells.Fourth;
using Server.Spells.Seventh;
using Server.Spells.Sixth;
using Server.Items;

namespace Server.Regions
{
    public class DreamRegion : DangerRegion
    {
        public DreamRegion(XmlElement xml, Map map, Region parent)
            : base(xml, map, parent)
        {
        }

        public override bool OnBeforeDeath(Mobile m)
        {
            if (!(m is PlayerMobile))
                return base.OnBeforeDeath(m);

            //Remove the player's items acquired in the dream
            for (int x = m.Items.Count - 1; x >= 0; x--)
            {
                if (!(m.Items[x] is Backpack))
                    m.RemoveItem(m.Items[x]);
            }

            for (int y = m.Backpack.Items.Count - 1; y >= 0; y--)
            {
                if ((m.Backpack.Items[y] is HelmOfCourage) || (m.Backpack.Items[y] is CrystalRoseOfLove) || (m.Backpack.Items[y] is MirrorOfTruth) || (m.Backpack.Items[y] is StaffOrb))
                { }
                else
                {
                    m.RemoveItem(m.Backpack.Items[y]);
                }
            }

            //Find the player's item container
            Item[] Items = Map.SerpentIsle.GetItemsInRange(new Point3D(0, 0, 0)).CastToArray<Item>();
                        
            foreach (Item found in Items)
            {
                if(found is MetalBox)
                {
                    MetalBox box = found as MetalBox;

                    if (box.Name == m.Name)
                    {
                        //Give them back their stuff
                        for(int x = box.Items.Count - 1; x >= 0; x--)
                        {
                            if(box.Items[x] is MetalBox)
                            {
                                MetalBox itemBox = box.Items[x] as MetalBox;
                                for (int y = itemBox.Items.Count - 1; y >= 0; y--)
                                {
                                    m.AddToBackpack(itemBox.Items[y]);
                                }
                                if (itemBox.Items.Count == 0)
                                    itemBox.Delete();
                            }                                
                            else
                                m.EquipItem(box.Items[x]);
                        }

                        if (box.Items.Count == 0)
                            box.Delete();
                    }
                }                
            }

            //Move pets back
            Mobile pMount = null;
            if (m.Mounted)
                pMount = m.Mount as Mobile;

            foreach (Mobile mobile in ((PlayerMobile)m).AllFollowers)
            {
                if (pMount != null)
                    if (mobile == pMount)
                        continue;

                mobile.MoveToWorld(new Point3D(650, 881, 0), Map.SerpentIsle);
                mobile.Frozen = false;
            }

            
            switch (Utility.Random(2))
            {
                case 0:
                    m.MoveToWorld(new Point3D(643, 881, 0), Map.SerpentIsle);
                    break;
                case 1:
                    m.MoveToWorld(new Point3D(662, 882, 0), Map.SerpentIsle);
                    break;
            }

            m.SendMessage("Thou hast awoken from thy dream.");

            //Restore player to full 
            ((PlayerMobile)m).Hits = m.HitsMax;
            ((PlayerMobile)m).Mana = m.ManaMax;
            ((PlayerMobile)m).Stam = m.StamMax;
            ((PlayerMobile)m).Hunger = 20;
            ((PlayerMobile)m).Thirst = 20;

            ((PlayerMobile)m).Paralyzed = false;
            ((PlayerMobile)m).Poison = null;

            EvilOmenSpell.TryEndEffect(m);
            StrangleSpell.RemoveCurse(m);
            CorpseSkinSpell.RemoveCurse(m);
            CurseSpell.RemoveEffect(m);
            MortalStrike.EndWound(m);
            BloodOathSpell.RemoveCurse(m);
            MindRotSpell.ClearMindRotScalar(m);

            BuffInfo.RemoveBuff(m, BuffIcon.Clumsy);
            BuffInfo.RemoveBuff(m, BuffIcon.FeebleMind);
            BuffInfo.RemoveBuff(m, BuffIcon.Weaken);
            BuffInfo.RemoveBuff(m, BuffIcon.Curse);
            BuffInfo.RemoveBuff(m, BuffIcon.MassCurse);
            BuffInfo.RemoveBuff(m, BuffIcon.MortalStrike);
            BuffInfo.RemoveBuff(m, BuffIcon.Mindrot);

            return false;            
        }        

        public override bool OnBeginSpellCast(Mobile m, ISpell s)
        {
            if ((s is GateTravelSpell || s is RecallSpell || s is MarkSpell || s is SacredJourneySpell) && m.IsPlayer())
            {
                m.SendLocalizedMessage(501802); // Thy spell doth not appear to work...

                return false;
            }

            return base.OnBeginSpellCast(m, s);
        }

        public override bool OnSkillUse(Mobile m, int Skill)
        {
            if(Skill == 35)
            {
                m.SendMessage("This doesn't seem possible.");
                return false;
            }
            return base.OnSkillUse(m, Skill);
        }
    }
}
