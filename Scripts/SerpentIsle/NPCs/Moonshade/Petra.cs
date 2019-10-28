using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Items;

namespace Server.Mobiles
{
    class Petra : TalkingBaseVendor
    {
        private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();

        [Constructable]
        public Petra() : base("the Barkeep")
        {
            Name = "Petra";
            Female = true;
            //InitBody();
            SetHits(HitsMax);
        }

        public Petra(Serial serial) : base(serial)
        {
        }

        public override void InitBody()
        {
            Body = 0x191;
            Hue = 996;
            SpeechHue = Utility.RandomDyedHue();

            //InitOutfit();
        }

        public override void InitOutfit()
        {
            Item hair = new Item(8252)
            {
                Hue = 1102,
                Layer = Layer.Hair,
                Movable = false
            };
            AddItem(hair);            

            PackGold(50, 200);
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
            this.m_SBInfos.Add(new SBBarkeeper());
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
