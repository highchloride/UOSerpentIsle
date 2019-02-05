using System;
using System.Collections;
using System.Collections.Generic;

namespace Server.Items
{
    public class ShipTicketToMoonshade : ShipTicket
    {
		[Constructable]
		public ShipTicketToMoonshade()
		{
            Name = "Ship Ticket to Moonshade";
			Hue = 0x494;
        }

        public ShipTicketToMoonshade(Serial serial)
            : base(serial)
        {
        }

        /*public override int LabelNumber
        {
            get
            {
                return 1020526;
            }
        }// bone machete*/

        public override void OnDoubleClick(Mobile from)
        {
            //Gotta be in the backpack to use it
            if (!from.Backpack.Items.Contains(this))
            {
                from.SendMessage("This must be in thy backpack to use it.");
            }
            else
            {
                //Get the ship ticket teleport the player is standing on
                List<Item> items = from.Location.GetItemsAt(from.Map);

                ShipTicketTele shipTicketTele = null;

                foreach (Item item in items)
                {
                    if (item.GetType() == typeof(ShipTicketTele))
                    {
                        shipTicketTele = item as ShipTicketTele;
                    }
                }

                //If not tele, no usage.
                if (shipTicketTele != null)
                {
                    if (shipTicketTele.CurrentLocation == 3)
                    {
                        from.SendMessage("Thou art already in Moonshade.");
                    }
                    else
                    {
                        this.Delete();
                        //shipTicketTele.PointDest = shipTicketTele.DestMoonshade;
                        //shipTicketTele.OnMoveOver(from);
                        from.SendMessage("Thy ticket is taken as thou board the vessel.");
                        from.MoveToWorld(shipTicketTele.DestMoonshade, Map.SerpentIsle);
                        from.PlaySound(0x013);
                    }
                }
                else
                {
                    from.SendMessage("Thou must be near a transport ship to use this ticket.");
                }
            }
            //base.OnDoubleClick(from);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();
        }
    }
}
