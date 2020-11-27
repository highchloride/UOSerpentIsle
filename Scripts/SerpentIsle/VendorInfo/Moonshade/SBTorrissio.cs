using System;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
	public class SBTorrissio : SBInfo
	{
		private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();
		public SBTorrissio()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : List<GenericBuyInfo>
		{
			public InternalBuyInfo()
			{
                Random rnd = new Random();

                Add(new GenericBuyInfo(typeof(MysticBook), 100, 10, 0x2D9D, 0));

                //if (Core.AOS)
                    //Add(new GenericBuyInfo(typeof(NecromancerSpellbook), 115, 10, 0x2253, 0));



                //Add(new GenericBuyInfo(typeof(ScribesPen), 8, 10, 0xFBF, 0));

                //Add(new GenericBuyInfo(typeof(BlankScroll), 5, 20, 0x0E34, 0));

                //Add(new GenericBuyInfo("1041072", typeof(MagicWizardsHat), 11, 10, 0x1718, Utility.RandomDyedHue()));

                //Add(new GenericBuyInfo(typeof(RecallRune), 15, 10, 0x1F14, 0));

                //UOSI - removed some of the potions. Go see an alchemist!
                //Add(new GenericBuyInfo(typeof(RefreshPotion), 15, 10, 0xF0B, 0, true));
                //Add(new GenericBuyInfo(typeof(AgilityPotion), 15, 10, 0xF08, 0, true));
                //Add(new GenericBuyInfo(typeof(NightSightPotion), 15, 10, 0xF06, 0, true));
                //Add(new GenericBuyInfo(typeof(LesserHealPotion), 15, 10, 0xF0C, 0, true));
                //Add(new GenericBuyInfo(typeof(StrengthPotion), 15, 10, 0xF09, 0, true));
                //Add(new GenericBuyInfo(typeof(LesserPoisonPotion), 15, 10, 0xF0A, 0, true));
                //Add(new GenericBuyInfo(typeof(LesserCurePotion), 15, 10, 0xF07, 0, true));
                //Add(new GenericBuyInfo(typeof(LesserExplosionPotion), 21, 10, 0xF0D, 0, true));

                //Add(new GenericBuyInfo(typeof(BlackPearl), 5, rnd.Next(30, 120), 0xF7A, 0));
                //Add(new GenericBuyInfo(typeof(Bloodmoss), 5, rnd.Next(30, 120), 0xF7B, 0));
                //Add(new GenericBuyInfo(typeof(Garlic), 3, rnd.Next(50, 150), 0xF84, 0));
                //Add(new GenericBuyInfo(typeof(Ginseng), 3, rnd.Next(50, 150), 0xF85, 0));
                //Add(new GenericBuyInfo(typeof(MandrakeRoot), 3, rnd.Next(30, 120), 0xF86, 0));
                //Add(new GenericBuyInfo(typeof(Nightshade), 3, rnd.Next(30, 120), 0xF88, 0));
                //Add(new GenericBuyInfo(typeof(SpidersSilk), 3, rnd.Next(30, 120), 0xF8D, 0));
                //Add(new GenericBuyInfo(typeof(SulfurousAsh), 3, rnd.Next(30, 120), 0xF8C, 0));

                if (Core.SA)
                {
                    Add(new GenericBuyInfo(typeof(Bone), 3, rnd.Next(30, 120), 0x0F7E, 0));
                    Add(new GenericBuyInfo(typeof(FertileDirt), 6, rnd.Next(30, 120), 0x0F81, 0));
                }

                Type[] types = Loot.MysticismScrollTypes;

                //UOSI Took this out so all scrolls are randomized.
                //int circles = 3;

                //for (int i = 0; i < circles * 8 && i < types.Length; ++i)
                //{
                //    int itemID = 0x1F2E + i;

                //    if (i == 6)
                //        itemID = 0x1F2D;
                //    else if (i > 6)
                //        --itemID;

                //    Add(new GenericBuyInfo(types[i], 12 + ((i / 8) * 10), 20, itemID, 0, true));
                //}

                /*
                 *  Magery Random Scrolls
                 *  by joew
                */
                List<KeyValuePair<int, int>> Retorno4 = new List<KeyValuePair<int, int>>(); // Array contains scrolls
                Retorno4 = RandomScroll(1, rnd.Next(2, 4)); // RandomScroll(Circle, Amount) => Sorts Amount units of circle Circle.

                Retorno4.AddRange(RandomScroll(2, rnd.Next(2, 4)));
                Retorno4.AddRange(RandomScroll(3, rnd.Next(2, 4)));
                Retorno4.AddRange(RandomScroll(4, rnd.Next(2, 4)));


                foreach (KeyValuePair<int, int> x in Retorno4)
                {
                    Add(new GenericBuyInfo(types[x.Key], 12 + (x.Value / (8 * 4)), 4, x.Value, 0, true)); //UOSI Changed so there's 10 of each scroll sold, from rnd.Next(1,10)
                }
            }

            public List<KeyValuePair<int, int>> RandomScroll(int circle, int qntd)
            {
                List<KeyValuePair<string, int>> NecroScrolls = new List<KeyValuePair<string, int>>(); // Scrolls List
                List<KeyValuePair<int, int>> Retorno = new List<KeyValuePair<int, int>>(); // Array that will be returned with sorted scrolls.

                List<int> sorteados = new List<int>();
                Type[] types = Loot.MysticismScrollTypes;

                Random rnd = new Random();

                int a, b, c;
                a = b = c = 0;

                // First Circle Scrolls
                NecroScrolls.Add(new KeyValuePair<string, int>("HealingStoneScroll", 0x2D9F));
                NecroScrolls.Add(new KeyValuePair<string, int>("NetherBoltScroll", 0x2D9E));

                NecroScrolls.Add(new KeyValuePair<string, int>("PurgeMagicScroll", 0x2DA0));
                NecroScrolls.Add(new KeyValuePair<string, int>("EnchantScroll", 0x2DA1));

                NecroScrolls.Add(new KeyValuePair<string, int>("SleepScroll", 0x2DA2));
                NecroScrolls.Add(new KeyValuePair<string, int>("EagleStrikeScroll", 0x2DA3));

                NecroScrolls.Add(new KeyValuePair<string, int>("AnimatedWeaponScroll", 0x2DA4));
                NecroScrolls.Add(new KeyValuePair<string, int>("StoneFormScroll", 0x2DA5));

                NecroScrolls.Add(new KeyValuePair<string, int>("SpellTriggerScroll", 0x2DA6));
                NecroScrolls.Add(new KeyValuePair<string, int>("MassSleepScroll", 0x2DA7));

                NecroScrolls.Add(new KeyValuePair<string, int>("CleansingWindsScroll", 0x2DA8));
                NecroScrolls.Add(new KeyValuePair<string, int>("BombardScroll", 0x2DA9));

                NecroScrolls.Add(new KeyValuePair<string, int>("SpellPlagueScroll", 0x2DAA));
                NecroScrolls.Add(new KeyValuePair<string, int>("HailStormScroll", 0x2DAB));

                NecroScrolls.Add(new KeyValuePair<string, int>("NetherCycloneScroll", 0x2DAC));
                NecroScrolls.Add(new KeyValuePair<string, int>("RisingColossusScroll", 0x2DAD));

                NecroScrolls.Sort((x, y) => x.Value.CompareTo(y.Value)); // Order by ItemID


                for (int i = 0; i < qntd; i++)
                {
                    // All Circles
                    if (circle == 0)
                    {
                        a = 0;
                        b = 16;
                    }

                    // Specific Circle
                    else if (circle > 0 && circle <= 4)
                    {
                        a = (circle - 1) * 4;
                        b = a + 4;
                    }

                    c = rnd.Next(a, b); // Get ItemID

                    while (sorteados.Contains(c))
                    {
                        c = rnd.Next(a, b);
                    }
                    sorteados.Add(c);

                    Retorno.Add(new KeyValuePair<int, int>(c, NecroScrolls[c].Value));
                }

                return Retorno;
            }
        }

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add( typeof( WizardsHat ), 15 );
				Add( typeof( Runebook ), 1250 );
				Add( typeof( BlackPearl ), 3 ); 
				Add( typeof( Bloodmoss ),4 ); 
				Add( typeof( MandrakeRoot ), 2 ); 
				Add( typeof( Garlic ), 2 ); 
				Add( typeof( Ginseng ), 2 ); 
				Add( typeof( Nightshade ), 2 ); 
				Add( typeof( SpidersSilk ), 2 ); 
				Add( typeof( SulfurousAsh ), 2 );
				Add( typeof( RecallRune ), 13 );
				Add( typeof( Spellbook ), 25 );
				
				if ( Core.AOS )
				{
				Add( typeof( PigIron ), 2 );
				Add( typeof( DaemonBlood ), 3 );
				Add( typeof( NoxCrystal ), 3 );
				Add( typeof( BatWing ), 1 );
				Add( typeof( GraveDust ), 1 );
				}

				Type[] types = Loot.RegularScrollTypes;

				for (int i = 0; i < types.Length; ++i)
                    Add(types[i], ((i / 8) + 2) * 2);
			}
		}
	}
}
