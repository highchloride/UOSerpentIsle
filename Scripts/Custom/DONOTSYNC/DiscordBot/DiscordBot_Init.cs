#region Header
//   Vorspire    _,-'/-'/  DiscordBot_Init.cs
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

using Server;

using VitaNex.Collections;
#endregion

namespace VitaNex.Modules.Discord
{
	[CoreModule("Discord Bot", "1.0.0.0", false, TaskPriority.Lowest)]
	public static partial class DiscordBot
	{
		static DiscordBot()
		{
			_SaveMessages = new[] {"The world will save in", "The world is saving", "World save complete"};

			_Pool = new DictionaryPool<string, object>();

			CMOptions = new DiscordBotOptions();
		}

		private static void CMConfig()
		{
			EventSink.Shutdown += OnServerShutdown;
			EventSink.Crashed += OnServerCrashed;
			EventSink.ServerStarted += OnServerStarted;

			Notify.Notify.OnBroadcast += OnNotifyBroadcast;

			AutoPvP.AutoPvP.OnBattleWorldBroadcast += OnBattleWorldBroadcast;
		}

		private static void CMInvoke()
		{
			//EventSink.WorldBroadcast += OnWorldBroadcast;

			CommandUtility.Register(
				"Discord",
				Access,
				e =>
				{
					var message = String.Format("[{0}]: {1}", e.Mobile.RawName, e.ArgString);

					SendMessage(message, false);
				});

			CommandUtility.Register("DiscordAdmin", Access, e => new DiscordBotUI(e.Mobile).Send());
		}
	}
}
