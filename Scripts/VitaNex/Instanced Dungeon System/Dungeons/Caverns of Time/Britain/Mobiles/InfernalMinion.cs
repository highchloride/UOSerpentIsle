#region Header
//   Vorspire    _,-'/-'/  InfernalMinion.cs
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
	public class InfernalMinion : LichLord
	{
        //public override bool RequiresDomination => false;

        [Constructable]
		public InfernalMinion()
		{
			Name = "Infernal Minion";
			Hue = 2076;

            Tamable = false;
		}

		public InfernalMinion(Serial serial)
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
