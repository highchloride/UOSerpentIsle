#region Header
//   Vorspire    _,-'/-'/  AspectSpawn.cs
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

using Server.Items;
#endregion

namespace Server.Mobiles
{
	public abstract class AspectSpawn : BaseCreature, IAspectSpawn
	{
		[CommandProperty(AccessLevel.GameMaster, true)]
		public BaseAspect Aspect { get; private set; }

		public override bool AlwaysAttackable { get { return true; } }
		public override bool AlwaysMurderer { get { return true; } }

		public override bool DeleteCorpseOnDeath { get { return true; } }

		public override bool CanBeParagon { get { return false; } }

		public override bool CanFlee { get { return Aspect == null || Aspect.Deleted || !Aspect.Alive; } }

		public AspectSpawn(BaseAspect aspect, AIType ai, FightMode mode, double dActiveSpeed, double dPassiveSpeed)
			: base(ai, mode, aspect.RangePerception, aspect.RangeFight, dActiveSpeed, dPassiveSpeed)
		{
			Aspect = aspect;

			Name = "Aspect Spawn";
			Body = 129;

			Hue = Aspect.Hue;
			Team = Aspect.Team;

			Resistances.SetAll(i => Aspect.Resistances[i]);

			SetDamageType(ResistanceType.Physical, Aspect.PhysicalDamage);
			SetDamageType(ResistanceType.Fire, Aspect.FireDamage);
			SetDamageType(ResistanceType.Cold, Aspect.ColdDamage);
			SetDamageType(ResistanceType.Poison, Aspect.PoisonDamage);
			SetDamageType(ResistanceType.Energy, Aspect.EnergyDamage);

			Aspect.Scale(this);

			SpawnAspectAbility.Register(this);
		}

		public AspectSpawn(Serial serial)
			: base(serial)
		{ }

		public override void OnThink()
		{
			base.OnThink();

			if (Aspect == null || Aspect.Deleted || !Aspect.Alive || !Aspect.InRange(this, Aspect.RangePerception * 2))
			{
				Kill();
			}
		}

		public override void AggressiveAction(Mobile aggressor)
		{
			base.AggressiveAction(aggressor);

			if (Aspect != null)
			{
				Aspect.AggressiveAction(aggressor);
			}
		}

		public override void OnDeath(Container c)
		{
			base.OnDeath(c);

			if (c != null)
			{
				c.Delete();
			}
		}

		public override void OnAfterDelete()
		{
			base.OnAfterDelete();

			SpawnAspectAbility.Unregister(this);

			if (Corpse != null)
			{
				Corpse.Delete();
			}

			if (Aspect != null)
			{
				Aspect = null;
			}
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.SetVersion(0);

			writer.Write(Aspect);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			reader.GetVersion();

			Aspect = reader.ReadMobile<BaseAspect>();

			SpawnAspectAbility.Register(this);
		}
	}
}