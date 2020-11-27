#region Header
//   Vorspire    _,-'/-'/  SewerFiend.cs
//   .      __,-; ,'( '/
//    \.    `-.__`-._`:_,-._       _ , . ``
//     `:-._,------' ` _,`--` -: `_ , ` ,' :
//        `---..__,,--'  (C) 2017  ` -'. -'
//        #  Vita-Nex [http://core.vita-nex.com]  #
//  {o)xxx|===============-   #   -===============|xxx(o}
//        #        The MIT License (MIT)          #
#endregion

namespace Server.Mobiles
{
	[CorpseName("a sewer fiend corpse")]
	public class SewerFiend : BaseCreature
	{
		public override bool BleedImmune { get { return true; } }

		[Constructable]
		public SewerFiend()
			: base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
		{
			Name = "a sewer fiend";
			Body = 305;
			Hue = 2967;
			BaseSoundID = 224;

			SetStr(261, 285);
			SetDex(141, 165);
			SetInt(146, 170);

			SetHits(197, 211);

			SetDamage(25, 31);

			SetDamageType(ResistanceType.Physical, 85);
			SetDamageType(ResistanceType.Poison, 15);

			SetResistance(ResistanceType.Physical, 35, 45);
			SetResistance(ResistanceType.Fire, 25, 35);
			SetResistance(ResistanceType.Cold, 15, 25);
			SetResistance(ResistanceType.Poison, 5, 15);
			SetResistance(ResistanceType.Energy, 30, 40);

			SetSkill(SkillName.MagicResist, 40.1, 55.0);
			SetSkill(SkillName.Tactics, 45.1, 70.0);
			SetSkill(SkillName.Wrestling, 50.1, 70.0);

			Fame = 1500;
			Karma = -1500;

			VirtualArmor = 24;
		}

        public SewerFiend(Serial serial)
			: base(serial)
		{ }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average, 1);
        }

		public override int GetDeathSound()
		{
			return 1218;
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			reader.ReadInt();
		}
	}
}