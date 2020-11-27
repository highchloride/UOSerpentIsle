#region Header
//   Vorspire    _,-'/-'/  Malus.cs
//   .      __,-; ,'( '/
//    \.    `-.__`-._`:_,-._       _ , . ``
//     `:-._,------' ` _,`--` -: `_ , ` ,' :
//        `---..__,,--'  (C) 2015  ` -'. -'
//        #  Vita-Nex [http://core.vita-nex.com]  #
//  {o)xxx|===============-   #   -===============|xxx(o}
//        #        The MIT License (MIT)          #
#endregion

#region References
using System;
using System.Collections.Generic;
using System.Linq;

using Server.ContextMenus;
using Server.Items;
using Server.Regions;

using VitaNex;
using VitaNex.FX;
#endregion

namespace Server.Mobiles
{
	[CorpseName("the decaying remains of Malus")]
	public class Malus : BaseAspect
	{
		private static readonly AIType[] _PossibleAI =
		{
			//
			AIType.AI_Melee, AIType.AI_Mage, AIType.AI_Archer //
		};

		private static readonly Layer[] _EquipLayers = LayerExtUtility.EquipLayers;

		private static readonly SkillName[] _Skills = ((SkillName)0).GetValues<SkillName>();

		private static bool ConstructItem(out Item item, params Type[] types)
		{
			return (item = (types == null || types.Length == 0) ? null : types.GetRandom().CreateInstanceSafe<Item>()) != null;
		}

		private DateTime _NextDevour = DateTime.UtcNow;
		private DateTime _NextAISwitch = DateTime.UtcNow;
		private DateTime _NextSpecial = DateTime.UtcNow;

		public virtual bool Devour { get { return false; } }
		public virtual TimeSpan DevourInterval { get { return TimeSpan.FromSeconds(Utility.RandomMinMax(10, 20)); } }
		public virtual TimeSpan AISwitchInterval { get { return TimeSpan.FromSeconds(Utility.RandomMinMax(30, 120)); } }

		public virtual bool ThrowBomb { get { return Alive; } }
		public virtual TimeSpan ThrowBombInterval { get { return TimeSpan.FromSeconds(Utility.RandomMinMax(60, 120)); } }

		public override FoodType FavoriteFood { get { return FoodType.Meat; } }

		public override bool HealFromPoison { get { return true; } }

		public override AspectFlags DefaultAspects { get { return AspectFlags.Death | AspectFlags.Despair | AspectFlags.Darkness; } }

		[Constructable]
		public Malus()
			: base(_PossibleAI.GetRandom(), FightMode.Weakest, 16, 1, 0.1, 0.2)
		{
			Name = "Malus";

			Race = Race.Gargoyle;
			Body = Race.Body(this);
			Hue = Race.RandomSkinHue();
			HairHue = FacialHairHue = Race.RandomHairHue();
			HairItemID = Race.RandomHair(this);
			FacialHairItemID = Race.RandomFacialHair(this);

			BaseSoundID = 372;

			SetDamageType(ResistanceType.Physical, 10);
			SetDamageType(ResistanceType.Fire, 30);
			SetDamageType(ResistanceType.Cold, 0);
			SetDamageType(ResistanceType.Poison, 30);
			SetDamageType(ResistanceType.Energy, 30);

			SetResistance(ResistanceType.Fire, 50, 75);
			SetResistance(ResistanceType.Energy, 50, 75);
			SetResistance(ResistanceType.Poison, 50, 75);

			AddItem(
				new RobeOfMalus
				{
					Name = "Robe of Malus",
					Movable = false,
					LootType = LootType.Blessed
				});
            
			AddPackItems();
		}

		public Malus(Serial serial)
			: base(serial)
		{ }

		protected override int InitBody()
		{
			return Race.Gargoyle.Body(this);
		}

