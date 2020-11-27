#region Header
//   Vorspire    _,-'/-'/  Chronos.cs
//   .      __,-; ,'( '/
//    \.    `-.__`-._`:_,-._       _ , . ``
//     `:-._,------' ` _,`--` -: `_ , ` ,' :
//        `---..__,,--'  (C) 2015  ` -'. -'
//        #  Vita-Nex [http://core.vita-nex.com]  #
//  {o)xxx|===============-   #   -===============|xxx(o}
//        #        The MIT License (MIT)          #
#endregion

namespace Server.Mobiles
{
	public class Chronos : BaseAspect
	{
		public override AspectFlags DefaultAspects
		{
			get { return AspectFlags.Time | AspectFlags.Illusion | AspectFlags.Energy | AspectFlags.Light; }
		}

		[Constructable]
		public Chronos()
			: base(AIType.AI_Mage, FightMode.Weakest, 16, 1, 0.1, 0.2)
		{
			Name = "Chronos";

			SetDamageType(ResistanceType.Physical, 50);
			SetDamageType(ResistanceType.Fire, 0);
			SetDamageType(ResistanceType.Cold, 25);
			SetDamageType(ResistanceType.Poison, 0);
			SetDamageType(ResistanceType.Energy, 25);

			SetResistance(ResistanceType.Physical, 75, 100);
			SetResistance(ResistanceType.Cold, 50, 75);
			SetResistance(ResistanceType.Energy, 50, 75);
		}

		public Chronos(Serial serial)
			: base(serial)
		{ }

		protected override int InitBody()
		{
			return 830;
		}

		public override int GetIdleSound()
		{
			return 1495;
		}

		public override int GetAngerSound()
		{
			return 1492;
		}

		public override int GetHurtSound()
		{
			return 1494;
		}

		public override int GetDeathSound()
		{
			return 1493;
		}

		/*
		public override int GetAttackSound()
		{
			return 0;
		}
		*/
		/*
		public override void OnDeath(Container c)
		{
			base.OnDeath(c);

			CreatureIdolHelper.CreateRandom(c);
		}
		*/
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