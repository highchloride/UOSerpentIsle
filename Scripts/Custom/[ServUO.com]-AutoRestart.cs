#region Header
//   Vorspire    _,-'/-'/  AutoRestart.cs
//   .      __,-; ,'( '/
//    \.    `-.__`-._`:_,-._       _ , . ``
//     `:-._,------' ` _,`--` -: `_ , ` ,' :
//        `---..__,,--'  (C) 2018  ` -'. -'
//        #  Vita-Nex [http://core.vita-nex.com]  #
//  {o)xxx|===============-   #   -===============|xxx(o}
//        #        The MIT License (MIT)          #
#endregion

#region References
using System;

using VitaNex;
using VitaNex.Schedules;
#endregion

namespace Server.Misc
{
	public static class AutoRestart
	{
		private static Schedule _Schedule;
		private static Timer _Timer;
		private static long _Expire;

		public static readonly TimeSpan WarningDuration = TimeSpan.FromMinutes(15);
		public static readonly TimeSpan WarningInterval = TimeSpan.FromMinutes(3);
		
		// Default state: this can be overridden in the [Schedules interface
		public static readonly bool Enabled = false;

		public static bool Restarting { get { return _Timer != null; } }

		public static void Configure()
		{
			// 12:00:00 noon
			var times = new ScheduleTimes(new TimeSpan(12, 00, 00));

			// Every month, every day (at 12 noon)
			_Schedule = new Schedule("Auto Restart", Enabled, ScheduleMonths.All, ScheduleDays.All, times, DoRestart);

			// Register the Schedule to appear in [Schedules interface.
			_Schedule.Register();

			EventSink.WorldLoad += Deserialize;
			EventSink.WorldSave += Serialize;
		}

		private static void DoRestart(Schedule s)
		{
			if (_Timer == null || !_Timer.Running)
			{
				_Expire = VitaNexCore.Ticks + (int)WarningDuration.TotalMilliseconds;
				_Timer = Timer.DelayCall(DoRestart);
			}
		}

		private static void DoRestart()
		{
			if (!_Schedule.Enabled)
			{
				_Expire = 0;

				_Timer.Stop();
				_Timer = null;

				World.Broadcast(0x22, true, "The server restart has been cancelled!");

				return;
			}

			var delay = TimeSpan.FromMilliseconds(_Expire - VitaNexCore.Ticks);

			if (delay > TimeSpan.Zero)
			{
				World.Broadcast(0x22, true, "The server will restart in {0}", delay.ToSimpleString("h:m:s"));

				if (delay > WarningInterval)
				{
					delay = WarningInterval;
				}

				_Timer = Timer.DelayCall(delay, DoRestart);
			}
			else
			{
				World.Broadcast(0x22, true, "The server is restarting now!");

				AutoSave.Save();
				
				_Timer = Timer.DelayCall(Core.Kill, true);
			}
		}

		private static void Serialize(WorldSaveEventArgs e)
		{
			Persistence.Serialize("Saves/Custom/AutoRestart.bin", _Schedule.Serialize);
		}

		private static void Deserialize()
		{
			Persistence.Deserialize("Saves/Custom/AutoRestart.bin", _Schedule.Deserialize);
		}
	}
}