		protected virtual Item[] AddPackItems()
		{
			Func<Item, string> resolveName = i =>
			{
				var name = i.ResolveName().ToUpperWords();

				if (Insensitive.StartsWith(name, "a "))
				{
					name = name.Substring(2);
				}
				else if (Insensitive.StartsWith(name, "an "))
				{
					name = name.Substring(3);
				}

				return "Hunting " + name;
			};

			var list = new List<Item>();
			Item item;

			if (ConstructItem(
				out item,
				typeof(Cleaver),
				typeof(Dagger),
				typeof(Longsword),
				typeof(Broadsword),
				typeof(Kryss),
				typeof(Halberd),
				typeof(Scythe)))
			{
				item.Name = resolveName(item);
				item.LootType = LootType.Blessed;

				PackItem(item);

				list.Add(item);
			}

			if (ConstructItem(
				out item,
				typeof(Buckler),
				typeof(WoodenKiteShield),
				typeof(MetalKiteShield),
				typeof(HeaterShield),
				typeof(MetalShield),
				typeof(BronzeShield)))
			{
				item.Name = resolveName(item);
				item.LootType = LootType.Blessed;

				PackItem(item);

				list.Add(item);
			}

			if (ConstructItem(out item, typeof(Bow), typeof(Crossbow)))
			{
				item.Name = resolveName(item);
				item.LootType = LootType.Blessed;

				PackItem(item);

				list.Add(item);
			}

			return list.ToArray();
		}

		public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
		{
			base.GetContextMenuEntries(from, list);

			list.RemoveAll(e => e is PaperdollEntry);
		}

		public override void OnThink()
		{
			base.OnThink();

			if (Deleted || Map == null || Map == Map.Internal || !this.InCombat())
			{
				return;
			}

			var now = DateTime.UtcNow;

			if (now > _NextAISwitch)
			{
				_NextAISwitch = now + AISwitchInterval;

				SwitchAI();
			}

			if (!this.InCombat(TimeSpan.Zero) || now <= _NextSpecial)
			{
				return;
			}

			TimeSpan delay;
			var ability = GetSpecialAbility(out delay);

			if (ability == null)
			{
				return;
			}

			_NextSpecial = now + delay;

			VitaNexCore.TryCatch(ability, x => x.ToConsole(true));
		}

		public virtual Action GetSpecialAbility(out TimeSpan delay)
		{
			switch (Utility.Random(1))
			{
				case 0:
					delay = ThrowBombInterval;
					return ThrowBombs;
				default:
					delay = TimeSpan.Zero;
					return null;
			}
		}

		#region AI Switching
		protected virtual void SwitchAI()
		{
			if (Deleted || Map == null || Map == Map.Internal || Location == Point3D.Zero)
			{
				return;
			}

			AI = _PossibleAI.Not(ai => ai == AI).GetRandom(AIType.AI_Mage);

			RangeFight = AI == AIType.AI_Archer ? 8 : 1;

			SwitchEquipment();
		}
		#endregion

		#region Equipment Switching
		protected virtual void Undress(params Layer[] layers)
		{
			Items.Not(i => i == null || i.Deleted || !i.Movable || i == Backpack || i == FindBankNoCreate() || i == Mount)
				 .Where(i => _EquipLayers.Contains(i.Layer))
				 .Where(item => layers == null || layers.Length == 0 || layers.Contains(item.Layer))
				 .ForEach(Backpack.DropItem);
		}

