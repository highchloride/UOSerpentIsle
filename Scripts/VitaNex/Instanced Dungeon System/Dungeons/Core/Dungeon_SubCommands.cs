#region Header
//   Vorspire    _,-'/-'/  Dungeon_SubCommands.cs
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
using System.Globalization;
using System.Linq;

using Server;
using Server.Gumps;
using Server.Mobiles;

using VitaNex.InstanceMaps;
#endregion

namespace VitaNex.Dungeons
{
	public class DungeonCommandState
	{
		public virtual Dungeon Dungeon { get; protected set; }
		public virtual PlayerMobile Mobile { get; protected set; }
		public virtual string Command { get; protected set; }
		public virtual string Speech { get; protected set; }
		public virtual string[] Args { get; protected set; }

		public DungeonCommandState(Dungeon dungeon, PlayerMobile from, string command, string[] args)
		{
			Dungeon = dungeon;
			Mobile = from;
			Command = command;
			Args = args;
			Speech = String.Join(" ", args);
		}
	}

	public class DungeonCommandInfo
	{
		public virtual string Command { get; protected set; }
		public virtual string Usage { get; set; }
		public virtual string Description { get; set; }
		public virtual AccessLevel Access { get; set; }
		public virtual Func<DungeonCommandState, bool> Handler { get; set; }

		public DungeonCommandInfo(
			string command,
			string desc,
			string usage,
			AccessLevel access,
			Func<DungeonCommandState, bool> handler)
		{
			Command = command;
			Description = desc;
			Usage = usage;
			Access = access;
			Handler = handler;
		}
	}

	public abstract partial class Dungeon
	{
		private char _SubCommandPrefix = '@';

		[CommandProperty(AccessLevel.GameMaster)]
		public virtual char SubCommandPrefix
		{
			get
			{
				if (!Char.IsSymbol(_SubCommandPrefix))
				{
					_SubCommandPrefix = '@';
				}

				return _SubCommandPrefix;
			}
			set
			{
				if (!Char.IsSymbol(value))
				{
					value = '@';
				}

				_SubCommandPrefix = value;
			}
		}

		public Dictionary<string, DungeonCommandInfo> SubCommands { get; private set; }

		protected virtual void RegisterSubCommands()
		{
			RegisterSubCommand(
				"help",
				state =>
				{
					if (state == null || state.Mobile == null || state.Mobile.Deleted)
					{
						return false;
					}
	
					foreach (var ci in
						SubCommands.Keys.Select(cmd => SubCommands[cmd]).Where(ci => state.Mobile.AccessLevel >= ci.Access))
					{
						state.Mobile.SendMessage("{0}{1} {2}", SubCommandPrefix, ci.Command, ci.Usage);
					}

					return true;
				},
				"Displays a list of available commands for this dungeon.",
				"[?]",
				AccessLevel.Player);
			RegisterSubCommandAlias("help", "commands");

			RegisterSubCommand(
				"options",
				state =>
				{
					if (state == null || state.Mobile == null || state.Mobile.Deleted)
					{
						return false;
					}
	
					state.Mobile.SendGump(new PropertiesGump(state.Mobile, this));

					return true;
				},
				"Opens the options for this dungeon.");
			RegisterSubCommandAlias("options", "props", "config");

			RegisterSubCommand(
				"delete",
				state =>
				{
					if (state == null || state.Mobile == null || state.Mobile.Deleted)
					{
						return false;
					}
	
					Delete();
	
					return true;
				},
				"Deletes this dungeon and removes the entire region, including all decorations and spawns.");

			RegisterSubCommand(
				"extend",
				state =>
				{
					if (state == null || state.Mobile == null || state.Mobile.Deleted || String.IsNullOrWhiteSpace(state.Speech))
					{
						return false;
					}
	
					var input = state.Args.Length > 0 ? state.Args[0] : String.Empty;
	
					if (!String.IsNullOrWhiteSpace(input))
					{
						TimeSpan t;
	
						if (TimeSpan.TryParse(input, out t))
						{
							Deadline += t;
	
							state.Mobile.SendMessage("Time extended: +{0}", t);
							return true;
						}
					}
	
					return false;
				},
				"Extend the amount of time left for this dungeon.",
				"hh:mm:ss");

			RegisterSubCommand(
				"group",
				state =>
				{
					if (state == null || state.Mobile == null || state.Mobile.Deleted || String.IsNullOrWhiteSpace(state.Speech))
					{
						return false;
					}
	
					GroupMessage(state.Mobile.SpeechHue, "{0}: {1}", state.Mobile.RawName, state.Speech);
	
					return true;
				},
				"Send a message to everyone in your group.",
				"",
				AccessLevel.Player);
			RegisterSubCommandAlias("group", "g");

			RegisterSubCommand(
				"zone",
				state =>
				{
					if (state == null || state.Mobile == null || state.Mobile.Deleted || String.IsNullOrWhiteSpace(state.Speech))
					{
						return false;
					}
	
					ZoneMessage(state.Mobile.SpeechHue, "{0}: {1}", state.Mobile.RawName, state.Speech);
	
					return true;
				},
				"Send a message to everyone in this dungeon.",
				"",
				AccessLevel.Player);
			RegisterSubCommandAlias("zone", "z");

			RegisterSubCommand(
				"dungeon",
				state =>
				{
					if (state == null || state.Mobile == null || state.Mobile.Deleted || String.IsNullOrWhiteSpace(state.Speech))
					{
						return false;
					}
	
					foreach (var d in Instances.Dungeons.Values.Where(d => d != null && d.ID == ID))
					{
						d.ZoneMessage(state.Mobile.SpeechHue, "{0}: {1}", state.Mobile.RawName, state.Speech);
					}
	
					return true;
				},
				"Send a message to everyone in this dungeon, in all alternate realities.",
				"",
				AccessLevel.Player);
			RegisterSubCommandAlias("dungeon", "d");
		}

