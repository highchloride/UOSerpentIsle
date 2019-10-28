using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Mobiles
{
    class MercenaryVendor : BaseVendor
    {
        private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();

        [Constructable]
        public MercenaryVendor()
            : base("the Mercenary Broker")
        {

        }

        public MercenaryVendor(Serial serial)
            : base(serial)
        {
        }

        protected override List<SBInfo> SBInfos
        {
            get
            {
                return this.m_SBInfos;
            }
        }


        public override void InitSBInfo()
        {
            this.m_SBInfos.Add(new SBMercenary());
        }

        public override void InitOutfit()
        {
            base.InitOutfit();
        }

        private static bool m_Talked;

        string[] kfcsay = new string[]
        {
            "If it's a good sword you seek, I can find you a contract.",
            "Swords for hire!",
            "Nothing earns loyalty like gold. Hire a sellsword today.",
            "It is dangerous to brave the wilds alone. Take a sellsword!"
        };

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            //base.OnMovement(m, oldLocation);
            if (m_Talked == false)
            {
                if (m.InRange(this, 5))
                {
                    m_Talked = true;
                    SayRandom(kfcsay, this);
                    this.Move(GetDirectionTo(m.Location));
                    SpamTimer t = new SpamTimer();
                    t.Start();
                }
            }
        }

        private class SpamTimer : Timer
        {
            public SpamTimer() : base(TimeSpan.FromSeconds(45))
            {
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                m_Talked = false;
            }
        }

        private static void SayRandom(string[] say, Mobile m)
        {
            m.Say(say[Utility.Random(say.Length)]);
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