		protected virtual void SwitchEquipment()
		{
			if (Deleted || Map == null || Map == Map.Internal || Location == Point3D.Zero)
			{
				return;
			}

			Undress(Layer.OneHanded, Layer.TwoHanded);

			Item weapon;
			Item shield = null;

			switch (AI)
			{
				case AIType.AI_Archer:
				{
					weapon = Backpack.FindItemsByType<BaseRanged>(true, w => w != null && !w.Deleted && w.CanEquip(this)).GetRandom();
				}
					break;
				case AIType.AI_Mage:
				{
					weapon = Backpack.FindItemsByType<Spellbook>(true, w => w != null && !w.Deleted && w.CanEquip(this)).GetRandom() ??
							 new Spellbook(Int64.MaxValue)
							 {
								 LootType = LootType.Blessed
							 };

					var regs = Backpack.FindItemByType<BagOfReagents>(true);

					if (regs == null || regs.Deleted || regs.Amount <= 0 || regs.Items.Count <= 0)
					{
						regs = new BagOfReagents(100)
						{
							LootType = LootType.Blessed
						};

						Backpack.DropItem(regs);
					}
				}
					break;
				case AIType.AI_Melee:
				{
					weapon =
						Backpack.FindItemsByType<BaseMeleeWeapon>(true, w => w != null && !w.Deleted && w.CanEquip(this)).GetRandom();

					if (weapon != null && weapon.Layer != Layer.TwoHanded)
					{
						shield = Backpack.FindItemsByType<BaseShield>(true, s => s != null && !s.Deleted && s.CanEquip(this)).GetRandom();
					}
				}
					break;
				default:
					weapon = Backpack.FindItemsByType<BaseWeapon>(true, w => w != null && !w.Deleted && w.CanEquip(this)).GetRandom();
					break;
			}

			if (weapon != null)
			{
				EquipItem(weapon);
			}

			if (shield != null)
			{
				EquipItem(shield);
			}

			Undress(_EquipLayers);

			var equip = new List<Item>(_EquipLayers.Length);
			var packItems = new List<Item>(Backpack.FindItemsByType<Item>(true, item => item != null && !item.Deleted && item.CanEquip(this)));

			foreach (var item in _EquipLayers.Select(l => FindItemOnLayer(l) ?? packItems.GetRandom()))
			{
				equip.AddOrReplace(item);
				packItems.Remove(item);
			}

			packItems.Free(true);

			foreach (var item in equip.Where(item => item != null && !item.IsEquipped()))
			{
				EquipItem(item);
			}

			equip.Free(true);
		}
		#endregion

		#region ThrowBombs
		protected virtual void ThrowBombs()
		{
			if (!ThrowBomb || Deleted || Map == null || Map == Map.Internal || Location == Point3D.Zero)
			{
				return;
			}

			var shout = false;

			foreach (var m in AcquireTargets(Location, RangePerception).Take(3))
			{
				var damage = (int)Math.Floor(m.Hits * 0.20);

				new MovingEffectInfo(this, m, Map, 8700).MovingImpact(
					e =>
					{
						new FirePentagramEffect(m, Map).Send();
						m.Damage(damage, this, true);
					});

				shout = true;
			}

			if (shout)
			{
				Yell(Utility.RandomBool() ? "CLEANSE BY FIRE!" : "CONFLAGRATE!");
			}
		}
		#endregion

		public override int GetAngerSound()
		{
			return 373;
		}

		public override int GetAttackSound()
		{
			return Female ? 1524 : 1528;
		}

		public override int GetDeathSound()
		{
			return Female ? 1525 : 1529;
		}

		public override int GetHurtSound()
		{
			return Female ? 1527 : 1531;
		}

		public override int GetIdleSound()
		{
			return Utility.RandomList(372, 373);
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.SuperBoss, 2);
		}

		public override void OnHitsChange(int oldValue)
		{
			base.OnHitsChange(oldValue);

			Flying = Hits >= HitsMax * 0.10 && this.InCombat();
		}

		protected override bool OnMove(Direction d)
		{
			var allow = base.OnMove(d);

			if (!Devour)
			{
				return allow;
			}

			var now = DateTime.UtcNow;

			if (now < _NextDevour)
			{
				return allow;
			}

			var gr = this.GetRegion<GuardedRegion>();

			if (gr != null && !gr.Disabled)
			{
				return allow;
			}

			if (!allow)
			{
				return false;
			}

			foreach (var c in
				this.FindEntitiesInRange<Corpse>(Map, 2)
					.Where(c => c != null && !c.Deleted && !c.IsDecoContainer && !c.IsBones && !(c.Owner is PlayerMobile))
					.Take(10))
			{
				Emote("*You see {0} completely devour a corpse and its contents*", RawName);

				c.Items.Where(
					item =>
						item != null && !item.Deleted && item.Movable && item.Visible && item.LootType == LootType.Regular &&
						item.BlessedFor == null && !item.Insured).ForEach(Backpack.DropItem);
				c.TurnToBones();
			}

			SwitchEquipment();

			_NextDevour = now + DevourInterval;

			return true;
		}

		public override void OnDeath(Container c)
		{
			base.OnDeath(c);

			if (c == null || c.Deleted)
			{
				return;
			}

			if (Core.SA && Utility.RandomDouble() < 0.05)
			{
				c.DropItem(new RobeOfMalus());
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

			reader.ReadInt();
		}
	}
}
