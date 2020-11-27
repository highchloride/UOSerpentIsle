#region Header
//   Vorspire    _,-'/-'/  InfernalBoss.cs
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
using System.Drawing;
using System.Linq;

using Server;
using Server.Items;
using Server.Mobiles;
#endregion

namespace VitaNex.Dungeons
{
	public abstract class InfernalBoss : BaseAspect
	{
		public override AspectFlags DefaultAspects
		{
			get
			{
				return AspectFlags.Chaos | AspectFlags.Darkness | AspectFlags.Death | AspectFlags.Decay | AspectFlags.Despair;
			}
		}

		public InfernalBoss(AIType ai, int rangeFight)
			: base(ai, FightMode.Closest, 16, rangeFight, 0.1, 0.2)
		{
			Hue = 2075;

			SetDamageType(ResistanceType.Physical, 60);
			SetDamageType(ResistanceType.Fire, 20);
			SetDamageType(ResistanceType.Cold, 0);
			SetDamageType(ResistanceType.Poison, 0);
			SetDamageType(ResistanceType.Energy, 20);

			SetResistance(ResistanceType.Physical, 75, 100);
			SetResistance(ResistanceType.Fire, 50, 75);
			SetResistance(ResistanceType.Energy, 50, 75);
		}

		public InfernalBoss(Serial serial)
			: base(serial)
		{ }

		protected override string InitTitle()
		{
			return String.Empty;
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
