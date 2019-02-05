using System;
using System.Collections;
using System.Collections.Generic;

namespace Server.Items
{
    /// <summary>
    /// UOSI Modified portion of the ship transit system. The Ship Ticket is direct to a single point.
    /// Each teleporter is configured in game to point to the desired location. Each teleporter can figure out which ticket is held, and send the player to
    /// the right destination. UPDATE - the ticket is now dblclicked and sends the player on its own so as to prevent missending other players.
    /// </summary>
    public class ShipTicketTele : Teleporter
    {
        //We're going to specify the Point3d to teleport the player to based on the ticket held.
        private Point3D m_DestMonitor;
        private Point3D m_DestFawn;
        private Point3D m_DestSleepingBull;
        private Point3D m_DestMoonshade;

        private int m_CurrentLocation;

        private int dest;

        [CommandProperty(AccessLevel.Seer)]
        public Point3D DestMonitor
        {
            get { return m_DestMonitor; }
            set
            {
                m_DestMonitor = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.Seer)]
        public Point3D DestFawn
        {
            get { return m_DestFawn; }
            set
            {
                m_DestFawn = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.Seer)]
        public Point3D DestSleepingBull
        {
            get { return m_DestSleepingBull; }
            set
            {
                m_DestSleepingBull = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.Seer)]
        public Point3D DestMoonshade
        {
            get { return m_DestMoonshade; }
            set
            {
                m_DestMoonshade = value;
                InvalidateProperties();
            }
        }

        /// <summary>
        /// CurrentLocation follows the same numerical pattern everywhere.
        /// 0-Monitor || 1-Fawn || 2-Sleeping Bull || 3-Moonshade
        /// </summary>
        [CommandProperty(AccessLevel.Seer)]
        public int CurrentLocation
        {
            get { return m_CurrentLocation; }
            set
            {
                m_CurrentLocation = value;
                InvalidateProperties();
            }
        }

        [Constructable]
        public ShipTicketTele()
            : base()
        {
            //Hardcoded but can still be changed in game just to be safe
            m_DestMonitor = new Point3D(360, 1501, 2);
            m_DestFawn = new Point3D(334, 935, 0);
            m_DestSleepingBull = new Point3D(603, 1198, 2);
            m_DestMoonshade = new Point3D(930, 1230, 2);
        }

        public ShipTicketTele(Serial serial)
            : base(serial)
        {
        }

        public static List<ShipTicket> GetTeleporterTicket(Mobile m)
        {
            List<ShipTicket> tickets = new List<ShipTicket>();

            for (int i = 0; i < m.Items.Count; i ++)
            {
                if (m.Items[i] is ShipTicket)
                    tickets.Add(m.Items[i] as ShipTicket);
            }

            if(tickets.Count > 0)
                return tickets;

            if (m.Backpack != null)
                return m.Backpack.FindItemsByType<ShipTicket>();

            //return m.Backpack.FindItemByType(typeof(ShipTicket), true) as ShipTicket;

            return null;
        }

    //    public override bool OnMoveOver(Mobile m)
    //    {
    //        List<ShipTicket> tickets = GetTeleporterTicket(m);
			
