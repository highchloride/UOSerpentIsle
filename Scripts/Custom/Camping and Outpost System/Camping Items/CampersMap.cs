using Server.Multis;
using Server.Prompts;
using Server.Regions;
using Server.Misc;
using System;

namespace Server.Items
{
    public enum LocationType
    {
        Normal,
        Shop,
        Ship
    }

    public class CampersMap : Item
    {
        private const string RuneFormat = "Marked with the location of {0}";
        private DateTime TravelTime = DateTime.UtcNow;
        private string m_Description;
        private bool m_Marked;
        private Map m_TargetMap;
        private BaseHouse m_House;
        private BaseGalleon m_Galleon;

        [CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
        public LocationType Type { get; set; }

        [Constructable]
        public CampersMap()
            : base(0x14EC)
        {
            Weight = 1.0;
	    Name = "camper's map";
            Type = LocationType.Normal;
        }

        public CampersMap(Serial serial)
            : base(serial)
        {
        }

        [CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
        public BaseHouse House
        {
            get
            {
                if (m_House != null && m_House.Deleted)
                    House = null;

                return m_House;
            }
            set
            {
                m_House = value;

                if (value != null)
                {
                    Type = LocationType.Shop;
                }

                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
        public BaseGalleon Galleon
        {
            get
            {
                if (m_Galleon != null && m_Galleon.Deleted)
                    Galleon = null;

                return m_Galleon;
            }
            set
            {
                m_Galleon = value;

                if (value != null)
                {
                    Type = LocationType.Ship;
                }

                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
        public string Description
        {
            get { return m_Description; }
            set
            {
                m_Description = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
        public bool Marked
        {
            get { return m_Marked; }
            set
            {
                if (m_Marked != value)
                {
                    m_Marked = value;
                    InvalidateProperties();
                }
            }
        }

        [CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
        public Point3D Target { get; set; }

        [CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
        public Map TargetMap
        {
            get { return m_TargetMap; }
            set
            {
                if (m_TargetMap != value)
                {
                    m_TargetMap = value;
                    InvalidateProperties();
                }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(2); // version

            writer.Write((int)Type);
            writer.Write(m_Galleon);
            writer.Write(m_House);
            writer.Write(m_Description);
            writer.Write(m_Marked);
            writer.Write(Target);
            writer.Write(m_TargetMap);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            switch (version)
            {
                case 2:
                    {
                        Type = (LocationType)reader.ReadInt();
                        m_Galleon = reader.ReadItem() as BaseGalleon;
                        m_House = reader.ReadItem() as BaseHouse;
                        m_Description = reader.ReadString();
                        m_Marked = reader.ReadBool();
                        Target = reader.ReadPoint3D();
                        m_TargetMap = reader.ReadMap();

                        break;
                    }
                case 1:
                    {
                        m_House = reader.ReadItem() as BaseHouse;
                        goto case 0;
                    }
                case 0:
                    {
                        m_Description = reader.ReadString();
                        m_Marked = reader.ReadBool();
                        Target = reader.ReadPoint3D();
                        m_TargetMap = reader.ReadMap();

                        break;
                    }
            }
        }

        public void SetGalleon(BaseGalleon galleon)
        {
            m_Marked = true;
            Galleon = galleon;
        }

        public void Mark(Mobile m)
        {
            LocationEmpty();

            m_Marked = true;

            bool setDesc = false;

            m_Galleon = BaseBoat.FindBoatAt(m) as BaseGalleon;

            if (m_Galleon != null)
            {
                Type = LocationType.Ship;
            }
            else
            {
                m_House = BaseHouse.FindHouseAt(m);

                if (m_House == null)
                {
                    Target = m.Location;
                    m_TargetMap = m.Map;

                    Type = LocationType.Normal;
                }
                else
                {
                    HouseSign sign = m_House.Sign;

                    if (sign != null)
                        m_Description = sign.Name;
                    else
                        m_Description = null;

                    if (m_Description == null || (m_Description = m_Description.Trim()).Length == 0)
                        m_Description = "a campsite near an unnamed house";

                    setDesc = true;

                    int x = m_House.BanLocation.X;
                    int y = m_House.BanLocation.Y + 2;
                    int z = m_House.BanLocation.Z;

                    Map map = m_House.Map;

                    if (map != null && !map.CanFit(x, y, z, 16, false, false))
                        z = map.GetAverageZ(x, y);

                    Target = new Point3D(x, y, z);
                    m_TargetMap = map;

                    Type = LocationType.Shop;
                }
            }

            if (!setDesc)
                m_Description = "a location in the wilderness of " + BaseRegion.GetRuneNameFor(Region.Find(Target, m_TargetMap));

            InvalidateProperties();
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            if (m_Marked)
            {
		string desc;
		if ((desc = m_Description) == null || (desc = desc.Trim()).Length == 0)
		    desc = "a campsite";

                    if (m_TargetMap == Map.Tokuno)
                        list.Add((House != null ? 1063260 : 1063259), RuneFormat, desc); // ~1_val~ (Tokuno Islands)[(House)]
                    else if (m_TargetMap == Map.Malas)
                        list.Add((House != null ? 1062454 : 1060804), RuneFormat, desc); // ~1_val~ (Malas)[(House)]
                    else if (m_TargetMap == Map.Felucca)
                        list.Add((House != null ? 1062452 : 1060805), RuneFormat, desc); // ~1_val~ (Felucca)[(House)]
                    else if (m_TargetMap == Map.Trammel)
                        list.Add((House != null ? 1062453 : 1060806), RuneFormat, desc); // ~1_val~ (Trammel)[(House)]
                    else if (m_TargetMap == Map.TerMur)
                        list.Add((House != null ? 1113206 : 1113205), RuneFormat, desc); // ~1_val~ (Ter Mur)(House)
                    else
                        list.Add((House != null ? "{0} ({1})(House)" : "{0} ({1})"), string.Format(RuneFormat, desc), m_TargetMap);
            }
        }

        public void LocationEmpty()
        {
            m_Description = null;
            m_Galleon = null;
            m_House = null;
            Target = Point3D.Zero;
            m_TargetMap = null;
            Type = LocationType.Normal;
            m_Marked = false;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (Type == LocationType.Ship)
            {
                if (Galleon == null)
                {
                    LocationEmpty();
                }

                InvalidateProperties();
                return;
            }

            string message = "";
	    int number;

            if (!IsChildOf(from.Backpack))
            {
		number = 1;
		message = "That must be in your backpack to use it.";
            }
            else if (House != null)
            {
		number = 1;
                //number = 1062399; // You cannot edit the description for this rune.
		message = "You cannot use this.";
            }
            else if (m_Marked)
            {
                number = 0;

		if(from.Skills[SkillName.Camping].Value < 40.0)
			from.SendMessage("You do not have the required camping skill to use that.");
		else if(CanTravel(from))
		    BeginTravel(from);
            }
            else
            {
		number = 0;
		message = "You can use this map to chart a route back to your campsite.";
            }

            if (number > 0)
                from.SendMessage(message);
        }

	public void BeginTravel(Mobile from)
	{
	    from.Say("*You begin your journey*");
	    from.Animate(AnimationType.Fidget, 0);
	    TimeSpan delay = TimeSpan.FromSeconds(3);
            Timer.DelayCall(delay, new TimerStateCallback<Mobile>(Travel), from);
	}

	public void Travel(Mobile from)
	{
	    from.SendMessage("You find your way back to your campsite.");
	    from.Location = Target;
	    from.Map = TargetMap;
	    TravelTime = DateTime.UtcNow + TimeSpan.FromMinutes(3);
	}

	public bool CanTravel(Mobile from)
	{
            	if (CheckCombat(from))
            	{
			from.SendMessage("It is too dangerous to begin your trek right now.");
                	return false;
            	}
            	else if (Misc.WeightOverloading.IsOverloaded(from))
            	{
                	from.SendLocalizedMessage(502359, "", 0x22); // Thou art too encumbered to move.
                	return false;
           	}
		else if (TravelTime > DateTime.UtcNow)
		{
			from.SendMessage("You cannot use that again so soon.");
                	return false;
		}
		else
			return true;

	}

        public static bool CheckCombat(Mobile m, bool restrict = true)
        {
            if (!restrict)
                return false;

            return Aggression.CheckHasAggression(m, true);
        }
    }
}
