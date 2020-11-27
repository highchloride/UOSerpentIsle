#region Header
//   Vorspire    _,-'/-'/  AspectFlags.cs
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
#endregion

namespace Server.Mobiles
{
	[Flags]
	public enum AspectFlags : ulong
	{
		None = 0x0,
		Time = 0x1,
		Light = 0x2,
		Darkness = 0x4,
		Faith = 0x8,
		Despair = 0x10,
		Illusion = 0x20,
		Life = 0x40,
		Death = 0x80,
		Elements = 0x100,
		Greed = 0x200,
		Famine = 0x400,
		Tech = 0x800,
		Decay = 0x1000,
		Nature = 0x2000,

		Earth = 0x4000,
		Fire = 0x8000,
		Frost = 0x10000,
		Poison = 0x20000,
		Energy = 0x40000,
		Chaos = 0x80000,

		All = ~None
	}
}