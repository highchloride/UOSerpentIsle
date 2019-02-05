using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Items;

namespace Server.Mobiles
{
    class FawnGuard : BaseRanger
    {
        [Constructable]
        public FawnGuard() : base(AIType.AI_Melee, FightMode.Closest, 15, 1, 0.1, 0.2)
        {
            Title = "the Fawn Guard";

            SetStr(1750, 1750);
            SetDex(150, 150);
            SetInt(61, 75);

            SetSkill(SkillName.MagicResist, 120.0, 120.0);
            SetSkill(SkillName.Swords, 120.0, 120.0);
            SetSkill(SkillName.Tactics, 120.0, 120.0);
            SetSkill(SkillName.Anatomy, 120.0, 120.0);

            SkullCap sc = new SkullCap();
            sc.Hue = 902;
            AddItem(sc);

            ChainChest cc = new ChainChest();
            cc.Hue = 902;
            AddItem(cc);

            Doublet doublet = new Doublet();
            doublet.Hue = 243;
            AddItem(doublet);

            AddItem(new ChainLegs());

            Shoes shoes = new Shoes();
            shoes.Hue = 902;
            AddItem(shoes);

            AddItem(new Broadsword());

            if (Female = Utility.RandomBool())
            {
                Body = 401;
                Name = NameList.RandomName("female");
            }
            else
            {
                Body = 400;
                Name = NameList.RandomName("male");
            }

            Utility.AssignRandomHair(this);
        }

        public FawnGuard(Serial serial) : base(serial)
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
