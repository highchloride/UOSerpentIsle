#region Header
//   Vorspire    _,-'/-'/  DungeonLootRoll.cs
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
using System.Globalization;

using Server;
using Server.Mobiles;
#endregion

namespace VitaNex.Dungeons
{
	public struct DungeonLootRoll : IEquatable<DungeonLootRoll>, IComparable<DungeonLootRoll>
	{
		public PlayerMobile Mobile { get; private set; }
		public DungeonLootAction Action { get; private set; }
		public int Value { get; private set; }

		public DungeonLootRoll(PlayerMobile m, DungeonLootAction action, int value)
			: this()
		{
			Mobile = m;
			Action = action;
			Value = value;
		}

		public DungeonLootRoll(GenericReader reader)
			: this()
		{
			Deserialize(reader);
		}

		public void Free()
		{
			Mobile = null;
		}

		public int CompareTo(DungeonLootRoll other)
		{
			if (Action > other.Action)
			{
				return -1;
			}

			if (Action < other.Action)
			{
				return 1;
			}

			if (Value > other.Value)
			{
				return -1;
			}

			if (Value < other.Value)
			{
				return 1;
			}

			return 0;
		}

		public override string ToString()
		{
			return Value.ToString(CultureInfo.InvariantCulture);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hash = Mobile != null ? Mobile.Serial.Value : 0;

				hash = (hash * 397) ^ (int)Action;
				hash = (hash * 397) ^ Value;

				return hash;
			}
		}

		public override bool Equals(object obj)
		{
			return obj is DungeonLootRoll && Equals((DungeonLootRoll)obj);
		}

		public bool Equals(DungeonLootRoll other)
		{
			return Mobile == other.Mobile && Action == other.Action && Value == other.Value;
		}

		public static implicit operator int(DungeonLootRoll roll)
		{
			return roll.Value;
		}

		public void Serialize(GenericWriter writer)
		{
			writer.SetVersion(0);

			writer.Write(Mobile);
			writer.WriteFlag(Action);
			writer.Write(Value);
		}

		public void Deserialize(GenericReader reader)
		{
			reader.GetVersion();

			Mobile = reader.ReadMobile<PlayerMobile>();
			Action = reader.ReadFlag<DungeonLootAction>();
			Value = reader.ReadInt();
		}
	}
}