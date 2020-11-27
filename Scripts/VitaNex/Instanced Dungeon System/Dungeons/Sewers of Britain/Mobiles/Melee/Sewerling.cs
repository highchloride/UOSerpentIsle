#region Header
//   Vorspire    _,-'/-'/  Sewerling.cs
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
	[CorpseName("a reeking corpse")]
	public class Sewerling : BaseCreature
	{
		[Constructable]
		public Sewerling()
			: base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
		{
			Name = "a sewerling";
			Body = 779;
			Hue = 2967;
			BaseSoundID = 422;

			SetStr(196, 220);
			SetDex(191, 215);
			SetInt(121, 145);

			SetHits(158, 172);

			SetDamage(15, 17);

			SetDamageType(ResistanceType.Physical, 100);

			SetResistance(ResistanceType.Physical, 20, 25);
			SetResistance(ResistanceType.Fire, 10, 20);
			SetResistance(ResistanceType.Cold, 15, 25);
			SetResistance(ResistanceType.Poison, 15, 25);
			SetResistance(ResistanceType.Energy, 15, 25);

			SetSkill(SkillName.MagicResist, 75.1, 100.0);
			SetSkill(SkillName.Tactics, 55.1, 80.0);
			SetSkill(SkillName.Wrestling, 55.1, 75.0);

			Fame = 1450;
			Karma = -1450;

			VirtualArmor = 28;
		}

        public Sewerling(Serial serial)
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