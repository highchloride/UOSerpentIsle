using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Items;

namespace Server.Mobiles
{
    class Mosh : TalkingBaseCreature
    {
        [Constructable]
        public Mosh() : base(AIType.AI_Mage, FightMode.None, 5, 1, 0.1, 0.2)
        {
            Name = "Mosh";
            Female = true;
            InitBody();
            SetHits(HitsMax);
        }

        public Mosh(Serial serial) : base(serial)
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
                Hue = 1102,
                Layer = Layer.Hair,
                Movable = false
            };
            AddItem(hair);

            Item robe = new Robe()
            {
                Hue = 547,
                Movable = false
            };
            AddItem(robe);

            Shoes shoes = new Shoes()
            {
                Hue = 1746,
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
