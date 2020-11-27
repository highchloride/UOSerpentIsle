using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;
using System.Collections.Generic;

namespace Server.Multis
{
    public enum CampType
    {
        Default,
        //EvilMage,
        //GoodMage,
        //Warlord,
        //Gypsy,
        Brigand,
        Lizardman,
        Ratman,
        Orc
    }



    public abstract class BaseCamp : BaseMulti
    {
        private List<Item> m_Items;
        private List<Mobile> m_Mobiles;
        private DateTime m_DecayTime;
        private Timer m_DecayTimer;
        private TimeSpan m_DecayDelay;
        #region BBS Quests
        private Mobile m_Prisoner;
        private CampType m_Camp;
        #endregion

        public static void Initialize()
        {
            Timer.DelayCall(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5), OnTick);
        }

        public static List<BaseCamp> _Camps = new List<BaseCamp>();

        //private List<Item> m_Items;
        //private List<Mobile> m_Mobiles;

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime TimeOfDecay { get; set; }

        //[CommandProperty(AccessLevel.GameMaster)]
        //public BaseCreature Prisoner { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public BaseContainer Treasure1 { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public BaseContainer Treasure2 { get; set; }

        //public override bool HandlesOnMovement
        //{
        //    get { return true; }
        //}

        //[CommandProperty(AccessLevel.GameMaster)]
        //public virtual TimeSpan DecayDelay { get { return TimeSpan.FromMinutes(30.0); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Decaying { get { return TimeOfDecay != DateTime.MinValue; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool ForceDecay
        {
            get { return false; }
            set { SetDecayTime(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool RestrictDecay { get; set; }

        public BaseCamp(int multiID)
            : base(multiID)
        {
            this.m_Items = new List<Item>();
            this.m_Mobiles = new List<Mobile>();
            this.m_DecayDelay = TimeSpan.FromMinutes(30.0);
            this.RefreshDecay(true);
            #region BBS Quests
            m_Camp = CampType.Default;
            #endregion

            Timer.DelayCall(TimeSpan.Zero, new TimerCallback(CheckAddComponents));
        }

        public BaseCamp(Serial serial)
            : base(serial)
        {
        }

        public virtual int EventRange
        {
            get
            {
                return 10;
            }
        }
        #region BBS Quests
        public virtual Mobile Prisoner
        {
            get { return m_Prisoner; }
            set
            {
                m_Prisoner = value;


            }
        }

        public virtual CampType Camp { get { return m_Camp; } set { m_Camp = value; } }

        public BulletinMessage Message { get; set; }
        #endregion

        public virtual TimeSpan DecayDelay
        {
            get
            {
                return this.m_DecayDelay;
            }
            set
            {
                this.m_DecayDelay = value;
                this.RefreshDecay(true);
                this.m_Camp = CampType.Default;
            }
        }
        public override bool HandlesOnMovement
        {
            get
            {
                return true;
            }
        }
        public void CheckAddComponents()
        {
            if (this.Deleted)
                return;

            this.AddComponents();
        }

        public virtual void AddComponents()
        {
        }

        public virtual void CheckDecay()
        {
            if (RestrictDecay)
                return;

            if (!Decaying)
            {
                if (((Treasure1 == null || Treasure1.Items.Count == 0) && (Treasure2 == null || Treasure2.Items.Count == 0)) ||
                    (Prisoner != null && (Prisoner.Deleted || !Prisoner.CantWalk)))
                {
                    SetDecayTime();
                }
            }
            else if (TimeOfDecay < DateTime.UtcNow)
            {
                Delete();
            }
        }

        public virtual void SetDecayTime()
        {
            if (Deleted || RestrictDecay)
                return;

            TimeOfDecay = DateTime.UtcNow + DecayDelay;
        }


        public virtual void RefreshDecay(bool setDecayTime)
        {
            if (this.Deleted)
                return;

            if (this.m_DecayTimer != null)
                this.m_DecayTimer.Stop();

            if (setDecayTime)
                this.m_DecayTime = DateTime.UtcNow + this.DecayDelay;

            this.m_DecayTimer = Timer.DelayCall(this.DecayDelay, new TimerCallback(Delete));
        }

        public virtual void AddItem(Item item, int xOffset, int yOffset, int zOffset)
        {
            this.m_Items.Add(item);

            int zavg = this.Map.GetAverageZ(this.X + xOffset, this.Y + yOffset);
            item.MoveToWorld(new Point3D(this.X + xOffset, this.Y + yOffset, zavg + zOffset), this.Map);
        }
        #region BBS Quest Statics
        public virtual void SpawnPrison()
        {
            AddItem(new Static(0x1), 0, 0, 0);
            AddItem(new Static(0x821), -1, -1, 0);
            AddItem(new Static(0x821), -1, 0, 0);
            AddItem(new Static(0x822), -1, 1, 0);
            AddItem(new Static(0xC10), 2, 0, 0);
            AddItem(new Static(0xB3C), 2, 1, 0);
            AddItem(new Static(0xB5C), 2, 2, 0);
            AddItem(new Static(0x826), -2, 1, 0);
            AddItem(new Static(0x821), -1, -2, 0);
            AddItem(new Static(0x1B7C), -2, -1, 0);
            AddItem(new Static(0x1B1E), -2, -2, 0);
            AddItem(new Static(0x97D), 2, 1, 6);
            AddItem(new Static(0xB5D), 3, 1, 0);
            AddItem(new Static(0x974), 1, 3, 0);
            AddItem(new Static(0xFAC), 1, 3, 0);
            AddItem(new Static(0x1640), 1, -3, 0);
            AddItem(new Static(0x821), -3, 0, 0);
            AddItem(new Static(0x821), -3, -1, 0);
            AddItem(new Static(0x821), -3, -2, 0);
            AddItem(new Static(0x823), -1, -3, 0);
            AddItem(new Static(0x823), -2, -3, 0);
            AddItem(new Static(0x821), -3, 1, 0);
            AddItem(new Static(0xE83), 3, -2, 0);
            AddItem(new Static(0x1260), 4, -2, 0);
            //AddItem(new Static(0xE7C), 4, 4, 0); chest

        }
        public virtual void SpawnBlueTent()
        {
            // Below is the component list for a blue tent.
            AddItem(new Static(0x01F3), 0, 0, 0);
            AddItem(new Static(0x01F8), 0, 1, 0);
            AddItem(new Static(0x01F8), 0, 2, 0);
            AddItem(new Static(0x01F8), 0, 3, 0);
            AddItem(new Static(0x01F8), 0, 4, 0);
            AddItem(new Static(0x01F8), 0, 5, 0);
            AddItem(new Static(0x01F7), 0, 6, 0);
            AddItem(new Static(0x01F4), 0, 7, 0);
            AddItem(new Static(0x01F9), 1, 0, 0);
            AddItem(new Static(0x0606), 1, 1, 20);
            AddItem(new Static(0x05FF), 1, 2, 20);
            AddItem(new Static(0x05FF), 1, 3, 20);
            AddItem(new Static(0x05FF), 1, 4, 20);
            AddItem(new Static(0x05FF), 1, 5, 20);
            AddItem(new Static(0x05FF), 1, 6, 20);
            AddItem(new Static(0x01F1), 1, 7, 0);
            AddItem(new Static(0x0607), 1, 7, 20);
            AddItem(new Static(0x01F9), 2, 0, 0);
            AddItem(new Static(0x0600), 2, 1, 20);
            AddItem(new Static(0x0606), 2, 2, 23);
            AddItem(new Static(0x05FF), 2, 3, 23);
            AddItem(new Static(0x05FF), 2, 4, 23);
            AddItem(new Static(0x05FF), 2, 5, 23);
            AddItem(new Static(0x0607), 2, 6, 23);
            AddItem(new Static(0x01F1), 2, 7, 0);
            AddItem(new Static(0x0601), 2, 7, 20);
            AddItem(new Static(0x01F9), 3, 0, 0);
            //AddItem(new Static(0x0E43),   3,   1,  0); // chest
            AddItem(new Static(0x0600), 3, 1, 20);
            AddItem(new Static(0x0600), 3, 2, 23);
            AddItem(new Static(0x0001), 3, 3, 0);
            AddItem(new Static(0x0606), 3, 3, 26);
            AddItem(new Static(0x05FF), 3, 4, 26);
            AddItem(new Static(0x0607), 3, 5, 26);
            AddItem(new Static(0x0601), 3, 6, 23);
            AddItem(new Static(0x0601), 3, 7, 20);
            AddItem(new Static(0x01F9), 4, 0, 0);
            AddItem(new Static(0x0600), 4, 1, 20);
            AddItem(new Static(0x0600), 4, 2, 23);
            AddItem(new Static(0x0600), 4, 3, 26);
            AddItem(new Static(0x0608), 4, 4, 34);
            AddItem(new Static(0x0601), 4, 5, 26);
            AddItem(new Static(0x0601), 4, 6, 23);
            AddItem(new Static(0x0601), 4, 7, 20);
            AddItem(new Static(0x01F9), 5, 0, 0);
            AddItem(new Static(0x0600), 5, 1, 20);
            AddItem(new Static(0x0600), 5, 2, 23);
            AddItem(new Static(0x0605), 5, 3, 26);
            AddItem(new Static(0x0602), 5, 4, 26);
            AddItem(new Static(0x0604), 5, 5, 26);
            AddItem(new Static(0x0601), 5, 6, 23);
            AddItem(new Static(0x01F1), 5, 7, 0);
            AddItem(new Static(0x0601), 5, 7, 20);
            AddItem(new Static(0x01F6), 6, 0, 0);
            AddItem(new Static(0x0600), 6, 1, 20);
            AddItem(new Static(0x0605), 6, 2, 23);
            AddItem(new Static(0x0602), 6, 3, 23);
            AddItem(new Static(0x0602), 6, 4, 23);
            AddItem(new Static(0x0602), 6, 5, 23);
            AddItem(new Static(0x0604), 6, 6, 23);
            AddItem(new Static(0x01F1), 6, 7, 0);
            AddItem(new Static(0x0601), 6, 7, 20);
            AddItem(new Static(0x01F5), 7, 0, 0);
            AddItem(new Static(0x01F2), 7, 1, 0);
            AddItem(new Static(0x0605), 7, 1, 20);
            AddItem(new Static(0x01F2), 7, 2, 0);
            AddItem(new Static(0x0602), 7, 2, 20);
            AddItem(new Static(0x01F2), 7, 3, 0);
            AddItem(new Static(0x0602), 7, 3, 20);
            AddItem(new Static(0x01F2), 7, 4, 0);
            AddItem(new Static(0x0602), 7, 4, 20);
            AddItem(new Static(0x01F2), 7, 5, 0);
            AddItem(new Static(0x0602), 7, 5, 20);
            AddItem(new Static(0x01F2), 7, 6, 0);
            AddItem(new Static(0x0602), 7, 6, 20);
            AddItem(new Static(0x01F0), 7, 7, 0);
            AddItem(new Static(0x0604), 7, 7, 20);
        }

        public virtual void SpawnGreenTent()
        {
            AddItem(new Static(0x0233), 0, 0, 0);
            AddItem(new Static(0x0238), 0, 1, 0);
            AddItem(new Static(0x0238), 0, 2, 0);
            AddItem(new Static(0x0238), 0, 3, 0);
            AddItem(new Static(0x0238), 0, 4, 0);
            AddItem(new Static(0x0238), 0, 5, 0);
            AddItem(new Static(0x0237), 0, 6, 0);
            AddItem(new Static(0x0234), 0, 7, 0);
            AddItem(new Static(0x0239), 1, 0, 0);
            AddItem(new Static(0x061E), 1, 1, 20);
            AddItem(new Static(0x0617), 1, 2, 20);
            AddItem(new Static(0x0617), 1, 3, 20);
            AddItem(new Static(0x0617), 1, 4, 20);
            AddItem(new Static(0x0617), 1, 5, 20);
            AddItem(new Static(0x0617), 1, 6, 20);
            AddItem(new Static(0x0231), 1, 7, 0);
            AddItem(new Static(0x061F), 1, 7, 20);
            AddItem(new Static(0x0239), 2, 0, 0);
            AddItem(new Static(0x0618), 2, 1, 20);
            AddItem(new Static(0x061E), 2, 2, 23);
            AddItem(new Static(0x0617), 2, 3, 23);
            AddItem(new Static(0x0617), 2, 4, 23);
            AddItem(new Static(0x0617), 2, 5, 23);
            AddItem(new Static(0x061F), 2, 6, 23);
            AddItem(new Static(0x0231), 2, 7, 0);
            AddItem(new Static(0x0619), 2, 7, 20);
            AddItem(new Static(0x0239), 3, 0, 0);
            //AddItem(new Static(chestid),     3,     0,    0);
            AddItem(new Static(0x0618), 3, 1, 20);
            AddItem(new Static(0x0618), 3, 2, 23);
            AddItem(new Static(0x0001), 3, 3, 0);
            AddItem(new Static(0x061E), 3, 3, 26);
            AddItem(new Static(0x0617), 3, 4, 26);
            AddItem(new Static(0x061F), 3, 5, 26);
            AddItem(new Static(0x0619), 3, 6, 23);
            AddItem(new Static(0x0619), 3, 7, 20);
            AddItem(new Static(0x0239), 4, 0, 0);
            AddItem(new Static(0x0618), 4, 1, 20);
            AddItem(new Static(0x0618), 4, 2, 23);
            AddItem(new Static(0x0618), 4, 3, 26);
            AddItem(new Static(0x0620), 4, 4, 34);
            AddItem(new Static(0x0619), 4, 5, 26);
            AddItem(new Static(0x0619), 4, 6, 23);
            AddItem(new Static(0x0619), 4, 7, 20);
            AddItem(new Static(0x0239), 5, 0, 0);
            AddItem(new Static(0x0618), 5, 1, 20);
            AddItem(new Static(0x0618), 5, 2, 23);
            AddItem(new Static(0x061D), 5, 3, 26);
            AddItem(new Static(0x061A), 5, 4, 26);
            AddItem(new Static(0x061C), 5, 5, 26);
            AddItem(new Static(0x0619), 5, 6, 23);
            AddItem(new Static(0x0231), 5, 7, 0);
            AddItem(new Static(0x0619), 5, 7, 20);
            AddItem(new Static(0x0236), 6, 0, 0);
            AddItem(new Static(0x0618), 6, 1, 20);
            AddItem(new Static(0x061D), 6, 2, 23);
            AddItem(new Static(0x061A), 6, 3, 23);
            AddItem(new Static(0x061A), 6, 4, 23);
            AddItem(new Static(0x061A), 6, 5, 23);
            AddItem(new Static(0x061C), 6, 6, 23);
            AddItem(new Static(0x0231), 6, 7, 0);
            AddItem(new Static(0x0619), 6, 7, 20);
            AddItem(new Static(0x0235), 7, 0, 0);
            AddItem(new Static(0x0232), 7, 1, 0);
            AddItem(new Static(0x061D), 7, 1, 20);
            AddItem(new Static(0x0232), 7, 2, 0);
            AddItem(new Static(0x061A), 7, 2, 20);
            AddItem(new Static(0x0232), 7, 3, 0);
            AddItem(new Static(0x061A), 7, 3, 20);
            AddItem(new Static(0x0232), 7, 4, 0);
            AddItem(new Static(0x061A), 7, 4, 20);
            AddItem(new Static(0x0232), 7, 5, 0);
            AddItem(new Static(0x061A), 7, 5, 20);
            AddItem(new Static(0x0232), 7, 6, 0);
            AddItem(new Static(0x061A), 7, 6, 20);
            AddItem(new Static(0x0230), 7, 7, 0);
            AddItem(new Static(0x061C), 7, 7, 20);
        }

        public virtual void SpawnTower()
        {
            AddItem(new Static(0x01EB), 0, 7, 66);
            AddItem(new Static(0x01EB), 0, 7, 69);
            AddItem(new Static(0x01DA), 0, 7, 72);
            AddItem(new Static(0x01EA), 0, 8, 66);
            AddItem(new Static(0x01EA), 0, 8, 69);
            AddItem(new Static(0x01EA), 0, 8, 72);
            AddItem(new Static(0x01EA), 0, 9, 66);
            AddItem(new Static(0x01EA), 0, 9, 69);
            AddItem(new Static(0x01EA), 0, 10, 66);
            AddItem(new Static(0x01EA), 0, 10, 69);
            AddItem(new Static(0x01EA), 0, 10, 72);
            AddItem(new Static(0x01EA), 0, 11, 66);
            AddItem(new Static(0x01EA), 0, 11, 69);
            AddItem(new Static(0x01EA), 0, 12, 66);
            AddItem(new Static(0x01EA), 0, 12, 69);
            AddItem(new Static(0x01EA), 0, 12, 72);
            AddItem(new Static(0x01EA), 0, 13, 66);
            AddItem(new Static(0x01EA), 0, 13, 69);
            AddItem(new Static(0x01DA), 0, 13, 72);
            AddItem(new Static(0x01E9), 1, 7, 66);
            AddItem(new Static(0x01E9), 1, 7, 69);
            AddItem(new Static(0x0786), 1, 8, 61);
            AddItem(new Static(0x0524), 1, 8, 66);
            AddItem(new Static(0x0524), 1, 9, 66);
            AddItem(new Static(0x0524), 1, 9, 66);
            AddItem(new Static(0x0524), 1, 10, 66);
            AddItem(new Static(0x0524), 1, 11, 66);
            AddItem(new Static(0x0524), 1, 12, 66);
            AddItem(new Static(0x0786), 1, 13, 61);
            AddItem(new Static(0x0524), 1, 13, 66);
            AddItem(new Static(0x01E9), 1, 13, 66);
            AddItem(new Static(0x01E9), 1, 13, 69);
            AddItem(new Static(0x01E9), 2, 7, 66);
            AddItem(new Static(0x01E9), 2, 7, 69);
            AddItem(new Static(0x01E9), 2, 7, 72);
            AddItem(new Static(0x0786), 2, 8, 56);
            AddItem(new Static(0x0788), 2, 8, 61);
            AddItem(new Static(0x0524), 2, 8, 66);
            AddItem(new Static(0x0524), 2, 9, 66);
            AddItem(new Static(0x0524), 2, 10, 66);
            AddItem(new Static(0x0524), 2, 11, 66);
            AddItem(new Static(0x0524), 2, 12, 66);
            AddItem(new Static(0x0786), 2, 13, 56);
            AddItem(new Static(0x0788), 2, 13, 61);
            AddItem(new Static(0x0524), 2, 13, 66);
            AddItem(new Static(0x01E9), 2, 13, 66);
            AddItem(new Static(0x01E9), 2, 13, 69);
            AddItem(new Static(0x01E9), 2, 13, 72);
            AddItem(new Static(0x01E9), 3, 7, 66);
            AddItem(new Static(0x01E9), 3, 7, 69);
            AddItem(new Static(0x0786), 3, 8, 51);
            AddItem(new Static(0x0788), 3, 8, 56);
            AddItem(new Static(0x0788), 3, 8, 61);
            AddItem(new Static(0x0524), 3, 8, 66);
            AddItem(new Static(0x0524), 3, 9, 66);
            AddItem(new Static(0x0524), 3, 10, 66);
            AddItem(new Static(0x0524), 3, 11, 66);
            AddItem(new Static(0x0524), 3, 12, 66);
            AddItem(new Static(0x0786), 3, 13, 51);
            AddItem(new Static(0x0788), 3, 13, 56);
            AddItem(new Static(0x0788), 3, 13, 61);
            AddItem(new Static(0x0524), 3, 13, 66);
            AddItem(new Static(0x01E9), 3, 13, 66);
            AddItem(new Static(0x01E9), 3, 13, 69);
            AddItem(new Static(0x01EB), 4, 0, 0);
            AddItem(new Static(0x01EB), 4, 0, 3);
            AddItem(new Static(0x01D2), 4, 0, 6);
            AddItem(new Static(0x01D2), 4, 0, 26);
            AddItem(new Static(0x01D2), 4, 0, 46);
            AddItem(new Static(0x01EB), 4, 0, 66);
            AddItem(new Static(0x01EB), 4, 0, 69);
            AddItem(new Static(0x01DA), 4, 0, 72);
            AddItem(new Static(0x01EA), 4, 1, 0);
            AddItem(new Static(0x01EA), 4, 1, 3);
            AddItem(new Static(0x01D1), 4, 1, 6);
            AddItem(new Static(0x01D1), 4, 1, 26);
            AddItem(new Static(0x01D1), 4, 1, 46);
            AddItem(new Static(0x01EA), 4, 1, 66);
            AddItem(new Static(0x01EA), 4, 1, 69);
            AddItem(new Static(0x01EA), 4, 2, 0);
            AddItem(new Static(0x01EA), 4, 2, 3);
            AddItem(new Static(0x01D1), 4, 2, 6);
            AddItem(new Static(0x01D1), 4, 2, 26);
            AddItem(new Static(0x01D1), 4, 2, 46);
            AddItem(new Static(0x01EA), 4, 2, 66);
            AddItem(new Static(0x01EA), 4, 2, 69);
            AddItem(new Static(0x01EA), 4, 2, 72);
            AddItem(new Static(0x01EA), 4, 3, 0);
            AddItem(new Static(0x01EA), 4, 3, 3);
            AddItem(new Static(0x01D1), 4, 3, 6);
            AddItem(new Static(0x01D1), 4, 3, 26);
            AddItem(new Static(0x01D1), 4, 3, 46);
            AddItem(new Static(0x01EA), 4, 3, 66);
            AddItem(new Static(0x01EA), 4, 3, 69);
            AddItem(new Static(0x01EA), 4, 4, 0);
            AddItem(new Static(0x01EA), 4, 4, 3);
            AddItem(new Static(0x01D1), 4, 4, 6);
            AddItem(new Static(0x01D4), 4, 4, 26);
            AddItem(new Static(0x01D1), 4, 4, 46);
            AddItem(new Static(0x01EA), 4, 4, 66);
            AddItem(new Static(0x01EA), 4, 4, 69);
            AddItem(new Static(0x01EA), 4, 4, 72);
            AddItem(new Static(0x01EA), 4, 5, 0);
            AddItem(new Static(0x01EA), 4, 5, 3);
            AddItem(new Static(0x01D1), 4, 5, 6);
            AddItem(new Static(0x01D1), 4, 5, 26);
            AddItem(new Static(0x01D1), 4, 5, 46);
            AddItem(new Static(0x01EA), 4, 5, 66);
            AddItem(new Static(0x01EA), 4, 5, 69);
            AddItem(new Static(0x01EA), 4, 6, 0);
            AddItem(new Static(0x01EA), 4, 6, 3);
            AddItem(new Static(0x01D1), 4, 6, 6);
            AddItem(new Static(0x01D1), 4, 6, 26);
            AddItem(new Static(0x01D1), 4, 6, 46);
            AddItem(new Static(0x01EA), 4, 6, 66);
            AddItem(new Static(0x01EA), 4, 6, 69);
            AddItem(new Static(0x01EA), 4, 6, 72);
            AddItem(new Static(0x01EA), 4, 7, 0);
            AddItem(new Static(0x01EA), 4, 7, 3);
            AddItem(new Static(0x01D1), 4, 7, 6);
            AddItem(new Static(0x01D1), 4, 7, 26);
            AddItem(new Static(0x01D1), 4, 7, 46);
            AddItem(new Static(0x01E8), 4, 7, 66);
            AddItem(new Static(0x01E8), 4, 7, 69);
            AddItem(new Static(0x01E9), 4, 7, 72);
            AddItem(new Static(0x01EA), 4, 8, 0);
            AddItem(new Static(0x01EA), 4, 8, 3);
            AddItem(new Static(0x01D1), 4, 8, 6);
            AddItem(new Static(0x01D1), 4, 8, 26);
            AddItem(new Static(0x01D1), 4, 8, 46);
            AddItem(new Static(0x0788), 4, 8, 61);
            AddItem(new Static(0x0524), 4, 8, 66);
            AddItem(new Static(0x01EA), 4, 9, 0);
            AddItem(new Static(0x01EA), 4, 9, 3);
            AddItem(new Static(0x01D1), 4, 9, 6);
            AddItem(new Static(0x01D1), 4, 9, 26);
            AddItem(new Static(0x01D1), 4, 9, 46);
            AddItem(new Static(0x0524), 4, 9, 66);
            AddItem(new Static(0x01EA), 4, 10, 0);
            AddItem(new Static(0x01EA), 4, 10, 3);
            AddItem(new Static(0x01D1), 4, 10, 6);
            AddItem(new Static(0x01D4), 4, 10, 26);
            AddItem(new Static(0x01D1), 4, 10, 46);
            AddItem(new Static(0x0524), 4, 10, 66);
            AddItem(new Static(0x01EA), 4, 11, 0);
            AddItem(new Static(0x01EA), 4, 11, 3);
            AddItem(new Static(0x01D1), 4, 11, 6);
            AddItem(new Static(0x01D1), 4, 11, 26);
            AddItem(new Static(0x01D1), 4, 11, 46);
            AddItem(new Static(0x0524), 4, 11, 66);
            AddItem(new Static(0x01EA), 4, 12, 0);
            AddItem(new Static(0x01EA), 4, 12, 3);
            AddItem(new Static(0x01D1), 4, 12, 6);
            AddItem(new Static(0x01D1), 4, 12, 26);
            AddItem(new Static(0x01D1), 4, 12, 46);
            AddItem(new Static(0x0524), 4, 12, 66);
            AddItem(new Static(0x01EA), 4, 13, 0);
            AddItem(new Static(0x01EA), 4, 13, 3);
            AddItem(new Static(0x01D1), 4, 13, 6);
            AddItem(new Static(0x01D1), 4, 13, 26);
            AddItem(new Static(0x01D1), 4, 13, 46);
            AddItem(new Static(0x0786), 4, 13, 46);
            AddItem(new Static(0x0788), 4, 13, 51);
            AddItem(new Static(0x0788), 4, 13, 56);
            AddItem(new Static(0x0788), 4, 13, 61);
            AddItem(new Static(0x0524), 4, 13, 66);
            AddItem(new Static(0x01E9), 4, 13, 66);
            AddItem(new Static(0x01E9), 4, 13, 69);
            AddItem(new Static(0x01E9), 4, 13, 72);
            AddItem(new Static(0x01E9), 5, 0, 0);
            AddItem(new Static(0x01E9), 5, 0, 3);
            AddItem(new Static(0x01D0), 5, 0, 6);
            AddItem(new Static(0x01D0), 5, 0, 26);
            AddItem(new Static(0x01D0), 5, 0, 46);
            AddItem(new Static(0x01E9), 5, 0, 66);
            AddItem(new Static(0x01E9), 5, 0, 69);
            AddItem(new Static(0x0521), 5, 1, 6);
            AddItem(new Static(0x0788), 5, 1, 6);
            AddItem(new Static(0x0788), 5, 1, 11);
            AddItem(new Static(0x0788), 5, 1, 16);
            AddItem(new Static(0x0788), 5, 1, 21);
            AddItem(new Static(0x0523), 5, 1, 46);
            AddItem(new Static(0x0788), 5, 1, 46);
            AddItem(new Static(0x0788), 5, 1, 51);
            AddItem(new Static(0x0788), 5, 1, 56);
            AddItem(new Static(0x0788), 5, 1, 61);
            AddItem(new Static(0x0521), 5, 2, 6);
            AddItem(new Static(0x0788), 5, 2, 6);
            AddItem(new Static(0x0788), 5, 2, 11);
            AddItem(new Static(0x0788), 5, 2, 16);
            AddItem(new Static(0x0789), 5, 2, 21);
            AddItem(new Static(0x0523), 5, 2, 46);
            AddItem(new Static(0x0788), 5, 2, 46);
            AddItem(new Static(0x0788), 5, 2, 51);
            AddItem(new Static(0x0788), 5, 2, 56);
            AddItem(new Static(0x0789), 5, 2, 61);
            AddItem(new Static(0x0521), 5, 3, 6);
            AddItem(new Static(0x0788), 5, 3, 6);
            AddItem(new Static(0x0788), 5, 3, 11);
            AddItem(new Static(0x0789), 5, 3, 16);
            AddItem(new Static(0x0523), 5, 3, 46);
            AddItem(new Static(0x0788), 5, 3, 46);
            AddItem(new Static(0x0788), 5, 3, 51);
            AddItem(new Static(0x0789), 5, 3, 56);
            AddItem(new Static(0x0521), 5, 4, 6);
            AddItem(new Static(0x0788), 5, 4, 6);
            AddItem(new Static(0x0789), 5, 4, 11);
            AddItem(new Static(0x0523), 5, 4, 46);
            AddItem(new Static(0x0788), 5, 4, 46);
            AddItem(new Static(0x0789), 5, 4, 51);
            AddItem(new Static(0x0521), 5, 5, 6);
            AddItem(new Static(0x0789), 5, 5, 6);
            AddItem(new Static(0x0523), 5, 5, 46);
            AddItem(new Static(0x0789), 5, 5, 46);
            AddItem(new Static(0x0521), 5, 6, 6);
            AddItem(new Static(0x0523), 5, 6, 46);
            AddItem(new Static(0x0521), 5, 7, 6);
            AddItem(new Static(0x0523), 5, 7, 46);
            AddItem(new Static(0x0521), 5, 8, 6);
            AddItem(new Static(0x01D0), 5, 8, 6);
            AddItem(new Static(0x0522), 5, 8, 26);
            AddItem(new Static(0x01D0), 5, 8, 26);
            AddItem(new Static(0x0523), 5, 8, 46);
            AddItem(new Static(0x01D0), 5, 8, 46);
            AddItem(new Static(0x0523), 5, 8, 66);
            AddItem(new Static(0x08BE), 5, 8, 66);
            AddItem(new Static(0x08C0), 5, 8, 66);
            AddItem(new Static(0x0521), 5, 9, 6);
            AddItem(new Static(0x0522), 5, 9, 26);
            AddItem(new Static(0x0523), 5, 9, 46);
            AddItem(new Static(0x0523), 5, 9, 66);
            AddItem(new Static(0x0521), 5, 10, 6);
            AddItem(new Static(0x0522), 5, 10, 26);
            AddItem(new Static(0x0523), 5, 10, 46);
            AddItem(new Static(0x0523), 5, 10, 66);
            AddItem(new Static(0x0521), 5, 11, 6);
            AddItem(new Static(0x0522), 5, 11, 26);
            AddItem(new Static(0x0523), 5, 11, 46);
            AddItem(new Static(0x0523), 5, 11, 66);
            AddItem(new Static(0x0521), 5, 12, 6);
            AddItem(new Static(0x0522), 5, 12, 26);
            AddItem(new Static(0x0523), 5, 12, 46);
            AddItem(new Static(0x0523), 5, 12, 66);
            AddItem(new Static(0x01E9), 5, 13, 0);
            AddItem(new Static(0x01E9), 5, 13, 3);
            AddItem(new Static(0x0521), 5, 13, 6);
            AddItem(new Static(0x01D0), 5, 13, 6);
            AddItem(new Static(0x0522), 5, 13, 26);
            AddItem(new Static(0x01D0), 5, 13, 26);
            AddItem(new Static(0x0523), 5, 13, 46);
            AddItem(new Static(0x01D0), 5, 13, 46);
            AddItem(new Static(0x0523), 5, 13, 66);
            AddItem(new Static(0x01E9), 5, 13, 66);
            AddItem(new Static(0x01E9), 5, 13, 69);
            AddItem(new Static(0x01E9), 6, 0, 0);
            AddItem(new Static(0x01E9), 6, 0, 3);
            AddItem(new Static(0x01D0), 6, 0, 6);
            AddItem(new Static(0x01D0), 6, 0, 26);
            AddItem(new Static(0x01D0), 6, 0, 46);
            AddItem(new Static(0x01E9), 6, 0, 66);
            AddItem(new Static(0x01E9), 6, 0, 69);
            AddItem(new Static(0x01E9), 6, 0, 72);
            AddItem(new Static(0x0521), 6, 1, 6);
            AddItem(new Static(0x0788), 6, 1, 6);
            AddItem(new Static(0x0788), 6, 1, 11);
            AddItem(new Static(0x0788), 6, 1, 16);
            AddItem(new Static(0x0788), 6, 1, 21);
            AddItem(new Static(0x0523), 6, 1, 46);
            AddItem(new Static(0x0788), 6, 1, 46);
            AddItem(new Static(0x0788), 6, 1, 51);
            AddItem(new Static(0x0788), 6, 1, 56);
            AddItem(new Static(0x0788), 6, 1, 61);
            AddItem(new Static(0x0521), 6, 2, 6);
            AddItem(new Static(0x0788), 6, 2, 6);
            AddItem(new Static(0x0788), 6, 2, 11);
            AddItem(new Static(0x0788), 6, 2, 16);
            AddItem(new Static(0x0789), 6, 2, 21);
            AddItem(new Static(0x0523), 6, 2, 46);
            AddItem(new Static(0x0788), 6, 2, 46);
            AddItem(new Static(0x0788), 6, 2, 51);
            AddItem(new Static(0x0788), 6, 2, 56);
            AddItem(new Static(0x0789), 6, 2, 61);
            AddItem(new Static(0x0521), 6, 3, 6);
            AddItem(new Static(0x0788), 6, 3, 6);
            AddItem(new Static(0x0788), 6, 3, 11);
            AddItem(new Static(0x0789), 6, 3, 16);
            AddItem(new Static(0x0523), 6, 3, 46);
            AddItem(new Static(0x0788), 6, 3, 46);
            AddItem(new Static(0x0788), 6, 3, 51);
            AddItem(new Static(0x0789), 6, 3, 56);
            AddItem(new Static(0x0521), 6, 4, 6);
            AddItem(new Static(0x0788), 6, 4, 6);
            AddItem(new Static(0x0789), 6, 4, 11);
            AddItem(new Static(0x0523), 6, 4, 46);
            AddItem(new Static(0x0788), 6, 4, 46);
            AddItem(new Static(0x0789), 6, 4, 51);
            AddItem(new Static(0x0521), 6, 5, 6);
            AddItem(new Static(0x0789), 6, 5, 6);
            AddItem(new Static(0x0523), 6, 5, 46);
            AddItem(new Static(0x0789), 6, 5, 46);
            AddItem(new Static(0x0521), 6, 6, 6);
            AddItem(new Static(0x0523), 6, 6, 46);
            AddItem(new Static(0x0521), 6, 7, 6);
            AddItem(new Static(0x0523), 6, 7, 46);
            AddItem(new Static(0x0521), 6, 8, 6);
            AddItem(new Static(0x01D6), 6, 8, 6);
            AddItem(new Static(0x0522), 6, 8, 26);
            AddItem(new Static(0x01D0), 6, 8, 26);
            AddItem(new Static(0x0523), 6, 8, 46);
            AddItem(new Static(0x01D0), 6, 8, 46);
            AddItem(new Static(0x0523), 6, 8, 66);
            AddItem(new Static(0x08BE), 6, 8, 66);
            AddItem(new Static(0x0521), 6, 9, 6);
            AddItem(new Static(0x0522), 6, 9, 26);
            AddItem(new Static(0x0523), 6, 9, 46);
            AddItem(new Static(0x0523), 6, 9, 66);
            AddItem(new Static(0x0521), 6, 10, 6);
            AddItem(new Static(0x0522), 6, 10, 26);
            AddItem(new Static(0x0523), 6, 10, 46);
            AddItem(new Static(0x0523), 6, 10, 66);
            AddItem(new Static(0x0521), 6, 11, 6);
            AddItem(new Static(0x0522), 6, 11, 26);
            AddItem(new Static(0x0523), 6, 11, 46);
            AddItem(new Static(0x0523), 6, 11, 66);
            AddItem(new Static(0x0521), 6, 12, 6);
            AddItem(new Static(0x0522), 6, 12, 26);
            AddItem(new Static(0x0523), 6, 12, 46);
            AddItem(new Static(0x0523), 6, 12, 66);
            AddItem(new Static(0x01E9), 6, 13, 0);
            AddItem(new Static(0x01E9), 6, 13, 3);
            AddItem(new Static(0x0521), 6, 13, 6);
            AddItem(new Static(0x01D0), 6, 13, 6);
            AddItem(new Static(0x0522), 6, 13, 26);
            AddItem(new Static(0x01D0), 6, 13, 26);
            AddItem(new Static(0x0523), 6, 13, 46);
            AddItem(new Static(0x01D0), 6, 13, 46);
            AddItem(new Static(0x0523), 6, 13, 66);
            AddItem(new Static(0x01E9), 6, 13, 66);
            AddItem(new Static(0x01E9), 6, 13, 69);
            AddItem(new Static(0x01E9), 6, 13, 72);
            AddItem(new Static(0x01E9), 7, 0, 0);
            AddItem(new Static(0x01E9), 7, 0, 3);
            AddItem(new Static(0x01D0), 7, 0, 6);
            AddItem(new Static(0x01D0), 7, 0, 26);
            AddItem(new Static(0x01D0), 7, 0, 46);
            AddItem(new Static(0x01E9), 7, 0, 66);
            AddItem(new Static(0x01E9), 7, 0, 69);
            AddItem(new Static(0x0521), 7, 1, 6);
            AddItem(new Static(0x0522), 7, 1, 26);
            AddItem(new Static(0x0523), 7, 1, 46);
            AddItem(new Static(0x0523), 7, 1, 66);
            AddItem(new Static(0x0521), 7, 2, 6);
            AddItem(new Static(0x0522), 7, 2, 26);
            AddItem(new Static(0x0523), 7, 2, 46);
            AddItem(new Static(0x0523), 7, 2, 66);
            AddItem(new Static(0x0521), 7, 3, 6);
            AddItem(new Static(0x0522), 7, 3, 26);
            AddItem(new Static(0x08C0), 7, 3, 26);
            AddItem(new Static(0x08BF), 7, 3, 26);
            AddItem(new Static(0x0523), 7, 3, 46);
            AddItem(new Static(0x0523), 7, 3, 66);
            AddItem(new Static(0x08C0), 7, 3, 66);
            AddItem(new Static(0x08BF), 7, 3, 66);
            AddItem(new Static(0x0521), 7, 4, 6);
            AddItem(new Static(0x0522), 7, 4, 26);
            AddItem(new Static(0x08BF), 7, 4, 26);
            AddItem(new Static(0x0523), 7, 4, 46);
            AddItem(new Static(0x0523), 7, 4, 66);
            AddItem(new Static(0x08BF), 7, 4, 66);
            AddItem(new Static(0x0521), 7, 5, 6);
            AddItem(new Static(0x0522), 7, 5, 26);
            AddItem(new Static(0x08BF), 7, 5, 26);
            AddItem(new Static(0x0523), 7, 5, 46);
            AddItem(new Static(0x0523), 7, 5, 66);
            AddItem(new Static(0x08BF), 7, 5, 66);
            AddItem(new Static(0x0521), 7, 6, 6);
            AddItem(new Static(0x0522), 7, 6, 26);
            AddItem(new Static(0x08BF), 7, 6, 26);
            AddItem(new Static(0x0523), 7, 6, 46);
            AddItem(new Static(0x0523), 7, 6, 66);
            AddItem(new Static(0x08BF), 7, 6, 66);
            AddItem(new Static(0x0521), 7, 7, 6);
            AddItem(new Static(0x0522), 7, 7, 26);
            AddItem(new Static(0x08BF), 7, 7, 26);
            AddItem(new Static(0x0523), 7, 7, 46);
            AddItem(new Static(0x0523), 7, 7, 66);
            AddItem(new Static(0x08BF), 7, 7, 66);
            AddItem(new Static(0x0521), 7, 8, 6);
            AddItem(new Static(0x0522), 7, 8, 26);
            AddItem(new Static(0x01D0), 7, 8, 26);
            AddItem(new Static(0x0523), 7, 8, 46);
            AddItem(new Static(0x01D6), 7, 8, 46);
            AddItem(new Static(0x0523), 7, 8, 66);
            AddItem(new Static(0x0521), 7, 9, 6);
            AddItem(new Static(0x0522), 7, 9, 26);
            AddItem(new Static(0x0523), 7, 9, 46);
            AddItem(new Static(0x0523), 7, 9, 66);
            AddItem(new Static(0x0521), 7, 10, 6);
            AddItem(new Static(0x0522), 7, 10, 26);
            AddItem(new Static(0x0523), 7, 10, 46);
            AddItem(new Static(0x0523), 7, 10, 66);
            AddItem(new Static(0x0521), 7, 11, 6);
            AddItem(new Static(0x0522), 7, 11, 26);
            AddItem(new Static(0x0523), 7, 11, 46);
            AddItem(new Static(0x0523), 7, 11, 66);
            AddItem(new Static(0x0521), 7, 12, 6);
            AddItem(new Static(0x0522), 7, 12, 26);
            AddItem(new Static(0x0523), 7, 12, 46);
            AddItem(new Static(0x0523), 7, 12, 66);
            AddItem(new Static(0x01E9), 7, 13, 0);
            AddItem(new Static(0x01E9), 7, 13, 3);
            AddItem(new Static(0x0521), 7, 13, 6);
            AddItem(new Static(0x01D0), 7, 13, 6);
            AddItem(new Static(0x0522), 7, 13, 26);
            AddItem(new Static(0x01D0), 7, 13, 26);
            AddItem(new Static(0x0523), 7, 13, 46);
            AddItem(new Static(0x01D3), 7, 13, 46);
            AddItem(new Static(0x0523), 7, 13, 66);
            AddItem(new Static(0x01E9), 7, 13, 66);
            AddItem(new Static(0x01E9), 7, 13, 69);
            AddItem(new Static(0x01E9), 8, 0, 0);
            AddItem(new Static(0x01E9), 8, 0, 3);
            AddItem(new Static(0x01D0), 8, 0, 6);
            AddItem(new Static(0x01D0), 8, 0, 26);
            AddItem(new Static(0x01D0), 8, 0, 46);
            AddItem(new Static(0x01E9), 8, 0, 66);
            AddItem(new Static(0x01E9), 8, 0, 69);
            AddItem(new Static(0x01E9), 8, 0, 72);
            AddItem(new Static(0x0521), 8, 1, 6);
            AddItem(new Static(0x0522), 8, 1, 26);
            AddItem(new Static(0x0523), 8, 1, 46);
            AddItem(new Static(0x0523), 8, 1, 66);
            AddItem(new Static(0x0521), 8, 2, 6);
            AddItem(new Static(0x0522), 8, 2, 26);
            AddItem(new Static(0x0523), 8, 2, 46);
            AddItem(new Static(0x0523), 8, 2, 66);
            AddItem(new Static(0x0521), 8, 3, 6);
            AddItem(new Static(0x0522), 8, 3, 26);
            AddItem(new Static(0x0523), 8, 3, 46);
            AddItem(new Static(0x0523), 8, 3, 66);
            AddItem(new Static(0x0521), 8, 4, 6);
            AddItem(new Static(0x0522), 8, 4, 26);
            AddItem(new Static(0x0523), 8, 4, 46);
            AddItem(new Static(0x0523), 8, 4, 66);
            AddItem(new Static(0x0521), 8, 5, 6);
            AddItem(new Static(0x0522), 8, 5, 26);
            AddItem(new Static(0x0523), 8, 5, 46);
            AddItem(new Static(0x0523), 8, 5, 66);
            AddItem(new Static(0x0521), 8, 6, 6);
            AddItem(new Static(0x0522), 8, 6, 26);
            AddItem(new Static(0x0523), 8, 6, 46);
            AddItem(new Static(0x0523), 8, 6, 66);
            AddItem(new Static(0x0521), 8, 7, 6);
            AddItem(new Static(0x0522), 8, 7, 26);
            AddItem(new Static(0x0523), 8, 7, 46);
            AddItem(new Static(0x0523), 8, 7, 66);
            AddItem(new Static(0x0521), 8, 8, 6);
            AddItem(new Static(0x01D7), 8, 8, 6);
            AddItem(new Static(0x0522), 8, 8, 26);
            AddItem(new Static(0x01D6), 8, 8, 26);
            AddItem(new Static(0x0523), 8, 8, 46);
            AddItem(new Static(0x0523), 8, 8, 66);
            AddItem(new Static(0x0521), 8, 9, 6);
            AddItem(new Static(0x0522), 8, 9, 26);
            AddItem(new Static(0x0523), 8, 9, 46);
            AddItem(new Static(0x0523), 8, 9, 66);
            AddItem(new Static(0x0521), 8, 10, 6);
            AddItem(new Static(0x0522), 8, 10, 26);
            AddItem(new Static(0x0523), 8, 10, 46);
            AddItem(new Static(0x0523), 8, 10, 66);
            AddItem(new Static(0x0521), 8, 11, 6);
            AddItem(new Static(0x0522), 8, 11, 26);
            AddItem(new Static(0x0523), 8, 11, 46);
            AddItem(new Static(0x0523), 8, 11, 66);
            AddItem(new Static(0x0521), 8, 12, 6);
            AddItem(new Static(0x0522), 8, 12, 26);
            AddItem(new Static(0x0523), 8, 12, 46);
            AddItem(new Static(0x0523), 8, 12, 66);
            AddItem(new Static(0x01E9), 8, 13, 0);
            AddItem(new Static(0x01E9), 8, 13, 3);
            AddItem(new Static(0x0521), 8, 13, 6);
            AddItem(new Static(0x01D0), 8, 13, 6);
            AddItem(new Static(0x0522), 8, 13, 26);
            AddItem(new Static(0x01D3), 8, 13, 26);
            AddItem(new Static(0x0523), 8, 13, 46);
            AddItem(new Static(0x01D0), 8, 13, 46);
            AddItem(new Static(0x0523), 8, 13, 66);
            AddItem(new Static(0x01E9), 8, 13, 66);
            AddItem(new Static(0x01E9), 8, 13, 69);
            AddItem(new Static(0x01E9), 8, 13, 72);
            AddItem(new Static(0x01E9), 9, 0, 0);
            AddItem(new Static(0x01E9), 9, 0, 3);
            AddItem(new Static(0x01D0), 9, 0, 6);
            AddItem(new Static(0x01D0), 9, 0, 26);
            AddItem(new Static(0x01D0), 9, 0, 46);
            AddItem(new Static(0x01E9), 9, 0, 66);
            AddItem(new Static(0x01E9), 9, 0, 69);
            AddItem(new Static(0x0521), 9, 1, 6);
            AddItem(new Static(0x01D1), 9, 1, 6);
            AddItem(new Static(0x0522), 9, 1, 26);
            AddItem(new Static(0x0523), 9, 1, 46);
            AddItem(new Static(0x0523), 9, 1, 66);
            AddItem(new Static(0x0521), 9, 2, 6);
            AddItem(new Static(0x01D1), 9, 2, 6);
            AddItem(new Static(0x0522), 9, 2, 26);
            AddItem(new Static(0x0523), 9, 2, 46);
            AddItem(new Static(0x08B9), 9, 2, 46);
            AddItem(new Static(0x0523), 9, 2, 66);
            AddItem(new Static(0x0521), 9, 3, 6);
            AddItem(new Static(0x01D1), 9, 3, 6);
            AddItem(new Static(0x0522), 9, 3, 26);
            AddItem(new Static(0x0523), 9, 3, 46);
            AddItem(new Static(0x08BB), 9, 3, 46);
            AddItem(new Static(0x0523), 9, 3, 66);
            AddItem(new Static(0x0521), 9, 4, 6);
            AddItem(new Static(0x01D1), 9, 4, 6);
            AddItem(new Static(0x0522), 9, 4, 26);
            AddItem(new Static(0x0523), 9, 4, 46);
            AddItem(new Static(0x08BB), 9, 4, 46);
            AddItem(new Static(0x0523), 9, 4, 66);
            AddItem(new Static(0x0521), 9, 5, 6);
            AddItem(new Static(0x01D1), 9, 5, 6);
            AddItem(new Static(0x0522), 9, 5, 26);
            AddItem(new Static(0x0523), 9, 5, 46);
            AddItem(new Static(0x08BB), 9, 5, 46);
            AddItem(new Static(0x0523), 9, 5, 66);
            AddItem(new Static(0x0521), 9, 6, 6);
            AddItem(new Static(0x01D1), 9, 6, 6);
            AddItem(new Static(0x0522), 9, 6, 26);
            AddItem(new Static(0x0523), 9, 6, 46);
            AddItem(new Static(0x08BB), 9, 6, 46);
            AddItem(new Static(0x0523), 9, 6, 66);
            AddItem(new Static(0x0521), 9, 7, 6);
            AddItem(new Static(0x01D1), 9, 7, 6);
            AddItem(new Static(0x0522), 9, 7, 26);
            AddItem(new Static(0x0523), 9, 7, 46);
            AddItem(new Static(0x08BB), 9, 7, 46);
            AddItem(new Static(0x0523), 9, 7, 66);
            AddItem(new Static(0x0521), 9, 8, 6);
            AddItem(new Static(0x01CF), 9, 8, 6);
            AddItem(new Static(0x0522), 9, 8, 26);
            AddItem(new Static(0x0523), 9, 8, 46);
            AddItem(new Static(0x01D7), 9, 8, 46);
            AddItem(new Static(0x0523), 9, 8, 66);
            AddItem(new Static(0x0521), 9, 9, 6);
            AddItem(new Static(0x0522), 9, 9, 26);
            AddItem(new Static(0x0523), 9, 9, 46);
            AddItem(new Static(0x0523), 9, 9, 66);
            AddItem(new Static(0x0521), 9, 10, 6);
            AddItem(new Static(0x0522), 9, 10, 26);
            AddItem(new Static(0x0523), 9, 10, 46);
            AddItem(new Static(0x0523), 9, 10, 66);
            AddItem(new Static(0x0521), 9, 11, 6);
            AddItem(new Static(0x0522), 9, 11, 26);
            AddItem(new Static(0x0523), 9, 11, 46);
            AddItem(new Static(0x0523), 9, 11, 66);
            AddItem(new Static(0x0521), 9, 12, 6);
            AddItem(new Static(0x0522), 9, 12, 26);
            AddItem(new Static(0x0523), 9, 12, 46);
            AddItem(new Static(0x0523), 9, 12, 66);
            AddItem(new Static(0x01E9), 9, 13, 0);
            AddItem(new Static(0x01E9), 9, 13, 3);
            AddItem(new Static(0x0521), 9, 13, 6);
            AddItem(new Static(0x01D0), 9, 13, 6);
            AddItem(new Static(0x0522), 9, 13, 26);
            AddItem(new Static(0x01D0), 9, 13, 26);
            AddItem(new Static(0x0523), 9, 13, 46);
            AddItem(new Static(0x01D0), 9, 13, 46);
            AddItem(new Static(0x0523), 9, 13, 66);
            AddItem(new Static(0x01E9), 9, 13, 66);
            AddItem(new Static(0x01E9), 9, 13, 69);
            AddItem(new Static(0x01E9), 10, 0, 0);
            AddItem(new Static(0x01E9), 10, 0, 3);
            AddItem(new Static(0x01D0), 10, 0, 6);
            AddItem(new Static(0x01D0), 10, 0, 26);
            AddItem(new Static(0x01D0), 10, 0, 46);
            AddItem(new Static(0x01E9), 10, 0, 66);
            AddItem(new Static(0x01E9), 10, 0, 69);
            AddItem(new Static(0x01E9), 10, 0, 72);
            AddItem(new Static(0x0521), 10, 1, 6);
            AddItem(new Static(0x0522), 10, 1, 26);
            AddItem(new Static(0x0788), 10, 1, 26);
            AddItem(new Static(0x0788), 10, 1, 31);
            AddItem(new Static(0x0788), 10, 1, 36);
            AddItem(new Static(0x0788), 10, 1, 41);
            AddItem(new Static(0x0523), 10, 1, 66);
            AddItem(new Static(0x0521), 10, 2, 6);
            AddItem(new Static(0x0522), 10, 2, 26);
            AddItem(new Static(0x0788), 10, 2, 26);
            AddItem(new Static(0x0788), 10, 2, 31);
            AddItem(new Static(0x0788), 10, 2, 36);
            AddItem(new Static(0x0789), 10, 2, 41);
            AddItem(new Static(0x0523), 10, 2, 66);
            AddItem(new Static(0x0521), 10, 3, 6);
            AddItem(new Static(0x0522), 10, 3, 26);
            AddItem(new Static(0x0788), 10, 3, 26);
            AddItem(new Static(0x0788), 10, 3, 31);
            AddItem(new Static(0x0789), 10, 3, 36);
            AddItem(new Static(0x0523), 10, 3, 66);
            AddItem(new Static(0x0521), 10, 4, 6);
            AddItem(new Static(0x0522), 10, 4, 26);
            AddItem(new Static(0x0788), 10, 4, 26);
            AddItem(new Static(0x0789), 10, 4, 31);
            AddItem(new Static(0x0523), 10, 4, 66);
            AddItem(new Static(0x0521), 10, 5, 6);
            AddItem(new Static(0x01D0), 10, 5, 6);
            AddItem(new Static(0x0522), 10, 5, 26);
            AddItem(new Static(0x0789), 10, 5, 26);
            AddItem(new Static(0x0523), 10, 5, 66);
            AddItem(new Static(0x0521), 10, 6, 6);
            AddItem(new Static(0x0522), 10, 6, 26);
            AddItem(new Static(0x0523), 10, 6, 66);
            AddItem(new Static(0x0521), 10, 7, 6);
            AddItem(new Static(0x0522), 10, 7, 26);
            AddItem(new Static(0x0523), 10, 7, 66);
            AddItem(new Static(0x0521), 10, 8, 6);
            AddItem(new Static(0x0522), 10, 8, 26);
            AddItem(new Static(0x01D7), 10, 8, 26);
            AddItem(new Static(0x0523), 10, 8, 46);
            AddItem(new Static(0x01D0), 10, 8, 46);
            AddItem(new Static(0x0523), 10, 8, 66);
            AddItem(new Static(0x0521), 10, 9, 6);
            AddItem(new Static(0x0522), 10, 9, 26);
            AddItem(new Static(0x0523), 10, 9, 46);
            AddItem(new Static(0x0523), 10, 9, 66);
            AddItem(new Static(0x0521), 10, 10, 6);
            AddItem(new Static(0x0522), 10, 10, 26);
            AddItem(new Static(0x0523), 10, 10, 46);
            AddItem(new Static(0x0523), 10, 10, 66);
            AddItem(new Static(0x0521), 10, 11, 6);
            AddItem(new Static(0x0522), 10, 11, 26);
            AddItem(new Static(0x0523), 10, 11, 46);
            AddItem(new Static(0x0523), 10, 11, 66);
            AddItem(new Static(0x0521), 10, 12, 6);
            AddItem(new Static(0x0522), 10, 12, 26);
            AddItem(new Static(0x0523), 10, 12, 46);
            AddItem(new Static(0x0523), 10, 12, 66);
            AddItem(new Static(0x01E9), 10, 13, 0);
            AddItem(new Static(0x01E9), 10, 13, 3);
            AddItem(new Static(0x0521), 10, 13, 6);
            AddItem(new Static(0x01D0), 10, 13, 6);
            AddItem(new Static(0x0522), 10, 13, 26);
            AddItem(new Static(0x01D0), 10, 13, 26);
            AddItem(new Static(0x0523), 10, 13, 46);
            AddItem(new Static(0x01D3), 10, 13, 46);
            AddItem(new Static(0x0523), 10, 13, 66);
            AddItem(new Static(0x01E9), 10, 13, 66);
            AddItem(new Static(0x01E9), 10, 13, 69);
            AddItem(new Static(0x01E9), 10, 13, 72);
            AddItem(new Static(0x078C), 10, 14, 1);
            AddItem(new Static(0x0790), 10, 15, 1);
            AddItem(new Static(0x01E9), 11, 0, 0);
            AddItem(new Static(0x01E9), 11, 0, 3);
            AddItem(new Static(0x01D0), 11, 0, 6);
            AddItem(new Static(0x01D0), 11, 0, 26);
            AddItem(new Static(0x01D0), 11, 0, 46);
            AddItem(new Static(0x01E9), 11, 0, 66);
            AddItem(new Static(0x01E9), 11, 0, 69);
            AddItem(new Static(0x0521), 11, 1, 6);
            AddItem(new Static(0x0522), 11, 1, 26);
            AddItem(new Static(0x0788), 11, 1, 26);
            AddItem(new Static(0x0788), 11, 1, 31);
            AddItem(new Static(0x0788), 11, 1, 36);
            AddItem(new Static(0x0788), 11, 1, 41);
            AddItem(new Static(0x0523), 11, 1, 66);
            AddItem(new Static(0x0521), 11, 2, 6);
            AddItem(new Static(0x0522), 11, 2, 26);
            AddItem(new Static(0x0788), 11, 2, 26);
            AddItem(new Static(0x0788), 11, 2, 31);
            AddItem(new Static(0x0788), 11, 2, 36);
            AddItem(new Static(0x0789), 11, 2, 41);
            AddItem(new Static(0x0523), 11, 2, 66);
            AddItem(new Static(0x0521), 11, 3, 6);
            AddItem(new Static(0x0522), 11, 3, 26);
            AddItem(new Static(0x0788), 11, 3, 26);
            AddItem(new Static(0x0788), 11, 3, 31);
            AddItem(new Static(0x0789), 11, 3, 36);
            AddItem(new Static(0x0523), 11, 3, 66);
            AddItem(new Static(0x0521), 11, 4, 6);
            AddItem(new Static(0x0522), 11, 4, 26);
            AddItem(new Static(0x0788), 11, 4, 26);
            AddItem(new Static(0x0789), 11, 4, 31);
            AddItem(new Static(0x0523), 11, 4, 66);
            AddItem(new Static(0x0521), 11, 5, 6);
            AddItem(new Static(0x01D0), 11, 5, 6);
            AddItem(new Static(0x0522), 11, 5, 26);
            AddItem(new Static(0x0789), 11, 5, 26);
            AddItem(new Static(0x0523), 11, 5, 66);
            AddItem(new Static(0x0521), 11, 6, 6);
            AddItem(new Static(0x0522), 11, 6, 26);
            AddItem(new Static(0x0523), 11, 6, 66);
            AddItem(new Static(0x0001), 11, 7, 0);
            AddItem(new Static(0x0521), 11, 7, 6);
            AddItem(new Static(0x0522), 11, 7, 26);
            AddItem(new Static(0x0523), 11, 7, 66);
            AddItem(new Static(0x0521), 11, 8, 6);
            AddItem(new Static(0x0522), 11, 8, 26);
            AddItem(new Static(0x01D0), 11, 8, 26);
            AddItem(new Static(0x0523), 11, 8, 46);
            AddItem(new Static(0x01D0), 11, 8, 46);
            AddItem(new Static(0x0523), 11, 8, 66);
            AddItem(new Static(0x0521), 11, 9, 6);
            AddItem(new Static(0x0522), 11, 9, 26);
            AddItem(new Static(0x0523), 11, 9, 46);
            AddItem(new Static(0x0523), 11, 9, 66);
            AddItem(new Static(0x0521), 11, 10, 6);
            AddItem(new Static(0x0522), 11, 10, 26);
            AddItem(new Static(0x0523), 11, 10, 46);
            AddItem(new Static(0x0523), 11, 10, 66);
            AddItem(new Static(0x0521), 11, 11, 6);
            AddItem(new Static(0x0522), 11, 11, 26);
            AddItem(new Static(0x0523), 11, 11, 46);
            AddItem(new Static(0x0523), 11, 11, 66);
            AddItem(new Static(0x0521), 11, 12, 6);
            AddItem(new Static(0x0522), 11, 12, 26);
            AddItem(new Static(0x0523), 11, 12, 46);
            AddItem(new Static(0x0523), 11, 12, 66);
            AddItem(new Static(0x01E9), 11, 13, 0);
            AddItem(new Static(0x01E9), 11, 13, 3);
            AddItem(new Static(0x0521), 11, 13, 6);

            AddItem(new MetalDoor(DoorFacing.WestCW), 11, 13, 6);
            //AddItem(new Static(0x0675),  11,  13,  6);
            AddItem(new Static(0x0522), 11, 13, 26);
            AddItem(new Static(0x01D0), 11, 13, 26);
            AddItem(new Static(0x0523), 11, 13, 46);
            AddItem(new Static(0x01D0), 11, 13, 46);
            AddItem(new Static(0x0523), 11, 13, 66);
            AddItem(new Static(0x01E9), 11, 13, 66);
            AddItem(new Static(0x01E9), 11, 13, 69);
            AddItem(new Static(0x0788), 11, 14, 1);
            AddItem(new Static(0x0789), 11, 15, 1);
            AddItem(new Static(0x01E9), 12, 0, 0);
            AddItem(new Static(0x01E9), 12, 0, 3);
            AddItem(new Static(0x01D0), 12, 0, 6);
            AddItem(new Static(0x01D0), 12, 0, 26);
            AddItem(new Static(0x01D0), 12, 0, 46);
            AddItem(new Static(0x01E9), 12, 0, 66);
            AddItem(new Static(0x01E9), 12, 0, 69);
            AddItem(new Static(0x01E9), 12, 0, 72);
            AddItem(new Static(0x0521), 12, 1, 6);
            AddItem(new Static(0x0522), 12, 1, 26);
            AddItem(new Static(0x01D1), 12, 1, 26);
            AddItem(new Static(0x0523), 12, 1, 46);
            AddItem(new Static(0x01D1), 12, 1, 46);
            AddItem(new Static(0x0523), 12, 1, 66);
            AddItem(new Static(0x0521), 12, 2, 6);
            AddItem(new Static(0x0522), 12, 2, 26);
            AddItem(new Static(0x01D1), 12, 2, 26);
            AddItem(new Static(0x0523), 12, 2, 46);
            AddItem(new Static(0x01D1), 12, 2, 46);
            AddItem(new Static(0x0523), 12, 2, 66);
            AddItem(new Static(0x0521), 12, 3, 6);
            AddItem(new Static(0x0522), 12, 3, 26);
            AddItem(new Static(0x01D1), 12, 3, 26);
            AddItem(new Static(0x0523), 12, 3, 46);
            AddItem(new Static(0x01D1), 12, 3, 46);
            AddItem(new Static(0x0523), 12, 3, 66);
            AddItem(new Static(0x0521), 12, 4, 6);
            AddItem(new Static(0x0522), 12, 4, 26);
            AddItem(new Static(0x01D1), 12, 4, 26);
            AddItem(new Static(0x0523), 12, 4, 46);
            AddItem(new Static(0x01D1), 12, 4, 46);
            AddItem(new Static(0x0523), 12, 4, 66);
            AddItem(new Static(0x0521), 12, 5, 6);
            AddItem(new Static(0x01D0), 12, 5, 6);
            AddItem(new Static(0x0522), 12, 5, 26);
            AddItem(new Static(0x01D1), 12, 5, 26);
            AddItem(new Static(0x0523), 12, 5, 46);
            AddItem(new Static(0x01D1), 12, 5, 46);
            AddItem(new Static(0x0523), 12, 5, 66);
            AddItem(new Static(0x0521), 12, 6, 6);
            AddItem(new Static(0x0522), 12, 6, 26);
            AddItem(new Static(0x01D1), 12, 6, 26);
            AddItem(new Static(0x0523), 12, 6, 46);
            AddItem(new Static(0x01D1), 12, 6, 46);
            AddItem(new Static(0x0523), 12, 6, 66);
            AddItem(new Static(0x0521), 12, 7, 6);
            AddItem(new Static(0x0522), 12, 7, 26);
            AddItem(new Static(0x01D1), 12, 7, 26);
            AddItem(new Static(0x0523), 12, 7, 46);
            AddItem(new Static(0x01D1), 12, 7, 46);
            AddItem(new Static(0x0523), 12, 7, 66);
            AddItem(new Static(0x0521), 12, 8, 6);
            AddItem(new Static(0x0522), 12, 8, 26);
            AddItem(new Static(0x01CF), 12, 8, 26);
            AddItem(new Static(0x0523), 12, 8, 46);
            AddItem(new Static(0x01CF), 12, 8, 46);
            AddItem(new Static(0x0523), 12, 8, 66);
            AddItem(new Static(0x0521), 12, 9, 6);
            AddItem(new Static(0x0522), 12, 9, 26);
            AddItem(new Static(0x01D1), 12, 9, 26);
            AddItem(new Static(0x0523), 12, 9, 46);
            AddItem(new Static(0x01D1), 12, 9, 46);
            AddItem(new Static(0x0523), 12, 9, 66);
            AddItem(new Static(0x0521), 12, 10, 6);
            AddItem(new Static(0x0522), 12, 10, 26);
            AddItem(new Static(0x01D1), 12, 10, 26);
            AddItem(new Static(0x0523), 12, 10, 46);
            AddItem(new Static(0x01D1), 12, 10, 46);
            AddItem(new Static(0x0523), 12, 10, 66);
            AddItem(new Static(0x0521), 12, 11, 6);
            AddItem(new Static(0x0522), 12, 11, 26);

            AddItem(new MetalDoor(DoorFacing.SouthCW), 12, 11, 26);

            AddItem(new Static(0x0523), 12, 11, 46);

            AddItem(new MetalDoor(DoorFacing.SouthCW), 12, 11, 46);

            AddItem(new Static(0x0523), 12, 11, 66);
            AddItem(new Static(0x0521), 12, 12, 6);
            AddItem(new Static(0x0522), 12, 12, 26);
            AddItem(new Static(0x01D1), 12, 12, 26);
            AddItem(new Static(0x0523), 12, 12, 46);
            AddItem(new Static(0x01D1), 12, 12, 46);
            AddItem(new Static(0x0523), 12, 12, 66);
            AddItem(new Static(0x01E9), 12, 13, 0);
            AddItem(new Static(0x01E9), 12, 13, 3);
            AddItem(new Static(0x0521), 12, 13, 6);

            AddItem(new MetalDoor(DoorFacing.EastCCW), 12, 13, 6);

            AddItem(new Static(0x0522), 12, 13, 26);
            AddItem(new Static(0x01CF), 12, 13, 26);
            AddItem(new Static(0x0523), 12, 13, 46);
            AddItem(new Static(0x01CF), 12, 13, 46);
            AddItem(new Static(0x0523), 12, 13, 66);
            AddItem(new Static(0x01E9), 12, 13, 66);
            AddItem(new Static(0x01E9), 12, 13, 69);
            AddItem(new Static(0x01E9), 12, 13, 72);
            AddItem(new Static(0x0788), 12, 14, 1);
            AddItem(new Static(0x0789), 12, 15, 1);
            AddItem(new Static(0x01E9), 13, 0, 0);
            AddItem(new Static(0x01E9), 13, 0, 3);
            AddItem(new Static(0x01D0), 13, 0, 6);
            AddItem(new Static(0x01D0), 13, 0, 26);
            AddItem(new Static(0x01D0), 13, 0, 46);
            AddItem(new Static(0x01E9), 13, 0, 66);
            AddItem(new Static(0x01E9), 13, 0, 69);
            AddItem(new Static(0x0521), 13, 1, 6);
            AddItem(new Static(0x0522), 13, 1, 26);
            AddItem(new Static(0x0523), 13, 1, 46);
            AddItem(new Static(0x0523), 13, 1, 66);
            AddItem(new Static(0x0521), 13, 2, 6);
            AddItem(new Static(0x0522), 13, 2, 26);
            AddItem(new Static(0x0523), 13, 2, 46);
            AddItem(new Static(0x0523), 13, 2, 66);
            AddItem(new Static(0x0521), 13, 3, 6);
            AddItem(new Static(0x0522), 13, 3, 26);
            AddItem(new Static(0x0523), 13, 3, 46);
            AddItem(new Static(0x0523), 13, 3, 66);
            AddItem(new Static(0x0521), 13, 4, 6);
            AddItem(new Static(0x0522), 13, 4, 26);
            AddItem(new Static(0x0523), 13, 4, 46);
            AddItem(new Static(0x0523), 13, 4, 66);
            AddItem(new Static(0x0521), 13, 5, 6);
            AddItem(new Static(0x01D0), 13, 5, 6);
            AddItem(new Static(0x0522), 13, 5, 26);
            AddItem(new Static(0x0523), 13, 5, 46);
            AddItem(new Static(0x0523), 13, 5, 66);
            AddItem(new Static(0x0521), 13, 6, 6);
            AddItem(new Static(0x0522), 13, 6, 26);
            AddItem(new Static(0x0523), 13, 6, 46);
            AddItem(new Static(0x0523), 13, 6, 66);
            AddItem(new Static(0x0521), 13, 7, 6);
            AddItem(new Static(0x0522), 13, 7, 26);
            AddItem(new Static(0x0523), 13, 7, 46);
            AddItem(new Static(0x0523), 13, 7, 66);
            AddItem(new Static(0x0521), 13, 8, 6);
            AddItem(new Static(0x0522), 13, 8, 26);
            AddItem(new Static(0x0523), 13, 8, 46);
            AddItem(new Static(0x0523), 13, 8, 66);
            AddItem(new Static(0x0523), 13, 8, 66);
            AddItem(new Static(0x0521), 13, 9, 6);
            AddItem(new Static(0x0522), 13, 9, 26);
            AddItem(new Static(0x0523), 13, 9, 46);
            AddItem(new Static(0x0523), 13, 9, 66);
            AddItem(new Static(0x0523), 13, 9, 66);
            AddItem(new Static(0x0521), 13, 10, 6);
            AddItem(new Static(0x0522), 13, 10, 26);
            AddItem(new Static(0x0523), 13, 10, 46);
            AddItem(new Static(0x0523), 13, 10, 66);
            AddItem(new Static(0x0523), 13, 10, 66);
            AddItem(new Static(0x0521), 13, 11, 6);
            AddItem(new Static(0x0522), 13, 11, 26);
            AddItem(new Static(0x0523), 13, 11, 46);
            AddItem(new Static(0x0523), 13, 11, 66);
            AddItem(new Static(0x0523), 13, 11, 66);
            AddItem(new Static(0x0521), 13, 12, 6);
            AddItem(new Static(0x0522), 13, 12, 26);
            AddItem(new Static(0x0523), 13, 12, 46);
            AddItem(new Static(0x0523), 13, 12, 66);
            AddItem(new Static(0x0523), 13, 12, 66);
            AddItem(new Static(0x01E9), 13, 13, 0);
            AddItem(new Static(0x01E9), 13, 13, 3);
            AddItem(new Static(0x0521), 13, 13, 6);
            AddItem(new Static(0x01D0), 13, 13, 6);
            AddItem(new Static(0x0522), 13, 13, 26);
            AddItem(new Static(0x01D0), 13, 13, 26);
            AddItem(new Static(0x0523), 13, 13, 46);
            AddItem(new Static(0x01D0), 13, 13, 46);
            AddItem(new Static(0x0523), 13, 13, 66);
            AddItem(new Static(0x0523), 13, 13, 66);
            AddItem(new Static(0x01E9), 13, 13, 66);
            AddItem(new Static(0x01E9), 13, 13, 69);
            AddItem(new Static(0x078A), 13, 14, 1);
            AddItem(new Static(0x078E), 13, 15, 1);
            AddItem(new Static(0x01E9), 14, 0, 0);
            AddItem(new Static(0x01E9), 14, 0, 3);
            AddItem(new Static(0x01D0), 14, 0, 6);
            AddItem(new Static(0x01D0), 14, 0, 26);
            AddItem(new Static(0x01D0), 14, 0, 46);
            AddItem(new Static(0x01E9), 14, 0, 66);
            AddItem(new Static(0x01E9), 14, 0, 69);
            AddItem(new Static(0x01E9), 14, 0, 72);
            AddItem(new Static(0x0521), 14, 1, 6);
            AddItem(new Static(0x0522), 14, 1, 26);
            AddItem(new Static(0x0523), 14, 1, 46);
            AddItem(new Static(0x0523), 14, 1, 66);
            AddItem(new Static(0x0521), 14, 2, 6);
            AddItem(new Static(0x0522), 14, 2, 26);
            AddItem(new Static(0x0523), 14, 2, 46);
            AddItem(new Static(0x0523), 14, 2, 66);
            AddItem(new Static(0x0521), 14, 3, 6);
            AddItem(new Static(0x0522), 14, 3, 26);
            AddItem(new Static(0x0523), 14, 3, 46);
            AddItem(new Static(0x0523), 14, 3, 66);
            AddItem(new Static(0x0521), 14, 4, 6);
            AddItem(new Static(0x0522), 14, 4, 26);
            AddItem(new Static(0x0523), 14, 4, 46);
            AddItem(new Static(0x0523), 14, 4, 66);
            AddItem(new Static(0x0521), 14, 5, 6);

            AddItem(new MetalDoor(DoorFacing.WestCW), 14, 5, 6);

            AddItem(new Static(0x0522), 14, 5, 26);
            AddItem(new Static(0x0523), 14, 5, 46);
            AddItem(new Static(0x0523), 14, 5, 66);
            AddItem(new Static(0x0521), 14, 6, 6);
            AddItem(new Static(0x0522), 14, 6, 26);
            AddItem(new Static(0x0523), 14, 6, 46);
            AddItem(new Static(0x0523), 14, 6, 66);
            AddItem(new Static(0x0521), 14, 7, 6);
            AddItem(new Static(0x0522), 14, 7, 26);
            AddItem(new Static(0x0523), 14, 7, 46);
            AddItem(new Static(0x0523), 14, 7, 66);
            AddItem(new Static(0x0521), 14, 8, 6);
            AddItem(new Static(0x0522), 14, 8, 26);
            AddItem(new Static(0x0523), 14, 8, 46);
            AddItem(new Static(0x0523), 14, 8, 66);
            AddItem(new Static(0x0521), 14, 9, 6);
            AddItem(new Static(0x0522), 14, 9, 26);
            AddItem(new Static(0x0523), 14, 9, 46);
            AddItem(new Static(0x0523), 14, 9, 66);
            AddItem(new Static(0x0521), 14, 10, 6);
            AddItem(new Static(0x0522), 14, 10, 26);
            AddItem(new Static(0x0523), 14, 10, 46);
            AddItem(new Static(0x0523), 14, 10, 66);
            AddItem(new Static(0x0521), 14, 11, 6);
            AddItem(new Static(0x0522), 14, 11, 26);
            AddItem(new Static(0x0523), 14, 11, 46);
            AddItem(new Static(0x0523), 14, 11, 66);
            AddItem(new Static(0x0521), 14, 12, 6);
            AddItem(new Static(0x0522), 14, 12, 26);
            AddItem(new Static(0x0523), 14, 12, 46);
            AddItem(new Static(0x0523), 14, 12, 66);
            AddItem(new Static(0x01E9), 14, 13, 0);
            AddItem(new Static(0x01E9), 14, 13, 3);
            AddItem(new Static(0x0521), 14, 13, 6);
            AddItem(new Static(0x01D0), 14, 13, 6);
            AddItem(new Static(0x0522), 14, 13, 26);
            AddItem(new Static(0x01D0), 14, 13, 26);
            AddItem(new Static(0x0523), 14, 13, 46);
            AddItem(new Static(0x01D0), 14, 13, 46);
            AddItem(new Static(0x0523), 14, 13, 66);
            AddItem(new Static(0x01E9), 14, 13, 66);
            AddItem(new Static(0x01E9), 14, 13, 69);
            AddItem(new Static(0x01E9), 14, 13, 72);
            AddItem(new Static(0x01E9), 15, 0, 0);
            AddItem(new Static(0x01E9), 15, 0, 3);
            AddItem(new Static(0x01D0), 15, 0, 6);
            AddItem(new Static(0x01D0), 15, 0, 26);
            AddItem(new Static(0x01D0), 15, 0, 46);
            AddItem(new Static(0x01E9), 15, 0, 66);
            AddItem(new Static(0x01E9), 15, 0, 69);
            AddItem(new Static(0x0521), 15, 1, 6);
            AddItem(new Static(0x0522), 15, 1, 26);
            AddItem(new Static(0x0523), 15, 1, 46);
            AddItem(new Static(0x0523), 15, 1, 66);
            AddItem(new Static(0x0521), 15, 2, 6);
            AddItem(new Static(0x0522), 15, 2, 26);
            AddItem(new Static(0x0523), 15, 2, 46);
            AddItem(new Static(0x0523), 15, 2, 66);
            AddItem(new Static(0x0521), 15, 3, 6);
            AddItem(new Static(0x0522), 15, 3, 26);
            AddItem(new Static(0x0523), 15, 3, 46);
            AddItem(new Static(0x0523), 15, 3, 66);
            AddItem(new Static(0x0521), 15, 4, 6);
            AddItem(new Static(0x0522), 15, 4, 26);
            AddItem(new Static(0x0523), 15, 4, 46);
            AddItem(new Static(0x0523), 15, 4, 66);
            AddItem(new Static(0x0521), 15, 5, 6);
            AddItem(new Static(0x01D0), 15, 5, 6);
            AddItem(new Static(0x0522), 15, 5, 26);
            AddItem(new Static(0x0523), 15, 5, 46);
            AddItem(new Static(0x0523), 15, 5, 66);
            AddItem(new Static(0x0521), 15, 6, 6);
            AddItem(new Static(0x0522), 15, 6, 26);
            AddItem(new Static(0x0523), 15, 6, 46);
            AddItem(new Static(0x0523), 15, 6, 66);
            AddItem(new Static(0x0521), 15, 7, 6);
            AddItem(new Static(0x0522), 15, 7, 26);
            AddItem(new Static(0x0523), 15, 7, 46);
            AddItem(new Static(0x0523), 15, 7, 66);
            AddItem(new Static(0x0521), 15, 8, 6);
            AddItem(new Static(0x0522), 15, 8, 26);
            AddItem(new Static(0x0523), 15, 8, 46);
            AddItem(new Static(0x0523), 15, 8, 66);
            AddItem(new Static(0x0521), 15, 9, 6);
            AddItem(new Static(0x0522), 15, 9, 26);
            AddItem(new Static(0x0523), 15, 9, 46);
            AddItem(new Static(0x0523), 15, 9, 66);
            AddItem(new Static(0x0521), 15, 10, 6);
            AddItem(new Static(0x0522), 15, 10, 26);
            AddItem(new Static(0x0523), 15, 10, 46);
            AddItem(new Static(0x0523), 15, 10, 66);
            AddItem(new Static(0x0521), 15, 11, 6);
            AddItem(new Static(0x0522), 15, 11, 26);
            AddItem(new Static(0x0523), 15, 11, 46);
            AddItem(new Static(0x0523), 15, 11, 66);
            AddItem(new Static(0x0521), 15, 12, 6);
            AddItem(new Static(0x0522), 15, 12, 26);
            AddItem(new Static(0x0523), 15, 12, 46);
            AddItem(new Static(0x0523), 15, 12, 66);
            AddItem(new Static(0x01E9), 15, 13, 0);
            AddItem(new Static(0x01E9), 15, 13, 3);
            AddItem(new Static(0x0521), 15, 13, 6);
            AddItem(new Static(0x01D0), 15, 13, 6);
            AddItem(new Static(0x0522), 15, 13, 26);
            AddItem(new Static(0x01D0), 15, 13, 26);
            AddItem(new Static(0x0523), 15, 13, 46);
            AddItem(new Static(0x01D3), 15, 13, 46);
            AddItem(new Static(0x0523), 15, 13, 66);
            AddItem(new Static(0x01E9), 15, 13, 66);
            AddItem(new Static(0x01E9), 15, 13, 69);
            // Sign.
            // AddItem(new Static(0x0BD2),  15,  14,  5);
            AddItem(new Static(0x0B9E), 15, 14, 5);
            AddItem(new Static(0x01E9), 16, 0, 0);
            AddItem(new Static(0x01E9), 16, 0, 3);
            AddItem(new Static(0x01D0), 16, 0, 6);
            AddItem(new Static(0x01D0), 16, 0, 26);
            AddItem(new Static(0x01D0), 16, 0, 46);
            AddItem(new Static(0x01E9), 16, 0, 66);
            AddItem(new Static(0x01E9), 16, 0, 69);
            AddItem(new Static(0x01E9), 16, 0, 72);
            AddItem(new Static(0x0521), 16, 1, 6);
            AddItem(new Static(0x0522), 16, 1, 26);
            AddItem(new Static(0x0523), 16, 1, 46);
            AddItem(new Static(0x0523), 16, 1, 66);
            AddItem(new Static(0x0521), 16, 2, 6);
            AddItem(new Static(0x0522), 16, 2, 26);
            AddItem(new Static(0x0523), 16, 2, 46);
            AddItem(new Static(0x0523), 16, 2, 66);
            AddItem(new Static(0x0521), 16, 3, 6);
            AddItem(new Static(0x0522), 16, 3, 26);
            AddItem(new Static(0x0523), 16, 3, 46);
            AddItem(new Static(0x0523), 16, 3, 66);
            AddItem(new Static(0x0521), 16, 4, 6);
            AddItem(new Static(0x0522), 16, 4, 26);
            AddItem(new Static(0x0523), 16, 4, 46);
            AddItem(new Static(0x0523), 16, 4, 66);
            AddItem(new Static(0x0521), 16, 5, 6);
            AddItem(new Static(0x01D0), 16, 5, 6);
            AddItem(new Static(0x0522), 16, 5, 26);
            AddItem(new Static(0x0523), 16, 5, 46);
            AddItem(new Static(0x0523), 16, 5, 66);
            AddItem(new Static(0x0521), 16, 6, 6);
            AddItem(new Static(0x0522), 16, 6, 26);
            AddItem(new Static(0x0523), 16, 6, 46);
            AddItem(new Static(0x0523), 16, 6, 66);
            AddItem(new Static(0x0521), 16, 7, 6);
            AddItem(new Static(0x0522), 16, 7, 26);
            AddItem(new Static(0x0523), 16, 7, 46);
            AddItem(new Static(0x0523), 16, 7, 66);
            AddItem(new Static(0x0521), 16, 8, 6);
            AddItem(new Static(0x0522), 16, 8, 26);
            AddItem(new Static(0x0523), 16, 8, 46);
            AddItem(new Static(0x0523), 16, 8, 66);
            AddItem(new Static(0x0521), 16, 9, 6);
            AddItem(new Static(0x0522), 16, 9, 26);
            AddItem(new Static(0x0523), 16, 9, 46);
            AddItem(new Static(0x0523), 16, 9, 66);
            AddItem(new Static(0x0521), 16, 10, 6);
            AddItem(new Static(0x0522), 16, 10, 26);
            AddItem(new Static(0x0523), 16, 10, 46);
            AddItem(new Static(0x0523), 16, 10, 66);
            AddItem(new Static(0x0521), 16, 11, 6);
            AddItem(new Static(0x0522), 16, 11, 26);
            AddItem(new Static(0x0523), 16, 11, 46);
            AddItem(new Static(0x0523), 16, 11, 66);
            AddItem(new Static(0x0521), 16, 12, 6);
            AddItem(new Static(0x0522), 16, 12, 26);
            AddItem(new Static(0x0523), 16, 12, 46);
            AddItem(new Static(0x0523), 16, 12, 66);
            AddItem(new Static(0x01E9), 16, 13, 0);
            AddItem(new Static(0x01E9), 16, 13, 3);
            AddItem(new Static(0x0521), 16, 13, 6);
            AddItem(new Static(0x01D0), 16, 13, 6);
            AddItem(new Static(0x0522), 16, 13, 26);
            AddItem(new Static(0x01D3), 16, 13, 26);
            AddItem(new Static(0x0523), 16, 13, 46);
            AddItem(new Static(0x01D0), 16, 13, 46);
            AddItem(new Static(0x0523), 16, 13, 66);
            AddItem(new Static(0x01E9), 16, 13, 66);
            AddItem(new Static(0x01E9), 16, 13, 69);
            AddItem(new Static(0x01E9), 16, 13, 72);
            AddItem(new Static(0x01E9), 17, 0, 0);
            AddItem(new Static(0x01E9), 17, 0, 3);
            AddItem(new Static(0x01D0), 17, 0, 6);
            AddItem(new Static(0x01D0), 17, 0, 26);
            AddItem(new Static(0x01D0), 17, 0, 46);
            AddItem(new Static(0x01E9), 17, 0, 66);
            AddItem(new Static(0x01E9), 17, 0, 69);
            AddItem(new Static(0x0521), 17, 1, 6);
            AddItem(new Static(0x0522), 17, 1, 26);
            AddItem(new Static(0x0523), 17, 1, 46);
            AddItem(new Static(0x0523), 17, 1, 66);
            AddItem(new Static(0x0521), 17, 2, 6);
            AddItem(new Static(0x0522), 17, 2, 26);
            AddItem(new Static(0x0523), 17, 2, 46);
            AddItem(new Static(0x0523), 17, 2, 66);
            AddItem(new Static(0x0521), 17, 3, 6);
            AddItem(new Static(0x0522), 17, 3, 26);
            AddItem(new Static(0x0523), 17, 3, 46);
            AddItem(new Static(0x0523), 17, 3, 66);
            AddItem(new Static(0x0521), 17, 4, 6);
            AddItem(new Static(0x0522), 17, 4, 26);
            AddItem(new Static(0x0523), 17, 4, 46);
            AddItem(new Static(0x0523), 17, 4, 66);
            AddItem(new Static(0x0521), 17, 5, 6);
            AddItem(new Static(0x01D0), 17, 5, 6);
            AddItem(new Static(0x0522), 17, 5, 26);
            AddItem(new Static(0x0523), 17, 5, 46);
            AddItem(new Static(0x0523), 17, 5, 66);
            AddItem(new Static(0x0521), 17, 6, 6);
            AddItem(new Static(0x0522), 17, 6, 26);
            AddItem(new Static(0x0523), 17, 6, 46);
            AddItem(new Static(0x0523), 17, 6, 66);
            AddItem(new Static(0x0521), 17, 7, 6);
            AddItem(new Static(0x0522), 17, 7, 26);
            AddItem(new Static(0x0523), 17, 7, 46);
            AddItem(new Static(0x0523), 17, 7, 66);
            AddItem(new Static(0x0521), 17, 8, 6);
            AddItem(new Static(0x0522), 17, 8, 26);
            AddItem(new Static(0x0523), 17, 8, 46);
            AddItem(new Static(0x0523), 17, 8, 66);
            AddItem(new Static(0x0521), 17, 9, 6);
            AddItem(new Static(0x0522), 17, 9, 26);
            AddItem(new Static(0x0523), 17, 9, 46);
            AddItem(new Static(0x0523), 17, 9, 66);
            AddItem(new Static(0x0521), 17, 10, 6);
            AddItem(new Static(0x0522), 17, 10, 26);
            AddItem(new Static(0x0523), 17, 10, 46);
            AddItem(new Static(0x0523), 17, 10, 66);
            AddItem(new Static(0x0521), 17, 11, 6);
            AddItem(new Static(0x0522), 17, 11, 26);
            AddItem(new Static(0x0523), 17, 11, 46);
            AddItem(new Static(0x0523), 17, 11, 66);
            AddItem(new Static(0x0521), 17, 12, 6);
            AddItem(new Static(0x0522), 17, 12, 26);
            AddItem(new Static(0x0523), 17, 12, 46);
            AddItem(new Static(0x0523), 17, 12, 66);
            AddItem(new Static(0x01E9), 17, 13, 0);
            AddItem(new Static(0x01E9), 17, 13, 3);
            AddItem(new Static(0x0521), 17, 13, 6);
            AddItem(new Static(0x01D0), 17, 13, 6);
            AddItem(new Static(0x0522), 17, 13, 26);
            AddItem(new Static(0x01D0), 17, 13, 26);
            AddItem(new Static(0x0523), 17, 13, 46);
            AddItem(new Static(0x01D0), 17, 13, 46);
            AddItem(new Static(0x0523), 17, 13, 66);
            AddItem(new Static(0x01E9), 17, 13, 66);
            AddItem(new Static(0x01E9), 17, 13, 69);
            AddItem(new Static(0x01E9), 18, 0, 0);
            AddItem(new Static(0x01E9), 18, 0, 3);
            AddItem(new Static(0x01D0), 18, 0, 6);
            AddItem(new Static(0x01D0), 18, 0, 26);
            AddItem(new Static(0x01D0), 18, 0, 46);
            AddItem(new Static(0x01E9), 18, 0, 66);
            AddItem(new Static(0x01E9), 18, 0, 69);
            AddItem(new Static(0x01E9), 18, 0, 72);
            AddItem(new Static(0x0521), 18, 1, 6);
            AddItem(new Static(0x0522), 18, 1, 26);
            AddItem(new Static(0x0523), 18, 1, 46);
            AddItem(new Static(0x0523), 18, 1, 66);
            AddItem(new Static(0x0521), 18, 2, 6);
            AddItem(new Static(0x0522), 18, 2, 26);
            AddItem(new Static(0x0523), 18, 2, 46);
            AddItem(new Static(0x0523), 18, 2, 66);
            AddItem(new Static(0x0521), 18, 3, 6);
            AddItem(new Static(0x0522), 18, 3, 26);
            AddItem(new Static(0x0523), 18, 3, 46);
            AddItem(new Static(0x0523), 18, 3, 66);
            AddItem(new Static(0x0521), 18, 4, 6);
            AddItem(new Static(0x0522), 18, 4, 26);
            AddItem(new Static(0x0523), 18, 4, 46);
            AddItem(new Static(0x0523), 18, 4, 66);
            AddItem(new Static(0x0521), 18, 5, 6);
            AddItem(new Static(0x01D0), 18, 5, 6);
            AddItem(new Static(0x0522), 18, 5, 26);
            AddItem(new Static(0x0523), 18, 5, 46);
            AddItem(new Static(0x0523), 18, 5, 66);
            AddItem(new Static(0x0521), 18, 6, 6);
            AddItem(new Static(0x0522), 18, 6, 26);
            AddItem(new Static(0x0523), 18, 6, 46);
            AddItem(new Static(0x0523), 18, 6, 66);
            AddItem(new Static(0x0521), 18, 7, 6);
            AddItem(new Static(0x0522), 18, 7, 26);
            AddItem(new Static(0x0523), 18, 7, 46);
            AddItem(new Static(0x0523), 18, 7, 66);
            AddItem(new Static(0x0521), 18, 8, 6);
            AddItem(new Static(0x0522), 18, 8, 26);
            AddItem(new Static(0x0523), 18, 8, 46);
            AddItem(new Static(0x0523), 18, 8, 66);
            AddItem(new Static(0x0521), 18, 9, 6);
            AddItem(new Static(0x0522), 18, 9, 26);
            AddItem(new Static(0x0523), 18, 9, 46);
            AddItem(new Static(0x0523), 18, 9, 66);
            AddItem(new Static(0x0521), 18, 10, 6);
            AddItem(new Static(0x0522), 18, 10, 26);
            AddItem(new Static(0x0523), 18, 10, 46);
            AddItem(new Static(0x0523), 18, 10, 66);
            AddItem(new Static(0x0521), 18, 11, 6);
            AddItem(new Static(0x0522), 18, 11, 26);
            AddItem(new Static(0x0523), 18, 11, 46);
            AddItem(new Static(0x0523), 18, 11, 66);
            AddItem(new Static(0x0521), 18, 12, 6);
            AddItem(new Static(0x0522), 18, 12, 26);
            AddItem(new Static(0x0523), 18, 12, 46);
            AddItem(new Static(0x0523), 18, 12, 66);
            AddItem(new Static(0x01E9), 18, 13, 0);
            AddItem(new Static(0x01E9), 18, 13, 3);
            AddItem(new Static(0x0521), 18, 13, 6);
            AddItem(new Static(0x01D0), 18, 13, 6);
            AddItem(new Static(0x0522), 18, 13, 26);
            AddItem(new Static(0x01D0), 18, 13, 26);
            AddItem(new Static(0x0523), 18, 13, 46);
            AddItem(new Static(0x01D3), 18, 13, 46);
            AddItem(new Static(0x0523), 18, 13, 66);
            AddItem(new Static(0x01E9), 18, 13, 66);
            AddItem(new Static(0x01E9), 18, 13, 69);
            AddItem(new Static(0x01E9), 18, 13, 72);
            AddItem(new Static(0x01E9), 19, 0, 0);
            AddItem(new Static(0x01E9), 19, 0, 3);
            AddItem(new Static(0x01D0), 19, 0, 6);
            AddItem(new Static(0x01D0), 19, 0, 26);
            AddItem(new Static(0x01D0), 19, 0, 46);
            AddItem(new Static(0x01E9), 19, 0, 66);
            AddItem(new Static(0x01E9), 19, 0, 69);
            AddItem(new Static(0x01DA), 19, 0, 72);
            AddItem(new Static(0x01EA), 19, 1, 0);
            AddItem(new Static(0x01EA), 19, 1, 3);
            AddItem(new Static(0x0521), 19, 1, 6);
            AddItem(new Static(0x01D1), 19, 1, 6);
            AddItem(new Static(0x0522), 19, 1, 26);
            AddItem(new Static(0x01D1), 19, 1, 26);
            AddItem(new Static(0x0523), 19, 1, 46);
            AddItem(new Static(0x01D1), 19, 1, 46);
            AddItem(new Static(0x0523), 19, 1, 66);
            AddItem(new Static(0x01EA), 19, 1, 66);
            AddItem(new Static(0x01EA), 19, 1, 69);
            AddItem(new Static(0x01EA), 19, 2, 0);
            AddItem(new Static(0x01EA), 19, 2, 3);
            AddItem(new Static(0x0521), 19, 2, 6);
            AddItem(new Static(0x01D1), 19, 2, 6);
            AddItem(new Static(0x0522), 19, 2, 26);
            AddItem(new Static(0x01D1), 19, 2, 26);
            AddItem(new Static(0x0523), 19, 2, 46);
            AddItem(new Static(0x01D1), 19, 2, 46);
            AddItem(new Static(0x0523), 19, 2, 66);
            AddItem(new Static(0x01EA), 19, 2, 66);
            AddItem(new Static(0x01EA), 19, 2, 69);
            AddItem(new Static(0x01EA), 19, 2, 72);
            AddItem(new Static(0x01EA), 19, 3, 0);
            AddItem(new Static(0x01EA), 19, 3, 3);
            AddItem(new Static(0x0521), 19, 3, 6);
            AddItem(new Static(0x01D1), 19, 3, 6);
            AddItem(new Static(0x0522), 19, 3, 26);
            AddItem(new Static(0x01D1), 19, 3, 26);
            AddItem(new Static(0x0523), 19, 3, 46);
            AddItem(new Static(0x01D4), 19, 3, 46);
            AddItem(new Static(0x0523), 19, 3, 66);
            AddItem(new Static(0x01EA), 19, 3, 66);
            AddItem(new Static(0x01EA), 19, 3, 69);
            AddItem(new Static(0x01EA), 19, 4, 0);
            AddItem(new Static(0x01EA), 19, 4, 3);
            AddItem(new Static(0x0521), 19, 4, 6);
            AddItem(new Static(0x01D1), 19, 4, 6);
            AddItem(new Static(0x0522), 19, 4, 26);
            AddItem(new Static(0x01D4), 19, 4, 26);
            AddItem(new Static(0x0523), 19, 4, 46);
            AddItem(new Static(0x01D1), 19, 4, 46);
            AddItem(new Static(0x0523), 19, 4, 66);
            AddItem(new Static(0x01EA), 19, 4, 66);
            AddItem(new Static(0x01EA), 19, 4, 69);
            AddItem(new Static(0x01EA), 19, 4, 72);
            AddItem(new Static(0x01EA), 19, 5, 0);
            AddItem(new Static(0x01EA), 19, 5, 3);
            AddItem(new Static(0x0521), 19, 5, 6);
            AddItem(new Static(0x01CF), 19, 5, 6);
            AddItem(new Static(0x0522), 19, 5, 26);
            AddItem(new Static(0x01D1), 19, 5, 26);
            AddItem(new Static(0x0523), 19, 5, 46);
            AddItem(new Static(0x01D4), 19, 5, 46);
            AddItem(new Static(0x0523), 19, 5, 66);
            AddItem(new Static(0x01EA), 19, 5, 66);
            AddItem(new Static(0x01EA), 19, 5, 69);
            AddItem(new Static(0x01EA), 19, 6, 0);
            AddItem(new Static(0x01EA), 19, 6, 3);
            AddItem(new Static(0x0521), 19, 6, 6);
            AddItem(new Static(0x01D1), 19, 6, 6);
            AddItem(new Static(0x0522), 19, 6, 26);
            AddItem(new Static(0x01D1), 19, 6, 26);
            AddItem(new Static(0x0523), 19, 6, 46);
            AddItem(new Static(0x01D1), 19, 6, 46);
            AddItem(new Static(0x0523), 19, 6, 66);
            AddItem(new Static(0x01EA), 19, 6, 66);
            AddItem(new Static(0x01EA), 19, 6, 69);
            AddItem(new Static(0x01EA), 19, 6, 72);
            AddItem(new Static(0x01EA), 19, 7, 0);
            AddItem(new Static(0x01EA), 19, 7, 3);
            AddItem(new Static(0x0521), 19, 7, 6);
            AddItem(new Static(0x01D1), 19, 7, 6);
            AddItem(new Static(0x0522), 19, 7, 26);
            AddItem(new Static(0x01D1), 19, 7, 26);
            AddItem(new Static(0x0523), 19, 7, 46);
            AddItem(new Static(0x01D1), 19, 7, 46);
            AddItem(new Static(0x0523), 19, 7, 66);
            AddItem(new Static(0x01EA), 19, 7, 66);
            AddItem(new Static(0x01EA), 19, 7, 69);
            AddItem(new Static(0x01EA), 19, 8, 0);
            AddItem(new Static(0x01EA), 19, 8, 3);
            AddItem(new Static(0x0521), 19, 8, 6);
            AddItem(new Static(0x01D1), 19, 8, 6);
            AddItem(new Static(0x0522), 19, 8, 26);
            AddItem(new Static(0x01D1), 19, 8, 26);
            AddItem(new Static(0x0523), 19, 8, 46);
            AddItem(new Static(0x01D1), 19, 8, 46);
            AddItem(new Static(0x0523), 19, 8, 66);
            AddItem(new Static(0x01EA), 19, 9, 0);
            AddItem(new Static(0x01EA), 19, 9, 3);
            AddItem(new Static(0x0521), 19, 9, 6);
            AddItem(new Static(0x01D1), 19, 9, 6);
            AddItem(new Static(0x0522), 19, 9, 26);
            AddItem(new Static(0x01D1), 19, 9, 26);
            AddItem(new Static(0x0523), 19, 9, 46);
            AddItem(new Static(0x01D1), 19, 9, 46);
            AddItem(new Static(0x0523), 19, 9, 66);
            AddItem(new Static(0x01EA), 19, 10, 0);
            AddItem(new Static(0x01EA), 19, 10, 3);
            AddItem(new Static(0x0521), 19, 10, 6);
            AddItem(new Static(0x01D1), 19, 10, 6);
            AddItem(new Static(0x0522), 19, 10, 26);
            AddItem(new Static(0x01D1), 19, 10, 26);
            AddItem(new Static(0x0523), 19, 10, 46);
            AddItem(new Static(0x01D1), 19, 10, 46);
            AddItem(new Static(0x0523), 19, 10, 66);
            AddItem(new Static(0x01EA), 19, 11, 0);
            AddItem(new Static(0x01EA), 19, 11, 3);
            AddItem(new Static(0x0521), 19, 11, 6);
            AddItem(new Static(0x01D1), 19, 11, 6);
            AddItem(new Static(0x0522), 19, 11, 26);
            AddItem(new Static(0x01D4), 19, 11, 26);
            AddItem(new Static(0x0523), 19, 11, 46);
            AddItem(new Static(0x01D1), 19, 11, 46);
            AddItem(new Static(0x0523), 19, 11, 66);
            AddItem(new Static(0x01EA), 19, 12, 0);
            AddItem(new Static(0x01EA), 19, 12, 3);
            AddItem(new Static(0x0521), 19, 12, 6);
            AddItem(new Static(0x01D1), 19, 12, 6);
            AddItem(new Static(0x0522), 19, 12, 26);
            AddItem(new Static(0x01D1), 19, 12, 26);
            AddItem(new Static(0x0523), 19, 12, 46);
            AddItem(new Static(0x01D1), 19, 12, 46);
            AddItem(new Static(0x0523), 19, 12, 66);
            AddItem(new Static(0x01E8), 19, 13, 0);
            AddItem(new Static(0x01E8), 19, 13, 3);
            AddItem(new Static(0x0521), 19, 13, 6);
            AddItem(new Static(0x01CF), 19, 13, 6);
            AddItem(new Static(0x0522), 19, 13, 26);
            AddItem(new Static(0x01CF), 19, 13, 26);
            AddItem(new Static(0x0523), 19, 13, 46);
            AddItem(new Static(0x01CF), 19, 13, 46);
            AddItem(new Static(0x0523), 19, 13, 66);
            AddItem(new Static(0x01E9), 19, 13, 66);
            AddItem(new Static(0x01E9), 19, 13, 69);
            AddItem(new Static(0x01E9), 20, 7, 66);
            AddItem(new Static(0x01E9), 20, 7, 69);
            AddItem(new Static(0x01E9), 20, 7, 72);
            AddItem(new Static(0x0785), 20, 8, 46);
            AddItem(new Static(0x0788), 20, 8, 51);
            AddItem(new Static(0x0788), 20, 8, 56);
            AddItem(new Static(0x0788), 20, 8, 61);
            AddItem(new Static(0x0524), 20, 8, 66);
            AddItem(new Static(0x0524), 20, 9, 66);
            AddItem(new Static(0x0524), 20, 10, 66);
            AddItem(new Static(0x0524), 20, 11, 66);
            AddItem(new Static(0x0524), 20, 12, 66);
            AddItem(new Static(0x0785), 20, 13, 46);
            AddItem(new Static(0x0788), 20, 13, 51);
            AddItem(new Static(0x0788), 20, 13, 56);
            AddItem(new Static(0x0788), 20, 13, 61);
            AddItem(new Static(0x0524), 20, 13, 66);
            AddItem(new Static(0x01E9), 20, 13, 66);
            AddItem(new Static(0x01E9), 20, 13, 69);
            AddItem(new Static(0x01E9), 20, 13, 72);
            AddItem(new Static(0x01E9), 21, 7, 66);
            AddItem(new Static(0x01E9), 21, 7, 69);
            AddItem(new Static(0x0785), 21, 8, 51);
            AddItem(new Static(0x0788), 21, 8, 56);
            AddItem(new Static(0x0788), 21, 8, 61);
            AddItem(new Static(0x0524), 21, 8, 66);
            AddItem(new Static(0x0524), 21, 9, 66);
            AddItem(new Static(0x0524), 21, 10, 66);
            AddItem(new Static(0x0524), 21, 11, 66);
            AddItem(new Static(0x0524), 21, 12, 66);
            AddItem(new Static(0x0785), 21, 13, 51);
            AddItem(new Static(0x0788), 21, 13, 56);
            AddItem(new Static(0x0788), 21, 13, 61);
            AddItem(new Static(0x0524), 21, 13, 66);
            AddItem(new Static(0x01E9), 21, 13, 66);
            AddItem(new Static(0x01E9), 21, 13, 69);
            AddItem(new Static(0x01E9), 22, 7, 66);
            AddItem(new Static(0x01E9), 22, 7, 69);
            AddItem(new Static(0x01E9), 22, 7, 72);
            AddItem(new Static(0x0785), 22, 8, 56);
            AddItem(new Static(0x0788), 22, 8, 61);
            AddItem(new Static(0x0524), 22, 8, 66);
            AddItem(new Static(0x0524), 22, 9, 66);
            AddItem(new Static(0x0524), 22, 10, 66);
            AddItem(new Static(0x0524), 22, 11, 66);
            AddItem(new Static(0x0524), 22, 12, 66);
            AddItem(new Static(0x0785), 22, 13, 56);
            AddItem(new Static(0x0788), 22, 13, 61);
            AddItem(new Static(0x0524), 22, 13, 66);
            AddItem(new Static(0x01E9), 22, 13, 66);
            AddItem(new Static(0x01E9), 22, 13, 69);
            AddItem(new Static(0x01E9), 22, 13, 72);
            AddItem(new Static(0x01E9), 23, 7, 66);
            AddItem(new Static(0x01E9), 23, 7, 69);
            AddItem(new Static(0x01DA), 23, 7, 72);
            AddItem(new Static(0x0785), 23, 8, 61);
            AddItem(new Static(0x0524), 23, 8, 66);
            AddItem(new Static(0x01EA), 23, 8, 66);
            AddItem(new Static(0x01EA), 23, 8, 69);
            AddItem(new Static(0x01EA), 23, 8, 72);
            AddItem(new Static(0x0524), 23, 9, 66);
            AddItem(new Static(0x01EA), 23, 9, 66);
            AddItem(new Static(0x01EA), 23, 9, 69);
            AddItem(new Static(0x0524), 23, 10, 66);
            AddItem(new Static(0x01EA), 23, 10, 66);
            AddItem(new Static(0x01EA), 23, 10, 69);
            AddItem(new Static(0x01EA), 23, 10, 72);
            AddItem(new Static(0x0524), 23, 11, 66);
            AddItem(new Static(0x01EA), 23, 11, 66);
            AddItem(new Static(0x01EA), 23, 11, 69);
            AddItem(new Static(0x0524), 23, 12, 66);
            AddItem(new Static(0x01EA), 23, 12, 66);
            AddItem(new Static(0x01EA), 23, 12, 69);
            AddItem(new Static(0x01EA), 23, 12, 72);
            AddItem(new Static(0x0785), 23, 13, 61);
            AddItem(new Static(0x0524), 23, 13, 66);
            AddItem(new Static(0x01E8), 23, 13, 66);
            AddItem(new Static(0x01E8), 23, 13, 69);
            AddItem(new Static(0x01DA), 23, 13, 72);
            AddItem(new Static(0x1DC6), 13, 1, 6);//missing table section

            // Fence blocking prisoner
            for (int i = 0; i < 5; i++)
            {
                AddItem(new Static(0x0821), 9, 5 + i, 66);	// west side fence
                AddItem(new Static(0x0821), 15, 5 + i, 66);	// east side fence

                int randomGarbage = Utility.Random(0, 3) + 0x10EE;
                if (i > 0 && i != 4)
                {
                    for (int x = 1; x <= 4; x++)
                    {
                        if (randomGarbage < 0x10F1)
                            randomGarbage++;
                        else
                            randomGarbage = 0x10EE;

                        AddItem(new Static(randomGarbage), 10 + x, 5 + i, 66);	// east side fence
                    }

                }
            }
            for (int i = 0; i < 6; i++)
            {
                AddItem(new Static(0x0823), 10 + i, 4, 66);	// south side fence
                if (i != 2 && i != 3)
                    AddItem(new Static(0x0823), 10 + i, 9, 66);	// south side fence
            }

            // Gates to prisoner (lock them?)
            AddItem(new Static(0x0823), 12, 9, 66);	//south side fence
            //AddItem(new IronGate(DoorFacing.WestCW), 12, 9, 66);	// left door
            //AddItem(new IronGate(DoorFacing.EastCCW), 13, 9, 66);	// right door

            // pentagram
            AddItem(new Static(0xFE7), 12, 10 - 5, 66);
            AddItem(new Static(0xFE8), 13, 10 - 5, 66);
            AddItem(new Static(0xFEB), 14, 10 - 5, 66);
            AddItem(new Static(0xFE6), 12, 11 - 5, 66);
            AddItem(new Static(0xFEA), 13, 11 - 5, 66);
            AddItem(new Static(0xFEE), 14, 11 - 5, 66);
            AddItem(new Static(0xFE9), 12, 12 - 5, 66);
            AddItem(new Static(0xFEC), 13, 12 - 5, 66);
            AddItem(new Static(0xFED), 14, 12 - 5, 66);
        }

        public void AddMageTowerStatics()
        {
            /// ROOF
            // Deco on side
            for (int y = 0; y < 5; y++) // y axis
            {
                // foundation
                int startTile = 0x07CD + Utility.Random(0, 3);
                for (int x = 0; x < 3; x++) // x axis
                {
                    AddItem(new Static(0x0738), 1 + x, 8 + y, 66);
                    // flooring
                    AddItem(new Static(startTile), 1 + x, 8 + y, 71);
                    if (startTile == 0x07D0)
                        startTile = 0x07CD;
                    else
                        startTile++;
                }
                // Stairs
                AddItem(new Static(0x73A), 4, 8 + y, 66);
            }
            AddItem(new Static(0x990), 2, 8, 72); // basket
            AddItem(new Static(0x0738), 2, 9, 72); // foundation for gullotine
            AddItem(new Static(0x1245), 2, 9, 77); // gullotine
            AddItem(new Static(0x739), 2, 10, 72); // stairs for gullotine

            AddItem(new Static(0x990), 1, 12, 72); // basket
            AddItem(new Static(0x1DA0), 1, 12, 76); // head
            AddItem(new Static(0x1D9F), 2, 12, 72); // torso
            AddItem(new Static(0x1CE4), 3, 12, 72); // legs

            // blood smears
            AddItem(new Static(0x122C), 3, 8, 72);
            AddItem(new Static(0x122A), 1, 9, 72);
            AddItem(new Static(0x122D), 3, 9, 72);
            AddItem(new Static(0x122F), 2, 11, 72);

            /// LEVEL 3
            // white pentagram
            AddItem(new Static(4628), 11 - 3, 4 + 7, 26);
            AddItem(new Static(4629), 11 - 3, 3 + 7, 26);
            AddItem(new Static(4622), 11 - 3, 2 + 7, 26);
            AddItem(new Static(4627), 12 - 3, 4 + 7, 26);
            AddItem(new Static(4630), 12 - 3, 3 + 7, 26);
            AddItem(new Static(4623), 12 - 3, 2 + 7, 26);
            AddItem(new Static(4626), 13 - 3, 4 + 7, 26);
            AddItem(new Static(4625), 13 - 3, 3 + 7, 26);
            AddItem(new Static(4624), 13 - 3, 2 + 7, 26);

            // curtains/skele
            AddItem(new Static(6665), 8 + 2, -3 + 6, 26);
            AddItem(new Static(6661), 8 + 2, -5 + 6, 26);
            AddItem(new Static(6658), 12 + 2, -5 + 6, 27);
            AddItem(new Static(6659), 15 + 2, -5 + 6, 27);
            AddItem(new Static(5645), 11 + 2, 6 + 6, 26);
            AddItem(new Static(5645), 11 + 2, 4 + 6, 26);
            AddItem(new Static(5645), 11 + 2, 3 + 6, 26);
            AddItem(new Static(5645), 11 + 2, 2 + 6, 26);
            AddItem(new Static(5645), 11 + 2, 1 + 6, 26);
            AddItem(new Static(5645), 11 + 2, 0 + 6, 26);
            AddItem(new Static(5645), 11 + 2, -1 + 6, 26);
            AddItem(new Static(5645), 11 + 2, -2 + 6, 26);
            AddItem(new Static(5645), 11 + 2, -3 + 6, 26);
            AddItem(new Static(5645), 11 + 2, -4 + 6, 26);
            AddItem(new Static(5645), 11 + 2, -5 + 6, 26);
            AddItem(new Static(5646), 11 + 2, -5 + 6, 26);
            AddItem(new Static(5646), 12 + 2, -5 + 6, 26);
            AddItem(new Static(5646), 13 + 2, -5 + 6, 26);
            AddItem(new Static(5646), 14 + 2, -5 + 6, 26);
            AddItem(new Static(5646), 15 + 2, -5 + 6, 26);
            AddItem(new Static(5646), 16 + 2, -5 + 6, 26);
            AddItem(new Static(7576), 13 + 2, 0 + 6, 26);
            AddItem(new Static(7575), 13 + 2, 1 + 6, 26);
            AddItem(new Static(7420), 14 + 2, 0 + 6, 26);
            AddItem(new Static(7418), 14 + 2, 1 + 6, 26);

            // table and body.
            AddItem(new Static(4609), 14 - 9, 1 + 11, 26);
            AddItem(new Static(4611), 14 - 9, 0 + 11, 26);
            AddItem(new Static(4611), 14 - 9, -1 + 11, 26);
            AddItem(new Static(4610), 14 - 9, -2 + 11, 26);
            AddItem(new Static(7400), 14 - 9, -1 + 11, 31);
            AddItem(new Static(7399), 14 - 9, 0 + 11, 31);

            // pentagram
            AddItem(new Static(4073), 10 + 4, 2 + 2, 26);
            AddItem(new Static(4070), 10 + 4, 1 + 2, 26);
            AddItem(new Static(4071), 10 + 4, 0 + 2, 26);
            AddItem(new Static(4076), 11 + 4, 2 + 2, 26);
            AddItem(new Static(4074), 11 + 4, 1 + 2, 26);
            AddItem(new Static(4072), 11 + 4, 0 + 2, 26);
            AddItem(new Static(4077), 12 + 4, 2 + 2, 26);
            AddItem(new Static(4078), 12 + 4, 1 + 2, 26);
            AddItem(new Static(4075), 12 + 4, 0 + 2, 26);

            // Floor 2
            AddItem(new Static(7619), -6 + 11, 5 + 6, 46);
            AddItem(new Static(7619), -6 + 11, 4 + 6, 46);
            AddItem(new Static(7618), -6 + 11, 3 + 6, 46);
            AddItem(new Static(0x1034), -6 + 11, 3 + 6, 52);
            AddItem(new Static(4138), -6 + 11, 4 + 6, 52);
            AddItem(new Static(3913), -6 + 11, 5 + 6, 52);
            AddItem(new Static(7617), -6 + 11, 6 + 6, 46);
            AddItem(new Static(3791), 3 + 11, -4 + 6, 46);
            AddItem(new Static(3794), 4 + 11, -2 + 6, 46);
            AddItem(new Static(3790), 5 + 11, 4 + 6, 46);
            AddItem(new Static(3787), 3 + 11, 1 + 6, 46);
            AddItem(new Static(3793), 5 + 11, 0 + 6, 46);
            AddItem(new Static(4650), 4 + 11, -2 + 6, 46);
            AddItem(new Static(4653), 4 + 11, 2 + 6, 46);
            AddItem(new Static(4654), 3 + 11, -4 + 6, 46);
            AddItem(new Static(2557), 2 + 11, -4 + 6, 58);
            AddItem(new Static(2557), 2 + 11, -1 + 6, 58);
            AddItem(new Static(2557), 2 + 11, 2 + 6, 58);
            AddItem(new Static(3682), 4 + 11, -4 + 6, 46);
            AddItem(new Static(3679), 2 + 11, -2 + 6, 46);
            AddItem(new Static(3676), 6 + 11, -2 + 6, 46);
            AddItem(new Static(3685), 4 + 11, 0 + 6, 46);
            AddItem(new Static(3688), 2 + 11, 2 + 6, 46);
            AddItem(new Static(3688), 4 + 11, 4 + 6, 46);
            AddItem(new Static(3679), 6 + 11, 2 + 6, 46);
            AddItem(new Static(2562), 5 + 11, -5 + 6, 58);

            // Ground floor
            AddItem(new Static(7621), -1 + 11, -5 + 6, 6);
            AddItem(new Static(8045), -1 + 11, -5 + 6, 12);
            AddItem(new Static(3094), -1 + 11, -3 + 6, 6);
            AddItem(new Static(3092), -1 + 11, 0 + 6, 6);
            AddItem(new Static(4611), -6 + 11, 5 + 6, 6);
            AddItem(new Static(4611), -6 + 11, 4 + 6, 6);
            AddItem(new Static(4610), -6 + 11, 3 + 6, 6);
            AddItem(new Static(6237), -6 + 11, 4 + 6, 12);

            AddItem(new Static(0x1856), -6 + 11, 5 + 6, 12);

            AddItem(new Static(4609), -6 + 11, 6 + 6, 6);
            AddItem(new Static(7622), 0 + 11, -5 + 6, 6);
            AddItem(new Static(7622), 1 + 11, -5 + 6, 6);
            AddItem(new Static(7622), 3 + 11, -5 + 6, 6);
            AddItem(new Static(7622), 4 + 11, -5 + 6, 6);
            AddItem(new Static(7622), 5 + 11, -5 + 6, 6);
            AddItem(new Static(7620), 6 + 11, -5 + 6, 6);
            AddItem(new Static(8046), 6 + 11, -5 + 6, 12);
            AddItem(new Static(8041), 3 + 11, -5 + 6, 12);
            AddItem(new Static(7716), 0 + 11, -5 + 6, 12);
            AddItem(new Static(3629), 5 + 11, -5 + 6, 12);
            AddItem(new Static(3089), 2 + 11, -4 + 6, 6);
            AddItem(new Static(3117), 0 + 11, 0 + 6, 6);
            AddItem(new Static(3108), 5 + 11, 0 + 6, 6);
        }
        #endregion
        public virtual void AddMobile(Mobile m, int wanderRange, int xOffset, int yOffset, int zOffset)
        {
            this.m_Mobiles.Add(m);

            int zavg = this.Map.GetAverageZ(this.X + xOffset, this.Y + yOffset);
            Point3D loc = new Point3D(this.X + xOffset, this.Y + yOffset, zavg + zOffset);
            BaseCreature bc = m as BaseCreature;

            if (bc != null)
            {
                bc.RangeHome = wanderRange;
                bc.Home = loc;
            }

            if (m is BaseVendor || m is Banker)
                m.Direction = Direction.South;

            m.MoveToWorld(loc, this.Map);
        }

        public virtual void OnEnter(Mobile m)
        {
            this.RefreshDecay(true);
        }

        public virtual void OnExit(Mobile m)
        {
            this.RefreshDecay(true);
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            bool inOldRange = Utility.InRange(oldLocation, this.Location, this.EventRange);
            bool inNewRange = Utility.InRange(m.Location, this.Location, this.EventRange);

            if (inNewRange && !inOldRange)
                this.OnEnter(m);
            else if (inOldRange && !inNewRange)
                this.OnExit(m);
        }

        public override void OnAfterDelete()
        {
            base.OnAfterDelete();

            for (int i = 0; i < this.m_Items.Count; ++i)
                this.m_Items[i].Delete();

            for (int i = 0; i < this.m_Mobiles.Count; ++i)
            {
                BaseCreature bc = (BaseCreature)this.m_Mobiles[i];

                if (bc.IsPrisoner == false)
                    this.m_Mobiles[i].Delete();
                else if (this.m_Mobiles[i].CantWalk == true)
                    this.m_Mobiles[i].Delete();
            }

            this.m_Items.Clear();
            this.m_Mobiles.Clear();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version

            writer.Write(m_Prisoner);
            writer.Write((int)m_Camp);

            writer.Write(m_Items, true);
            writer.Write(m_Mobiles, true);
            writer.WriteDeltaTime(m_DecayTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    {
                        m_Prisoner = reader.ReadMobile();
                        m_Camp = (CampType)reader.ReadInt();
                        goto case 0;
                    }
                case 0:
                    {
                        m_Items = reader.ReadStrongItemList();
                        m_Mobiles = reader.ReadStrongMobileList();
                        m_DecayTime = reader.ReadDeltaTime();

                        RefreshDecay(false);

                        break;
                    }
            }
        }

        public static void OnTick()
        {
            List<BaseCamp> list = new List<BaseCamp>(_Camps);

            list.ForEach(c =>
            {
                if (!c.Deleted && c.Map != null && c.Map != Map.Internal && !c.RestrictDecay)
                    c.CheckDecay();
            });

            ColUtility.Free(list);
        }
    }

    public class LockableBarrel : LockableContainer
    {
        [Constructable]
        public LockableBarrel()
            : base(0xE77)
        {
            this.Weight = 1.0;
        }

        public LockableBarrel(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (this.Weight == 8.0)
                this.Weight = 1.0;
        }
    }
}
