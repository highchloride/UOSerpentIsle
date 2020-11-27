#region References
using Server.Targeting;
using Server.Items;
#endregion

namespace Server.Mobiles
{
	[CorpseName("a goblin corpse")]
	public class GreenGoblinScoutOverland : BaseCreature
	{
		[Constructable]
		public GreenGoblinScoutOverland()
			: base(AIType.AI_OrcScout, FightMode.Closest, 10, 7, 0.2, 0.4)
		{
			Name = "a green goblin scout";
			Body = 723;
			BaseSoundID = 0x600;

            this.SetStr(96, 120);
            this.SetDex(81, 105);
            this.SetInt(36, 60);

            this.SetHits(58, 72);

            this.SetDamage(5, 7);

            this.SetDamageType(ResistanceType.Physical, 100);

            this.SetResistance(ResistanceType.Physical, 25, 30);
            this.SetResistance(ResistanceType.Fire, 20, 30);
            this.SetResistance(ResistanceType.Cold, 10, 20);
            this.SetResistance(ResistanceType.Poison, 10, 20);
            this.SetResistance(ResistanceType.Energy, 20, 30);

            this.SetSkill(SkillName.MagicResist, 50.1, 75.0);
            this.SetSkill(SkillName.Tactics, 55.1, 80.0);
            this.SetSkill(SkillName.Wrestling, 50.1, 70.0);

            this.Fame = 1500;
            this.Karma = -1500;  
        }

		public GreenGoblinScoutOverland(Serial serial)
			: base(serial)
		{ }

		public override int GetAngerSound() { return 0x600; }
        public override int GetIdleSound() { return 0x600; }
        public override int GetAttackSound() { return 0x5FD; }
        public override int GetHurtSound() { return 0x5FF; }
        public override int GetDeathSound() { return 0x5FE; }

        public override bool CanRummageCorpses { get { return true; } }
        public override int TreasureMapLevel { get { return 1; } }
        public override int Meat { get { return 1; } }
        public override TribeType Tribe { get { return TribeType.GreenGoblin; } }

		public override void GenerateLoot()
		{
			AddLoot(LootPack.Meager);
		}

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Utility.RandomDouble() < 0.01)
                c.DropItem(new LuckyCoin());
        }

        public override void OnThink()
		{
			if (Utility.RandomDouble() < 0.2)
			{
				TryToDetectHidden();
			}
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}

		private Mobile FindTarget()
		{
            IPooledEnumerable eable = GetMobilesInRange(10);
			foreach (Mobile m in eable)
			{
				if (m.Player && m.Hidden && m.IsPlayer())
				{
                    eable.Free();
					return m;
				}
			}

            eable.Free();
			return null;
		}

		private void TryToDetectHidden()
		{
			Mobile m = FindTarget();

			if (m != null)
			{
				if (Core.TickCount >= NextSkillTime && UseSkill(SkillName.DetectHidden))
				{
					Target targ = Target;

					if (targ != null)
					{
						targ.Invoke(this, this);
					}

					Effects.PlaySound(Location, Map, 0x340);
				}
			}
		}
	}
}
