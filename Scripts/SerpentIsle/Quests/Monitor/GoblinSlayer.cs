//===============================================================================
//                      This script was created by Gizmo's UoDevPro
//                      This script was created on 1/11/2019 5:56:07 PM
//===============================================================================


using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
	public class GoblinSlayer : BaseQuest
	{
		public GoblinSlayer() : base()
		{
			//The player must slay 8 Green Goblin
			this.AddObjective(new SlayObjective(typeof(GreenGoblin), "Green Goblin", 8));
			//The player must slay 2 Green Goblin Scout
			this.AddObjective(new SlayObjective(typeof(GreenGoblinScout), "Green Goblin Scout", 2));
			//Reward the Player Gold
			this.AddReward(new BaseReward("500-1000 Gold"));
		}

		//The player will have a delay before they can redo quest again
		public override TimeSpan RestartDelay { get { return TimeSpan.FromMinutes(15); } }

		//Quest Title
		public override object Title { get { return "Goblin Slayer"; } }
		//Quest Description
		public override object Description { get { return "Monitor is constantly under assault by the goblin hordes. We would be grateful if you would lend your skills to the cause of defending Monitor."; } }
		//Quest Refuse Message
		public override object Refuse { get { return "Perhaps I misjudged you...thinking you bold enough to take on the goblin horde..."; } }
		//Quest Uncompleted Message
		public override object Uncomplete { get { return "The goblins continue to assault us! How goes your progress?"; } }
		//Quest Completed Message
		public override object Complete { get { return "The City of Monitor is in your debt."; } }

		public override void GiveRewards()
		{
			//Give Gold to player in form of a bank check
			BankCheck gold = new BankCheck(Utility.RandomMinMax(500, 1000));
			if(!Owner.AddToBackpack( gold ))
				gold.MoveToWorld(Owner.Location,Owner.Map);

			base.GiveRewards();
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0); // version
		}
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}
