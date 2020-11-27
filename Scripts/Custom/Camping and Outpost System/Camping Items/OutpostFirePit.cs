using Server.ContextMenus;
using Server.Gumps;
using Server.Mobiles;
using System.Collections.Generic;
using Server.Multis;
using System;

namespace Server.Items
{

    public class OutpostFirepit : Item
    {

	private InternalItem m_Item;
	public OutpostCamp LinkedOutpost;
	public bool Active;

	[Constructable]
	public OutpostFirepit()
            : base(0xFAC)
	{
	    Movable = false;
	    Active = false;

	}

        public OutpostFirepit(Serial serial)
            : base(serial)
        {
        }

	public override void OnDoubleClick(Mobile m)
	{

	    if(!Active && m.Skills[SkillName.Camping].Value >= 50)
	    {
		m.SendMessage("You light the fire.");
		TurnOn();
	    }
	    else if(!Active)
		m.SendMessage("A skilled camper could establish an outpost here.");

	}

	public void TurnOff()
	{
	    if(m_Item != null)
	    {
	    	m_Item.Delete();
	    	m_Item = null;
	    }

	    Active = false;
	}

	public void TurnOn()
	{
	    if(m_Item == null)
	    {
		m_Item = new InternalItem(this);
                m_Item.Location = new Point3D(X, Y, Z+1);
		m_Item.Map = Map;
	    }

	    if(LinkedOutpost != null)
		LinkedOutpost.AddSupplies();

	    Active = true;
	}

        public override void OnLocationChange(Point3D oldLocation)
        {
            if (m_Item != null && Active)
                m_Item.Location = new Point3D(X, Y, Z+1);
        }


        public override void OnMapChange()
        {
            if (m_Item != null)
                m_Item.Map = Map;
        }

        public override void OnAfterDelete()
        {
            base.OnAfterDelete();

            if (m_Item != null)
                m_Item.Delete();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version

            writer.Write(m_Item);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            m_Item = reader.ReadItem() as InternalItem;
        }

	private class InternalItem : Item
	{
	    private OutpostFirepit m_Item;
	    public InternalItem(OutpostFirepit item)
		: base(0xDE3)
	    {
	    	Movable = false;

		m_Item = item;
	    }

	    public InternalItem(Serial serial)
		: base(serial)
	    {
	    }

            [Hue, CommandProperty(AccessLevel.GameMaster)]
            public override int Hue
            {
                get
                {
                    return base.Hue;
                }
                set
                {
                    base.Hue = value;
                    if (m_Item.Hue != value)
                        m_Item.Hue = value;
                }
            }

            public override void OnMapChange()
            {
                if (m_Item != null)
                    m_Item.Map = Map;
            }

            public override void OnAfterDelete()
            {
                base.OnAfterDelete();

            }

            public override void Serialize(GenericWriter writer)
            {
                base.Serialize(writer);

                writer.Write(0); // version

                writer.Write(m_Item);
            }

            public override void Deserialize(GenericReader reader)
            {
                base.Deserialize(reader);

                int version = reader.ReadInt();

                m_Item = reader.ReadItem() as OutpostFirepit;
            }
	}
    }   
}