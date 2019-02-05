using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Items;

namespace Server.Mobiles
{
    class Pothos : TalkingBaseCreature
    {
        [Constructable]
        public Pothos() : base(AIType.AI_Mage, FightMode.None, 5, 1, 0.1, 0.2)
        {
            Name = "Pothos";
            Female = false;
            InitBody();
            SetHits(HitsMax);
        }

        public Pothos(Serial serial) : base(serial)
        {
        }

        public void InitBody()
        {
            Body = 0x190;
            Hue = 33770;
            SpeechHue = Utility.RandomDyedHue();

            InitOutfit();
        }

        public void InitOutfit()
        {
            Item hair = new Item(8261)
            {
                Hue = 1120,
                Layer = Layer.Hair,
                Movable = false
            };
            AddItem(hair);

            Item bonnet = new Bonnet()
            {
                Hue = 438,
                Movable = false
            };
            AddItem(bonnet);

            Item shirt = new ElvenShirt()
            {
                Hue = 963,
                Movable = false
            };
            AddItem(shirt);

            Item chest = new FemalePlateChest()
            {
                Hue = 54,
                Movable = false
            };
            AddItem(chest);

            Item skirt = new Skirt()
            {
                Hue = 948,
                Movable = false
            };
            AddItem(skirt);

            Shoes shoes = new Shoes()
            {
                Hue = 1729,
                Movable = false
            };
            AddItem(shoes);

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
