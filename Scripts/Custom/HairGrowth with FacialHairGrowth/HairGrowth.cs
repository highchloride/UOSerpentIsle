// Scripted by Thor86  from   http://http://www.servuo.com/
// Thanks to Runuo's Liacs for the Hairbrush script I got the switching code from http://www.runuo.com/community/threads/runuo-2-0-rc1-hairbrush.73003/ 
// Thanks to Servuo's m309 for telling me about the timer 
// Thanks to Servuo's zerodowned & Enroq for helping with crash issues I got when I first tried to script this
// Got the idea from this from a old sphere shard yearssss ago 

using System;
using Server;
using System.Collections;
using Server.Targeting;
using Server.Mobiles;
using Server.Items;
using Server.Network;


namespace Server.Misc
{

    public class HairGrowthTimer : Timer
    {
        public static void Configure() 
        {
            HairGrowthTimer timer = new HairGrowthTimer();
            timer.Start();
        }

        public HairGrowthTimer()
       : base(TimeSpan.FromSeconds(3600), TimeSpan.FromSeconds(3600)) // 3600 seconds is 1 hour 
     //    : base(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10)) // set at 10secs for testing to show the hair growth cycle
        {
            Priority = TimerPriority.OneSecond;
        }
      
        protected override void OnTick()
        {
            HairGrowth();                                                      
        }

        public static void HairGrowth()
        {
            foreach (NetState ns in NetState.Instances)
            {
                if (ns.Mobile != null)
                {
                    HairGrowth(ns.Mobile);
                }
            }
        } 
        public static void HairGrowth(Mobile m) 
        {
            if (m is PlayerMobile)
            // SHORT
            if (m.HairItemID == 0) // None
            {
                m.HairItemID = 0x203B; // Short
                return;
            }
            if (m.HairItemID == 0x2044) // Mohawk
            {
                m.HairItemID = 0x203B; // Short
                return;
             }
            if (m.HairItemID == 0x204A) // Krisna
            {
                m.HairItemID = 0x203B; // Short
                return;
            }
            if (m.HairItemID == 0x203B) // Short
            {
                m.HairItemID = 0x2FD1; // Spiked
                return;
            }
            if (m.HairItemID == 0x2FC1) // ShortElven                     
            {
                m.HairItemID = 0x2FD1; // Spiked
                return;
            }
            if (m.HairItemID == 0x2FD1) // Spiked                    
            {
                m.HairItemID = 0x2045; // PageBoy
                return;
            }
            if (m.HairItemID == 0x2045) // PageBoy                    
            {
                m.HairItemID = 0x2047; // Afro
                return;
            }
            // MED
            if (m.HairItemID == 0x2047) // Afro                   
            {
                m.HairItemID = 0x2FBF; // MidLong
                return;
            }
            if (m.HairItemID == 0x2FBF) // MidLong                  
            {
                m.HairItemID = 0x2FC2; // Mullet
                return;
            }
            if (m.HairItemID == 0x2FC2) // Mullet                 
            {
                m.HairItemID = 0x2FCE; // ElfKnot
                return;
            }
            //LONG
            if (m.HairItemID == 0x2FCE) // ElfKnot
            {
                m.HairItemID = 0x2FD0; // BigBun
                return;
            }
            if (m.HairItemID == 0x2046) // Bun
            {
                m.HairItemID = 0x203C; // Long
                return;
            }
            if (m.HairItemID == 0x2FD0) // BigBun
            {
                m.HairItemID = 0x203C; // Long
                return;
            }
            if (m.HairItemID == 0x203D) // Ponytail
            {
                m.HairItemID = 0x203C; // Long
                return;
            }
            if (m.HairItemID == 0x2FCF) // BraidElf
            {
                m.HairItemID = 0x2FCD; // LongElf
                return;
            }
            if (m.HairItemID == 0x2049) // Two Pigtails
            {
                m.HairItemID = 0x2FCF; // BraidElf
                return;
            }
            if (m.HairItemID == 0x2FCC) // Flower
            {
                m.HairItemID = 0x2049; // Two Pigtails
                return;
            }

            /*  uncomment for balding effect
                        if (m.HairItemID == 0x2FCD) // LongElf
                        {
                            m.HairItemID = 0x2048; // receeding
                        return;
                        }
                        if (m.HairItemID == 0x203C) // Long
                        {
                            m.HairItemID = 0x2048; // receeding
                        return;
                        }
            */ 

            if (m.HairItemID == 0x2048) // receeding
            {
                m.SendMessage("");  // hair wont grow anymore if its or after it reaches receeding
                return;
            }

            {
                m.SendMessage(""); // m.SendMessage("Your hair stopped growing.");
                return;
            }

            if (m == null)
                return; 

   
             }
        }
    }
