using System;
using Server;
using System.Collections;
using Server.Network;
using Server.Misc;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;
using Server.Commands;
using Server.Engines.XmlSpawner2;

namespace Server.Gumps
{
    public class PlayerLevelGump : Gump
    {
		Configured c = new Configured();
		
        private Mobile m_From;
        private GumpPage m_Page;
        private SkillCategory m_Cat;
        
        private enum GumpPage
        {
            None,
            SkillList
        }

        public enum SkillCategory
        {
            Misc,
            Combat,
            Trade,
            Magic,
            Wild,
            Bard,
            Thief,
			StatSheet
        }

        //public SkillCategory m_Category;

        private const int LabelHue = 0x480; //1153 maybe
        private const int TitleHue = 1153;//0x12B
        private const int LabelHue2 = 155;
		int hue = 1149;
		
		

		public static void Initialize()
		{
			/* not allowing players access to this command may break some options!  suggest to leave as is!*/
         CommandSystem.Register( "level", AccessLevel.Player, new CommandEventHandler( level_OnCommand ) );
        }
    
		public static void Register( string command, AccessLevel access, CommandEventHandler handler )
		{
            CommandSystem.Register(command, access, handler);
		}

		[Usage( "level" )]
		[Description( "Opens Level Gump." )]
		public static void level_OnCommand( CommandEventArgs e )
		{
			XMLPlayerLevelAtt xmlplayer = (XMLPlayerLevelAtt)XmlAttach.FindAttachment(e.Mobile, typeof(XMLPlayerLevelAtt));
			if (xmlplayer == null)
			{
				e.Mobile.SendMessage("You may not use this!");
				return;
			}
			else
			{
				Mobile from = e.Mobile;
				from.CloseGump( typeof( PlayerLevelGump ) );
				from.SendGump( new PlayerLevelGump( from, GumpPage.None, SkillCategory.Misc ) );
			}
				
        }        
		private PlayerLevelGump ( Mobile from, GumpPage page, SkillCategory cat ) : base( 40, 40 )
		{			
            m_From = from;
            m_Page = page;
            m_Cat = cat;
			
            PlayerMobile pm = from as PlayerMobile;

			m_From.CloseGump( typeof( PlayerLevelGump ) );

			Closable=true;
			Disposable=true;
			Dragable=true;
			Resizable=false;

			AddPage(0);
			AddBackground(50, 35, 540, 382, 9270);

			//custom information
			AddLabel( 100, 69, 1153, "You can go anywhere you want in this world with a single blade."        );						
			AddLabel(262, 56, TitleHue, @"Level Window");

            AddLabel(136, 93, TitleHue, @"Categories");
			
			if (c.Miscelaneous)
			{
				AddButton(75, 116, 4005, 4007, GetButtonID( 1, 0 ), GumpButtonType.Reply, 0);
				AddLabel(112, 117, TitleHue, @"Miscelaneous");
			}
			if (c.Combat)
			{
				AddButton(75, 138, 4005, 4007, GetButtonID( 1, 1 ), GumpButtonType.Reply, 0);
				AddLabel(112, 139, TitleHue, @"Combat");
			}
			if (c.TradeSkills)
			{
				AddButton(75, 160, 4005, 4007, GetButtonID( 1, 2 ), GumpButtonType.Reply, 0);
				AddLabel(112, 161, TitleHue, @"Trade Skills");
			}
			if (c.Magic)
			{
				AddButton(75, 182, 4005, 4007, GetButtonID( 1, 3 ), GumpButtonType.Reply, 0);
				AddLabel(112, 183, TitleHue, @"Magic");
			}
			if (c.Wilderness)
			{
				AddButton(75, 204, 4005, 4007, GetButtonID( 1, 4 ), GumpButtonType.Reply, 0);
				AddLabel(112, 205, TitleHue, @"Wilderness");
			}
			if (c.Thieving)
			{
				AddButton(75, 226, 4005, 4007, GetButtonID( 1, 5 ), GumpButtonType.Reply, 0);
				AddLabel(112, 227, TitleHue, @"Thieving");
			}
			if (c.Bard)
			{
				AddButton(75, 248, 4005, 4007, GetButtonID( 1, 6 ), GumpButtonType.Reply, 0);
				AddLabel(112, 249, TitleHue, @"Bard");
			}
			
			AddImage(0, 4, 10440);
			AddImage(554, 4, 10441);

            CreatePlayerExpList(from);

            int totalBaseStats;

            totalBaseStats = pm.RawStr + pm.RawInt + pm.RawDex;

			AddButton(75, 379, 241, 243, 0, GumpButtonType.Reply, 0); //Cancel
			AddButton(150, 379, 2027, 2028, GetButtonID(1,65), GumpButtonType.Reply, 0);
			
			
            AddButton(225, 379, 4005, 4007, GetButtonID(1, 69), GumpButtonType.Reply, 0);
            AddLabel(260, 379, LabelHue2, @"StatSheet"); //case 69
			
			
			
            AddLabel(335, 380, LabelHue2, @" StatTotal: ");
            AddLabel(405, 380, LabelHue, totalBaseStats.ToString());
            AddLabel(430, 380, LabelHue2, @" SkillTotal: ");
            int totalSkill = pm.SkillsTotal;
            totalSkill = totalSkill / 10;
            AddLabel(500, 380, LabelHue, totalSkill.ToString());

            if (page == GumpPage.SkillList)
            {
                CreateSkillList(from, cat);
                return;
            }
            else
            {
                CreatePlayerStats(from); 
                return;
            }
            

		}
		
        public void CreatePlayerExpList( Mobile from )
		{
			XMLPlayerLevelAtt xmlplayer = (XMLPlayerLevelAtt)XmlAttach.FindAttachment(from, typeof(XMLPlayerLevelAtt));
			PlayerMobile pm = from as PlayerMobile;
           		
            AddLabel(75, 275, LabelHue2, @"Current Level:");
            AddLabel(164, 275, LabelHue, xmlplayer.Levell.ToString());

            AddLabel(75, 297, LabelHue2, @"Experience:");
            AddLabel(144, 297, LabelHue, xmlplayer.Expp.ToString());

            AddLabel(75, 319, LabelHue2, @"Exp. to next level:");
            AddLabel(191, 319, LabelHue, xmlplayer.ToLevell.ToString());

            AddLabel(75, 341, LabelHue2, @"Skill Points Avail:");
            AddLabel(185, 341, LabelHue, xmlplayer.SKPoints.ToString());

            return;
        }

