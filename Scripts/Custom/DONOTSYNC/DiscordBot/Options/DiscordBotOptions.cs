#region Header
//   Vorspire    _,-'/-'/  DiscordBotOptions.cs
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
#endregion

namespace VitaNex.Modules.Discord
{
	public class DiscordBotOptions : CoreModuleOptions
	{
		[CommandProperty(DiscordBot.Access)]
		public string WebhookID { get; set; }

		[CommandProperty(DiscordBot.Access)]
		public string WebhookKey { get; set; }

		[CommandProperty(DiscordBot.Access)]
		public string WebhookDebugID { get; set; }

		[CommandProperty(DiscordBot.Access)]
		public string WebhookDebugKey { get; set; }

		[CommandProperty(DiscordBot.Access)]
		public bool FilterSaves { get; set; }

		[CommandProperty(DiscordBot.Access)]
		public bool FilterRepeat { get; set; }

		[CommandProperty(DiscordBot.Access)]
		public bool HandleBroadcast { get; set; }

		[CommandProperty(DiscordBot.Access)]
		public bool HandleNotify { get; set; }

		[CommandProperty(DiscordBot.Access)]
		public bool HandleBattles { get; set; }

		[CommandProperty(DiscordBot.Access)]
		public bool HandleStatus { get; set; }

		public DiscordBotOptions()
			: base(typeof(DiscordBot))
		{
			SetDefaults();
		}

		public DiscordBotOptions(GenericReader reader)
			: base(reader)
		{ }

		public override void Clear()
		{
			base.Clear();

			SetDefaults();
		}

		public override void Reset()
		{
			base.Reset();

			SetDefaults();
		}

		public void SetDefaults()
		{
			WebhookID = String.Empty;
			WebhookKey = String.Empty;
			WebhookDebugID = String.Empty;
			WebhookDebugKey = String.Empty;

			FilterSaves = true;
			FilterRepeat = true;

			HandleBroadcast = true;
			HandleNotify = true;
			HandleBattles = true;
			HandleStatus = true;
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.SetVersion(0);

			writer.Write(WebhookID);
			writer.Write(WebhookKey);

			writer.Write(WebhookDebugID);
			writer.Write(WebhookDebugKey);

			writer.Write(FilterSaves);
			writer.Write(FilterRepeat);

			writer.Write(HandleBroadcast);
			writer.Write(HandleNotify);
			writer.Write(HandleBattles);
			writer.Write(HandleStatus);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			reader.GetVersion();

			WebhookID = reader.ReadString();
			WebhookKey = reader.ReadString();

			WebhookDebugID = reader.ReadString();
			WebhookDebugKey = reader.ReadString();

			FilterSaves = reader.ReadBool();
			FilterRepeat = reader.ReadBool();

			HandleBroadcast = reader.ReadBool();
			HandleNotify = reader.ReadBool();
			HandleBattles = reader.ReadBool();
			HandleStatus = reader.ReadBool();
		}
	}
}