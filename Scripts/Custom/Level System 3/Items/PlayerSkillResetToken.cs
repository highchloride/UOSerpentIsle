/*
Note To Admins: Skill Points for players will not match up if your
manually boost their skills via command.  The Only skill points that
are not account for are the starting skill points.  Earned skill points
are tracked and can be refunded! Is it possible for the system to track
starting skill and add to skill points when toon is created?  Absolutely.
Will I do it? not likely.  Skills are annoying to say the least...

ALSO.  SkillBalls, if you use a skill ball or anything outside the leveling
system to boost a players skill, unless YOU modify that script to account 
for skill points it will not match up correctly. This is still drag and drop
but only if you choose to use it as intended, otherwise you will be coding 
other scripts to accomodate this one.  
*/

using System;
using Server;
using Server.Misc;
using Server.Items;
using Server.Gumps;
using Server.Network;
using Server.Mobiles;
using Server.Commands;
using Server.Engines.XmlSpawner2;
 
namespace Server
{
	public class SkillResetGump : Gump
	{
		private PlayerSkillResetToken  m_PlayerSkillResetToken;
		private Mobile m_From;
        private const int LabelHue = 0x480; //1153 maybe
        private const int TitleHue = 1153;//0x12B
        private const int LabelHue2 = 155;
		int hue = 1149;