        public void CreatePlayerStats(Mobile from)
        {
			XMLPlayerLevelAtt xmlplayer = (XMLPlayerLevelAtt)XmlAttach.FindAttachment(from, typeof(XMLPlayerLevelAtt));

            PlayerMobile pm = from as PlayerMobile;
            AddLabel(337, 93, TitleHue, @"Raw Stats - Bonus not included");

            AddButton(300, 116, 4005, 4007, GetButtonID(1, 7), GumpButtonType.Reply, 0);
            AddLabel(337, 117, LabelHue2, @"Strength:");
            AddLabel(407, 117, LabelHue, pm.RawStr.ToString());

            AddButton(300, 138, 4005, 4007, GetButtonID(1, 8), GumpButtonType.Reply, 0);
            AddLabel(337, 139, LabelHue2, @"Intelligence:");
            AddLabel(407, 139, LabelHue, pm.RawInt.ToString());

            AddButton(300, 160, 4005, 4007, GetButtonID(1, 9), GumpButtonType.Reply, 0);
            AddLabel(337, 161, LabelHue2, @"Dexterity:");
            AddLabel(407, 161, LabelHue, pm.RawDex.ToString());

            AddLabel(300, 183, LabelHue2, @"Available stat points: ");
            AddLabel(435, 183, LabelHue, xmlplayer.StatPoints.ToString());
			
            AddLabel(434, 117, LabelHue2, @"Str Points Used: ");
            AddLabel(544, 117, LabelHue, xmlplayer.StrPointsUsed.ToString());
			
			AddLabel(434, 139, LabelHue2, @"Int  Points Used: ");
            AddLabel(544, 139, LabelHue, xmlplayer.IntPointsUsed.ToString());
			
			AddLabel(434, 161, LabelHue2, @"Dex Points Used: ");
            AddLabel(544, 161, LabelHue, xmlplayer.DexPointsUsed.ToString());
			
			
			
			AddLabel(300, 199, 1153, @"Vitals");
			AddLabel(400, 199, 1153, @"Bonus");
            AddLabel(450, 199, 1153, @"Regen");
            
			
			AddLabel(300, 214, 155, @"HP: ");
			AddLabel(330, 214, hue, @"" + pm.Hits + "/" + pm.HitsMax + "");
			AddLabel(400, 214, hue, @"(+ " + AosAttributes.GetValue(pm, AosAttribute.BonusHits) + ")");
			AddLabel(458, 214, hue, @"" + AosAttributes.GetValue(pm, AosAttribute.RegenHits) + "");

			AddLabel(300, 229, 155, @"MP: ");
			AddLabel(330, 229, hue, @"" + pm.Mana + "/" + pm.ManaMax + "");
			AddLabel(400, 229, hue, @"(+ " + AosAttributes.GetValue(pm, AosAttribute.BonusMana) + ")");
            AddLabel(458, 229, hue, @"" + AosAttributes.GetValue(pm, AosAttribute.RegenMana) + "");
			
			AddLabel(300, 244, 155, @"SP: ");
			AddLabel(330, 244, hue, @"" + pm.Stam + "/" + pm.StamMax + "");
			AddLabel(400, 244, hue, @"(+ " + AosAttributes.GetValue(pm, AosAttribute.BonusStam) + ")");
            AddLabel(458, 244, hue, @"" + AosAttributes.GetValue(pm, AosAttribute.RegenStam) + "");
			
			AddLabel(300, 259, hue, @"Adding is Based on RawStats.");
			AddLabel(300, 274, hue, @"Bonus stats included with vitals.");
			
			if(c.LevelSheetStatResetButton == true)
			{
				AddButton(300, 290, 4005, 4007, GetButtonID(1, 70), GumpButtonType.Reply, 0);
				AddLabel(340, 290, LabelHue2, @"Master Stat Reset - No Confirmation!");
			}
			if(c.LevelSheetSkillResetButton == true)
			{
				AddButton(300, 305, 4005, 4007, GetButtonID(1, 71), GumpButtonType.Reply, 0);
				AddLabel(340, 305, LabelHue2, @"Master Skill Reset - No Confirmation!");
			}
            return;

        }
        public void CreateSkillList(Mobile from, SkillCategory cat)
        {
            m_From = from;
            m_Cat = cat;
            PlayerMobile pm = from as PlayerMobile;
            Mobile m = from as Mobile;
			XMLPlayerLevelAtt xmlplayer = (XMLPlayerLevelAtt)XmlAttach.FindAttachment(from, typeof(XMLPlayerLevelAtt));
 
			// configuration
            int LRCCap = 100;
            int LMCCap = 40;
            double BandageSpeedCap = 0.0;
            int SwingSpeedCap = 125;
            int HCICap = 45;
            int DCICap = 45;
            int FCCap = 4;
            int FCRCap = 6;
            int DamageIncreaseCap = 100;
            int SDICap = 100;
            int ReflectDamageCap = 50;
            int SSICap = 100;

            int LRC = AosAttributes.GetValue(pm, AosAttribute.LowerRegCost) > LRCCap ? LRCCap : AosAttributes.GetValue(pm, AosAttribute.LowerRegCost);
            int LMC = AosAttributes.GetValue(pm, AosAttribute.LowerManaCost) > LMCCap ? LMCCap : AosAttributes.GetValue(pm, AosAttribute.LowerManaCost);
            double BandageSpeed = (2.0 + (0.5 * ((double)(205 - pm.Dex) / 10))) < BandageSpeedCap ? BandageSpeedCap : (2.0 + (0.5 * ((double)(205 - pm.Dex) / 10)));
            TimeSpan SwingSpeed = (pm.Weapon as BaseWeapon).GetDelay(pm) > TimeSpan.FromSeconds(SwingSpeedCap) ? TimeSpan.FromSeconds(SwingSpeedCap) : (pm.Weapon as BaseWeapon).GetDelay(pm);
            int HCI = AosAttributes.GetValue(pm, AosAttribute.AttackChance) > HCICap ? HCICap : AosAttributes.GetValue(pm, AosAttribute.AttackChance);
            int DCI = AosAttributes.GetValue(pm, AosAttribute.DefendChance) > DCICap ? DCICap : AosAttributes.GetValue(pm, AosAttribute.DefendChance);
            int FC = AosAttributes.GetValue(pm, AosAttribute.CastSpeed) > FCCap ? FCCap : AosAttributes.GetValue(pm, AosAttribute.CastSpeed);
            int FCR = AosAttributes.GetValue(pm, AosAttribute.CastRecovery) > FCRCap ? FCRCap : AosAttributes.GetValue(pm, AosAttribute.CastRecovery);
            int DamageIncrease = AosAttributes.GetValue(pm, AosAttribute.WeaponDamage) > DamageIncreaseCap ? DamageIncreaseCap : AosAttributes.GetValue(pm, AosAttribute.WeaponDamage);
            int SDI = AosAttributes.GetValue(pm, AosAttribute.SpellDamage) > SDICap ? SDICap : AosAttributes.GetValue(pm, AosAttribute.SpellDamage);
            int ReflectDamage = AosAttributes.GetValue(pm, AosAttribute.ReflectPhysical) > ReflectDamageCap ? ReflectDamageCap : AosAttributes.GetValue(pm, AosAttribute.ReflectPhysical);
            int SSI = AosAttributes.GetValue(pm, AosAttribute.WeaponSpeed) > SSICap ? SSICap : AosAttributes.GetValue(pm, AosAttribute.WeaponSpeed);
			


            AddLabel(375, 93, TitleHue, @"Skills");
			AddLabel(501, 93, TitleHue, @"Points Used");
			if (m_Cat == SkillCategory.Misc)
			{
				AddButton(300, 116, 4005, 4007, GetButtonID(1, 10), GumpButtonType.Reply, 0);
				AddLabel(337, 117, LabelHue2, @"Arms Lore");  
				AddLabel(475, 117, LabelHue, pm.Skills.ArmsLore.Base.ToString());
				AddLabel(501, 117, LabelHue2, @"Points: ");
				AddLabel(551, 117, LabelHue, xmlplayer.SkillPointsUsedArmsLore.ToString());

				AddButton(300, 138, 4005, 4007, GetButtonID(1, 11), GumpButtonType.Reply, 0);
				AddLabel(337, 139,  LabelHue2, @"Begging");  
				AddLabel(475, 139, LabelHue, pm.Skills.Begging.Base.ToString());
				AddLabel(501, 139, LabelHue2, @"Points: ");
				AddLabel(551, 139, LabelHue, xmlplayer.SkillPointsUsedBegging.ToString());

				AddButton(300, 160, 4005, 4007, GetButtonID(1, 12), GumpButtonType.Reply, 0);
				AddLabel(337, 161, LabelHue2, pm.Skills.Camping.Name.ToString());
				AddLabel(475, 161, LabelHue, pm.Skills.Camping.Base.ToString());
				AddLabel(501, 161, LabelHue2, @"Points: ");
				AddLabel(551, 161, LabelHue, xmlplayer.SkillPointsUsedCamping.ToString());	

				AddButton(300, 182, 4005, 4007, GetButtonID(1, 13), GumpButtonType.Reply, 0);
				AddLabel(337, 183, LabelHue2, pm.Skills.Cartography.Name.ToString());
				AddLabel(475, 183, LabelHue, pm.Skills.Cartography.Base.ToString());
				AddLabel(501, 183, LabelHue2, @"Points: ");
				AddLabel(551, 183, LabelHue, xmlplayer.SkillPointsUsedCartography.ToString());

				AddButton(300, 204, 4005, 4007, GetButtonID(1, 14), GumpButtonType.Reply, 0);
				AddLabel(337, 205, LabelHue2, pm.Skills.Forensics.Name.ToString());
				AddLabel(475, 205, LabelHue, pm.Skills.Forensics.Base.ToString());
				AddLabel(501, 205, LabelHue2, @"Points: ");
				AddLabel(551, 205, LabelHue, xmlplayer.SkillPointsUsedForensics.ToString());

				AddButton(300, 226, 4005, 4007, GetButtonID(1, 15), GumpButtonType.Reply, 0);
				AddLabel(337, 227, LabelHue2, pm.Skills.ItemID.Name.ToString());
				AddLabel(475, 227, LabelHue, pm.Skills.ItemID.Base.ToString());
				AddLabel(501, 227, LabelHue2, @"Points: ");
				AddLabel(551, 227, LabelHue, xmlplayer.SkillPointsUsedItemID.ToString());

				AddButton(300, 248, 4005, 4007, GetButtonID(1, 16), GumpButtonType.Reply, 0);
				AddLabel(337, 249, LabelHue2, pm.Skills.TasteID.Name.ToString());
				AddLabel(475, 249, LabelHue, pm.Skills.TasteID.Base.ToString());
				AddLabel(501, 249, LabelHue2, @"Points: ");
				AddLabel(551, 249, LabelHue, xmlplayer.SkillPointsUsedTasteID.ToString());

			
			}
			if (m_Cat == SkillCategory.Combat)
			{
				AddButton(300, 116, 4005, 4007, GetButtonID(1, 17), GumpButtonType.Reply, 0);
				AddLabel(337, 117, LabelHue2, pm.Skills.Anatomy.Name.ToString());
				AddLabel(475, 117, LabelHue, pm.Skills.Anatomy.Base.ToString());
				AddLabel(501, 117, LabelHue2, @"Points: ");
				AddLabel(551, 117, LabelHue, xmlplayer.SkillPointsUsedAnatomy.ToString());

				AddButton(300, 138, 4005, 4007, GetButtonID(1, 18), GumpButtonType.Reply, 0);
				AddLabel(337, 139, LabelHue2, pm.Skills.Archery.Name.ToString());
				AddLabel(475, 139, LabelHue, pm.Skills.Archery.Base.ToString());
				AddLabel(501, 139, LabelHue2, @"Points: ");
				AddLabel(551, 139, LabelHue, xmlplayer.SkillPointsUsedArchery.ToString());

				AddButton(300, 160, 4005, 4007, GetButtonID(1, 19), GumpButtonType.Reply, 0);
				AddLabel(337, 161, LabelHue2, pm.Skills.Fencing.Name.ToString());
				AddLabel(475, 161, LabelHue, pm.Skills.Fencing.Base.ToString());
				AddLabel(501, 161, LabelHue2, @"Points: ");
				AddLabel(551, 161, LabelHue, xmlplayer.SkillPointsUsedFencing.ToString());

                if (c.Focus)
                {
                    AddButton(300, 182, 4005, 4007, GetButtonID(1, 20), GumpButtonType.Reply, 0);
                    AddLabel(337, 183, LabelHue2, pm.Skills.Focus.Name.ToString());
                    AddLabel(475, 183, LabelHue, pm.Skills.Focus.Base.ToString());
                    AddLabel(501, 183, LabelHue2, @"Points: ");
                    AddLabel(551, 183, LabelHue, xmlplayer.SkillPointsUsedFocus.ToString());
                }

				AddButton(300, 204, 4005, 4007, GetButtonID(1, 66), GumpButtonType.Reply, 0);
				AddLabel(337, 205, LabelHue2, pm.Skills.Healing.Name.ToString());
				AddLabel(475, 205, LabelHue, pm.Skills.Healing.Base.ToString());
				AddLabel(501, 205, LabelHue2, @"Points: ");
				AddLabel(551, 205, LabelHue, xmlplayer.SkillPointsUsedHealing.ToString());

				AddButton(300, 226, 4005, 4007, GetButtonID(1, 21), GumpButtonType.Reply, 0);
				AddLabel(337, 227, LabelHue2, pm.Skills.Macing.Name.ToString());
				AddLabel(475, 227, LabelHue, pm.Skills.Macing.Base.ToString());
				AddLabel(501, 227, LabelHue2, @"Points: ");
				AddLabel(551, 227, LabelHue, xmlplayer.SkillPointsUsedMacing.ToString());

				AddButton(300, 248, 4005, 4007, GetButtonID(1, 22), GumpButtonType.Reply, 0);
				AddLabel(337, 249, LabelHue2, pm.Skills.Parry.Name.ToString());
				AddLabel(475, 249, LabelHue, pm.Skills.Parry.Base.ToString());
				AddLabel(501, 249, LabelHue2, @"Points: ");
				AddLabel(551, 249, LabelHue, xmlplayer.SkillPointsUsedParry.ToString());
										
				AddButton(300, 270, 4005, 4007, GetButtonID(1, 23), GumpButtonType.Reply, 0);
				AddLabel(337, 271, LabelHue2, pm.Skills.Swords.Name.ToString());
				AddLabel(475, 271, LabelHue, pm.Skills.Swords.Base.ToString());
				AddLabel(501, 271, LabelHue2, @"Points: ");
				AddLabel(551, 271, LabelHue, xmlplayer.SkillPointsUsedSwords.ToString());

				AddButton(300, 292, 4005, 4007, GetButtonID(1, 24), GumpButtonType.Reply, 0);
				AddLabel(337, 293, LabelHue2, pm.Skills.Tactics.Name.ToString());
				AddLabel(475, 293, LabelHue, pm.Skills.Tactics.Base.ToString());
				AddLabel(501, 293, LabelHue2, @"Points: ");
				AddLabel(551, 293, LabelHue, xmlplayer.SkillPointsUsedTactics.ToString());
									
				AddButton(300, 316, 4005, 4007, GetButtonID(1, 26), GumpButtonType.Reply, 0);
				AddLabel(337, 317, LabelHue2, pm.Skills.Wrestling.Name.ToString());
				AddLabel(475, 317, LabelHue, pm.Skills.Wrestling.Base.ToString());
				AddLabel(501, 317, LabelHue2, @"Points: ");
				AddLabel(551, 317, LabelHue, xmlplayer.SkillPointsUsedWrestling.ToString());
				
				if (c.Throwing)
				{
					AddButton(300, 338, 4005, 4007, GetButtonID(1, 25), GumpButtonType.Reply, 0);
					AddLabel(337, 339, LabelHue2, pm.Skills.Throwing.Name.ToString());
					AddLabel(475, 339, LabelHue, pm.Skills.Throwing.Base.ToString());
					AddLabel(501, 339, LabelHue2, @"Points: ");
					AddLabel(551, 339, LabelHue, xmlplayer.SkillPointsUsedThrowing.ToString());
				}				
			}
			if (m_Cat == SkillCategory.Trade)
			{
				AddButton(300, 116, 4005, 4007, GetButtonID(1, 27), GumpButtonType.Reply, 0);
				AddLabel(337, 117, LabelHue2, pm.Skills.Alchemy.Name.ToString());
				AddLabel(475, 117, LabelHue, pm.Skills.Alchemy.Base.ToString());
				AddLabel(501, 117, LabelHue2, @"Points: ");
				AddLabel(551, 117, LabelHue, xmlplayer.SkillPointsUsedAlchemy.ToString());

				AddButton(300, 138, 4005, 4007, GetButtonID(1, 28), GumpButtonType.Reply, 0);
				AddLabel(337, 139, LabelHue2, pm.Skills.Blacksmith.Name.ToString());
				AddLabel(475, 139, LabelHue, pm.Skills.Blacksmith.Base.ToString());
				AddLabel(501, 139, LabelHue2, @"Points: ");
				AddLabel(551, 139, LabelHue, xmlplayer.SkillPointsUsedBlacksmith.ToString());

				AddButton(300, 160, 4005, 4007, GetButtonID(1, 29), GumpButtonType.Reply, 0);
				AddLabel(337, 161, LabelHue2, pm.Skills.Fletching.Name.ToString());
				AddLabel(475, 161, LabelHue, pm.Skills.Fletching.Base.ToString());
				AddLabel(501, 161, LabelHue2, @"Points: ");
				AddLabel(551, 161, LabelHue, xmlplayer.SkillPointsUsedFletching.ToString());

				AddButton(300, 182, 4005, 4007, GetButtonID(1, 30), GumpButtonType.Reply, 0);
				AddLabel(337, 183, LabelHue2, pm.Skills.Carpentry.Name.ToString());
				AddLabel(475, 183, LabelHue, pm.Skills.Carpentry.Base.ToString());
				AddLabel(501, 183, LabelHue2, @"Points: ");
				AddLabel(551, 183, LabelHue, xmlplayer.SkillPointsUsedCarpentry.ToString());

				AddButton(300, 204, 4005, 4007, GetButtonID(1, 31), GumpButtonType.Reply, 0);
				AddLabel(337, 205, LabelHue2, pm.Skills.Cooking.Name.ToString());
				AddLabel(475, 205, LabelHue, pm.Skills.Cooking.Base.ToString());
				AddLabel(501, 205, LabelHue2, @"Points: ");
				AddLabel(551, 205, LabelHue, xmlplayer.SkillPointsUsedCooking.ToString());

				AddButton(300, 226, 4005, 4007, GetButtonID(1, 32), GumpButtonType.Reply, 0);
				AddLabel(337, 227, LabelHue2, pm.Skills.Inscribe.Name.ToString());
				AddLabel(475, 227, LabelHue, pm.Skills.Inscribe.Base.ToString());
				AddLabel(501, 227, LabelHue2, @"Points: ");
				AddLabel(551, 227, LabelHue, xmlplayer.SkillPointsUsedInscribe.ToString());

				AddButton(300, 248, 4005, 4007, GetButtonID(1, 33), GumpButtonType.Reply, 0);
				AddLabel(337, 249, LabelHue2, pm.Skills.Lumberjacking.Name.ToString());
				AddLabel(475, 249, LabelHue, pm.Skills.Lumberjacking.Base.ToString());
				AddLabel(501, 249, LabelHue2, @"Points: ");
				AddLabel(551, 249, LabelHue, xmlplayer.SkillPointsUsedLumberjacking.ToString());

				AddButton(300, 270, 4005, 4007, GetButtonID(1, 34), GumpButtonType.Reply, 0);
				AddLabel(337, 271, LabelHue2, pm.Skills.Mining.Name.ToString());
				AddLabel(475, 271, LabelHue, pm.Skills.Mining.Base.ToString());
				AddLabel(501, 271, LabelHue2, @"Points: ");
				AddLabel(551, 271, LabelHue, xmlplayer.SkillPointsUsedMining.ToString());

				AddButton(300, 292, 4005, 4007, GetButtonID(1, 35), GumpButtonType.Reply, 0);
				AddLabel(337, 293, LabelHue2, pm.Skills.Tailoring.Name.ToString());
				AddLabel(475, 293, LabelHue, pm.Skills.Tailoring.Base.ToString());
				AddLabel(501, 293, LabelHue2, @"Points: ");
				AddLabel(551, 293, LabelHue, xmlplayer.SkillPointsUsedTailoring.ToString());
				
				AddButton(300, 316, 4005, 4007, GetButtonID(1, 36), GumpButtonType.Reply, 0);
				AddLabel(337, 317, LabelHue2, pm.Skills.Tinkering.Name.ToString());
				AddLabel(475, 317, LabelHue, pm.Skills.Tinkering.Base.ToString());
				AddLabel(501, 317, LabelHue2, @"Points: ");
				AddLabel(551, 317, LabelHue, xmlplayer.SkillPointsUsedTinkering.ToString());
				
				if (c.Imbuing)
				{
					AddButton(300, 338, 4005, 4007, GetButtonID(1, 67), GumpButtonType.Reply, 0);
					AddLabel(337, 339, LabelHue2, pm.Skills.Imbuing.Name.ToString());
					AddLabel(475, 339, LabelHue, pm.Skills.Imbuing.Base.ToString());
					AddLabel(501, 339, LabelHue2, @"Points: ");
					AddLabel(551, 339, LabelHue, xmlplayer.SkillPointsUsedImbuing.ToString());
				}
			}
			if (m_Cat == SkillCategory.Magic)
			{
                if (c.Bushido)
                {
                    AddButton(300, 116, 4005, 4007, GetButtonID(1, 37), GumpButtonType.Reply, 0);
                    AddLabel(337, 117, LabelHue2, @"Bushido");
                    AddLabel(475, 117, LabelHue, pm.Skills.Bushido.Base.ToString());
                    AddLabel(501, 117, LabelHue2, @"Points: ");
                    AddLabel(551, 117, LabelHue, xmlplayer.SkillPointsUsedBushido.ToString());
                }

                if (c.Chivalry)
                {
                    AddButton(300, 138, 4005, 4007, GetButtonID(1, 38), GumpButtonType.Reply, 0);
                    AddLabel(337, 139, LabelHue2, @"Chivalry");
                    AddLabel(475, 139, LabelHue, pm.Skills.Chivalry.Base.ToString());
                    AddLabel(501, 139, LabelHue2, @"Points: ");
                    AddLabel(551, 139, LabelHue, xmlplayer.SkillPointsUsedChivalry.ToString());
                }

				AddButton(300, 160, 4005, 4007, GetButtonID(1, 39), GumpButtonType.Reply, 0);
				AddLabel(337, 161, LabelHue2, pm.Skills.EvalInt.Name.ToString());
				AddLabel(475, 161, LabelHue, pm.Skills.EvalInt.Base.ToString());
				AddLabel(501, 161, LabelHue2, @"Points: ");
				AddLabel(551, 161, LabelHue, xmlplayer.SkillPointsUsedEvalInt.ToString());

				AddButton(300, 182, 4005, 4007, GetButtonID(1, 40), GumpButtonType.Reply, 0);
				AddLabel(337, 183, LabelHue2, @"Magery");
				AddLabel(475, 183, LabelHue, pm.Skills.Magery.Base.ToString());
				AddLabel(501, 183, LabelHue2, @"Points: ");
				AddLabel(551, 183, LabelHue, xmlplayer.SkillPointsUsedMagery.ToString());

				AddButton(300, 204, 4005, 4007, GetButtonID(1, 41), GumpButtonType.Reply, 0);
				AddLabel(337, 205, LabelHue2, pm.Skills.Meditation.Name.ToString());
				AddLabel(475, 205, LabelHue, pm.Skills.Meditation.Base.ToString());
				AddLabel(501, 205, LabelHue2, @"Points: ");
				AddLabel(551, 205, LabelHue, xmlplayer.SkillPointsUsedMeditation.ToString());

                if(c.Necromancy)
                { 
				    AddButton(300, 226, 4005, 4007, GetButtonID(1, 42), GumpButtonType.Reply, 0);
				    AddLabel(337, 227,  LabelHue2, @"Necromancy");  
				    AddLabel(475, 227, LabelHue, pm.Skills.Necromancy.Base.ToString());
				    AddLabel(501, 227, LabelHue2, @"Points: ");
				    AddLabel(551, 227, LabelHue, xmlplayer.SkillPointsUsedNecromancy.ToString());
                }

                if (c.Ninjitsu)
                {
                    AddButton(300, 248, 4005, 4007, GetButtonID(1, 43), GumpButtonType.Reply, 0);
                    AddLabel(337, 249, LabelHue2, @"Ninjitsu");
                    AddLabel(475, 249, LabelHue, pm.Skills.Ninjitsu.Base.ToString());
                    AddLabel(501, 249, LabelHue2, @"Points: ");
                    AddLabel(551, 249, LabelHue, xmlplayer.SkillPointsUsedNinjitsu.ToString());
                }

				AddButton(300, 270, 4005, 4007, GetButtonID(1, 44), GumpButtonType.Reply, 0);
				AddLabel(337, 271, LabelHue2, pm.Skills.MagicResist.Name.ToString());
				AddLabel(475, 271, LabelHue, pm.Skills.MagicResist.Base.ToString());
				AddLabel(501, 271, LabelHue2, @"Points: ");
				AddLabel(551, 271, LabelHue, xmlplayer.SkillPointsUsedMagicResist.ToString());
				
				if (c.Spellweaving)
				{
					AddButton(300, 292, 4005, 4007, GetButtonID(1, 45), GumpButtonType.Reply, 0);
					AddLabel(337, 293,  LabelHue2, @"Spellweaving");  
					AddLabel(475, 293, LabelHue, pm.Skills.Spellweaving.Base.ToString());
					AddLabel(501, 293, LabelHue2, @"Points: ");
					AddLabel(551, 293, LabelHue, xmlplayer.SkillPointsUsedSpellweaving.ToString());
				}
				
				AddButton(300, 316, 4005, 4007, GetButtonID(1, 46), GumpButtonType.Reply, 0);
				AddLabel(337, 317,  LabelHue2, @"SpiritSpeak");  
				AddLabel(475, 317, LabelHue, pm.Skills.SpiritSpeak.Base.ToString());
				AddLabel(501, 317, LabelHue2, @"Points: ");
				AddLabel(551, 317, LabelHue, xmlplayer.SkillPointsUsedSpiritSpeak.ToString());
				
				if (c.Mysticism)
				{
					AddButton(300, 338, 4005, 4007, GetButtonID(1, 68), GumpButtonType.Reply, 0);
					AddLabel(337, 339,  LabelHue2, @"Mysticism");  
					AddLabel(475, 339, LabelHue, pm.Skills.Mysticism.Base.ToString());
					AddLabel(501, 339, LabelHue2, @"Points: ");
					AddLabel(551, 339, LabelHue, xmlplayer.SkillPointsUsedMysticism.ToString());
				}

			}
			if (m_Cat == SkillCategory.Wild)
			{
				AddButton(300, 116, 4005, 4007, GetButtonID(1, 47), GumpButtonType.Reply, 0);
				AddLabel(337, 117,  LabelHue2, @"Animal Lore");  
				AddLabel(475, 117, LabelHue, pm.Skills.AnimalLore.Base.ToString());
				AddLabel(501, 117, LabelHue2, @"Points: ");
				AddLabel(551, 117, LabelHue, xmlplayer.SkillPointsUsedAnimalLore.ToString());

				AddButton(300, 138, 4005, 4007, GetButtonID(1, 48), GumpButtonType.Reply, 0);
				AddLabel(337, 139,  LabelHue2, @"Animal Taming");  
				AddLabel(475, 139, LabelHue, pm.Skills.AnimalTaming.Base.ToString());
				AddLabel(501, 139, LabelHue2, @"Points: ");
				AddLabel(551, 139, LabelHue, xmlplayer.SkillPointsUsedAnimalTaming.ToString());

				AddButton(300, 160, 4005, 4007, GetButtonID(1, 49), GumpButtonType.Reply, 0);
				AddLabel(337, 161, LabelHue2, pm.Skills.Fishing.Name.ToString());
				AddLabel(475, 161, LabelHue, pm.Skills.Fishing.Base.ToString());
				AddLabel(501, 161, LabelHue2, @"Points: ");
				AddLabel(551, 161, LabelHue, xmlplayer.SkillPointsUsedFishing.ToString());

				AddButton(300, 182, 4005, 4007, GetButtonID(1, 50), GumpButtonType.Reply, 0);
				AddLabel(337, 183, LabelHue2, pm.Skills.Herding.Name.ToString());
				AddLabel(475, 183, LabelHue, pm.Skills.Herding.Base.ToString());
				AddLabel(501, 183, LabelHue2, @"Points: ");
				AddLabel(551, 183, LabelHue, xmlplayer.SkillPointsUsedHerding.ToString());

				AddButton(300, 204, 4005, 4007, GetButtonID(1, 51), GumpButtonType.Reply, 0);
				AddLabel(337, 205, LabelHue2, pm.Skills.Tracking.Name.ToString());
				AddLabel(475, 205, LabelHue, pm.Skills.Tracking.Base.ToString());
				AddLabel(501, 205, LabelHue2, @"Points: ");
				AddLabel(551, 205, LabelHue, xmlplayer.SkillPointsUsedTracking.ToString());

				AddButton(300, 226, 4005, 4007, GetButtonID(1, 52), GumpButtonType.Reply, 0);
				AddLabel(337, 227, LabelHue2, pm.Skills.Veterinary.Name.ToString());
				AddLabel(475, 227, LabelHue, pm.Skills.Veterinary.Base.ToString());
				AddLabel(501, 227, LabelHue2, @"Points: ");
				AddLabel(551, 227, LabelHue, xmlplayer.SkillPointsUsedVeterinary.ToString());

			}                 
			if (m_Cat == SkillCategory.Thief)
			{
				AddButton(300, 116, 4005, 4007, GetButtonID(1, 53), GumpButtonType.Reply, 0);
				AddLabel(337, 117, LabelHue2, pm.Skills.DetectHidden.Name.ToString());
				AddLabel(475, 117, LabelHue, pm.Skills.DetectHidden.Base.ToString());
				AddLabel(501, 117, LabelHue2, @"Points: ");
				AddLabel(551, 117, LabelHue, xmlplayer.SkillPointsUsedDetectHidden.ToString());

				AddButton(300, 138, 4005, 4007, GetButtonID(1, 54), GumpButtonType.Reply, 0);
				AddLabel(337, 139, LabelHue2, pm.Skills.Hiding.Name.ToString());
				AddLabel(475, 139, LabelHue, pm.Skills.Hiding.Base.ToString());
				AddLabel(501, 139, LabelHue2, @"Points: ");
				AddLabel(551, 139, LabelHue, xmlplayer.SkillPointsUsedHiding.ToString());

				AddButton(300, 160, 4005, 4007, GetButtonID(1, 55), GumpButtonType.Reply, 0);
				AddLabel(337, 161, LabelHue2, pm.Skills.Lockpicking.Name.ToString());
				AddLabel(475, 161, LabelHue, pm.Skills.Lockpicking.Base.ToString());
				AddLabel(501, 161, LabelHue2, @"Points: ");
				AddLabel(551, 161, LabelHue, xmlplayer.SkillPointsUsedLockpicking.ToString());

				AddButton(300, 182, 4005, 4007, GetButtonID(1, 56), GumpButtonType.Reply, 0);
				AddLabel(337, 183, LabelHue2, pm.Skills.Poisoning.Name.ToString());
				AddLabel(475, 183, LabelHue, pm.Skills.Poisoning.Base.ToString());
				AddLabel(501, 183, LabelHue2, @"Points: ");
				AddLabel(551, 183, LabelHue, xmlplayer.SkillPointsUsedPoisoning.ToString());

				AddButton(300, 204, 4005, 4007, GetButtonID(1, 57), GumpButtonType.Reply, 0);
				AddLabel(337, 205, LabelHue2, pm.Skills.RemoveTrap.Name.ToString());
				AddLabel(475, 205, LabelHue, pm.Skills.RemoveTrap.Base.ToString());
				AddLabel(501, 205, LabelHue2, @"Points: ");
				AddLabel(551, 205, LabelHue, xmlplayer.SkillPointsUsedRemoveTrap.ToString());

				AddButton(300, 226, 4005, 4007, GetButtonID(1, 58), GumpButtonType.Reply, 0);
				AddLabel(337, 227, LabelHue2, pm.Skills.Snooping.Name.ToString());
				AddLabel(475, 227, LabelHue, pm.Skills.Snooping.Base.ToString());
				AddLabel(501, 227, LabelHue2, @"Points: ");
				AddLabel(551, 227, LabelHue, xmlplayer.SkillPointsUsedSnooping.ToString());

				AddButton(300, 248, 4005, 4007, GetButtonID(1, 59), GumpButtonType.Reply, 0);
				AddLabel(337, 249, LabelHue2, pm.Skills.Stealing.Name.ToString());
				AddLabel(475, 249, LabelHue, pm.Skills.Stealing.Base.ToString());
				AddLabel(501, 249, LabelHue2, @"Points: ");
				AddLabel(551, 249, LabelHue, xmlplayer.SkillPointsUsedStealing.ToString());

				AddButton(300, 270, 4005, 4007, GetButtonID(1, 60), GumpButtonType.Reply, 0);
				AddLabel(337, 271, LabelHue2, pm.Skills.Stealth.Name.ToString());
				AddLabel(475, 271, LabelHue, pm.Skills.Stealth.Base.ToString());
				AddLabel(501, 271, LabelHue2, @"Points: ");
				AddLabel(551, 271, LabelHue, xmlplayer.SkillPointsUsedStealth.ToString());
			}    
			if(m_Cat == SkillCategory.Bard)
			{
				AddButton(300, 116, 4005, 4007, GetButtonID(1, 61), GumpButtonType.Reply, 0);
				AddLabel(337, 117, LabelHue2, pm.Skills.Discordance.Name.ToString());
				AddLabel(475, 117, LabelHue, pm.Skills.Discordance.Base.ToString());
				AddLabel(501, 117, LabelHue2, @"Points: ");
				AddLabel(551, 117, LabelHue, xmlplayer.SkillPointsUsedDiscordance.ToString());

				AddButton(300, 138, 4005, 4007, GetButtonID(1, 62), GumpButtonType.Reply, 0);
				AddLabel(337, 139, LabelHue2, pm.Skills.Musicianship.Name.ToString());
				AddLabel(475, 139, LabelHue, pm.Skills.Musicianship.Base.ToString());
				AddLabel(501, 139, LabelHue2, @"Points: ");
				AddLabel(551, 139, LabelHue, xmlplayer.SkillPointsUsedMusicianship.ToString());

				AddButton(300, 160, 4005, 4007, GetButtonID(1, 63), GumpButtonType.Reply, 0);
				AddLabel(337, 161, LabelHue2, pm.Skills.Peacemaking.Name.ToString());
				AddLabel(475, 161, LabelHue, pm.Skills.Peacemaking.Base.ToString());
				AddLabel(501, 161, LabelHue2, @"Points: ");
				AddLabel(551, 161, LabelHue, xmlplayer.SkillPointsUsedPeacemaking.ToString());

				AddButton(300, 182, 4005, 4007, GetButtonID(1, 64), GumpButtonType.Reply, 0);
				AddLabel(337, 183, LabelHue2, pm.Skills.Provocation.Name.ToString());
				AddLabel(475, 183, LabelHue, pm.Skills.Provocation.Base.ToString());
				AddLabel(501, 183, LabelHue2, @"Points: ");
				AddLabel(551, 183, LabelHue, xmlplayer.SkillPointsUsedProvocation.ToString());
			}
			if(m_Cat == SkillCategory.StatSheet)
			{
				IWeapon weapon = m.Weapon;

				int min = 0, max = 0;
				if (weapon != null)
					weapon.GetStatusDamage(m, out min, out max);
	
				/* section */ 
				AddLabel(301, 197, 155, @"Damage Increase: ");
				AddLabel(410, 197, hue, @"" + DamageIncrease + " %");
				
				AddLabel(301, 217, 155, @"Bandage Speed: ");
				AddLabel(410, 217, hue, String.Format("{0:0.0}s", new DateTime(TimeSpan.FromSeconds(BandageSpeed).Ticks).ToString("s.ff")));
				
				AddLabel(301, 237, 155, @"Swing Speed: ");
				AddLabel(410, 237, hue, String.Format("{0}s", new DateTime(SwingSpeed.Ticks).ToString("s.ff")));
				
				AddLabel(301, 257, 155, @"Reflect Damage: ");
				AddLabel(410, 257, hue, @"" + ReflectDamage + " %");
				
				AddLabel(301, 277, 155, @"Skill Total: ");
				AddLabel(410, 277, hue, @"" + pm.SkillsTotal + "");
		
				AddLabel(301, 297, 155, @"Player Kills: ");
				AddLabel(410, 297, hue, @"" + pm.Kills + "");
				
				AddLabel(301, 317, 155, @"Tithing Points: ");
				AddLabel(410, 317, hue, @"" + pm.TithingPoints + "");
				
				 /* section */
				AddLabel(300, 117, 155, @"LRC: ");
				AddLabel(334, 117, hue, @"" + LRC + " %");
				
				AddLabel(300, 137, 155, @"FC: ");
				AddLabel(334, 137, hue, @"" + FC + "");
				
				AddLabel(300, 157, 155, @"SDI: ");
				AddLabel(334, 157, hue, @"" + SDI + " %");
				
				AddLabel(300, 177, 155, @"HCI: ");
				AddLabel(334, 177, hue, @"" + HCI + " %");
				
				AddLabel(378, 117, 155, @"LMC: ");
				AddLabel(408, 117, hue, @"" + LMC + " %"); 
				
				AddLabel(378, 137, 155, @"FCR: ");
				AddLabel(408, 137, hue, @"" + FCR + "");
				
				AddLabel(378, 157, 155, @"SSI: ");
				AddLabel(408, 157, hue, @"" + SSI + " %");
				
				AddLabel(378, 177, 155, @"DCI: ");
				AddLabel(408, 177, hue, @"" + DCI + " %");
				
				/* section */
				AddLabel(460, 117, 155, @"Luck: ");
				AddLabel(517, 117, hue, @"" + pm.Luck + "");
				
				AddLabel(460, 137, 155, @"Fame: ");
				AddLabel(517, 137, hue, @"" + pm.Fame + "");
				
				AddLabel(460, 157, 155, @"Karma: ");
				AddLabel(517, 157, hue, @"" + pm.Karma + "");
				
				int weight = Mobile.BodyWeight + m.TotalWeight;
				AddLabel(460, 177, 155, @"Damage: ");
				AddLabel(517, 177, hue, @"" + min + " - " + max + "");
				
				AddLabel(460, 197, 155, @"Followers: ");
				AddLabel(517, 197, hue, @"" + pm.Followers + "/" + pm.FollowersMax + "");
				
				AddLabel(460, 217, 155, @"Weight: ");
				AddLabel(517, 217, hue, @"" + weight + "/" + pm.MaxWeight + "");
				/* section */
				
			}

			return;
           
        }

