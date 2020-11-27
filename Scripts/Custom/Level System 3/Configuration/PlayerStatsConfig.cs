/*	System Configuration, changes to the system should be 
	made here instead of changes to the scripts
*/
using System;
using Server;
using Server.Items;
using Server.Guilds;
using Server.Mobiles;
using Server.Network;
using Server.Accounting;
using System.Collections;

namespace Server
{
    public class PlayerStatsConfig
    {		
		/* PlayerStatsConfig psc = new PlayerStatsConfig(); */	
		/*
		The changes made to here will apply to any player that
		is using the level system.  These are overrides for the
		system character creation steps.  After character creation
		completes it's work, then the player level attachment gets
		put on.  Once that is added, the below options can be applied
		if they are turned on . These ONLY affect players with the attachment
		and only affect players as they are attached.  If a player already
		has the attachment, and a change is made below, that change WILL
		NOT pass to the player.  Please be sure you set this before you
		start allowing people to use the level system, if you of course
		wanted to use it. Re attaching the attachment to a player will work
		but they will lose any gained level progress.  If you rather use the eventsinks
		then please use the built in process instead of this file. :)
		*/
		/* EVERYTHING FALSE BY DEFAULT! */
		
		#region Forced Stats after receiving level attachment
		public bool ForceStartingStats						= false;
		public int	ForceStartingStatsSTR					= 50;
		public int	ForceStartingStatsDEX					= 50;
		public int	ForceStartingStatsINT					= 50;
		#endregion
		
		#region PlayerCaps
		public bool AutoActivate_StartingStrCap				= false;	/* Strength */
		public int	StartingStrCapVar						= 125;		/* Regular Cap 125 */
		public int	StartingStrCapMaxVar					= 150;		/* Cap Possible with Scrolls 150 */
		public bool AutoActivate_StartingDexCap				= false;	/* Dexterity */
		public int	StartingDexCapVar						= 125;		/* Regular Cap 125 */
		public int	StartingDexCapMaxVar					= 150;		/* Cap Possible with Scrolls 150 */
		public bool AutoActivate_StartingIntCap				= false;	/* Intelligence */
		public int	StartingIntCapVar						= 125;		/* Regular Cap 125 */
		public int	StartingIntCapMaxVar					= 150;		/* Cap Possible with Scrolls 150 */
		public bool AutoActivate_StartingTotalStatCap		= false;	/* Total Stat Cap */
		public int	AutoActivate_StartingTotalStatCapVar	= 225;		/* Regular Total Stat Cap 225*/
		
		
		
		#endregion
		
		#region PlayerMobile Features
		public bool AutoActivate_GemMining					= false;
		public bool AutoActivate_BasketWeaving				= false;
		public bool AutoActivate_CanBuyCarpets				= false;
		public bool AutoActivate_AcceptGuildInvites			= false;
		public bool AutoActivate_Glassblowing				= false;
		public bool AutoActivate_LibraryFriend				= false;
		public bool AutoActivate_Masonry					= false;
		public bool AutoActivate_SandMining					= false;
		public bool AutoActivate_StoneMining				= false;
		public bool AutoActivate_Spellweaving				= false;
		public bool AutoActivate_MechanicalLife				= false;
		public bool AutoActivate_DisabledPvpWarning			= false;
		public bool AutoActivate_isYoung					= false;
		public bool AutoActivate_CantWalk					= false; /* Assuming you want the new player stuck in place */
			
		public bool	AutoActivate_MaxFollowSlots				= false;
		public int	AutoActivate_MaxFollowSlotsTotal		= 5;

		public bool	AutoActivate_SkillsCap					= false;
		public int	AutoActivate_SkillsCapVar				= 700;	
		#endregion

		#region Add your entries of items you want to drop in pack
		public bool AddToBackpackOnAttach					= true;
		public static void CustomBackPackDrops(Mobile m)
		{
			/* Format;  
			m.AddToBackPack(new Item(ValueifAny));
			*/
			m.AddToBackpack(new LevelSheet());
			//m.AddToBackpack(new Gold(9000));
			//m.AddToBackpack(new Candle());
			//m.AddToBackpack(new Dagger());
		}
		#endregion
		
		#region Force new Player Into Guild
		public bool ForceNewPlayerIntoGuild					= false;
		public static void ForceIntoGuild (Mobile m)
		{
			/* Make sure guild exist, or this wont work */
			Guild g = BaseGuild.FindByName("NameOfGuild") as Guild;
			if(g != null)
			{
				g.AddMember(m);
			}
		}
		#endregion
		
		#region NewStartingLocation
		public bool NewStartingLocation						= false;
		public static void StartingLocation (Mobile m)
		{
			/* Adjust Map name and coordinates, this is a SINGLE location for all.  */
			m.Map = Map.Trammel; 
			m.Location = new Point3D(1475, 1621, 20);
		}
		#endregion
	}
}
