//===============================================================================
//                      This script was created by Gizmo's UoDevPro
//                      This script was created on 9/10/2019 3:24:01 PM
//===============================================================================


using System;
using Server;
using Server.Engines.CityLoyalty;
using Server.Items;
using Server.Mobiles;
using Server.SerpentIsle.Items.Corpses;

namespace Server.Engines.Quests
{
	public class TestofKnighthood : BaseQuest
	{
		public TestofKnighthood() : base()
		{
			//The player must collect 1 of any of the three totem corpses.
			this.AddObjective(new ObtainObjective(typeof(TotemAnimalCarcass), "Totem Animal Carcass", 1));

            //Reward the Player Gold
            this.AddReward(new BaseReward("500-1000 Gold"));
        }

        //This logic determines worthiness via city loyalty.
        public override bool CanOffer()
        {
            CityLoyaltySystem system = CityLoyaltySystem.Monitor; //UOSI Make this Monitor

            if(system != null)
            {
                if (system.GetLoyaltyRating(Owner) >= LoyaltyRating.Commended)
                    return true;
            }            

            return false;
        }

        public override void GiveRewards()
        {
            //Give Gold to player in form of a bank check
            BankCheck gold = new BankCheck(Utility.RandomMinMax(500, 1000));
            if (!Owner.AddToBackpack(gold))
                gold.MoveToWorld(Owner.Location, Owner.Map);

            base.GiveRewards();
        }

        //The player will have a delay before they can redo quest again
        public override TimeSpan RestartDelay { get { return TimeSpan.FromMinutes(1); } }

        //Player can only do quest once
		public override bool DoneOnce{ get{ return false; } }

        //We only require one of the carcasses
        public override bool AllObjectives { get { return false; } }

        //Quest Title
        public override object Title { get { return "Test of Knighthood"; } }

        //Quest Description
		public override object Description { get { return "Well met, Knight! Now that you have proven thy worth, it is time for thee to take the Knight's Test. All Monitorians have braved the test and earned their place among their command. You may end up with the Wolves, the Bears, or the Leopards. The Test will tell. Go and see Shmed at the entrance of the test, and tell him 'Courage is the Soul of Life.' Walk in Courage."; } }

        //Quest Refuse Message
		public override object Refuse { get { return "Perhaps we have misjudged thee...know that you will not be permitted to progress in this city until thou hast conquered the Test of Knighthood!"; } }

        //Quest Uncompleted Message
		public override object Uncomplete { get { return "I await word that thou hast completed the Test of Knighthood!"; } }

        //Quest Completed Message
		public override object Complete { get { return "It is with great honor I welcome thee to our ranks, Knight! Thou shoudst now prepare for thy Knight's Banquet!"; } }

        

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
