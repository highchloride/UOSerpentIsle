using Server.Mobiles;
using System;
using System.Xml;

namespace Server.Regions
{
    /// <summary>
    /// A DangerRegion is just a DungeonRegion w/o the AlterLightLevel call. For making overland dungeon areas.
    /// </summary>
    public class DangerRegion : BaseRegion
    {
        private Point3D m_EntranceLocation;
        private Map m_EntranceMap;

        public DangerRegion(XmlElement xml, Map map, Region parent)
            : base(xml, map, parent)
        {
            XmlElement entrEl = xml["entrance"];

            Map entrMap = map;
            ReadMap(entrEl, "map", ref entrMap, false);

            if (ReadPoint3D(entrEl, entrMap, ref this.m_EntranceLocation, false))
                this.m_EntranceMap = entrMap;
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public override bool YoungProtected
        {
            get
            {
                return false;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D EntranceLocation
        {
            get
            {
                return this.m_EntranceLocation;
            }
            set
            {
                this.m_EntranceLocation = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Map EntranceMap
        {
            get
            {
                return this.m_EntranceMap;
            }
            set
            {
                this.m_EntranceMap = value;
            }
        }

        //UOSI
        public override void OnEnter(Mobile m)
        {
            base.OnEnter(m);

            if (!(m is PlayerMobile))
                return;

            m.SendMessage("Thou hast entered " + Name);
        }

        public override bool AllowHousing(Mobile from, Point3D p)
        {
            return false;
        }
    }
}