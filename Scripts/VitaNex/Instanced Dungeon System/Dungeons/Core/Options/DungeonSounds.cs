#region Header
//   Vorspire    _,-'/-'/  DungeonSounds.cs
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
	public class DungeonSounds : PropertyObject
	{
		[CommandProperty(Instances.Access)]
		public bool Enabled { get; set; }

		[CommandProperty(Instances.Access)]
		public int Teleport { get; set; }

		public DungeonSounds()
		{
			Enabled = true;

			Teleport = 0x029;
		}

		public DungeonSounds(GenericReader reader)
			: base(reader)
		{ }

		public override string ToString()
		{
			return "Sounds";
		}

		public override void Clear()
		{
			Enabled = false;

			Teleport = -1;
		}

		public override void Reset()
		{
			Enabled = true;

			Teleport = 0x029;
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			var version = writer.SetVersion(0);

			switch (version)
			{
				case 0:
				{
					writer.Write(Enabled);
					writer.Write(Teleport);
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
					Enabled = reader.ReadBool();
					Teleport = reader.ReadInt();
				}
					break;
			}
		}
	}
}