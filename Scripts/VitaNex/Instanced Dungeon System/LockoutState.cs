using System;
using System.Collections.Generic;

using Server;
using Server.Mobiles;

using VitaNex.Dungeons;

namespace VitaNex.InstanceMaps
{
	public sealed class LockoutState : PropertyObject
	{
		public PlayerMobile Owner { get; private set; }

		public Dictionary<DungeonID, DateTime> Entries { get; private set; }

		public bool IsEmpty { get { return Entries.Count == 0; } }

		public LockoutState(PlayerMobile owner)
		{
			Owner = owner;

			Entries = new Dictionary<DungeonID, DateTime>(0x20);
		}

		public LockoutState(GenericReader reader)
			: base(reader)
		{ }

		public override void Clear()
		{
			Entries.Clear();
		}

		public override void Reset()
		{
			Entries.Clear();
		}

		public TimeSpan GetLockout(DungeonID id)
		{
			var t = TimeSpan.Zero;

			DateTime d;

			if (Entries.TryGetValue(id, out d))
			{
				var n = DateTime.UtcNow;

				if (d > n)
				{
					t = d - n;
				}
				else
				{
					Entries.Remove(id);
				}
			}

			return t;
		}

		public void SetLockout(DungeonID id, TimeSpan t)
		{
			if (t > TimeSpan.Zero)
			{
				Entries[id] = DateTime.UtcNow + t;
			}
			else
			{
				Entries.Remove(id);
			}
		}

		public void ClearLockout(DungeonID id)
		{
			SetLockout(id, TimeSpan.Zero);
		}

		public bool IsLockedOut(DungeonID id)
		{
			return GetLockout(id) > TimeSpan.Zero;
		}

		public bool IsLockedOut(DungeonID id, out TimeSpan t)
		{
			return (t = GetLockout(id)) > TimeSpan.Zero;
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			var version = writer.SetVersion(0);

			writer.Write(Owner);

			switch (version)
			{
				case 0:
				{
					writer.WriteDictionary(
						Entries,
						(w, k, v) =>
						{
							w.WriteFlag(k);
							w.Write(v);
						});
				}
					break;
			}
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			var version = reader.GetVersion();

			Owner = reader.ReadMobile<PlayerMobile>();

			switch (version)
			{
				case 0:
				{
					Entries = reader.ReadDictionary(
						r =>
						{
							var k = r.ReadFlag<DungeonID>();
							var v = r.ReadDateTime();

							return new KeyValuePair<DungeonID, DateTime>(k, v);
						},
						Entries);
				}
					break;
			}
		}
	}
}