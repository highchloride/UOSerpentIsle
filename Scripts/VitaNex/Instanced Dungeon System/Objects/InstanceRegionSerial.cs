#region Header
//   Vorspire    _,-'/-'/  InstanceRegionSerial.cs
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

namespace VitaNex.InstanceMaps
{
	public class InstanceRegionSerial : CryptoHashCode
	{
		public override string Value { get { return base.Value.Replace("-", String.Empty); } }

		public InstanceRegionSerial(string name, int mapIndex)
			: base(CryptoHashType.MD5, mapIndex + "|" + name)
		{ }

		public InstanceRegionSerial(GenericReader reader)
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