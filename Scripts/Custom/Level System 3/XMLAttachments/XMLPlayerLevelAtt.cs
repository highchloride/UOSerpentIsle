using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using System.Collections;
using System.Collections.Generic;

namespace Server.Engines.XmlSpawner2
{
    public class XMLPlayerLevelAtt : XmlAttachment
    {
        private int m_Levell;
        private int m_MaxLevel;
        private int m_Expp;
        private int m_ToLevell;
        private int m_kxp;
        private int m_SKPoints;
        private int m_StatPoints;
		private int m_StrPointsUsed;
		private int m_DexPointsUsed;
		private int m_IntPointsUsed;
		private int m_TotalStatPointsAquired;
		public int m_TotalSkillPointsUsed;
		public int m_SkillPointsUsedArmsLore;
		public int m_SkillPointsUsedBegging;
		public int m_SkillPointsUsedCamping;
		public int m_SkillPointsUsedCartography;
		public int m_SkillPointsUsedForensics;
		public int m_SkillPointsUsedItemID;
		public int m_SkillPointsUsedTasteID;
		public int m_SkillPointsUsedAnatomy;
		public int m_SkillPointsUsedArchery;
		public int m_SkillPointsUsedFencing;
		public int m_SkillPointsUsedFocus;
		public int m_SkillPointsUsedHealing;
		public int m_SkillPointsUsedMacing;
		public int m_SkillPointsUsedParry;
		public int m_SkillPointsUsedSwords;
		public int m_SkillPointsUsedTactics;
		public int m_SkillPointsUsedWrestling;
		public int m_SkillPointsUsedThrowing;
		public int m_SkillPointsUsedAlchemy;
		public int m_SkillPointsUsedBlacksmith;
		public int m_SkillPointsUsedFletching;
		public int m_SkillPointsUsedCarpentry;
		public int m_SkillPointsUsedCooking;
		public int m_SkillPointsUsedInscribe;
		public int m_SkillPointsUsedLumberjacking;
		public int m_SkillPointsUsedMining;
		public int m_SkillPointsUsedTailoring;
		public int m_SkillPointsUsedTinkering;
		public int m_SkillPointsUsedImbuing;
		public int m_SkillPointsUsedBushido;
		public int m_SkillPointsUsedChivalry;
		public int m_SkillPointsUsedEvalInt;
		public int m_SkillPointsUsedMagery;
		public int m_SkillPointsUsedMeditation;
		public int m_SkillPointsUsedNecromancy;
		public int m_SkillPointsUsedNinjitsu;
		public int m_SkillPointsUsedMagicResist;
		public int m_SkillPointsUsedSpellweaving;
		public int m_SkillPointsUsedSpiritSpeak;
		public int m_SkillPointsUsedMysticism;
		public int m_SkillPointsUsedAnimalLore;
		public int m_SkillPointsUsedAnimalTaming;
		public int m_SkillPointsUsedFishing;
		public int m_SkillPointsUsedHerding;
		public int m_SkillPointsUsedTracking;
		public int m_SkillPointsUsedVeterinary;
		public int m_SkillPointsUsedDetectHidden;
		public int m_SkillPointsUsedHiding;
		public int m_SkillPointsUsedLockpicking;
		public int m_SkillPointsUsedPoisoning;
		public int m_SkillPointsUsedRemoveTrap;
		public int m_SkillPointsUsedSnooping;
		public int m_SkillPointsUsedStealing;
		public int m_SkillPointsUsedStealth;
		public int m_SkillPointsUsedDiscordance;
		public int m_SkillPointsUsedMusicianship;
		public int m_SkillPointsUsedPeacemaking;
		public int m_SkillPointsUsedProvocation;
		/*Unused but for future feature 57 */

