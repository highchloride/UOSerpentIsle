using System;
using Server;
using Server.Gumps;
using Server.ContextMenus;
using System.Collections.Generic;
using System.Collections;
using Server.Misc;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Commands;
using Server.Engines.XmlSpawner2;


namespace Server.CorpseSystem
{
    #region MinionFollowAI
    public class FollowMasterAI : BaseAI
    {
        private CorpseSummoner m_Parent;
        private CorpseMinion m_MinMobile;


        public FollowMasterAI(BaseCreature m)
            : base(m)
        {
            if (m is CorpseMinion)
                m_MinMobile = (CorpseMinion)m;



        }
        public override void WalkRandom(int iChanceToNotMove, int iChanceToDir, int iSteps)
        {
            FollowMaster();
        }

        public override void OnCurrentOrderChanged()
        {
            FollowMaster();
        }
        public bool FollowMaster()
        {
            if (m_MinMobile.Deleted || m_MinMobile == null)
                return false;

            m_Parent = m_MinMobile.Parent;

            if (m_Parent != null && !m_Parent.Deleted)
                return WalkMobileRange(m_Parent, 3, false, 1, 2);

            return false;

        }

    }
    #endregion

    #region CorpseMinion
    public class CorpseMinion : Banker
    {
        private bool m_IsTransformed;
        private bool m_IsBanker;
        private Timer m_TransformTimer;
        private CorpseSummoner m_Parent;

        protected override BaseAI ForcedAI { get { return new FollowMasterAI(this); } }

        public CorpseSummoner Parent
        {
            get
            {
                return m_Parent;
            }
            set
            {
                m_Parent = value;
            }
        }
        public bool IsBanker
        {
            get
            {
                return m_IsBanker;
            }
            set
            {
                m_IsBanker = value;
            }
        }

        public bool IsTransformed
        {
            get
            {
                return m_IsTransformed;
            }
            set
            {
                m_IsTransformed = value;
            }
        }
        [Constructable]
        public CorpseMinion(CorpseSummoner cs)
        {
            m_IsTransformed = false;

            ActiveSpeed = cs.ActiveSpeed;
            PassiveSpeed = cs.PassiveSpeed;
            CurrentSpeed = cs.CurrentSpeed;

            m_Parent = cs;
            m_IsBanker = cs.IsBanker;

            BodyValue = 776;
            Name = "an enslaved minion";
            RangePerception = 100;
            SpeechHue = 33;
            Title = "";
        }
        //begin bank override
        public override void InitSBInfo()
        {
            if (m_IsBanker)
                base.InitSBInfo();
        }
        public override bool HandlesOnSpeech(Mobile from)
        {
            if (m_IsBanker)
            {
                base.HandlesOnSpeech(from);
                return true;
            }
            else
                return false;
        }
        public override void OnSpeech(SpeechEventArgs e)
        {
            if (m_IsBanker)
                base.OnSpeech(e);
        }
        public override void AddCustomContextEntries(Mobile from, List<ContextMenuEntry> list)
        {
            if (m_IsBanker)
                base.AddCustomContextEntries(from, list);
        }

        public void Transform(Mobile m)
        {
            if (m == null)
                return;

            m.FixedParticles(0x3728, 1, 13, 9912, 1150, 7, EffectLayer.Head);
            m.FixedParticles(0x3779, 1, 15, 9502, 67, 7, EffectLayer.Head);

            Direction = GetDirectionTo(m);

            int[] sounds = { 1115, 1114, 1113, 1112, 1153, 1150 };
            string[] msgs = {"Unleashed!! I feed again.", "The many souls I feed on...", 
                             "Yet another corpse to leech from.", "Shackled into such a horrid service..", "I will eat more than your corpse next time!!" };



            System.Random r = new Random();


            if (IsTransformed)
                return;

            Effects.PlaySound(this.Location, this.Map, sounds[r.Next(0, 5)]);
            Say(msgs[r.Next(0, 5)]);

            this.FixedParticles(0x375A, 1, 17, 9919, 33, 7, EffectLayer.Waist);
            this.FixedParticles(0x3728, 1, 13, 9502, 33, 7, (EffectLayer)255);

            this.BodyValue = 303;

            new ExpireTimer(this, new TimeSpan(0, 0, 10)).Start();

            m_IsTransformed = true;
        }


        private class ExpireTimer : Timer
        {
            private CorpseMinion m_CorpseMinion;

