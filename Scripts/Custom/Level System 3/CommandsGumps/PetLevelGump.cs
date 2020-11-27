/*
Special thanks to Lokai from the Servuo.com forums for the help with this script.  
His knowledge and experience is always appreciated when he offers it.  Without
him this script would not have been possible with my current knowledge. 
*/
using System;
using Server;
using Server.Network;
using Server.Items;
using Server.Mobiles;
using Server.Commands;
using Server.Targeting;
using Server.Regions;
using Server.Gumps;
using Server.Engines.XmlSpawner2;

namespace Server.Gumps
{
	public class PetCommands
	{
		public static void Initialize()
		{
			CommandHandlers.Register( "petlevel", AccessLevel.Player, new CommandEventHandler( PetInfo_OnCommand ) );
		}

		[Usage( "petlevel" )]
		[Description( "Pets Level." )]
		private static void PetInfo_OnCommand( CommandEventArgs e )
		{
			PlayerMobile from = e.Mobile as PlayerMobile;

			if ( null != from )
				from.Target = new PetTarget( from, true );
		}
	}

	public class PetTarget : Target
	{
		private bool m_StaffCommand;

		public PetTarget( Mobile from, bool staffCommand ) : base( 10, false, TargetFlags.None )
		{
			m_StaffCommand = staffCommand;
			from.SendMessage( "Target the pet you want to check." );
		}

		protected override void OnTarget( Mobile from, object target )
		{
			BaseCreature pet = target as BaseCreature;
			XMLPetLevelAtt petxml = (XMLPetLevelAtt)XmlAttach.FindAttachment(pet, typeof(XMLPetLevelAtt));

			if (petxml == null)
				from.SendMessage( "You cannot check this!" );
			
			else if ( target == from )
				from.SendMessage( "You cannot check yourself!" );

			else if ( target is Item )
				from.SendMessage( "You cannot check that!" );

			else if ( target is PlayerMobile )
				from.SendMessage( "That person gives you a dirty look!" );

			else if ( Server.Spells.SpellHelper.CheckCombat( from ) )
				from.SendMessage( "You cannot check your pet while you are fighting." );

			else if ( null == pet )
				from.SendMessage( "That is not a pet!" );

			else if ( ( pet.BodyValue == 400 || pet.BodyValue == 401 ) && pet.Controlled == false )
				from.SendMessage( "That person gives you a dirty look!" );

			else if ( pet.IsDeadPet )
				from.SendMessage( "You cannot check the dead!" );

			else if ( pet.Summoned )
				from.SendMessage( "You cannot check a summoned creature!" );

			else if ( !m_StaffCommand && pet.Combatant != null && pet.InRange( pet.Combatant, 12 ) && pet.Map == pet.Combatant.Map )
				from.SendMessage( "Your pet is fighting; you cannot check it yet." );

			else if ( pet.BodyMod != 0 )
				from.SendMessage( "You cannot check your pet while it is polymorphed." );

			else if ( !m_StaffCommand && pet.Controlled == false )
				from.SendMessage( "You cannot not check wild creatures." );

			else if ( !m_StaffCommand && pet.ControlMaster != from )
				from.SendMessage( "That is not your pet." );


			else
			{
				from.SendGump( new PetLevelGump(from, pet ));
			}
		}
	}

