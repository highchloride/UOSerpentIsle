using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Server.Gumps;
using System.Linq;

namespace Server.Items
{
    public class GumpPad : Teleporter
    {
        private int m_PadType;

        [Constructable]
        public GumpPad()
            : base()
        {
            m_PadType = 0;
        }

        public GumpPad(Serial serial)
            : base(serial)
        {
        }

        [CommandProperty(AccessLevel.Decorator)]
        public int PadType
        {
            get { return m_PadType; }
            set { m_PadType = value; }
        }

        public override bool OnMoveOver(Mobile m)
        {
            switch(m_PadType)
            {
                default:
                case 0:  //Sail to the Serpent Isle?
                    {
                        m.CantWalk = true;

                        if (m.FindGump<GumpSailToSerpentIsle>() == null)
                            m.SendGump(new GumpSailToSerpentIsle(m));
                        break;
                    }
            }
            return true;
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
}
