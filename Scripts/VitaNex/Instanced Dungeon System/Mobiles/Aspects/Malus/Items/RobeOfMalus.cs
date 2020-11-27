#region Header
//   Vorspire    _,-'/-'/  RobeOfMalus.cs
//   .      __,-; ,'( '/
//    \.    `-.__`-._`:_,-._       _ , . ``
//     `:-._,------' ` _,`--` -: `_ , ` ,' :
//        `---..__,,--'  (C) 2015  ` -'. -'
//        #  Vita-Nex [http://core.vita-nex.com]  #
//  {o)xxx|===============-   #   -===============|xxx(o}
//        #        The MIT License (MIT)          #
#endregion

namespace Server.Items
{
	public class RobeOfMalus : Robe
	{
		[Constructable]
		public RobeOfMalus()
		{
			Name = "Robe of Malus [Replica]";
			Hue = Utility.RandomRedHue();

			Attributes.DefendChance = 6;
			Attributes.WeaponDamage = 6;
			Attributes.SpellDamage = 6;
		}

		public RobeOfMalus(Serial serial)
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