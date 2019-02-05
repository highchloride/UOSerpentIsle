using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Items;

namespace Server.Mobiles
{
    class Bucia : TalkingBaseCreature
    {
        [Constructable]
        public Bucia() : base(AIType.AI_Mage, FightMode.None, 5, 1, 0.1, 0.2)
        {
            Name = "Bucia";
            Female = true;
            InitBody();
            SetHits(HitsMax);
        }

        public Bucia(Serial serial) : base(serial)
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

            Item dress = new MalabellesDress()
            {
                Hue = 0,
                Movable = false
            };
            AddItem(dress);

            Item obi = new Obi()
            {
                Hue = 742,
                Movable = false
            };
            AddItem(obi);

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
