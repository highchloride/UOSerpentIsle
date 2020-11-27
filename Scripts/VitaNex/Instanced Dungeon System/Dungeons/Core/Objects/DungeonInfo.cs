#region Header
//   Vorspire    _,-'/-'/  DungeonInfo.cs
//   .      __,-; ,'( '/
//    \.    `-.__`-._`:_,-._       _ , . ``
//     `:-._,------' ` _,`--` -: `_ , ` ,' :
//        `---..__,,--'  (C) 2017  ` -'. -'
//        #  Vita-Nex [http://core.vita-nex.com]  #
//  {o)xxx|===============-   #   -===============|xxx(o}
//        #        The MIT License (MIT)          #
#endregion

#region References
using System;

using Server;

using VitaNex.InstanceMaps;
#endregion

namespace VitaNex.Dungeons
{
	[PropertyObject]
	public sealed class DungeonInfo : IDungeon
	{
		[CommandProperty(Instances.Access, true)]
		public Type Type { get; private set; }

		[CommandProperty(Instances.Access, true)]
		public DungeonID ID { get; private set; }

		[CommandProperty(Instances.Access, true)]
		public Map MapParent { get; private set; }

		[CommandProperty(Instances.Access, true)]
		public Point3D Entrance { get; private set; }

		[CommandProperty(Instances.Access, true)]
		public Point3D Exit { get; private set; }

		[CommandProperty(Instances.Access, true)]
		public TimeSpan Duration { get; private set; }

		[CommandProperty(Instances.Access, true)]
		public TimeSpan Lockout { get; private set; }

		[CommandProperty(Instances.Access, true)]
		public int GroupMax { get; private set; }

		[CommandProperty(Instances.Access, true)]
		public string Name { get; private set; }

		[CommandProperty(Instances.Access, true)]
		public string Desc { get; private set; }

		[CommandProperty(Instances.Access, true)]
		public Expansion Expansion { get; private set; }

		public DungeonInfo(Dungeon dungeon)
			: this(
				dungeon.GetType(),
				dungeon.ID,
				dungeon.MapParent,
				dungeon.Entrance,
				dungeon.Exit,
				dungeon.Duration,
				dungeon.Lockout,
				dungeon.GroupMax,
				dungeon.Name,
				dungeon.Desc,
				dungeon.Expansion)
		{ }

		public DungeonInfo(
			Type type,
			DungeonID id,
			Map mapParent,
			Point3D entrance,
			Point3D exit,
			TimeSpan duration,
			TimeSpan lockout,
			int groupSize,
			string name,
			string desc,
			Expansion expansion)
		{
			Type = type;

			ID = id;

			MapParent = mapParent;

			Entrance = entrance;
			Exit = exit;

			GroupMax = groupSize;

			Duration = duration;
			Lockout = lockout;

			Name = name;
			Desc = desc;

			Expansion = expansion;
		}

		public override string ToString()
		{
			return Name;
		}
	}
}