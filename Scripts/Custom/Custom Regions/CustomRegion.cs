#region References

using System;
using System.Collections;
using System.Linq;
using Server.Items;
using Server.Mobiles;
using Server.Spells;

#endregion

namespace Server.Regions
{
    public class CustomRegion : GuardedRegion
    {
        private readonly RegionControl m_Controller;

        public RegionControl Controller
        {
            get { return m_Controller; }
        }

        public CustomRegion(RegionControl control)
            : base(control.RegionName, control.Map, control.RegionPriority, control.RegionArea)
        {
            Disabled = !control.IsGuarded;
            Music = control.Music;
            m_Controller = control;
        }

        private Timer m_Timer;

        public override void OnDeath(Mobile m)
        {
            if (m == null || m.Deleted)
            {
                return;
            }

            if (m is PlayerMobile && m_Controller.NoPlayerItemDrop)
            {
                if (m.Female)
                {
                    m.FixedParticles(0x374A, 10, 30, 5013, 1153, 2, EffectLayer.Waist);
                    m.Body = 403;
                    m.Hidden = true;
                }
                else
                {
                    m.FixedParticles(0x374A, 10, 30, 5013, 1153, 2, EffectLayer.Waist);
                    m.Body = 402;
                    m.Hidden = true;
                }
                m.Hidden = false;
            }
            else if (!(m is PlayerMobile) && m_Controller.NoNPCItemDrop)
            {
                if (m.Female)
                {
                    m.FixedParticles(0x374A, 10, 30, 5013, 1153, 2, EffectLayer.Waist);
                    m.Body = 403;
                    m.Hidden = true;
                }
                else
                {
                    m.FixedParticles(0x374A, 10, 30, 5013, 1153, 2, EffectLayer.Waist);
                    m.Body = 402;
                    m.Hidden = true;
                }
                m.Hidden = false;
            }
            else
            {            
                m_Timer = new MovePlayerTimer(m, m_Controller);
            }
            m_Timer.Start();
        }

        private class MovePlayerTimer : Timer
        {
            private readonly Mobile m;
            private readonly RegionControl m_Controller;

            public MovePlayerTimer(Mobile m_Mobile, RegionControl controller)
                : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0))
            {
                Priority = TimerPriority.FiftyMS;
                m = m_Mobile;
                m_Controller = controller;
            }

