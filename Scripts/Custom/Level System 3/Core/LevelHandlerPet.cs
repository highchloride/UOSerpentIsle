using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Engines.Craft;
using Server.Engines.PartySystem;
using Server.Engines.XmlSpawner2;

namespace Server
{
    public class LevelHandlerPet
    {
        public ArrayList MemberCount = new ArrayList();

        public static void Set(Mobile killer, Mobile killed)
        {
			if (killer is PlayerMobile || killed is PlayerMobile)
			{
				return;
			}
			
			ConfiguredPetXML cp = new ConfiguredPetXML();
			BaseCreature bc = (BaseCreature)killer;
			XMLPetLevelAtt petxml = (XMLPetLevelAtt)XmlAttach.FindAttachment(bc, typeof(XMLPetLevelAtt));			
            BaseCreature klr = null;
            BaseCreature klrr = null;
            Party pty = null;
            Configured c = new Configured();
            LevelHandlerPet lh = new LevelHandlerPet();
            klr = killer as BaseCreature;
            klrr = killed as BaseCreature;

			if (klr.Summoned == true || klr.Summoned == true)
				return;
			if (klrr.Summoned == true || klrr.Summoned == true)
				return;
			
			/* Suggestion: Have the below , when NULL, set to attach if system enabled
				then continue.   Pets without the attachment would then not have a level on them yet. 
				No real reason to really indicate a pet is Level 1...  */ 
				
			if (petxml == null)
			{
				if (cp.EnabledLevelPets == true)
					XmlAttach.AttachTo(bc, new XMLPetLevelAtt());
				
				else
					return;
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
				BaseCreature bcr = (BaseCreature)killer;
				pty = Party.Get(klr);
				if (bcr is BaseCreature)
				{
				}

                AddExp(klr, killed, pty, new Configured());
            }
        }

        public static void AddExp(Mobile m, Mobile k, Party p, Configured c)
        {
			BaseCreature bc = (BaseCreature)m;
			XMLPetLevelAtt petxml = (XMLPetLevelAtt)XmlAttach.FindAttachment(bc, typeof(XMLPetLevelAtt));
			ConfiguredPetXML cp = new ConfiguredPetXML();
            PlayerMobile pm = null;
            LevelHandlerPet lh = new LevelHandlerPet();
			Mobile cm = bc.ControlMaster;

            double orig = 0;  //Monster Xp
            double fig = 0;   //Party Xp
            double give = 0;  //Xp To Give

            if (k != null)
                orig = LevelCore.Base(k);

			fig = orig;

            if (fig > 0)
                give = LevelHandlerPet.ExpFilter(m, fig, p, false);

            if (give > 0)
            {
				if (cp.NotifyOnPetExpGain == true)
				{
					cm.SendMessage("{0} gained " + give + " exp for the kill!", bc.Name);
				}
				petxml.kxp += (int)give;

				if (petxml.Expp >= petxml.ToLevell && petxml.Levell < petxml.MaxLevel)
					DoLevel(bc, new Configured());
            }
        }

        public static int ExpFilter(Mobile m, double o, Party p, bool craft)
        {
			BaseCreature bc = (BaseCreature)m;
			XMLPetLevelAtt petxml = (XMLPetLevelAtt)XmlAttach.FindAttachment(bc, typeof(XMLPetLevelAtt));
			ConfiguredPetXML cp = new ConfiguredPetXML();
            PlayerMobile pm = null;
			BaseCreature bcc = null;
            Configured c = new Configured();

            double n;
            double New = 0;

			if (p != null && c.PartyExpShare)
			{
			}
			else
			{
				bcc = m as BaseCreature;
				XMLPetLevelAtt petxml3 = (XMLPetLevelAtt)XmlAttach.FindAttachment(bcc, typeof(XMLPetLevelAtt));
				
				if (petxml3 == null)
					return 0;
				
				if (petxml3.Expp + o > petxml3.ToLevell && petxml3.Levell >= petxml3.MaxLevel)
				{
					n = (o + petxml3.Expp) - petxml3.ToLevell;
					New = (int)(o - n);
				}
				else
					New = o;
			}
				
            return (int)New;
        }

