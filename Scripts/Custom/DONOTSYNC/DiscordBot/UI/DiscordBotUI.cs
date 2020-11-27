#region Header
//   Vorspire    _,-'/-'/  DiscordBotUI.cs
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

using Server;
using Server.Commands;
using Server.Gumps;

using VitaNex.SuperGumps;
#endregion

namespace VitaNex.Modules.Discord
{
	public class DiscordBotUI : SuperGump
	{
		public DiscordBotUI(Mobile user)
			: base(user)
		{ }

		protected override void CompileLayout(SuperGumpLayout layout)
		{
			base.CompileLayout(layout);

			const int w = 600, h = 400;

			layout.Add("bg", () => AddBackground(0, 0, w, h, SupportsUltimaStore ? 40000 : 9270));

			layout.Add(
				"title",
				() =>
				{
					var label = "Discord Bot Settings";

					label = label.WrapUOHtmlBig();
					label = label.WrapUOHtmlCenter();
					label = label.WrapUOHtmlColor(Color.Gold, false);

					AddHtml(15, 10, w - 30, 40, label, false, false);
				});

			layout.Add(
				"webhook/live",
				() =>
				{
					var label = "Live Webhook";

					label = label.WrapUOHtmlRight();
					label = label.WrapUOHtmlColor(Color.Gold, false);

					const int ww = (w - 30) / 4;

					AddHtml(15, 40, ww, 40, label, false, false);

					AddImageTiled(15 + (ww + 10), 40, (w - 30) - (ww + 10), 20, 2624);

					AddTextEntry(
						15 + (ww + 15),
						40,
						(w - 30) - (ww + 20),
						20,
						TextHue,
						DiscordBot.GetWebhookUri(false),
						(e, t) => DiscordBot.SetWebhook(t, false));
				});

			layout.Add(
				"webhook/debug",
				() =>
				{
					var label = "Debug Webhook";

					label = label.WrapUOHtmlRight();
					label = label.WrapUOHtmlColor(Color.Gold, false);

					const int ww = (w - 30) / 4;

					AddHtml(15, 60, ww, 40, label, false, false);

					AddImageTiled(15 + (ww + 10), 60, (w - 30) - (ww + 10), 20, 2624);

					AddTextEntry(
						15 + (ww + 15),
						60,
						(w - 30) - (ww + 20),
						20,
						TextHue,
						DiscordBot.GetWebhookUri(true),
						(e, t) => DiscordBot.SetWebhook(t, true));
				});

			layout.Add(
				"options",
				() =>
				{
					var xx = 15;
					var yy = 90;

					const int ww = (w - 30) / 4;

					var col = DiscordBot.CMOptions.FilterSaves ? Color.LawnGreen : Color.OrangeRed;

					AddHtmlButton(
						xx,
						yy,
						ww,
						25,
						b =>
						{
							DiscordBot.CMOptions.FilterSaves = !DiscordBot.CMOptions.FilterSaves;
							Refresh(true);
						},
						"Saves Filter".WrapUOHtmlCenter(),
						col,
						Color.Black,
						Color.Silver,
						1);

					xx += ww;

					col = DiscordBot.CMOptions.FilterRepeat ? Color.LawnGreen : Color.OrangeRed;

					AddHtmlButton(
						xx,
						yy,
						ww,
						25,
						b =>
						{
							DiscordBot.CMOptions.FilterRepeat = !DiscordBot.CMOptions.FilterRepeat;
							Refresh(true);
						},
						"Repeat Filter".WrapUOHtmlCenter(),
						col,
						Color.Black,
						col,
						1);

					xx = 15;
					yy += 35;

					col = DiscordBot.CMOptions.HandleBroadcast ? Color.LawnGreen : Color.OrangeRed;

					AddHtmlButton(
						xx,
						yy,
						ww,
						25,
						b =>
						{
							DiscordBot.CMOptions.HandleBroadcast = !DiscordBot.CMOptions.HandleBroadcast;
							Refresh(true);
						},
						"Handle Broadcasts".WrapUOHtmlCenter(),
						col,
						Color.Black,
						Color.Silver,
						1);

					xx += ww;

					col = DiscordBot.CMOptions.HandleNotify ? Color.LawnGreen : Color.OrangeRed;

					AddHtmlButton(
						xx,
						yy,
						ww,
						25,
						b =>
						{
							DiscordBot.CMOptions.HandleNotify = !DiscordBot.CMOptions.HandleNotify;
							Refresh(true);
						},
						"Handle Notifications".WrapUOHtmlCenter(),
						col,
						Color.Black,
						col,
						1);

					xx += ww;

					col = DiscordBot.CMOptions.HandleBattles ? Color.LawnGreen : Color.OrangeRed;

					AddHtmlButton(
						xx,
						yy,
						ww,
						25,
						b =>
						{
							DiscordBot.CMOptions.HandleBattles = !DiscordBot.CMOptions.HandleBattles;
							Refresh(true);
						},
						"Handle PvP Battles".WrapUOHtmlCenter(),
						col,
						Color.Black,
						col,
						1);

					xx += ww;

					col = DiscordBot.CMOptions.HandleStatus ? Color.LawnGreen : Color.OrangeRed;

					AddHtmlButton(
						xx,
						yy,
						ww,
						25,
						b =>
						{
							DiscordBot.CMOptions.HandleStatus = !DiscordBot.CMOptions.HandleStatus;
							Refresh(true);
						},
						"Handle Server Status".WrapUOHtmlCenter(),
						col,
						Color.Black,
						col,
						1);

					xx = 15;
					yy += 35;

					AddHtmlButton(
						xx,
						yy,
						ww,
						25,
						b =>
						{
							Refresh();
							User.SendGump(new PropertiesGump(User, DiscordBot.CMOptions));
						},
						"Module Config".WrapUOHtmlCenter(),
						Color.Gold,
						Color.Black,
						Color.Gold,
						1);

					xx = 0;
					yy += 35;

					AddBackground(xx, yy, w, h - yy, SupportsUltimaStore ? 40000 : 9270);

					xx += 15;
					yy += 10;

					var label = "Information";

					label = label.WrapUOHtmlBig();
					label = label.WrapUOHtmlCenter();
					label = label.WrapUOHtmlColor(Color.Gold, false);

					AddHtml(xx, yy, w - 30, 40, label, false, false);

					yy += 35;

					label = Information;

					label = label.WrapUOHtmlColor(Color.PaleGoldenrod, false);

					AddHtml(xx, yy, w - 30, (h - 20) - yy, label, false, true);
				});
		}

		public const string WebhooksUri = "https://support.discordapp.com/hc/en-us/articles/228383668-Intro-to-Webhooks";

		public static readonly string Information = //
			"Configure Webhooks to use when sending messages to Discord.\n" +
			"Discord - Intro to Webhooks".WrapUOHtmlUrl(WebhooksUri) +
			"\n\nThe Debug Webhook will be used when the Discord Bot is in Debug Mode, otherwise, the Live Webhook is used.\n" +
			"Make sure to use a fully qualified URL when update your Webhook settings.\n\nYou may use the " +
			CommandSystem.Prefix + "Discord command to send messages directly to Discord.\n" +
			"Messages sent this way will not be subject to filtering.\n\n";
	}
}