            protected override void OnTick()
            {
                if (m is PlayerMobile)
                {
                    if (m_Controller.EmptyPlayerCorpse)
                    {
                        if (m != null && m.Corpse != null)
                        {
                            ArrayList corpseitems = new ArrayList(m.Corpse.Items);

                            foreach (Item item in from Item item in corpseitems where item != null && !item.Deleted && (item.Layer != Layer.Bank) && (item.Layer != Layer.Backpack) && (item.Layer != Layer.Hair) && (item.Layer != Layer.FacialHair) && (item.Layer != Layer.Mount) where (item.LootType != LootType.Blessed) select item)
                            {
                                item.MoveToWorld(m.Corpse.Location, m.Corpse.Map);
                            }
                        }
                    }
                }
                else if (m_Controller.EmptyNPCCorpse)
                {
                    if (m != null && m.Corpse != null)
                    {
                        ArrayList corpseitems = new ArrayList(m.Corpse.Items);

                        foreach (Item item in from Item item in corpseitems where item != null && !item.Deleted && (item.Layer != Layer.Bank) && (item.Layer != Layer.Backpack) && (item.Layer != Layer.Hair) && (item.Layer != Layer.FacialHair) && (item.Layer != Layer.Mount) where (item.LootType != LootType.Blessed) select item)
                        {
                            item.MoveToWorld(m.Corpse.Location, m.Corpse.Map);
                        }
                    }
                }

                Mobile newnpc = null;

                // Resurrects Players
                if (m is PlayerMobile)
                {
                    if (m_Controller.ResPlayerOnDeath)
                    {
                        if (m != null)
                        {
                            m.Resurrect();
                            m.SendMessage("Thou hast been Resurrected");
                        }
                    }
                }
                else if (m_Controller.ResNPCOnDeath)
                {
                    if (m != null && m.Corpse != null)
                    {
                        Type type = m.GetType();
                        newnpc = Activator.CreateInstance(type) as Mobile;
                        if (newnpc != null)
                        {
                            newnpc.Location = m.Corpse.Location;
                            newnpc.Map = m.Corpse.Map;
                        }
                    }
                }

                // Deletes the corpse 
                if (m is PlayerMobile)
                {
                    if (m_Controller.DeletePlayerCorpse)
                    {
                        if (m != null && m.Corpse != null)
                        {
                            m.Corpse.Delete();
                        }
                    }
                }
                else if (m_Controller.DeleteNPCCorpse)
                {
                    if (m != null && m.Corpse != null)
                    {
                        m.Corpse.Delete();
                    }
                }

                // Move Mobiles
                if (m is PlayerMobile)
                {
                    if (m_Controller.MovePlayerOnDeath)
                    {
                        if (m != null)
                        {
                            m.Map = m_Controller.MovePlayerToMap;
                            m.Location = m_Controller.MovePlayerToLoc;
                        }
                    }
                }
                else if (m_Controller.MoveNPCOnDeath)
                {
                    if (newnpc != null)
                    {
                        newnpc.Map = m_Controller.MoveNPCToMap;
                        newnpc.Location = m_Controller.MoveNPCToLoc;
                    }
                }

                Stop();
            }
        }

        public override bool IsDisabled()
        {
            if (!m_Controller.IsGuarded != Disabled)
            {
                m_Controller.IsGuarded = !Disabled;
            }

            return Disabled;
        }

        public override bool AllowBeneficial(Mobile from, Mobile target)
        {
            if ((m_Controller.AllowBenefitPlayer || !(target is PlayerMobile)) &&
                (m_Controller.AllowBenefitNPC || !(target is BaseCreature)))
            {
                return base.AllowBeneficial(@from, target);
            }
            @from.SendMessage("Thou canst not perform benificial acts on thy target.");
            return false;
        }

        public override bool AllowHarmful(Mobile from, IDamageable target)
        {
            if ((m_Controller.AllowHarmPlayer || !(target is PlayerMobile)) &&
                (m_Controller.AllowHarmNPC || !(target is BaseCreature)))
            {
                return base.AllowHarmful(@from, target);
            }
            @from.SendMessage("Thou canst not perform harmful acts on thy target.");
            return false;
        }

        public override bool AllowHousing(Mobile from, Point3D p)
        {
            return m_Controller.AllowHousing;
        }

        public override bool AllowSpawn()
        {
            return m_Controller.AllowSpawn;
        }

        public override bool CanUseStuckMenu(Mobile m)
        {
            if (!m_Controller.CanUseStuckMenu)
            {
                m.SendMessage("Thou canst not use the Stuck menu here.");
            }
            return m_Controller.CanUseStuckMenu;
        }

        public override bool OnDamage(Mobile m, ref int Damage)
        {
            if (!m_Controller.CanBeDamaged)
            {
                m.SendMessage("Thou canst not be damaged here.");
            }

            return m_Controller.CanBeDamaged;
        }

        public override bool OnResurrect(Mobile m)
        {
            if (!m_Controller.CanRessurect && m.AccessLevel == AccessLevel.Player)
            {
                m.SendMessage("Thou canst not ressurect here.");
            }
            return m_Controller.CanRessurect;
        }

        public override bool OnBeginSpellCast(Mobile from, ISpell s)
        {
            if (@from.AccessLevel >= AccessLevel.Counselor)
            {
                return true;
            }
            bool restricted = m_Controller.IsRestrictedSpell(s);
            if (restricted)
            {
                @from.SendMessage("Thou canst not cast thy spell here.");
                return false;
            }
       
            if (m_Controller.CanMountEthereal || ((Spell) s).Info.Name != "Ethereal Mount")
            {
                return true;
            }
            @from.SendMessage("Thou canst not mount thy ethereal here.");
            return false;
        }

        public override bool OnDecay(Item item)
        {
            return m_Controller.ItemDecay;
        }

        public override bool OnHeal(Mobile m, ref int Heal)
        {
            if (!m_Controller.CanHeal)
            {
                m.SendMessage("Thou canst not be healed here.");
            }

            return m_Controller.CanHeal;
        }

        public override bool OnSkillUse(Mobile m, int skill)
        {
            bool restricted = m_Controller.IsRestrictedSkill(skill);
            if (!restricted || m.AccessLevel >= AccessLevel.Counselor)
            {
                return base.OnSkillUse(m, skill);
            }
            m.SendMessage("Thou canst not use thy skill here.");
            return false;
        }

        public override void OnExit(Mobile m)
        {
            if (m_Controller.ShowExitMessage)
            {
                m.SendMessage("Thou hast left {0}", this.Name);
            }

            base.OnExit(m);
        }

        public override void OnEnter(Mobile m)
        {
            if (m_Controller.ShowEnterMessage)
            {
                m.SendMessage("Thou hast entered {0}", this.Name);
            }

            base.OnEnter(m);
        }

        public override bool OnMoveInto(Mobile m, Direction d, Point3D newLocation, Point3D oldLocation)
        {
            if (m_Controller.CanEnter || Contains(oldLocation))
            {
                return true;
            }
            m.SendMessage("Thou canst not enter this area.");
            return false;
        }

        public override TimeSpan GetLogoutDelay(Mobile m)
        {
            return m.AccessLevel == AccessLevel.Player ? m_Controller.PlayerLogoutDelay : base.GetLogoutDelay(m);
        }

        public override bool OnDoubleClick(Mobile m, object o)
        {
            if (o is BasePotion && !m_Controller.CanUsePotions)
            {
                m.SendMessage("Thou canst not drink potions here.");
                return false;
            }

            if (!(o is Corpse))
            {
                return base.OnDoubleClick(m, o);
            }

            Corpse c = (Corpse) o;

            bool canLoot;

            if (c.Owner == m)
            {
                canLoot = m_Controller.CanLootOwnCorpse;
            }
            else if (c.Owner is PlayerMobile)
            {
                canLoot = m_Controller.CanLootPlayerCorpse;
            }
            else
            {
                canLoot = m_Controller.CanLootNPCCorpse;
            }

            if (!canLoot)
            {
                m.SendMessage("Thou canst not loot thy corpse here.");
            }

            if (m.AccessLevel < AccessLevel.GameMaster || canLoot)
            {
                return canLoot;
            }
            m.SendMessage("This is unlootable to players.");
            return true;
        }

        public override void AlterLightLevel(Mobile m, ref int global, ref int personal)
        {
            if (m_Controller.LightLevel >= 0)
            {
                global = m_Controller.LightLevel;
            }
            else
            {
                base.AlterLightLevel(m, ref global, ref personal);
            }
        }
    }
}