	public class PetLevelGump : Gump
	{
		private BaseCreature m_Pet;
		XMLPetLevelAtt m_Petxml;
		XMLPetAttacksBonus m_PetAttack;
		private Mobile m_Master;
        private const int LabelHue = 0x480;
        private const int TitleHue = 0x12B;
		private const int LabelHue2 = 155;
		
		
		public PetLevelGump ( Mobile from, BaseCreature target ) : base( 40, 40 )
		{					
			m_Pet = target;
			ConfiguredPetXML cp = new ConfiguredPetXML();
			m_Petxml = (XMLPetLevelAtt)XmlAttach.FindAttachment(m_Pet, typeof(XMLPetLevelAtt));
			m_PetAttack = (XMLPetAttacksBonus)XmlAttach.FindAttachment(m_Pet, typeof(XMLPetAttacksBonus));

			int hue		 = 1149;
			
			
			Closable=true;
			Disposable=true;
			Dragable=true;
			Resizable=false;

			AddPage(0);
			AddBackground(50, 35, 540, 382, 9270);

			int totalBaseStats;
			
			//custom information
			AddLabel( 100, 69, 1153, "You can go anywhere you want in this world with a single blade."        );						
			AddLabel(262, 56, TitleHue, @" Pet Level Window");

			AddLabel(112, 93, TitleHue, @"Boosted Skills");
			AddLabel(220, 93, TitleHue, @"Required Level");
			AddImage(0, 4, 10440);
			AddImage(554, 4, 10441);

			totalBaseStats = m_Pet.RawStr + m_Pet.RawInt + m_Pet.RawDex;

			AddLabel(300, 380, LabelHue2, @"Stat Total: ");
			AddLabel(370, 380, LabelHue, totalBaseStats.ToString());
//22
			int totalSkill = m_Pet.SkillsTotal;
			totalSkill = totalSkill / 10;

            AddLabel(75, 336, LabelHue2, @"Current Level:");
            AddLabel(164, 336, LabelHue, m_Petxml.Levell.ToString());

            AddLabel(75, 358, LabelHue2, @"Experience:");
            AddLabel(144, 358, LabelHue, m_Petxml.Expp.ToString());

            AddLabel(75, 380, LabelHue2, @"Exp. to next level:");
            AddLabel(191, 380, LabelHue, m_Petxml.ToLevell.ToString());


			//stats
            AddLabel(394, 93, TitleHue, @"Stats");

            AddButton(300, 116, 4005, 4007, GetButtonID(1, 2), GumpButtonType.Reply, 0);
            AddLabel(337, 117, LabelHue2, @"Strength:");
            AddLabel(407, 117, LabelHue, m_Pet.RawStr.ToString());

            AddButton(300, 138, 4005, 4007, GetButtonID(1, 3), GumpButtonType.Reply, 0);
            AddLabel(337, 139, LabelHue2, @"Intelligence:");
            AddLabel(407, 139, LabelHue, m_Pet.RawInt.ToString());

            AddButton(300, 160, 4005, 4007, GetButtonID(1, 4), GumpButtonType.Reply, 0);
            AddLabel(337, 161, LabelHue2, @"Dexterity:");
            AddLabel(407, 161, LabelHue, m_Pet.RawDex.ToString());

            AddLabel(300, 183, LabelHue2, @"Available stat points: ");
            AddLabel(435, 183, LabelHue, m_Petxml.StatPoints.ToString());
			///
			
			AddLabel(300, 205, 155, @"Hit Points:");
            AddLabel(435, 205, hue, @"" + m_Pet.Hits );
			AddLabel(465, 205, hue, @"/ " + m_Pet.HitsMax);
            AddLabel(300, 227, 155, @"Mana:");
            AddLabel(435, 227, hue, @"" + m_Pet.Mana );
            AddLabel(465, 227, hue, @"/ " + m_Pet.ManaMax );
            AddLabel(300, 249, 155, @"Stamina:");
            AddLabel(435, 249, hue, @"" + m_Pet.Stam );
            AddLabel(465, 249, hue, @"/ " + m_Pet.StamMax );
			
			if (m_PetAttack != null)
			{
				if (cp.TelePortToTarget)
				{
					AddLabel(112, 117, LabelHue2, @"Teleport To Target");
					AddLabel(230, 117, hue, @": " + cp.TelePortToTargetReq );
					if (m_Petxml.Levell < cp.TelePortToTargetReq)
					{
						this.AddImage(75, 116, 5231);
					}
					else
					{	
						if (m_PetAttack.m_TeleToTargetSwitch == true)
							AddButton(75, 116, 4006, 4005, GetButtonID( 1, 5 ), GumpButtonType.Reply, 0);
						if (m_PetAttack.m_TeleToTargetSwitch == false)
							AddButton(75, 116, 4005, 4006, GetButtonID( 1, 5 ), GumpButtonType.Reply, 0);
					}

				}
				else
				{
					AddLabel(112, 117, LabelHue2, @"Disabled by Admin");
				}
				if (cp.MassProvokeToAtt)
				{
					AddLabel(112, 139, LabelHue2, @"Mass Provoke");
					AddLabel(230, 139, hue, @": " + cp.MassProvokeToAttReq );
					if (m_Petxml.Levell < cp.MassProvokeToAttReq)
					{
						this.AddImage(75, 138, 5231);
						m_PetAttack.m_MassProvokeSwitch = false;
					}
					else
					{	
						if (m_PetAttack.m_MassProvokeSwitch == true)
							AddButton(75, 138, 4006, 4005, GetButtonID( 1, 6 ), GumpButtonType.Reply, 0);
						if (m_PetAttack.m_MassProvokeSwitch == false)
							AddButton(75, 138, 4005, 4006, GetButtonID( 1, 6 ), GumpButtonType.Reply, 0);
					}
				}
				else
				{
					AddLabel(112, 139, LabelHue2, @"Disabled by Admin");
				}
				if (cp.MassPeaceArea)
				{
					AddLabel(112, 161, LabelHue2, @"Mass Peace Area");
					AddLabel(230, 161, hue, @": " + cp.MassPeaceReq );
					if (m_Petxml.Levell < cp.MassPeaceReq)
					{
						this.AddImage(75, 160, 5231);
						m_PetAttack.m_MassPeace = false;
					}
					else
					{	
						if (m_PetAttack.m_MassPeace == true)
							AddButton(75, 160, 4006, 4005, GetButtonID( 1, 7 ), GumpButtonType.Reply, 0);
						if (m_PetAttack.m_MassPeace == false)
							AddButton(75, 160, 4005, 4006, GetButtonID( 1, 7 ), GumpButtonType.Reply, 0);
					}
				}
				else
				{
					AddLabel(112, 161, LabelHue2, @"Disabled by Admin");
				}
				if (cp.BlessedPower)
				{
					AddLabel(112, 183, LabelHue2, @"Blessed Power");
					AddLabel(230, 183, hue, @": " + cp.BlessedPowerReq );
					if (m_Petxml.Levell < cp.BlessedPowerReq)
					{
						this.AddImage(75, 182, 5231);
						m_PetAttack.m_BlessedPower = false;
					}
					else
					{	
						if (m_PetAttack.m_BlessedPower == true)
							AddButton(75, 182, 4006, 4005, GetButtonID( 1, 8 ), GumpButtonType.Reply, 0);
						if (m_PetAttack.m_BlessedPower == false)
							AddButton(75, 182, 4005, 4006, GetButtonID( 1, 8 ), GumpButtonType.Reply, 0);
					}
					
				}
				else
				{
					AddLabel(112, 183, LabelHue2, @"Disabled by Admin");
				}
				if (cp.AreaFireBlast)
				{
					AddLabel(112, 205, LabelHue2, @"Area Fire Blast");
					AddLabel(230, 205, hue, @": " + cp.AreaFireBlastReq );
					if (m_Petxml.Levell < cp.AreaFireBlastReq)
					{
						this.AddImage(75, 204, 5231);
						m_PetAttack.m_AreaFireBlast = false;
					}
					else
					{	
						if (m_PetAttack.m_AreaFireBlast == true)
							AddButton(75, 204, 4006, 4005, GetButtonID( 1, 9 ), GumpButtonType.Reply, 0);
						if (m_PetAttack.m_AreaFireBlast == false)
							AddButton(75, 204, 4005, 4006, GetButtonID( 1, 9 ), GumpButtonType.Reply, 0);
					}
				}
				else
				{
					AddLabel(112, 205, LabelHue2, @"Disabled by Admin");
				}
				if (cp.AreaIceBlast)
				{
					AddLabel(112, 227, LabelHue2, @"Area Ice Blast");
					AddLabel(230, 227, hue, @": " + cp.AreaIceBlastReq );
					if (m_Petxml.Levell < cp.AreaIceBlastReq)
					{
						this.AddImage(75, 226, 5231);
						m_PetAttack.m_AreaIceBlast = false;
					}
					else
					{	
						if (m_PetAttack.m_AreaIceBlast == true)
							AddButton(75, 226, 4006, 4005, GetButtonID( 1, 10 ), GumpButtonType.Reply, 0);
						if (m_PetAttack.m_AreaIceBlast == false)
							AddButton(75, 226, 4005, 4006, GetButtonID( 1, 10 ), GumpButtonType.Reply, 0);
					}

				}
				else
				{
					AddLabel(112, 227, LabelHue2, @"Disabled by Admin");
				}
				if (cp.AreaAirBlast)
				{
					AddLabel(112, 249, LabelHue2, @"Area Air Blast");
					AddLabel(230, 249, hue, @": " + cp.AreaAirBlastReq );
					if (m_Petxml.Levell < cp.AreaAirBlastReq)
					{
						this.AddImage(75, 248, 5231);
						m_PetAttack.m_AreaAirBlast = false;
					}
					else
					{	
						if (m_PetAttack.m_AreaAirBlast == true)
							AddButton(75, 248, 4006, 4005, GetButtonID( 1, 11 ), GumpButtonType.Reply, 0);
						if (m_PetAttack.m_AreaAirBlast == false)
							AddButton(75, 248, 4005, 4006, GetButtonID( 1, 11 ), GumpButtonType.Reply, 0);
					}

				}
				else
				{
					AddLabel(112, 249, LabelHue2, @"Disabled by Admin");
				}
				if (cp.SuperHeal)
				{
					if (m_Petxml.Levell < cp.SuperHealReq)
						AddLabel(75, 271, LabelHue2, @"Disabled :Super Healing");
					else
					{
						AddLabel(75, 271, LabelHue2, @"Enabled :Super Healing");
					}
					AddLabel(230, 271, hue, @": " + cp.SuperHealReq );
				}
				else
				{
					AddLabel(112, 271, LabelHue2, @"Disabled by Admin");
				}

				if (cp.AuraStatBoost)
				{
					if (m_Petxml.Levell < cp.AuraStatBoostReq)
						AddLabel(75, 293, LabelHue2, @"Disabled :Aura Stat Boost");
					else
					{
						AddLabel(75, 293, LabelHue2, @"Enabled :Aura Stat Boost");
					}
					AddLabel(230, 293, hue, @": " + cp.AuraStatBoostReq );
				}
				else
				{
					AddLabel(112, 293, LabelHue2, @"Disabled by Admin");
				}
			}
			else
			{
				AddLabel(112, 117, LabelHue2, @"Pet Bonus System Disabled.");
			}
			if (cp.EnableLvLMountChkonPetAtt)
			{
				if (m_Pet is Beetle)
				{
					AddLabel(75, 315, LabelHue2, @"Level Req to Mount Pet");
					AddLabel(230, 315, hue, @": " + cp.Beetle );
				}
				if (m_Pet is DesertOstard)
				{
					AddLabel(75, 315, LabelHue2, @"Level Req to Mount Pet");
					AddLabel(230, 315, hue, @": " + cp.DesertOstard );
				}
				if (m_Pet is FireSteed)
				{
					AddLabel(75, 315, LabelHue2, @"Level Req to Mount Pet");
					AddLabel(230, 315, hue, @": " + cp.FireSteed );
				}
				if (m_Pet is ForestOstard)
				{
					AddLabel(75, 315, LabelHue2, @"Level Req to Mount Pet");
					AddLabel(230, 315, hue, @": " + cp.ForestOstard );
				}
				if (m_Pet is FrenziedOstard)
				{
					AddLabel(75, 315, LabelHue2, @"Level Req to Mount Pet");
					AddLabel(230, 315, hue, @": " + cp.FrenziedOstard );
				}
				if (m_Pet is HellSteed)
				{
					AddLabel(75, 315, LabelHue2, @"Level Req to Mount Pet");
					AddLabel(230, 315, hue, @": " + cp.HellSteed );
				}
				if (m_Pet is Hiryu)
				{
					AddLabel(75, 315, LabelHue2, @"Level Req to Mount Pet");
					AddLabel(230, 315, hue, @": " + cp.Hiryu );
				}
				if (m_Pet is Horse)
				{
					AddLabel(75, 315, LabelHue2, @"Level Req to Mount Pet");
					AddLabel(230, 315, hue, @": " + cp.Horse );
				}
				if (m_Pet is Kirin)
				{
					AddLabel(75, 315, LabelHue2, @"Level Req to Mount Pet");
					AddLabel(230, 315, hue, @": " + cp.Kirin );
				}
				if (m_Pet is LesserHiryu)
				{
					AddLabel(75, 315, LabelHue2, @"Level Req to Mount Pet");
					AddLabel(230, 315, hue, @": " + cp.LesserHiryu );
				}
				if (m_Pet is Nightmare)
				{
					AddLabel(75, 315, LabelHue2, @"Level Req to Mount Pet");
					AddLabel(230, 315, hue, @": " + cp.Nightmare );
				}
				if (m_Pet is RidableLlama)
				{
					AddLabel(75, 315, LabelHue2, @"Level Req to Mount Pet");
					AddLabel(230, 315, hue, @": " + cp.RidableLlama );
				}
				if (m_Pet is Ridgeback)
				{
					AddLabel(75, 315, LabelHue2, @"Level Req to Mount Pet");
					AddLabel(230, 315, hue, @": " + cp.Ridgeback );
				}
				if (m_Pet is SavageRidgeback)
				{
					AddLabel(75, 315, LabelHue2, @"Level Req to Mount Pet");
					AddLabel(230, 315, hue, @": " + cp.SavageRidgeback );
				}
				if (m_Pet is ScaledSwampDragon)
				{
					AddLabel(75, 315, LabelHue2, @"Level Req to Mount Pet");
					AddLabel(230, 315, hue, @": " + cp.ScaledSwampDragon );
				}
				if (m_Pet is SeaHorse)
				{
					AddLabel(75, 315, LabelHue2, @"Level Req to Mount Pet");
					AddLabel(230, 315, hue, @": " + cp.SeaHorse );
				}
				if (m_Pet is SilverSteed)
				{
					AddLabel(75, 315, LabelHue2, @"Level Req to Mount Pet");
					AddLabel(230, 315, hue, @": " + cp.SilverSteed );
				}
				if (m_Pet is SkeletalMount)
				{
					AddLabel(75, 315, LabelHue2, @"Level Req to Mount Pet");
					AddLabel(230, 315, hue, @": " + cp.SkeletalMount );
				}
				if (m_Pet is SwampDragon)
				{
					AddLabel(75, 315, LabelHue2, @"Level Req to Mount Pet");
					AddLabel(230, 315, hue, @": " + cp.SwampDragon );
				}
				if (m_Pet is Unicorn)
				{
					AddLabel(75, 315, LabelHue2, @"Level Req to Mount Pet");
					AddLabel(230, 315, hue, @": " + cp.Unicorn );
				}
				if (m_Pet is Reptalon)
				{
					AddLabel(75, 315, LabelHue2, @"Level Req to Mount Pet");
					AddLabel(230, 315, hue, @": " + cp.Reptalon );
				}
				if (m_Pet is WildTiger)
				{
					AddLabel(75, 315, LabelHue2, @"Level Req to Mount Pet");
					AddLabel(230, 315, hue, @": " + cp.WildTiger );
				}
				if (m_Pet is Windrunner)
				{
					AddLabel(75, 315, LabelHue2, @"Level Req to Mount Pet");
					AddLabel(230, 315, hue, @": " + cp.Windrunner );
				}
				if (m_Pet is Lasher)
				{
					AddLabel(75, 315, LabelHue2, @"Level Req to Mount Pet");
					AddLabel(230, 315, hue, @": " + cp.Lasher );
				}
				if (m_Pet is Eowmu)
				{
					AddLabel(75, 315, LabelHue2, @"Level Req to Mount Pet");
					AddLabel(230, 315, hue, @": " + cp.Eowmu );
				}
				if (m_Pet is DreadWarhorse)
				{
					AddLabel(75, 315, LabelHue2, @"Level Req to Mount Pet");
					AddLabel(230, 315, hue, @": " + cp.DreadWarhorse );
				}
				if (m_Pet is CuSidhe)
				{
					AddLabel(75, 315, LabelHue2, @"Level Req to Mount Pet");
					AddLabel(230, 315, hue, @": " + cp.CuSidhe );
				}
				if (m_Pet is CuSidhe)
				{
					AddLabel(75, 315, LabelHue2, @"Level Req to Mount Pet");
					AddLabel(230, 315, hue, @": " + cp.CuSidhe );
				}
				
			}
		}
		public static int GetButtonID( int type, int index )
		{
			return 1 + type + (index * 7);
		}

