using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Items;

namespace Server.Mobiles
{
    class PikemanArcher : BaseRanger
    {
        [Constructable]
        public PikemanArcher() : base(AIType.AI_Archer, FightMode.Weakest, 15, 5, 0.1, 0.2)
        {
            Title = "the Pikeman";

            SetStr(1200, 1200);
            SetDex(250, 250);
            SetInt(61, 75);

            SetSkill(SkillName.Anatomy, 120.0, 120.0);
            SetSkill(SkillName.Archery, 120.0, 120.0);
            SetSkill(SkillName.Tactics, 120.0, 120.0);
            SetSkill(SkillName.MagicResist, 120.0, 120.0);

            AddItem(new ChainCoif());
            AddItem(new PlateArms());
            AddItem(new PlateLegs());
            AddItem(new Bow());

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

        public PikemanArcher(Serial serial) : base(serial)
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
