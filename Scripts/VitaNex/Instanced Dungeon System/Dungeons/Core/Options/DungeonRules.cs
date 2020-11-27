#region Header
//   Vorspire    _,-'/-'/  DungeonRules.cs
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
	public class DungeonRules : PropertyObject
	{
		[CommandProperty(Instances.Access)]
		public bool AllowSpeech { get; set; }

		[CommandProperty(Instances.Access)]
		public bool AllowBeneficial { get; set; }

		[CommandProperty(Instances.Access)]
		public bool AllowHarmful { get; set; }

		[CommandProperty(Instances.Access)]
		public bool AllowHousing { get; set; }

		[CommandProperty(Instances.Access)]
		public bool AllowSpawn { get; set; }

		[CommandProperty(Instances.Access)]
		public bool CanDie { get; set; }

		[CommandProperty(Instances.Access)]
		public bool CanHeal { get; set; }

		[CommandProperty(Instances.Access)]
		public bool CanBeDamaged { get; set; }

		[CommandProperty(Instances.Access)]
		public bool AllowPets { get; set; }

		[CommandProperty(Instances.Access)]
		public bool CanMount { get; set; }

		[CommandProperty(Instances.Access)]
		public bool CanFly { get; set; }

		[CommandProperty(Instances.Access)]
		public bool CanMountEthereal { get; set; }

		[CommandProperty(Instances.Access)]
		public bool CanMoveThrough { get; set; }

		[CommandProperty(Instances.Access)]
		public bool CanResurrect { get; set; }

		[CommandProperty(Instances.Access)]
		public bool CanUseStuckMenu { get; set; }

		public DungeonRules()
		{
			AllowBeneficial = true;
			AllowHarmful = true;
			AllowHousing = false;
			AllowPets = true;
			AllowSpawn = true;
			AllowSpeech = true;
			CanBeDamaged = true;
			CanDie = true;
			CanHeal = true;
			CanFly = true;
			CanMount = true;
			CanMountEthereal = true;
			CanMoveThrough = true;
			CanResurrect = true;
			CanUseStuckMenu = true;
		}

		public DungeonRules(GenericReader reader)
			: base(reader)
		{ }

		public override string ToString()
		{
			return "Rules";
		}

		public override void Clear()
		{
			AllowBeneficial = false;
			AllowHarmful = false;
			AllowHousing = false;
			AllowPets = false;
			AllowSpawn = false;
			AllowSpeech = false;
			CanBeDamaged = false;
			CanDie = false;
			CanHeal = false;
			CanFly = false;
			CanMount = false;
			CanMountEthereal = false;
			CanMoveThrough = false;
			CanResurrect = false;
			CanUseStuckMenu = false;
		}

		public override void Reset()
		{
			AllowBeneficial = true;
			AllowHarmful = true;
			AllowHousing = false;
			AllowPets = true;
			AllowSpawn = true;
			AllowSpeech = true;
			CanBeDamaged = true;
			CanDie = true;
			CanHeal = true;
			CanFly = true;
			CanMount = true;
			CanMountEthereal = true;
			CanMoveThrough = true;
			CanResurrect = true;
			CanUseStuckMenu = true;
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			var version = writer.SetVersion(0);

			switch (version)
			{
				case 0:
				{
					writer.Write(AllowBeneficial);
					writer.Write(AllowHarmful);
					writer.Write(AllowHousing);
					writer.Write(AllowPets);
					writer.Write(AllowSpawn);
					writer.Write(AllowSpeech);
					writer.Write(CanBeDamaged);
					writer.Write(CanDie);
					writer.Write(CanHeal);
					writer.Write(CanFly);
					writer.Write(CanMount);
					writer.Write(CanMountEthereal);
					writer.Write(CanMoveThrough);
					writer.Write(CanResurrect);
					writer.Write(CanUseStuckMenu);
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
				case 0:
				{
					AllowBeneficial = reader.ReadBool();
					AllowHarmful = reader.ReadBool();
					AllowHousing = reader.ReadBool();
					AllowPets = reader.ReadBool();
					AllowSpawn = reader.ReadBool();
					AllowSpeech = reader.ReadBool();
					CanBeDamaged = reader.ReadBool();
					CanDie = reader.ReadBool();
					CanHeal = reader.ReadBool();
					CanFly = reader.ReadBool();
					CanMount = reader.ReadBool();
					CanMountEthereal = reader.ReadBool();
					CanMoveThrough = reader.ReadBool();
					CanResurrect = reader.ReadBool();
					CanUseStuckMenu = reader.ReadBool();
				}
					break;
			}
		}
	}
}