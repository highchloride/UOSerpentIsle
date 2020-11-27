#region Header
//   Vorspire    _,-'/-'/  DungeonPack.cs
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
using Server.Items;
#endregion

namespace VitaNex.Dungeons
{
	public class DungeonPack : Backpack
	{
		[CommandProperty(AccessLevel.GameMaster)]
		public Mobile Owner { get; private set; }

		public DungeonPack(Mobile owner)
		{
			Owner = owner;
		}

		public DungeonPack(Serial serial)
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