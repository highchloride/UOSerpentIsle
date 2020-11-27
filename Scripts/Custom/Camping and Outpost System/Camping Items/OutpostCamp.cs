using Server.Engines.Quests;
using Server.Items;
using Server.Mobiles;
using System;
using Server.Gumps;

namespace Server.Multis
{
    public class OutpostCamp : BaseCamp
    {
	public OutpostFirepit m_Fire;
	public OutpostSupplies m_Supplies;
	public Item m_Stash;
	public Item m_Ankh;
	public Item m_Tent;
	public Item m_Bed;
	public bool Active = false;

        [Constructable]
        public OutpostCamp()
            : base(0x1F6D) 
        {
        }

        public OutpostCamp(Serial serial)
            : base(serial)
        {
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public override TimeSpan DecayDelay => TimeSpan.FromMinutes(60.0);


        public override void AddComponents()
        {
            Visible = false;

	    AddFire();
        }

        // Don't refresh decay timer
        public override void OnEnter(Mobile m)
        {
	    if(!Active)
		m.SendMessage("You find an abandoned outpost");
	    else
		m.SendMessage("You enter the comfort of an outpost.");
        }


        public override void AddItem(Item item, int xOffset, int yOffset, int zOffset)
        {
            if (item != null)
                item.Movable = false;

            base.AddItem(item, xOffset, yOffset, zOffset);
        }

        public void AddBank()
        {
            CampStash stash = new CampStash();
	    m_Stash = stash;
            AddItem(stash, 5, 6, 0);
        }

	public void AddAnkh()
	{
	    AnkhWest ankh = new AnkhWest();
	    m_Ankh = ankh;
	    AddItem(ankh, -4, 3, 0);
	}

	public void AddFire()
	{
	    OutpostFirepit fire = new OutpostFirepit();
	    fire.LinkedOutpost = this;
	    m_Fire = fire;
	    AddItem(fire, 0, 5, 0);
	}

	public void AddTent()
	{
	    SmallTent tent = new SmallTent();
	    m_Tent = tent;
	    AddItem(tent, 6, -2, 0);

	    OutpostBedroll bed = new OutpostBedroll();
	    m_Bed = bed;
            AddItem(bed, 4, 2, 0);
	}

	public void AddSupplies()
	{
	    OutpostSupplies supplies = new OutpostSupplies();
	    supplies.LinkedOutpost = this;
	    m_Supplies = supplies;
	    AddItem(supplies, -1, 6, 0);
	}

	public void Reset()
	{
               IPooledEnumerable eable = GetItemsInRange(10);

		foreach(Item target in eable)
		{
		    if(target is OutpostFirepit)
		    {
			OutpostFirepit firepit = target as OutpostFirepit;
			firepit.LinkedOutpost = this;
			m_Fire = firepit;
			firepit.TurnOff();

			Active = false;
			TimeOfDecay = DateTime.MinValue;
		    }

		    if(target is OutpostSupplies)
		    {
			OutpostSupplies supplies = target as OutpostSupplies;
			m_Supplies = supplies;
			m_Supplies.Delete();
			m_Supplies = null;
		    }
		    if(target is SmallTent)
		    {
			m_Tent = target;
			m_Tent.Delete();
			m_Tent = null;
		    }
		    if(target is OutpostBedroll)
		    {
			m_Bed = target;
			m_Bed.Delete();
			m_Bed = null;
		    }
		    if(target is CampStash)
		    {
			m_Stash = target;
			m_Stash.Delete();
			m_Stash = null;
		    }
		    if(target is AnkhWest)
		    {
			m_Ankh = target;
			m_Ankh.Delete();
			m_Ankh = null;
		    }		
		}		
	}

	public override void OnDoubleClick(Mobile m)
	{

	    if(Active)
	    {
		if (!m.HasGump(typeof(OutpostGump)) && (m.Skills[SkillName.Camping].Value >= 50))
		{
		    m.SendGump(new OutpostGump(m, this));
		}
	    }
	}

	public override void CheckDecay()
	{

	    if(m_Fire == null)
		Reset();

            if (!Decaying)
            {
		if(m_Fire != null && m_Fire.Active && !Active) //This should set outpost to active and begin decaying
		{
		    Active = true;
		    SetDecayTime();
		}
	    }
	    else if (TimeOfDecay < DateTime.UtcNow && m_Fire != null) 
	    {

		if(m_Stash != null)
		{
		    m_Stash.Delete();
		    m_Stash = null;
		}
		if(m_Ankh != null)
		{
		    m_Ankh.Delete();
		    m_Ankh = null;
		}
		if(m_Tent != null)
		{
		    m_Tent.Delete();
		    m_Tent = null;
		}
		if(m_Bed != null)
		{
		    m_Bed.Delete();
		    m_Bed = null;
		}
		if(m_Supplies != null)
		{
		    m_Supplies.Delete();
		    m_Supplies = null;
		}

		m_Fire.TurnOff();
		Active = false;
		TimeOfDecay = DateTime.MinValue;

	    }
	    
	}

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}