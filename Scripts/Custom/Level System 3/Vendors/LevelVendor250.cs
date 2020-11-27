using System;
using Server.Mobiles;
using Server.Items;
using System.Collections.Generic;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    public class LevelVendor250 : BaseVendor
    {

        private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();
        [Constructable]
        public LevelVendor250()
            : base("Level 250 Bonus Vender")
        {
            this.SetSkill(SkillName.ArmsLore, 36.0, 68.0);
            this.SetSkill(SkillName.Blacksmith, 65.0, 88.0);
            this.SetSkill(SkillName.Fencing, 60.0, 83.0);
            this.SetSkill(SkillName.Macing, 61.0, 93.0);
            this.SetSkill(SkillName.Swords, 60.0, 83.0);
            this.SetSkill(SkillName.Tactics, 60.0, 83.0);
            this.SetSkill(SkillName.Parry, 61.0, 93.0);
        }

        public override VendorShoeType ShoeType
        {
            get
            {
                return VendorShoeType.Boots;
            }
        }
		
        public override void InitBody()
        {
            this.InitStats(120, 100, 75);
            this.Female = true;
            this.CantWalk = true;
            this.Body = 606;
            this.HairItemID = this.Race.RandomHair(this.Female);
            this.HairHue = this.Race.RandomHairHue();
			this.PackGold(10000, 20000);
        }

        public override void InitOutfit()
        {
            Item item = (Utility.RandomBool() ? null : new Server.Items.LeafChest());

            if (item != null && !EquipItem(item))
            {
                item.Delete();
                item = null;
            }

            if (item == null)
                AddItem(new Server.Items.HoodedShroudOfShadows());

            base.InitOutfit();
        }
        
        public LevelVendor250(Serial serial)
            : base(serial)
        {
        }
		
		public override void VendorBuy(Mobile from)
		{
			XMLPlayerLevelAtt xmlplayer = (XMLPlayerLevelAtt)XmlAttach.FindAttachment(from, typeof(XMLPlayerLevelAtt));
			if (xmlplayer != null)
			{
				if (this is LevelVendor250)
				{
					if (xmlplayer.Levell < 250)
					{
						Say(true, "You must be level 250 or higher to work with me! Back to Training for you!");
						return;
					}
				}	
			}
			else 
			{
				Say(true, "You lack a level, are you normal?");
				return;
			}
			base.VendorBuy(from);
		}

        protected override List<SBInfo> SBInfos
        {
            get
            {
                return this.m_SBInfos;
            }
        }
        public override void InitSBInfo()
        {
            this.m_SBInfos.Add(new SBLevelVendor250());
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
    public class SBLevelVendor250 : SBInfo 
    { 
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();
        public SBLevelVendor250() 
        { 
        }

        public override IShopSellInfo SellInfo
        {
            get
            {
                return this.m_SellInfo;
            }
        }
        public override List<GenericBuyInfo> BuyInfo
        {
            get
            {
                return this.m_BuyInfo;
            }
        }

        public class InternalBuyInfo : List<GenericBuyInfo> 
        { 
            public InternalBuyInfo() 
            {
                Add(new BeverageBuyInfo(typeof(BeverageBottle), BeverageType.Ale, 7, 20, 0x99F, 0));
                Add(new BeverageBuyInfo(typeof(BeverageBottle), BeverageType.Wine, 7, 20, 0x9C7, 0));
                Add(new BeverageBuyInfo(typeof(BeverageBottle), BeverageType.Liquor, 7, 20, 0x99B, 0));
                Add(new GenericBuyInfo(typeof(Backpack), 15, 20, 0x9B2, 0));
                Add(new GenericBuyInfo("1016450", typeof(Chessboard), 2, 20, 0xFA6, 0));
                Add(new GenericBuyInfo("1016449", typeof(CheckerBoard), 2, 20, 0xFA6, 0));
                Add(new GenericBuyInfo(typeof(Backgammon), 2, 20, 0xE1C, 0));
                Add(new GenericBuyInfo(typeof(Dices), 2, 20, 0xFA7, 0));
            }
        }
        public class InternalSellInfo : GenericSellInfo 
        { 
            public InternalSellInfo() 
            {
                this.Add(typeof(Backpack), 3);
            }
        }
    }
}