		public static int GetButtonID( int type, int index )
		{
			return 1 + type + (index * 7);
		}

        private bool CheckRegion(Region region)
        {
            if(region is Regions.TavernRegion)
            {
                return true;
            }


            return false;
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile m = sender.Mobile;
			XMLPlayerLevelAtt xmlplayer = (XMLPlayerLevelAtt)XmlAttach.FindAttachment(m, typeof(XMLPlayerLevelAtt));
            int AvlSkillPoints;
            int AvlStatPoints;

            PlayerMobile pm = m_From as PlayerMobile;
            AvlSkillPoints = xmlplayer.SKPoints;
            AvlStatPoints = xmlplayer.StatPoints;

			int str = (int)(xmlplayer.StrPointsUsed);
			int dex = (int)(xmlplayer.DexPointsUsed);
			int intt = (int)(xmlplayer.IntPointsUsed);
			int totalstatsreturned = (int)(xmlplayer.StrPointsUsed) + (xmlplayer.IntPointsUsed) + (xmlplayer.DexPointsUsed);
			int totalskillpoints   = (int)xmlplayer.TotalSkillPointsUsed;
            if (info.ButtonID <= 0)
                return; // Canceled

            int buttonID = info.ButtonID - 1;
            int type = buttonID % 7;
            int index = buttonID / 7;

            //UOSI
            Region region = m.Region;

            //int cost = 0;
            //int attrvalue = 0;

            switch (type)
            {
                case 0: // Cancel
                    {
                        break;
                    }
                case 1: // Select Attribute Type
                    {
                        switch (index)
                        {
                            case 0: // Misc
                                {
                                    m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Misc));
                                    break;
                                }
                            case 1: // Combat
                                {
                                   m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Combat));
                                   break;
                                }
                            case 2: // Trade
                                {

                                    m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Trade));
                                    break;
                                }
                            case 3: //Magic
                                {
                                    m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Magic));
                                    break;
                                }
                            case 4: // Wild
                                {
                                    m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Wild));
                                    break;
                                }
                            case 5: // Bard / thief
                                {
                                    m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Thief));
                                    
                                    break;
                                }
                            case 6: // Bard 
                                {
                                    m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Bard));
                                   
                                    break;
                                }
                            case 7: //str
                                {
                                    if (AvlStatPoints > 0)
                                    {
                                        m_From.SendMessage("One Stat Point has been added to your Strength");
                                        pm.RawStr += 1;
                                        AvlStatPoints -= 1;
										xmlplayer.StrPointsUsed += 1;
                                        xmlplayer.StatPoints = AvlStatPoints;
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Misc));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Misc));
                                        return;
                                    }
                                    
                                }
                            case 8: //int
                                {
                                    if (AvlStatPoints > 0)
                                    {
                                        m_From.SendMessage("One Stat Point has been added to your Intelligence");
                                        pm.RawInt += 1;
                                        AvlStatPoints -= 1;
										xmlplayer.IntPointsUsed += 1;
                                        xmlplayer.StatPoints = AvlStatPoints;
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Misc));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Misc));
                                        return;
                                    }
                                    
                                }
                            case 9: // Dex
                                {
                                    if (AvlStatPoints > 0)
                                    {
                                        m_From.SendMessage("One Stat Point has been added to your Dexterity");
                                        pm.RawDex += 1;
                                        AvlStatPoints -= 1;
										xmlplayer.DexPointsUsed += 1;
                                        xmlplayer.StatPoints = AvlStatPoints;
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Misc));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Misc));
                                        return;
                                    }
                                    
                                }
                            case 10: // ArmsLore
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if (pm.Skills.ArmsLore.Base < pm.Skills.ArmsLore.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to Arms lore");
                                            pm.Skills.ArmsLore.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedArmsLore += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Misc));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Misc));
                                           return;

                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Misc));
                                        return;
                                    }
                                    
                                }
                            case 11: // Begging
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if(pm.Skills.Begging.Base < pm.Skills.Begging.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to begging");
                                            pm.Skills.Begging.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedBegging += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Misc));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Misc));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Misc));
                                        return;
                                    }
                                    
                                    
                                }
                            case 12: // Camping
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if (pm.Skills.Camping.Base < pm.Skills.Camping.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to Camping");
                                            pm.Skills.Camping.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedCamping += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Misc));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Misc));
                                           return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Misc));
                                        return;
                                    }
                                    
                                    
                                }
                            case 13: // Cart
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if(pm.Skills.Cartography.Base < pm.Skills.Cartography.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to Cart");
                                            pm.Skills.Cartography.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedCartography += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Misc));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Misc));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Misc));
                                        return;
                                    }
                                    
                                    
                                }
                            case 14: // Forensics
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if(pm.Skills.Forensics.Base < pm.Skills.Forensics.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to Forensics");
                                            pm.Skills.Forensics.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedForensics += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Misc));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Misc));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Misc));
                                        return;
                                    }
                                   
                                }
                            case 15: // Item ID
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if (pm.Skills.ItemID.Base < pm.Skills.ItemID.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to Item ID");
                                            pm.Skills.ItemID.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedItemID += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Misc));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Misc));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Misc));
                                        return;
                                    }
                                  
                                    
                                }
                            case 16: // Taste ID
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if (pm.Skills.TasteID.Base < pm.Skills.TasteID.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to Taste ID");
                                            pm.Skills.TasteID.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedTasteID += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Misc));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Misc));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Misc));
                                        return;
                                    }
                                  
                                    
                                }
                            case 17: // Anatomy
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if (pm.Skills.Anatomy.Base < pm.Skills.Anatomy.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to Anatomy");
                                            pm.Skills.Anatomy.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedAnatomy += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Combat));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Combat));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Combat));
                                        return;
                                    }
                                   
                                    
                                }
                            case 18: // Archery
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if (pm.Skills.Archery.Base < pm.Skills.Archery.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to Archery");
                                            pm.Skills.Archery.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedArchery += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Combat));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Combat));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Combat));
                                        return;
                                    }
                                    
                                    
                                }
                            case 19: // Fencing
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if (pm.Skills.Fencing.Base < pm.Skills.Fencing.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to Fencing");
                                            pm.Skills.Fencing.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedFencing += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Combat));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Combat));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Combat));
                                        return;
                                    }
                                   
                                    
                                }
                            case 20: // Focus
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if (pm.Skills.Focus.Base < pm.Skills.Focus.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to Focus");
                                            pm.Skills.Focus.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedFocus += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Combat));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Combat));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Combat));
                                        return;
                                    }
                                    
                                    
                                }
                            case 21: //Mace
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if (pm.Skills.Macing.Base < pm.Skills.Macing.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to Macing");
                                            pm.Skills.Macing.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedMacing += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Combat));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Combat));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Combat));
                                        return;
                                    }
                                    
                                    
                                }
                            case 22: // Parry
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if (pm.Skills.Parry.Base < pm.Skills.Parry.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to Parry");
                                            pm.Skills.Parry.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedParry += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Combat));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Combat));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Combat));
                                        return;
                                    }
                                    
                                    
                                }
                            case 23: // Swords
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if (pm.Skills.Swords.Base < pm.Skills.Swords.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to Swords");
                                            pm.Skills.Swords.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedSwords += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Combat));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Combat));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Combat));
                                        return;
                                    }
                                    
                                    
                                }
                            case 24: // Tactics
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if (pm.Skills.Tactics.Base < pm.Skills.Tactics.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to Tactics");
                                            pm.Skills.Tactics.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedTactics += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Combat));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Combat));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Combat));
                                        return;
                                    }
                                   
                                    
                                }
								
                            case 25: // Throwing
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if (pm.Skills.Throwing.Base < pm.Skills.Throwing.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to Throwing");
                                            pm.Skills.Throwing.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedThrowing += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Combat));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Combat));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Combat));
                                        return;
                                    }
								}
								
                            case 26: // Wrestling
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if (pm.Skills.Wrestling.Base < pm.Skills.Wrestling.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to Wrestling");
                                            pm.Skills.Wrestling.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedWrestling += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Combat));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Combat));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Combat));
                                        return;
                                    }        
                                    
                                }
                            case 27: // Alchemy
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if (pm.Skills.Alchemy.Base < pm.Skills.Alchemy.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to Alchemy");
                                            pm.Skills.Alchemy.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedAlchemy += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Trade));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Trade));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Trade));
                                        return;
                                    }
                                    
                                    
                                }
                            case 28: // Blacksmith
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if (pm.Skills.Blacksmith.Base < pm.Skills.Blacksmith.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to Blacksmith");
                                            pm.Skills.Blacksmith.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedBlacksmith += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Trade));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Trade));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Trade));
                                        return;
                                    }
                                    
                                    
                                }
                            case 29: // Fletching
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if (pm.Skills.Fletching.Base < pm.Skills.Fletching.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to Fletching");
                                            pm.Skills.Fletching.Base += 1;
											xmlplayer.SkillPointsUsedFletching += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            AvlSkillPoints -= 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Trade));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Trade));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Trade));
                                        return;
                                    }
                                   
                                    
                                }
                            case 30: // Carpentry
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if (pm.Skills.Carpentry.Base < pm.Skills.Carpentry.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to Carpentry");
                                            pm.Skills.Carpentry.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedCarpentry += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Trade));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Trade));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Trade));
                                        return;
                                    }
                                    
                                    
                                }
                            case 31: // Cooking
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if (pm.Skills.Cooking.Base < pm.Skills.Cooking.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to Cooking");
                                            pm.Skills.Cooking.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedCooking += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Trade));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Trade));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Trade));
                                        return;
                                    }
                                    
                                    
                                }
                            case 32: // Inscirbe
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if (pm.Skills.Inscribe.Base < pm.Skills.Inscribe.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to Inscribe");
                                            pm.Skills.Inscribe.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedInscribe += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Trade));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Trade));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Trade));
                                        return;
                                    }
                                
                                    
                                }
								
								
                            case 33: // Lumberjacking
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if (pm.Skills.Lumberjacking.Base < pm.Skills.Lumberjacking.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to Lumberjacking");
                                            pm.Skills.Lumberjacking.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedLumberjacking += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Trade));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Trade));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Trade));
                                        return;
                                    }
                                    
                                    
                                }
                            case 34: // Mining
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if (pm.Skills.Mining.Base < pm.Skills.Mining.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to Mining");
                                            pm.Skills.Mining.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedMining += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Trade));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Trade));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Trade));
                                        return;
                                    }
                                  
                                    
                                }
                            case 35: // Tailoring
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if (pm.Skills.Tailoring.Base < pm.Skills.Tailoring.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to Tailoring");
                                            pm.Skills.Tailoring.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedTailoring += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Trade));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Trade));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Trade));
                                        return;
                                    }
                                   
                                    
                                }
                            case 36: // Tinkering
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if (pm.Skills.Tinkering.Base < pm.Skills.Tinkering.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to Tinkering");
                                            pm.Skills.Tinkering.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedTinkering += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Trade));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Trade));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Trade));
                                        return;
                                    }
                                    
                                    
                                }
                            case 37: // Bushido
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if (pm.Skills.Bushido.Base < pm.Skills.Bushido.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to Bushido");
                                            pm.Skills.Bushido.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedBushido += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Magic));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Magic));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Magic));
                                        return;
                                    }
                                    
                                    
                                }
                            case 38: // Chivalry
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if (pm.Skills.Chivalry.Base < pm.Skills.Chivalry.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to Chivalry");
                                            pm.Skills.Chivalry.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedChivalry += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Magic));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Magic));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Magic));
                                        return;
                                    }
                                   
                                    
                                }
                            case 39: // Eval Int
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if (pm.Skills.EvalInt.Base < pm.Skills.EvalInt.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to Eval Int");
                                            pm.Skills.EvalInt.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedEvalInt += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Magic));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Magic));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Magic));
                                        return;
                                    }
                                    
                                    
                                }
                            case 40: // Magery
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if (pm.Skills.Magery.Base < pm.Skills.Magery.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to Magery");
                                            pm.Skills.Magery.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedMagery += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Magic));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Magic));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Magic));
                                        return;
                                    }
                                    
                                    
                                }
                            case 41: // Meditation
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if (pm.Skills.Meditation.Base < pm.Skills.Meditation.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to Meditation");
                                            pm.Skills.Meditation.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedMeditation += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Magic));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Magic));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Magic));
                                        return;
                                    }
                                    
                                    
                                }
                            case 42: // Necromancy
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if (pm.Skills.Necromancy.Base < pm.Skills.Necromancy.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to Necromancy");
                                            pm.Skills.Necromancy.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedNecromancy += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Magic));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Magic));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Magic));
                                        return;
                                    }
                                    
                                    
                                }
                            case 43: // Ninjitsu
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if (pm.Skills.Ninjitsu.Base < pm.Skills.Ninjitsu.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to Ninjitsu");
                                            pm.Skills.Ninjitsu.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedNinjitsu += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Magic));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Magic));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Magic));
                                        return;
                                    }
                                   
                                    
                                }
                            case 44: // Magic Resist
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if (pm.Skills.MagicResist.Base < pm.Skills.MagicResist.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to Magic Resist");
                                            pm.Skills.MagicResist.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedMagicResist += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Magic));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Magic));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Magic));
                                        return;
                                    }
                                    
                                    
                                }
                            case 45: // Spellweaving
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if (pm.Skills.Spellweaving.Base < pm.Skills.Spellweaving.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to Spellweaving");
                                            pm.Skills.Spellweaving.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedSpellweaving += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Magic));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Magic));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Magic));
                                        return;
                                    }
                                   
                                    
                                }
                            case 46: // Spirit Speak
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if (pm.Skills.SpiritSpeak.Base < pm.Skills.SpiritSpeak.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to Spirit Speak");
                                            pm.Skills.SpiritSpeak.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedSpiritSpeak += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Magic));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Magic));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Magic));
                                        return;
                                    }
                              
                                    
                                }
                            case 47: // Animal Lore
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if (pm.Skills.AnimalLore.Base < pm.Skills.AnimalLore.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to Animal Lore");
                                            pm.Skills.AnimalLore.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedAnimalLore += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Wild));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Wild));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Wild));
                                        return;
                                    }
                                   
                                    
                                }
                            case 48: // Taming
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if (pm.Skills.AnimalTaming.Base < pm.Skills.AnimalTaming.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to Animal Taming");
                                            pm.Skills.AnimalTaming.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedAnimalTaming += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Wild));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Wild));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Wild));
                                        return;
                                    }
                                   
                                    
                                }
                            case 49: // Fishing
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if (pm.Skills.Fishing.Base < pm.Skills.Fishing.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to Fishing");
                                            pm.Skills.Fishing.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedFishing += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Wild));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Wild));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Wild));
                                        return;
                                    }
                                    
                                    
                                }
                            case 50: // Herding 
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if (pm.Skills.Herding.Base < pm.Skills.Herding.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to Herding");
                                            pm.Skills.Herding.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedHerding += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Wild));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Wild));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Wild));
                                        return;
                                    }
                                  
                                    
                                }
                            case 51: // Tracking
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if (pm.Skills.Tracking.Base < pm.Skills.Tracking.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to Tracking");
                                            pm.Skills.Tracking.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedTracking += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Wild));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Wild));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Wild));
                                        return;
                                    }
                                    
                                    
                                }
                            case 52: // Vet
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if (pm.Skills.Veterinary.Base < pm.Skills.Veterinary.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to Vetrinary");
                                            pm.Skills.Veterinary.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedVeterinary += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Wild));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Wild));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Wild));
                                        return;
                                    }
                                   
                                    
                                }
                            case 53: // Detect Hidden
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if (pm.Skills.DetectHidden.Base < pm.Skills.DetectHidden.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to Detect_Hidden");
                                            pm.Skills.DetectHidden.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedDetectHidden += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Thief));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Thief));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Thief));
                                        return;
                                    }
                                   
                                    
                                }
                            case 54: // Hiding
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if (pm.Skills.Hiding.Base < pm.Skills.Hiding.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to hiding");
                                            pm.Skills.Hiding.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedHiding += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Thief));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Thief));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Thief));
                                        return;
                                    }
                                    
                                    
                                }
                            case 55: // Lock Picking
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if (pm.Skills.Lockpicking.Base < pm.Skills.Lockpicking.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to Lock Picking");
                                            pm.Skills.Lockpicking.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedLockpicking += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Thief));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Thief));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Thief));
                                        return;
                                    }
                                   
                                    
                                }
                            case 56: // Poisoning
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if (pm.Skills.Poisoning.Base < pm.Skills.Poisoning.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to Poisoning");
                                            pm.Skills.Poisoning.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedPoisoning += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Thief));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Thief));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Thief));
                                        return;
                                    }
                                   
                                    
                                }
                            case 57: // Remove Trap
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if (pm.Skills.RemoveTrap.Base < pm.Skills.RemoveTrap.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to Remove Traps");
                                            pm.Skills.RemoveTrap.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedRemoveTrap += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Thief));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Thief));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Thief));
                                        return;
                                    }
                                    
                                    
                                }
                            case 58: // Snooping
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if (pm.Skills.Snooping.Base < pm.Skills.Snooping.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to Snooping");
                                            pm.Skills.Snooping.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedSnooping += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Thief));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Thief));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Thief));
                                        return;
                                    }
                                    
                                    
                                }
                            case 59: // Stealing
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if (pm.Skills.Stealing.Base < pm.Skills.Stealing.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to Stealing");
                                            pm.Skills.Stealing.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedStealing += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Thief));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Thief));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Thief));
                                        return;
                                    }
                                    
                                    
                                }
                            case 60: // Stealth
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if (pm.Skills.Stealth.Base < pm.Skills.Stealth.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to Stealth");
                                            pm.Skills.Stealth.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedStealth += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Thief));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Thief));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Thief));
                                        return;
                                    }
                                   
                                    
                                }
                            case 61: // Discord
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if (pm.Skills.Discordance.Base < pm.Skills.Discordance.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to Discordance");
                                            pm.Skills.Discordance.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedDiscordance += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Bard));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Bard));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Bard));
                                        return;
                                    }
                                   
                                    
                                }
                            case 62: // Musicianship
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if (pm.Skills.Musicianship.Base < pm.Skills.Musicianship.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to Musicianship");
                                            pm.Skills.Musicianship.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedMusicianship += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Bard));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Bard));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Bard));
                                        return;
                                    }
                                    
                                    
                                }
                            case 63: // Peacemaking
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if (pm.Skills.Peacemaking.Base < pm.Skills.Peacemaking.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to Peacemaking");
                                            pm.Skills.Peacemaking.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedPeacemaking += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Bard));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Bard));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Bard));
                                        return;
                                    }
                                   
                                    
                                }
                            case 64: // Provocation
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if (pm.Skills.Provocation.Base < pm.Skills.Provocation.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to Provocation");
                                            pm.Skills.Provocation.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedProvocation += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Bard));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Bard));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Bard));
                                        return;
                                    }
                                   
                                    
                                }
                            case 65: // Stats
                                {
                                    m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Misc));
                                    break;
                                }
                            case 66: //Healing
							{
                                if (AvlSkillPoints > 0)
                                {
                                    if (pm.Skills.Healing.Base < pm.Skills.Healing.Cap)
                                    {
                                        m_From.SendMessage("One Skill point has been added to Healing");
                                        pm.Skills.Healing.Base += 1;
                                        AvlSkillPoints -= 1;
										xmlplayer.SkillPointsUsedHealing += 1;
										xmlplayer.TotalSkillPointsUsed += 1;
                                        xmlplayer.SKPoints = AvlSkillPoints;
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Combat));
                                        return;
                                    }
                                    pm.SendMessage("You have reached the cap in this skill");
                                    pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Combat));
                                    return;
                                }
                                else
                                {
                                    m_From.SendMessage("You do not have any available skill points left");
                                    m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Combat));
                                    return;
                                }
                                
                           

							}
							
						 //Comment out if your server does not support SA
							    case 67: // Imbuing
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if (pm.Skills.Imbuing.Base < pm.Skills.Imbuing.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to Imbuing");
                                            pm.Skills.Imbuing.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedImbuing += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Trade));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Trade));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Trade));
                                        return;
                                    }
                                   
                                    
                                }
								
								//Comment out if your server does not support SA
							    case 68: // Mysticism
                                {
                                    if (AvlSkillPoints > 0)
                                    {
                                        if (pm.Skills.Mysticism.Base < pm.Skills.Mysticism.Cap)
                                        {
                                            m_From.SendMessage("One Skill point has been added to Mysticism");
                                            pm.Skills.Mysticism.Base += 1;
                                            AvlSkillPoints -= 1;
											xmlplayer.SkillPointsUsedMysticism += 1;
											xmlplayer.TotalSkillPointsUsed += 1;
                                            xmlplayer.SKPoints = AvlSkillPoints;
                                            m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Magic));
                                            return;
                                        }
                                        pm.SendMessage("You have reached the cap in this skill");
                                        pm.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.Magic));
                                        return;
                                    }
                                    else
                                    {
                                        m_From.SendMessage("You do not have any available skill points left");
                                        m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.None, SkillCategory.Magic));
                                        return;
                                    }
                                   
                                    
                                }
							    case 69: // StatSheetGumpCall
                                {
									m_From.SendGump(new PlayerLevelGump(m_From, GumpPage.SkillList, SkillCategory.StatSheet));
									break;
								}
							    case 70: // Master Stat Reset
                                {
									if (totalstatsreturned == 0)  /* Max Level per System */
									{
										pm.SendMessage("You have no stats to reset!");
										m_From.SendGump( new PlayerLevelGump( m_From, GumpPage.None, SkillCategory.Misc ) );
										return;
									}
									else
									{	
										if (xmlplayer.StrPointsUsed > 0)
										{
											m_From.Str += -str;
											xmlplayer.StatPoints += str;
											xmlplayer.StrPointsUsed = 0;
										}
										if (xmlplayer.DexPointsUsed > 0)
										{
											m_From.Dex += -dex;
											xmlplayer.StatPoints += dex;
											xmlplayer.DexPointsUsed = 0;
										}
										if (xmlplayer.IntPointsUsed > 0)
										{
											m_From.Int += -intt;
											xmlplayer.StatPoints += intt;
											xmlplayer.IntPointsUsed = 0;
										}
										m_From.SendGump( new PlayerLevelGump( m_From, GumpPage.None, SkillCategory.Misc ) );
										pm.SendMessage("Any used stat points have been returned!");
									}
									break;
								}
								case 71: // Master Skill Reset
								{
									if (totalskillpoints == 0)
									{
										pm.SendMessage("You have no skill points to refund!");
										return;
									}
									if (m.Skills[SkillName.Alchemy].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedAlchemy;
										m.Skills[SkillName.Alchemy].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedAlchemy -= skillpoint;	
									}
									if (m.Skills[SkillName.Anatomy].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedAnatomy;
										m.Skills[SkillName.Anatomy].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedAnatomy -= skillpoint;
									}
									if (m.Skills[SkillName.AnimalLore].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedAnimalLore;
										m.Skills[SkillName.AnimalLore].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedAnimalLore -= skillpoint;
									}
									if (m.Skills[SkillName.AnimalTaming].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedAnimalTaming;
										m.Skills[SkillName.AnimalTaming].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedAnimalTaming -= skillpoint;
									}
									if (m.Skills[SkillName.Archery].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedArchery;
										m.Skills[SkillName.Archery].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedArchery -= skillpoint;
									}
									if (m.Skills[SkillName.ArmsLore].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedArmsLore;
										m.Skills[SkillName.ArmsLore].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedArmsLore -= skillpoint;
									}
									if (m.Skills[SkillName.Begging].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedBegging;
										m.Skills[SkillName.Begging].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedBegging -= skillpoint;
									}	

									if (m.Skills[SkillName.Blacksmith].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedBlacksmith;
										m.Skills[SkillName.Blacksmith].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedBlacksmith -= skillpoint;
									}
									if (m.Skills[SkillName.Camping].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedCamping;
										m.Skills[SkillName.Camping].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedCamping -= skillpoint;
									}
									if (m.Skills[SkillName.Carpentry].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedCarpentry;
										m.Skills[SkillName.Carpentry].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedCarpentry -= skillpoint;
									}
									if (m.Skills[SkillName.Cooking].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedCooking;
										m.Skills[SkillName.Cooking].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedCooking -= skillpoint;
									}
									if (m.Skills[SkillName.Fishing].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedFishing;
										m.Skills[SkillName.Fishing].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedFishing -= skillpoint;
									}
									if (m.Skills[SkillName.Healing].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedHealing;
										m.Skills[SkillName.Healing].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedHealing -= skillpoint;
									}
									if (m.Skills[SkillName.Herding].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedHerding;
										m.Skills[SkillName.Herding].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedHerding -= skillpoint;
									}
									if (m.Skills[SkillName.Lockpicking].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedLockpicking;
										m.Skills[SkillName.Lockpicking].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedLockpicking -= skillpoint;
									}
									if (m.Skills[SkillName.Lumberjacking].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedLumberjacking;
										m.Skills[SkillName.Lumberjacking].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedLumberjacking -= skillpoint;
									}
									if (m.Skills[SkillName.Magery].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedMagery;
										m.Skills[SkillName.Magery].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedMagery -= skillpoint;
									}
									if (m.Skills[SkillName.Meditation].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedMeditation;
										m.Skills[SkillName.Meditation].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedMeditation -= skillpoint;
									}
									if (m.Skills[SkillName.Mining].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedMining;
										m.Skills[SkillName.Mining].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedMining -= skillpoint;
									}
									if (m.Skills[SkillName.Musicianship].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedMusicianship;
										m.Skills[SkillName.Musicianship].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedMusicianship -= skillpoint;
									}
									if (m.Skills[SkillName.RemoveTrap].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedRemoveTrap;
										m.Skills[SkillName.RemoveTrap].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedRemoveTrap -= skillpoint;
									}
									if (m.Skills[SkillName.MagicResist].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedMagicResist;
										m.Skills[SkillName.MagicResist].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedMagicResist -= skillpoint;
									}
									if (m.Skills[SkillName.Snooping].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedSnooping;
										m.Skills[SkillName.Snooping].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedSnooping -= skillpoint;
									}
									if (m.Skills[SkillName.Stealing].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedStealing;
										m.Skills[SkillName.Stealing].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedStealing -= skillpoint;
									}
									if (m.Skills[SkillName.Stealth].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedStealth;
										m.Skills[SkillName.Stealth].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedStealth -= skillpoint;
									}
									if (m.Skills[SkillName.Tailoring].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedTailoring;
										m.Skills[SkillName.Tailoring].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedTailoring -= skillpoint;
									}
									if (m.Skills[SkillName.Tinkering].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedTinkering;
										m.Skills[SkillName.Tinkering].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedTinkering -= skillpoint;
									}
									if (m.Skills[SkillName.Veterinary].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedVeterinary;
										m.Skills[SkillName.Veterinary].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedVeterinary -= skillpoint;
									}
									if (m.Skills[SkillName.Fencing].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedFencing;
										m.Skills[SkillName.Fencing].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedFencing -= skillpoint;
									}
									if (m.Skills[SkillName.Macing].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedMacing;
										m.Skills[SkillName.Macing].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedMacing -= skillpoint;
									}
									if (m.Skills[SkillName.Parry].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedParry;
										m.Skills[SkillName.Parry].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedParry -= skillpoint;
									}
									if (m.Skills[SkillName.Swords].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedSwords;
										m.Skills[SkillName.Swords].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedSwords -= skillpoint;
									}
									if (m.Skills[SkillName.Tactics].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedTactics;
										m.Skills[SkillName.Tactics].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedTactics -= skillpoint;
									}
									if (m.Skills[SkillName.Wrestling].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedWrestling;
										m.Skills[SkillName.Wrestling].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedWrestling -= skillpoint;
									}
									if (m.Skills[SkillName.Cartography].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedCartography;
										m.Skills[SkillName.Cartography].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedCartography -= skillpoint;
									}
									if (m.Skills[SkillName.DetectHidden].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedDetectHidden;
										m.Skills[SkillName.DetectHidden].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedDetectHidden -= skillpoint;
									}
									if (m.Skills[SkillName.Inscribe].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedInscribe;
										m.Skills[SkillName.Inscribe].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedInscribe -= skillpoint;
									}
									if (m.Skills[SkillName.Peacemaking].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedPeacemaking;
										m.Skills[SkillName.Peacemaking].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedPeacemaking -= skillpoint;
									}
									if (m.Skills[SkillName.Poisoning].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedPoisoning;
										m.Skills[SkillName.Poisoning].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedPoisoning -= skillpoint;
									}
									if (m.Skills[SkillName.Provocation].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedProvocation;
										m.Skills[SkillName.Provocation].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedProvocation -= skillpoint;
									}
									if (m.Skills[SkillName.SpiritSpeak].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedSpiritSpeak;
										m.Skills[SkillName.SpiritSpeak].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedSpiritSpeak -= skillpoint;
									}
									if (m.Skills[SkillName.Tracking].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedTracking;
										m.Skills[SkillName.Tracking].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedTracking -= skillpoint;
									}
									if (m.Skills[SkillName.EvalInt].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedEvalInt;
										m.Skills[SkillName.EvalInt].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedEvalInt -= skillpoint;
									}
									if (m.Skills[SkillName.Forensics].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedForensics;
										m.Skills[SkillName.Forensics].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedForensics -= skillpoint;
									}
									if (m.Skills[SkillName.ItemID].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedItemID;
										m.Skills[SkillName.ItemID].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedItemID -= skillpoint;
									}
									if (m.Skills[SkillName.TasteID].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedTasteID;
										m.Skills[SkillName.TasteID].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedTasteID -= skillpoint;
									}
									if (m.Skills[SkillName.Hiding].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedHiding;
										m.Skills[SkillName.Hiding].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedHiding -= skillpoint;
									}
									if (m.Skills[SkillName.Fletching].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedFletching;
										m.Skills[SkillName.Fletching].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedFletching -= skillpoint;
									}
									if (m.Skills[SkillName.Focus].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedFocus;
										m.Skills[SkillName.Focus].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedFocus -= skillpoint;
									}
									if (m.Skills[SkillName.Throwing].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedThrowing;
										m.Skills[SkillName.Throwing].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedThrowing -= skillpoint;
									}
									if (m.Skills[SkillName.Bushido].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedBushido;
										m.Skills[SkillName.Bushido].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedBushido -= skillpoint;
									}
									if (m.Skills[SkillName.Chivalry].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedChivalry;
										m.Skills[SkillName.Chivalry].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedChivalry -= skillpoint;
									}
									if (m.Skills[SkillName.Imbuing].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedImbuing;
										m.Skills[SkillName.Imbuing].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedImbuing -= skillpoint;
									}
									if (m.Skills[SkillName.Mysticism].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedMysticism;
										m.Skills[SkillName.Mysticism].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedMysticism -= skillpoint;
									}
									if (m.Skills[SkillName.Necromancy].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedNecromancy;
										m.Skills[SkillName.Necromancy].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedNecromancy -= skillpoint;
									}
									if (m.Skills[SkillName.Ninjitsu].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedNinjitsu;
										m.Skills[SkillName.Ninjitsu].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedNinjitsu -= skillpoint;
									}
									if (m.Skills[SkillName.Spellweaving].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedSpellweaving;
										m.Skills[SkillName.Spellweaving].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedSpellweaving -= skillpoint;
									}
									if (m.Skills[SkillName.Discordance].Base != 0)
									{
										int skillpoint = xmlplayer.SkillPointsUsedDiscordance;
										m.Skills[SkillName.Discordance].Base -= skillpoint;
										xmlplayer.TotalSkillPointsUsed -= skillpoint;
										xmlplayer.SKPoints += skillpoint;
										xmlplayer.SkillPointsUsedDiscordance -= skillpoint;
									}

									m.SendMessage("All skill points have been returned to your skill pool.");
									m_From.SendGump( new PlayerLevelGump( m_From, GumpPage.None, SkillCategory.Misc ) );
									break;
								}
								
						}		
								//End comment
                        break;
                    }
            }
        }
    
    
    
    }
   
}
