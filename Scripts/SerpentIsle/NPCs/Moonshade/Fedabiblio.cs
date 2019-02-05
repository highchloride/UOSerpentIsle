using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Items;

namespace Server.Mobiles
{
    class Fedabiblio : TalkingBaseCreature
    {
        [Constructable]
        public Fedabiblio() : base(AIType.AI_Mage, FightMode.None, 5, 1, 0.1, 0.2)
        {
            Name = "Fedabiblio";
            Female = false;
            InitBody();
            SetHits(HitsMax);
        }

        public Fedabiblio(Serial serial) : base(serial)
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

            Item fhair = new Item(8256)
            {
                Hue = 1109,
                Layer = Layer.FacialHair,
                Movable = false
            };
            AddItem(fhair);

            Item hakama = new HakamaShita()
            {
                Hue = 803,
                Movable = false
            };
            AddItem(hakama);

            Item tattsuke = new TattsukeHakama()
            {
                Hue = 902,
                Movable = false
            };
            AddItem(tattsuke);

            Item shirt = new FancyShirt()
            {
                Hue = 0,
                Movable = false
            };
            AddItem(shirt);

            Item tabi = new NinjaTabi()
            {
                Hue = 707,
                Movable = false
            };
            AddItem(tabi);

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
