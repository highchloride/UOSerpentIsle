#region Header
//   Vorspire    _,-'/-'/  Dungeon_Loot.cs
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
using System.Collections.Generic;
using System.Linq;

using Server;
using Server.Items;
using Server.Mobiles;

using VitaNex.InstanceMaps;
using VitaNex.SuperGumps;
#endregion

namespace VitaNex.Dungeons
{
	public abstract partial class Dungeon
	{
		#region Corpse Gold
		private static long GiveGold(List<PlayerMobile> k, long gold)
		{
			if (gold <= 0 || k.IsNullOrEmpty())
			{
				return 0;
			}

			if (k.Count > 1)
			{
				gold = (long)Math.Truncate(gold / (double)k.Count);
			}

			return k.Sum(m => GiveGold(m, gold));
		}

		private static long GiveGold(PlayerMobile m, long gold)
		{
			if (gold <= 0 || m == null)
			{
				return 0;
			}

			var looted = 0L;

			// Autobank Gold
			while (gold > 0)
			{
				var deposit = Banker.DepositUpTo(m, (int)Math.Min(Int32.MaxValue, gold));

				if (deposit > 0)
				{
					looted += deposit;
					gold -= deposit;
				}
				else
				{
					break;
				}
			}

			while (gold > 0)
			{
				var a = (int)Math.Min(gold, 10000);
				var g = new Gold(a);

				if (m.GiveItem(g, GiveFlags.PackBankDelete).WasReceived())
				{
					looted += a;
					gold -= a;
				}
				else
				{
					break;
				}
			}

			if (looted > 0)
			{
				m.SendMessage(1258, "You received {0:#,0} gold.", looted);
			}

			return looted;
		}

		private static long ConsumeGold(List<Item> items)
		{
			var total = 0L;

			foreach (var item in FindGold(items))
			{
				if (item is Gold)
				{
					total += item.Amount;
				}
				else if (item is BankCheck)
				{
					total += ((BankCheck)item).Worth;
				}
				else
				{
					continue;
				}

				item.Delete();
				items.Remove(item);
			}

			return total;
		}

		private static IEnumerable<Item> FindGold(List<Item> items)
		{
			var i = items.Count;

			while (--i >= 0)
			{
				var item = items[i];

				if (item is Container)
				{
					if (item is LockableContainer && ((LockableContainer)item).Locked)
					{
						continue;
					}

					foreach (var o in FindGold(item.Items))
					{
						yield return o;
					}
				}
				else if (item is Gold || item is BankCheck)
				{
					yield return item;
				}
			}
		}
		#endregion

		public virtual int GoldMin { get { return 10; } }
		public virtual int GoldMax { get { return 50; } }

		/// <summary>
		/// Used to adjust the final value of generated loot gold.
		/// </summary>
		public virtual double GoldFactor { get { return 1.0; } }

		/// <summary>
		/// Used to adjust the final attributes of a generated loot item.
		/// </summary>
		public virtual double LootFactor { get { return 1.0; } }

		/// <summary>
		/// Used when generating loot with Reforging (SA).
		/// </summary>
		public virtual int LootBudgetMin { get { return 100; } }

		/// <summary>
		/// Used when generating loot with Reforging (SA).
		/// </summary>
		public virtual int LootBudgetMax { get { return 700; } }

		/// <summary>
		/// Used when generating loot with Reforging (SA).
		/// </summary>
		public virtual double LootArtifactChance { get { return 0.05; } }

		/// <summary>
		/// Used when generating loot with BaseRunicTool (AOS).
		/// </summary>
		public virtual int LootPropsMin { get { return 1; } }

		/// <summary>
		/// Used when generating loot with BaseRunicTool (AOS).
		/// </summary>
		public virtual int LootPropsMax { get { return 5; } }

		/// <summary>
		/// Used when generating loot with BaseRunicTool (AOS).
		/// </summary>
		public virtual int LootIntensityMin { get { return 10; } }

		/// <summary>
		/// Used when generating loot with BaseRunicTool (AOS).
		/// </summary>
		public virtual int LootIntensityMax { get { return 100; } }

		public virtual TimeSpan LootTimeout { get { return TimeSpan.FromMinutes(3.0); } }
		
