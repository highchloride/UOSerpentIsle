using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Items;

namespace Server.Mobiles
{
    class Ducio : TalkingBaseVendor
    {
        private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();

        [Constructable]
        public Ducio() : base("the Artisan") //Blacksmith, Tanner, Tailor
        {
            Name = "Ducio";
            Female = false;
            //InitBody();
            SetHits(HitsMax);
        }

        public Ducio(Serial serial) : base(serial)
        {
        }

        public override void InitBody()
        {
            Body = 0x190;
            Hue = 33770;
            SpeechHue = Utility.RandomDyedHue();

            //InitOutfit();
        }

        public override void InitOutfit()
        {
            Item hair = new Item(8261)
            {
                Hue = 1133,
                Layer = Layer.Hair,
                Movable = false
            };
            AddItem(hair);

            Item jinbaori = new JinBaori()
            {
                Hue = 247,
                Movable = false
            };
            AddItem(jinbaori);

            Item pants = new ShortPants()
            {
                Hue = 207,
                Movable = false
            };
            AddItem(pants);

            Shoes shoes = new Shoes()
            {
                Hue = 1725,
                Movable = false
            };
            AddItem(shoes);

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
            this.m_SBInfos.Add(new SBBlacksmith());
            this.m_SBInfos.Add(new SBTailor());
            this.m_SBInfos.Add(new SBTanner());
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
