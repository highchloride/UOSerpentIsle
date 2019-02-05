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

    public class FacialHairGrowthTimer : Timer
    {
        public static void Configure() 
        {
            FacialHairGrowthTimer timer = new FacialHairGrowthTimer();
            timer.Start();
        }

        public FacialHairGrowthTimer()
       : base(TimeSpan.FromSeconds(3600), TimeSpan.FromSeconds(3600)) // 3600 seconds is 1 hour         
     //    : base(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10)) // set at 10secs for testing to show the hair growth cycle
        {
            Priority = TimerPriority.OneSecond;
        }
      
        protected override void OnTick()
        {
            FacialHairGrowth();                                                      
        }

        public static void FacialHairGrowth()
        {
            foreach (NetState ns in NetState.Instances)
            {
                if (ns.Mobile != null)
                {
                    FacialHairGrowth(ns.Mobile);
                }
            }
        }
        public static void FacialHairGrowth(Mobile m) 
        {
            if (m is PlayerMobile)
                if (m.Female)
                {
                    m.FacialHairItemID = 0; // None
                    return;
                }
            //
            if (m.FacialHairItemID == 0) // None
            {
                m.FacialHairItemID = 0x2040; // Goatee
                return;
            }
            if (m.FacialHairItemID == 0x2040) // Goatee
            {
                m.FacialHairItemID = 0x2041; // Mustache
                return;
             }
            if (m.FacialHairItemID == 0x2041) // Mustache
            {
                m.FacialHairItemID = 0x204D; // Vandyke
                return;
            }
            if (m.FacialHairItemID == 0x204D) // Vandyke
            {
                m.FacialHairItemID = 0x203F; // ShortBeard
                return;
            }
            if (m.FacialHairItemID == 0x203F) // ShortBeard                    
            {
                m.FacialHairItemID = 0x204B; // MediumShortBeard
                return;
            }
            if (m.FacialHairItemID == 0x204B) // MediumShortBeard                   
            {
                m.FacialHairItemID = 0x203E; // LongBeard
                return;
            }
            if (m.FacialHairItemID == 0x203E) // LongBeard                    
            {
                m.FacialHairItemID = 0x204C; // MediumLongBeard
                return;
            }

            if (m.FacialHairItemID == 0x204C) // MediumLongBeard
            {
                m.SendMessage("");  // hair wont grow anymore if its or after it reaches MediumLongBeard
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
