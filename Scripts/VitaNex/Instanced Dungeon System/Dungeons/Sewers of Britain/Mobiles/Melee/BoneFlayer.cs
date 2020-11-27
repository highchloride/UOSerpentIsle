#region Header
//   Vorspire    _,-'/-'/  BoneFlayer.cs
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
	[CorpseName("a bone flayer corpse")]
	public class BoneFlayer : BaseCreature
	{
		[Constructable]
		public BoneFlayer()
			: base(AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4)
		{
			Name = "a bone flayer";
			Body = 302;
			Hue = 2967;
			BaseSoundID = 959;

			SetStr(141, 165);
			SetDex(191, 215);
			SetInt(126, 150);

			SetHits(131, 145);

			SetDamage(13, 15);

			SetDamageType(ResistanceType.Physical, 100);

			SetResistance(ResistanceType.Physical, 5, 10);
			SetResistance(ResistanceType.Cold, 10, 20);
			SetResistance(ResistanceType.Energy, 5, 10);

			SetSkill(SkillName.MagicResist, 30.1, 45.0);
			SetSkill(SkillName.Tactics, 45.1, 70.0);
			SetSkill(SkillName.Wrestling, 40.1, 60.0);

			Fame = 300;
			Karma = 0;

			VirtualArmor = 12;
		}

        public BoneFlayer(Serial serial)
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