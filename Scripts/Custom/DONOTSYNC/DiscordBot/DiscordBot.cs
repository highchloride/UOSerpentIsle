#region Header
//   Vorspire    _,-'/-'/  DiscordBot.cs
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
using System.Linq;

using Server;
using Server.Misc;

using VitaNex.Collections;
using VitaNex.IO;
using VitaNex.Modules.AutoPvP;
using VitaNex.Text;
using VitaNex.Web;
#endregion

namespace VitaNex.Modules.Discord
{
	public static partial class DiscordBot
	{
		public const AccessLevel Access = AccessLevel.Administrator;

		private static readonly string[] _SaveMessages;

		private static string _LastMessage;

		private static readonly DictionaryPool<string, object> _Pool;

		public static DiscordBotOptions CMOptions { get; private set; }
		/*
		private static void OnWorldBroadcast(WorldBroadcastEventArgs e)
		{
			if (CMOptions.HandleBroadcast)
			{
				SendMessage(e.Text);
			}
		}
		*/

		private static void OnServerShutdown(ShutdownEventArgs e)
		{
			if (CMOptions.HandleStatus)
			{
				SendMessage("Status: Offline");
			}
		}

		private static void OnServerCrashed(CrashedEventArgs e)
		{
			if (CMOptions.HandleStatus)
			{
				SendMessage("Status: Offline (Back Soon!)");
			}
		}

		private static void OnServerStarted()
		{
			if (CMOptions.HandleStatus)
			{
				SendMessage("Status: Online");
			}
		}

		private static void OnNotifyBroadcast(string message)
		{
			if (CMOptions.HandleNotify)
			{
				SendMessage(message);
			}
		}

		private static void OnBattleWorldBroadcast(PvPBattle b, string text)
		{
			if (CMOptions.HandleBattles)
			{
				SendMessage(text);
			}
		}

		public static void SendMessage(string message)
		{
			SendMessage(message, true);
		}

		public static void SendMessage(string message, bool filtered)
		{
			if (!CMOptions.ModuleEnabled || String.IsNullOrWhiteSpace(message))
			{
				return;
			}

			var uri = GetWebhookUri();

			if (uri.Contains("NULL"))
			{
				return;
			}

			message = message.StripHtmlBreaks(true).StripHtml(false);

			if (filtered)
			{
				if (CMOptions.FilterSaves && _SaveMessages.Any(o => Insensitive.Contains(message, o)))
				{
					return;
				}

				if (CMOptions.FilterRepeat && _LastMessage == message)
				{
					return;
				}
			}

			_LastMessage = message;

			var d = _Pool.Acquire();

			d["content"] = message;
			d["username"] = ServerList.ServerName;
			d["file"] = null;
			d["embeds"] = null;

			WebAPI.BeginRequest(
				uri,
				d,
				(req, o) =>
				{
					req.Method = "POST";
					req.ContentType = FileMime.Lookup("json");
					req.SetContent(Json.Encode(o));

					_Pool.Free(o);
				},
				null);
		}

		public static string GetWebhookUri()
		{
			return GetWebhookUri(CMOptions.ModuleDebug);
		}

		public static string GetWebhookUri(bool debug)
		{
			return GetWebhook(debug).ToString();
		}

		public static Uri GetWebhook(bool debug)
		{
			var id = debug ? CMOptions.WebhookDebugID : CMOptions.WebhookID;
			var key = debug ? CMOptions.WebhookDebugKey : CMOptions.WebhookKey;

			if (String.IsNullOrWhiteSpace(id))
			{
				id = "NULL";
			}

			if (String.IsNullOrWhiteSpace(key))
			{
				key = "NULL";
			}

			return new Uri("https://discordapp.com/api/webhooks/" + id + "/" + key);
		}

		public static bool SetWebhook(string uri, bool debug)
		{
			try
			{
				return SetWebhook(new Uri(uri), debug);
			}
			catch
			{
				return false;
			}
		}

		public static bool SetWebhook(Uri u, bool debug)
		{
			try
			{
				var s = u.ToString();

				var i = s.IndexOf("discordapp.com/api/webhooks/", StringComparison.OrdinalIgnoreCase);

				if (i < 0 || i >= s.Length - 28)
				{
					return false;
				}

				s = s.Substring(i + 28);

				i = s.IndexOf('/');

				if (i <= 0 || i >= s.Length - 1)
				{
					return false;
				}

				if (debug)
				{
					CMOptions.WebhookDebugID = s.Substring(0, i);
					CMOptions.WebhookDebugKey = s.Substring(i + 1);
				}
				else
				{
					CMOptions.WebhookID = s.Substring(0, i);
					CMOptions.WebhookKey = s.Substring(i + 1);
				}

				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}