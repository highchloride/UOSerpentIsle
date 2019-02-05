using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Items;

namespace Server.Mobiles
{
    class Columna : TalkingBaseCreature
    {
        [Constructable]
        public Columna() : base(AIType.AI_Mage, FightMode.None, 5, 1, 0.1, 0.2)
        {
            Name = "Columna";
            Female = true;
            InitBody();
            SetHits(HitsMax);
        }

        public Columna(Serial serial) : base(serial)
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
                Hue = 1109,
                Layer = Layer.Hair,
                Movable = false
            };
            AddItem(hair);

            Item bandana = new Bandana()
            {
                Hue = 33,
                Movable = false
            };
            AddItem(bandana);

            Item arm = new AssassinArms()
            {
                Hue = 1150,
                Movable = false
            };
            AddItem(arm);

            Item obi = new Obi()
            {
                Hue = 163,
                Movable = false
            };
            AddItem(obi);

            Item skirt = new Skirt()
            {
                Hue = 0,
                Movable = false
            };
            AddItem(skirt);

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