		public void RegisterSubCommand(
			string command,
			Func<DungeonCommandState, bool> handler,
			string desc = "",
			string usage = "",
			AccessLevel access = Instances.Access)
		{
			if (!String.IsNullOrWhiteSpace(command) && handler != null)
			{
				RegisterSubCommand(new DungeonCommandInfo(command, desc, usage, access, handler));
			}
		}

		public void RegisterSubCommand(DungeonCommandInfo info)
		{
			if (info == null)
			{
				return;
			}

			if (SubCommands.ContainsKey(info.Command))
			{
				SubCommands[info.Command] = info;
			}
			else
			{
				SubCommands.Add(info.Command, info);
			}
		}

		public void RegisterSubCommandAlias(string command, params string[] alias)
		{
			if (!IsCommand(command))
			{
				return;
			}

			var info = SubCommands[command];

			foreach (var cmd in alias)
			{
				RegisterSubCommand(cmd, info.Handler, info.Description, info.Usage, info.Access);
			}
		}

		public bool HandleSubCommand(DungeonZone zone, PlayerMobile pm, string speech)
		{
			if (pm == null || pm.Deleted || String.IsNullOrWhiteSpace(speech))
			{
				return false;
			}

			speech = speech.Trim();

			if (!speech.StartsWith(SubCommandPrefix.ToString(CultureInfo.InvariantCulture)))
			{
				return false;
			}

			var command = String.Empty;
			var args = new string[0];

			speech = speech.TrimStart(SubCommandPrefix);

			var split = speech.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);

			if (split.Length > 0)
			{
				command = split[0];
				args = new string[split.Length - 1];

				for (var i = 0; i < args.Length; i++)
				{
					args[i] = split[i + 1];
				}
			}

			if (!IsCommand(command))
			{
				pm.SendMessage("Command not found.");
				return true;
			}

			var info = SubCommands[command];

			if (pm.AccessLevel < info.Access)
			{
				pm.SendMessage("You do not have access to that command.");
				return true;
			}

			if (args.Length > 0 && (args[0] == "?" || Insensitive.Equals(args[0], "help")))
			{
				pm.SendMessage("Usage: {0}{1} {2}", SubCommandPrefix, info.Command, info.Usage);
				pm.SendMessage(info.Description);
				return true;
			}

			var state = new DungeonCommandState(this, pm, command, args);

			if (info.Handler.Invoke(state))
			{
				OnCommand(state);
				return true;
			}

			pm.SendMessage("Usage: {0}{1} {2}", SubCommandPrefix, info.Command, info.Usage);
			pm.SendMessage(info.Description);
			return true;
		}

		public bool IsCommand(string command)
		{
			return !String.IsNullOrWhiteSpace(command) && SubCommands.ContainsKey(command);
		}

		protected virtual void OnCommand(DungeonCommandState state)
		{ }
	}
}