using Server.Mobiles;
using Server.Network;
using System;
using System.Collections;
using Server.Items;
using Server.Gumps;

namespace Server.Items
{
    public class OutpostBedroll : Item
    {
        public static readonly int SecureRange = 6;
        private static readonly Hashtable m_Table = new Hashtable();
        private readonly Timer m_Timer;
        private readonly DateTime m_Created;
        private readonly ArrayList m_Entries;

 	public bool IsUpgraded = false;

        [Constructable]
        public OutpostBedroll()
            : base(0xA57)
        {
            Movable = false;

            m_Entries = new ArrayList();

            m_Created = DateTime.UtcNow;
            m_Timer = Timer.DelayCall(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0), OnTick);
        }

        public OutpostBedroll(Serial serial)
            : base(serial)
        {
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime Created => m_Created;

        public static OutpostEntry GetEntry(Mobile player)
        {
            return (OutpostEntry)m_Table[player];
        }

        public static void RemoveEntry(OutpostEntry entry)
        {
            m_Table.Remove(entry.Player);
            entry.Bed.m_Entries.Remove(entry);
        }

        public override void OnAfterDelete()
        {
            if (m_Timer != null)
                m_Timer.Stop();

            ClearEntries();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            Delete();
        }

        private void OnTick()
        {

            DateTime now = DateTime.UtcNow;
            TimeSpan age = now - Created;

            if (Deleted)
                return;

            foreach (OutpostEntry entry in new ArrayList(m_Entries))
            {

                if (!entry.Valid || entry.Player.NetState == null)
                {
                    RemoveEntry(entry);
                }
                else if (!entry.Safe && now - entry.Start >= TimeSpan.FromSeconds(5.0)) //originally was: TimeSpan.FromSeconds(30.0)
                {
                    entry.Safe = true;
                    //entry.Player.SendLocalizedMessage(500621); // The camp is now secure.

		    if(!entry.Player.BedrollLogout)
		    {
		    	entry.Player.BedrollLogout = true;
		    	entry.Player.SendMessage("This outpost feels like a safe place to rest.");
		    }

                }

            }

            IPooledEnumerable eable = GetClientsInRange(SecureRange);

            foreach (NetState state in eable)
            {
                PlayerMobile pm = state.Mobile as PlayerMobile;

                if (pm != null && GetEntry(pm) == null)
                {
                    OutpostEntry entry = new OutpostEntry(pm, this);

                    m_Table[pm] = entry;
                    m_Entries.Add(entry);

		    if(!pm.BedrollLogout)
                    	pm.SendMessage("It will take a few moment before you feel secure within the outpost."); // You feel it would take a few moments to secure your camp.
                }
            }

            eable.Free();
        }

        private void ClearEntries()
        {
            if (m_Entries == null)
                return;

            foreach (OutpostEntry entry in new ArrayList(m_Entries))
            {
                RemoveEntry(entry);
            }
        }
    }

    public class OutpostEntry
    {
        private readonly PlayerMobile m_Player;
        private readonly OutpostBedroll m_Bed;
        private readonly DateTime m_Start;
        private bool m_Safe;
        public OutpostEntry(PlayerMobile player, OutpostBedroll bed)
        {
            m_Player = player;
            m_Bed = bed;
            m_Start = DateTime.UtcNow;
            m_Safe = false;
        }

        public PlayerMobile Player => m_Player;
        public OutpostBedroll Bed => m_Bed;
        public DateTime Start => m_Start;
        public bool Valid => !Bed.Deleted && Player.Map == Bed.Map && Player.InRange(Bed, OutpostBedroll.SecureRange);
	//public bool IsBuffed;
        public bool Safe
        {
            get
            {
                return Valid && m_Safe;
            }
            set
            {
                m_Safe = value;
            }
        }
    }
}