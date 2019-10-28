using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Server.Mobiles;
using Server.Network;

namespace Server.Regions
{
    public class OverlandRegion : BaseRegion
    {
        public OverlandRegion(XmlElement xml, Map map, Region parent)
            : base(xml, map, parent)
        {
        }

        //UOSI
        public override void OnEnter(Mobile m)
        {
            base.OnEnter(m);

            
            if (!(m is PlayerMobile))
                return;

            m.SendMessage("Thou hast entered " + Name);
            
            m.Send(PlayMusic.GetInstance(MusicName.Invalid));
            m.Send(PlayMusic.GetInstance((m as PlayerMobile).GetRandomWildernessMusic())); 
        }
    }
}
