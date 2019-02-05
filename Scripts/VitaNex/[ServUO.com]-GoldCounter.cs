#region Header
//   Vorspire    _,-'/-'/  GoldCounter.cs
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
using System.Linq;
using System.Text;

using Server.Accounting;
using Server.Commands;
using Server.Gumps;
using Server.Misc;
using Server.Network;

using VitaNex;
using VitaNex.Notify;
#endregion

namespace Server.Items
{
	public static class GoldCounter
	{
		public static void Configure()
		{
			CommandUtility.Register("CountGold", AccessLevel.Administrator, Handler);
		}

		[Usage("CountGold <staff:true|false>")]
		[Description("Pop a notification containing the world gold query results. Staff owned gold is excluded by default.")]
		private static void Handler(CommandEventArgs e)
		{
			GoldNotify(e.Mobile, e.GetBoolean(0));
		}

		public static void GoldNotify(Mobile m, bool includeStaff)
		{
			var html = new StringBuilder();

			html.AppendLine("{0} World Gold".WrapUOHtmlBig().WrapUOHtmlColor(Color.Gold), ServerList.ServerName);

			double ac, ch, go;
			var total = CountGold(includeStaff, out ac, out ch, out go);

			html.AppendLine("Total: {0}", total.ToString("#,0").WrapUOHtmlColor(Color.LawnGreen, Color.Gold));
			html.AppendLine();
			html.AppendLine("Accounts: {0}", ac.ToString("#,0").WrapUOHtmlColor(Color.LawnGreen, Color.Gold));
			html.AppendLine("Coins: {0}", go.ToString("#,0").WrapUOHtmlColor(Color.LawnGreen, Color.Gold));
			html.AppendLine("Checks: {0}", ch.ToString("#,0").WrapUOHtmlColor(Color.LawnGreen, Color.Gold));

			m.SendNotification<GoldCountNotifyGump>(html.ToString(), false);
		}

		public static double CountGold(bool includeStaff, out double accounts, out double checks, out double gold)
		{
			accounts = checks = gold = 0;

			NetState.Pause();

			try
			{
				int g;
				double t;

				accounts = Accounts.GetAccounts().Aggregate(
					accounts,
					(c, a) =>
					{
						if (!includeStaff && a.AccessLevel >= AccessLevel.Counselor)
						{
							return c;
						}

						a.GetGoldBalance(out g, out t);

						return c + t;
					});

				foreach (var i in World.Items.Values.Where(i => !i.Deleted && (i is Gold || i is BankCheck)))
				{
					if (!includeStaff)
					{
						var p = i.RootParent as Mobile;

						if (p != null && p.AccessLevel >= AccessLevel.Counselor)
						{
							continue;
						}
					}

					if (i is Gold)
					{
						gold += i.Amount;
					}
					else if (i is BankCheck)
					{
						checks += ((BankCheck)i).Worth;
					}
				}
			}
			catch (Exception e)
			{
				VitaNexCore.ToConsole(e);

				accounts = checks = gold = 0;
			}

			NetState.Resume();

			return accounts + gold + checks;
		}
	}
}

namespace Server.Gumps
{
	public class GoldCountNotifyGump : NotifyGump
	{
		private static readonly Size _SizeMax = new Size(SizeMax.Width, SizeMax.Height * 2);

		public GoldCountNotifyGump(Mobile user, string html)
			: base(user, html)
		{ }

		protected override Size GetSizeMax()
		{
			return _SizeMax;
		}
	}
}