        public override void OnResponse(NetState sender, RelayInfo info)
        {
			Mobile from = sender.Mobile;
//			Target t = new PetTarget( from, true );
			
			if ( from == null )
				return;
			
            int AvlStatPoints;

            AvlStatPoints = m_Petxml.StatPoints;


            if (info.ButtonID <= 0)
                return; // Canceled

            int buttonID = info.ButtonID - 1;
            int type = buttonID % 7;
            int index = buttonID / 7;

			
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
                            case 0: // 
                                {
                                    //empty - future use
									return;
                                }
                            case 1: // 
                                {
                                   //empty - future use
								   return;
								}
                            case 2: //str
                                {
                                    if (AvlStatPoints > 0)
                                    {
										from.SendMessage("One Stat Point has been added to your Strength");
                                        m_Pet.RawStr += 1;
										m_Pet.HitsMaxSeed += 1;
                                        AvlStatPoints -= 1;
										m_Petxml.StrPointsUsed += 1;
                                        m_Petxml.StatPoints = AvlStatPoints;
										from.SendGump(new PetLevelGump(from, m_Pet ));
                                        return;
                                    }
                                    else
                                    {
										from.SendMessage("You do not have any available skill points left");
										from.SendGump(new PetLevelGump(from, m_Pet ));
                                        return;
                                    }
                                    
                                }
                            case 3: //int
                                {
                                    if (AvlStatPoints > 0)
                                    {
										from.SendMessage("One Stat Point has been added to your Intelligence");
                                        m_Pet.RawInt += 1;
                                        AvlStatPoints -= 1;
										m_Petxml.IntPointsUsed += 1;
                                        m_Petxml.StatPoints = AvlStatPoints;
                                        from.SendGump(new PetLevelGump(from, m_Pet ));
                                        return;
                                    }
                                    else
                                    {
										from.SendMessage("You do not have any available skill points left");
										from.SendGump(new PetLevelGump(from, m_Pet ));
                                        return;
                                    }
                                    
                                }
                            case 4: // Dex
                                {
                                    if (AvlStatPoints > 0)
                                    {
										from.SendMessage("One Stat Point has been added to your Dexterity");
                                        m_Pet.RawDex += 1;
                                        AvlStatPoints -= 1;
										m_Petxml.DexPointsUsed += 1;
                                        m_Petxml.StatPoints = AvlStatPoints;
                                        from.SendGump(new PetLevelGump(from, m_Pet ));
                                        return;
                                    }
                                    else
                                    {
										from.SendMessage("You do not have any available skill points left");
										from.SendGump(new PetLevelGump(from, m_Pet ));
                                        return;
                                    }
                                    
                                }
                            case 5: // TelePortToTargetToggle
                                {
									if (m_PetAttack.m_TeleToTargetSwitch == true)
										m_PetAttack.m_TeleToTargetSwitch = false;
									else
									{
										if (m_PetAttack.m_TeleToTargetSwitch == false)
										m_PetAttack.m_TeleToTargetSwitch = true;
									}
									from.SendGump(new PetLevelGump(from, m_Pet ));
                                    return;
                                }
                            case 6: // MassProvoke
                                {
									if (m_PetAttack.m_MassProvokeSwitch == true)
										m_PetAttack.m_MassProvokeSwitch = false;
									else
									{
										if (m_PetAttack.m_MassProvokeSwitch == false)
										m_PetAttack.m_MassProvokeSwitch = true;
									}
									from.SendGump(new PetLevelGump(from, m_Pet ));
                                    return;
                                }
                            case 7: // MassPeace
                                {
									if (m_PetAttack.m_MassPeace == true)
										m_PetAttack.m_MassPeace = false;
									else
									{
										if (m_PetAttack.m_MassPeace == false)
										m_PetAttack.m_MassPeace = true;
									}
									from.SendGump(new PetLevelGump(from, m_Pet ));
                                    return;
                                }
                            case 8: // BlessedPower
                                {
									if (m_PetAttack.m_BlessedPower == true)
										m_PetAttack.m_BlessedPower = false;
									else
									{
										if (m_PetAttack.m_BlessedPower == false)
										m_PetAttack.m_BlessedPower = true;
									}
									from.SendGump(new PetLevelGump(from, m_Pet ));
                                    return;
                                }
                            case 9: // FireBlast
                                {
									if (m_PetAttack.m_AreaFireBlast == true)
										m_PetAttack.m_AreaFireBlast = false;
									else
									{
										if (m_PetAttack.m_AreaFireBlast == false)
										m_PetAttack.m_AreaFireBlast = true;
									}
									from.SendGump(new PetLevelGump(from, m_Pet ));
                                    return;
                                }
                            case 10: // Ice Blast
                                {
									if (m_PetAttack.m_AreaIceBlast == true)
										m_PetAttack.m_AreaIceBlast = false;
									else
									{
										if (m_PetAttack.m_AreaIceBlast == false)
										m_PetAttack.m_AreaIceBlast = true;
									}
									from.SendGump(new PetLevelGump(from, m_Pet ));
                                    return;
                                }
                            case 11: // Air Blast
                                {
									if (m_PetAttack.m_AreaAirBlast == true)
										m_PetAttack.m_AreaAirBlast = false;
									else
									{
										if (m_PetAttack.m_AreaAirBlast == false)
										m_PetAttack.m_AreaAirBlast = true;
									}
									from.SendGump(new PetLevelGump(from, m_Pet ));
                                    return;
                                }
						}
						break;
					}
			}
		}
	}
}