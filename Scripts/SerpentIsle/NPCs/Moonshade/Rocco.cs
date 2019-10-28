using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Items;

namespace Server.Mobiles
{
    class Rocco : TalkingBaseVendor
    {
        private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();

        [Constructable]
        public Rocco() : base("the Barkeep")
        {
            Name = "Rocco";
            Female = false;
            //InitBody();
            SetHits(HitsMax);
        }

        public Rocco(Serial serial) : base(serial)
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
            Item hair = new Item(8263)
            {
                Hue = 1133,
                Layer = Layer.Hair,
                Movable = false
            };
            AddItem(hair);

            Item coat = new Surcoat()
            {
                Hue = 448,
                Movable = false
            };
            AddItem(coat);

            Item shirt = new ElvenShirt()
            {
                Hue = 0,
                Movable = false
            };
            AddItem(shirt);

            Item pants = new LongPants()
            {
                Hue = 148,
                Movable = false
            };
            AddItem(pants);

            Shoes shoes = new Shoes()
            {
                Hue = 1754,
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
            this.m_SBInfos.Add(new SBTavernKeeper());
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
