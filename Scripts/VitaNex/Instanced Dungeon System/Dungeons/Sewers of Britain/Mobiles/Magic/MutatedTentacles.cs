#region Header
//   Vorspire    _,-'/-'/  MutatedTentacles.cs
//   .      __,-; ,'( '/
//    \.    `-.__`-._`:_,-._       _ , . ``
//     `:-._,------' ` _,`--` -: `_ , ` ,' :
//        `---..__,,--'  (C) 2015  ` -'. -'
//        #  Vita-Nex [http://core.vita-nex.com]  #
//  {o)xxx|===============-   #   -===============|xxx(o}
//        #        The MIT License (MIT)          #
#endregion

namespace Server.Mobiles
{
	[CorpseName("mutated shadowling remains")]
	public class MutatedTentacles : BaseCreature
	{
		public override Poison PoisonImmune { get { return Poison.Greater; } }

		[Constructable]
		public MutatedTentacles()
			: base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
		{
			Name = "mutated shadowling";
			Body = 66;
			Hue = 2967;
			BaseSoundID = 352;

			SetStr(196, 220);
			SetDex(166, 185);
			SetInt(216, 230);

			SetHits(158, 172);
			SetMana(100);

			SetDamage(16, 22);

			SetDamageType(ResistanceType.Physical, 40);
			SetDamageType(ResistanceType.Poison, 60);

			SetResistance(ResistanceType.Physical, 25, 35);
			SetResistance(ResistanceType.Fire, 10, 20);
			SetResistance(ResistanceType.Cold, 10, 20);
			SetResistance(ResistanceType.Poison, 60, 80);
			SetResistance(ResistanceType.Energy, 10, 20);

			SetSkill(SkillName.MagicResist, 15.1, 20.0);
			SetSkill(SkillName.Tactics, 65.1, 80.0);
			SetSkill(SkillName.Wrestling, 65.1, 80.0);

			Fame = 3000;
			Karma = -3000;

			VirtualArmor = 28;
		}

        public MutatedTentacles(Serial serial)
			: base(serial)
		{ }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average, 1);
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
