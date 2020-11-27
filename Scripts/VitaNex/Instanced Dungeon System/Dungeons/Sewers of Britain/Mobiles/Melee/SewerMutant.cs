#region Header
//   Vorspire    _,-'/-'/  SewerMutant.cs
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
	[CorpseName("a sewer mutant corpse")]
	public class SewerMutant : BaseCreature
	{
		[Constructable]
		public SewerMutant()
			: base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
		{
			Name = "a sewer mutant";
			Body = 307;
			Hue = 2967;
			BaseSoundID = 422;

			SetStr(241, 265);
			SetDex(201, 225);
			SetInt(156, 180);

			SetHits(185, 199);

			SetDamage(22, 27);

			SetDamageType(ResistanceType.Physical, 0);
			SetDamageType(ResistanceType.Fire, 40);
			SetDamageType(ResistanceType.Energy, 60);

			SetResistance(ResistanceType.Physical, 45, 55);
			SetResistance(ResistanceType.Fire, 25, 35);
			SetResistance(ResistanceType.Cold, 25, 35);
			SetResistance(ResistanceType.Poison, 10, 20);
			SetResistance(ResistanceType.Energy, 30, 40);

			SetSkill(SkillName.MagicResist, 45.1, 70.0);
			SetSkill(SkillName.Tactics, 67.6, 92.5);
			SetSkill(SkillName.Wrestling, 60.1, 80.0);

			Fame = 1500;
			Karma = -1500;

			VirtualArmor = 27;
		}

        public SewerMutant(Serial serial)
			: base(serial)
		{ }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average, 1);
        }

		public override WeaponAbility GetWeaponAbility()
		{
			return WeaponAbility.Dismount;
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