using System;
using Server;
using System.Xml;
using Server.Misc;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Regions;
using System.Collections;
using Server.Engines.Craft;
using System.Collections.Generic;
using Server.Engines.PartySystem;
using Server.Engines.XmlSpawner2;

namespace Server
{
    public class LevelHandler
    {
        public ArrayList MemberCount = new ArrayList();

        public static void Set(Mobile killer, Mobile killed)
        {
			XMLPlayerLevelAtt xmlplayerklr = (XMLPlayerLevelAtt)XmlAttach.FindAttachment(killer, typeof(XMLPlayerLevelAtt));
            PlayerMobile klr = null;
            Party pty = null;
            Configured c = new Configured();
            LevelHandler lh = new LevelHandler();
			ConfiguredPetXML cp = new ConfiguredPetXML();

            if (killer is BaseCreature)
            {
                BaseCreature bc = killer as BaseCreature;
				BaseCreature bckilled = killed as BaseCreature;

                if (bc.Controlled && c.PetKillGivesExp)
                    klr = bc.GetMaster() as PlayerMobile;
				
                if (bc.Summoned && c.PetKillGivesExp)
                    klr = bc.GetMaster() as PlayerMobile;
            }
            else
            {
                if (killer is PlayerMobile) //double check ;)
                    klr = killer as PlayerMobile;
            }

            if (lh.MemberCount.Count > 0)
            {
                foreach (Mobile il in lh.MemberCount)
                {
                    lh.MemberCount.Remove(il);
                }
            }

            if (klr != null)
            {
				Mobile m = (Mobile)killer;
				pty = Party.Get(klr);
				if (m is PlayerMobile)
				{
					XMLPlayerLevelAtt xmlplayerklr2 = (XMLPlayerLevelAtt)XmlAttach.FindAttachment(m, typeof(XMLPlayerLevelAtt));
					
					if (xmlplayerklr2.Levell < 1)
						xmlplayerklr2.Levell = 1;

					if (xmlplayerklr2.ToLevell < 50)
						xmlplayerklr2.ToLevell = 50;

					if (!(xmlplayerklr2.MaxLevel == c.StartMaxLvl && xmlplayerklr2.MaxLevel > c.EndMaxLvl))
						xmlplayerklr2.MaxLevel = c.StartMaxLvl;
				}

                AddExp(klr, killed, pty, new Configured());
            }
        }

        public static void AddExp(Mobile m, Mobile k, Party p, Configured c)
        {
			XMLPlayerLevelAtt xmlplayer = (XMLPlayerLevelAtt)XmlAttach.FindAttachment(m, typeof(XMLPlayerLevelAtt));
			ExpPowerHour exppower = (ExpPowerHour)XmlAttach.FindAttachment(m, typeof(ExpPowerHour));
            PlayerMobile pm = null;
            LevelHandler lh = new LevelHandler();

            int range = c.PartyRange;

            double orig	= 0;	//Monster Xp
            double fig	= 0;	//Party Xp
            double give	= 0;	//Xp To Give

            if (k != null)
                orig = LevelCore.Base(k);

            if (p != null && c.PartyExpShare)
            {
				if (c.PartySplitExp)
				{
					foreach (PartyMemberInfo mi in p.Members)
					{
						pm = mi.Mobile as PlayerMobile;

						if (pm.InRange(k, range) && lh.MemberCount.Count < 6)
							lh.MemberCount.Add(pm);
					}

					if (lh.MemberCount.Count > 1)
						fig = (orig / lh.MemberCount.Count);
				}
				else
				{
					pm = m as PlayerMobile;
					fig = orig;
				}
            }
            else
            {
                pm = m as PlayerMobile;
                fig = orig;
            }

            if (fig > 0)
                give = LevelHandler.ExpFilter(pm, fig, p, false);

            if (give > 0)
            {
				#region PartyExpShare
                if (p != null && c.PartyExpShare)
                {
                    foreach (PartyMemberInfo mi in p.Members)
                    {
						pm = mi.Mobile as PlayerMobile;
                        if (pm.Alive && pm.InRange(k, range))
                        {
							XMLPlayerLevelAtt xmlplayerparty = (XMLPlayerLevelAtt)XmlAttach.FindAttachment(pm, typeof(XMLPlayerLevelAtt));
							ExpPowerHour exppowerparty = (ExpPowerHour)XmlAttach.FindAttachment(pm, typeof(ExpPowerHour));
							if (xmlplayerparty == null)
							{
								return;
							}
							else
							{
								if (exppowerparty != null)
								{
									pm.SendMessage("You gained " + (give + c.ExpPowerAmount) + " boosted exp for the party kill!");
									xmlplayerparty.kxp += (int)give + c.ExpPowerAmount;
									if (pm.HasGump(typeof(ExpBar)))
									{
										pm.CloseGump(typeof(ExpBar));
										pm.SendGump(new ExpBar(pm));
									}
									if (xmlplayerparty.Expp >= xmlplayerparty.ToLevell && xmlplayerparty.Levell < xmlplayerparty.MaxLevel)
										DoLevel(pm, new Configured());
								}
								else
								{
									pm.SendMessage("You gained " + give + " exp for the party kill!");
									xmlplayerparty.kxp += (int)give;
									if (pm.HasGump(typeof(ExpBar)))
									{
										pm.CloseGump(typeof(ExpBar));
										pm.SendGump(new ExpBar(pm));
									}
									if (xmlplayerparty.Expp >= xmlplayerparty.ToLevell && xmlplayerparty.Levell < xmlplayerparty.MaxLevel)
										DoLevel(pm, new Configured());
								}
							}
                        }
                    }
                }
				#endregion
                else
                {
					if (exppower != null)
					{
						pm.SendMessage("You gained " + (give + c.ExpPowerAmount) + " boosted exp for the kill!");
						xmlplayer.kxp += (int)give + c.ExpPowerAmount;
					}
					else
					{
						pm.SendMessage("You gained " + give + " exp for the kill!");
						xmlplayer.kxp += (int)give;
					}
					if (pm.HasGump(typeof(ExpBar)))
					{
						pm.CloseGump(typeof(ExpBar));
						pm.SendGump(new ExpBar(pm));
					}

                    if (xmlplayer.Expp >= xmlplayer.ToLevell && xmlplayer.Levell < xmlplayer.MaxLevel)
                        DoLevel(pm, new Configured());
                }
            }
        }

