using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Server.Mobiles;
using Server.Targeting;
using Xanthos.ShrinkSystem;

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

            //UOSI - Do not apply anything below here to anything that isn't a Playermobile.
            if (!(m is PlayerMobile))
                return;

            Timer.DelayCall(TimeSpan.FromSeconds(11), () => RegainHunger(m as PlayerMobile));
        }

        private void RegainHunger(PlayerMobile m)
        {
            if (!(m.Region.GetType() == typeof(TavernRegion)))
                return;

            if (m.Hunger != 20 || m.Thirst != 20)
            {
                if (m.Hunger > m.Thirst)
                    m.Thirst++;
                else
                    m.Hunger++;
            }

            Timer.DelayCall(TimeSpan.FromSeconds(11), () => RegainHunger(m as PlayerMobile));
        }

        public override bool OnDoubleClick(Mobile m, object o)
        {
            if(o is ShrinkItem)
            {
                m.SendMessage("You cannot do this in a tavern.");
                return false;
            }

            return base.OnDoubleClick(m, o);
        }
    }
}
