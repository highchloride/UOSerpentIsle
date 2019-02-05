using System;
using Server;
using Server.Misc;
using Server.Network;
using Server.Mobiles;
using Server.Commands;
using Server.Targeting;
using System.Collections;
using Server.Engines.Plants;
using Server.Engines.Quests;
using Server.Engines.Quests.Hag;
using System.Collections.Generic;
using Server.Commands.Generic;
using Server.Gumps;

namespace Server.Items
{
	public class FoodDecayTimer : Timer
	{
        static bool active = false;


        public static void Initialize()
		{
            //If these changes work, the hunger system will only be activated based on a value in server.cfg
            active = Config.Get("Server.UseHunger", false);

            new FoodDecayTimer().Start();
		}

		public FoodDecayTimer() : base( TimeSpan.FromMinutes( 10 ), TimeSpan.FromMinutes( 10 ) )
		{
			Priority = TimerPriority.OneMinute;
		}

		protected override void OnTick()
		{
            if(active)
			    FoodDecay();			
		}

		public static void FoodDecay()
		{
			foreach ( NetState state in NetState.Instances )
			{
				if ( state.Mobile != null && state.Mobile.AccessLevel == AccessLevel.Player && !state.Mobile.IsYoung() && state.ConnectedFor > new TimeSpan(5000) ) // Check if player and not young and actually connected
				{
					HungerDecay( state.Mobile );
					ThirstDecay( state.Mobile );
                }
			}
		}

		public static void HungerDecay( Mobile m )
		{
			if ( m != null )
			{
				if ( m.Hunger >= 1 )
				{
					m.Hunger -= 1;
					// added to give hunger value a real meaning.
					if ( m.Hunger < 5 )
						m.SendMessage( "Thou art extremely hungry." );
					else if ( m.Hunger < 10 )
						m.SendMessage( "Thou art getting very hungry." );
				}	
				else
				{
					if ( m.Hits > 5 )
						m.Hits -= 5;
					m.SendMessage( "Thou art starving to death!" );
				}
			}
		}

		public static void ThirstDecay( Mobile m )
		{
			if ( m != null )
			{
                if ( m.Thirst >= 1 )
				{
					m.Thirst -= 1;
				// added to give thirst value a real meaning.
					if ( m.Thirst < 5 )
						m.SendMessage( "Thou art extremely thirsty." );
					else if ( m.Thirst < 10 )
						m.SendMessage( "Thou art getting thirsty." );
				}
				else
				{
					if ( m.Stam > 5 )
						m.Stam -= 5;
					m.SendMessage( "Thou art exhausted from thirst" );
				}
			}
		}
	}
	// Create the timer that monitors the current state of hunger
	public class HitsDecayTimer : Timer
	{
		public static void Initialize()
		{
			new HitsDecayTimer().Start();
		}
		// Based on the same timespan used in RegenRates.cs
		public HitsDecayTimer() : base( TimeSpan.FromSeconds( 11 ), TimeSpan.FromSeconds( 11 ) )
		{
			Priority = TimerPriority.OneSecond;
		}
		
		protected override void OnTick()
		{
			HitsDecay();
		}
		// Check the NetState and call the decaying function
		public static void HitsDecay()
		{
			foreach ( NetState state in NetState.Instances )
			{
				HitsDecaying( state.Mobile );
			}
		}

		// Check hunger level if below the value set take away 1 hit
		public static void HitsDecaying( Mobile m )
		{
			if ( m != null && m.Hunger < 5 && m.Hits > 3 )
			{
				switch (m.Hunger)
				{
					case 4: m.Hits -= 1; break;
					case 3: m.Hits -= 1; break;
					case 2: m.Hits -= 2; break;
					case 1: m.Hits -= 2; break;
					case 0:
					{
						m.Hits -= 3;
						m.SendMessage( "Thou art starving to death!" );
						break;
					}
				}
			}
		}
	}
	// Create the timer that monitors the current state of thirst
	public class StamDecayTimer : Timer
	{
		public static void Initialize()
		{
			new StamDecayTimer().Start();
		}
		// Based on the same timespan used in RegenRates.cs
		public StamDecayTimer() : base( TimeSpan.FromSeconds( 7 ), TimeSpan.FromSeconds( 7 ) )
		{
			Priority = TimerPriority.OneSecond;
		}
		
		protected override void OnTick()
		{
			StamDecay();
		}
		// Check the NetState and call the decaying function
		public static void StamDecay()
		{
			foreach ( NetState state in NetState.Instances )
			{
				StamDecaying( state.Mobile );
			}
		}
		
		// Check thirst level if below the value set take away 1 point of stam
		public static void StamDecaying( Mobile m )
		{
			if ( m != null && m.Thirst < 5 && m.Stam > 3 )
			{
				switch (m.Thirst)
				{
					case 4: m.Stam -= 1; break;
					case 3: m.Stam -= 1; break;
					case 2: m.Stam -= 2; break;
					case 1: m.Stam -= 2; break;
					case 0:
					{
						m.Stam -= 3;
						m.SendMessage( "Thou art exhausted from thirst!" );
						break;
					}
				}
			}
		}
	}
	public class MyHunger
	{
		public static void Initialize()
		{
			CommandSystem.Register("mhgr", AccessLevel.Player, new CommandEventHandler( MyHunger_OnCommand ));
			CommandSystem.Register("myhunger", AccessLevel.Player, new CommandEventHandler( MyHunger_OnCommand ));
		}
		public static void MyHunger_OnCommand( CommandEventArgs e )
		{
			int h = e.Mobile.Hunger; // Variable to hold the hunger value of the player
			// these values are taken from Food.cs and relate directly to the message
			// you get when you eat.
			if (h <= 0 )
				e.Mobile.SendMessage( "Thou art starving to death." );
			else if ( h <= 5 )
			       	e.Mobile.SendMessage( "Thou art extremely hungry." );
			else if ( h <= 10 )
				e.Mobile.SendMessage( "Thou art very hungry." );
			else if ( h <= 15 )
				e.Mobile.SendMessage( "Thou art slightly hungry." );
			else if ( h <= 19 )
				e.Mobile.SendMessage( "Thou art not really hungry." );
			else if ( h > 19 )
				e.Mobile.SendMessage( "Thou art quite full." );
			else
				e.Mobile.SendMessage( "Error: Please report this error: hunger not found." );

			int t = e.Mobile.Thirst; // Variable to hold the thirst value of the player
			// read the comments above to see where these values came from
			if ( t <= 0 )
				e.Mobile.SendMessage( "Thou art exhausted from thirst." );
			else if ( t <= 5 )
			       	e.Mobile.SendMessage( "Thou art extremely thirsty." );
			else if ( t <= 10 )
				e.Mobile.SendMessage( "Thou art very thirsty." );
			else if ( t <= 15 )
				e.Mobile.SendMessage( "Thou art slightly thirsty." );
			else if ( t <= 19 )
				e.Mobile.SendMessage( "Thou art not really thirsty." );
			else if ( t > 19 )
				e.Mobile.SendMessage( "Thou art not thirsty." );
			else
				e.Mobile.SendMessage( "Error: Please report this error: thirst not found." );
		}
	}
}