            private DateTime m_End;

            public ExpireTimer(CorpseMinion mobile, TimeSpan delay)
                : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0))
            {
                m_CorpseMinion = mobile;

                m_End = DateTime.UtcNow + delay;

                Priority = TimerPriority.TwoFiftyMS;
            }

            protected override void OnTick()
            {
                if (m_CorpseMinion.Deleted || DateTime.UtcNow >= m_End)
                {
                    Effects.SendLocationParticles(EffectItem.Create(m_CorpseMinion.Location, m_CorpseMinion.Map, EffectItem.DefaultDuration), 0x37CC, 1, 40, 97, 3, 9917, 0);
                    //m_CorpseMinion.FixedParticles(0x374A, 1, 15, 9502, 97, 3, (EffectLayer)255);
                    m_CorpseMinion.BodyValue = 776;

                    Effects.PlaySound(m_CorpseMinion.Location, m_CorpseMinion.Map, 1200);

                    m_CorpseMinion.IsTransformed = false;

                    m_CorpseMinion.PublicOverheadMessage(MessageType.Emote, 0, false, "the creature returns to its suppressed state");

                    Stop();

                    //cleanup
                    foreach (Item i in m_CorpseMinion.GetItemsInRange(4))
                    {
                        if (i is Corpse)
                        {
                            if (!i.Deleted && CorpseBookAtt.IsEmpty((Corpse)i))
                            {
                                Effects.SendLocationParticles(EffectItem.Create(i.Location, i.Map, EffectItem.DefaultDuration), 0x36B0, 1, 14, 63, 7, 9915, 0);
                                i.Delete();
                            }
                        }
                    }
                }
            }
        }
        public CorpseMinion(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version

            writer.Write((bool)m_IsBanker);
            writer.Write((CorpseSummoner)m_Parent);

        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            //return to normal state
            m_IsTransformed = false;

            BodyValue = 776;

            m_IsBanker = reader.ReadBool();
            m_Parent = reader.ReadMobile() as CorpseSummoner;

        }
    }
    #endregion

    #region CorpseSummoner
    public class CorpseSummoner : BaseHealer
    {
        private int m_ResPrice;
        private int m_CorpsePrice;
        private bool m_CanRes;
        private bool m_IsBanker;
        private CorpseMinion m_Minion;

        public override bool IsInvulnerable { get { return true; } }
        public override bool CanTeach { get { return false; } }
        public override bool IsActiveVendor { get { return false; } }
        public override bool IsActiveBuyer { get { return false; } }
        public override bool IsActiveSeller { get { return false; } }

        public CorpseMinion Minion
        {
            get
            {
                return m_Minion;
            }
            set
            {
                m_Minion = value;
            }
        }


        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsBanker
        {
            get
            {
                return m_IsBanker;
            }
            set
            {
                m_IsBanker = value;

                TrySpawnMinion();
                m_Minion.IsBanker = value;
            }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public bool CanRes
        {
            get
            {
                return m_CanRes;
            }
            set
            {
                m_CanRes = value;
            }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public int ResPrice
        {
            get
            {
                return m_ResPrice;
            }
            set
            {
                m_ResPrice = value;
            }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public int CorspePrice
        {
            get
            {
                return m_CorpsePrice;
            }
            set
            {
                m_CorpsePrice = value;
            }
        }


        [Constructable]
        public CorpseSummoner()
        {
            m_ResPrice = 0;
            m_CorpsePrice = 3000;
            m_CanRes = true;
            m_IsBanker = false;

            Title = "the corpse summoner";

        }

        public override void InitOutfit()
        {

            AddItem(new Scythe());
            AddItem(new HoodedShroudOfShadows());
            AddItem(new Sandals());

        }

        public override void AddCustomContextEntries(Mobile from, List<ContextMenuEntry> list)
        {
            if (from.Alive)
            {
                list.Add(new CorpseEntry(from, this));
            }

            base.AddCustomContextEntries(from, list);
        }
        public override void OfferResurrection(Mobile m)
        {
            if (!CanRes)
                return;

            if (m_ResPrice <= 0)
            {
                base.OfferResurrection(m);
            }
            else
            {
                Direction = GetDirectionTo(m);

                m.PlaySound(0x214);
                m.FixedEffect(0x376A, 10, 16);

                m.CloseGump(typeof(ResurrectGump));
                m.SendGump(new ResurrectGump(m, this, m_ResPrice));
            }
        }
        public override void OnDoubleClick(Mobile m)
        {
            if (InRange(m, 4) && InLOS(m))
                GetCorpse(m);
        }
        public void GetCorpse(Mobile m)
        {
            m.CloseGump(typeof(CorpseMenu));
            m.SendGump(new CorpseMenu(m, m_CorpsePrice, this));
        }

        //method doubles as null protection - used often
        public void TrySpawnMinion()
        {
            if (m_Minion != null && !m_Minion.Deleted)
                return;

            PublicOverheadMessage(MessageType.Spell, 908, true, "Ahn Durst Ku");
            Animate(200, 2, 2, true, true, 2);

            CorpseMinion cm = new CorpseMinion(this);

            m_Minion = cm;

            cm.OnBeforeSpawn(this.Location, this.Map);
            InvalidateProperties(); //on spawner ??

            cm.MoveToWorld(this.Location, this.Map);

            cm.OnAfterSpawn();

        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            TrySpawnMinion();

            //lost minion
            if (!InRange(m_Minion, 6))
                m_Minion.Delete();

            //invoke follow 
            m_Minion.ControlOrder = OrderType.Follow;

            base.OnMovement(m, oldLocation);
        }

        public override void OnDelete()
        {
            if (m_Minion != null)
                m_Minion.Delete();

            base.OnDelete();
        }
        public void TransformMinion(Mobile m)
        {

            TrySpawnMinion();

            m_Minion.Transform(m);

        }


        public CorpseSummoner(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version

            writer.Write((int)m_ResPrice);
            writer.Write((int)m_CorpsePrice);
            writer.Write((bool)m_CanRes);
            writer.Write((bool)m_IsBanker);
            writer.Write((CorpseMinion)m_Minion);



        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            m_ResPrice = reader.ReadInt();
            m_CorpsePrice = reader.ReadInt();
            m_CanRes = reader.ReadBool();
            m_IsBanker = reader.ReadBool();
            m_Minion = reader.ReadMobile() as CorpseMinion;
        }
    }
    #endregion

    #region CorpseMod
    public class CorpseMod
    {
        public static void CorpseCheck(PlayerMobile pm)
        {
            Mobile m = (Mobile)pm;

            if (m.Backpack == null)
                return;

            //   Get CorpseBookAtt attachment 
            CorpseBookAtt book = new CorpseBookAtt();

            // does player already have a CorpseBookAtt attachment?
            if (XmlAttach.FindAttachment(m, typeof(CorpseBookAtt)) == null)
            {
                // They do not have an attachment yet
                // Add the Attachment
                XmlAttach.AttachTo(m, book);
                book.Player = m;
                book.Update((Corpse)m.Corpse);
            }
            else
            {
                // they have the attachment just load their data
                book = (CorpseBookAtt)XmlAttach.FindAttachment(m, typeof(CorpseBookAtt));
                book.Update((Corpse)m.Corpse);
            }
            /*
            CorpseBookAtt book = (CorpseBookAtt)(m.Backpack.FindItemByType(typeof(CorpseBookAtt)));

            if (book != null)
                book.Update((Corpse)m.Corpse);
            else
            {
                m.AddToBackpack(book = new CorpseBookAtt(m));
                book.Update((Corpse)m.Corpse);
            }*/
        }
    }
    #endregion

    #region CorpseMenu
    public class CorpseMenu : Gump
    {
        private CorpseSummoner parent;
        private Mobile caller;
        private int Price;
        private int hue;


        //alias of Book's List
        private List<Corpse> Corpses;

        public CorpseMenu(Mobile from, int price, CorpseSummoner mob)
            : this()
        {
            parent = mob;

            caller = from;
            Price = price;

            Corpses = UploadBook(from);
            FillMenu();
        }

        public CorpseMenu()
            : base(0, 0)
        {
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            AddPage(0);


            AddImageTiled(269, 75, 296, 412, 9274);
            AddImageTiled(268, 75, 295, 4, 9107);
            AddImageTiled(266, 486, 301, 4, 9107);
            AddImage(381, 103, 9804);

            AddLabel(291, 215, 1541, @"Minutes Until Decay");
            AddLabel(369, 83, 47, @"Corpse Summon");



            AddButton(359, 439, 2450, 2451, (int)Buttons.OkayButton, GumpButtonType.Reply, 0);
            AddButton(434, 439, 2453, 2454, (int)Buttons.CancelButton, GumpButtonType.Reply, 0);

            AddLabel(299, 153, 55, @"I will summon your corpse");
            AddLabel(343, 193, 55, @".. but you will have to pay a price.");
            AddItem(543, 399, 8708);
            AddItem(543, 312, 8708);
            AddItem(544, 226, 8708);
            AddItem(545, 141, 8708);
            AddItem(545, 73, 8708);
            AddItem(244, 401, 8708);
            AddItem(244, 314, 8708);
            AddItem(245, 228, 8708);
            AddItem(246, 143, 8708);
            AddItem(247, 73, 8708);
            AddImageTiled(331, 185, 213, 2, 9157);
            AddImageTiled(304, 181, 213, 2, 9107);
            AddImageTiled(327, 425, 213, 1, 9107);
            AddImage(270, 396, 111);



        }
        public enum Buttons
        {
            OkayButton = 5,
            CancelButton,
        }
        public void NoCorpseImage()
        {
            AddLabel(299, 338, 32, @"You have no corpse for me to summon.");
            AddImage(378, 254, 7026, 0);
        }
        //Green(57) Yellow(47)  Red(37)
        private int ReadOutHue(int minute)
        {

            if (minute >= 0 && minute <= 3)
                return 57;
            if (minute >= 4 && minute <= 9)
                return 47;
            else
                return 37;
        }
        //Note: Default decay time is 14* minutes
        private String ReadOut(Corpse c)
        {
            TimeSpan lapsed;
            String readOut;

            lapsed = (DateTime.UtcNow).Subtract(c.TimeOfDeath);
            hue = ReadOutHue(lapsed.Minutes);

            readOut = "   " + (14 - lapsed.Minutes) + "    Items: "
                + c.GetTotal(TotalType.Items) + "  Gold: " + (c.GetTotal(TotalType.Gold) / 1000) + " k";

            return readOut;

        }
        public void FillMenu()
        {

            String str;
            int radioNum = 0;

            int yPosRadio = 242, yPosLabel = 239;

            if (Price == 0)
                AddLabel(380, 398, 337, @"Price: Free");
            else
                AddLabel(380, 398, 337, @"Price: " + Price);

            //player has no book
            if (Corpses == null)
            {
                Corpse c = (Corpse)caller.Corpse;

                //try for last corpse
                if (c != null && !c.Deleted)
                {
                    Corpses = new List<Corpse>();
                    Corpses.Add((Corpse)caller.Corpse);
                }
                else
                {
                    NoCorpseImage();
                    return;
                }

            }

            //book found
            for (int index = 0; index < Corpses.Count; index++)
            {
                try
                {
                    //'filters' 
                    if (Corpses[index] != null && !Corpses[index].Deleted)
                    {

                        str = ReadOut(Corpses[index]);

                        AddRadio(289, yPosRadio, 5230, 5224, false, radioNum);
                        AddLabel(320, yPosLabel, hue, @str);

                        radioNum++;
                        yPosRadio += 30;
                        yPosLabel += 30;
                    }
                    //corpse no longer meets 'requirements'
                    else
                    {
                        Corpses.RemoveAt(index);
                    }

                }

                catch (System.ArgumentNullException)
                {
                }
                catch (System.ArgumentException)
                {
                }

            }

        }


        public List<Corpse> UploadBook(Mobile m)
        {

            CorpseBookAtt book;
            PlayerMobile from;


            from = m as PlayerMobile;

            //   Get CorpseBookAtt attachment 
            //CorpseBookAtt book = new CorpseBookAtt();

            // does player already have a CorpseBookAtt attachment?
            //if (XmlAttach.FindAttachment(from, typeof(CorpseBookAtt)) == null)
            //{
            // They do not have an attachment yet
            // Add the Attachment
            //   XmlAttach.AttachTo(from, book);
            //book.Update((Corpse)m.Corpse);
            //}
            // else
            //{
            // they have the attachment just load their data
            book = (CorpseBookAtt)XmlAttach.FindAttachment(from, typeof(CorpseBookAtt));
            //book.Update((Corpse)m.Corpse);
            // }

            //book = (CorpseBookAtt)(from.Backpack.FindItemByType(typeof(CorpseBookAtt)));

            //will short circuit
            if (book == null || book.Player == null || book.Corpses.Count == 0)
                return null;

            return book.Corpses;
        }

        public void Summon(Corpse c, int pos)
        {
            if (parent != null && !parent.Deleted && parent.InRange(caller, 4) && parent.InLOS(caller))
            {
                //must factor in possible delay
                if (c == null || c.Deleted)
                {
                    caller.SendMessage("Your corpse is beyond my reach, it must have rotted.");
                    return;
                }
                if (Price > 0)
                {

                    if (Banker.Withdraw(caller, Price))
                    {
                        caller.SendLocalizedMessage(1060398, Price.ToString()); // Amount charged
                        caller.SendLocalizedMessage(1060022, Banker.GetBalance(caller).ToString()); // Amount left, caller bank
                    }
                    else
                    {
                        caller.SendMessage("Unfortunately, you do not have enough cash in your bank to summon your corpse");
                        return;
                    }
                }
                //act
                parent.TransformMinion(caller);


                //move corpse


                c.Map = caller.Map;
                c.Location = caller.Location;

                Corpses.RemoveAt(pos);
            }
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            int selected = -1;

            switch (info.ButtonID)
            {

                case (int)Buttons.OkayButton:
                    {


                        //no radio buttons exist (no options)
                        try
                        {       //only one may be selected
                            selected = info.Switches[0];

                        }
                        catch (IndexOutOfRangeException)
                        {
                            from.SendMessage("You did not select a corpse to summon.");
                            break;
                        }

                        if (selected < 0)
                        {
                            from.SendMessage("You did not select a corpse to summon.");
                            break;
                        }

                        Summon(Corpses[selected], selected);

                        from.CloseGump(typeof(CorpseMenu));

                        break;
                    }
                case (int)Buttons.CancelButton:
                    {
                        from.CloseGump(typeof(CorpseMenu));
                        break;
                    }

            }
        }
    }
    #endregion

    #region CorpseEntry
    public class CorpseEntry : ContextMenuEntry
    {
        private CorpseSummoner m_Vendor;

        public CorpseEntry(Mobile from, CorpseSummoner vendor)
            : base(6215, 2)
        {
            m_Vendor = vendor;
            Enabled = true;
        }

        public override void OnClick()
        {
            m_Vendor.GetCorpse(this.Owner.From);
        }
    }
    #endregion

    #region CorpseBookAtt Attachment
    public class CorpseBookAtt : XmlAttachment
    {

        public static void Initialize()
        {
            EventSink.PlayerDeath += new PlayerDeathEventHandler(EventSink_CorpseCheck);
        }

        private static void EventSink_CorpseCheck(PlayerDeathEventArgs e)
        {
            PlayerMobile owner = e.Mobile as PlayerMobile;

            if (owner == null)
                return;

            //Corpse Retrieval System Mod
            CorpseSystem.CorpseMod.CorpseCheck(owner);
        }

        //Corpses stored
        private const int NUM_MAX = 5;

        private Mobile m_Player;
        private List<Corpse> m_Corpses;
        private Corpse m_LastValidCorpse;


        [Attachable]
        public CorpseBookAtt()
        {
            m_Corpses = new List<Corpse>();
        }

        [Attachable]
        public CorpseBookAtt(Mobile m)
        {
            m_Player = m;
            m_Corpses = new List<Corpse>();
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Player
        {
            get
            {
                return m_Player;
            }
            set
            {
                m_Player = value;
                m_Corpses = new List<Corpse>();
                m_LastValidCorpse = null;
            }
        }

        public List<Corpse> Corpses
        {
            get
            {
                return m_Corpses;
            }
            set
            {
                m_Corpses = value;
            }
        }

        public Corpse LastValidCorpse
        {
            get
            {
                return m_LastValidCorpse;
            }
            set
            {
                Update(value);
            }
        }
        //older corpses shift left
        private void AddCorpse(Corpse c)
        {
            //full - this will rarely happen
            if (m_Corpses.Count >= NUM_MAX)
            {
                m_Corpses.RemoveAt(0);
                m_Corpses.Add(c);
            }
            else
            {
                m_Corpses.Add(c);
            }

        }
        public void Update(Corpse c)
        {
            if (m_Player == null || NeedRemoval(c))
                return;

            m_LastValidCorpse = c;

            AddCorpse(c);

        }

        public static bool NeedRemoval(Corpse c)
        {
            if (c == null || c.Deleted || IsEmpty(c))
                return true;
            else
                return false;
        }

        public static bool IsEmpty(Corpse c)
        {
            if (c.GetTotal(TotalType.Gold) > 0 || c.GetTotal(TotalType.Items) > 0)
                return false;

            return true;
        }
        public static Item CorpseToItem(Corpse c)
        {
            return (Item)c;
        }
        public static Corpse ItemToCorpse(Item i)
        {
            return (Corpse)i;
        }
        public CorpseBookAtt(ASerial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            m_Corpses.ConvertAll(new System.Converter<Corpse, Item>(CorpseToItem));
            writer.Write((int)0); // version

            writer.Write(m_Player);
            writer.WriteItemList(m_Corpses);
            writer.Write(m_LastValidCorpse);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            // version 0

            int version = reader.ReadInt();

            m_Player = reader.ReadMobile();
            m_Corpses = (reader.ReadStrongItemList()).ConvertAll(new System.Converter<Item, Corpse>(ItemToCorpse));
            m_LastValidCorpse = reader.ReadItem() as Corpse;
        }
    }

    #endregion

    //#region CorpseBook
    /*
    public class CorpseBook : Item
    {
        public static void Initialize()
        {
            EventSink.PlayerDeath += new PlayerDeathEventHandler(EventSink_CorpseCheck);
        }

        private static void EventSink_CorpseCheck(PlayerDeathEventArgs e)
        {
            PlayerMobile owner = e.Mobile as PlayerMobile;

            if (owner == null)
                return;

            //Corpse Retrieval System Mod
            CorpseSystem.CorpseMod.CorpseCheck(owner);

        }

        //Corpses stored
        private const int NUM_MAX = 5;

        private Mobile m_Player;
        private List<Corpse> m_Corpses;
        private Corpse m_LastValidCorpse;


        [Constructable]
        public CorpseBook()
            : base(8787)
        {
            m_Corpses = new List<Corpse>();

            LootType = LootType.Blessed;
            Visible = false;
            Name = "Corpse Book [Do not delete]";
            Hue = 1109;
            Weight = 0;
        }

        public CorpseBook(Mobile m): base(8787)
        {
            m_Player = m;
            m_Corpses = new List<Corpse>();

            LootType = LootType.Blessed;
            Visible = false;
            Name = "Corpse Book [Do not delete]";
            Hue = 1109;
            Weight = 0;

        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Player
        {
            get
            {
                return m_Player;
            }
            set
            {
                m_Player = value;
                m_Corpses = new List<Corpse>();
                m_LastValidCorpse = null;
            }
        }

        public List<Corpse> Corpses
        {
            get
            {
                return m_Corpses;
            }
            set
            {
                m_Corpses = value;
            }
        }


        public Corpse LastValidCorpse
        {
            get
            {
                return m_LastValidCorpse;
            }
            set
            {
                Update(value);
            }
        }
        //older corpses shift left
        private void AddCorpse(Corpse c)
        {
            //full - this will rarely happen
            if (m_Corpses.Count >= NUM_MAX)
            {
                m_Corpses.RemoveAt(0);
                m_Corpses.Add(c);
            }
            else
            {
                m_Corpses.Add(c);
            }

        }

        public void Update(Corpse c)
        {
            if (m_Player == null || NeedRemoval(c))
                return;

            m_LastValidCorpse = c;

            AddCorpse(c);

        }

        public static bool NeedRemoval(Corpse c)
        {
            if (c == null || c.Deleted || IsEmpty(c))
                return true;
            else
                return false;
        }

        public static bool IsEmpty(Corpse c)
        {
            if (c.GetTotal(TotalType.Gold) > 0 || c.GetTotal(TotalType.Items) > 0)
                return false;

            return true;
        }
        public static Item CorpseToItem(Corpse c)
        {
            return (Item)c;
        }
        public static Corpse ItemToCorpse(Item i)
        {
            return (Corpse)i;
        }
        public CorpseBook(Serial serial)
            : base(serial)
        {
        }


        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);


            m_Corpses.ConvertAll(new System.Converter<Corpse, Item>(CorpseToItem));
            writer.Write((int)0); // version

            writer.Write(m_Player);
            writer.WriteItemList(m_Corpses);
            writer.Write(m_LastValidCorpse);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            m_Player = reader.ReadMobile();
            m_Corpses = (reader.ReadStrongItemList()).ConvertAll(new System.Converter<Item, Corpse>(ItemToCorpse));
            m_LastValidCorpse = reader.ReadItem() as Corpse;
        }
    }
     */
    //#endregion

}
