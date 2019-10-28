using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Items;

namespace Server.Mobiles
{
    class LordBritish : TalkingBaseCreature
    {
        [Constructable]
        public LordBritish() : base(AIType.AI_Mage, FightMode.None, 5, 1, 0.1, 0.2)
        {
            Name = "Lord British";
            Female = false;
            InitBody();
            SetHits(HitsMax);
            
        }

        public LordBritish(Serial serial) : base(serial)
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

            InitOutfit();
        }
        public void InitOutfit()
        {
            LordBritishSuit suit = new LordBritishSuit();
            suit.AccessLevel = AccessLevel.Player;
            suit.IsLockedDown = true;
            suit.BlessedFor = this;
            suit.Movable = false;
            AddItem(suit);
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
