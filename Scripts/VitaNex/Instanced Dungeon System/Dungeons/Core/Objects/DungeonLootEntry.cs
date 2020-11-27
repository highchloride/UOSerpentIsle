#region Header
//   Vorspire    _,-'/-'/  DungeonLootEntry.cs
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
using Server.Mobiles;
#endregion

namespace VitaNex.Dungeons
{
	public class DungeonLootEntry
	{
		public Dungeon Dungeon { get; private set; }
		public Item Item { get; private set; }
		public DateTime Created { get; private set; }
		public DateTime Expire { get; set; }
		public PlayerMobile Winner { get; set; }

		public Dictionary<PlayerMobile, DungeonLootRoll?> Rolls { get; private set; }

		public bool Valid { get { return Item != null && !Item.Deleted && Rolls != null && Rolls.Count > 0; } }
		public bool HasWinner { get { return Winner != null && !Winner.Deleted; } }

		public DungeonLootEntry(Dungeon dungeon, Item item, TimeSpan decay, IEnumerable<PlayerMobile> group)
		{
			Dungeon = dungeon;

			Item = item;

			Created = DateTime.UtcNow;
			Expire = Created.Add(decay);

			Rolls = group.Where(m => m != null && !m.Deleted).ToDictionary(m => m, m => (DungeonLootRoll?)null);

			if (Item != null)
			{
				Item.Movable = false;
			}
		}

		public DungeonLootEntry(Dungeon dungeon, GenericReader reader)
		{
			Dungeon = dungeon;

			Deserialize(reader);

			if (Item != null)
			{
				Item.Movable = false;
			}
		}

		private int Roll(PlayerMobile m, DungeonLootAction action)
		{
			if (!Valid || HasWinner || m == null || m.Deleted)
			{
				return -1;
			}

			var roll = Rolls.GetValue(m);

			if (roll == null)
			{
				var value = action != DungeonLootAction.Pass ? (1 + Utility.Random(100)) : 0;

				Rolls.AddOrReplace(m, roll = new DungeonLootRoll(m, action, value));
			}

			if (Dungeon != null)
			{
				Dungeon.OnLootRoll(this, roll.Value);
			}

			return roll.Value;
		}

		public int Pass(PlayerMobile m)
		{
			return Roll(m, DungeonLootAction.Pass);
		}

		public int Greed(PlayerMobile m)
		{
			return Roll(m, DungeonLootAction.Greed);
		}

		public int Need(PlayerMobile m)
		{
			return Roll(m, DungeonLootAction.Need);
		}

		public bool Process(bool timeout)
		{
			if (!Valid || HasWinner || (!timeout && Rolls.Values.Any(r => !r.HasValue)))
			{
				return false;
			}

			Rolls.RemoveRange(
				(m, r) =>
				{
					if (r != null && (r.Value.Action == DungeonLootAction.Pass || r.Value.Mobile == null || r.Value.Mobile.Deleted))
					{
						r.Value.Free();
						return true;
					}
	
					if (m == null || m.Deleted)
					{
						if (r != null)
						{
							r.Value.Free();
						}
	
						return true;
					}
	
					return false;
				});

			if (Rolls.Count == 0)
			{
				return false;
			}

			var rolls = new List<DungeonLootRoll>(Rolls.Count);
			var count = 0;

			foreach (var r in Rolls.Values.Where(r => r != null))
			{
				rolls.Add(r.Value);

				++count;
			}

			if (count > 0 && rolls.Any(r => r.Action == DungeonLootAction.Need))
			{
				count -= rolls.RemoveAll(r => r.Action != DungeonLootAction.Need);
			}

			if (count <= 0)
			{
				rolls.Free(true);
				return false;
			}

			if (count > 1)
			{
				rolls.Sort();
			}

			var roll = rolls[0];

			Winner = roll.Mobile;

			if (Dungeon != null)
			{
				Dungeon.OnLootWin(this, roll, timeout);
			}

			rolls.Free(true);

			return HasWinner;
		}

		public void Free()
		{
			if (Rolls != null)
			{
				foreach (var r in Rolls.Values.Where(r => r.HasValue))
				{
					r.Value.Free();
				}

				Rolls.Clear();
				Rolls = null;
			}

			if (Item != null && !Item.Movable)
			{
				Item.Movable = true;
			}

			Item = null;
		}

		public override int GetHashCode()
		{
			return Item == null ? 0 : Item.Serial.Value;
		}

		public override string ToString()
		{
			return Item == null ? "Unknown Item" : Item.ResolveName();
		}

		public virtual void Serialize(GenericWriter writer)
		{
			writer.SetVersion(0);

			writer.Write(Item);
			writer.Write(Created);
			writer.WriteDeltaTime(Expire);
			writer.Write(Winner);

			writer.WriteDictionary(
				Rolls,
				(w, k, v) =>
				{
					w.Write(k);
	
					if (v != null)
					{
						w.Write(true);
						v.Value.Serialize(w);
					}
					else
					{
						w.Write(false);
					}
				});
		}

		public virtual void Deserialize(GenericReader reader)
		{
			reader.GetVersion();

			Item = reader.ReadItem();
			Created = reader.ReadDateTime();
			Expire = reader.ReadDeltaTime();
			Winner = reader.ReadMobile<PlayerMobile>();

			Rolls = reader.ReadDictionary(
				r =>
				{
					var k = r.ReadMobile<PlayerMobile>();
					DungeonLootRoll? v = null;
	
					if (r.ReadBool())
					{
						v = new DungeonLootRoll(r);
					}
	
					return new KeyValuePair<PlayerMobile, DungeonLootRoll?>(k, v);
				});
		}
	}
}
