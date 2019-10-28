//===============================================================================
//                      This script was created by Gizmo's UoDevPro
//                      This script was created on 9/18/2019 1:04:58 PM
//===============================================================================


using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
	public class CurseintheCrypts : BaseQuest
	{
		public CurseintheCrypts() : base()
		{
			//The player must slay 1 Lich
			this.AddObjective(new SlayObjective(typeof(Lich), "Lich", 1));
			//Reward the Player Gold
			this.AddReward(new BaseReward("2500-5000 Gold"));
			//Quest Has Chance at Special Item
			this.AddReward(new BaseReward("Serpent Jawbone"));

		}

        public override QuestChain ChainID { get { return QuestChain.MonitorCrypts; } }

        public override bool CanOffer()
        {
            List<QuestRestartInfo> completes = Owner.DoneQuests;

            foreach(QuestRestartInfo info in completes)
            {
                if(info.QuestType == typeof(TheCryptsArise))
                {
                    return true;
                }
            }

            return false;
        }


        //Player can only do quest once
        public override bool DoneOnce { get { return true; } }

        //Quest Title
        public override object Title { get { return "Curse in the Crypts"; } }
		//Quest Description
		public override object Description { get { return "I have been thinking about the matter and Knight Caladin reminded me of a time past when the Crypts were similarly infested. A group of bold Monitorians dove into the far reaches of the Crypts and discovered a Lich had taken up residence. I wouldst ask thee to check this possibility, and if the Lich has come back, destroy it. I would be willing to part with a valuable Ophidian artifact for thy trouble."; } }
		//Quest Refuse Message
		public override object Refuse { get { return "Tis a dangerous request - I expect only the boldest will undertake it."; } }
		//Quest Uncompleted Message
		public override object Uncomplete { get { return "I will be thinking of thy safety, and hoping thou dost not return for cremation."; } }
		//Quest Completed Message
		public override object Complete { get { return "Praise Courage! Thou hast vanquished a powerful foe, and done a great service for the people of Monitor. As promised, here is the Ophidian artifact. I know not the purpose of this Serpent Jawbone, but it should, if nothing else, be worth some gold."; } }

		public override void GiveRewards()
		{
			//Give Gold to player in form of a bank check
			BankCheck gold = new BankCheck(Utility.RandomMinMax(2500, 5000));
			if(!Owner.AddToBackpack( gold ))
				gold.MoveToWorld(Owner.Location,Owner.Map);


			Item item = new SerpentJawbone(  );
			if(!Owner.AddToBackpack( item ) )
			{
				item.MoveToWorld(Owner.Location,Owner.Map);
			}



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
