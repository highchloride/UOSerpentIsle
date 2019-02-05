using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Items;

namespace Server.Mobiles
{
    class Andrio : TalkingBaseCreature
    {
        [Constructable]
        public Andrio() : base(AIType.AI_Mage, FightMode.None, 5, 1, 0.1, 0.2)
        {
            Name = "Andrio";
            Female = false;
            InitBody();
            SetHits(HitsMax);
        }

        public Andrio(Serial serial) : base(serial)
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
            Item hair = new Item(8251)
            {
                Hue = 1136,
                Layer = Layer.Hair,
                Movable = false
            };
            AddItem(hair);

            Item shirt = new Shirt()
            {
                Hue = 3,
                Movable = false
            };
            AddItem(shirt);

            Item jinBaori = new JinBaori()
            {
                Hue = 118,
                Movable = false
            };
            AddItem(jinBaori);

            Item pants = new LongPants()
            {
                Hue = 3,
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