        public static int ExpFilter(Mobile m, double o, Party p, bool craft)
        {
			XMLPlayerLevelAtt xmlplayer = (XMLPlayerLevelAtt)XmlAttach.FindAttachment(m, typeof(XMLPlayerLevelAtt));
            PlayerMobile pm = null;
            Configured c = new Configured();

            double n;
            double New = 0;
			
			#region PartyExpShare
            if (p != null && c.PartyExpShare)
            {
                if (craft)
                    return 0;
                foreach (PartyMemberInfo mi in p.Members)
                {
                    pm = mi.Mobile as PlayerMobile;
					XMLPlayerLevelAtt xmlplayerparty = (XMLPlayerLevelAtt)XmlAttach.FindAttachment(pm, typeof(XMLPlayerLevelAtt));

					if (xmlplayerparty != null)
					{
						
						if (xmlplayerparty.Expp + o > xmlplayerparty.ToLevell && xmlplayerparty.Levell >= xmlplayerparty.MaxLevel)
						{
							n = (o + xmlplayerparty.Expp) - xmlplayerparty.ToLevell;
							New = (o - n);
						}
						else
							New = o;
					}
                } 
            }
			#endregion
            else
            {
                pm = m as PlayerMobile;

                if (xmlplayer.Expp + o > xmlplayer.ToLevell && xmlplayer.Levell >= xmlplayer.MaxLevel)
                {
                    n = (o + xmlplayer.Expp) - xmlplayer.ToLevell;
                    New = (int)(o - n);
                }
                else
                    New = o;
            }

            return (int)New;
        }

