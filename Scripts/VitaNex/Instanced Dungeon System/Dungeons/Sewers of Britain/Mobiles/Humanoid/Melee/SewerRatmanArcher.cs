
#region Header
//   Vorspire    _,-'/-'/  SewerRatmanArcher.cs
//   .      __,-; ,'( '/
//    \.    `-.__`-._`:_,-._       _ , . ``
//     `:-._,------' ` _,`--` -: `_ , ` ,' :
//        `---..__,,--'  (C) 2017  ` -'. -'
//        #  Vita-Nex [http://core.vita-nex.com]  #
//  {o)xxx|===============-   #   -===============|xxx(o}
//        #        The MIT License (MIT)          #
#endregion

#region References
using Server.Items;
using Server.Misc;
#endregion

namespace Server.Mobiles
{
	[CorpseName("a sewer ratman archer corpse")]
	public class SewerRatmanArcher : BaseCreature
	{
		public override InhumanSpeech SpeechType { get { return InhumanSpeech.Ratman; } }

		public override bool CanRummageCorpses { get { return true; } }

		public override bool AlwaysMurderer { get { return true; } }

		[Constructable]
		public SewerRatmanArcher()
			: base(AIType.AI_Archer, FightMode.Closest, 10, 1, 0.2, 0.4)
		{
			Name = NameList.RandomName("ratman");
			Title = "the Sewer Guard";
			Body = 0x8E;
			BaseSoundID = 437;

			SetStr(246, 280);
			SetDex(201, 230);
			SetInt(216, 240);

			SetHits(188, 208);

			SetDamage(14, 20);

			SetDamageType(ResistanceType.Physical, 100);

			SetResistance(ResistanceType.Physical, 40, 55);
			SetResistance(ResistanceType.Fire, 10, 20);
			SetResistance(ResistanceType.Cold, 10, 20);
			SetResistance(ResistanceType.Poison, 10, 20);
			SetResistance(ResistanceType.Energy, 10, 20);

			SetSkill(SkillName.Anatomy, 60.2, 100.0);
			SetSkill(SkillName.Archery, 80.1, 90.0);
			SetSkill(SkillName.MagicResist, 65.1, 90.0);
			SetSkill(SkillName.Tactics, 50.1, 75.0);
			SetSkill(SkillName.Wrestling, 50.1, 75.0);

			Fame = 6500;
			Karma = -6500;

			VirtualArmor = 56;

			AddItem(new Bow());
		}

        public SewerRatmanArcher(Serial serial)
			: base(serial)
		{ }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average, 2);
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