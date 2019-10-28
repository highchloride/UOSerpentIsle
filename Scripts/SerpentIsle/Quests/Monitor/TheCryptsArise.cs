//===============================================================================
//                      This script was created by Gizmo's UoDevPro
//                      This script was created on 9/18/2019 1:10:02 PM
//===============================================================================


using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class TheCryptsArise : BaseQuest
    {
        public TheCryptsArise() : base()
        {
            //The player must slay 10 Skeleton
            this.AddObjective(new SlayObjective(typeof(Skeleton), "Skeleton", 10));
            //The player must slay 10 Mongbat
            this.AddObjective(new SlayObjective(typeof(Mongbat), "Mongbat", 10));
            //Reward the Player Gold
            this.AddReward(new BaseReward("2000-5000 Gold"));
            //Reward the Player Magic Item(s)
            this.AddReward(new BaseReward("1 Magic Item"));
        }

        public override QuestChain ChainID { get { return QuestChain.MonitorCrypts; } }
        public override Type NextQuest { get { return typeof(CurseintheCrypts); } }

        //Player can only do quest once
        public override bool DoneOnce { get { return true; } }

        //Quest Title
        public override object Title { get { return "The Crypts Arise"; } }
		//Quest Description
		public override object Description { get { return "Well-met, traveller! As caretaker of this Crematorium, certain tasks fall to me - including the upkeep of the Crypts, where Monitor's honored dead are interred. Lately, the spirits within have stirred to life, threatening any who would come to pay their respects to their loved ones. Wouldst thou be willing to clear out some of the dangerous things that now roam the passages?"; } }
		//Quest Refuse Message
		public override object Refuse { get { return "I understand, traveller. Monitor's problems are not thine own."; } }
		//Quest Uncompleted Message
		public override object Uncomplete { get { return "I will be here when thou has found victory against thine foes."; } }
		//Quest Completed Message
		public override object Complete { get { return "I thank thee, traveller. The people of Monitor do, too."; } }

		public override void GiveRewards()
		{
			//Give Gold to player in form of a bank check
			BankCheck gold = new BankCheck(Utility.RandomMinMax(2000, 5000));
			if(!Owner.AddToBackpack( gold ))
				gold.MoveToWorld(Owner.Location,Owner.Map);

			Item item;

			//Random Magic Item #1
			item = Loot.RandomArmorOrShieldOrWeaponOrJewelry();
			if( item is BaseWeapon )
				BaseRunicTool.ApplyAttributesTo((BaseWeapon)item, 2, 5, 30 );
			if( item is BaseArmor )
				BaseRunicTool.ApplyAttributesTo((BaseArmor)item, 2, 5, 30 );
			if( item is BaseJewel )
				BaseRunicTool.ApplyAttributesTo((BaseJewel)item, 2, 5, 30 );
			if( item is BaseHat )
				BaseRunicTool.ApplyAttributesTo((BaseHat)item, 2, 5, 30 );
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
