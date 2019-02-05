using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Items;

namespace Server.Mobiles
{ 
    public class Pikeman : BaseRanger
    {
        [Constructable]
        public Pikeman() : base (AIType.AI_Melee, FightMode.Closest, 15, 1, 0.1, 0.2)
        {
            Title = "the pikeman";

            SetStr(1750, 1750);
            SetDex(150, 150);
            SetInt(61, 75);

            SetSkill(SkillName.MagicResist, 120.0, 120.0);
            SetSkill(SkillName.Swords, 120.0, 120.0);
            SetSkill(SkillName.Tactics, 120.0, 120.0);
            SetSkill(SkillName.Anatomy, 120.0, 120.0);

            AddItem(new ChainCoif());
            AddItem(new PlateArms());
            AddItem(new PlateLegs());

            if((Utility.Random(1000) % 2) == 1)
            {
                AddItem(new Halberd());
            }
            else
            {
                AddItem(new Broadsword());
                AddItem(new BronzeShield());
            }

            if (Female = Utility.RandomBool())
            {
                Body = 401;
                Name = NameList.RandomName("female");

                FemalePlateChest fpc = new FemalePlateChest();
                fpc.Hue = 0x106;
                AddItem(fpc);
            }
            else
            {
                Body = 400;
                Name = NameList.RandomName("male");

                PlateChest pc = new PlateChest();
                pc.Hue = 0x106;
                AddItem(pc);
            }

            Utility.AssignRandomHair(this);
        }

        public Pikeman(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
