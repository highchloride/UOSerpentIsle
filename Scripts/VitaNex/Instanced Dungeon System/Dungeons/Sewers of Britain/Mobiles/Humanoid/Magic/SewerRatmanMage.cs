#region Header
//   Vorspire    _,-'/-'/  SewerRatmanMage.cs
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
	[CorpseName("a glowing ratman corpse")]
	public class SewerRatmanMage : BaseCreature
	{
		public override InhumanSpeech SpeechType { get { return InhumanSpeech.Ratman; } }

		public override bool CanRummageCorpses { get { return true; } }

		public override bool AlwaysMurderer { get { return true; } }

		[Constructable]
		public SewerRatmanMage()
			: base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
		{
			Name = NameList.RandomName("ratman");
			Title = "the Sewer Guard";
			Body = 0x8F;
			BaseSoundID = 437;

			SetStr(246, 280);
			SetDex(201, 230);
			SetInt(286, 310);

			SetHits(188, 208);

			SetDamage(17, 24);

			SetDamageType(ResistanceType.Physical, 100);

			SetResistance(ResistanceType.Physical, 40, 45);
			SetResistance(ResistanceType.Fire, 10, 20);
			SetResistance(ResistanceType.Cold, 10, 20);
			SetResistance(ResistanceType.Poison, 10, 20);
			SetResistance(ResistanceType.Energy, 10, 20);

			SetSkill(SkillName.EvalInt, 70.1, 80.0);
			SetSkill(SkillName.Magery, 70.1, 80.0);
			SetSkill(SkillName.MagicResist, 65.1, 90.0);
			SetSkill(SkillName.Tactics, 50.1, 75.0);
			SetSkill(SkillName.Wrestling, 50.1, 75.0);

			Fame = 7500;
			Karma = -7500;

			VirtualArmor = 44;
		}

	    public SewerRatmanMage(Serial serial)
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