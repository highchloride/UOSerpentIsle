using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Items;

namespace Server.Mobiles
{
    class Torrissio : TalkingBaseVendor
    {
        private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();

        [Constructable]
        public Torrissio() : base("the Mystic")
        {
            Name = "Torrissio";
            Female = false;
            InitBody();
            SetHits(HitsMax);
        }

        public Torrissio(Serial serial) : base(serial)
        {
        }

        public void InitBody()
        {
            Body = 0x190;
            Hue = 33798;
            SpeechHue = Utility.RandomDyedHue();

            InitOutfit();
        }

        public void InitOutfit()
        {
            Item hair = new Item(8261)
            {
                Hue = 1609,
                Layer = Layer.Hair,
                Movable = false
            };
            AddItem(hair);

            Item fhair = new Item(8257)
            {
                Hue = 1509,
                Layer = Layer.FacialHair,
                Movable = false
            };
            AddItem(fhair);

            Item surcoat = new LibraryFriendSurcoat()
            {
                Hue = 768,
                Movable = false
            };
            AddItem(surcoat);

            Item fancyShirt = new FancyShirt()
            {
                Hue = 0,
                Movable = false
            };
            AddItem(fancyShirt);

            Item leatherBelt = new LeatherNinjaBelt()
            {
                Hue = 903,
                Movable = false
            };
            AddItem(leatherBelt);

            Item pants = new ElvenPants()
            {
                Hue = 1153,
                Movable = false
            };
            AddItem(pants);

            Shoes shoes = new Shoes()
            {
                Hue = 903,
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
            this.m_SBInfos.Add(new SBTorrissio());
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
