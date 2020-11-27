#region Header
//   Vorspire    _,-'/-'/  ElementalAspectHead.cs
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

using VitaNex.Dungeons;
#endregion

namespace Server.Items
{
	public abstract class ElementalAspectHead : Item
	{
		public static readonly Type[] Types =
		{
			typeof(EarthAspectHead), typeof(FireAspectHead), typeof(FrostAspectHead),
			typeof(PoisonAspectHead), typeof(EnergyAspectHead)
		};

		public static ElementalAspectHead CreateInstance(int index, string name, int hue)
		{
			return Types[index].CreateInstanceSafe<ElementalAspectHead>(name, hue);
		}

		public override bool Nontransferable
		{
			get
			{
				var r = this.GetRegion<DungeonZone>();

				return r == null || !(r.Dungeon is TrialOfElements);
			}
		}

		public ElementalAspectHead(string name, int hue)
			: this(name, hue, 1)
		{ }

		public ElementalAspectHead(string name, int hue, int amount)
			: base(0x2DB4)
		{
			Name = String.Format("Head of {0}", name);
			Hue = hue;

			Stackable = true;
			Amount = Math.Max(1, Math.Min(60000, amount));
		}

		public ElementalAspectHead(Serial serial)
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