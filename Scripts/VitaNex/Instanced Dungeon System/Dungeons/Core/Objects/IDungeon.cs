#region Header
//   Vorspire    _,-'/-'/  IDungeon.cs
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
#endregion

namespace VitaNex.Dungeons
{
	public interface IDungeon
	{
		DungeonID ID { get; }

		Map MapParent { get; }

		Point3D Entrance { get; }
		Point3D Exit { get; }

		TimeSpan Duration { get; }
		TimeSpan Lockout { get; }

		int GroupMax { get; }

		string Name { get; }
		string Desc { get; }
	}
}