		[CommandProperty(Instances.Access)]
		public DungeonLootMode LootMode { get; set; }

		protected virtual void OnPlayerDeath(PlayerMobile dead, Container corpse)
		{ }

		protected virtual void OnCreatureDeath(BaseCreature dead, Container corpse, List<Item> loot)
		{
			if (IsLootEligible(dead))
			{
				GenerateLootPack(dead, loot);
			}
		}

		public virtual void HandlePlayerDeath(PlayerDeathEventArgs e)
		{
			if (e.Mobile is PlayerMobile)
			{
				OnPlayerDeath((PlayerMobile)e.Mobile, e.Mobile.Corpse);
			}
		}

		public virtual void HandleCreatureDeath(CreatureDeathEventArgs e)
		{
			if (!CheckCreatureDeath(e))
			{
				return;
			}

			var dead = e.Creature as BaseCreature;

			if (dead == null)
			{
				return;
			}

			OnCreatureDeath(dead, e.Corpse, e.ForcedLoot);

			var gold = ConsumeGold(e.ForcedLoot);

			if (e.Corpse != null && !e.Corpse.Deleted)
			{
				gold += ConsumeGold(e.Corpse.Items);
			}

			if (gold > 0)
			{
				gold -= GiveGold(ActiveGroup, gold);

				while (gold > 0)
				{
					var g = GenerateGold(e.ForcedLoot, (int)Math.Min(Int32.MaxValue, gold));

					if (g > 0)
					{
						gold -= g;
					}
					else
					{
						break;
					}
				}
			}

			if (ActiveGroup.Count > 1 && LootMode == DungeonLootMode.Advanced)
			{
				Timer.DelayCall(OnDeferredLoot, e);
			}
		}

		private void OnDeferredLoot(CreatureDeathEventArgs e)
		{
			if (e.Corpse == null || e.Corpse.Deleted || e.ClearCorpse || e.Corpse.Items.IsNullOrEmpty())
			{
				return;
			}

			var dead = e.Creature as BaseCreature;

			if (dead == null)
			{
				return;
			}

			var decay = LootTimeout;

			e.Corpse.Items.ForEachReverse(
				i =>
				{
					if (i != null && !i.Deleted && !Loot.Exists(o => o.Item == i) && ShouldRoll(dead, i))
					{
						Loot.Add(new DungeonLootEntry(this, i, decay, ActiveGroup));
					}
				});
		}

		public virtual bool CheckCreatureDeath(CreatureDeathEventArgs e)
		{
			e.PreventDelete = true;

			return true;
		}

		public virtual bool IsLootEligible(BaseCreature c)
		{
			return c != null && !c.Blessed && !c.IsChampionSpawn && !c.IsAnimatedDead && !c.IsDeadPet && !c.IsDeadBondedPet &&
				   !c.IsBonded && !c.Controlled && !c.Summoned && c.LastOwner == null && (c.Owners == null || c.Owners.Count <= 0);
		}

		public virtual double ComputeLootChance(Mobile m, BaseCreature bc)
		{
			if (!IsLootEligible(bc))
			{
				return 0.0;
			}

			Mobile master;

			while (m.IsControlled(out master))
			{
				m = master;
			}

			var chance = Core.AOS ? LootPack.GetLuckChance(m, bc) / 10000.0 : Utility.RandomDouble() * 0.12;

			chance *= 2;
			
			if (bc is BaseChampion)
			{
				chance += chance * 0.10;
			}

			if (bc.IsParagon)
			{
				chance += chance * 0.10;
			}
			/*
			if (m.HasPowerHour())
			{
				chance += chance * 0.10;
			}
			*/
			return Math.Max(0.0, Math.Min(1.0, chance));
		}

		public virtual void GenerateLootPack(Mobile m, List<Item> loot)
		{
			GenerateLootPack(m as BaseCreature, loot);
		}

		public virtual void GenerateLootPack(BaseCreature m, List<Item> loot)
		{
			if (IsLootEligible(m))
			{
				GenerateLoot(m, loot, LootFactor);
				GenerateGold(m, loot, GoldFactor, GoldMin, GoldMax);
			}
		}

