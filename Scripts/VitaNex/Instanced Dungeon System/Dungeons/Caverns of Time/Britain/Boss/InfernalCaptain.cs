#region Header
//   Vorspire    _,-'/-'/  InfernalCaptain.cs
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
using Server.Mobiles;
#endregion

namespace VitaNex.Dungeons
{
	public class InfernalCaptain : InfernalBoss
	{
		public override AspectLevel DefaultLevel { get { return AspectLevel.Normal; } }

		[Constructable]
		public InfernalCaptain()
			: base(AIType.AI_Melee, 2)
		{
			Name = "Infernal Captain";

			BaseSoundID = 357;
		}

		public InfernalCaptain(Serial serial)
			: base(serial)
		{ }

		protected override int InitBody()
		{
			return 40;
		}

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