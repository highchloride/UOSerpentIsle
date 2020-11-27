#region Header
//   Vorspire    _,-'/-'/  SewerThing.cs
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
	[CorpseName("stinking remains of Sewer Thing")]
	public class SewerThing : BaseAspect
	{
		public override AspectFlags DefaultAspects { get { return AspectFlags.Decay; } }

		public override bool GivesMLMinorArtifact { get { return true; } }

		public override Poison PoisonImmune { get { return Poison.Lethal; } }
		public override Poison HitPoison { get { return Poison.Lethal; } }

		//public override bool CanAreaPoison { get { return true; } }
		//public override Poison HitAreaPoison { get { return Poison.Lethal; } }

		[Constructable]
		public SewerThing()
			: base(AIType.AI_Mage, FightMode.Strongest, 10, 2, 0.02, 0.04)
		{
			Name = "Sewer Thing";
			Hue = 2967;

			SetDamageType(ResistanceType.Physical, 20);
			SetDamageType(ResistanceType.Fire, 20);
			SetDamageType(ResistanceType.Cold, 20);
			SetDamageType(ResistanceType.Poison, 20);
			SetDamageType(ResistanceType.Energy, 20);

			SetDamage(15, 20);
			SetHits(50000);
		}

		public SewerThing(Serial serial)
			: base(serial)
		{ }

		protected override int InitBody()
		{
			return 780;
		}

		public override void GenerateLoot()
		{
			base.GenerateLoot();

			AddLoot(LootPack.LowScrolls, 4);
			AddLoot(LootPack.MedScrolls, 4);
			AddLoot(LootPack.HighScrolls, 4);
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
