using Server.Items;
using System;

namespace Server.Commands
{
	public class ShardTime
	{
        //Overwritten with Felladrin's time/region time command.
		public static void Initialize()
		{
			CommandSystem.Register( "Time", AccessLevel.Player, new CommandEventHandler( Time_OnCommand ) );
		}

		[Usage( "Time" )]
		[Description( "Returns the server's local time." )]
		private static void Time_OnCommand( CommandEventArgs e )
		{
            //e.Mobile.SendMessage( DateTime.UtcNow.ToString() );
            var m = e.Mobile;
            int currentHour, currentMinute;
            Clock.GetTime(m.Map, m.X, m.Y, out currentHour, out currentMinute);
            m.SendMessage("It's {0} now in {1}.", System.DateTime.Parse(currentHour + ":" + currentMinute).ToString("HH:mm"), (m.Region.Name ?? m.Map.Name));
            m.SendMessage("Real-World UTC: {0}, Server Time: {1}.", System.DateTime.UtcNow.ToString("HH:mm"), System.DateTime.Now.ToString("HH:mm"));
        }
	}
}
