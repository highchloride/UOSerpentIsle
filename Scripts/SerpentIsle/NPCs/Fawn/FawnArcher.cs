using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Items;

namespace Server.Mobiles
{
    class FawnArcher : BaseRanger
    {
        [Constructable]
        public FawnArcher() : base(AIType.AI_Archer, FightMode.Weakest, 15, 5, 0.1, 0.2)
        {
            Title = "the Fawn Guard";

            SetStr(1200, 1200);
            SetDex(250, 250);
            SetInt(61, 75);

            SetSkill(SkillName.Anatomy, 120.0, 120.0);
            SetSkill(SkillName.Archery, 120.0, 120.0);
            SetSkill(SkillName.Tactics, 120.0, 120.0);
            SetSkill(SkillName.MagicResist, 120.0, 120.0);

            AddItem(new Crossbow());

            AddItem(new SkullCap(902));

            ChainChest cc = new ChainChest();
            cc.Hue = 902;
            AddItem(cc);

            AddItem(new Doublet(243));

            AddItem(new ChainLegs());

            AddItem(new Shoes(902));

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

        public FawnArcher(Serial serial) : base(serial)
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
