#region Header
//   Vorspire    _,-'/-'/  PoisonAspectHead.cs
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
	public sealed class PoisonAspectHead : ElementalAspectHead
	{
		[Constructable]
		public PoisonAspectHead(string name, int hue)
			: this(name, hue, 1)
		{ }

		[Constructable]
		public PoisonAspectHead(string name, int hue, int amount)
			: base(name, hue, amount)
		{ }

		public PoisonAspectHead(Serial serial)
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