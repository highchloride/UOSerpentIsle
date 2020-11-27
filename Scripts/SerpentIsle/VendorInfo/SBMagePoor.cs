using System;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
    public class SBMagePoor : SBInfo
    {
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();
        public SBMagePoor()
        {
        }

        public override IShopSellInfo SellInfo
        {
            get
            {
                return m_SellInfo;
            }
        }
        public override List<GenericBuyInfo> BuyInfo
        {
            get
            {
                return m_BuyInfo;
            }
        }

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Random rnd = new Random();

                Add(new GenericBuyInfo(typeof(Spellbook), 18, 10, 0xEFA, 0));
				
                if (Core.AOS)
                    Add(new GenericBuyInfo(typeof(NecromancerSpellbook), 115, 10, 0x2253, 0));
				
                Add(new GenericBuyInfo(typeof(ScribesPen), 8, 10, 0xFBF, 0));

                Add(new GenericBuyInfo(typeof(BlankScroll), 5, 20, 0x0E34, 0));

                Add(new GenericBuyInfo("1041072", typeof(MagicWizardsHat), 11, 10, 0x1718, Utility.RandomDyedHue()));

                //Add(new GenericBuyInfo(typeof(RecallRune), 15, 10, 0x1F14, 0));

                //UOSI - removed some of the potions. Go see an alchemist!
                Add(new GenericBuyInfo(typeof(RefreshPotion), 15, 10, 0xF0B, 0, true));
                //Add(new GenericBuyInfo(typeof(AgilityPotion), 15, 10, 0xF08, 0, true));
                //Add(new GenericBuyInfo(typeof(NightSightPotion), 15, 10, 0xF06, 0, true));
                Add(new GenericBuyInfo(typeof(LesserHealPotion), 15, 10, 0xF0C, 0, true));
                //Add(new GenericBuyInfo(typeof(StrengthPotion), 15, 10, 0xF09, 0, true));
                //Add(new GenericBuyInfo(typeof(LesserPoisonPotion), 15, 10, 0xF0A, 0, true));
                Add(new GenericBuyInfo(typeof(LesserCurePotion), 15, 10, 0xF07, 0, true));
                //Add(new GenericBuyInfo(typeof(LesserExplosionPotion), 21, 10, 0xF0D, 0, true));

                Add(new GenericBuyInfo(typeof(BlackPearl), 5, rnd.Next(30, 60), 0xF7A, 0));
                Add(new GenericBuyInfo(typeof(Bloodmoss), 5, rnd.Next(30, 60), 0xF7B, 0));
                Add(new GenericBuyInfo(typeof(Garlic), 3, rnd.Next(50, 99), 0xF84, 0));
                Add(new GenericBuyInfo(typeof(Ginseng), 3, rnd.Next(50, 99), 0xF85, 0));
                Add(new GenericBuyInfo(typeof(MandrakeRoot), 3, rnd.Next(30, 60), 0xF86, 0));
                Add(new GenericBuyInfo(typeof(Nightshade), 3, rnd.Next(30, 60), 0xF88, 0));
                Add(new GenericBuyInfo(typeof(SpidersSilk), 3, rnd.Next(30, 60), 0xF8D, 0));
                Add(new GenericBuyInfo(typeof(SulfurousAsh), 3, rnd.Next(30, 60), 0xF8C, 0));

                if (Core.AOS)
                {
                    Add(new GenericBuyInfo(typeof(BatWing), 3, rnd.Next(30, 60), 0xF78, 0));
                    Add(new GenericBuyInfo(typeof(DaemonBlood), 6, rnd.Next(30, 60), 0xF7D, 0));
                    Add(new GenericBuyInfo(typeof(PigIron), 5, rnd.Next(30, 60), 0xF8A, 0));
                    Add(new GenericBuyInfo(typeof(NoxCrystal), 6, rnd.Next(30, 60), 0xF8E, 0));
                    Add(new GenericBuyInfo(typeof(GraveDust), 3, rnd.Next(30, 60), 0xF8F, 0));
                }

                Type[] types = Loot.RegularScrollTypes;

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
                Retorno4 = RandomScroll(1, rnd.Next(4,5)); // RandomScroll(Circle, Amount) => Sorts Amount units of circle Circle.

                Retorno4.AddRange(RandomScroll(2, rnd.Next(4, 5)));
                Retorno4.AddRange(RandomScroll(3, rnd.Next(4, 5)));

                Retorno4.AddRange(RandomScroll(4, rnd.Next(2, 3)));
                Retorno4.AddRange(RandomScroll(5, rnd.Next(2, 3)));
                Retorno4.AddRange(RandomScroll(6, rnd.Next(1, 3)));
                
                if(Utility.Random(1,2) == 1)
                {
                    Retorno4.AddRange(RandomScroll(7, rnd.Next(1, 3)));
                }
                

                foreach (KeyValuePair<int, int> x in Retorno4)
                {
                    Add(new GenericBuyInfo(types[x.Key], 12 + (x.Value / (8 * 4)), 10, x.Value, 0, true)); //UOSI Changed so there's 10 of each scroll sold, from rnd.Next(1,10)
                }
            }
			
			public List<KeyValuePair<int, int>> RandomScroll(int circle, int qntd)
            {
                List<KeyValuePair<string, int>> MageryScrolls = new List<KeyValuePair<string, int>>(); // Scrolls List
                List<KeyValuePair<int, int>> Retorno = new List<KeyValuePair<int, int>>(); // Array that will be returned with sorted scrolls.

                List<int> sorteados = new List<int>();
                Type[] types = Loot.RegularScrollTypes;

                Random rnd = new Random();

                int a,b,c;
                a = b = c = 0;
                
                // First Circle Scrolls
                MageryScrolls.Add(new KeyValuePair<string, int>("ClumsyScroll", 0x1F2E));
                MageryScrolls.Add(new KeyValuePair<string, int>("CreateFoodScroll", 0x1F2F));
                MageryScrolls.Add(new KeyValuePair<string, int>("FeeblemindScroll", 0x1F30));
                MageryScrolls.Add(new KeyValuePair<string, int>("HealScroll", 0x1F31));
                MageryScrolls.Add(new KeyValuePair<string, int>("MagicArrowScroll", 0x1F32));
                MageryScrolls.Add(new KeyValuePair<string, int>("NightSightScroll", 0x1F33));
                MageryScrolls.Add(new KeyValuePair<string, int>("ReactiveArmorScroll", 0x1F2D));
                MageryScrolls.Add(new KeyValuePair<string, int>("WeakenScroll", 0x1F34));

                // Second Circle Scrolls
                MageryScrolls.Add(new KeyValuePair<string, int>("AgilityScroll", 0x1F35));
                MageryScrolls.Add(new KeyValuePair<string, int>("CunningScroll", 0x1F36));
                MageryScrolls.Add(new KeyValuePair<string, int>("CureScroll", 0x1F37));
                MageryScrolls.Add(new KeyValuePair<string, int>("HarmScroll", 0x1F38));
                MageryScrolls.Add(new KeyValuePair<string, int>("MagicTrapScroll", 0x1F39));
                MageryScrolls.Add(new KeyValuePair<string, int>("MagicUnTrapScroll", 0x1F3A));
                MageryScrolls.Add(new KeyValuePair<string, int>("ProtectionScroll", 0x1F3B));
                MageryScrolls.Add(new KeyValuePair<string, int>("StrengthScroll", 0x1F3C));

                // Third Circle Scrolls
                MageryScrolls.Add(new KeyValuePair<string, int>("BlessScroll", 0x1F3D));
                MageryScrolls.Add(new KeyValuePair<string, int>("FireballScroll", 0x1F3E));
                MageryScrolls.Add(new KeyValuePair<string, int>("MagicLockScroll", 0x1F3F));
                MageryScrolls.Add(new KeyValuePair<string, int>("PoisonScroll", 0x1F40));
                MageryScrolls.Add(new KeyValuePair<string, int>("TelekinisisScroll", 0x1F41));
                MageryScrolls.Add(new KeyValuePair<string, int>("TeleportScroll", 0x1F42));
                MageryScrolls.Add(new KeyValuePair<string, int>("UnlockScroll", 0x1F43));
                MageryScrolls.Add(new KeyValuePair<string, int>("WallOfStoneScroll", 0x1F44));

                // Fourth Circle Scrolls
                MageryScrolls.Add(new KeyValuePair<string, int>("ArchcureScroll", 0x1F45));
                MageryScrolls.Add(new KeyValuePair<string, int>("ArchProtectionScroll", 0x1F46));                
                MageryScrolls.Add(new KeyValuePair<string, int>("CurseScroll", 0x1F47));
                MageryScrolls.Add(new KeyValuePair<string, int>("FireFieldScroll", 0x1F48));
                MageryScrolls.Add(new KeyValuePair<string, int>("GreaterHealScroll", 0x1F49));
                MageryScrolls.Add(new KeyValuePair<string, int>("LightningScroll", 0x1F4A));
                MageryScrolls.Add(new KeyValuePair<string, int>("ManaDrainScroll", 0x1F4B));
                MageryScrolls.Add(new KeyValuePair<string, int>("GreaterHealScroll", 0x1F49)); // Double to make the numbers the same while removing the travel spells that we don't want.
                //MageryScrolls.Add(new KeyValuePair<string, int>("RecallScroll", 0x1F4C));

                // Fifth Circle Scrolls
                MageryScrolls.Add(new KeyValuePair<string, int>("BladeSpiritsScroll", 0x1F4D));
                MageryScrolls.Add(new KeyValuePair<string, int>("DispelFieldScroll", 0x1F4E));
                MageryScrolls.Add(new KeyValuePair<string, int>("IncognitoScroll", 0x1F4F));
                MageryScrolls.Add(new KeyValuePair<string, int>("MagicReflectScroll", 0x1F50));
                MageryScrolls.Add(new KeyValuePair<string, int>("MindBlastScroll", 0x1F51));
                MageryScrolls.Add(new KeyValuePair<string, int>("ParalyzeScroll", 0x1F52));
                MageryScrolls.Add(new KeyValuePair<string, int>("PoisonFieldScroll", 0x1F53));
                MageryScrolls.Add(new KeyValuePair<string, int>("SummonCreatureScroll", 0x1F54));

                // Sixth Circle Scrolls
                MageryScrolls.Add(new KeyValuePair<string, int>("DispelScroll", 0x1F55));
                MageryScrolls.Add(new KeyValuePair<string, int>("EnergyBoltScroll", 0x1F56));
                MageryScrolls.Add(new KeyValuePair<string, int>("ExplosionScroll", 0x1F57));
                MageryScrolls.Add(new KeyValuePair<string, int>("InvisibilityScroll", 0x1F58));
                MageryScrolls.Add(new KeyValuePair<string, int>("EnergyBoltScroll", 0x1F56)); // Double to make the numbers the same while removing the travel spells that we don't want.
                //MageryScrolls.Add(new KeyValuePair<string, int>("MarkScroll", 0x1F59));
                MageryScrolls.Add(new KeyValuePair<string, int>("MassCurseScroll", 0x1F5A));
                MageryScrolls.Add(new KeyValuePair<string, int>("ParalyzeFieldSpell", 0x1F5B));
                MageryScrolls.Add(new KeyValuePair<string, int>("RevealScroll", 0x1F5C));

                // Seventh Circle Scrolls
                MageryScrolls.Add(new KeyValuePair<string, int>("ChainLightningScroll", 0x1F5D));
                MageryScrolls.Add(new KeyValuePair<string, int>("EnergyFieldScroll", 0x1F5E));
                MageryScrolls.Add(new KeyValuePair<string, int>("FlamestrikeScroll", 0x1F5F));
                //MageryScrolls.Add(new KeyValuePair<string, int>("GateTravelScroll", 0x1F60));
                MageryScrolls.Add(new KeyValuePair<string, int>("ManaVampireScroll", 0x1F61));
                MageryScrolls.Add(new KeyValuePair<string, int>("MassDispelScroll", 0x1F62));
                MageryScrolls.Add(new KeyValuePair<string, int>("MeteorStormScroll", 0x1F63));
                MageryScrolls.Add(new KeyValuePair<string, int>("PolymorphScroll", 0x1F64));
                MageryScrolls.Add(new KeyValuePair<string, int>("PolymorphScroll", 0x1F64));// Double to make the numbers the same while removing the travel spells that we don't want.

                // Eighth Circle Scrolls
                MageryScrolls.Add(new KeyValuePair<string, int>("EarthquakeScroll", 0x1F65));
                MageryScrolls.Add(new KeyValuePair<string, int>("EnergyVortexScroll", 0x1F66));
                MageryScrolls.Add(new KeyValuePair<string, int>("ResurrectionScroll", 0x1F67));
                MageryScrolls.Add(new KeyValuePair<string, int>("SummonAirElementalScroll", 0x1F68));
                MageryScrolls.Add(new KeyValuePair<string, int>("SummonDaemonScroll", 0x1F69));
                MageryScrolls.Add(new KeyValuePair<string, int>("SummonEarthElementalScroll", 0x1F6A));
                MageryScrolls.Add(new KeyValuePair<string, int>("SummonFireElementalScroll", 0x1F6B));
                MageryScrolls.Add(new KeyValuePair<string, int>("SummonWaterElementalScroll", 0x1F6C));

                MageryScrolls.Sort((x, y) => x.Value.CompareTo(y.Value)); // Order by ItemID


                for (int i = 0; i < qntd; i++)
                {
                    // All Circles
                    if (circle == 0)
                    {
                        a = 0;
                        b = 63;
                    }

                    // Specific Circle
                    else if (circle > 0 && circle <= 8)
                    {
                        a = (circle - 1) * 8;
                        b = a + 8;
                    }

                    c = rnd.Next(a, b); // Get ItemID

                    while (sorteados.Contains(c))
                    {
                        c = rnd.Next(a, b);
                    }
                    sorteados.Add(c);

                    Retorno.Add(new KeyValuePair<int, int>(c, MageryScrolls[c].Value));
                }

                return Retorno;
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
                Add(typeof(WizardsHat), 15);
                Add(typeof(BlackPearl), 3); 
                Add(typeof(Bloodmoss), 4); 
                Add(typeof(MandrakeRoot), 2); 
                Add(typeof(Garlic), 2); 
                Add(typeof(Ginseng), 2); 
                Add(typeof(Nightshade), 2); 
                Add(typeof(SpidersSilk), 2); 
                Add(typeof(SulfurousAsh), 2); 

                if (Core.AOS)
                {
                    Add(typeof(BatWing), 1);
                    Add(typeof(DaemonBlood), 3);
                    Add(typeof(PigIron), 2);
                    Add(typeof(NoxCrystal), 3);
                    Add(typeof(GraveDust), 1);
                }

                Add(typeof(RecallRune), 13);
                Add(typeof(Spellbook), 25);

                Type[] types = Loot.RegularScrollTypes;

                for (int i = 0; i < types.Length; ++i)
                    Add(types[i], ((i / 8) + 2) * 2);

                if (Core.SE)
                { 
                    Add(typeof(ExorcismScroll), 3);
                    Add(typeof(AnimateDeadScroll), 8);
                    Add(typeof(BloodOathScroll), 8);
                    Add(typeof(CorpseSkinScroll), 8);
                    Add(typeof(CurseWeaponScroll), 8);
                    Add(typeof(EvilOmenScroll), 8);
                    Add(typeof(PainSpikeScroll), 8);
                    Add(typeof(SummonFamiliarScroll), 8);
                    Add(typeof(HorrificBeastScroll), 8);
                    Add(typeof(MindRotScroll), 10);
                    Add(typeof(PoisonStrikeScroll), 10);
                    Add(typeof(WraithFormScroll), 15);
                    Add(typeof(LichFormScroll), 16);
                    Add(typeof(StrangleScroll), 16);
                    Add(typeof(WitherScroll), 16);
                    Add(typeof(VampiricEmbraceScroll), 20);
                    Add(typeof(VengefulSpiritScroll), 20);
                }
            }
        }
    }
}
