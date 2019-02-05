using System;
using Server;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientLocateSpell : AncientSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Locate", "In Wis",
                                                        224,
                                                        9061,
                                                        Reagent.Nightshade
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public AncientLocateSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                if (this.Scroll != null)
                    Scroll.Consume();
                int xLong = 0, yLat = 0;
                int xMins = 0, yMins = 0;
                bool xEast = false, ySouth = false;

                if (AncientLocateSpell.Format(Caster.Location, Caster.Map, ref xLong, ref yLat, ref xMins, ref yMins, ref xEast, ref ySouth))
                {
                    string location = String.Format("Your current location is: {0} {1}'{2}, {3} {4}'{5}", yLat, yMins, ySouth ? "S" : "N", xLong, xMins, xEast ? "E" : "W");
                    Caster.LocalOverheadMessage(MessageType.Regular, Caster.SpeechHue, false, location);
                }
            }
        }

        public static bool Format(Point3D p, Map map, ref int xLong, ref int yLat, ref int xMins, ref int yMins, ref bool xEast, ref bool ySouth)
        {
            if (map == null || map == Map.Internal)
                return false;

            int x = p.X, y = p.Y;
            int xCenter, yCenter;
            int xWidth, yHeight;

            xWidth = 5120; yHeight = 4096;

            if (map == Map.Trammel || map == Map.Felucca)
            {
                if (x >= 0 && y >= 0 && x < 5120 && y < 4096)
                {
                    xCenter = 1323; yCenter = 1624;
                }
                else if (x >= 0 && y >= 0 && x < map.Width && y < map.Height)
                {
                    xCenter = 1323; yCenter = 1624;
                }
                else
                {
                    return false;
                }
            }
            else if (x >= 0 && y >= 0 && x < map.Width && y < map.Height)
            {
                xCenter = 5936; yCenter = 3112;
            }
            else
            {
                return false;
            }

            double absLong = (double)((x - xCenter) * 360) / xWidth;
            double absLat = (double)((y - yCenter) * 360) / yHeight;

            if (absLong > 180.0)
                absLong = -180.0 + (absLong % 180.0);

            if (absLat > 180.0)
                absLat = -180.0 + (absLat % 180.0);

            bool east = (absLong >= 0), south = (absLat >= 0);

            if (absLong < 0.0)
                absLong = -absLong;

            if (absLat < 0.0)
                absLat = -absLat;

            xLong = (int)absLong;
            yLat = (int)absLat;

            xMins = (int)((absLong % 1.0) * 60);
            yMins = (int)((absLat % 1.0) * 60);

            xEast = east;
            ySouth = south;

            return true;
        }
    }
}
