using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Items;

namespace Server.Mobiles
{
    class Siranush : TalkingBaseCreature
    {
        [Constructable]
        public Siranush() : base(AIType.AI_Mage, FightMode.None, 5, 1, 0.1, 0.2)
        {
            Name = "Siranush";
            Female = true;
            InitBody();
            SetHits(HitsMax);
            
        }

        public Siranush(Serial serial) : base(serial)
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
            HairItemID = 8261;
            HairHue = 0x475;

            InitOutfit();
        }
        public void InitOutfit()
        {
            Shirt shirt = new Shirt();
            shirt.Hue = 218;
            shirt.IsLockedDown = true;
            shirt.BlessedFor = this;
            shirt.Movable = false;


            Doublet doublet = new Doublet();
            doublet.Hue = 707;
            doublet.IsLockedDown = true;
            doublet.BlessedFor = this;
            doublet.Movable = false;

            FancyShirt fancyShirt = new FancyShirt();
            fancyShirt.Hue = 333;
            fancyShirt.IsLockedDown = true;
            fancyShirt.BlessedFor = this;
            fancyShirt.Movable = false;

            Skirt skirt = new Skirt();
            skirt.Hue = 518;

            Boots boots = new Boots();
            boots.Hue = 707;
            boots.IsLockedDown = true;
            boots.BlessedFor = this;
            boots.Movable = false;

            AddItem(skirt);
            AddItem(fancyShirt);
            AddItem(doublet);
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
