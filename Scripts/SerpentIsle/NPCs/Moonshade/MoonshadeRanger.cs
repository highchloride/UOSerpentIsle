using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Items;

namespace Server.Mobiles
{
    class MoonshadeRanger : BaseRanger
    {
        [Constructable]
        public MoonshadeRanger() : base(AIType.AI_Melee, FightMode.Closest, 15, 1, 0.1, 0.2)
        {
            Title = "the Ranger";

            SetStr(1750, 1750);
            SetDex(150, 150);
            SetInt(61, 75);

            SetSkill(SkillName.MagicResist, 120.0, 120.0);
            SetSkill(SkillName.Swords, 120.0, 120.0);
            SetSkill(SkillName.Tactics, 120.0, 120.0);
            SetSkill(SkillName.Anatomy, 120.0, 120.0);

            StuddedArms arms = new StuddedArms()
            {
                Hue = 0x29B
            };

            AddItem(arms);

            StuddedLegs legs = new StuddedLegs()
            {
                Hue = 0x29B
            };

            AddItem(legs);

            AddItem(new FeatheredHat(0x21F));
            AddItem(new Shoes(0x21F));
            AddItem(new StuddedGorget());

            AddItem(new Longsword());

            if (Female = Utility.RandomBool())
            {
                Body = 401;
                Name = NameList.RandomName("female");

                FemaleStuddedChest fpc = new FemaleStuddedChest();
                fpc.Hue = 0xA8;
                AddItem(fpc);
            }
            else
            {
                Body = 400;
                Name = NameList.RandomName("male");

                StuddedChest pc = new StuddedChest();
                pc.Hue = 0xA8;
                AddItem(pc);
            }

            Utility.AssignRandomHair(this);
        }

        public MoonshadeRanger(Serial serial) : base(serial)
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