        public static void DoLevel(Mobile klr, Configured c)
        {
			BaseCreature bc = (BaseCreature)klr;
			XMLPetLevelAtt petxml = (XMLPetLevelAtt)XmlAttach.FindAttachment(bc, typeof(XMLPetLevelAtt));
			ConfiguredPetXML cs = new ConfiguredPetXML();
            PlayerMobile pm = klr as PlayerMobile;
            LevelHandlerPet lh = new LevelHandlerPet();
			ConfiguredSkills css = new ConfiguredSkills();
			Mobile cm = bc.ControlMaster;

			/* Still adding Skill Points for Future Development */
			
            if (petxml.Expp >= petxml.ToLevell)
            {
                petxml.Expp = 0;
                petxml.kxp = 0;
                petxml.Levell += 1;
				
				int totalStats = bc.RawDex + bc.RawInt + bc.RawStr;

                if (petxml.Levell <= 20)
				{
                    petxml.ToLevell = (int)(petxml.Levell * 20);
					if (bc.SkillsTotal < cs.SkillsTotal)
					{
						petxml.SKPoints += cs.Below20;
					}
					if (totalStats < cs.MaxStatPoints)
					{
						petxml.StatPoints += cs.Below20Stat;
					}
				}
				
                else if (petxml.Levell <= 40)
				{
                    petxml.ToLevell = (int)(petxml.Levell * 40);
					if (bc.SkillsTotal < cs.SkillsTotal)
					{
						petxml.SKPoints += cs.Below40;
					}
					if (totalStats < cs.MaxStatPoints)
					{
						petxml.StatPoints += cs.Below40Stat;
					}
				}
                else if (petxml.Levell <= 60)
				{
                    petxml.ToLevell = (int)(petxml.Levell * 60);
					if (bc.SkillsTotal < cs.SkillsTotal)
					{
						petxml.SKPoints += cs.Below60;
					}
					if (totalStats < cs.MaxStatPoints)
					{
						petxml.StatPoints += cs.Below60Stat;
					}
				}
                else if (petxml.Levell <= 70)
				{
                    petxml.ToLevell = (int)(petxml.Levell * 80);
					if (bc.SkillsTotal < cs.SkillsTotal)
					{
						petxml.SKPoints += cs.Below70;
					}
					if (totalStats < cs.MaxStatPoints)
					{
						petxml.StatPoints += cs.Below70Stat;
					}
				}
                else if (petxml.Levell <= 80)
				{
                    petxml.ToLevell = (int)(petxml.Levell * 100);
					if (bc.SkillsTotal < cs.SkillsTotal)
					{
						petxml.SKPoints += cs.Below80;
					}
					if (totalStats < cs.MaxStatPoints)
					{
						petxml.StatPoints += cs.Below80Stat;
					}
				}
                else if (petxml.Levell <= 90)
				{
                    petxml.ToLevell = (int)(petxml.Levell * 120);
					if (bc.SkillsTotal < cs.SkillsTotal)
					{
						petxml.SKPoints += cs.Below90;
					}
					if (totalStats < cs.MaxStatPoints)
					{
						petxml.StatPoints += cs.Below90Stat;
					}
				}
                else if (petxml.Levell <= 100)
				{
                    petxml.ToLevell = (int)(petxml.Levell * 140);
					if (bc.SkillsTotal < cs.SkillsTotal)
					{
						petxml.SKPoints += cs.Below100;
					}
					if (totalStats < cs.MaxStatPoints)
					{
						petxml.StatPoints += cs.Below100Stat;
					}
				}
				else if (petxml.Levell <= 110)
				{
                    petxml.ToLevell = (int)(petxml.Levell * 140);
					if (bc.SkillsTotal < cs.SkillsTotal)
					{
						petxml.SKPoints += cs.Below110;
					}
					if (totalStats < cs.MaxStatPoints)
					{
						petxml.StatPoints += cs.Below110Stat;
					}
				}
				else if (petxml.Levell <= 120)
				{
                    petxml.ToLevell = (int)(petxml.Levell * 150);
					if (bc.SkillsTotal < cs.SkillsTotal)
					{
						petxml.SKPoints += cs.Below120;
					}
					if (totalStats < cs.MaxStatPoints)
					{
						petxml.StatPoints += cs.Below120Stat;
					}
				}
				else if (petxml.Levell <= 130)
				{
                    petxml.ToLevell = (int)(petxml.Levell * 150);
					if (bc.SkillsTotal < cs.SkillsTotal)
					{
						petxml.SKPoints += cs.Below130;
					}
					if (totalStats < cs.MaxStatPoints)
					{
						petxml.StatPoints += cs.Below130Stat;
					}
				}
				else if (petxml.Levell <= 140)
				{
                    petxml.ToLevell = (int)(petxml.Levell * 160);
					if (bc.SkillsTotal < cs.SkillsTotal)
					{
						petxml.SKPoints += cs.Below140;
					}
					if (totalStats < cs.MaxStatPoints)
					{
						petxml.StatPoints += cs.Below140Stat;
					}
				}
				else if (petxml.Levell <= 150)
				{
                    petxml.ToLevell = (int)(petxml.Levell * 180);
					if (bc.SkillsTotal < cs.SkillsTotal)
					{
						petxml.SKPoints += cs.Below150;
					}
					if (totalStats < cs.MaxStatPoints)
					{
						petxml.StatPoints += cs.Below150Stat;
					}
				}
				else if (petxml.Levell <= 160)
				{
                    petxml.ToLevell = (int)(petxml.Levell * 180);
					if (bc.SkillsTotal < cs.SkillsTotal)
					{
						petxml.SKPoints += cs.Below160;
					}
					if (totalStats < cs.MaxStatPoints)
					{
						petxml.StatPoints += cs.Below160Stat;
					}
				}
				else if (petxml.Levell <= 170)
				{
                    petxml.ToLevell = (int)(petxml.Levell * 190);
					if (bc.SkillsTotal < cs.SkillsTotal)
					{
						petxml.SKPoints += cs.Below170;
					}
					if (totalStats < cs.MaxStatPoints)
					{
						petxml.StatPoints += cs.Below170Stat;
					}
				}
				else if (petxml.Levell <= 180)
				{
                    petxml.ToLevell = (int)(petxml.Levell * 190);
					if (bc.SkillsTotal < cs.SkillsTotal)
					{
						petxml.SKPoints += cs.Below180;
					}
					if (totalStats < cs.MaxStatPoints)
					{
						petxml.StatPoints += cs.Below180Stat;
					}
				}
				else if (petxml.Levell <= 190)
				{
                    petxml.ToLevell = (int)(petxml.Levell * 190);
					if (bc.SkillsTotal < cs.SkillsTotal)
					{
						petxml.SKPoints += cs.Below190;
					}
					if (totalStats < cs.MaxStatPoints)
					{
						petxml.StatPoints += cs.Below190Stat;
					}
				}
				else if (petxml.Levell <= 200)
				{
                    petxml.ToLevell = (int)(petxml.Levell * 200);
					if (bc.SkillsTotal < cs.SkillsTotal)
					{
						petxml.SKPoints += cs.Below200;
					}
					if (totalStats < cs.MaxStatPoints)
					{
						petxml.StatPoints += cs.Below200Stat;
					}
				}
            }
            
            if (cs.RefreshOnLevel)
            {
                if (bc.Hits < bc.HitsMax)
                    bc.Hits = bc.HitsMax;

                if (bc.Mana < bc.ManaMax)
                    bc.Mana = bc.ManaMax;

                if (bc.Stam < bc.StamMax)
                    bc.Stam = bc.StamMax;
            }

            bc.PlaySound(0x20F);
            bc.FixedParticles(0x376A, 1, 31, 9961, 1160, 0, EffectLayer.Waist);
            bc.FixedParticles(0x37C4, 1, 31, 9502, 43, 2, EffectLayer.Waist);
			
			if (cs.NotifyOnPetlevelUp == true)
			{
				cm.SendMessage( "Your Pet level has increased" );
			}
            petxml.Expp = 0;
            petxml.kxp = 0;
          
            
        }

