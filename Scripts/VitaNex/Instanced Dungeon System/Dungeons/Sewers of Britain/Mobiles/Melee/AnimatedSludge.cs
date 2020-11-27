#region Header
//   Vorspire    _,-'/-'/  AnimatedSludge.cs
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
	[CorpseName("a pool of sludge")]
	public class AnimatedSludge : BaseCreature
	{
		public override Poison PoisonImmune { get { return Poison.Lethal; } }

		[Constructable]
		public AnimatedSludge()
			: base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
		{
			Name = "animated sludge";
			Body = 319;
			Hue = 2967;
			BaseSoundID = 898;

			SetStr(161, 170);
			SetDex(161, 170);
			SetInt(110);

			SetMana(10);

			SetDamage(13, 19);

			SetDamageType(ResistanceType.Physical, 50);
			SetDamageType(ResistanceType.Poison, 50);

			SetResistance(ResistanceType.Physical, 90);
			SetResistance(ResistanceType.Poison, 100);

			SetSkill(SkillName.Tactics, 50.0);
			SetSkill(SkillName.Wrestling, 50.1, 60.0);

			Fame = 1000;
			Karma = -1000;

			VirtualArmor = 24;
		}

        public AnimatedSludge(Serial serial)
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