        [CommandProperty(AccessLevel.GameMaster)]
        public int Levell
        {
            get { return m_Levell; }
            set { m_Levell = value; InvalidateParentProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxLevel
        {
            get { return m_MaxLevel; }
            set { m_MaxLevel = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Expp
        {  
            get { return m_Expp = LevelCore.TExp(((PlayerMobile)this.AttachedTo)); }
            set { m_Expp = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int ToLevell
        {
            get { return m_ToLevell; }
            set { m_ToLevell = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int kxp
        {
            get { return m_kxp; }
            set { m_kxp = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int SKPoints
        {
            get { return m_SKPoints; }
            set { m_SKPoints = value; }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public int StatPoints
        {
            get { return m_StatPoints; }
            set { m_StatPoints = value; }
        }
		
        [CommandProperty(AccessLevel.GameMaster)]
        public int StrPointsUsed
        {
            get { return m_StrPointsUsed; }
            set { m_StrPointsUsed = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int DexPointsUsed
        {
            get { return m_DexPointsUsed; }
            set { m_DexPointsUsed = value; }
        }
		
        [CommandProperty(AccessLevel.GameMaster)]
        public int IntPointsUsed
        {
            get { return m_IntPointsUsed; }
            set { m_IntPointsUsed = value; }
        }
		
        [CommandProperty(AccessLevel.GameMaster)]
        public int TotalStatPointsAquired
        {
            get { return m_TotalStatPointsAquired; }
            set { m_TotalStatPointsAquired = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int TotalSkillPointsUsed
        { get { return m_TotalSkillPointsUsed; } set { m_TotalSkillPointsUsed = value; } }
		
        [CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedArmsLore
        { get { return m_SkillPointsUsedArmsLore; } set { m_SkillPointsUsedArmsLore = value; } }
		
        [CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedBegging
        { get { return m_SkillPointsUsedBegging; } set { m_SkillPointsUsedBegging = value; } }
		
        [CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedCamping
        { get { return m_SkillPointsUsedCamping; } set { m_SkillPointsUsedCamping = value; } }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedCartography
        { get { return m_SkillPointsUsedCartography; } set { m_SkillPointsUsedCartography = value; } }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedForensics
        { get { return m_SkillPointsUsedForensics; } set { m_SkillPointsUsedForensics = value; } }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedItemID
        { get { return m_SkillPointsUsedItemID; } set { m_SkillPointsUsedItemID = value; } }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedTasteID
        { get { return m_SkillPointsUsedTasteID; } set { m_SkillPointsUsedTasteID = value; } }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedAnatomy
        { get { return m_SkillPointsUsedAnatomy; } set { m_SkillPointsUsedAnatomy = value; } }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedArchery
        { get { return m_SkillPointsUsedArchery; } set { m_SkillPointsUsedArchery = value; } }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedFencing
        { get { return m_SkillPointsUsedFencing; } set { m_SkillPointsUsedFencing = value; } }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedFocus
        { get { return m_SkillPointsUsedFocus; } set { m_SkillPointsUsedFocus = value; } }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedHealing
        { get { return m_SkillPointsUsedHealing; } set { m_SkillPointsUsedHealing = value; } }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedMacing
        { get { return m_SkillPointsUsedMacing; } set { m_SkillPointsUsedMacing = value; } }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedParry
        { get { return m_SkillPointsUsedParry; } set { m_SkillPointsUsedParry = value; } }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedSwords
        { get { return m_SkillPointsUsedSwords; } set { m_SkillPointsUsedSwords = value; } }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedTactics
        { get { return m_SkillPointsUsedTactics; } set { m_SkillPointsUsedTactics = value; } }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedWrestling
        { get { return m_SkillPointsUsedWrestling; } set { m_SkillPointsUsedWrestling = value; } }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedThrowing
        { get { return m_SkillPointsUsedThrowing; } set { m_SkillPointsUsedThrowing = value; } }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedAlchemy
        { get { return m_SkillPointsUsedAlchemy; } set { m_SkillPointsUsedAlchemy = value; } }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedBlacksmith
        { get { return m_SkillPointsUsedBlacksmith; } set { m_SkillPointsUsedBlacksmith = value; } }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedFletching
        { get { return m_SkillPointsUsedFletching; } set { m_SkillPointsUsedFletching = value; } }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedCarpentry
        { get { return m_SkillPointsUsedCarpentry; } set { m_SkillPointsUsedCarpentry = value; } }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedCooking
        { get { return m_SkillPointsUsedCooking; } set { m_SkillPointsUsedCooking = value; } }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedInscribe
        { get { return m_SkillPointsUsedInscribe; } set { m_SkillPointsUsedInscribe = value; } }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedLumberjacking
        { get { return m_SkillPointsUsedLumberjacking; } set { m_SkillPointsUsedLumberjacking = value; } }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedMining
        { get { return m_SkillPointsUsedMining; } set { m_SkillPointsUsedMining = value; } }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedTailoring
        { get { return m_SkillPointsUsedTailoring; } set { m_SkillPointsUsedTailoring = value; } }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedTinkering
        { get { return m_SkillPointsUsedTinkering; } set { m_SkillPointsUsedTinkering = value; } }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedImbuing
        { get { return m_SkillPointsUsedImbuing; } set { m_SkillPointsUsedImbuing = value; } }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedBushido
        { get { return m_SkillPointsUsedBushido; } set { m_SkillPointsUsedBushido = value; } }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedChivalry
        { get { return m_SkillPointsUsedChivalry; } set { m_SkillPointsUsedChivalry = value; } }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedEvalInt
        { get { return m_SkillPointsUsedEvalInt; } set { m_SkillPointsUsedEvalInt = value; } }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedMagery
        { get { return m_SkillPointsUsedMagery; } set { m_SkillPointsUsedMagery = value; } }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedMeditation
        { get { return m_SkillPointsUsedMeditation; } set { m_SkillPointsUsedMeditation = value; } }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedNecromancy
        { get { return m_SkillPointsUsedNecromancy; } set { m_SkillPointsUsedNecromancy = value; } }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedNinjitsu
        { get { return m_SkillPointsUsedNinjitsu; } set { m_SkillPointsUsedNinjitsu = value; } }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedMagicResist
        { get { return m_SkillPointsUsedMagicResist; } set { m_SkillPointsUsedMagicResist = value; } }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedSpellweaving
        { get { return m_SkillPointsUsedSpellweaving; } set { m_SkillPointsUsedSpellweaving = value; } }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedSpiritSpeak
        { get { return m_SkillPointsUsedSpiritSpeak; } set { m_SkillPointsUsedSpiritSpeak = value; } }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedMysticism
        { get { return m_SkillPointsUsedMysticism; } set { m_SkillPointsUsedMysticism = value; } }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedAnimalLore
        { get { return m_SkillPointsUsedAnimalLore; } set { m_SkillPointsUsedAnimalLore = value; } }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedAnimalTaming
        { get { return m_SkillPointsUsedAnimalTaming; } set { m_SkillPointsUsedAnimalTaming = value; } }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedFishing
        { get { return m_SkillPointsUsedFishing; } set { m_SkillPointsUsedFishing = value; } }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedHerding
        { get { return m_SkillPointsUsedHerding; } set { m_SkillPointsUsedHerding = value; } }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedTracking
        { get { return m_SkillPointsUsedTracking; } set { m_SkillPointsUsedTracking = value; } }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedVeterinary
        { get { return m_SkillPointsUsedVeterinary; } set { m_SkillPointsUsedVeterinary = value; } }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedDetectHidden
        { get { return m_SkillPointsUsedDetectHidden; } set { m_SkillPointsUsedDetectHidden = value; } }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedHiding
        { get { return m_SkillPointsUsedHiding; } set { m_SkillPointsUsedHiding = value; } }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedLockpicking
        { get { return m_SkillPointsUsedLockpicking; } set { m_SkillPointsUsedLockpicking = value; } }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedPoisoning
        { get { return m_SkillPointsUsedPoisoning; } set { m_SkillPointsUsedPoisoning = value; } }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedRemoveTrap
        { get { return m_SkillPointsUsedRemoveTrap; } set { m_SkillPointsUsedRemoveTrap = value; } }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedSnooping
        { get { return m_SkillPointsUsedSnooping; } set { m_SkillPointsUsedSnooping = value; } }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedStealing
        { get { return m_SkillPointsUsedStealing; } set { m_SkillPointsUsedStealing = value; } }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedStealth
        { get { return m_SkillPointsUsedStealth; } set { m_SkillPointsUsedStealth = value; } }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedDiscordance
        { get { return m_SkillPointsUsedDiscordance; } set { m_SkillPointsUsedDiscordance = value; } }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedMusicianship
        { get { return m_SkillPointsUsedMusicianship; } set { m_SkillPointsUsedMusicianship = value; } }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedPeacemaking
        { get { return m_SkillPointsUsedPeacemaking; } set { m_SkillPointsUsedPeacemaking = value; } }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public int SkillPointsUsedProvocation
        { get { return m_SkillPointsUsedProvocation; } set { m_SkillPointsUsedProvocation = value; } }

        public XMLPlayerLevelAtt(ASerial serial) : base(serial)
        {
        }

        [Attachable]
        public XMLPlayerLevelAtt()
        {
        }

        [Attachable]
        public XMLPlayerLevelAtt(int level)
        {
        }

		public override void OnAttach()
		{
			base.OnAttach();
			if(AttachedTo is PlayerMobile)
			{
				PlayerStatsConfig psc = new PlayerStatsConfig();
				Configured c = new Configured();
				if(this.m_Levell == 0)
				{
					m_Levell = 1;
					m_MaxLevel = c.StartMaxLvl;
					m_ToLevell = 100;
				}
				
				PlayerAttloop loop = (PlayerAttloop)XmlAttach.FindAttachment((((PlayerMobile)this.AttachedTo)), typeof(PlayerAttloop));
				if (loop == null)
				{
					XmlAttach.AttachTo((((PlayerMobile)this.AttachedTo)), new PlayerAttloop());
				}
				
				if (psc.ForceStartingStats == true)
				{
					(((PlayerMobile)this.AttachedTo)).RawStr = psc.ForceStartingStatsSTR;
					(((PlayerMobile)this.AttachedTo)).RawDex = psc.ForceStartingStatsDEX;
					(((PlayerMobile)this.AttachedTo)).RawInt = psc.ForceStartingStatsINT;
				}
				
				if (psc.ForceNewPlayerIntoGuild == true)
					PlayerStatsConfig.ForceIntoGuild((((PlayerMobile)this.AttachedTo)));
				if (psc.AddToBackpackOnAttach == true)
					PlayerStatsConfig.CustomBackPackDrops((((PlayerMobile)this.AttachedTo)));
				if (psc.NewStartingLocation == true)
					PlayerStatsConfig.StartingLocation((((PlayerMobile)this.AttachedTo)));
				if (psc.AutoActivate_GemMining == true)
					(((PlayerMobile)this.AttachedTo)).GemMining = true;
				if (psc.AutoActivate_BasketWeaving == true)
					(((PlayerMobile)this.AttachedTo)).BasketWeaving = true;
				if (psc.AutoActivate_CanBuyCarpets == true)
					(((PlayerMobile)this.AttachedTo)).CanBuyCarpets = true;
				if (psc.AutoActivate_AcceptGuildInvites == true)
					(((PlayerMobile)this.AttachedTo)).AcceptGuildInvites = true;
				if (psc.AutoActivate_Glassblowing == true)
					(((PlayerMobile)this.AttachedTo)).Glassblowing = true;
				if (psc.AutoActivate_LibraryFriend == true)
					(((PlayerMobile)this.AttachedTo)).LibraryFriend = true;
				if (psc.AutoActivate_Masonry == true)
					(((PlayerMobile)this.AttachedTo)).Masonry = true;
				if (psc.AutoActivate_SandMining == true)
					(((PlayerMobile)this.AttachedTo)).SandMining = true;
				if (psc.AutoActivate_StoneMining == true)
					(((PlayerMobile)this.AttachedTo)).StoneMining = true;
				if (psc.AutoActivate_Spellweaving == true)
					(((PlayerMobile)this.AttachedTo)).Spellweaving = true;
				if (psc.AutoActivate_MechanicalLife == true)
					(((PlayerMobile)this.AttachedTo)).MechanicalLife = true;
				if (psc.AutoActivate_DisabledPvpWarning == true)
					(((PlayerMobile)this.AttachedTo)).DisabledPvpWarning = true;
				if (psc.AutoActivate_isYoung == true)
					(((PlayerMobile)this.AttachedTo)).Young = true;
				if (psc.AutoActivate_CantWalk == true)
					(((PlayerMobile)this.AttachedTo)).CantWalk = true;
				if (psc.AutoActivate_MaxFollowSlots == true)
					(((PlayerMobile)this.AttachedTo)).FollowersMax = psc.AutoActivate_MaxFollowSlotsTotal;
				if (psc.AutoActivate_SkillsCap == true)
					(((PlayerMobile)this.AttachedTo)).SkillsCap = psc.AutoActivate_SkillsCapVar;
				if (psc.AutoActivate_StartingTotalStatCap == true)
					(((PlayerMobile)this.AttachedTo)).StatCap = psc.AutoActivate_StartingTotalStatCapVar;
			}
			else
				Delete();
		}
		public override void OnDelete()
		{
			base.OnDelete();
			if(AttachedTo is PlayerMobile)
			{
				InvalidateParentProperties();
			}
		}
		

		
		
		public override bool HandlesOnKill { get { return true; } }
		public override void OnKill(Mobile killed, Mobile killer)
		{
			ConfiguredPetXML cp = new ConfiguredPetXML();
			Configured cc = new Configured();
			if (cc.ExpGainFromKill == true)
			LevelHandler.Set(killer, killed);
					
			if (killer is PlayerMobile)
			{
				PlayerMobile master = (PlayerMobile)killer;
				List<Mobile> pets = master.AllFollowers;
                if (pets.Count > 0)
                {
                   for (int i = 0; i < pets.Count; ++i)
					{
                        Mobile pet = (Mobile)pets[i];

                        if (pet is IMount && ((IMount)pet).Rider != null)
						{
							if (cp.MountedPetsGainEXP == false)
								return;
						}
						
						if (cp.EnabledLevelPets == true)
						LevelHandlerPet.Set(pet, killed);
					}
				}

			}
		}

		public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
		{
		}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize(writer);

			writer.Write( (int) 1 );
			
			// version 1
			writer.Write((int) m_SkillPointsUsedMeditation);
			
			// version 0
			
            writer.Write((int) m_Levell);
            writer.Write((int) m_MaxLevel);
            writer.Write((int) m_Expp);
            writer.Write((int) m_ToLevell);
            writer.Write((int) m_kxp);
            writer.Write((int) m_SKPoints);
            writer.Write((int) m_StatPoints);
            writer.Write((int) m_StrPointsUsed);
            writer.Write((int) m_DexPointsUsed);
            writer.Write((int) m_IntPointsUsed);
            writer.Write((int) m_TotalStatPointsAquired);
            writer.Write((int) m_TotalSkillPointsUsed);			
			writer.Write((int) m_SkillPointsUsedArmsLore);
			writer.Write((int) m_SkillPointsUsedBegging);
			writer.Write((int) m_SkillPointsUsedCamping);
			writer.Write((int) m_SkillPointsUsedCartography);
			writer.Write((int) m_SkillPointsUsedForensics);
			writer.Write((int) m_SkillPointsUsedItemID);
			writer.Write((int) m_SkillPointsUsedTasteID);
			writer.Write((int) m_SkillPointsUsedAnatomy);
			writer.Write((int) m_SkillPointsUsedArchery);
			writer.Write((int) m_SkillPointsUsedFencing);
			writer.Write((int) m_SkillPointsUsedFocus);
			writer.Write((int) m_SkillPointsUsedHealing);
			writer.Write((int) m_SkillPointsUsedMacing);
			writer.Write((int) m_SkillPointsUsedParry);
			writer.Write((int) m_SkillPointsUsedSwords);
			writer.Write((int) m_SkillPointsUsedTactics);
			writer.Write((int) m_SkillPointsUsedWrestling);
			writer.Write((int) m_SkillPointsUsedThrowing);
			writer.Write((int) m_SkillPointsUsedAlchemy);
			writer.Write((int) m_SkillPointsUsedBlacksmith);
			writer.Write((int) m_SkillPointsUsedFletching);
			writer.Write((int) m_SkillPointsUsedCarpentry);
			writer.Write((int) m_SkillPointsUsedCooking);
			writer.Write((int) m_SkillPointsUsedInscribe);
			writer.Write((int) m_SkillPointsUsedLumberjacking);
			writer.Write((int) m_SkillPointsUsedMining);
			writer.Write((int) m_SkillPointsUsedTailoring);
			writer.Write((int) m_SkillPointsUsedTinkering);
			writer.Write((int) m_SkillPointsUsedImbuing);
			writer.Write((int) m_SkillPointsUsedBushido);
			writer.Write((int) m_SkillPointsUsedChivalry);
			writer.Write((int) m_SkillPointsUsedEvalInt);
			writer.Write((int) m_SkillPointsUsedMagery);
			writer.Write((int) m_SkillPointsUsedNecromancy);
			writer.Write((int) m_SkillPointsUsedNinjitsu);
			writer.Write((int) m_SkillPointsUsedMagicResist);
			writer.Write((int) m_SkillPointsUsedSpellweaving);
			writer.Write((int) m_SkillPointsUsedSpiritSpeak);
			writer.Write((int) m_SkillPointsUsedMysticism);
			writer.Write((int) m_SkillPointsUsedAnimalLore);
			writer.Write((int) m_SkillPointsUsedAnimalTaming);
			writer.Write((int) m_SkillPointsUsedFishing);
			writer.Write((int) m_SkillPointsUsedHerding);
			writer.Write((int) m_SkillPointsUsedTracking);
			writer.Write((int) m_SkillPointsUsedVeterinary);
			writer.Write((int) m_SkillPointsUsedDetectHidden);
			writer.Write((int) m_SkillPointsUsedHiding);
			writer.Write((int) m_SkillPointsUsedLockpicking);
			writer.Write((int) m_SkillPointsUsedPoisoning);
			writer.Write((int) m_SkillPointsUsedRemoveTrap);
			writer.Write((int) m_SkillPointsUsedSnooping);
			writer.Write((int) m_SkillPointsUsedStealing);
			writer.Write((int) m_SkillPointsUsedStealth);
			writer.Write((int) m_SkillPointsUsedDiscordance);
			writer.Write((int) m_SkillPointsUsedMusicianship);
			writer.Write((int) m_SkillPointsUsedPeacemaking);
			writer.Write((int) m_SkillPointsUsedProvocation);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
            switch (version)
            {
                case 1:
					m_SkillPointsUsedMeditation = reader.ReadInt();
					goto case 0;
				case 0:
					// version 0
					
					m_Levell = reader.ReadInt();
					m_MaxLevel = reader.ReadInt();
					m_Expp = reader.ReadInt();
					m_ToLevell = reader.ReadInt();
					m_kxp = reader.ReadInt();
					m_SKPoints = reader.ReadInt();
					m_StatPoints = reader.ReadInt();
					m_StrPointsUsed = reader.ReadInt();
					m_DexPointsUsed = reader.ReadInt();
					m_IntPointsUsed = reader.ReadInt();
					m_TotalStatPointsAquired = reader.ReadInt();
					m_TotalSkillPointsUsed = reader.ReadInt();
					m_SkillPointsUsedArmsLore = reader.ReadInt();
					m_SkillPointsUsedBegging = reader.ReadInt();
					m_SkillPointsUsedCamping = reader.ReadInt();
					m_SkillPointsUsedCartography = reader.ReadInt();
					m_SkillPointsUsedForensics = reader.ReadInt();
					m_SkillPointsUsedItemID = reader.ReadInt();
					m_SkillPointsUsedTasteID = reader.ReadInt();
					m_SkillPointsUsedAnatomy = reader.ReadInt();
					m_SkillPointsUsedArchery = reader.ReadInt();
					m_SkillPointsUsedFencing = reader.ReadInt();
					m_SkillPointsUsedFocus = reader.ReadInt();
					m_SkillPointsUsedHealing = reader.ReadInt();
					m_SkillPointsUsedMacing = reader.ReadInt();
					m_SkillPointsUsedParry = reader.ReadInt();
					m_SkillPointsUsedSwords = reader.ReadInt();
					m_SkillPointsUsedTactics = reader.ReadInt();
					m_SkillPointsUsedWrestling = reader.ReadInt();
					m_SkillPointsUsedThrowing = reader.ReadInt();
					m_SkillPointsUsedAlchemy = reader.ReadInt();
					m_SkillPointsUsedBlacksmith = reader.ReadInt();
					m_SkillPointsUsedFletching = reader.ReadInt();
					m_SkillPointsUsedCarpentry = reader.ReadInt();
					m_SkillPointsUsedCooking = reader.ReadInt();
					m_SkillPointsUsedInscribe = reader.ReadInt();
					m_SkillPointsUsedLumberjacking = reader.ReadInt();
					m_SkillPointsUsedMining = reader.ReadInt();
					m_SkillPointsUsedTailoring = reader.ReadInt();
					m_SkillPointsUsedTinkering = reader.ReadInt();
					m_SkillPointsUsedImbuing = reader.ReadInt();
					m_SkillPointsUsedBushido = reader.ReadInt();
					m_SkillPointsUsedChivalry = reader.ReadInt();
					m_SkillPointsUsedEvalInt = reader.ReadInt();
					m_SkillPointsUsedMagery = reader.ReadInt();
					m_SkillPointsUsedNecromancy = reader.ReadInt();
					m_SkillPointsUsedNinjitsu = reader.ReadInt();
					m_SkillPointsUsedMagicResist = reader.ReadInt();
					m_SkillPointsUsedSpellweaving = reader.ReadInt();
					m_SkillPointsUsedSpiritSpeak = reader.ReadInt();
					m_SkillPointsUsedMysticism = reader.ReadInt();
					m_SkillPointsUsedAnimalLore = reader.ReadInt();
					m_SkillPointsUsedAnimalTaming = reader.ReadInt();
					m_SkillPointsUsedFishing = reader.ReadInt();
					m_SkillPointsUsedHerding = reader.ReadInt();
					m_SkillPointsUsedTracking = reader.ReadInt();
					m_SkillPointsUsedVeterinary = reader.ReadInt();
					m_SkillPointsUsedDetectHidden = reader.ReadInt();
					m_SkillPointsUsedHiding = reader.ReadInt();
					m_SkillPointsUsedLockpicking = reader.ReadInt();
					m_SkillPointsUsedPoisoning = reader.ReadInt();
					m_SkillPointsUsedRemoveTrap = reader.ReadInt();
					m_SkillPointsUsedSnooping = reader.ReadInt();
					m_SkillPointsUsedStealing = reader.ReadInt();
					m_SkillPointsUsedStealth = reader.ReadInt();
					m_SkillPointsUsedDiscordance = reader.ReadInt();
					m_SkillPointsUsedMusicianship = reader.ReadInt();
					m_SkillPointsUsedPeacemaking = reader.ReadInt();
					m_SkillPointsUsedProvocation = reader.ReadInt();	
					break;
			}
		}
		
    }
}
