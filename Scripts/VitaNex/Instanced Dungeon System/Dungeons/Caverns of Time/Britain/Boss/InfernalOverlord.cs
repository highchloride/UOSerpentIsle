#region Header
//   Vorspire    _,-'/-'/  InfernalOverlord.cs
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
	public class InfernalOverlord : InfernalBoss
	{
		public override AspectFlags DefaultAspects
		{
			get { return base.DefaultAspects | AspectFlags.Time | AspectFlags.Illusion; }
		}

		public override AspectLevel DefaultLevel { get { return AspectLevel.Extreme; } }

		[Constructable]
		public InfernalOverlord()
			: base(AIType.AI_NecroMage, 10)
		{
			Name = "Infernal Overlord";
		}

		public InfernalOverlord(Serial serial)
			: base(serial)
		{ }

		protected override int InitBody()
		{
			return 1071;
		}

		public override int GetIdleSound()
		{
			return 0x300 + Utility.Random(1);
		}

		public override int GetAngerSound()
		{
			return 0x300 + Utility.Random(1);
		}

		public override int GetAttackSound()
		{
			return 0x302;
		}

		public override int GetHurtSound()
		{
			return 0x303;
		}

		public override int GetDeathSound()
		{
			return 0x304;
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
