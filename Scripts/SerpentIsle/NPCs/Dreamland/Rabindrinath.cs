using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Items;

namespace Server.Mobiles
{
    class Rabindrinath : TalkingBaseCreature
    {
        [Constructable]
        public Rabindrinath() : base(AIType.AI_Mage, FightMode.None, 5, 1, 0.1, 0.2)
        {
            Name = "Rabindrinath";
            Female = false;
            InitBody();
            SetHits(HitsMax);
            
        }

        public Rabindrinath(Serial serial) : base(serial)
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
            Body = 0x190;
            Hue = 33770;
            SpeechHue = Utility.RandomDyedHue();
            HairItemID = 8252;
            HairHue = 0x455;

            FacialHairItemID = 8255;
            FacialHairHue = 1109;

            InitOutfit();
        }
        public void InitOutfit()
        {
            Robe robe = new Robe();
            robe.Hue = 802;

            Boots boots = new Boots();
            boots.Hue = 707;            

            AddItem(robe);
            AddItem(boots);
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