		public SkillResetGump(PlayerSkillResetToken tokenc )
			: base( 0, 0 )
		{		
			this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=true;
			m_PlayerSkillResetToken = tokenc;

            this.AddPage(0);
			this.AddBackground(39, 33, 563, 460, 9270);//5120
			this.AddLabel(55, 41, TitleHue, "Please choose a skill to reset. Check Level Window to see what skill points are available.");
			this.AddButton(530, 460, 2119, 2120, (int)Buttons.Close, GumpButtonType.Reply, 0);
			
			
			this.AddButton(410, 460, 4005, 4007, 67, GumpButtonType.Reply, 67); 
			this.AddLabel(445, 460, TitleHue, "Level Sheet");
			
			this.AddImage(488, 338, 9000);
			this.AddPage(1);
			this.AddLabel(55, 460, TitleHue, "Caution! One click! No confirmation window!");
			this.AddButton(55, 65, 1210, 1209, 40, GumpButtonType.Reply, 40);		//40
            this.AddButton(55, 90, 1210, 1209, 2, GumpButtonType.Reply, 2);			// 2);
            this.AddButton(55, 115, 1210, 1209, 39, GumpButtonType.Reply, 39);		// 39);
            this.AddButton(55, 140, 1210, 1209, 36, GumpButtonType.Reply, 36);		// 36);
            this.AddButton(55, 165, 1210, 1209, 5, GumpButtonType.Reply, 5);		// 5);
            this.AddButton(55, 190, 1210, 1209, 37, GumpButtonType.Reply, 37);		// 37);
            this.AddButton(55, 215, 1210, 1209, 38, GumpButtonType.Reply, 38);		// 38);
            this.AddButton(55, 240, 1210, 1209, 6, GumpButtonType.Reply, 6);		// 6);
            this.AddButton(55, 265, 1210, 1209, 41, GumpButtonType.Reply, 41);		// 41);
            this.AddButton(55, 290, 1210, 1209, 9, GumpButtonType.Reply, 9);		// 9);
            this.AddButton(55, 315, 1210, 1209, 13, GumpButtonType.Reply, 13);		// 13);
            this.AddButton(55, 340, 1210, 1209, 34, GumpButtonType.Reply, 34);		// 34);
            this.AddButton(55, 365, 1210, 1209, 33, GumpButtonType.Reply, 33);		// 33);
            this.AddButton(55, 390, 1210, 1209, 15, GumpButtonType.Reply, 15);		//, 15);
            this.AddButton(55, 415, 1210, 1209, 14, GumpButtonType.Reply, 14);		//, 14);
            this.AddButton(55, 440, 1210, 1209, 56, GumpButtonType.Reply, 56);		//, 56);
            this.AddLabel(80, 65, LabelHue2, @"Tactics");			//40
            this.AddLabel(80, 90, LabelHue2, @"Anatomy");          	//2
            this.AddLabel(80, 115, LabelHue2, @"Swordsmanship");   	//39
            this.AddLabel(80, 140, LabelHue2, @"Fencing");         	//36
            this.AddLabel(80, 165, LabelHue2, @"Archery");         	//5
            this.AddLabel(80, 190, LabelHue2, @"Macefighting");    	//37
            this.AddLabel(80, 215, LabelHue2, @"Parry");           	//38
            this.AddLabel(80, 240, LabelHue2, @"Arms Lore");       	//6
            this.AddLabel(80, 265, LabelHue2, @"Wrestling");       	//41
            this.AddLabel(80, 290, LabelHue2, @"Blacksmithing");	//9
            this.AddLabel(80, 315, LabelHue2, @"Carpentry");		//13
            this.AddLabel(80, 340, LabelHue2, @"Tinkering");       	//34
            this.AddLabel(80, 365, LabelHue2, @"Tailoring");       	//33
            this.AddLabel(80, 390, LabelHue2, @"Fishing");         	//15
            this.AddLabel(80, 415, LabelHue2, @"Cooking");         	//14
            this.AddLabel(80, 440, LabelHue2, @"Fletching");       	//56
            this.AddButton(200, 65, 1210, 1209, 23, GumpButtonType.Reply, 23);	//, 23);
            this.AddButton(200, 90, 1210, 1209, 20, GumpButtonType.Reply, 20);	//, 20);
            this.AddButton(200, 115, 1210, 1209, 1, GumpButtonType.Reply, 1);	//, 1);
            this.AddButton(200, 140, 1210, 1209, 44, GumpButtonType.Reply, 44); //, 44);
            this.AddButton(200, 165, 1210, 1209, 21, GumpButtonType.Reply, 21); //, 21);
            this.AddButton(200, 190, 1210, 1209, 48, GumpButtonType.Reply, 48); //, 48);
            this.AddButton(200, 215, 1210, 1209, 50, GumpButtonType.Reply, 50); //, 50);
            this.AddButton(200, 240, 1210, 1209, 22, GumpButtonType.Reply, 22); //, 22);
            this.AddButton(200, 265, 1210, 1209, 55, GumpButtonType.Reply, 55); //, 55);
            this.AddButton(200, 290, 1210, 1209, 32, GumpButtonType.Reply, 32); //, 32);
            this.AddButton(200, 315, 1210, 1209, 29, GumpButtonType.Reply, 29); //, 29);
            this.AddButton(200, 340, 1210, 1209, 31, GumpButtonType.Reply, 31); //, 31);
            this.AddButton(200, 365, 1210, 1209, 19, GumpButtonType.Reply, 19); //, 19);
            this.AddButton(200, 390, 1210, 1209, 43, GumpButtonType.Reply, 43); //, 43);
            this.AddButton(200, 415, 1210, 1209, 27, GumpButtonType.Reply, 27); //, 27);
            this.AddButton(200, 440, 1210, 1209, 45, GumpButtonType.Reply, 45); //, 45);
            this.AddLabel(225, 65, LabelHue2, @"Mining");				//23
            this.AddLabel(225, 90, LabelHue2, @"Lumberjacking");  		//20
            this.AddLabel(225, 115, LabelHue2, @"Alchemy");				//1
            this.AddLabel(225, 140, LabelHue2, @"Inscription");			//44
            this.AddLabel(225, 165, LabelHue2, @"Magery");        	 	//21
            this.AddLabel(225, 190, LabelHue2, @"Spirit Speak");  	 	//48
            this.AddLabel(225, 215, LabelHue2, @"Evaluating Int"); 		//50
            this.AddLabel(225, 240, LabelHue2, @"Meditation");     		//22
            this.AddLabel(225, 265, LabelHue2, @"Hiding");				//55
            this.AddLabel(225, 290, LabelHue2, @"Stealth");       	 	//32
            this.AddLabel(225, 315, LabelHue2, @"Snooping");      	 	//29
            this.AddLabel(225, 340, LabelHue2, @"Stealing");      	 	//31
            this.AddLabel(225, 365, LabelHue2, @"Lockpicking");   	 	//19
            this.AddLabel(225, 390, LabelHue2, @"Detecting Hidden"); 	//43
            this.AddLabel(225, 415, LabelHue2, @"Remove Trap");     	//27
            this.AddLabel(225, 440, LabelHue2, @"Tracking");        	//45
            this.AddButton(345, 65, 1210, 1209, 46, GumpButtonType.Reply, 46); 	//, 46);
            this.AddButton(345, 90, 1210, 1209, 4, GumpButtonType.Reply, 4); 	//, 4);
            this.AddButton(345, 115, 1210, 1209, 3, GumpButtonType.Reply, 3); 	//, 3);
            this.AddButton(345, 140, 1210, 1209, 11, GumpButtonType.Reply, 11); //, 11);
            this.AddButton(345, 165, 1210, 1209, 24, GumpButtonType.Reply, 24); //, 24);
            this.AddButton(345, 190, 1210, 1209, 47, GumpButtonType.Reply, 47); //, 47);
            this.AddButton(345, 215, 1210, 1209, 45, GumpButtonType.Reply, 45); //, 45);
            this.AddButton(345, 240, 1210, 1209, 52, GumpButtonType.Reply, 52); //, 52);
            this.AddButton(345, 265, 1210, 1209, 53, GumpButtonType.Reply, 53); //, 53);
            this.AddButton(345, 290, 1210, 1209, 51, GumpButtonType.Reply, 51); //, 51);
            this.AddButton(345, 315, 1210, 1209, 7, GumpButtonType.Reply, 7); 	//, 7);
            this.AddButton(345, 340, 1210, 1209, 17, GumpButtonType.Reply, 17); //, 17);
            this.AddButton(345, 365, 1210, 1209, 18, GumpButtonType.Reply, 18); //, 18);
            this.AddButton(345, 390, 1210, 1209, 28, GumpButtonType.Reply, 28); //, 28);
            this.AddButton(345, 415, 1210, 1209, 35, GumpButtonType.Reply, 35); //, 35);
            this.AddButton(345, 440, 1210, 1209, 42, GumpButtonType.Reply, 42); //, 42);
            this.AddLabel(370, 65, LabelHue2, @"Poisoning");       		//46
            this.AddLabel(370, 90, LabelHue2, @"Animal Taming");   		//4
            this.AddLabel(370, 115, LabelHue2, @"Animal Lore");			//3
            this.AddLabel(370, 140, LabelHue2, @"Camping");       	 	//11
            this.AddLabel(370, 165, LabelHue2, @"Musicianship");  	 	//24
            this.AddLabel(370, 190, LabelHue2, @"Provocation");    		//47
            this.AddLabel(370, 215, LabelHue2, @"Peacemaking");  	  	//45
            this.AddLabel(370, 240, LabelHue2, @"Item Ident"); 			//52
            this.AddLabel(370, 265, LabelHue2, @"Taste Ident"); 		//53
            this.AddLabel(370, 290, LabelHue2, @"Foresic Evaluation");  //51
            this.AddLabel(370, 315, LabelHue2, @"Begging"); 			//7
            this.AddLabel(370, 340, LabelHue2, @"Healing"); 			//17
            this.AddLabel(370, 365, LabelHue2, @"Herding"); 			//18
            this.AddLabel(370, 390, LabelHue2, @"Resisting Spells"); 	//28
            this.AddLabel(370, 415, LabelHue2, @"Veterinary"); 			//35
            this.AddLabel(370, 440, LabelHue2, @"Cartography");   	 	//42
            //this.AddButton(490, 65,  1210, 1209, 57, GumpButtonType.Reply, 57); //, 57);
            //this.AddButton(490, 90,  1210, 1209, 59, GumpButtonType.Reply, 59); //, 59);
            //this.AddButton(490, 115, 1210, 1209, 60, GumpButtonType.Reply, 60); //, 60);
            //this.AddButton(490, 140, 1210, 1209, 63, GumpButtonType.Reply, 63); //, 63);
            //this.AddButton(490, 165, 1210, 1209, 64, GumpButtonType.Reply, 64); //, 64);
            //this.AddButton(490, 190, 1210, 1209, 65, GumpButtonType.Reply, 65); //, 65);
            this.AddButton(490, 215, 1210, 1209, 66, GumpButtonType.Reply, 66); //, 66);
            //this.AddLabel(515, 65,  LabelHue2, @"Focus");     				 	//57
            //this.AddLabel(515, 90,  LabelHue2, @"Bushido");  	 				//59
            //this.AddLabel(515, 115, LabelHue2, @"chivalry");     				//60
            //this.AddLabel(515, 140, LabelHue2, @"Necromancy");   				//63
            //this.AddLabel(515, 165, LabelHue2, @"Ninjitsu"); 	 				//64
            //this.AddLabel(515, 190, LabelHue2, @"Spellweaving"); 				//65
            this.AddLabel(515, 215, LabelHue2, @"Discordance");  				//66
			//this.AddButton(490, 240, 1210, 1209, 62, GumpButtonType.Reply, 62); //, 62);
            //this.AddButton(490, 265, 1210, 1209, 61, GumpButtonType.Reply, 61); //, 61);
            //this.AddButton(490, 290, 1210, 1209, 58, GumpButtonType.Reply, 58); //, 58);
            //this.AddLabel(515, 240, LabelHue2, @"Mysticism");    				//62
            //this.AddLabel(515, 265, LabelHue2, @"Imbuing");   	 				//61
            //this.AddLabel(515, 290, LabelHue2, @"Throwing");  					//58											
		}
		
