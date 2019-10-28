using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Items;

namespace Server.Mobiles
{
    class NakedWoman : TalkingBaseCreature
    {
        [Constructable]
        public NakedWoman() : base(AIType.AI_Mage, FightMode.None, 5, 1, 0.1, 0.2)
        {
            Name = NameList.RandomName("female");

            Female = true;
            InitBody();
            SetHits(HitsMax);
            
        }

        public NakedWoman(Serial serial) : base(serial)
        { }

        public override bool IsInvulnerable
        {
            get
            {
                return true;
            }
        }

        public void InitBody()
        {
            Body = 0x191;
            Hue = 33770;
            SpeechHue = Utility.RandomDyedHue();
            HairItemID = 8252;
            HairHue = 1121;           

            InitOutfit();
        }
        public void InitOutfit()
        {

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
