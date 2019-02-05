using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Items;

namespace Server.Mobiles
{
    class Freli : TalkingBaseCreature
    {
        [Constructable]
        public Freli() : base(AIType.AI_Mage, FightMode.None, 5, 1, 0.1, 0.2)
        {
            Name = "Freli";
            Female = false;
            InitBody();
            SetHits(HitsMax);
        }

        public Freli(Serial serial) : base(serial)
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
                Hue = 1118,
                Layer = Layer.Hair,
                Movable = false
            };
            AddItem(hair);

            Item shirt = new Tunic()
            {
                Hue = 3,
                Movable = false
            };
            AddItem(shirt);

            Item belt = new LeatherNinjaBelt()
            {
                Hue = 0,
                Movable = false
            };
            AddItem(belt);

            Item pants = new LongPants()
            {
                Hue = 1150,
                Movable = false
            };
            AddItem(pants);

            Shoes shoes = new Shoes()
            {
                Hue = 1150,
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
