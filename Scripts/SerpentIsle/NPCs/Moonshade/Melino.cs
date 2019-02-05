using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Items;

namespace Server.Mobiles
{
    class Melino : TalkingBaseCreature
    {
        [Constructable]
        public Melino() : base(AIType.AI_Mage, FightMode.None, 5, 1, 0.1, 0.2)
        {
            Name = "Melino";
            Female = false;
            InitBody();
            SetHits(HitsMax);
        }

        public Melino(Serial serial) : base(serial)
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
            Item hair = new Item(8264)
            {
                Hue = 1102,
                Layer = Layer.Hair,
                Movable = false
            };
            AddItem(hair);

            Item fhair = new Item(8268)
            {
                Hue = 1102,
                Layer = Layer.FacialHair,
                Movable = false
            };
            AddItem(fhair);

            Item skirt = new Skirt()
            {
                Hue = 0,
                Movable = false
            };
            AddItem(skirt);

            Item jinBaori = new JinBaori()
            {
                Hue = 33,
                Movable = false
            };
            AddItem(jinBaori);

            Item shirt = new FancyShirt()
            {
                Hue = 1150,
                Movable = false
            };
            AddItem(shirt);

            Shoes shoes = new Shoes()
            {
                Hue = 1754,
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
