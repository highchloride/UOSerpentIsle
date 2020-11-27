using Server.Gumps;
using Server.Mobiles;
using System;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Regions;
using Server.Spells;
using Server.Network;
using Server.Multis;
using Server.ContextMenus;

namespace Server.Items
{
    public class SerpentJawbone : Item
    {
        private List<SerpentTooth> m_SerpentTeeth; //A list of SerpentTooth objects

        private static readonly TimeSpan delay = TimeSpan.FromMinutes(15.0);
        private static readonly bool oneStonePerMobile = true;
        private static readonly TimeSpan totalDelay = TimeSpan.FromMinutes(15.0);

        private static List<HomeStoneUse> useRegistry = new List<HomeStoneUse>();

        private bool noWaitTime;
        private DateTime lastUsed;
        private DateTime lastMarked;
        private Point3D home;
        private Map homeMap;
        private Mobile owner;

        [CommandProperty(AccessLevel.GameMaster)]
        public bool NoWaitTime
        {
            get { return (owner.AccessLevel >= AccessLevel.GameMaster ? true : noWaitTime); }
            set { noWaitTime = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime LastUsed
        {
            get { return lastUsed; }
            set { lastUsed = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D Home
        {
            get { return home; }
            set { home = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Map HomeMap
        {
            get { return homeMap; }
            set { homeMap = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        [CommandProperty(AccessLevel.Seer)]
        public List<SerpentTooth> SerpentTeeth
        {
            get { return m_SerpentTeeth; }
            set
            {
                m_SerpentTeeth = value;
                InvalidateProperties();
            }
        }

        [Constructable]
        public SerpentJawbone()
            : base(0x0F05)
        {
            LootType = LootType.Blessed;
            Weight = 0;
            m_SerpentTeeth = new List<SerpentTooth>();
            Name = "Unowned Jawbone";
        }

        [Constructable]
		public SerpentJawbone(Mobile from) : base(0x0F05)
		{
            this.owner = from;
            Weight = 0.0;
			Name = "Unowned Jawbone";
            m_SerpentTeeth = new List<SerpentTooth>();
            LootType = LootType.Blessed;
        }

        public SerpentJawbone(Serial serial)
            : base(serial)
        {
        }

        public static void RegisterUse(Mobile from)
        {
            useRegistry.Add(new HomeStoneUse(from));
        }

        public static TimeSpan GetRemainingTimeToUseForMobile(Mobile from)
        {
            List<HomeStoneUse> innerUseRegistry = useRegistry.FindAll(delegate (HomeStoneUse hsu) {
                return hsu.User == from && hsu.UseTime > DateTime.Now - totalDelay;
            });

            if (innerUseRegistry.Count > 0)
            {
                foreach (HomeStoneUse hsu in innerUseRegistry) // TODO: Better way to solve this?
                {
                    // delay - ( now - used )
                    return totalDelay - (DateTime.Now - hsu.UseTime);
                }
            }

            return TimeSpan.Zero;
        }

        public override bool OnDroppedToMobile(Mobile from, Mobile target)
        {
            Item jawbone = target.Backpack.FindItemByType(typeof(SerpentJawbone));
            Item thisjawbone = from.FindItemByType(typeof(SerpentJawbone));
            
            if(jawbone != null || thisjawbone != null)
            {
                if(target == from)
                {
                    from.SendMessage("Thou canst only carry one Serpent Jawbone!");
                }
                else
                {
                    from.SendMessage("That person canst only carry one Serpent Jawbone!");
                }
                return false;
            }
            return true;
        }

        public static void cleanUseList()
        {
            useRegistry.RemoveAll(delegate (HomeStoneUse hsu) { return hsu.UseTime < DateTime.Now - totalDelay; });
        }

        public static void GetContextMenuEntries(Mobile from, Item item, List<ContextMenuEntry> list)
        {
            list.Add(new GoHomeEntry(from, item));
            list.Add(new SetHomeEntry(from, item));
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            if (owner == null)
            {
                return;
            }
            else
            {
                if (owner != from)
                {
                    from.SendMessage("This is not thine to use.");
                    return;
                }
                else
                {
                    base.GetContextMenuEntries(from, list);
                    SerpentJawbone.GetContextMenuEntries(from, this, list);
                }
            }
        }

        public static bool IsValidTile(int itemID)
        {
            //Serpent Gate Tiles
            return (itemID == 16109 || itemID == 16110 || itemID == 16111 || itemID == 16112);
        }

        public override void OnDoubleClick(Mobile from)
        {
            //base.OnDoubleClick(from);

            if (!this.IsChildOf(from.Backpack))
            {
                from.SendMessage("This must be in thy pack to use it.");
                return;
            }
            else if (owner == null)
            {
                SwitchOwner(from);
                return;
            }
            else if (this.m_SerpentTeeth.Count == 0)
            {
                from.SendMessage("Thy Jawbone is empty.");
                return;
            }
            else
            {
                //Get the serpent gate the player is standing on
                IPooledEnumerable enumerable = Map.GetItemsInRange(from.Location, 0);

                SerpentTeleporterTileAddon serpentGate = null;

                foreach (Item item in enumerable)
                {
                    if (IsValidTile(item.ItemID))
                    {
                        AddonComponent component = item as AddonComponent;
                        serpentGate = component.Addon as SerpentTeleporterTileAddon;
                    }
                }

                //If not standing on a tele, show the gump instead.
                if (serpentGate != null)
                {
                    if (serpentGate.RequiredTooth != null) //If there's a required tooth, check for its presence. For Dark Path teleporters.
                    {
                        foreach (SerpentTooth tooth in SerpentTeeth)
                        {
                            if (serpentGate.RequiredTooth.GetType() == tooth.GetType())
                            {
                                from.SendMessage("Thy jawbone reacts with the gate!");
                                from.SetLocation(serpentGate.Destination, true); //Send the player to the destination of the tooth
                                return;
                            }
                        }
                        from.SendMessage("Thy jawbone appears to be missing the tooth for this gate."); //The player doesn't have the required tooth.
                    }
                    else //No required tooth - this is a world teleporter
                    {
                        from.SendMessage("Thy jawbone reacts with the gate!");
                        from.SetLocation(new Point3D(2185, 1491, 6), true);//Send to Dark Path
                        return;
                    }
                }
                else if (from is PlayerMobile)
                {
                    from.CloseGump(typeof(GumpSerpentJawbone));
                    GumpSerpentJawbone gump = new GumpSerpentJawbone(this.m_SerpentTeeth);
                    from.SendGump(gump);
                }
            }            
        }

        public override void OnSingleClick(Mobile from)
        {
            // why not base? we dont like to see the [blessed] tag, just like on runebooks,spellbooks etc
            // + we dont want it to be called coconut or whatever
            //base.OnSingleClick( from );

            LabelTo(from, this.Name);

            string label;

            TimeSpan timetouse = GetRemainingTimeToUse();
            //TimeSpan timetouseTotal = GetRemainingTimeToUseForMobile( from );

            if (owner == null)
                return;


            if (timetouse.TotalSeconds <= 0.0)
                label = "[ready]";
            else
                label = "[" + timetouse.Minutes + " minutes]";



            if (this.homeMap != null)
                LabelTo(from, label);
            else
                LabelTo(from, "[unmarked]");

        }

        private class SetHomeEntry : ContextMenuEntry
        {
            private readonly SerpentJawbone m_Item;
            private readonly Mobile m_Mobile;
            public SetHomeEntry(Mobile from, Item item)
                : base(2055)// uses "Mark" entry
            {
                m_Item = (SerpentJawbone)item;
                m_Mobile = from;
            }

            public override void OnClick()
            {
                if (m_Item.IsInMarkAbleRegion(m_Mobile))
                {
                    m_Item.Mark(m_Mobile);
                }
                else
                {
                    m_Mobile.SendMessage("This can only be marked in thy home, a tavern, or an inn.");
                }
            }
        }

        private void Mark(Mobile from)
        {
            if (this.lastMarked > DateTime.Now - TimeSpan.FromSeconds(5))
            {
                from.SendMessage("Thou canst not mark thy Jawbone again so soon.");
            }
            else
            {
                this.home = from.Location;
                this.homeMap = from.Map;
                this.lastMarked = DateTime.Now;
                from.PlaySound(0x1E9);
                from.FixedParticles(0x375A, 10, 15, 5037, EffectLayer.Waist);
                from.SendMessage("Thou hast marked this as your home.");
            }
        }

        private class GoHomeEntry : ContextMenuEntry
        {
            private readonly SerpentJawbone m_Item;
            private readonly Mobile m_Mobile;
            public GoHomeEntry(Mobile from, Item item)
                : base(5134)// uses "Goto Loc" entry 5134
            {
                m_Item = (SerpentJawbone)item;
                m_Mobile = from;
            }

            public override void OnClick()
            {
                if (m_Item.Validate(m_Mobile))
                {
                    new HomeStoneSpell(m_Item, m_Mobile).Cast();
                }
            }
        }

        private void SwitchOwner(Mobile from)
        {
            if (owner == null) // double check..
            {
                owner = from;
                from.SendMessage("Thou hast taken possession of this jawbone!");
                RenameThisStone();
            }
            else
                from.SendMessage("This is not thy jawbone!");
        }

        private void RenameThisStone()
        {
            if (owner != null)
            {
                this.Name = owner.Name + "'s jawbone";
                //this.Hue = 0x501;
            }
            else
            {
                this.Name = "a jawbone with no owner";
                this.Hue = 0;
            }
        }

        private bool Validate(Mobile from)
        {

            if (from != owner)
            {
                from.SendMessage("This is not thy jawbone!");
                return false;
            }

            else if (this.homeMap == null)
            {
                from.SendMessage("Thy jawbone is not yet marked anywhere!");
                return false;
            }
            //else if ( from.Criminal )
            //{
            //    from.SendLocalizedMessage( 1005561, "", 0x22 ); // your criminal and cannot escape so easily
            //    return false;
            //}
            else if (Server.Spells.SpellHelper.CheckCombat(from))
            {
                from.SendLocalizedMessage(1005564, "", 0x22); // Wouldst thou flee during the heat of battle??
                return false;
            }
            else if (Server.Factions.Sigil.ExistsOn(from))
            {
                from.SendLocalizedMessage(1061632); // You can't do that while carrying the sigil.
                return false;
            }
            else if (Server.Misc.WeightOverloading.IsOverloaded(from))
            {
                from.SendLocalizedMessage(502359, "", 0x22); // Thou art too encumbered to move.
                return false;
            }
            else if (GetRemainingTimeToUse() > TimeSpan.Zero && !noWaitTime)
            {
                from.SendMessage("Thy jawbone will be ready again in {0} minutes!", (int)SerpentJawbone.GetRemainingTimeToUseForMobile(from).TotalMinutes);
                return false;
            }
            else if (SerpentJawbone.GetRemainingTimeToUseForMobile(from) > TimeSpan.Zero && oneStonePerMobile)
            {
                from.SendMessage("Thou must wait {0} minutes before using thy jawbone.", (int)SerpentJawbone.GetRemainingTimeToUseForMobile(from).TotalMinutes);
                return false;
            }
            else
            {
                return true;
            }

        }

        private bool IsInMarkAbleRegion(Mobile from)
        {
            bool house = false;
            if (from.Region is HouseRegion)
                if (((HouseRegion)from.Region).House.IsOwner(from))
                    house = true;

            if (from.Region.GetLogoutDelay(from) == TimeSpan.Zero || house || from.Region is TavernRegion)
                return true;

            return false;
        }

        private TimeSpan GetRemainingTimeToUse()
        {
            return lastUsed + delay - DateTime.Now;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            if (this.SerpentTeeth != null)
                list.Add(this.SerpentTeeth.Count + " teeth");
            else
                list.Add("0 teeth");
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(1); // version

            writer.Write((Mobile)owner);
            writer.Write((Point3D)home);
            writer.Write((DateTime)lastUsed);
            writer.Write((Map)homeMap);
            RenameThisStone();
            cleanUseList();

            writer.WriteList(m_SerpentTeeth, (w, t) => w.WriteType(t));
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();

            switch(version)
            {
                case 1:
                    {
                        owner = reader.ReadMobile();
                        home = reader.ReadPoint3D();
                        lastUsed = reader.ReadDateTime();
                        homeMap = reader.ReadMap();
                        RenameThisStone();
                        cleanUseList();
                        goto case 0;
                    }
                case 0:
                    {
                        if (m_SerpentTeeth == null)
                        {
                            m_SerpentTeeth = new List<SerpentTooth>();
                        }
                        List<Type> types = new List<Type>();
                        types = reader.ReadList(r => r.ReadType());
                        foreach (Type obj in types)
                        {
                            if (obj == typeof(SerpentToothMonitor))
                                m_SerpentTeeth.Add(Activator.CreateInstance<SerpentToothMonitor>());
                            else if (obj == typeof(SerpentToothMoonshade))
                                m_SerpentTeeth.Add(Activator.CreateInstance<SerpentToothMoonshade>());
                        }
                        break;
                    }
            }
        }

        public class GumpSerpentJawbone : Gump
        {
            public GumpSerpentJawbone(List<SerpentTooth> serpentTeeth) : base(0,0)
            {
                this.Closable=true;
			    this.Disposable=true;
			    this.Dragable=true;
			    this.Resizable=false;

			    AddPage(0);

                AddBackground(0, 0, 363, 405, 9380);
                AddLabel(140, 5, 68, @"Collected Teeth");

                foreach(SerpentTooth tooth in serpentTeeth)
                {
                    if(tooth is SerpentToothMonitor)
                        AddItem(10, 50, tooth.ItemID, tooth.Hue); //Monitor
                    //else if (tooth is SerpentToothFawn)
                        
                    else if(tooth is SerpentToothMoonshade)
                        AddItem(10, 86, tooth.ItemID, tooth.Hue); //Moonshade
                }                    

                AddLabel(60, 50, 0, @"Monitor");

                //AddItem(177, 50, 22343);
                AddLabel(227, 50, 0, @"Fawn");
                
                AddLabel(60, 86, 0, @"Moonshade");

                //AddItem(177, 86, 22343);
                AddLabel(227, 86, 0, @"Sleeping Bull");

                //AddItem(10, 122, 22343);
                AddLabel(60, 122, 0, @"Furnace");

                //AddItem(177, 122, 22343);
                AddLabel(227, 122, 0, @"Emotion");

                //AddItem(10, 158, 22343);
                AddLabel(60, 158, 0, @"Skullcrusher");

                //AddItem(177, 158, 22343);
                AddLabel(227, 158, 0, @"Balance");
                
                //AddItem(10, 194, 22343);
                AddLabel(60, 194, 0, @"Spinebreaker");

                //AddItem(177, 194, 22343);
                AddLabel(227, 194, 0, @"Discipline");

                //AddItem(10, 230, 22343);
                AddLabel(60, 230, 0, @"Monk Isle");

                //AddItem(177, 230, 22343);
                AddLabel(227, 230, 0, @"Isle of Crypts");

                //AddItem(10, 266, 22343);
                AddLabel(60, 266, 0, @"Northern Forest");

                //AddItem(177, 266, 22343);
                AddLabel(227, 266, 0, @"Logic");

                //AddItem(10, 302, 22343);
                AddLabel(60, 302, 0, @"Ethic");

                //AddItem(177, 302, 22343);
                AddLabel(227, 302, 0, @"Tolerance");

                //AddItem(10, 338, 22343);
                AddLabel(60, 338, 0, @"Enthusiasm");

                //AddItem(177, 338, 22343);
                AddLabel(227, 338, 0, @"Mad Mage Isle");
            }
        }

        public class HomeStoneSpell : Spell
        {
            private static SpellInfo m_Info = new SpellInfo("Home Stone", "", 230);
            public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds(2.5); } }
            private SerpentJawbone m_homeStone;

            public HomeStoneSpell(SerpentJawbone homeStone, Mobile caster) : base(caster, null, m_Info)
            {
                m_homeStone = homeStone;

                //          Effects.SendLocationEffect( new Point3D( caster.X + 1, caster.Y, caster.Z + 4 ), caster.Map, 0x3709, 30, 1281, 2 );
                //          Effects.SendLocationEffect( new Point3D( caster.X + 1, caster.Y, caster.Z ), caster.Map, 0x3709, 30, 1281, 2 );
                //          Effects.SendLocationEffect( new Point3D( caster.X + 1, caster.Y, caster.Z - 4 ), caster.Map, 0x3709, 30, 1281, 2 );
                //          Effects.SendLocationEffect( new Point3D( caster.X, caster.Y + 1, caster.Z + 4 ), caster.Map, 0x3709, 30, 1281, 2 );
                //          Effects.SendLocationEffect( new Point3D( caster.X, caster.Y + 1, caster.Z ), caster.Map, 0x3709, 30, 1281, 2 );
                //          Effects.SendLocationEffect( new Point3D( caster.X, caster.Y + 1, caster.Z - 4 ), caster.Map, 0x3709, 30, 1281, 2 );
                //
                //          Effects.SendLocationEffect( new Point3D( caster.X + 1, caster.Y + 1, caster.Z + 11 ), caster.Map, 0x3709, 30, 1281, 2 );
                //          Effects.SendLocationEffect( new Point3D( caster.X + 1, caster.Y + 1, caster.Z + 7 ), caster.Map, 0x3709, 30, 1281, 2 );
                //          Effects.SendLocationEffect( new Point3D( caster.X + 1, caster.Y + 1, caster.Z + 3 ), caster.Map, 0x3709, 30, 1281, 2 );
                //          Effects.SendLocationEffect( new Point3D( caster.X + 1, caster.Y + 1, caster.Z - 1 ), caster.Map, 0x3709, 30, 1281, 2 );
                caster.FixedParticles(0x3709, 10, 30, 1281, 1, 5037, EffectLayer.Waist);
            }

            public override bool ClearHandsOnCast { get { return false; } }
            public override bool RevealOnCast { get { return false; } }
            public override TimeSpan GetCastRecovery()
            {
                return TimeSpan.Zero;
            }

            public override TimeSpan GetCastDelay()
            {
                return TimeSpan.FromSeconds(3);//( (m_Mount.IsDonationItem && RewardSystem.GetRewardLevel( m_Rider ) < 3 )? 12.5 : 5.0 ) );
            }

            public override int GetMana()
            {
                return 10;
            }

            public override bool ConsumeReagents()
            {
                return false;
            }

            public override bool CheckFizzle()
            {
                return true;
            }

            private bool m_Stop;

            public void Stop()
            {
                m_Stop = true;
                Disturb(DisturbType.Hurt, false, false);
            }

            public override bool CheckDisturb(DisturbType type, bool checkFirst, bool resistable)
            {
                //if ( type == DisturbType.EquipRequest || type == DisturbType.UseRequest/* || type == DisturbType.Hurt*/ )
                //  return false;
                if (type == DisturbType.Hurt)
                    return false;
                else
                    return true;
            }

            public override void DoHurtFizzle()
            {
                if (!m_Stop)
                    base.DoHurtFizzle();
            }

            public override void DoFizzle()
            {
                if (!m_Stop)
                    base.DoFizzle();
            }

            public override void OnDisturb(DisturbType type, bool message)
            {
                if (message && !m_Stop)
                    Caster.SendMessage("You have been disrupted while attempting to use your jawbone");

                //m_Mount.UnmountMe();
            }

            public override void OnCast()
            {

                HomeStoneTeleport();
                FinishSequence();
            }

            private void HomeStoneTeleport()
            {
                if (m_homeStone.Validate(Caster))
                {
                    SerpentJawbone.RegisterUse(Caster);
                    BaseCreature.TeleportPets(Caster, m_homeStone.home, m_homeStone.homeMap, true);
                    Caster.Location = new Point3D(m_homeStone.home);
                    Caster.Map = m_homeStone.homeMap;
                    m_homeStone.LastUsed = DateTime.Now;
                    Caster.PlaySound(0x1FC);
                    Caster.FixedParticles(0x3709, 10, 30, 1281, 1, 5037, EffectLayer.Waist);
                }

            }
        }

        private class HomeStoneUse
        {
            private DateTime useTime;
            private Mobile user;
            public DateTime UseTime { get { return this.useTime; } }
            public Mobile User { get { return this.user; } }

            public HomeStoneUse(Mobile from)
            {
                useTime = DateTime.Now;
                user = from;
            }
        }
    }
}
