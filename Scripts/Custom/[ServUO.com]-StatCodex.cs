#region Header
//   Vorspire    _,-'/-'/  StatCodex.cs
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
using System.Drawing;

using Server.Gumps;

using VitaNex.SuperGumps;
#endregion

namespace Server.Items
{
	public class StatCodex : Item
	{
		[Constructable]
		public StatCodex()
			: base(8793)
		{
			Name = "Stat Study Book";
			LootType = LootType.Blessed;
			Stackable = false;
		}

		public StatCodex(Serial serial)
			: base(serial)
		{ }

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);

			list.Add("Use: Modifies your character stats".WrapUOHtmlColor(Color.LawnGreen));
		}

		public override void OnDoubleClick(Mobile m)
		{
			if (this.CheckDoubleClick(m, true, false, 2, true))
			{
				new StatCodexUI(m, this).Send();
			}
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

    //Modified - Max is 75 points to spend, at an inc of 5 points.
	public sealed class StatCodexUI : SuperGump
	{
		public const int StatMin = 10, StatMax = 125;

		public StatCodex Codex { get; private set; }

		public int StatInc { get; private set; }

		//public int StatsCap { get { return User.StatCap; } }
        public int StatsCap { get { return 75; } }

		private int _Str = StatMin;
		private int _Dex = StatMin;
		private int _Int = StatMin;

		public int Str { get { return _Str; } set { _Str = Math.Max(StatMin, Math.Min(StatMax, value)); } }
		public int Dex { get { return _Dex; } set { _Dex = Math.Max(StatMin, Math.Min(StatMax, value)); } }
		public int Int { get { return _Int; } set { _Int = Math.Max(StatMin, Math.Min(StatMax, value)); } }

		public int StatTotal { get { return Str + Dex + Int; } }
		public int StatPoints { get { return StatsCap - StatTotal; } }

		public bool CanAddStat { get { return StatPoints > 0; } }
		public bool CanRemoveStat { get { return StatTotal > StatMin; } }

		public bool CanAddStr { get { return Str < StatMax; } }
		public bool CanRemoveStr { get { return Str > StatMin; } }

		public bool CanAddDex { get { return Dex < StatMax; } }
		public bool CanRemoveDex { get { return Dex > StatMin; } }

		public bool CanAddInt { get { return Int < StatMax; } }
		public bool CanRemoveInt { get { return Int > StatMin; } }

		public StatCodexUI(Mobile user, StatCodex codex)
			: base(user)
		{
			Codex = codex;

			StatInc = 5;

			Str = User.RawStr;
			Dex = User.RawDex;
			Int = User.RawInt;

			FixStats();
		}

		protected override bool OnBeforeSend()
		{
			return base.OnBeforeSend() && Codex != null && !Codex.Deleted && Codex.IsChildOf(User.Backpack);
		}

		protected override void CompileLayout(SuperGumpLayout layout)
		{
			base.CompileLayout(layout);

			const int width = 265, height = 200;

			var sup = SupportsUltimaStore;

			var bor = sup ? 15 : 10;

			layout.Add("bg", () => AddBackground(0, 0, width + (bor * 2), height + (bor * 2), sup ? 40000 : 9270));

			layout.Add(
				"title",
				() =>
				{
					var title = "Stat Study Book";
					title = title.WrapUOHtmlBig();
					title = title.WrapUOHtmlColor(Color.Gold, false);

					AddHtml(bor, bor, width, 40, title, false, false);
				});

			layout.Add("stats", () => RenderStatsTab(bor, bor + 25, width, height - 25));
		}

		private void AddIncControl(int x, ref int y, int w, ref int h, int o, Action<GumpButton> dec, Action<GumpButton> inc)
		{
			var label = String.Format("Inc: {0:#,0}", o);
			label = label.WrapUOHtmlSmall();
			label = label.WrapUOHtmlRight();
			label = label.WrapUOHtmlColor(GetColor(o, 1, 10), false);

			AddHtml(x, y, w - 35, 40, label, false, false);

			if (o > 1)
			{
				AddButton(x + (w - 30), y + 2, 2437, 2438, dec);
			}

			if (o < 10)
			{
				AddButton(x + (w - 15), y + 2, 2435, 2436, inc);
			}

			y += 20;
			h -= 20;
		}

		private void RenderStatsTab(int x, int y, int w, int h)
		{
			var label = String.Format("Total: {0:#,0} / {1:#,0}", StatTotal, StatsCap);
			label = label.WrapUOHtmlSmall();
			label = label.WrapUOHtmlColor(Color.White, false);

			AddHtml(x, y, w - (w / 3), 40, label, false, false);

			AddIncControl(x + (w - (w / 3)), ref y, w / 3, ref h, StatInc, DecreaseInc, IncreaseInc);

			// STR

			label = "Strength";
			label = label.WrapUOHtmlSmall();
			label = label.WrapUOHtmlColor(Color.Gold, false);

			AddHtml(x, y, w - 115, 40, label, false, false);

			label = String.Format("{0:#,0} / {1:#,0}", Str, StatMax);
			label = label.WrapUOHtmlSmall();
			label = label.WrapUOHtmlRight();
			label = label.WrapUOHtmlColor(GetColor(Str, StatMin, StatMax), false);

			AddHtml(x + (w - 115), y, 80, 40, label, false, false);

			if (CanRemoveStat && CanRemoveStr)
			{
				AddButton(
					x + (w - 30),
					y + 2,
					2437,
					2438,
					b =>
					{
						OffsetStat(StatType.Str, -StatInc);

						Refresh(true);
					});
			}

			if (CanAddStat && CanAddStr)
			{
				AddButton(
					x + (w - 15),
					y + 2,
					2435,
					2436,
					b =>
					{
						OffsetStat(StatType.Str, StatInc);

						Refresh(true);
					});
			}

			AddRectangle(x, y + 15, w, 1, Color.FromArgb(0xFF, 0x29, 0x31, 0x39), true);

			y += 15;
			h -= 15;

			// DEX

			label = "Dexterity";
			label = label.WrapUOHtmlSmall();
			label = label.WrapUOHtmlColor(Color.Gold, false);

			AddHtml(x, y, w - 115, 40, label, false, false);

			label = String.Format("{0:#,0} / {1:#,0}", Dex, StatMax);
			label = label.WrapUOHtmlSmall();
			label = label.WrapUOHtmlRight();
			label = label.WrapUOHtmlColor(GetColor(Dex, StatMin, StatMax), false);

			AddHtml(x + (w - 115), y, 80, 40, label, false, false);

			if (CanRemoveStat && CanRemoveDex)
			{
				AddButton(
					x + (w - 30),
					y + 2,
					2437,
					2438,
					b =>
					{
						OffsetStat(StatType.Dex, -StatInc);

						Refresh(true);
					});
			}

			if (CanAddStat && CanAddDex)
			{
				AddButton(
					x + (w - 15),
					y + 2,
					2435,
					2436,
					b =>
					{
						OffsetStat(StatType.Dex, StatInc);

						Refresh(true);
					});
			}

			AddRectangle(x, y + 15, w, 1, Color.FromArgb(0xFF, 0x29, 0x31, 0x39), true);

			y += 15;
			h -= 15;

			// INT

			label = "Intelligence";
			label = label.WrapUOHtmlSmall();
			label = label.WrapUOHtmlColor(Color.Gold, false);

			AddHtml(x, y, w - 115, 40, label, false, false);

			label = String.Format("{0:#,0} / {1:#,0}", Int, StatMax);
			label = label.WrapUOHtmlSmall();
			label = label.WrapUOHtmlRight();
			label = label.WrapUOHtmlColor(GetColor(Int, StatMin, StatMax), false);

			AddHtml(x + (w - 115), y, 80, 40, label, false, false);

			if (CanRemoveStat && CanRemoveInt)
			{
				AddButton(
					x + (w - 30),
					y + 2,
					2437,
					2438,
					b =>
					{
						OffsetStat(StatType.Int, -StatInc);

						Refresh(true);
					});
			}

			if (CanAddStat && CanAddInt)
			{
				AddButton(
					x + (w - 15),
					y + 2,
					2435,
					2436,
					b =>
					{
						OffsetStat(StatType.Int, StatInc);

						Refresh(true);
					});
			}

			AddRectangle(x, y + 15, w, 1, Color.FromArgb(0xFF, 0x29, 0x31, 0x39), true);

			y += 15;
			h -= 15;

			// ALL

			label = "All Stats";
			label = label.WrapUOHtmlSmall();
			label = label.WrapUOHtmlColor(Color.Gold, false);

			AddHtml(x, y, w - 115, 40, label, false, false);

			label = String.Format("{0:#,0} / {1:#,0}", StatTotal, StatsCap);
			label = label.WrapUOHtmlSmall();
			label = label.WrapUOHtmlRight();
			label = label.WrapUOHtmlColor(GetColor(StatTotal, StatMin * 3, StatsCap), false);

			AddHtml(x + (w - 115), y, 80, 40, label, false, false);

			if (CanRemoveStat)
			{
				AddButton(
					x + (w - 30),
					y + 2,
					2437,
					2438,
					b =>
					{
						OffsetStat(StatType.All, -StatInc);

						Refresh(true);
					});
			}

			if (CanAddStat)
			{
				AddButton(
					x + (w - 15),
					y + 2,
					2435,
					2436,
					b =>
					{
						OffsetStat(StatType.All, StatInc);

						Refresh(true);
					});
			}

			AddRectangle(x, y + 15, w, 1, Color.FromArgb(0xFF, 0x29, 0x31, 0x39), true);

			y += h - 50;
			//h -= h - 50;

			if (SupportsUltimaStore)
			{
				AddButton(x, y, 40019, 40029, Reset);

				label = "Reset";
				label = label.WrapUOHtmlCenter();
				label = label.WrapUOHtmlColor(Color.Yellow, false);

				AddHtml(x, y + 2, 126, 40, label, false, false);
			}
			else
			{
				label = "Reset";
				label = label.WrapUOHtmlCenter();

				AddHtmlButton(x, y, 126, 25, Reset, label, Color.Yellow, Color.Black);
			}

			y += 25;
			//h -= 25;

			if (SupportsUltimaStore)
			{
				AddButton(x, y, 40019, 40029, Close);

				label = "Cancel";
				label = label.WrapUOHtmlCenter();
				label = label.WrapUOHtmlColor(Color.OrangeRed, false);

				AddHtml(x, y + 2, 126, 40, label, false, false);
			}
			else
			{
				label = "Cancel";
				label = label.WrapUOHtmlCenter();

				AddHtmlButton(x, y, 126, 25, Close, label, Color.OrangeRed, Color.Black);
			}

			x += 130;
			//w -= 130;

			if (SupportsUltimaStore)
			{
				AddButton(x, y, 40019, 40029, Apply);

				label = "Apply";
				label = label.WrapUOHtmlCenter();
				label = label.WrapUOHtmlColor(Color.LawnGreen, false);

				AddHtml(x, y + 2, 126, 40, label, false, false);
			}
			else
			{
				label = "Apply";
				label = label.WrapUOHtmlCenter();

				AddHtmlButton(x, y, 126, 25, Apply, label, Color.LawnGreen, Color.Black);
			}
		}

		private void DecreaseInc(GumpButton b)
		{
			StatInc = Math.Max(1, StatInc - 1);

			Refresh(true);
		}

		private void IncreaseInc(GumpButton b)
		{
			StatInc = Math.Min(10, StatInc + 1);

			Refresh(true);
		}

		private void Reset(GumpButton b)
		{
			Str = User.RawStr;
			Dex = User.RawDex;
			Int = User.RawInt;

			FixStats();

			Refresh(true);
		}

		private void Apply(GumpButton b)
		{
			if (Codex == null || Codex.Deleted || !Codex.IsChildOf(User.Backpack))
			{
				Close();
				return;
			}

			var changed = false;

			if (User.RawStr != Str)
			{
				User.RawStr = Str;
				changed = true;
			}

			if (User.RawDex != Dex)
			{
				User.RawDex = Dex;
				changed = true;
			}

			if (User.RawInt != Int)
			{
				User.RawInt = Int;
				changed = true;
			}

			if (changed)
			{
				Codex.Consume();
			}

			Close();
		}

		private int GetStat(StatType stat)
		{
			switch (stat)
			{
				case StatType.Str:
					return Str;
				case StatType.Dex:
					return Dex;
				case StatType.Int:
					return Int;
				case StatType.All:
					return StatTotal;
			}

			return 0;
		}

		private void OffsetStat(StatType stat, int diff)
		{
			if (stat == StatType.All)
			{
				OffsetStat(StatType.Str, diff);
				OffsetStat(StatType.Dex, diff);
				OffsetStat(StatType.Int, diff);

				return;
			}

			SetStat(stat, GetStat(stat) + diff);
		}

		private void SetStat(StatType stat, int value)
		{
			if (stat == StatType.All)
			{
				SetStat(StatType.Str, value);
				SetStat(StatType.Dex, value);
				SetStat(StatType.Int, value);

				return;
			}

			var points = StatTotal - GetStat(stat);

			if (points + value > StatsCap)
			{
				value = StatsCap - points;
			}

			value = Math.Max(StatMin, Math.Min(StatMax, value));

			switch (stat)
			{
				case StatType.Str:
					_Str = value;
					break;
				case StatType.Dex:
					_Dex = value;
					break;
				case StatType.Int:
					_Int = value;
					break;
			}

			FixStats();
		}

		private void FixStats()
		{
			if (StatPoints >= 0)
			{
				return;
			}

			while (StatPoints < 0)
			{
				if (Str >= Dex && Str >= Int)
				{
					--Str;
				}
				else if (Dex >= Str && Dex >= Int)
				{
					--Dex;
				}
				else if (Int >= Str && Int >= Dex)
				{
					--Int;
				}
			}
		}

		private static Color GetColor(double val, double min, double max, double offset = 0)
		{
			if (val < min || offset < 0)
			{
				return Color.OrangeRed;
			}

			return Color.White.Interpolate(Color.LawnGreen, val / max);
		}
	}
}
