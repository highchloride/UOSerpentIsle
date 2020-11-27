#region Header
//   Vorspire    _,-'/-'/  InfernalCommander.cs
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
	public class InfernalCommander : InfernalBoss
	{
		public override AspectFlags DefaultAspects
		{
			get { return base.DefaultAspects | AspectFlags.Fire; }
		}

		public override AspectLevel DefaultLevel { get { return AspectLevel.Taxing; } }

		[Constructable]
		public InfernalCommander()
			: base(AIType.AI_NecroMage, 1)
		{
			Name = "Infernal Commander";
		}

		public InfernalCommander(Serial serial)
			: base(serial)
		{ }

		protected override int InitBody()
		{
			return 830;
		}

		public override int GetIdleSound()
		{
			return 0x621;
		}

		public override int GetAngerSound()
		{
			return 0x621;
		}

		public override int GetAttackSound()
		{
			return 0x61E;
		}

		public override int GetHurtSound()
		{
			return 0x620;
		}

		public override int GetDeathSound()
		{
			return 0x61F;
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