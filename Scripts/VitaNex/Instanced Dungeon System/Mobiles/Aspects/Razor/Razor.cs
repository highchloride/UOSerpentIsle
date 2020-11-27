#region Header
// **********
// ReliveUO - Razor.cs
// **********
#endregion

namespace Server.Mobiles
{
	public class Razor : BaseAspect
	{
		public override AspectFlags DefaultAspects
		{
			get { return AspectFlags.Elements | AspectFlags.Death | AspectFlags.Chaos; }
		}

		[Constructable]
		public Razor()
			: base(AIType.AI_Melee, FightMode.Closest, 16, 1, 0.1, 0.2)
		{
			Name = "Razor";

			SetDamageType(ResistanceType.Physical, 50);
			SetDamageType(ResistanceType.Fire, 0);
			SetDamageType(ResistanceType.Cold, 0);
			SetDamageType(ResistanceType.Poison, 25);
			SetDamageType(ResistanceType.Energy, 25);

			SetResistance(ResistanceType.Physical, 75, 100);
			SetResistance(ResistanceType.Poison, 50, 75);
			SetResistance(ResistanceType.Energy, 50, 75);
		}

		public Razor(Serial serial)
			: base(serial)
		{ }

		protected override int InitBody()
		{
			return 757;
		}

		public override int GetIdleSound()
		{
			return 466;
		}

		public override int GetAngerSound()
		{
			return 467;
		}

		public override int GetDeathSound()
		{
			return 468;
		}

		public override int GetHurtSound()
		{
			return 469;
		}

		public override int GetAttackSound()
		{
			return Utility.RandomList(568, 569, 571, 572);
		}
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