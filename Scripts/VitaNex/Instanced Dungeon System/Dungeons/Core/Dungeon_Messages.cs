#region Header
//   Vorspire    _,-'/-'/  Dungeon_Messages.cs
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
using Server.Mobiles;
using Server.Network;
#endregion

namespace VitaNex.Dungeons
{
	public abstract partial class Dungeon
	{
		public void GroupMessage(Func<Mobile, string> resolver)
		{
			GroupMessage(946, resolver);
		}

		public void GroupMessage(int hue, Func<Mobile, string> resolver)
		{
			if (Deleted || Map == null || resolver == null)
			{
				return;
			}

			foreach (var m in Group.Where(m => m != null && !m.Deleted && m.IsOnline()))
			{
				m.SendMessage(hue, "[{0}] {1}", ID, resolver(m));
			}
		}

		public void GroupMessage(string message, params object[] args)
		{
			GroupMessage(946, message, args);
		}

		public void GroupMessage(int hue, string message, params object[] args)
		{
			message = String.Format(message, args);

			if (Deleted || Map == null || String.IsNullOrWhiteSpace(message))
			{
				return;
			}

			message = String.Format("[{0}] {1}", ID, message);

			foreach (var m in Group.Where(m => m != null && !m.Deleted && m.IsOnline()))
			{
				m.SendMessage(hue, message);
			}
		}

		public void ZoneMessage(Func<Mobile, string> resolver)
		{
			ZoneMessage(946, resolver);
		}

		public void ZoneMessage(int hue, Func<Mobile, string> resolver)
		{
			if (Deleted || Map == null || resolver == null)
			{
				return;
			}

			foreach (var m in FindMobiles<PlayerMobile>(m => m != null && !m.Deleted && m.IsOnline()))
			{
				m.SendMessage(hue, "[{0}] {1}", ID, resolver(m));
			}
		}

		public void ZoneMessage(string message, params object[] args)
		{
			ZoneMessage(946, message, args);
		}

		public void ZoneMessage(int hue, string message, params object[] args)
		{
			message = String.Format(message, args);

			if (Deleted || Map == null || String.IsNullOrWhiteSpace(message))
			{
				return;
			}

			message = String.Format("[{0}] {1}", ID, message);

			foreach (var m in FindMobiles<PlayerMobile>(m => m != null && !m.Deleted && m.IsOnline()))
			{
				m.SendMessage(hue, message);
			}
		}

		public void GlobalMessage(Func<Mobile, string> resolver)
		{
			GlobalMessage(946, resolver);
		}

		public void GlobalMessage(int hue, Func<Mobile, string> resolver)
		{
			if (Deleted || Map == null || resolver == null)
			{
				return;
			}

			foreach (var m in Map.Mobiles.OfType<PlayerMobile>().Where(m => !m.Deleted && m.IsOnline()))
			{
				m.SendMessage(hue, "[{0}] {1}", ID, resolver(m));
			}
		}

		public void GlobalMessage(string message, params object[] args)
		{
			GlobalMessage(946, message, args);
		}

		public void GlobalMessage(int hue, string message, params object[] args)
		{
			message = String.Format(message, args);

			if (Deleted || Map == null || String.IsNullOrWhiteSpace(message))
			{
				return;
			}

			message = String.Format("[{0}] {1}", ID, message);

			foreach (var m in Map.Mobiles.OfType<PlayerMobile>().Where(m => !m.Deleted && m.IsOnline()))
			{
				m.SendMessage(hue, message);
			}
		}

		public void WorldMessage(Func<Mobile, string> resolver)
		{
			WorldMessage(946, resolver);
		}

		public void WorldMessage(int hue, Func<Mobile, string> resolver)
		{
			if (Deleted || Map == null || resolver == null)
			{
				return;
			}

			foreach (var m in
				NetState.Instances.Where(n => n != null && n.Running && n.Socket != null && n.Mobile is PlayerMobile)
						.Select(n => n.Mobile))
			{
				m.SendMessage(hue, resolver(m));
			}
		}

		public void WorldMessage(string message, params object[] args)
		{
			WorldMessage(946, message, args);
		}

		public void WorldMessage(int hue, string message, params object[] args)
		{
			message = String.Format(message, args);

			if (Deleted || Map == null || String.IsNullOrWhiteSpace(message))
			{
				return;
			}

			message = String.Format("[{0}] {1}", ID, message);

			foreach (var m in
				NetState.Instances.Where(n => n != null && n.Running && n.Socket != null && n.Mobile is PlayerMobile)
						.Select(n => n.Mobile))
			{
				m.SendMessage(hue, message);
			}
		}
	}
}