using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Items;

namespace Server.Mobiles
{
    class Gustacio : TalkingBaseCreature
    {
        [Constructable]
        public Gustacio() : base(AIType.AI_Mage, FightMode.None, 5, 1, 0.1, 0.2)
        {
            Name = "Gustacio";
            Female = false;
            InitBody();
            SetHits(HitsMax);
        }

        public Gustacio(Serial serial) : base(serial)
        {
        }

        public void InitBody()
        {
            Body = 0x190;
            Hue = 33798;
            SpeechHue = Utility.RandomDyedHue();

            InitOutfit();
        }

        public void InitOutfit()
        {
            Item hair = new Item(8263)
            {
                Hue = 2404,
                Layer = Layer.Hair,
                Movable = false
            };
            AddItem(hair);

            Item fhair = new Item(8267)
            {
                Hue = 0,
                Layer = Layer.FacialHair,
                Movable = false
            };
            AddItem(fhair);

            Item doublet = new Doublet()
            {
                Hue = 573,
                Movable = false
            };
            AddItem(doublet);

            Item elvenShirt = new ElvenShirt()
            {
                Hue = 0,
                Movable = false
            };
            AddItem(elvenShirt);

            Item longpants = new LongPants()
            {
                Hue = 0,
                Movable = false
            };
            AddItem(longpants);

            Item leatherBelt = new LeatherNinjaBelt()
            {
                Hue = 903,
                Movable = false
            };
            AddItem(leatherBelt);

            Item staff = new GnarledStaff()
            {
                Hue = 0,
                Movable = false
            };

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
