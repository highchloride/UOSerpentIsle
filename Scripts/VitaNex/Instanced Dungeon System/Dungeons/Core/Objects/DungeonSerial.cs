#region Header
//   Vorspire    _,-'/-'/  DungeonSerial.cs
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

using VitaNex.Crypto;
#endregion

namespace VitaNex.Dungeons
{
	public sealed class DungeonSerial : CryptoHashCode
	{
		public override string Value { get { return base.Value.Replace("-", String.Empty); } }

		public DungeonSerial()
			: base(CryptoHashType.MD5, TimeStamp.UtcNow + "+" + Utility.RandomDouble())
		{ }

		public DungeonSerial(GenericReader reader)
			: base(reader)
		{ }

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.SetVersion(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			reader.GetVersion();
		}
	}
}