		public static void PetOnDeath( BaseCreature bc )
		{
			XMLPetLevelAtt petxml = (XMLPetLevelAtt)XmlAttach.FindAttachment(bc, typeof(XMLPetLevelAtt));
			Mobile master = bc.ControlMaster;
			ConfiguredPetXML cp = new ConfiguredPetXML();
			
			if ( master != null && bc.Controlled == true && petxml != null )
			{
				if (cp.LoseExpLevelOnDeath == true)
				{
					if (cp.LoseStatOnDeath == true)
					{
						int strloss = bc.Str		/ cp.PetStatLossAmount;
						int dexloss = bc.Dex		/ cp.PetStatLossAmount;
						int intloss = bc.Int		/ cp.PetStatLossAmount;	
						if (bc.Str > strloss )
							bc.Str -= strloss;
						if (bc.Dex > dexloss )
							bc.Dex -= dexloss;
						if (bc.Int > intloss )
							bc.Int -= intloss;
					}
					int ExpLoss = petxml.Expp	/ cp.PetStatLossAmount;
					int KXPLoss = petxml.kxp	/ cp.PetStatLossAmount;
					petxml.Expp	-= ExpLoss;
					petxml.kxp	-= KXPLoss;

					if (petxml.Expp <= 0)
					{
						petxml.Levell		-= 1;
						petxml.Expp			= 0;
						petxml.kxp			= 0;
					}
					master.SendMessage( 38, "Your pet has suffered a 5% stat lose due to its untimely death." );					
				}
				else
				{
					master.SendMessage( 64, "Your pet has been killed!" );
				}
			}
		}
    }
}