		public enum Buttons
		{
			Close,
			FinishButton,
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			Mobile m = state.Mobile;
			XMLPlayerLevelAtt xmlplayer = (XMLPlayerLevelAtt)XmlAttach.FindAttachment(m, typeof(XMLPlayerLevelAtt));
			int totalskillpoints = xmlplayer.TotalSkillPointsUsed;
			int[] numArray = new int[1];
			string str = CommandSystem.Prefix;

            if( m is PlayerMobile )
            {
				int buttonID = info.ButtonID;
            
				if( buttonID == 1)
				{
					if (m.Skills[SkillName.Alchemy].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedAlchemy;
					m.Skills[SkillName.Alchemy].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedAlchemy -= skillpoint;	
					m_PlayerSkillResetToken.Delete();
				}
				if( buttonID == 2)
				{
					if (m.Skills[SkillName.Anatomy].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedAnatomy;
					m.Skills[SkillName.Anatomy].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedAnatomy -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}
				if( buttonID == 3)
				{
					if (m.Skills[SkillName.AnimalLore].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedAnimalLore;
					m.Skills[SkillName.AnimalLore].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedAnimalLore -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}
				if( buttonID == 4)
				{
					if (m.Skills[SkillName.AnimalTaming].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedAnimalTaming;
					m.Skills[SkillName.AnimalTaming].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedAnimalTaming -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}
				if( buttonID == 5)
				{
					if (m.Skills[SkillName.Archery].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedArchery;
					m.Skills[SkillName.Archery].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedArchery -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}
				if( buttonID == 6)
				{
					if (m.Skills[SkillName.ArmsLore].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedArmsLore;
					m.Skills[SkillName.ArmsLore].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedArmsLore -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}
				if( buttonID == 7)
				{
					if (m.Skills[SkillName.Begging].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedBegging;
					m.Skills[SkillName.Begging].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedBegging -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}
				if( buttonID == 9)
				{
					if (m.Skills[SkillName.Blacksmith].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedBlacksmith;
					m.Skills[SkillName.Blacksmith].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedBlacksmith -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}
			

				if( buttonID == 11)
				{
					if (m.Skills[SkillName.Camping].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedCamping;
					m.Skills[SkillName.Camping].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedCamping -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}
				if( buttonID == 13)
				{
					if (m.Skills[SkillName.Carpentry].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedCarpentry;
					m.Skills[SkillName.Carpentry].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedCarpentry -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}
				if( buttonID == 14)
				{
					if (m.Skills[SkillName.Cooking].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedCooking;
					m.Skills[SkillName.Cooking].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedCooking -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}
				if( buttonID == 15)
				{
					if (m.Skills[SkillName.Fishing].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedFishing;
					m.Skills[SkillName.Fishing].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedFishing -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}
				if( buttonID == 17)
				{
					if (m.Skills[SkillName.Healing].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedHealing;
					m.Skills[SkillName.Healing].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedHealing -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}
				if( buttonID == 18)
				{
					if (m.Skills[SkillName.Herding].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedHerding;
					m.Skills[SkillName.Herding].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedHerding -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}
				if( buttonID == 19)
				{
					if (m.Skills[SkillName.Lockpicking].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedLockpicking;
					m.Skills[SkillName.Lockpicking].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedLockpicking -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}
				if( buttonID == 20)
				{
					if (m.Skills[SkillName.Lumberjacking].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedLumberjacking;
					m.Skills[SkillName.Lumberjacking].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedLumberjacking -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}
				if( buttonID == 21)
				{
					if (m.Skills[SkillName.Magery].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedMagery;
					m.Skills[SkillName.Magery].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedMagery -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}
				if( buttonID == 22)
				{
					if (m.Skills[SkillName.Meditation].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedMeditation;
					m.Skills[SkillName.Meditation].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedMeditation -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}
				if( buttonID == 23)
				{
					if (m.Skills[SkillName.Mining].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedMining;
					m.Skills[SkillName.Mining].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedMining -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}			
				if( buttonID == 24)
				{
					if (m.Skills[SkillName.Musicianship].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedMusicianship;
					m.Skills[SkillName.Musicianship].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedMusicianship -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}	
				if( buttonID == 27)
				{
					if (m.Skills[SkillName.RemoveTrap].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedRemoveTrap;
					m.Skills[SkillName.RemoveTrap].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedRemoveTrap -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}	
				if( buttonID == 28)
				{
					if (m.Skills[SkillName.MagicResist].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedMagicResist;
					m.Skills[SkillName.MagicResist].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedMagicResist -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}	
				if( buttonID == 29)
				{
					if (m.Skills[SkillName.Snooping].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedSnooping;
					m.Skills[SkillName.Snooping].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedSnooping -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}	
				if( buttonID == 31)
				{
					if (m.Skills[SkillName.Stealing].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedStealing;
					m.Skills[SkillName.Stealing].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedStealing -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}	
				if( buttonID == 32)
				{
					if (m.Skills[SkillName.Stealth].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedStealth;
					m.Skills[SkillName.Stealth].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedStealth -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}	
				if( buttonID == 33)
				{
					if (m.Skills[SkillName.Tailoring].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedTailoring;
					m.Skills[SkillName.Tailoring].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedTailoring -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}		
				if( buttonID == 34)
				{
					if (m.Skills[SkillName.Tinkering].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedTinkering;
					m.Skills[SkillName.Tinkering].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedTinkering -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}					
				if( buttonID == 35)
				{
					if (m.Skills[SkillName.Veterinary].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedVeterinary;
					m.Skills[SkillName.Veterinary].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedVeterinary -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}	
				if( buttonID == 36)
				{
					if (m.Skills[SkillName.Fencing].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedFencing;
					m.Skills[SkillName.Fencing].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedFencing -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}
				if( buttonID == 37)
				{
					if (m.Skills[SkillName.Macing].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedMacing;
					m.Skills[SkillName.Macing].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedMacing -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}
				if( buttonID == 38)
				{
					if (m.Skills[SkillName.Parry].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedParry;
					m.Skills[SkillName.Parry].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedParry -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}
				if( buttonID == 39)
				{
					if (m.Skills[SkillName.Swords].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedSwords;
					m.Skills[SkillName.Swords].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedSwords -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}
				if( buttonID == 40)
				{
					if (m.Skills[SkillName.Tactics].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedTactics;
					m.Skills[SkillName.Tactics].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedTactics -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}				
				if( buttonID == 41)
				{
					if (m.Skills[SkillName.Wrestling].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedWrestling;
					m.Skills[SkillName.Wrestling].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedWrestling -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}	
				if( buttonID == 42)
				{
					if (m.Skills[SkillName.Cartography].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedCartography;
					m.Skills[SkillName.Cartography].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedCartography -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}					
				if( buttonID == 43)
				{
					if (m.Skills[SkillName.DetectHidden].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedDetectHidden;
					m.Skills[SkillName.DetectHidden].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedDetectHidden -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}	
				if( buttonID == 44)
				{
					if (m.Skills[SkillName.Inscribe].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedInscribe;
					m.Skills[SkillName.Inscribe].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedInscribe -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}
				if( buttonID == 45)
				{
					if (m.Skills[SkillName.Peacemaking].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedPeacemaking;
					m.Skills[SkillName.Peacemaking].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedPeacemaking -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}
				if( buttonID == 46)
				{
					if (m.Skills[SkillName.Poisoning].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedPoisoning;
					m.Skills[SkillName.Poisoning].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedPoisoning -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}
				if( buttonID == 47)
				{
					if (m.Skills[SkillName.Provocation].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedProvocation;
					m.Skills[SkillName.Provocation].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedProvocation -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}				
				if( buttonID == 48)
				{
					if (m.Skills[SkillName.SpiritSpeak].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedSpiritSpeak;
					m.Skills[SkillName.SpiritSpeak].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedSpiritSpeak -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}		
				if( buttonID == 49)
				{
					if (m.Skills[SkillName.Tracking].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedTracking;
					m.Skills[SkillName.Tracking].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedTracking -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}				
				if( buttonID == 50)
				{
					if (m.Skills[SkillName.EvalInt].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedEvalInt;
					m.Skills[SkillName.EvalInt].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedEvalInt -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}	
				if( buttonID == 51)
				{
					if (m.Skills[SkillName.Forensics].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedForensics;
					m.Skills[SkillName.Forensics].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedForensics -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}
				if( buttonID == 52)
				{
					if (m.Skills[SkillName.ItemID].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedItemID;
					m.Skills[SkillName.ItemID].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedItemID -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}				
				if( buttonID == 53)
				{
					if (m.Skills[SkillName.TasteID].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedTasteID;
					m.Skills[SkillName.TasteID].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedTasteID -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}	
				if( buttonID == 55)
				{
					if (m.Skills[SkillName.Hiding].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedHiding;
					m.Skills[SkillName.Hiding].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedHiding -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}
				if( buttonID == 56)
				{
					if (m.Skills[SkillName.Fletching].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedFletching;
					m.Skills[SkillName.Fletching].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedFletching -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}				
				if( buttonID == 57)
				{
					if (m.Skills[SkillName.Focus].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedFocus;
					m.Skills[SkillName.Focus].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedFocus -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}
				if( buttonID == 58)
				{
					if (m.Skills[SkillName.Throwing].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedThrowing;
					m.Skills[SkillName.Throwing].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedThrowing -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}
				if( buttonID == 59)
				{
					if (m.Skills[SkillName.Bushido].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedBushido;
					m.Skills[SkillName.Bushido].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedBushido -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}				
				if( buttonID == 60)
				{
					if (m.Skills[SkillName.Chivalry].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedChivalry;
					m.Skills[SkillName.Chivalry].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedChivalry -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}	
				if( buttonID == 61)
				{
					if (m.Skills[SkillName.Imbuing].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedImbuing;
					m.Skills[SkillName.Imbuing].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedImbuing -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}				
				if( buttonID == 62)
				{
					if (m.Skills[SkillName.Mysticism].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedMysticism;
					m.Skills[SkillName.Mysticism].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedMysticism -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}
				if( buttonID == 62)
				{
					if (m.Skills[SkillName.Necromancy].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedNecromancy;
					m.Skills[SkillName.Necromancy].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedNecromancy -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}
				if( buttonID == 64)
				{
					if (m.Skills[SkillName.Ninjitsu].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedNinjitsu;
					m.Skills[SkillName.Ninjitsu].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedNinjitsu -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}
				if( buttonID == 65)
				{
					if (m.Skills[SkillName.Spellweaving].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedSpellweaving;
					m.Skills[SkillName.Spellweaving].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedSpellweaving -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}				
				if( buttonID == 66)
				{
					if (m.Skills[SkillName.Discordance].Base == 0)
					{
						m.SendMessage("Skill has 0 points to refund, try again.");
						m.SendGump(new SkillResetGump(m_PlayerSkillResetToken));
						return;
					}
					int skillpoint = xmlplayer.SkillPointsUsedDiscordance;
					m.Skills[SkillName.Discordance].Base -= skillpoint;
					xmlplayer.TotalSkillPointsUsed -= skillpoint;
					xmlplayer.SKPoints += skillpoint;
					m.SendMessage("{0} points have been returned to your skill pool.", skillpoint);
					xmlplayer.SkillPointsUsedDiscordance -= skillpoint;
					m_PlayerSkillResetToken.Delete();
				}	
				if( buttonID == 67)
				{
					CommandSystem.Handle(m, string.Format("{0}level", str));
				}					
			}
		}        
	}

	public class PlayerSkillResetToken : Item
	{
		[Constructable] 
		public PlayerSkillResetToken() :  base( 0x1869 ) 
		{ 
			Weight = 0.0; 
			Hue = 49; 
			Name = "Single Skill Reset Token"; 
			Movable =  true;
			ItemID = 10922;
            LootType = LootType.Blessed;
		}
		public override void OnDoubleClick( Mobile m )
		{
            if (m.Backpack != null && m.Backpack.GetAmount(typeof(PlayerSkillResetToken)) > 0)
            {
                m.SendMessage("Please choose a single skill to reset to 0 and get points back.");
                m.CloseGump(typeof(SkillResetGump));
                m.SendGump(new SkillResetGump(this));
            }
            else
                m.SendMessage(" You need to have the rest token in backpack to use.");
			
		}
        public PlayerSkillResetToken( Serial serial ) : base( serial ) 
		{ 
		} 

		public override void Serialize( GenericWriter writer ) 
		{ 
			base.Serialize( writer ); 
			writer.Write( (int) 1 ); // version 
		}

		public override void Deserialize( GenericReader reader ) 
		{ 
			base.Deserialize( reader ); 
			int version = reader.ReadInt();
		}
	}         
}
