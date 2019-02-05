using System;
using System.Collections.Generic;
using Server.Network;
#region BBS Quests
using System.Collections;
using System.Text;
using Server;
using Server.Mobiles;
using System.IO;
using Server.Multis;
#endregion

namespace Server.Items
{
    #region BBS Quests
    public class PrisonerMessage : BulletinMessage
    {
        private BaseCamp m_Camp;

        public BaseCamp Camp { get { return m_Camp; } }

        public PrisonerMessage(BaseCamp c, BaseEscortable escort)
            : base(c.Prisoner, null, "", null)
        {
            m_Camp = c;

            switch (Utility.Random(13))
            {
                case 0: Subject = "A kidnapping!"; break;
                case 1: Subject = "Help!"; break;
                case 2: Subject = "Help us, please!"; break;
                case 3: Subject = "Adventurers needed!"; break;
                case 4: Subject = "Seeking assistance"; break;
                case 5: Subject = "In need of aid"; break;
                case 6: Subject = "Canst thou help us?"; break;
                case 7: Subject = "Shall any save our friend?"; break;
                case 8: Subject = "A friend was kidnapped!"; break;
                case 9: Subject = "Heroes wanted!"; break;
                case 10: Subject = "Can any assist us?"; break;
                case 11: Subject = "Kidnapped!"; break;
                case 12: Subject = "Taken prisoner"; break;
            }

            double distance;
            BulletinBoard board = FindClosestBB(c.Prisoner, out distance);

            List<String> myLines = new List<String>();
            string[] subtext1 = { "foul", "vile", "evil", "dark", "cruel", "vicious", "scoundrelly", "dastardly", "cowardly", "craven", "foul and monstrous", "monstrous", "hideous", "terrible", "cruel, evil", "truly vile", "vicious and cunning", "" };

            string camp;

            switch (c.Camp)
            {
                default:
                case CampType.Default: camp = ""; break;
                //case CampType.EvilMage: camp = "evil mages"; break;
                //case CampType.GoodMage: camp = "mages"; break;
                case CampType.Lizardman: camp = "lizardmen"; break;
                case CampType.Orc: camp = "orcs"; break;
                case CampType.Ratman: camp = "ratmen"; break;
                case CampType.Brigand: camp = "brigands"; break;
                //case CampType.Gypsy: camp = "gypsys"; break;
                //case CampType.Warlord: camp = "a warlord"; break;
            }

            myLines.Add(String.Format("Help us please! {0} hath", escort.Name));
            myLines.Add(String.Format("been kidnapped by "));
            myLines.Add(String.Format("{0} {1}!", subtext1[Utility.Random(subtext1.Length)], camp));
            myLines.Add(String.Format("We believe that {0} is held at", escort.Female ? "she" : "he"));

            int xLong = 0, yLat = 0;
            int xMins = 0, yMins = 0;
            bool xEast = false, ySouth = false;

            if (Sextant.Format(c.Location, c.Map, ref xLong, ref yLat, ref xMins, ref yMins, ref xEast, ref ySouth))
            {
                myLines.Add(String.Format("{0}o {1}'{2}, {3}o {4}'{5}", yLat, yMins, ySouth ? "S" : "N", xLong, xMins, xEast ? "E" : "W"));
            }
            /*myLines.Add(String.Format("{0}"));
            myLines.Add(String.Format("{0}."));
            myLines.Add(String.Format("{0}"));*/

            Lines = myLines.ToArray();

            if (board != null)
                board.AddItem(this);
            else
                Delete();
        }

