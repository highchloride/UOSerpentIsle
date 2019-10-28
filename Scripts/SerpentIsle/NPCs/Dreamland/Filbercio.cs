using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Items;

namespace Server.Mobiles
{
    class DreamerFilbercio : TalkingBaseCreature
    {
        [Constructable]
        public DreamerFilbercio() : base(AIType.AI_Mage, FightMode.None, 5, 1, 0.1, 0.2)
        {
            Name = "Filbercio";
            Female = false;
            InitBody();
            SetHits(HitsMax);
        }

        public DreamerFilbercio(Serial serial) : base(serial)
        {
        }

        public void InitBody()
        {
            Body = 0x190;
            Hue = 0x83EA;
            SpeechHue = Utility.RandomDyedHue();

            InitOutfit();
        }

        public void InitOutfit()
        {
            Item hair = new Item(8252)
            {
                Hue = 1133,
                Layer = Layer.Hair,
                Movable = false
            };
            AddItem(hair);

            Bonnet bonnet = new Bonnet()
            {
                Hue = 3
            };
            AddItem(bonnet);

            FancyShirt fancyShirt = new FancyShirt()
            {
                Hue = 718
            };
            AddItem(fancyShirt);

            GildedDress gildedDress = new GildedDress()
            {
                Hue = 718
            };
            AddItem(gildedDress);

            Shoes shoes = new Shoes()
            {
                Hue = 503
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
