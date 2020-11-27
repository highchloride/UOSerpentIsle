#region Header
//   Vorspire    _,-'/-'/  Dungeon_Sounds.cs
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
		public void GroupSound(Func<Mobile, int> resolver)
		{
			if (Deleted || Map == null || resolver == null)
			{
				return;
			}

			foreach (var m in Group.Where(m => m != null && !m.Deleted && m.IsOnline()))
			{
				m.SendSound(resolver(m));
			}
		}

		public void GroupSound(int id)
		{
			if (Deleted || Map == null || id <= 0)
			{
				return;
			}

			foreach (var m in Group.Where(m => m != null && !m.Deleted && m.IsOnline()))
			{
				m.SendSound(id);
			}
		}

		public void ZoneSound(Func<Mobile, int> resolver)
		{
			if (Deleted || Map == null || resolver == null)
			{
				return;
			}

			foreach (var m in FindMobiles<PlayerMobile>(m => m != null && !m.Deleted && m.IsOnline()))
			{
				m.SendSound(resolver(m));
			}
		}

		public void ZoneSound(int id)
		{
			if (Deleted || Map == null || id <= 0)
			{
				return;
			}

			foreach (var m in FindMobiles<PlayerMobile>(m => m != null && !m.Deleted && m.IsOnline()))
			{
				m.SendSound(id);
			}
		}

		public void GlobalSound(Func<Mobile, int> resolver)
		{
			if (Deleted || Map == null || resolver == null)
			{
				return;
			}

			foreach (var m in Map.Mobiles.OfType<PlayerMobile>().Where(m => !m.Deleted && m.IsOnline()))
			{
				m.SendSound(resolver(m));
			}
		}

		public void GlobalSound(int id)
		{
			if (Deleted || Map == null || id <= 0)
			{
				return;
			}

			foreach (var m in Map.Mobiles.OfType<PlayerMobile>().Where(m => !m.Deleted && m.IsOnline()))
			{
				m.SendSound(id);
			}
		}

		public void WorldSound(Func<Mobile, int> resolver)
		{
			if (Deleted || Map == null || resolver == null)
			{
				return;
			}

			foreach (var m in
				NetState.Instances.Where(n => n != null && n.Running && n.Socket != null && n.Mobile is PlayerMobile)
						.Select(n => n.Mobile))
			{
				m.SendSound(resolver(m));
			}
		}

		public void WorldSound(int id)
		{
			if (Deleted || Map == null || id <= 0)
			{
				return;
			}

			foreach (var m in
				NetState.Instances.Where(n => n != null && n.Running && n.Socket != null && n.Mobile is PlayerMobile)
						.Select(n => n.Mobile))
			{
				m.SendSound(id);
			}
		}

		public void SendSound(PlayerMobile m, int id)
		{
			if (m != null && !m.Deleted && id > 0)
			{
				m.SendSound(id);
			}
		}

		public void PlaySound(IPoint3D loc, int id)
		{
			if (loc != null && id > 0)
			{
				Effects.PlaySound(loc, Map, id);
			}
		}
	}
}