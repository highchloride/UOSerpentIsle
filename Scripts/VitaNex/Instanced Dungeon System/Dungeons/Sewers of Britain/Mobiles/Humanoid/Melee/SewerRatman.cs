#region Header
//   Vorspire    _,-'/-'/  SewerRatman.cs
//   .      __,-; ,'( '/
//    \.    `-.__`-._`:_,-._       _ , . ``
//     `:-._,------' ` _,`--` -: `_ , ` ,' :
//        `---..__,,--'  (C) 2017  ` -'. -'
//        #  Vita-Nex [http://core.vita-nex.com]  #
//  {o)xxx|===============-   #   -===============|xxx(o}
//        #        The MIT License (MIT)          #
#endregion

#region References
using Server.Misc;
#endregion

namespace Server.Mobiles
{
	[CorpseName("a sewer ratman's corpse")]
	public class SewerRatman : BaseCreature
	{
		public override InhumanSpeech SpeechType { get { return InhumanSpeech.Ratman; } }

		public override bool CanRummageCorpses { get { return true; } }

		public override bool AlwaysMurderer { get { return true; } }

		[Constructable]
		public SewerRatman()
			: base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
		{
			Name = NameList.RandomName("ratman");
			Title = "the Sewer Guard";
			Body = 42;
			BaseSoundID = 437;

			SetStr(196, 220);
			SetDex(181, 200);
			SetInt(136, 160);

			SetHits(158, 172);

			SetDamage(14, 15);

			SetDamageType(ResistanceType.Physical, 100);

			SetResistance(ResistanceType.Physical, 25, 30);
			SetResistance(ResistanceType.Fire, 10, 20);
			SetResistance(ResistanceType.Cold, 10, 20);
			SetResistance(ResistanceType.Poison, 10, 20);
			SetResistance(ResistanceType.Energy, 10, 20);

			SetSkill(SkillName.MagicResist, 35.1, 60.0);
			SetSkill(SkillName.Tactics, 50.1, 75.0);
			SetSkill(SkillName.Wrestling, 50.1, 75.0);

			Fame = 1500;
			Karma = -1500;

			VirtualArmor = 28;
		}

        public SewerRatman(Serial serial)
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