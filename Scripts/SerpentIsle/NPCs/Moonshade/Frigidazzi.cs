using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Items;

namespace Server.Mobiles
{
    class Frigidazzi : TalkingBaseCreature
    {
        [Constructable]
        public Frigidazzi() : base(AIType.AI_Mage, FightMode.None, 5, 1, 0.1, 0.2)
        {
            Name = "Frigidazzi";
            Female = true;
            InitBody();
            SetHits(HitsMax);
        }

        public Frigidazzi(Serial serial) : base(serial)
        {
        }

        public void InitBody()
        {
            Body = 0x191;
            Hue = 33770;
            SpeechHue = Utility.RandomDyedHue();

            InitOutfit();
        }

        public void InitOutfit()
        {
            Item hair = new Item(8252)
            {
                Hue = 1118,
                Layer = Layer.Hair,
                Movable = false
            };
            AddItem(hair);

            Item tabi = new SamuraiTabi()
            {
                Hue = 707,
                Movable = false
            };
            AddItem(tabi);

            Item dress = new AntiqueWeddingDress()
            {
                Hue = 3,
                Movable = false
            };
            AddItem(dress);

            Item shirt = new ElvenShirt()
            {
                Hue = 996,
                Movable = false
            };
            AddItem(shirt);

            Item coat = new Surcoat()
            {
                Hue = 1001,
                Movable = false
            };
            AddItem(coat);

            //Shoes shoes = new Shoes()
            //{
            //    Hue = 903,
            //    Movable = false
            //};
            //AddItem(shoes);

            PackGold(50, 200);
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
