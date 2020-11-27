using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Items;

namespace Server.Mobiles
{
    class Mortegro : TalkingBaseVendor
    {
        private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();

        [Constructable]
        public Mortegro() : base("the Necromancer")
        {
            Name = "Mortegro";
            Female = false;
            //InitBody();
            SetHits(HitsMax);
        }

        public Mortegro(Serial serial) : base(serial)
        {
        }

        public override void InitBody()
        {
            Body = 0x190;
            Hue = 33791;
            SpeechHue = Utility.RandomDyedHue();

            InitOutfit();
        }

        public override void InitOutfit()
        {
            Item hair = new Item(8261)
            {
                Hue = 1609,
                Layer = Layer.Hair,
                Movable = false
            };
            AddItem(hair);

            Item kimono = new MaleKimono()
            {
                Hue = 0,
                Movable = false
            };
            AddItem(kimono);

            Item thighboots = new ThighBoots()
            {
                Hue = 1864,
                Movable = false
            };
            AddItem(thighboots);

            //Shoes shoes = new Shoes()
            //{
            //    Hue = 903,
            //    Movable = false
            //};
            //AddItem(shoes);

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
            this.m_SBInfos.Add(new SBMortegro());
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
