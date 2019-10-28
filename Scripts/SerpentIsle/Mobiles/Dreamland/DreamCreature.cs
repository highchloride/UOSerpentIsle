using System;
using Server.Items;
using Server.Services;

namespace Server.Mobiles
{
    [CorpseName("a dream creature corpse")]
    public class DreamCreature : BaseCreature
    {
        [Constructable]
        public DreamCreature() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a dream creature";
            Body = 738;

            switch(Utility.Random(2))
            {
                default:
                case 0:
                    Hue = 513;
                    break;
                case 1:
                    Hue = 608;
                    break;
            }

            SetStr(100);
            SetDex(108);
            SetInt(25);

            SetHits(150);

            SetDamage(10, 12);

            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Fire, 20);

            SetResistance(ResistanceType.Physical, 22);
            SetResistance(ResistanceType.Fire, 36);
            SetResistance(ResistanceType.Cold, 16);
            SetResistance(ResistanceType.Poison, 20);
            SetResistance(ResistanceType.Energy, 16);

            SetSkill(SkillName.Anatomy, 2.7);
            SetSkill(SkillName.MagicResist, 35.1);
            SetSkill(SkillName.Tactics, 44.2);
            SetSkill(SkillName.Wrestling, 35.4);
        }

        public DreamCreature(Serial serial) : base(serial)
        {
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average, 2);
        }

        public override int GetIdleSound()
        {
            return 846;
        }

        public override int GetAngerSound()
        {
            return 849;
        }

        public override int GetHurtSound()
        {
            return 852;
        }

        public override int GetDeathSound()
        {
            return 850;
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            var version = reader.ReadInt();
        }
    }
}
