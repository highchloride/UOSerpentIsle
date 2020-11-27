#region Header
//   Vorspire    _,-'/-'/  FecalConstruct.cs
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
#endregion

namespace Server.Mobiles
{
	[CorpseName("a fecal construct corpse")]
	public class FecalConstruct : BaseCreature
	{
		public override bool BleedImmune { get { return true; } }

		[Constructable]
		public FecalConstruct()
			: base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
		{
			Name = "a fecal construct";
			Body = 304;
			Hue = 2967;
			BaseSoundID = 684;

			SetStr(276, 300);
			SetDex(151, 175);
			SetInt(146, 170);

			SetHits(206, 220);

			SetDamage(28, 32);

			SetDamageType(ResistanceType.Physical, 100);

			SetResistance(ResistanceType.Physical, 50, 60);
			SetResistance(ResistanceType.Fire, 25, 35);
			SetResistance(ResistanceType.Cold, 15, 25);
			SetResistance(ResistanceType.Poison, 60, 70);
			SetResistance(ResistanceType.Energy, 30, 40);

			SetSkill(SkillName.MagicResist, 50.1, 75.0);
			SetSkill(SkillName.Tactics, 55.1, 80.0);
			SetSkill(SkillName.Wrestling, 60.1, 70.0);

			Fame = 1000;
			Karma = -1800;

			VirtualArmor = 34;
		}

        public FecalConstruct(Serial serial)
			: base(serial)
		{ }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average, 1);
        }

		public override WeaponAbility GetWeaponAbility()
		{
			return WeaponAbility.BleedAttack;
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