		protected virtual int ComputeGold(BaseCreature m, double factor, int min, int max)
		{
			var g = Utility.RandomMinMax(min, max);

			if (g > 0 && ActiveGroup.Count > 1)
			{
				g += g * (ActiveGroup.Count - 1);
			}

			if (g > 0 && factor > 0 && factor != 1)
			{
				g = (int)Math.Floor(g * factor);
			}

			return Math.Max(0, g);
		}

		protected virtual void GenerateGold(BaseCreature m, List<Item> loot, double factor, int min, int max)
		{
			GenerateGold(loot, ComputeGold(m, factor, min, max));
		}

		protected virtual int GenerateGold(List<Item> loot, int gold)
		{
			var amount = 0;

			Item item;

			while (gold > 0)
			{
				item = null;

				if (gold >= 5000)
				{
					item = new Gold(5000);
				}
				else if (gold > 0)
				{
					item = new Gold(gold);
				}

				if (item == null)
				{
					break;
				}

				gold -= item.Amount;
				amount += item.Amount;

				loot.Add(item);
			}

			return amount;
		}

		protected virtual void GenerateLoot(BaseCreature m, List<Item> loot, double scale)
		{
			double c;

			if (ActiveGroup.Count <= 0)
			{
				c = Utility.RandomDouble() * (Math.Log10(m.RawStatTotal) / 10.0);
			}
			else
			{
				c = ActiveGroup.Average(o => ComputeLootChance(o, m));
			}

			var count = Math.Max(1, ActiveGroup.Count);
			
			Item item;

			while (--count >= 0)
			{
				if (Utility.RandomDouble() > c)
				{
					continue;
				}

				item = GenerateLootItem(m);

				if (item == null || item.Deleted)
				{
					continue;
				}

				MutateLootItem(item);

				if (item.Deleted)
				{
					continue;
				}

				ScaleItem(item, scale);

				if (!item.Deleted)
				{
					loot.Add(item);
				}
			}
		}

		public virtual Item GenerateLootItem(BaseCreature m)
		{
			if (Core.AOS)
			{
				return Server.Loot.RandomArmorOrShieldOrWeaponOrJewelry(Core.SE && Map.MapID == 4, Core.ML);
			}

			return Server.Loot.RandomArmorOrShieldOrWeapon();
		}

		protected virtual void MutateLootItem(Item item)
		{
			if (Core.SA)
			{
				var budget = Utility.RandomMinMax(LootBudgetMin, LootBudgetMax);
				var artifact = Utility.RandomDouble() < LootArtifactChance;

				RunicReforging.GenerateRandomItem(item, null, budget, 0, 0, 0, MapParent, artifact);
			}
			else if (Core.AOS)
			{
				var tot = Utility.RandomMinMax(LootPropsMin, LootPropsMax);
				var min = LootIntensityMin;
				var max = LootIntensityMax;

				if (item is BaseArmor)
				{
					BaseRunicTool.ApplyAttributesTo((BaseArmor)item, tot, min, max);
				}
				else if (item is BaseHat)
				{
					BaseRunicTool.ApplyAttributesTo((BaseHat)item, tot, min, max);
				}
				else if (item is BaseJewel)
				{
					BaseRunicTool.ApplyAttributesTo((BaseJewel)item, tot, min, max);
				}
				else if (item is BaseWeapon)
				{
					BaseRunicTool.ApplyAttributesTo((BaseWeapon)item, tot, min, max);
				}
				else if (item is Spellbook)
				{
					BaseRunicTool.ApplyAttributesTo((Spellbook)item, tot, min, max);
				}
			}
			else if (item is BaseWeapon)
			{
				var weapon = (BaseWeapon)item;

				weapon.DamageLevel = (WeaponDamageLevel)Utility.Random(6);
				weapon.AccuracyLevel = (WeaponAccuracyLevel)Utility.Random(6);
				weapon.DurabilityLevel = (WeaponDurabilityLevel)Utility.Random(6);
			}
			else if (item is BaseArmor)
			{
				var armor = (BaseArmor)item;

				armor.ProtectionLevel = (ArmorProtectionLevel)Utility.Random(6);
				armor.Durability = (ArmorDurabilityLevel)Utility.Random(6);
			}
		}

		public virtual void OnCorpseCreated(Mobile dead, Container c)
		{ }

