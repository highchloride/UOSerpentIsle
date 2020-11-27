using System;
using System.Collections;
using Server;

namespace Server
{
    public class ConfiguredSkills
	{
		
		public int SkillsTotal		= 1200000;		/* If Player is equal to or exceeds they will not
													gain skill points on level up */
		public int MaxStatPoints	= 15000;			/* If player is equal to or exceeds they will not
													gain stat points on level up */
		
		/* How many skill points awarded per level.
			Scenario: If turning level 18, below20 applies*/
		
		public int Below20	= 4;		/*	Below Level 20	*/
		public int Below40	= 4;		/*	Below Level 40	*/
		public int Below60	= 4;		/*	Below Level 60	*/
		public int Below70	= 4;		/*	Below Level 70	*/
		public int Below80	= 4;		/*	Below Level 80	*/
		public int Below90	= 4;		/*	Below Level 90	*/
		public int Below100	= 4;		/*	Below Level 100	*/
		public int Below110	= 4;		/*	Below Level 110	*/
		public int Below120	= 4;		/*	Below Level 120	*/
		public int Below130	= 4;		/*	Below Level 130	*/
		public int Below140	= 4;		/*	Below Level 140	*/
		public int Below150	= 4;		/*	Below Level 150	*/
		public int Below160	= 4;		/*	Below Level 160	*/
		public int Below170	= 4;		/*	Below Level 170	*/
		public int Below180	= 4;		/*	Below Level 180	*/
		public int Below190	= 4;		/*	Below Level 190	*/
		public int Below200	= 4;		/*	Below Level 200	*/

		/* How many stat points to be awarded per level */
		public int Below20Stat	= 3;		/*	Below Level 20	*/
		public int Below40Stat	= 3;		/*	Below Level 40	*/
		public int Below60Stat	= 3;		/*	Below Level 60	*/
		public int Below70Stat	= 3;		/*	Below Level 70	*/
		public int Below80Stat	= 3;		/*	Below Level 80	*/
		public int Below90Stat	= 3;		/*	Below Level 90	*/
		public int Below100Stat	= 3;		/*	Below Level 100	*/
		public int Below110Stat	= 3;		/*	Below Level 110	*/
		public int Below120Stat	= 3;		/*	Below Level 120	*/
		public int Below130Stat	= 3;		/*	Below Level 130	*/
		public int Below140Stat	= 3;		/*	Below Level 140	*/
		public int Below150Stat	= 3;		/*	Below Level 150	*/
		public int Below160Stat	= 3;		/*	Below Level 160	*/
		public int Below170Stat	= 3;		/*	Below Level 170	*/
		public int Below180Stat	= 3;		/*	Below Level 180	*/
		public int Below190Stat	= 3;		/*	Below Level 190	*/
		public int Below200Stat	= 3;		/*	Below Level 200	*/
		
		/* How Many Follower points to award per level? can even be zero */
		public bool GainFollowerSlotOnLevel = false;
		/* false by default, grant a follower per 20-10 levels */
		/* Warning. Without proper planning this could be game breaking or 
		over kill*/
		public int GainFollowerSlotOnLevel20	= 1;		/*	At level 20		*/
		public bool GainOn20					= true;		/* Gain On Level?	*/
		public int GainFollowerSlotOnLevel30	= 1;		/*	At level 30		*/
		public bool GainOn30					= true;		/* Gain On Level?	*/
		public int GainFollowerSlotOnLevel40	= 1;		/*	At level 40		*/
		public bool GainOn40					= true;		/* Gain On Level?	*/
		public int GainFollowerSlotOnLevel50	= 1;		/*	At level 50		*/
		public bool GainOn50					= true;		/* Gain On Level?	*/
		public int GainFollowerSlotOnLevel60	= 1;		/*	At level 60		*/
		public bool GainOn60					= true;		/* Gain On Level?	*/
		public int GainFollowerSlotOnLevel70	= 1;		/*	At level 70		*/
		public bool GainOn70					= true;		/* Gain On Level?	*/
		public int GainFollowerSlotOnLevel80	= 1;		/*	At level 80		*/
		public bool GainOn80					= true;		/* Gain On Level?	*/
		public int GainFollowerSlotOnLevel90	= 1;		/*	At level 90		*/
		public bool GainOn90					= true;		/* Gain On Level?	*/
		public int GainFollowerSlotOnLevel100	= 1;		/*	At level 100	*/
		public bool GainOn100					= true;		/* Gain On Level?	*/

		public int GainFollowerSlotOnLevel110	= 1;		/*	At level 110	*/
		public bool GainOn110					= true;		/* Gain On Level?	*/
		public int GainFollowerSlotOnLevel120	= 1;		/*	At level 120	*/
		public bool GainOn120					= true;		/* Gain On Level?	*/
		public int GainFollowerSlotOnLevel130	= 1;		/*	At level 130	*/
		public bool GainOn130					= true;		/* Gain On Level?	*/
		public int GainFollowerSlotOnLevel140	= 1;		/*	At level 140	*/
		public bool GainOn140					= true;		/* Gain On Level?	*/
		public int GainFollowerSlotOnLevel150	= 1;		/*	At level 150	*/
		public bool GainOn150					= true;		/* Gain On Level?	*/
		public int GainFollowerSlotOnLevel160	= 1;		/*	At level 160	*/
		public bool GainOn160					= true;		/* Gain On Level?	*/
		public int GainFollowerSlotOnLevel170	= 1;		/*	At level 170	*/
		public bool GainOn170					= true;		/* Gain On Level?	*/
		public int GainFollowerSlotOnLevel180	= 1;		/*	At level 180	*/
		public bool GainOn180					= true;		/* Gain On Level?	*/
		public int GainFollowerSlotOnLevel190	= 1;		/*	At level 190	*/
		public bool GainOn190					= true;		/* Gain On Level?	*/
		public int GainFollowerSlotOnLevel200	= 1;		/*	At level 200	*/
		public bool GainOn200					= true;		/* Gain On Level?	*/
	}
 
}