        public PrisonerMessage(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version

            writer.Write(m_Camp);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    m_Camp = reader.ReadItem() as BaseCamp;
                    break;
            }
        }
    }

    public class EscortMessage : BulletinMessage
    {
        private Mobile m_Mobile;

        public Mobile Escort { get { return m_Mobile; } }

        public EscortMessage(Mobile m)
            : base(m, null, "", null)
        {
            m_Mobile = m;

            BaseEscortable escort = m as BaseEscortable;
            String subtext = "";

            switch (m.GetType().Name)
            {
                case "EscortableHealer1": subtext = "Greetings! I am a poor healer who seeks a worthy escort. I can offer some small pay to any doughty warrior able to assist me. It is imperative that I reach my destination, or innocents may perish!"; break;
                case "Peasant": subtext = "'Tis a terrible thing to be a parent with an ungrateful child! Yet such is my situation. Because of the poor behavior and lack of character of this offspring of mine, I am obliged to foster them away from home. So now I am in need of an able escort of good character who might serve as role model, and who can ensure that my child reaches their destination safely. I shall let my child post their whereabouts so that thou mayst meet with them and arrange terms." + "---"; break;
                case "BrideGroom": subtext = "I am so happy! I am to be married, and my life will finally be complete! Alas, I am no warrior, and the wedding is not to take place here. I am in need of an escort, for the roads are treacherous and my future spouse would be sad indeed to hear that an ettin ate me before the wedding."; break;
                case "EscortableMage1": subtext = "Wizard seeks escort to a conference."; break;
                case "Merchant": subtext = "Reputable merchant seeks able warriors to serve as mercantile escort. Pay is scale; we prefer to hire experienced mercenaries."; break;
                case "Messenger": subtext = "I am one of Lord British's couriers, and I seek an able warrior to escort me safely, as the message I carry is of utmost importance to the realm!"; break;
                case "Noble": subtext = "'Tis a bit of a problem to admit it, but our normally trustworthy household guard seem to have broken his leg! If thou art able with a weapon, we are pleased to take applications for his replacement, to serve as guard and escort on our forthcoming journey."; break;
                default:
                case "SeekerOfAdventure": subtext = "I've always wished for adventure! Now I can have it at last! My weaponsmaster in school always said I was a dab hand with a blade, and I am afire with the love of adventure! Plus I have money. So if you are willing to hire on as my bodyguard and join me as we seek the deepest depths of the Abyss, and as we conquer dragons with the rapid flick of our sharp swords, disregarding all danger and ignorant of fear, seek me out! Cowards need not apply!"; break;
            }

            switch (Utility.Random(9))
            {
                case 0: Subject = "Escort needed"; break;
                case 1: Subject = "Guard needed"; break;
                case 2: Subject = "I need an escort!"; break;
                case 3: Subject = "Traveling companion?"; break;
                case 4: Subject = "Seeking companion"; break;
                case 5: Subject = "Now hiring"; break;
                case 6: Subject = "Hiring a guard"; break;
                case 7: Subject = "Hiring an escort"; break;
                case 8: Subject = "Seeking escort"; break;
            }

            double distance;
            BulletinBoard board = FindClosestBB(m, out distance);

            String direction;

            if (m.GetDirectionTo(board) == Direction.North)
                direction = "South";//North
            else if (m.GetDirectionTo(board) == Direction.South)
                direction = "North";//South
            else if (m.GetDirectionTo(board) == Direction.East)
                direction = "West";//East
            else if (m.GetDirectionTo(board) == Direction.West)
                direction = "East";//West
            else if (m.GetDirectionTo(board) == Direction.Up)
                direction = "Southeast";//Northwest
            else if (m.GetDirectionTo(board) == Direction.Left)
                direction = "Northeast";//Southwest
            else if (m.GetDirectionTo(board) == Direction.Right)
                direction = "Southwest";//Northeast
            else if (m.GetDirectionTo(board) == Direction.Down)
                direction = "Northwest";//Southeast
            else
                direction = "in some direction.";

            String line = String.Format("{0} I can be found {1} {2} of here. When thou dost find me, look at me close to accept the task of taking me to {3}. {4}", subtext, (distance < 50 ? "a fair distance" : "a long way"), direction, escort.Destination, escort.Name);
            Lines = MakeLines(line);

            if (board != null)
                board.AddItem(this);
            else
                Delete();
            //board.AddItem(this);
        }

        public EscortMessage(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version

            writer.Write(m_Mobile);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    m_Mobile = reader.ReadMobile();
                    break;
            }
        }
    }
    #endregion
    public struct BulletinEquip
    {
        public int itemID;
        public int hue;
        public BulletinEquip(int itemID, int hue)
        {
            this.itemID = itemID;
            this.hue = hue;
        }
    }

    [Flipable(0x1E5E, 0x1E5F)]
    public class BulletinBoard : BaseBulletinBoard
    {
        [Constructable]
        public BulletinBoard()
            : base(0x1E5E)
        {
        }

        public BulletinBoard(Serial serial)
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
        }
    }

    public abstract class BaseBulletinBoard : Item
    {
        // Threads will be removed six hours after the last post was made
        private static readonly TimeSpan ThreadDeletionTime = TimeSpan.FromHours(6.0);
        // A player may only create a thread once every two minutes
        private static readonly TimeSpan ThreadCreateTime = TimeSpan.FromMinutes(2.0);
        // A player may only reply once every thirty seconds
        private static readonly TimeSpan ThreadReplyTime = TimeSpan.FromSeconds(30.0);
        private string m_BoardName;

      /*  #region BBS Quests
        private static BulletinBoard m_MasterBoard;
        public static BulletinBoard MasterBoard
        {
            get
            {
                if (m_MasterBoard == null)
                    m_MasterBoard = new BulletinBoard();

                return m_MasterBoard;
            }
        }
        #endregion*/

        public BaseBulletinBoard(int itemID)
            : base(itemID)
        {
            this.m_BoardName = "bulletin board";
            this.Movable = false;
        }

        public BaseBulletinBoard(Serial serial)
            : base(serial)
        {
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public string BoardName
        {
            get
            {
                return this.m_BoardName;
            }
            set
            {
                this.m_BoardName = value;
            }
        }
        public static bool CheckTime(DateTime time, TimeSpan range)
        {
            return (time + range) < DateTime.UtcNow;
        }

        public static string FormatTS(TimeSpan ts)
        {
            int totalSeconds = (int)ts.TotalSeconds;
            int seconds = totalSeconds % 60;
            int minutes = totalSeconds / 60;

            if (minutes != 0 && seconds != 0)
                return String.Format("{0} minute{1} and {2} second{3}", minutes, minutes == 1 ? "" : "s", seconds, seconds == 1 ? "" : "s");
            else if (minutes != 0)
                return String.Format("{0} minute{1}", minutes, minutes == 1 ? "" : "s");
            else
                return String.Format("{0} second{1}", seconds, seconds == 1 ? "" : "s");
        }

        #region BBS Quests
         public override void OnSingleClick(Mobile from)
        {
            from.Send(new AsciiMessage(this.Serial, this.ItemID, MessageType.Label, 0, 3, "", "a bulletin board"));
        }
        #endregion
        public static void Initialize()
        {
            PacketHandlers.Register(0x71, 0, true, new OnPacketReceive(BBClientRequest));
        }

        public static void BBClientRequest(NetState state, PacketReader pvSrc)
        {
            Mobile from = state.Mobile;

            int packetID = pvSrc.ReadByte();
            BaseBulletinBoard board = World.FindItem(pvSrc.ReadInt32()) as BaseBulletinBoard;

            if (board == null || !board.CheckRange(from))
                return;

            switch ( packetID )
            {
                case 3:
                    BBRequestContent(from, board, pvSrc);
                    break;
                case 4:
                    BBRequestHeader(from, board, pvSrc);
                    break;
                case 5:
                    BBPostMessage(from, board, pvSrc);
                    break;
                case 6:
                    BBRemoveMessage(from, board, pvSrc);
                    break;
            }
        }

        public static void BBRequestContent(Mobile from, BaseBulletinBoard board, PacketReader pvSrc)
        {
            BulletinMessage msg = World.FindItem(pvSrc.ReadInt32()) as BulletinMessage;

            if (msg == null || msg.Parent != board)
                return;

            from.Send(new BBMessageContent(board, msg));
        }

        public static void BBRequestHeader(Mobile from, BaseBulletinBoard board, PacketReader pvSrc)
        {
            BulletinMessage msg = World.FindItem(pvSrc.ReadInt32()) as BulletinMessage;

            if (msg == null || msg.Parent != board)
                return;

            from.Send(new BBMessageHeader(board, msg));
        }

        public static void BBPostMessage(Mobile from, BaseBulletinBoard board, PacketReader pvSrc)
        {
            BulletinMessage thread = World.FindItem(pvSrc.ReadInt32()) as BulletinMessage;

            if (thread != null && thread.Parent != board)
                thread = null;

            int breakout = 0;

            while (thread != null && thread.Thread != null && breakout++ < 10)
                thread = thread.Thread;

            DateTime lastPostTime = DateTime.MinValue;

            if (board.GetLastPostTime(from, (thread == null), ref lastPostTime))
            {
                if (!CheckTime(lastPostTime, (thread == null ? ThreadCreateTime : ThreadReplyTime)))
                {
                    if (thread == null)
                        from.SendMessage("You must wait {0} before creating a new thread.", FormatTS(ThreadCreateTime));
                    else
                        from.SendMessage("You must wait {0} before replying to another thread.", FormatTS(ThreadReplyTime));

                    return;
                }
            }

            string subject = pvSrc.ReadUTF8StringSafe(pvSrc.ReadByte());

            if (subject.Length == 0)
                return;

            string[] lines = new string[pvSrc.ReadByte()];

            if (lines.Length == 0)
                return;

            for (int i = 0; i < lines.Length; ++i)
                lines[i] = pvSrc.ReadUTF8StringSafe(pvSrc.ReadByte());

            board.PostMessage(from, thread, subject, lines);
        }

        public static void BBRemoveMessage(Mobile from, BaseBulletinBoard board, PacketReader pvSrc)
        {
            BulletinMessage msg = World.FindItem(pvSrc.ReadInt32()) as BulletinMessage;

            if (msg == null || msg.Parent != board)
                return;

            if (from.AccessLevel < AccessLevel.GameMaster && msg.Poster != from)
                return;

            msg.Delete();
        }

        public virtual void Cleanup()
        {
            List<Item> items = this.Items;

            for (int i = items.Count - 1; i >= 0; --i)
            {
                if (i >= items.Count)
                    continue;

                BulletinMessage msg = items[i] as BulletinMessage;

                if (msg == null)
                    continue;
                #region BBS Quests
                 //Stop escort messages from being deleted
                if (msg is EscortMessage)
                    continue;
                
                //Stop prisoner messages from being deleted
                if (msg is PrisonerMessage)
                    continue;
                #endregion
                if (msg.Thread == null && CheckTime(msg.LastPostTime, ThreadDeletionTime))
                {
                    msg.Delete();
                    this.RecurseDelete(msg); // A root-level thread has expired
                }
            }
        }

        public virtual bool GetLastPostTime(Mobile poster, bool onlyCheckRoot, ref DateTime lastPostTime)
        {
            List<Item> items = this.Items;
            bool wasSet = false;

            for (int i = 0; i < items.Count; ++i)
            {
                BulletinMessage msg = items[i] as BulletinMessage;

                if (msg == null || msg.Poster != poster)
                    continue;

                if (onlyCheckRoot && msg.Thread != null)
                    continue;

                if (msg.Time > lastPostTime)
                {
                    wasSet = true;
                    lastPostTime = msg.Time;
                }
            }

            return wasSet;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (this.CheckRange(from))
            {
                this.Cleanup();

                NetState state = from.NetState;

                state.Send(new BBDisplayBoard(this));
                if (state.ContainerGridLines)
                    state.Send(new ContainerContent6017(from, this));
                else
                    state.Send(new ContainerContent(from, this));
            }
            else
            {
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
            }
        }
    /*    #region BBS Quests
          private class BulletinComparer : IComparer<Item>
        {
            public int Compare(Item x, Item y)
            {
                if (((BulletinMessage)x) == null && ((BulletinMessage)y) == null)
                    return 0;
                else if (((BulletinMessage)x) == null)
                    return -1;
                else if (((BulletinMessage)y) == null)
                    return 1;

                return ((BulletinMessage)x).LastPostTime.CompareTo(((BulletinMessage)y).LastPostTime);
            }
        }

        private sealed class BBContainerContent6017 : Packet
        {
            public BBContainerContent6017(Mobile beholder, Item beheld)
                : base(0x3C)
            {
                List<Item> items = new List<Item>();

                items.InsertRange(0, MasterBoard.Items);
                items.InsertRange(0, beheld.Items);

                items.Sort(new BulletinComparer());

                int count = items.Count;

                this.EnsureCapacity(5 + (count * 20));

                long pos = m_Stream.Position;

                int written = 0;

                m_Stream.Write((ushort)0);

                for (int i = 0; i < count; ++i)
                {
                    Item child = items[i];

                    if (!child.Deleted)
                    {
                        Point3D loc = child.Location;

                        m_Stream.Write((int)child.Serial);
                        m_Stream.Write((ushort)child.ItemID);
                        m_Stream.Write((byte)0); // signed, itemID offset
                        m_Stream.Write((ushort)child.Amount);
                        m_Stream.Write((short)loc.X);
                        m_Stream.Write((short)loc.Y);
                        m_Stream.Write((byte)0); // Grid Location?
                        m_Stream.Write((int)beheld.Serial);
                        m_Stream.Write((ushort)child.Hue);

                        ++written;
                    }
                }

                m_Stream.Seek(pos, SeekOrigin.Begin);
                m_Stream.Write((ushort)written);
            }
        }

        private sealed class BBContainerContent : Packet
        {
            public BBContainerContent(Mobile beholder, Item beheld)
                : base(0x3C)
            {
                List<Item> items = new List<Item>();

                items.InsertRange(0, MasterBoard.Items);
                items.InsertRange(0, beheld.Items);

                items.Sort(new BulletinComparer());

                int count = items.Count;

                this.EnsureCapacity(5 + (count * 19));

                long pos = m_Stream.Position;

                int written = 0;

                m_Stream.Write((ushort)0);

                for (int i = 0; i < count; ++i)
                {
                    Item child = items[i];

                    if (!child.Deleted)
                    {
                        Point3D loc = child.Location;

                        m_Stream.Write((int)child.Serial);
                        m_Stream.Write((ushort)child.ItemID);
                        m_Stream.Write((byte)0); // signed, itemID offset
                        m_Stream.Write((ushort)child.Amount);
                        m_Stream.Write((short)loc.X);
                        m_Stream.Write((short)loc.Y);
                        m_Stream.Write((int)beheld.Serial);
                        m_Stream.Write((ushort)child.Hue);

                        ++written;
                    }
                }

                m_Stream.Seek(pos, SeekOrigin.Begin);
                m_Stream.Write((ushort)written);
            }
        }
        #endregion */
        public virtual bool CheckRange(Mobile from)
        {
            if (from.AccessLevel >= AccessLevel.GameMaster)
                return true;

            return (from.Map == this.Map && from.InRange(this.GetWorldLocation(), 2));
        }

        public void PostMessage(Mobile from, BulletinMessage thread, string subject, string[] lines)
        {
            if (thread != null)
                thread.LastPostTime = DateTime.UtcNow;

            this.AddItem(new BulletinMessage(from, thread, subject, lines));
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version

            writer.Write((string)this.m_BoardName);
            #region BBS Quest
           // writer.Write(m_MasterBoard);
            #endregion
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch ( version )
            {
                case 0:
                    {
                        this.m_BoardName = reader.ReadString();
                        //this.m_MasterBoard = (BulletinBoard)reader.ReadItem();
                        break;
                    }
            }
        }

        private void RecurseDelete(BulletinMessage msg)
        {
            List<Item> found = new List<Item>();
            List<Item> items = this.Items;

            for (int i = items.Count - 1; i >= 0; --i)
            {
                if (i >= items.Count)
                    continue;

                BulletinMessage check = items[i] as BulletinMessage;

                if (check == null)
                    continue;

                if (check.Thread == msg)
                {
                    check.Delete();
                    found.Add(check);
                }
            }

            for (int i = 0; i < found.Count; ++i)
                this.RecurseDelete((BulletinMessage)found[i]);
        }
    }

    public class BulletinMessage : Item
    {
        private Mobile m_Poster;
        private string m_Subject;
        private DateTime m_Time, m_LastPostTime;
        private BulletinMessage m_Thread;
        private string m_PostedName;
        private int m_PostedBody;
        private int m_PostedHue;
        private BulletinEquip[] m_PostedEquip;
        private string[] m_Lines;
        public BulletinMessage(Mobile poster, BulletinMessage thread, string subject, string[] lines)
            : base(0xEB0)
        {
            this.Movable = false;

            this.m_Poster = poster;
            this.m_Subject = subject;
            this.m_Time = DateTime.UtcNow;
            this.m_LastPostTime = this.m_Time;
            this.m_Thread = thread;
            this.m_PostedName = this.m_Poster.Name;
            this.m_PostedBody = this.m_Poster.Body;
            this.m_PostedHue = this.m_Poster.Hue;
            this.m_Lines = lines;

            List<BulletinEquip> list = new List<BulletinEquip>();

            for (int i = 0; i < poster.Items.Count; ++i)
            {
                Item item = poster.Items[i];

                if (item.Layer >= Layer.OneHanded && item.Layer <= Layer.Mount)
                    list.Add(new BulletinEquip(item.ItemID, item.Hue));
            }

            this.m_PostedEquip = list.ToArray();
        }

        public BulletinMessage(Serial serial)
            : base(serial)
        {
        }

        public Mobile Poster
        {
            get
            {
                return this.m_Poster;
            }
        }
        public BulletinMessage Thread
        {
            get
            {
                return this.m_Thread;
            }
        }
        #region BBS Quest
        public string Subject
        {
            get { return m_Subject; }
            set { m_Subject = value; }
        }
        #endregion
        public DateTime Time
        {
            get
            {
                return this.m_Time;
            }
        }
        public DateTime LastPostTime
        {
            get
            {
                return this.m_LastPostTime;
            }
            set
            {
                this.m_LastPostTime = value;
            }
        }
        public string PostedName
        {
            get
            {
                return this.m_PostedName;
            }
        }
        public int PostedBody
        {
            get
            {
                return this.m_PostedBody;
            }
        }
        public int PostedHue
        {
            get
            {
                return this.m_PostedHue;
            }
        }
        public BulletinEquip[] PostedEquip
        {
            get
            {
                return this.m_PostedEquip;
            }
        }
        #region BBS Quests
        public string[] Lines
        {
            get { return m_Lines; }
            set { m_Lines = value; }
        }
       
        #endregion
        public string GetTimeAsString()
        {
            return this.m_Time.ToString("MMM dd, yyyy");
        }

        public override bool CheckTarget(Mobile from, Server.Targeting.Target targ, object targeted)
        {
            return false;
        }

        public override bool IsAccessibleTo(Mobile check)
        {
            return false;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version

            writer.Write((Mobile)this.m_Poster);
            writer.Write((string)this.m_Subject);
            writer.Write((DateTime)this.m_Time);
            writer.Write((DateTime)this.m_LastPostTime);
            writer.Write((bool)(this.m_Thread != null));
            writer.Write((Item)this.m_Thread);
            writer.Write((string)this.m_PostedName);
            writer.Write((int)this.m_PostedBody);
            writer.Write((int)this.m_PostedHue);

            writer.Write((int)this.m_PostedEquip.Length);

            for (int i = 0; i < this.m_PostedEquip.Length; ++i)
            {
                writer.Write((int)this.m_PostedEquip[i].itemID);
                writer.Write((int)this.m_PostedEquip[i].hue);
            }

            writer.Write((int)this.m_Lines.Length);

            for (int i = 0; i < this.m_Lines.Length; ++i)
                writer.Write((string)this.m_Lines[i]);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch ( version )
            {
                case 1:
                case 0:
                    {
                        this.m_Poster = reader.ReadMobile();
                        this.m_Subject = reader.ReadString();
                        this.m_Time = reader.ReadDateTime();
                        this.m_LastPostTime = reader.ReadDateTime();
                        bool hasThread = reader.ReadBool();
                        this.m_Thread = reader.ReadItem() as BulletinMessage;
                        this.m_PostedName = reader.ReadString();
                        this.m_PostedBody = reader.ReadInt();
                        this.m_PostedHue = reader.ReadInt();

                        this.m_PostedEquip = new BulletinEquip[reader.ReadInt()];

                        for (int i = 0; i < this.m_PostedEquip.Length; ++i)
                        {
                            this.m_PostedEquip[i].itemID = reader.ReadInt();
                            this.m_PostedEquip[i].hue = reader.ReadInt();
                        }

                        this.m_Lines = new string[reader.ReadInt()];

                        for (int i = 0; i < this.m_Lines.Length; ++i)
                            this.m_Lines[i] = reader.ReadString();

                        if (hasThread && this.m_Thread == null)
                            this.Delete();

                        if (version == 0)
                            ValidationQueue<BulletinMessage>.Add(this);

                        break;
                    }
            }
        }

        public void Validate()
        {
            if (!(this.Parent is BulletinBoard && ((BulletinBoard)this.Parent).Items.Contains(this)))
                this.Delete();
        }
    
    #region BBS Quests
        public static BulletinBoard FindClosestBB(Mobile m, out double distance)
        {
            BulletinBoard closest = null;
            distance = double.MaxValue;
            double tempdist = 0;

            foreach (Item item in World.Items.Values)
            {
                if (item is BulletinBoard && item.Map == m.Map) //
                {
                    tempdist = m.GetDistanceToSqrt(item.Location);
                    if (tempdist < distance)
                    {
                        closest = item as BulletinBoard;
                        distance = tempdist;
                    }
                }
            }

            return closest;
        }

        public static string[] MakeLines(String text)
        {
            int current = 0;
            int lineCount = 25;
            List<String> linesList = new List<string>();

            string[] lines = new string[lineCount];
            char space = ' ';

            // break up the text into single line length pieces
            while (text != null && current < text.Length)
            {
                // make each line 25 chars long
                int length = text.Length - current;

                if (length > 25)
                {
                    length = 25;

                    while (text[current + length] != space)
                        length--;

                    length++;
                    linesList.Add(text.Substring(current, length));
                }
                else
                {
                    linesList.Add(String.Format("{0} ", text.Substring(current, length)));
                }

                current += length;
            }

            return linesList.ToArray();
        }
    }
    #endregion
    public class BBDisplayBoard : Packet
    {
        public BBDisplayBoard(BaseBulletinBoard board)
            : base(0x71)
        {
            string name = board.BoardName;

            if (name == null)
                name = "";

            this.EnsureCapacity(38);

            byte[] buffer = Utility.UTF8.GetBytes(name);

            this.m_Stream.Write((byte)0x00); // PacketID
            this.m_Stream.Write((int)board.Serial); // Bulletin board serial

            // Bulletin board name
            if (buffer.Length >= 29)
            {
                this.m_Stream.Write(buffer, 0, 29);
                this.m_Stream.Write((byte)0);
            }
            else
            {
                this.m_Stream.Write(buffer, 0, buffer.Length);
                this.m_Stream.Fill(30 - buffer.Length);
            }
        }
    }

    public class BBMessageHeader : Packet
    {
        public BBMessageHeader(BaseBulletinBoard board, BulletinMessage msg)
            : base(0x71)
        {
            string poster = this.SafeString(msg.PostedName);
            string subject = this.SafeString(msg.Subject);
            string time = this.SafeString(msg.GetTimeAsString());

            this.EnsureCapacity(22 + poster.Length + subject.Length + time.Length);

            this.m_Stream.Write((byte)0x01); // PacketID
            this.m_Stream.Write((int)board.Serial); // Bulletin board serial
            this.m_Stream.Write((int)msg.Serial); // Message serial

            BulletinMessage thread = msg.Thread;

            if (thread == null)
                this.m_Stream.Write((int)0); // Thread serial--root
            else
                this.m_Stream.Write((int)thread.Serial); // Thread serial--parent

            this.WriteString(poster);
            this.WriteString(subject);
            this.WriteString(time);
        }

        public void WriteString(string v)
        {
            byte[] buffer = Utility.UTF8.GetBytes(v);
            int len = buffer.Length + 1;

            if (len > 255)
                len = 255;

            this.m_Stream.Write((byte)len);
            this.m_Stream.Write(buffer, 0, len - 1);
            this.m_Stream.Write((byte)0);
        }

        public string SafeString(string v)
        {
            if (v == null)
                return String.Empty;

            return v;
        }
    }

    public class BBMessageContent : Packet
    {
        public BBMessageContent(BaseBulletinBoard board, BulletinMessage msg)
            : base(0x71)
        {
            string poster = this.SafeString(msg.PostedName);
            string subject = this.SafeString(msg.Subject);
            string time = this.SafeString(msg.GetTimeAsString());

            this.EnsureCapacity(22 + poster.Length + subject.Length + time.Length);

            this.m_Stream.Write((byte)0x02); // PacketID
            this.m_Stream.Write((int)board.Serial); // Bulletin board serial
            this.m_Stream.Write((int)msg.Serial); // Message serial

            this.WriteString(poster);
            this.WriteString(subject);
            this.WriteString(time);

            this.m_Stream.Write((short)msg.PostedBody);
            this.m_Stream.Write((short)msg.PostedHue);

            int len = msg.PostedEquip.Length;

            if (len > 255)
                len = 255;

            this.m_Stream.Write((byte)len);

            for (int i = 0; i < len; ++i)
            {
                BulletinEquip eq = msg.PostedEquip[i];

                this.m_Stream.Write((short)eq.itemID);
                this.m_Stream.Write((short)eq.hue);
            }

            len = msg.Lines.Length;

            if (len > 255)
                len = 255;

            this.m_Stream.Write((byte)len);

            for (int i = 0; i < len; ++i)
                this.WriteString(msg.Lines[i], true);
        }

        public void WriteString(string v)
        {
            this.WriteString(v, false);
        }

        public void WriteString(string v, bool padding)
        {
            byte[] buffer = Utility.UTF8.GetBytes(v);
            int tail = padding ? 2 : 1;
            int len = buffer.Length + tail;

            if (len > 255)
                len = 255;

            this.m_Stream.Write((byte)len);
            this.m_Stream.Write(buffer, 0, len - tail);

            if (padding)
                this.m_Stream.Write((short)0); // padding compensates for a client bug
            else
                this.m_Stream.Write((byte)0);
        }

        public string SafeString(string v)
        {
            if (v == null)
                return String.Empty;

            return v;
        }
    }
}