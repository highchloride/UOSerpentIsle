#region Header
//   Vorspire    _,-'/-'/  PoisonAspect.cs
//   .      __,-; ,'( '/
//    \.    `-.__`-._`:_,-._       _ , . ``
//     `:-._,------' ` _,`--` -: `_ , ` ,' :
//        `---..__,,--'  (C) 2017  ` -'. -'
//        #  Vita-Nex [http://core.vita-nex.com]  #
//  {o)xxx|===============-   #   -===============|xxx(o}
//        #        The MIT License (MIT)          #
#endregion

#region References
using Server.Items;
#endregion

namespace Server.Mobiles
{
	public class PoisonAspect : ElementalAspect
	{
		public override AspectFlags DefaultAspects { get { return AspectFlags.Poison; } }

		[Constructable]
		public PoisonAspect()
		{
			Name = "Venenum";
		}

		public PoisonAspect(Serial serial)
			: base(serial)
		{ }

		public override ElementalAspectHead CreateHead()
		{
			return new PoisonAspectHead(Name, Hue);
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