		public virtual bool ShouldRoll(BaseCreature dead, Item item)
		{
			if (LootMode != DungeonLootMode.Advanced || dead == null || item == null || item.Deleted || item.BlessedFor != null)
			{
				return false;
			}

			if (item.IsArtifact)
			{
				return true;
			}

			if (item.LootType == LootType.Blessed)
			{
				return true;
			}

			if (item is EtherealMount)
			{
				return true;
			}

			if (Core.AOS)
			{
				var attrs = item.GetAttributeCount();

				if (attrs >= (Core.SA ? 5 : Core.ML ? 4 : 3))
				{
					return true;
				}
			}

			return false;
		}

		public virtual void OnLootRoll(DungeonLootEntry entry, DungeonLootRoll roll)
		{
			if (entry == null || !entry.Valid)
			{
				return;
			}

			GroupMessage(
				m =>
					String.Format(
						"{0} select{1} {2} for:  {3}",
						m == roll.Mobile ? "You" : roll.Mobile.RawName,
						m != roll.Mobile ? "s" : String.Empty,
						roll.Action,
						entry.Item.ResolveName(m).ToUpperWords()));
		}

		public virtual void OnLootWin(DungeonLootEntry entry, DungeonLootRoll roll, bool timeout)
		{
			if (entry == null || !entry.Valid)
			{
				return;
			}

			entry.Item.Movable = true;

			if (!GiveLoot(roll.Mobile, entry.Item, false))
			{
				entry.Winner = null;
				entry.Rolls.Remove(roll.Mobile);
				roll.Free();
				return;
			}

			foreach (var val in entry.Rolls.Values)
			{
				DungeonLootRoll r;

				if (val != null)
				{
					r = val.Value;
				}
				else
				{
					continue;
				}

				GroupMessage(
					m =>
						String.Format(
							"{0} roll{1} {2}",
							m == r.Mobile ? "You" : r.Mobile.RawName,
							m != r.Mobile ? "s" : String.Empty,
							r.Value));
			}

			GroupMessage(
				m =>
					String.Format(
						"{0} receive{1} loot: {2}",
						m == roll.Mobile ? "You" : roll.Mobile.RawName,
						m != roll.Mobile ? "s" : String.Empty,
						entry.Item.ResolveName(m).ToUpperWords()));
		}

		public virtual bool GiveLoot(PlayerMobile m, Item i, bool message)
		{
			return m.GiveItem(i, GiveFlags.Pack, message) == GiveFlags.Pack;
		}

		private bool _ProcessingLoot;

		protected virtual void ProcessLoot()
		{
			if (_ProcessingLoot || Loot == null || Loot.Count == 0)
			{
				return;
			}

			_ProcessingLoot = true;

			Loot.RemoveAll(
				e =>
				{
					if (e == null)
					{
						return true;
					}

					if (!e.Valid || e.HasWinner)
					{
						e.Free();
						return true;
					}

					return false;
				});

			if (Loot.Count == 0)
			{
				Loot.Free(false);

				_ProcessingLoot = false;
				return;
			}

			var now = DateTime.UtcNow;
			var players = new List<PlayerMobile>();

			var count = Loot.Count;

			while (--count >= 0)
			{
				var e = Loot[count];

				if (e.Process(e.Expire < now))
				{
					Loot.Remove(e);

					e.Free();
				}
				else
				{
					DungeonLootUI dui;

					foreach (
						var kv in e.Rolls.Where(kv => kv.Value == null && kv.Key != null && !kv.Key.Deleted && !players.Contains(kv.Key)))
					{
						dui = SuperGump.EnumerateInstances<DungeonLootUI>(kv.Key).FirstOrDefault(ui => ui != null && !ui.IsDisposed);

						if (dui == null || dui.Dungeon != this)
						{
							players.AddOrReplace(kv.Key);
						}

						if (dui == null)
						{
							continue;
						}

						if (!Loot.Where(loot => loot.Rolls.ContainsKey(kv.Key) && loot.Rolls[kv.Key] == null).All(dui.List.Contains))
						{
							players.AddOrReplace(kv.Key);
						}
					}
				}
			}

			players.ForEach(m => DungeonLootUI.DisplayTo(m, false, this));
			players.Free(true);

			_ProcessingLoot = false;
		}
	}
}