    //        if (tickets.Count > 0)
    //        {
    //            //Change this to display a gump of choices between the available tickets you have
    //            if (tickets.Count > 1)
    //            {
    //                switch (tickets[0].Name)
    //                {
    //                    case "Ship Ticket to Monitor":
    //                        {
    //                            if (m_CurrentLocation != 0)
    //                            {
    //                                tickets[0].Delete();
    //                                m.SendMessage("Thy ticket is taken as thou boardest the ship.");
    //                                PointDest = m_DestMonitor;
    //                                break;
    //                            }
    //                            else
    //                            {
    //                                m.SendMessage("Thou art already in Monitor.");
    //                                return true;
    //                            }
    //                        }
    //                    case "Ship Ticket to Fawn":
    //                        {
    //                            if (m_CurrentLocation != 1)
    //                            {
    //                                tickets[0].Delete();
    //                                m.SendMessage("Thy ticket is taken as thou boardest the ship.");
    //                                PointDest = m_DestFawn;
    //                                break;
    //                            }
    //                            else
    //                            {
    //                                m.SendMessage("Thou art already in Fawn.");
    //                                return true;
    //                            }
    //                        }
    //                    case "Ship Ticket to Sleeping Bull":
    //                        {
    //                            if (m_CurrentLocation != 2)
    //                            {
    //                                tickets[0].Delete();
    //                                m.SendMessage("Thy ticket is taken as thou boardest the ship.");
    //                                PointDest = m_DestSleepingBull;
    //                                break;
    //                            }
    //                            else
    //                            {
    //                                m.SendMessage("Thou art already in Sleeping Bull.");
    //                                return true;
    //                            }
    //                        }
    //                    case "Ship Ticket to Moonshade":
    //                        {
    //                            if (m_CurrentLocation != 3)
    //                            {
    //                                tickets[0].Delete();
    //                                m.SendMessage("Thy ticket is taken as thou boardest the ship.");
    //                                PointDest = m_DestMoonshade;
    //                                break;
    //                            }
    //                            else
    //                            {
    //                                m.SendMessage("Thou art already in Moonshade.");
    //                                return true;
    //                            }
    //                        }
    //                }
    //            }
    //            else
    //            {
    //                switch (tickets[0].Name)
    //                {
    //                    case "Ship Ticket to Monitor":
    //                        {
    //                            if (m_CurrentLocation != 0)
    //                            {
    //                                tickets[0].Delete();
    //                                m.SendMessage("Thy ticket is taken as thou boardest the ship.");
    //                                PointDest = m_DestMonitor;
    //                                break;
    //                            }
    //                            else
    //                            {
    //                                m.SendMessage("Thou art already in Monitor.");
    //                                return true;
    //                            }
    //                        }
    //                    case "Ship Ticket to Fawn":
    //                        {
    //                            if (m_CurrentLocation != 1)
    //                            {
    //                                tickets[0].Delete();
    //                                m.SendMessage("Thy ticket is taken as thou boardest the ship.");
    //                                PointDest = m_DestFawn;
    //                                break;
    //                            }
    //                            else
    //                            {
    //                                m.SendMessage("Thou art already in Fawn.");
    //                                return true;
    //                            }
    //                        }
    //                    case "Ship Ticket to Sleeping Bull":
    //                        {
    //                            if (m_CurrentLocation != 2)
    //                            {
    //                                tickets[0].Delete();
    //                                m.SendMessage("Thy ticket is taken as thou boardest the ship.");
    //                                PointDest = m_DestSleepingBull;
    //                                break;
    //                            }
    //                            else
    //                            {
    //                                m.SendMessage("Thou art already in Sleeping Bull.");
    //                                return true;
    //                            }
    //                        }
    //                    case "Ship Ticket to Moonshade":
    //                        {
    //                            if (m_CurrentLocation != 3)
    //                            {
    //                                tickets[0].Delete();
    //                                m.SendMessage("Thy ticket is taken as thou boardest the ship.");
    //                                PointDest = m_DestMoonshade;
    //                                break;
    //                            }
    //                            else
    //                            {
    //                                m.SendMessage("Thou art already in Moonshade.");
    //                                return true;
    //                            }
    //                        }

    //                }
    //            }
    //            return base.OnMoveOver(m);
    //        }
    //        else
				//m.SendMessage("Thou will needest a ticket to board this ship.");
				
    //        return true;
    //    }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
            writer.Write(m_CurrentLocation);
            writer.Write(m_DestFawn);
            writer.Write(m_DestMonitor);
            writer.Write(m_DestMoonshade);
            writer.Write(m_DestSleepingBull);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
			
            int version = reader.ReadInt();

            m_CurrentLocation = reader.ReadInt();
            m_DestFawn = reader.ReadPoint3D();
            m_DestMonitor = reader.ReadPoint3D();
            m_DestMoonshade = reader.ReadPoint3D();
            m_DestSleepingBull = reader.ReadPoint3D();
        }
    }
}
