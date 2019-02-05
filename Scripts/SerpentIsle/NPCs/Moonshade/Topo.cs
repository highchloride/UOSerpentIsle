using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Items;

namespace Server.Mobiles
{
    class Topo : TalkingBaseCreature
    {
        [Constructable]
        public Topo() : base(AIType.AI_Mage, FightMode.None, 5, 1, 0.1, 0.2)
        {
            Name = "Topo";
            Female = false;
            InitBody();
            SetHits(HitsMax);
        }

        public Topo(Serial serial) : base(serial)
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
                Hue = 1109,
                Layer = Layer.Hair,
                Movable = false
            };
            AddItem(hair);

            Item coat = new Surcoat()
            {
                Hue = 322,
                Movable = false
            };
            AddItem(coat);

            Item shirt = new ElvenDarkShirt()
            {
                Hue = 738,
                Movable = false
            };
            AddItem(shirt);

            Item pants = new LongPants()
            {
                Hue = 247,
                Movable = false
            };
            AddItem(pants);

            Item gloves = new AssassinGloves()
            {
                Hue = 0,
                Movable = false
            };
            AddItem(gloves);

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
