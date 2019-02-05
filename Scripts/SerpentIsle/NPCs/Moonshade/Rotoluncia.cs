using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Items;

namespace Server.Mobiles
{
    class Rotoluncia : TalkingBaseCreature
    {
        [Constructable]
        public Rotoluncia() : base(AIType.AI_Mage, FightMode.None, 5, 1, 0.1, 0.2)
        {
            Name = "Rotoluncia";
            Female = true;
            InitBody();
            SetHits(HitsMax);
        }

        public Rotoluncia(Serial serial) : base(serial)
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
                Hue = 1138,
                Layer = Layer.Hair,
                Movable = false
            };
            AddItem(hair);

            Item furcape = new FurCape()
            {
                Hue = 2039,
                Movable = false
            };
            AddItem(furcape);

            Item obi = new Obi()
            {
                Hue = 902,
                Movable = false
            };
            AddItem(obi);

            Item bracers = new MidnightBracers()
            {
                Hue = 1109,
                Movable = false
            };
            AddItem(bracers);

            Item gloves = new AssassinGloves()
            {
                Hue = 238,
                Movable = false
            };
            AddItem(gloves);

            Item skirt = new Skirt()
            {
                Hue = 318,
                Movable = false
            };
            AddItem(skirt);

            Shoes shoes = new Shoes()
            {
                Hue = 903,
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
