#region Header
//   Vorspire    _,-'/-'/  DungeonOptions.cs
//   .      __,-; ,'( '/
//    \.    `-.__`-._`:_,-._       _ , . ``
//     `:-._,------' ` _,`--` -: `_ , ` ,' :
//        `---..__,,--'  (C) 2017  ` -'. -'
//        #  Vita-Nex [http://core.vita-nex.com]  #
//  {o)xxx|===============-   #   -===============|xxx(o}
//        #        The MIT License (MIT)          #
#endregion

#region References
using Server;

using VitaNex.InstanceMaps;
#endregion

namespace VitaNex.Dungeons
{
	public class DungeonOptions : PropertyObject
	{
		public static double DefPetGiveDamageScalar = 0.66; // 33% decrease
		public static double DefPetTakeDamageScalar = 1.50; // 50% increase

		[CommandProperty(Instances.Access)]
		public DungeonRestrictions Restrictions { get; set; }

		[CommandProperty(Instances.Access)]
		public DungeonRules Rules { get; set; }

		[CommandProperty(Instances.Access)]
		public DungeonSounds Sounds { get; set; }

		[CommandProperty(Instances.Access)]
		public double PetGiveDamageScalar { get; set; }

		[CommandProperty(Instances.Access)]
		public double PetTakeDamageScalar { get; set; }

		public DungeonOptions()
		{
			Restrictions = new DungeonRestrictions();
			Rules = new DungeonRules();
			Sounds = new DungeonSounds();

			PetGiveDamageScalar = DefPetGiveDamageScalar;
			PetTakeDamageScalar = DefPetTakeDamageScalar;
		}

		public DungeonOptions(GenericReader reader)
			: base(reader)
		{ }

		public override void Clear()
		{
			Restrictions.Clear();
			Rules.Clear();
			Sounds.Clear();

			PetGiveDamageScalar = DefPetGiveDamageScalar;
			PetTakeDamageScalar = DefPetTakeDamageScalar;
		}

		public override void Reset()
		{
			Restrictions.Reset();
			Rules.Reset();
			Sounds.Reset();

			PetGiveDamageScalar = DefPetGiveDamageScalar;
			PetTakeDamageScalar = DefPetTakeDamageScalar;
		}

		public override string ToString()
		{
			return "Options";
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			var version = writer.SetVersion(1);

			switch (version)
			{
				case 1:
				{
					writer.Write(PetGiveDamageScalar);
					writer.Write(PetTakeDamageScalar);
				}
					goto case 0;
				case 0:
				{
					writer.WriteBlock(w => w.WriteType(Restrictions, t => Restrictions.Serialize(w)));
					writer.WriteBlock(w => w.WriteType(Rules, t => Rules.Serialize(w)));
					writer.WriteBlock(w => w.WriteType(Sounds, t => Sounds.Serialize(w)));
				}
					break;
			}
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			var version = reader.GetVersion();

			switch (version)
			{
				case 1:
				{
					PetGiveDamageScalar = reader.ReadDouble();
					PetTakeDamageScalar = reader.ReadDouble();
				}
					goto case 0;
				case 0:
				{
					reader.ReadBlock(r => Restrictions = r.ReadTypeCreate<DungeonRestrictions>(r));
					reader.ReadBlock(r => Rules = r.ReadTypeCreate<DungeonRules>(r));
					reader.ReadBlock(r => Sounds = r.ReadTypeCreate<DungeonSounds>(r));
				}
					break;
			}

			if (Restrictions == null)
			{
				Restrictions = new DungeonRestrictions();
			}

			if (Rules == null)
			{
				Rules = new DungeonRules();
			}

			if (Sounds == null)
			{
				Sounds = new DungeonSounds();
			}
		}
	}
}