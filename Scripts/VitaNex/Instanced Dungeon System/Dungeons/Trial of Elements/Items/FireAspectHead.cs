#region Header
//   Vorspire    _,-'/-'/  FireAspectHead.cs
//   .      __,-; ,'( '/
//    \.    `-.__`-._`:_,-._       _ , . ``
//     `:-._,------' ` _,`--` -: `_ , ` ,' :
//        `---..__,,--'  (C) 2017  ` -'. -'
//        #  Vita-Nex [http://core.vita-nex.com]  #
//  {o)xxx|===============-   #   -===============|xxx(o}
//        #        The MIT License (MIT)          #
#endregion

namespace Server.Items
{
	public sealed class FireAspectHead : ElementalAspectHead
	{
		[Constructable]
		public FireAspectHead(string name, int hue)
			: this(name, hue, 1)
		{ }

		[Constructable]
		public FireAspectHead(string name, int hue, int amount)
			: base(name, hue, amount)
		{ }

		public FireAspectHead(Serial serial)
			: base(serial)
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