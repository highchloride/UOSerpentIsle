#region Header
//   Vorspire    _,-'/-'/  InfernalStalker.cs
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
	public class InfernalStalker : SkeletalKnight
	{
        //public override bool RequiresDomination => false;
        public override bool CanStealth { get { return true; } }

		[Constructable]
		public InfernalStalker()
		{
			Name = "Infernal Stalker";
			Hue = 2076;

			SetSkill(SkillName.Stealth, 80.0, 100.0);
            Tamable = false;
		}

		public InfernalStalker(Serial serial)
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
