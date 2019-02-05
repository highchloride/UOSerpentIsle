//////////////////////////////////
//				                //
//				                //
//				                //
//    Created by Lord Talon	    //
//     www.uohelmsdeep.com	    //
//				                //
//				                //
//				                //
//////////////////////////////////	

/* DESCRIPTION: A player guide gump that contains all player commands, shardinfo. Aswell as 
 *              some other useful information
 */

using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Mobiles;

namespace Server.Gumps
{
    public class PlayerGuidegump : Gump
    {
        public PlayerGuidegump()
            : base(0, 0)
        {

            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            this.AddPage(1);
            this.AddBackground(23, 50, 450, 166, 9250);

            this.AddLabel(195, 65, 0, @"Player Guide");//......Gump Title
            this.AddLabel(85, 90, 0, @"Commands");//......Button1 Title
            this.AddLabel(85, 120, 0, @"Server Specs");//......Button2 Title
            this.AddLabel(85, 150, 0, @"Shard Info");//......Button3 Title
            this.AddButton(40, 90, 4005, 4005, (int)Buttons.Button1, GumpButtonType.Page, 2);//.......Go to page 2
            this.AddButton(40, 120, 4005, 4005, (int)Buttons.Button2, GumpButtonType.Page, 3);//......Go to page 3
            this.AddButton(40, 150, 4005, 4005, (int)Buttons.Button3, GumpButtonType.Page, 4);//......Go to page 4

            this.AddPage(2);
            this.AddBackground(23, 50, 525, 303, 9250);

            this.AddLabel(195, 65, 0, @"Player Commands");//......Gump Title
            this.AddButton(35, 320, 4014, 4014, (int)Buttons.BackButton1, GumpButtonType.Page, 1);//......Back to page 1
            this.AddImage(455, 71, 9000);
            this.AddHtml(42, 85, 407, 224, @"
[hunger: Displays the Hunger/Thirst gump.
[time: Displays the in-game and server times.
[changeHairStyle: Only usable near Barbers, adjusts hair and beard.
[afk: Set yourself away for others to know.", (bool)true, (bool)true);//......Add your command list here
            this.AddButton(215, 320, 4020, 4020, (int)Buttons.WebsiteButton1, GumpButtonType.Reply, 0);//......To command list on website
            this.AddLabel(250, 320, 0, @"Check website for a more detailed list");//......Website button label


            this.AddPage(3);
            this.AddBackground(23, 50, 450, 228, 9250);

            this.AddLabel(195, 65, 0, @"Server Specs");//......Gump Title
            this.AddLabel(50, 95, 0, @"Client Version:");
            this.AddLabel(155, 95, 0, @"7.0.20.0");//......Detects Client Version  **Disabled, you must manually put it**
            this.AddLabel(50, 120, 0, @"Client Patch:");
            this.AddLabel(185, 120, 0, @"59");//......Detects player patch  **Disabled, you must manually put it**
            this.AddLabel(50, 145, 0, @"Skillcap:");
            this.AddLabel(110, 145, 0, @"700");//......Set Skillcap
            this.AddLabel(50, 170, 0, @"Statcap:");
            this.AddLabel(110, 170, 0, @"225");//......Set Statcap
            this.AddLabel(50, 195, 0, @"Accounts per IP:");
            this.AddLabel(160, 195, 0, @"1");//......Set Accounts pe IP limit
            this.AddButton(35, 245, 4014, 4014, (int)Buttons.BackButton3, GumpButtonType.Page, 1);//......Back to page 1
            this.AddImage(395, 200, 5608);

            this.AddPage(4);
            this.AddBackground(23, 50, 450, 228, 9250);

            this.AddLabel(195, 65, 0, @"Shard Info");//......Gump Title
            this.AddLabel(85, 95, 0, @"Shard Rules");//......Button title 1
            this.AddLabel(85, 120, 0, @"Shard Features");//......Button title 2
            this.AddLabel(85, 145, 0, @"Donation Item's");//......Button title 3
            this.AddLabel(85, 170, 0, @"Shard Updates");//......Button title 4
            this.AddButton(50, 95, 4020, 4020, (int)Buttons.WebsiteButton2, GumpButtonType.Reply, 0);//......To shard rules on website
            this.AddButton(50, 120, 4020, 4020, (int)Buttons.WebsiteButton3, GumpButtonType.Reply, 0);//......To shard features on website
            this.AddButton(50, 145, 4020, 4020, (int)Buttons.WebsiteButton4, GumpButtonType.Reply, 0);//......To Donation store
            this.AddButton(50, 170, 4020, 4020, (int)Buttons.WebsiteButton5, GumpButtonType.Reply, 0);//......To Shard updates on website
            this.AddButton(35, 245, 4014, 4014, (int)Buttons.BackButton4, GumpButtonType.Page, 1);//......Back to page 1

        }
                
        public enum Buttons
        {
            Button1,
            Button2,
            Button3,
            BackButton1,
            BackButton3,
            BackButton4,
            WebsiteButton1,
            WebsiteButton2,
            WebsiteButton3,
            WebsiteButton4,
            WebsiteButton5,
        }


        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile m = sender.Mobile;
            {
                switch (info.ButtonID)
                {
                    case (int)Buttons.WebsiteButton1:
                        sender.LaunchBrowser("http://www.uoserpentisle.com");//......Command List Url
                        break;

                    case (int)Buttons.WebsiteButton2:
                        sender.LaunchBrowser("http://www.uoserpentisle.com");//......Shard Rules Url
                        break;

                    case (int)Buttons.WebsiteButton3:
                        sender.LaunchBrowser("http://www.uoserpentisle.com");//......Shard Features Url
                        break;

                    case (int)Buttons.WebsiteButton4:
                        sender.LaunchBrowser("http://www.uoserpentisle.com");//......Shard Donation Store Url
                        break;

                    case (int)Buttons.WebsiteButton5:
                        sender.LaunchBrowser("http://www.uoserpentisle.com");//......Shard Updates Url
                        break;
                }
            }
        }
    }
}

