#region Header
//   Vorspire    _,-'/-'/  SludgeElemental.cs
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
	[CorpseName("a sludge elemental corpse")]
	public class SludgeElemental : BaseCreature
	{
		public override bool BleedImmune { get { return true; } }

		public override Poison HitPoison { get { return Poison.Lethal; } }
		public override double HitPoisonChance { get { return 0.6; } }

		[Constructable]
		public SludgeElemental()
			: base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
		{
			Name = "a sludge elemental";
			Body = 16;
			Hue = 2967;
			BaseSoundID = 278;

			SetStr(426, 455);
			SetDex(166, 185);
			SetInt(371, 395);

			SetHits(296, 313);

			SetDamage(19, 25);

			SetDamageType(ResistanceType.Physical, 25);
			SetDamageType(ResistanceType.Cold, 50);
			SetDamageType(ResistanceType.Energy, 25);

			SetResistance(ResistanceType.Physical, 45, 55);
			SetResistance(ResistanceType.Fire, 20, 30);
			SetResistance(ResistanceType.Cold, 40, 50);
			SetResistance(ResistanceType.Poison, 30, 40);
			SetResistance(ResistanceType.Energy, 10, 20);

			SetSkill(SkillName.Anatomy, 30.3, 60.0);
			SetSkill(SkillName.EvalInt, 70.1, 85.0);
			SetSkill(SkillName.Magery, 70.1, 85.0);
			SetSkill(SkillName.MagicResist, 60.1, 75.0);
			SetSkill(SkillName.Tactics, 80.1, 90.0);
			SetSkill(SkillName.Wrestling, 70.1, 90.0);

			Fame = 10000;
			Karma = -10000;

			VirtualArmor = 40;
		}

        public SludgeElemental(Serial serial)
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