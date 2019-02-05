using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Items;

namespace Server.Mobiles
{
    class Mortegro : TalkingBaseCreature
    {
        [Constructable]
        public Mortegro() : base(AIType.AI_Mage, FightMode.None, 5, 1, 0.1, 0.2)
        {
            Name = "Mortegro";
            Female = false;
            InitBody();
            SetHits(HitsMax);
        }

        public Mortegro(Serial serial) : base(serial)
        {
        }

        public void InitBody()
        {
            Body = 0x190;
            Hue = 33791;
            SpeechHue = Utility.RandomDyedHue();

            InitOutfit();
        }

        public void InitOutfit()
        {
            Item hair = new Item(8261)
            {
                Hue = 1609,
                Layer = Layer.Hair,
                Movable = false
            };
            AddItem(hair);

            Item kimono = new MaleKimono()
            {
                Hue = 0,
                Movable = false
            };
            AddItem(kimono);

            Item thighboots = new ThighBoots()
            {
                Hue = 1864,
                Movable = false
            };
            AddItem(thighboots);

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
