using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Items;

namespace Server.Mobiles
{
    class Fedabiblio : TalkingBaseVendor
    {
        private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();

        [Constructable]
        public Fedabiblio() : base("the Headmaster-Mage")
        {
            Name = "Fedabiblio";
            Female = false;
            //InitBody();
            SetHits(HitsMax);
        }

        public Fedabiblio(Serial serial) : base(serial)
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
            Item hair = new Item(8251)
            {
                Hue = 1109,
                Layer = Layer.Hair,
                Movable = false
            };
            AddItem(hair);

            Item fhair = new Item(8256)
            {
                Hue = 1109,
                Layer = Layer.FacialHair,
                Movable = false
            };
            AddItem(fhair);

            Item hakama = new HakamaShita()
            {
                Hue = 803,
                Movable = false
            };
            AddItem(hakama);

            Item tattsuke = new TattsukeHakama()
            {
                Hue = 902,
                Movable = false
            };
            AddItem(tattsuke);

            Item shirt = new FancyShirt()
            {
                Hue = 0,
                Movable = false
            };
            AddItem(shirt);

            Item tabi = new NinjaTabi()
            {
                Hue = 707,
                Movable = false
            };
            AddItem(tabi);

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
            this.m_SBInfos.Add(new SBFedabiblio());
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