        public static void DoLevel(Mobile klr, Configured c)
        {
			XMLPlayerLevelAtt xmlplayer = (XMLPlayerLevelAtt)XmlAttach.FindAttachment(klr, typeof(XMLPlayerLevelAtt));
            PlayerMobile pm = klr as PlayerMobile;
            LevelHandler lh = new LevelHandler();
			ConfiguredSkills cs = new ConfiguredSkills();

            if (xmlplayer.Expp >= xmlplayer.ToLevell)
            {
                xmlplayer.Expp = 0;
                xmlplayer.kxp = 0;
                xmlplayer.Levell += 1;
				
				int totalStats = pm.RawDex + pm.RawInt + pm.RawStr;

                if (xmlplayer.Levell <= 20)
				{
                    xmlplayer.ToLevell = (int)(xmlplayer.Levell * c.L2to20Multipier);
					if (pm.SkillsTotal < cs.SkillsTotal)
					{
						xmlplayer.SKPoints += cs.Below20;
					}
					if (totalStats < cs.MaxStatPoints)
					{
						xmlplayer.StatPoints += cs.Below20Stat;
					}
				}
				
                else if (xmlplayer.Levell <= 40)
				{
                    xmlplayer.ToLevell = (int)(xmlplayer.Levell * c.L21to40Multiplier);
					if (pm.SkillsTotal < cs.SkillsTotal)
					{
						xmlplayer.SKPoints += cs.Below40;
					}
					if (totalStats < cs.MaxStatPoints)
					{
						xmlplayer.StatPoints += cs.Below40Stat;
					}
				}
                else if (xmlplayer.Levell <= 60)
				{
                    xmlplayer.ToLevell = (int)(xmlplayer.Levell * c.L41to60Multiplier);
					if (pm.SkillsTotal < cs.SkillsTotal)
					{
						xmlplayer.SKPoints += cs.Below60;
					}
					if (totalStats < cs.MaxStatPoints)
					{
						xmlplayer.StatPoints += cs.Below60Stat;
					}
				}
                else if (xmlplayer.Levell <= 70)
				{
                    xmlplayer.ToLevell = (int)(xmlplayer.Levell * c.L61to70Multiplier);
					if (pm.SkillsTotal < cs.SkillsTotal)
					{
						xmlplayer.SKPoints += cs.Below70;
					}
					if (totalStats < cs.MaxStatPoints)
					{
						xmlplayer.StatPoints += cs.Below70Stat;
					}
				}
                else if (xmlplayer.Levell <= 80)
				{
                    xmlplayer.ToLevell = (int)(xmlplayer.Levell * c.L71to80Multiplier);
					if (pm.SkillsTotal < cs.SkillsTotal)
					{
						xmlplayer.SKPoints += cs.Below80;
					}
					if (totalStats < cs.MaxStatPoints)
					{
						xmlplayer.StatPoints += cs.Below80Stat;
					}
				}
                else if (xmlplayer.Levell <= 90)
				{
                    xmlplayer.ToLevell = (int)(xmlplayer.Levell * c.L81to90Multipier);
					if (pm.SkillsTotal < cs.SkillsTotal)
					{
						xmlplayer.SKPoints += cs.Below90;
					}
					if (totalStats < cs.MaxStatPoints)
					{
						xmlplayer.StatPoints += cs.Below90Stat;
					}
				}
                else if (xmlplayer.Levell <= 100)
				{
                    xmlplayer.ToLevell = (int)(xmlplayer.Levell * c.L91to100Multipier);
					if (pm.SkillsTotal < cs.SkillsTotal)
					{
						xmlplayer.SKPoints += cs.Below100;
					}
					if (totalStats < cs.MaxStatPoints)
					{
						xmlplayer.StatPoints += cs.Below100Stat;
					}
				}
				else if (xmlplayer.Levell <= 110)
				{
                    xmlplayer.ToLevell = (int)(xmlplayer.Levell * c.L101to110Multiplier);
					if (pm.SkillsTotal < cs.SkillsTotal)
					{
						xmlplayer.SKPoints += cs.Below110;
					}
					if (totalStats < cs.MaxStatPoints)
					{
						xmlplayer.StatPoints += cs.Below110Stat;
					}
				}
				else if (xmlplayer.Levell <= 120)
				{
                    xmlplayer.ToLevell = (int)(xmlplayer.Levell * c.L111to120Multiplier);
					if (pm.SkillsTotal < cs.SkillsTotal)
					{
						xmlplayer.SKPoints += cs.Below120;
					}
					if (totalStats < cs.MaxStatPoints)
					{
						xmlplayer.StatPoints += cs.Below120Stat;
					}
				}
				else if (xmlplayer.Levell <= 130)
				{
                    xmlplayer.ToLevell = (int)(xmlplayer.Levell * c.L121to130Multiplier);
					if (pm.SkillsTotal < cs.SkillsTotal)
					{
						xmlplayer.SKPoints += cs.Below130;
					}
					if (totalStats < cs.MaxStatPoints)
					{
						xmlplayer.StatPoints += cs.Below130Stat;
					}
				}
				else if (xmlplayer.Levell <= 140)
				{
                    xmlplayer.ToLevell = (int)(xmlplayer.Levell * c.L131to140Multiplier);
					if (pm.SkillsTotal < cs.SkillsTotal)
					{
						xmlplayer.SKPoints += cs.Below140;
					}
					if (totalStats < cs.MaxStatPoints)
					{
						xmlplayer.StatPoints += cs.Below140Stat;
					}
				}
				else if (xmlplayer.Levell <= 150)
				{
                    xmlplayer.ToLevell = (int)(xmlplayer.Levell * c.L141to150Multiplier);
					if (pm.SkillsTotal < cs.SkillsTotal)
					{
						xmlplayer.SKPoints += cs.Below150;
					}
					if (totalStats < cs.MaxStatPoints)
					{
						xmlplayer.StatPoints += cs.Below150Stat;
					}

				}
				else if (xmlplayer.Levell <= 160)
				{
                    xmlplayer.ToLevell = (int)(xmlplayer.Levell * c.L151to160Multiplier);
					if (pm.SkillsTotal < cs.SkillsTotal)
					{
						xmlplayer.SKPoints += cs.Below160;
					}
					if (totalStats < cs.MaxStatPoints)
					{
						xmlplayer.StatPoints += cs.Below160Stat;
					}
				}
				else if (xmlplayer.Levell <= 170)
				{
                    xmlplayer.ToLevell = (int)(xmlplayer.Levell * c.L161to170Multiplier);
					if (pm.SkillsTotal < cs.SkillsTotal)
					{
						xmlplayer.SKPoints += cs.Below170;
					}
					if (totalStats < cs.MaxStatPoints)
					{
						xmlplayer.StatPoints += cs.Below170Stat;
					}
				}
				else if (xmlplayer.Levell <= 180)
				{
                    xmlplayer.ToLevell = (int)(xmlplayer.Levell * c.L171to180Multiplier);
					if (pm.SkillsTotal < cs.SkillsTotal)
					{
						xmlplayer.SKPoints += cs.Below180;
					}
					if (totalStats < cs.MaxStatPoints)
					{
						xmlplayer.StatPoints += cs.Below180Stat;
					}
				}
				else if (xmlplayer.Levell <= 190)
				{
                    xmlplayer.ToLevell = (int)(xmlplayer.Levell * c.L181to190Multiplier);
					if (pm.SkillsTotal < cs.SkillsTotal)
					{
						xmlplayer.SKPoints += cs.Below190;
					}
					if (totalStats < cs.MaxStatPoints)
					{
						xmlplayer.StatPoints += cs.Below190Stat;
					}
				}
				else if (xmlplayer.Levell <= 200)
				{
                    xmlplayer.ToLevell = (int)(xmlplayer.Levell * c.L191to200Multiplier);
					if (pm.SkillsTotal < cs.SkillsTotal)
					{
						xmlplayer.SKPoints += cs.Below200;
					}
					if (totalStats < cs.MaxStatPoints)
					{
						xmlplayer.StatPoints += cs.Below200Stat;
					}
				}
				
				if (xmlplayer.Levell >= c.WhatLevelToDelete)
				{
					XMLNewPlayer xmlnewplayer = (XMLNewPlayer)XmlAttach.FindAttachment(klr, typeof(XMLNewPlayer));
					if (xmlnewplayer != null)
					{
						xmlnewplayer.Delete();
						XmlAttach.AttachTo(klr, new XMLNewPlayer());
					}
				}
				
				if (cs.GainFollowerSlotOnLevel == true)
				{
					if (xmlplayer.Levell == 20 && cs.GainOn20 == true)
						klr.FollowersMax += cs.GainFollowerSlotOnLevel20;
					if (xmlplayer.Levell == 30 && cs.GainOn30 == true)
						klr.FollowersMax += cs.GainFollowerSlotOnLevel30;
					if (xmlplayer.Levell == 40 && cs.GainOn40 == true)
						klr.FollowersMax += cs.GainFollowerSlotOnLevel40;
					if (xmlplayer.Levell == 50 && cs.GainOn50 == true)
						klr.FollowersMax += cs.GainFollowerSlotOnLevel50;
					if (xmlplayer.Levell == 60 && cs.GainOn60 == true)
						klr.FollowersMax += cs.GainFollowerSlotOnLevel60;
					if (xmlplayer.Levell == 70 && cs.GainOn70 == true)
						klr.FollowersMax += cs.GainFollowerSlotOnLevel70;
					if (xmlplayer.Levell == 80 && cs.GainOn80 == true)
						klr.FollowersMax += cs.GainFollowerSlotOnLevel80;
					if (xmlplayer.Levell == 90 && cs.GainOn90 == true)
						klr.FollowersMax += cs.GainFollowerSlotOnLevel90;
					if (xmlplayer.Levell == 100 && cs.GainOn100 == true)
						klr.FollowersMax += cs.GainFollowerSlotOnLevel100;	
					if (xmlplayer.Levell == 110 && cs.GainOn110 == true)
						klr.FollowersMax += cs.GainFollowerSlotOnLevel110;	
					if (xmlplayer.Levell == 120 && cs.GainOn120 == true)
						klr.FollowersMax += cs.GainFollowerSlotOnLevel120;	
					if (xmlplayer.Levell == 130 && cs.GainOn130 == true)
						klr.FollowersMax += cs.GainFollowerSlotOnLevel130;	
					if (xmlplayer.Levell == 140 && cs.GainOn140 == true)
						klr.FollowersMax += cs.GainFollowerSlotOnLevel140;	
					if (xmlplayer.Levell == 150 && cs.GainOn150 == true)
						klr.FollowersMax += cs.GainFollowerSlotOnLevel150;	
					if (xmlplayer.Levell == 160 && cs.GainOn160 == true)
						klr.FollowersMax += cs.GainFollowerSlotOnLevel160;	
					if (xmlplayer.Levell == 170 && cs.GainOn170 == true)
						klr.FollowersMax += cs.GainFollowerSlotOnLevel170;	
					if (xmlplayer.Levell == 180 && cs.GainOn180 == true)
						klr.FollowersMax += cs.GainFollowerSlotOnLevel180;	
					if (xmlplayer.Levell == 190 && cs.GainOn190 == true)
						klr.FollowersMax += cs.GainFollowerSlotOnLevel190;	
					if (xmlplayer.Levell == 200 && cs.GainOn200 == true)
						klr.FollowersMax += cs.GainFollowerSlotOnLevel200;	
				}
            }
            
            if (c.RefreshOnLevel)
            {
                if (pm.Hits < pm.HitsMax)
                    pm.Hits = pm.HitsMax;

                if (pm.Mana < pm.ManaMax)
                    pm.Mana = pm.ManaMax;

                if (pm.Stam < pm.StamMax)
                    pm.Stam = pm.StamMax;
            }

            pm.PlaySound(0x20F);
            pm.FixedParticles(0x376A, 1, 31, 9961, 1160, 0, EffectLayer.Waist);
            pm.FixedParticles(0x37C4, 1, 31, 9502, 43, 2, EffectLayer.Waist);
            pm.SendMessage( "Your level has increased" );
            xmlplayer.Expp = 0;
            xmlplayer.kxp = 0;
			if (pm.HasGump(typeof(ExpBar)))
			{	/* updates EXPBar */
				pm.CloseGump(typeof(ExpBar));
				pm.SendGump(new ExpBar(pm));
			}
          
            
        }
        public static int Classic(Mobile from)
        {
			XMLPlayerLevelAtt xmlplayer = (XMLPlayerLevelAtt)XmlAttach.FindAttachment(from, typeof(XMLPlayerLevelAtt));
            PlayerMobile pm = from as PlayerMobile;

            int exp = LevelCore.GetExp(pm);//LevelCore.Stats(pm) + LevelCore.Skills(pm);
            int ToLevell = (int)(xmlplayer.Levell * 100);
            int highest = 100;

            if (Cl.Enabled)
            {
                if (exp >= ToLevell && xmlplayer.Levell != highest)
                {
                    xmlplayer.Levell += 1;
                    xmlplayer.ToLevell = (int)(xmlplayer.Levell * 100);

                    if (exp >= ToLevell)
                        LevelHandler.Classic(pm);

                    return exp;
                }
                return exp;
            }
            return 0;
        }
		public static bool DoGainSkillExp(Mobile from, Skill skill, SkillName skillname)
		{
			ConfiguredSkillsEXP css = new ConfiguredSkillsEXP();
			Configured c = new Configured();
			XMLPlayerLevelAtt xmlplayer = (XMLPlayerLevelAtt)XmlAttach.FindAttachment(from, typeof(XMLPlayerLevelAtt));
			if (xmlplayer == null)
				return false;

			
			if (css.EnableEXPFromSkills == false)
				return false;
			
			if (skill == from.Skills.Provocation && css.ProvocationGain)
			{
				int gain = (int) css.ProvocationGainAmount;
				xmlplayer.kxp += gain;
				from.SendMessage( "You have gained " + (gain) + " exp from using {0}!", skillname );
				if (xmlplayer.Expp >= xmlplayer.ToLevell && xmlplayer.Levell < xmlplayer.MaxLevel)
					LevelHandler.DoLevel(from, new Configured());
			}
			if (skill == from.Skills.Peacemaking && css.PeacemakingGain)
			{
				int gain = (int) css.PeacemakingGainAmount;
				xmlplayer.kxp += gain;
				from.SendMessage( "You have gained " + (gain) + " exp from using {0}!", skillname );
				if (xmlplayer.Expp >= xmlplayer.ToLevell && xmlplayer.Levell < xmlplayer.MaxLevel)
					LevelHandler.DoLevel(from, new Configured());
			}
			if (skill == from.Skills.Discordance && css.DiscordanceGain)
			{
				int gain = (int) css.DiscordanceGainAmount;
				xmlplayer.kxp += gain;
				from.SendMessage( "You have gained " + (gain) + " exp from using {0}!", skillname );
				if (xmlplayer.Expp >= xmlplayer.ToLevell && xmlplayer.Levell < xmlplayer.MaxLevel)
					LevelHandler.DoLevel(from, new Configured());
			}
			if (skill == from.Skills.Stealing && css.StealingGain)
			{
				int gain = (int) css.StealingGainAmount;
				xmlplayer.kxp += gain;
				from.SendMessage( "You have gained " + (gain) + " exp from using {0}!", skillname );
				if (xmlplayer.Expp >= xmlplayer.ToLevell && xmlplayer.Levell < xmlplayer.MaxLevel)
					LevelHandler.DoLevel(from, new Configured());
			}
			if (skill == from.Skills.RemoveTrap && css.RemoveTrapGain)
			{
				int gain = (int) css.RemoveTrapGainAmount;
				xmlplayer.kxp += gain;
				from.SendMessage( "You have gained " + (gain) + " exp from using {0}!", skillname );
				if (xmlplayer.Expp >= xmlplayer.ToLevell && xmlplayer.Levell < xmlplayer.MaxLevel)
					LevelHandler.DoLevel(from, new Configured());
			}
			if (skill == from.Skills.Poisoning && css.PoisoningGain)
			{
				int gain = (int) css.PoisoningGainAmount;
				xmlplayer.kxp += gain;
				from.SendMessage( "You have gained " + (gain) + " exp from using {0}!", skillname );
				if (xmlplayer.Expp >= xmlplayer.ToLevell && xmlplayer.Levell < xmlplayer.MaxLevel)
					LevelHandler.DoLevel(from, new Configured());
			}
			if (skill == from.Skills.DetectHidden && css.DetectHiddenGain)
			{
				int gain = (int) css.DetectHiddenGainAmount;
				xmlplayer.kxp += gain;
				from.SendMessage( "You have gained " + (gain) + " exp from using {0}!", skillname );
				if (xmlplayer.Expp >= xmlplayer.ToLevell && xmlplayer.Levell < xmlplayer.MaxLevel)
					LevelHandler.DoLevel(from, new Configured());
			}
			if (skill == from.Skills.Tracking && css.TrackingGain)
			{
				int gain = (int) css.TrackingGainAmount;
				xmlplayer.kxp += gain;
				from.SendMessage( "You have gained " + (gain) + " exp from using {0}!", skillname );
				if (xmlplayer.Expp >= xmlplayer.ToLevell && xmlplayer.Levell < xmlplayer.MaxLevel)
					LevelHandler.DoLevel(from, new Configured());
			}
			if (skill == from.Skills.Herding && css.HerdingGain)
			{
				int gain = (int) css.HerdingGainAmount;
				xmlplayer.kxp += gain;
				from.SendMessage( "You have gained " + (gain) + " exp from using {0}!", skillname );
				if (xmlplayer.Expp >= xmlplayer.ToLevell && xmlplayer.Levell < xmlplayer.MaxLevel)
					LevelHandler.DoLevel(from, new Configured());
			}
			if (skill == from.Skills.AnimalTaming && css.AnimalTamingGain)
			{
				int gain = (int) css.AnimalTamingGainAmount;
				xmlplayer.kxp += gain;
				from.SendMessage( "You have gained " + (gain) + " exp from using {0}!", skillname );
				if (xmlplayer.Expp >= xmlplayer.ToLevell && xmlplayer.Levell < xmlplayer.MaxLevel)
					LevelHandler.DoLevel(from, new Configured());
			}
			if (skill == from.Skills.AnimalLore && css.AnimalLoreGain)
			{
				int gain = (int) css.AnimalLoreGainAmount;
				xmlplayer.kxp += gain;
				from.SendMessage( "You have gained " + (gain) + " exp from using {0}!", skillname );
				if (xmlplayer.Expp >= xmlplayer.ToLevell && xmlplayer.Levell < xmlplayer.MaxLevel)
					LevelHandler.DoLevel(from, new Configured());
			}
			if (skill == from.Skills.SpiritSpeak && css.SpiritSpeakGain)
			{
				int gain = (int) css.SpiritSpeakGainAmount;
				xmlplayer.kxp += gain;
				from.SendMessage( "You have gained " + (gain) + " exp from using {0}!", skillname );
				if (xmlplayer.Expp >= xmlplayer.ToLevell && xmlplayer.Levell < xmlplayer.MaxLevel)
					LevelHandler.DoLevel(from, new Configured());
			}
			if (skill == from.Skills.Meditation && css.MeditationGain)
			{
				int gain = (int) css.MeditationGainAmount;
				xmlplayer.kxp += gain;
				from.SendMessage( "You have gained " + (gain) + " exp from using {0}!", skillname );
				if (xmlplayer.Expp >= xmlplayer.ToLevell && xmlplayer.Levell < xmlplayer.MaxLevel)
					LevelHandler.DoLevel(from, new Configured());
			}
			if (skill == from.Skills.EvalInt && css.EvalIntGain)
			{
				int gain = (int) css.EvalIntGainAmount;
				xmlplayer.kxp += gain;
				from.SendMessage( "You have gained " + (gain) + " exp from using {0}!", skillname );
				if (xmlplayer.Expp >= xmlplayer.ToLevell && xmlplayer.Levell < xmlplayer.MaxLevel)
					LevelHandler.DoLevel(from, new Configured());
			}
			if (skill == from.Skills.Imbuing && css.ImbuingGain)
			{
				int gain = (int) css.ImbuingGainAmount;
				xmlplayer.kxp += gain;
				from.SendMessage( "You have gained " + (gain) + " exp from using {0}!", skillname );
				if (xmlplayer.Expp >= xmlplayer.ToLevell && xmlplayer.Levell < xmlplayer.MaxLevel)
					LevelHandler.DoLevel(from, new Configured());
			}
			if (skill == from.Skills.Tinkering && css.TinkeringGain)
			{
				int gain = (int) css.TinkeringGainAmount;
				xmlplayer.kxp += gain;
				from.SendMessage( "You have gained " + (gain) + " exp from using {0}!", skillname );
				if (xmlplayer.Expp >= xmlplayer.ToLevell && xmlplayer.Levell < xmlplayer.MaxLevel)
					LevelHandler.DoLevel(from, new Configured());
			}
			if (skill == from.Skills.Tailoring && css.TailoringGain)
			{
				int gain = (int) css.TailoringGainAmount;
				xmlplayer.kxp += gain;
				from.SendMessage( "You have gained " + (gain) + " exp from using {0}!", skillname );
				if (xmlplayer.Expp >= xmlplayer.ToLevell && xmlplayer.Levell < xmlplayer.MaxLevel)
					LevelHandler.DoLevel(from, new Configured());
			}
			if (skill == from.Skills.Inscribe && css.InscribeGain)
			{
				int gain = (int) css.InscribeGainAmount;
				xmlplayer.kxp += gain;
				from.SendMessage( "You have gained " + (gain) + " exp from using {0}!", skillname );
				if (xmlplayer.Expp >= xmlplayer.ToLevell && xmlplayer.Levell < xmlplayer.MaxLevel)
					LevelHandler.DoLevel(from, new Configured());
			}
			if (skill == from.Skills.Cooking && css.CookingGain)
			{
				int gain = (int) css.CookingGainAmount;
				xmlplayer.kxp += gain;
				from.SendMessage( "You have gained " + (gain) + " exp from using {0}!", skillname );
				if (xmlplayer.Expp >= xmlplayer.ToLevell && xmlplayer.Levell < xmlplayer.MaxLevel)
					LevelHandler.DoLevel(from, new Configured());
			}	
			if (skill == from.Skills.Carpentry && css.CarpentryGain)
			{
				int gain = (int) css.CarpentryGainAmount;
				xmlplayer.kxp += gain;
				from.SendMessage( "You have gained " + (gain) + " exp from using {0}!", skillname );
				if (xmlplayer.Expp >= xmlplayer.ToLevell && xmlplayer.Levell < xmlplayer.MaxLevel)
					LevelHandler.DoLevel(from, new Configured());
			}	
			if (skill == from.Skills.Blacksmith && css.BlacksmithGain)
			{
				int gain = (int) css.BlacksmithGainAmount;
				xmlplayer.kxp += gain;
				from.SendMessage( "You have gained " + (gain) + " exp from using {0}!", skillname );
				if (xmlplayer.Expp >= xmlplayer.ToLevell && xmlplayer.Levell < xmlplayer.MaxLevel)
					LevelHandler.DoLevel(from, new Configured());
			}	
			if (skill == from.Skills.Fletching && css.FletchingGain)
			{
				int gain = (int) css.FletchingGainAmount;
				xmlplayer.kxp += gain;
				from.SendMessage( "You have gained " + (gain) + " exp from using {0}!", skillname );
				if (xmlplayer.Expp >= xmlplayer.ToLevell && xmlplayer.Levell < xmlplayer.MaxLevel)
					LevelHandler.DoLevel(from, new Configured());
			}	
			if (skill == from.Skills.Alchemy && css.AlchemyGain)
			{
				int gain = (int) css.AlchemyGainAmount;
				xmlplayer.kxp += gain;
				from.SendMessage( "You have gained " + (gain) + " exp from using {0}!", skillname );
				if (xmlplayer.Expp >= xmlplayer.ToLevell && xmlplayer.Levell < xmlplayer.MaxLevel)
					LevelHandler.DoLevel(from, new Configured());
			}	
			if (skill == from.Skills.Anatomy && css.AnatomyGain)
			{
				int gain = (int) css.AnatomyGainAmount;
				xmlplayer.kxp += gain;
				from.SendMessage( "You have gained " + (gain) + " exp from using {0}!", skillname );
				if (xmlplayer.Expp >= xmlplayer.ToLevell && xmlplayer.Levell < xmlplayer.MaxLevel)
					LevelHandler.DoLevel(from, new Configured());
			}	
			if (skill == from.Skills.TasteID && css.TasteIDGain)
			{
				int gain = (int) css.TasteIDGainAmount;
				xmlplayer.kxp += gain;
				from.SendMessage( "You have gained " + (gain) + " exp from using {0}!", skillname );
				if (xmlplayer.Expp >= xmlplayer.ToLevell && xmlplayer.Levell < xmlplayer.MaxLevel)
					LevelHandler.DoLevel(from, new Configured());
			}	
			if (skill == from.Skills.ItemID && css.ItemIDGain)
			{
				int gain = (int) css.ItemIDGainAmount;
				xmlplayer.kxp += gain;
				from.SendMessage( "You have gained " + (gain) + " exp from using {0}!", skillname );
				if (xmlplayer.Expp >= xmlplayer.ToLevell && xmlplayer.Levell < xmlplayer.MaxLevel)
					LevelHandler.DoLevel(from, new Configured());
			}	
			if (skill == from.Skills.Forensics && css.ForensicsGain)
			{
				int gain = (int) css.ForensicsGainAmount;
				xmlplayer.kxp += gain;
				from.SendMessage( "You have gained " + (gain) + " exp from using {0}!", skillname );
				if (xmlplayer.Expp >= xmlplayer.ToLevell && xmlplayer.Levell < xmlplayer.MaxLevel)
					LevelHandler.DoLevel(from, new Configured());
			}	
			if (skill == from.Skills.Cartography && css.CartographyGain)
			{
				int gain = (int) css.CartographyGainAmount;
				xmlplayer.kxp += gain;
				from.SendMessage( "You have gained " + (gain) + " exp from using {0}!", skillname );
				if (xmlplayer.Expp >= xmlplayer.ToLevell && xmlplayer.Levell < xmlplayer.MaxLevel)
					LevelHandler.DoLevel(from, new Configured());
			}	
			if (skill == from.Skills.Camping && css.CampingGain)
			{
				int gain = (int) css.CampingGainAmount;
				xmlplayer.kxp += gain;
				from.SendMessage( "You have gained " + (gain) + " exp from using {0}!", skillname );
				if (xmlplayer.Expp >= xmlplayer.ToLevell && xmlplayer.Levell < xmlplayer.MaxLevel)
					LevelHandler.DoLevel(from, new Configured());
			}	
			if (skill == from.Skills.Begging && css.BeggingGain)
			{
				int gain = (int) css.BeggingGainAmount;
				xmlplayer.kxp += gain;
				from.SendMessage( "You have gained " + (gain) + " exp from using {0}!", skillname );
				if (xmlplayer.Expp >= xmlplayer.ToLevell && xmlplayer.Levell < xmlplayer.MaxLevel)
					LevelHandler.DoLevel(from, new Configured());
			}	
			if (skill == from.Skills.ArmsLore && css.ArmsLoreGain)
			{
				int gain = (int) css.ArmsLoreGainAmount;
				xmlplayer.kxp += gain;
				from.SendMessage( "You have gained " + (gain) + " exp from using {0}!", skillname );
				if (xmlplayer.Expp >= xmlplayer.ToLevell && xmlplayer.Levell < xmlplayer.MaxLevel)
					LevelHandler.DoLevel(from, new Configured());
			}			
			if (skill == from.Skills.Fishing && css.FishingGain)
			{
				int gain = (int) css.FishingGainAmount;
				xmlplayer.kxp += gain;
				from.SendMessage( "You have gained " + (gain) + " exp from using {0}!", skillname );
				if (xmlplayer.Expp >= xmlplayer.ToLevell && xmlplayer.Levell < xmlplayer.MaxLevel)
					LevelHandler.DoLevel(from, new Configured());
			}
			if (skill == from.Skills.Hiding && css.HidingGain)
			{
				int gain = (int) css.HidingGainAmount;
				xmlplayer.kxp += gain;
				from.SendMessage( "You have gained " + (gain) + " exp from using {0}!", skillname );
				if (xmlplayer.Expp >= xmlplayer.ToLevell && xmlplayer.Levell < xmlplayer.MaxLevel)
					LevelHandler.DoLevel(from, new Configured());
			}
			return true;
		}
//		public static bool BodGainEXP(Mobile from, Skill skill, SkillName skillname)
		public static void BodGainEXP(Mobile from, int points)
		{	
			XMLPlayerLevelAtt xmlplayer = (XMLPlayerLevelAtt)XmlAttach.FindAttachment(from, typeof(XMLPlayerLevelAtt));
			xmlplayer.kxp += points;
			from.SendMessage("You have been awarded {0} EXP points for turning in the bulk order.", points);
			if (xmlplayer.Expp >= xmlplayer.ToLevell && xmlplayer.Levell < xmlplayer.MaxLevel)
				LevelHandler.DoLevel(from, new Configured());
		}
	}
}