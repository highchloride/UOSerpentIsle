#region Header
//   Vorspire    _,-'/-'/  InfernalGeneral.cs
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
	public class InfernalGeneral : InfernalBoss
	{
		public override AspectLevel DefaultLevel { get { return AspectLevel.Hard; } }

		[Constructable]
		public InfernalGeneral()
			: base(AIType.AI_Melee, 2)
		{
			Name = "Infernal General";
		}

		public InfernalGeneral(Serial serial)
			: base(serial)
		{ }

		protected override int InitBody()
		{
			return 280;
		}

		public override int GetIdleSound()
		{
			return 0x596;
		}

		public override int GetAngerSound()
		{
			return 0x597;
		}

		public override int GetAttackSound()
		{
			return 0x599;
		}

		public override int GetHurtSound()
		{
			return 0x59A;
		}

		public override int GetDeathSound()
		{
			return 0x59C;
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