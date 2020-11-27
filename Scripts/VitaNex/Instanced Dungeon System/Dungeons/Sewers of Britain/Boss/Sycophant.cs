#region Header
//   Vorspire    _,-'/-'/  Sycophant.cs
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
	[CorpseName("rotting remains of Sycophant")]
	public class Sycophant : BaseAspect
	{
		public override AspectFlags DefaultAspects { get { return AspectFlags.Greed | AspectFlags.Famine; } }

		public override Poison PoisonImmune { get { return Poison.Greater; } }

		[Constructable]
		public Sycophant()
			: base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
		{
			Name = "Sycophant";
			Hue = 2967;

			SetDamageType(ResistanceType.Physical, 20);
			SetDamageType(ResistanceType.Fire, 20);
			SetDamageType(ResistanceType.Cold, 20);
			SetDamageType(ResistanceType.Poison, 20);
			SetDamageType(ResistanceType.Energy, 20);

			SetDamage(15, 20);
			SetHits(50000);
		}

		public Sycophant(Serial serial)
			: base(serial)
		{ }

		protected override int InitBody()
		{
			return 315;
		}

		public override void GenerateLoot()
		{
			base.GenerateLoot();

			AddLoot(LootPack.SuperBoss, 4);
		}

		public override WeaponAbility GetWeaponAbility()
		{
			return Utility.RandomBool() ? WeaponAbility.Dismount : WeaponAbility.ParalyzingBlow;
		}

		public override int GetAttackSound()
		{
			return 0x34C;
		}

		public override int GetHurtSound()
		{
			return 0x354;
		}

		public override int GetAngerSound()
		{
			return 0x34C;
		}

		public override int GetIdleSound()
		{
			return 0x34C;
		}

		public override int GetDeathSound()
		{
			return 0x354;
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