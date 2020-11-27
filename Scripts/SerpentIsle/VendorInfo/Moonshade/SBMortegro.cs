using System;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
	public class SBMortegro : SBInfo
	{
		private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();
		public SBMortegro()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : List<GenericBuyInfo>
		{
			public InternalBuyInfo()
			{
                Random rnd = new Random();

                //Add(new GenericBuyInfo(typeof(Spellbook), 18, 10, 0xEFA, 0));

                if (Core.AOS)
                    Add(new GenericBuyInfo(typeof(NecromancerSpellbook), 115, 10, 0x2253, 0));

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

                if (Core.AOS)
                {
                    Add(new GenericBuyInfo(typeof(BatWing), 3, rnd.Next(30, 120), 0xF78, 0));
                    Add(new GenericBuyInfo(typeof(DaemonBlood), 6, rnd.Next(30, 120), 0xF7D, 0));
                    Add(new GenericBuyInfo(typeof(PigIron), 5, rnd.Next(30, 120), 0xF8A, 0));
                    Add(new GenericBuyInfo(typeof(NoxCrystal), 6, rnd.Next(30, 120), 0xF8E, 0));
                    Add(new GenericBuyInfo(typeof(GraveDust), 3, rnd.Next(30, 120), 0xF8F, 0));
                }

                Type[] types = Loot.NecromancyScrollTypes;

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
                Retorno4 = RandomScroll(1, rnd.Next(3, 5)); // RandomScroll(Circle, Amount) => Sorts Amount units of circle Circle.

                Retorno4.AddRange(RandomScroll(2, rnd.Next(3, 5)));
                Retorno4.AddRange(RandomScroll(3, rnd.Next(3, 5)));
                Retorno4.AddRange(RandomScroll(4, rnd.Next(1, 2)));



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
                Type[] types = Loot.NecromancyScrollTypes;

                Random rnd = new Random();

                int a, b, c;
                a = b = c = 0;

                // First Circle Scrolls
                NecroScrolls.Add(new KeyValuePair<string, int>("AnimateDeadScroll", 0x2260));
                NecroScrolls.Add(new KeyValuePair<string, int>("BloodOathScroll", 0x2261));
                NecroScrolls.Add(new KeyValuePair<string, int>("CorpseSkinScroll", 0x2262));
                NecroScrolls.Add(new KeyValuePair<string, int>("CurseWeaponScroll", 0x2263));
                NecroScrolls.Add(new KeyValuePair<string, int>("EvilOmenScroll", 0x2264));
                NecroScrolls.Add(new KeyValuePair<string, int>("HorrificBeastScroll", 0x2265));
                NecroScrolls.Add(new KeyValuePair<string, int>("LichTransformation", 0x2266));
                NecroScrolls.Add(new KeyValuePair<string, int>("MindRotScroll", 0x2267));
                NecroScrolls.Add(new KeyValuePair<string, int>("PainSpikeScroll", 0x2268));
                NecroScrolls.Add(new KeyValuePair<string, int>("PoisonStrikeScroll", 0x2269));
                NecroScrolls.Add(new KeyValuePair<string, int>("Strangulation", 0x226A));
                NecroScrolls.Add(new KeyValuePair<string, int>("SummonFamiliarScroll", 0x226B));
                NecroScrolls.Add(new KeyValuePair<string, int>("VampiricEmbraceScroll", 0x226C));
                NecroScrolls.Add(new KeyValuePair<string, int>("VengefulSpiritScroll", 0x226D));
                NecroScrolls.Add(new KeyValuePair<string, int>("WitherScroll", 0x226E));
                NecroScrolls.Add(new KeyValuePair<string, int>("WraithFormScroll", 0x226F));
                NecroScrolls.Add(new KeyValuePair<string, int>("AnimateDeadScroll", 0x2260));

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
