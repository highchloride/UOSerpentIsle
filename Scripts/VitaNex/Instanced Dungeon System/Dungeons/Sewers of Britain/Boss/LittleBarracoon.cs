#region Header
//   Vorspire    _,-'/-'/  LittleBarracoon.cs
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
using Server.Spells.Fifth;
using Server.Spells.Seventh;
#endregion

namespace Server.Mobiles
{
	public class LittleBarracoon : BaseCreature
	{
		public override int AcquireOnApproachRange { get { return 5; } }

		public override bool AlwaysMurderer { get { return true; } }
		public override bool AutoDispel { get { return true; } }
		public override double AutoDispelChance { get { return 1.0; } }
		public override bool BardImmune { get { return !Core.SE; } }
		public override bool Unprovokable { get { return Core.SE; } }
		public override bool Uncalmable { get { return Core.SE; } }
		public override Poison PoisonImmune { get { return Poison.Deadly; } }
		public override int TreasureMapLevel { get { return 4; } }
		public override bool ShowFameTitle { get { return false; } }
		public override bool ClickTitle { get { return false; } }

		[Constructable]
		public LittleBarracoon()
			: base(AIType.AI_NecroMage, FightMode.Weakest, 10, 1, 0.2, 0.4)
		{
			Name = "Barracoon Jr";
			Title = " little piper";
			Body = 0x190;
			Hue = 0x83EC;

			SetStr(305, 425);
			SetDex(72, 150);
			SetInt(505, 750);

			SetHits(4200);
			SetStam(102, 300);

			SetDamage(15, 20);

			SetDamageType(ResistanceType.Physical, 100);

			SetResistance(ResistanceType.Physical, 60, 70);
			SetResistance(ResistanceType.Fire, 40, 60);
			SetResistance(ResistanceType.Cold, 50, 60);
			SetResistance(ResistanceType.Poison, 40, 50);
			SetResistance(ResistanceType.Energy, 40, 50);

			SetSkill(SkillName.MagicResist, 100.0);
			SetSkill(SkillName.Tactics, 97.6, 100.0);
			SetSkill(SkillName.Wrestling, 97.6, 100.0);

			Fame = 22500;
			Karma = -22500;

			VirtualArmor = 70;

			AddItem(new FancyShirt(Utility.RandomGreenHue()));
			AddItem(new LongPants(Utility.RandomYellowHue()));
			AddItem(new JesterHat(Utility.RandomPinkHue()));
			AddItem(new Cloak(Utility.RandomPinkHue()));
			AddItem(new Sandals());

			HairItemID = 0x203B; // Short Hair
			HairHue = 0x94;
		}

		public LittleBarracoon(Serial serial)
			: base(serial)
		{ }

		public void Polymorph(Mobile m)
		{
			if (!m.CanBeginAction(typeof(PolymorphSpell)) || !m.CanBeginAction(typeof(IncognitoSpell)) || m.IsBodyMod)
			{
				return;
			}

			var mount = m.Mount;

			if (mount != null)
			{
				mount.Rider = null;
			}

			if (m.Mounted)
			{
				return;
			}

			if (m.BeginAction(typeof(PolymorphSpell)))
			{
				var disarm = m.FindItemOnLayer(Layer.OneHanded);

				if (disarm != null && disarm.Movable)
				{
					m.AddToBackpack(disarm);
				}

				disarm = m.FindItemOnLayer(Layer.TwoHanded);

				if (disarm != null && disarm.Movable)
				{
					m.AddToBackpack(disarm);
				}

				m.BodyMod = 42;
				m.HueMod = 0;

				new ExpirePolymorphTimer(m).Start();
			}
		}

		private class ExpirePolymorphTimer : Timer
		{
			private readonly Mobile m_Owner;

			public ExpirePolymorphTimer(Mobile owner)
				: base(TimeSpan.FromMinutes(3.0))
			{
				m_Owner = owner;

				Priority = TimerPriority.OneSecond;
			}

			protected override void OnTick()
			{
				if (!m_Owner.CanBeginAction(typeof(PolymorphSpell)))
				{
					m_Owner.BodyMod = 0;
					m_Owner.HueMod = -1;
					m_Owner.EndAction(typeof(PolymorphSpell));
				}
			}
		}

		public void SpawnRatmen(Mobile target)
		{
			var map = Map;

			if (map == null)
			{
				return;
			}

			var rats = 0;

			foreach (var m in this.FindMobilesInRange(map, 10))
			{
				if (m is SewerRatman || m is SewerRatmanArcher || m is SewerRatmanMage)
				{
					++rats;
				}
			}

			if (rats < 16)
			{
				PlaySound(0x3D);

				var newRats = Utility.RandomMinMax(1, 2);

				for (var i = 0; i < newRats; ++i)
				{
					BaseCreature rat = null;

					switch (Utility.Random(2))
					{
						case 0:
							rat = new SewerRatman();
							break;
						case 1:
							rat = new SewerRatmanArcher();
							break;
					}

					if (rat == null)
					{
						continue;
					}

					rat.Team = Team;

					var loc = Location;

					bool validLocation;

					for (var j = 0; j < 10; ++j)
					{
						var x = X + Utility.Random(3) - 1;
						var y = Y + Utility.Random(3) - 1;
						var z = map.GetAverageZ(x, y);

						validLocation = map.CanFit(x, y, Z, 16, false, false);

						if (validLocation)
						{
							loc = new Point3D(x, y, Z);
						}
						else
						{
							validLocation = map.CanFit(x, y, z, 16, false, false);
	
							if (validLocation)
							{
								loc = new Point3D(x, y, z);
							}
						}

						if (validLocation)
						{
							break;
						}
					}

					rat.MoveToWorld(loc, map);
					rat.Combatant = target;
				}
			}
		}

		public void DoSpecialAbility(Mobile target)
		{
			if (target == null || target.Deleted) //sanity
			{
				return;
			}

			if (Utility.RandomDouble() < 0.60) // 60% chance to polymorph attacker into a ratman
			{
				Polymorph(target);
			}

			if (Utility.RandomDouble() < 0.10) // 10% chance to more ratmen
			{
				SpawnRatmen(target);
			}

			if (Hits / (double)HitsMax <= 0.05 && !IsBodyMod) // Baracoon is low on life, polymorph into a ratman
			{
				Polymorph(this);
			}
		}

		public override void OnGotMeleeAttack(Mobile attacker)
		{
			base.OnGotMeleeAttack(attacker);

			DoSpecialAbility(attacker);
		}

		public override void OnGaveMeleeAttack(Mobile defender)
		{
			base.OnGaveMeleeAttack(defender);

			DoSpecialAbility(defender);
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			reader.ReadInt();
		}
	}
}
