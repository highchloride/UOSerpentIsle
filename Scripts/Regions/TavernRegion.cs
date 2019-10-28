using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Server.Mobiles;

namespace Server.Regions
{
    class TavernRegion : GuardedRegion
    {
        public TavernRegion(XmlElement xml, Map map, Region parent)
            : base(xml, map, parent)
        {
        }

        public override void OnEnter(Mobile m)
        {
            base.OnEnter(m);

            //UOSI
            if (!(m is PlayerMobile))
                return;

            Timer.DelayCall(TimeSpan.FromSeconds(11), () => RegainHunger(m as PlayerMobile));
        }

        private void RegainHunger(PlayerMobile m)
        {
            if (!(m.Region.GetType() == typeof(TavernRegion)))
                return;

            switch(Utility.Random(2))
            {
                default:
                case 0:
                    {
                        if(m.Hunger < 20)
                        {
                            m.Hunger += 1;
                            break;
                        }
                        else
                        {
                            goto case 1;
                        }
                    }                    
                case 1:
                    if (m.Thirst < 20)
                    {
                        m.Thirst += 1;
                        break;
                    }
                    else
                    {
                        if (m.Hunger < 21)
                            goto case 0;
                        else
                            break;
                    }
            }

            Timer.DelayCall(TimeSpan.FromSeconds(11), () => RegainHunger(m as PlayerMobile));
        }
    }
}
