#region Header
//   Vorspire    _,-'/-'/  Restrictions.cs
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
	public class DungeonRestrictions : PropertyObject
	{
		[CommandProperty(Instances.Access)]
		public DungeonItemRestrictions Items { get; protected set; }

		[CommandProperty(Instances.Access)]
		public DungeonPetRestrictions Pets { get; protected set; }

		[CommandProperty(Instances.Access)]
		public DungeonSkillRestrictions Skills { get; protected set; }

		[CommandProperty(Instances.Access)]
		public DungeonSpellRestrictions Spells { get; protected set; }

		public DungeonRestrictions()
		{
			Items = new DungeonItemRestrictions();
			Pets = new DungeonPetRestrictions();
			Skills = new DungeonSkillRestrictions();
			Spells = new DungeonSpellRestrictions();
		}

		public DungeonRestrictions(GenericReader reader)
			: base(reader)
		{ }

		public override void Clear()
		{
			Items.Clear();
			Pets.Clear();
			Skills.Clear();
			Spells.Clear();
		}

		public override void Reset()
		{
			Items.Reset(false);
			Pets.Reset(false);
			Skills.Reset(false);
			Spells.Reset(false);
		}

		public override string ToString()
		{
			return "Restrictions";
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			var version = writer.SetVersion(0);

			switch (version)
			{
				case 0:
				{
					writer.WriteBlock(w => w.WriteType(Items, t => Items.Serialize(w)));
					writer.WriteBlock(w => w.WriteType(Pets, t => Pets.Serialize(w)));
					writer.WriteBlock(w => w.WriteType(Skills, t => Skills.Serialize(w)));
					writer.WriteBlock(w => w.WriteType(Spells, t => Spells.Serialize(w)));
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
					reader.ReadBlock(r => Items = r.ReadTypeCreate<DungeonItemRestrictions>(r));
					reader.ReadBlock(r => Pets = r.ReadTypeCreate<DungeonPetRestrictions>(r));
					reader.ReadBlock(r => Skills = r.ReadTypeCreate<DungeonSkillRestrictions>(r));
					reader.ReadBlock(r => Spells = r.ReadTypeCreate<DungeonSpellRestrictions>(r));
				}
					break;
			}

			if (Items == null)
			{
				Items = new DungeonItemRestrictions();
			}

			if (Pets == null)
			{
				Pets = new DungeonPetRestrictions();
			}

			if (Skills == null)
			{
				Skills = new DungeonSkillRestrictions();
			}

			if (Spells == null)
			{
				Spells = new DungeonSpellRestrictions();
			}
		}
	}
}