#region Header
//   Vorspire    _,-'/-'/  ElementalAspect.cs
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
	public abstract class ElementalAspect : BaseAspect
	{
		public override AspectLevel DefaultLevel { get { return AspectLevel.Hard; } }

		public override int Scales { get { return 10; } }

		public ElementalAspect()
			: base(AIType.AI_Melee, FightMode.Closest, 16, 1, 0.2, 0.2)
		{
			BaseSoundID = 362;

			Team = 667;

			InitElement();

			Hue = GetElementHue();
		}

		public ElementalAspect(Serial serial)
			: base(serial)
		{ }

		public override void AspectChanged(AspectFlags a, bool state)
		{
			base.AspectChanged(a, state);

			InitElement();
		}

		public override void OnCarve(Mobile m, Corpse corpse, Item with)
		{
			base.OnCarve(m, corpse, with);

			if (m != null)
			{
				var h = CreateHead();

				if (h != null)
				{
					if (corpse != null && !corpse.IsBones)
					{
						corpse.TurnToBones();
					}

					if (m.GiveItem(h, GiveFlags.Pack) != GiveFlags.Pack)
					{
						if (corpse != null)
						{
							corpse.DropItem(h);
						}
						else
						{
							h.MoveToWorld(m.Location, m.Map);
						}
					}
				}
			}
		}

		public abstract ElementalAspectHead CreateHead();

		protected override int InitBody()
		{
			return 826;
		}

		public virtual void InitElement()
		{
			switch (Aspects.Flags)
			{
				case AspectFlags.Earth:
				{
					SetDamageType(ResistanceType.Physical, 70);
					SetDamageType(ResistanceType.Fire, 10);
					SetDamageType(ResistanceType.Cold, 10);
					SetDamageType(ResistanceType.Poison, 0);
					SetDamageType(ResistanceType.Energy, 10);

					SetResistance(ResistanceType.Physical, 40, 60);
					SetResistance(ResistanceType.Fire, 20, 40);
					SetResistance(ResistanceType.Cold, 20, 40);
					SetResistance(ResistanceType.Poison, 10, 20);
					SetResistance(ResistanceType.Energy, 20, 40);
				}
					break;
				case AspectFlags.Fire:
				{
					SetDamageType(ResistanceType.Physical, 10);
					SetDamageType(ResistanceType.Fire, 70);
					SetDamageType(ResistanceType.Cold, 10);
					SetDamageType(ResistanceType.Poison, 10);
					SetDamageType(ResistanceType.Energy, 0);

					SetResistance(ResistanceType.Physical, 20, 40);
					SetResistance(ResistanceType.Fire, 40, 60);
					SetResistance(ResistanceType.Cold, 20, 40);
					SetResistance(ResistanceType.Poison, 20, 40);
					SetResistance(ResistanceType.Energy, 10, 20);
				}
					break;
				case AspectFlags.Frost:
				{
					SetDamageType(ResistanceType.Physical, 0);
					SetDamageType(ResistanceType.Fire, 10);
					SetDamageType(ResistanceType.Cold, 70);
					SetDamageType(ResistanceType.Poison, 10);
					SetDamageType(ResistanceType.Energy, 10);

					SetResistance(ResistanceType.Physical, 10, 20);
					SetResistance(ResistanceType.Fire, 20, 40);
					SetResistance(ResistanceType.Cold, 40, 60);
					SetResistance(ResistanceType.Poison, 20, 40);
					SetResistance(ResistanceType.Energy, 20, 40);
				}
					break;
				case AspectFlags.Poison:
				{
					SetDamageType(ResistanceType.Physical, 10);
					SetDamageType(ResistanceType.Fire, 0);
					SetDamageType(ResistanceType.Cold, 10);
					SetDamageType(ResistanceType.Poison, 70);
					SetDamageType(ResistanceType.Energy, 10);

					SetResistance(ResistanceType.Physical, 20, 40);
					SetResistance(ResistanceType.Fire, 10, 20);
					SetResistance(ResistanceType.Cold, 20, 40);
					SetResistance(ResistanceType.Poison, 40, 60);
					SetResistance(ResistanceType.Energy, 20, 40);
				}
					break;
				case AspectFlags.Energy:
				{
					SetDamageType(ResistanceType.Physical, 10);
					SetDamageType(ResistanceType.Fire, 10);
					SetDamageType(ResistanceType.Cold, 0);
					SetDamageType(ResistanceType.Poison, 10);
					SetDamageType(ResistanceType.Energy, 70);

					SetResistance(ResistanceType.Physical, 20, 40);
					SetResistance(ResistanceType.Fire, 20, 40);
					SetResistance(ResistanceType.Cold, 10, 20);
					SetResistance(ResistanceType.Poison, 20, 40);
					SetResistance(ResistanceType.Energy, 40, 60);
				}
					break;
				default: // Chaos
				{
					SetDamageType(ResistanceType.Physical, 20);
					SetDamageType(ResistanceType.Fire, 20);
					SetDamageType(ResistanceType.Cold, 20);
					SetDamageType(ResistanceType.Poison, 20);
					SetDamageType(ResistanceType.Energy, 20);

					SetResistance(ResistanceType.Physical, 20, 40);
					SetResistance(ResistanceType.Fire, 20, 40);
					SetResistance(ResistanceType.Cold, 20, 40);
					SetResistance(ResistanceType.Poison, 20, 40);
					SetResistance(ResistanceType.Energy, 20, 40);
				}
					break;
			}
		}

		public virtual int GetElementHue()
		{
			int val, hue = 0, max = 50;

			if ((val = PoisonDamage) >= max)
			{
				hue = 1267 + (val - 50) / 10;
				max = val;
			}

			if ((val = FireDamage) >= max)
			{
				hue = 1255 + (val - 50) / 10;
				max = val;
			}

			if ((val = EnergyDamage) >= max)
			{
				hue = 1273 + (val - 50) / 10;
				max = val;
			}

			if ((val = ColdDamage) >= max)
			{
				hue = 1261 + (val - 50) / 10;
			}

			if (hue == 0)
			{
				hue = 147;
			}

			return hue;
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