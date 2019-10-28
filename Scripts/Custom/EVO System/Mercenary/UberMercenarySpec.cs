#region AuthorHeader
//
//	EvoSystem version 2.1, by Xanthos
//
//
#endregion AuthorHeader
using System;
using Server;

namespace Xanthos.Evo
{
	public sealed class UberMercenarySpec : BaseEvoSpec
	{
		// This class implements a singleton pattern; meaning that no matter how many times the
		// Instance attribute is used, there will only ever be one of these created in the entire system.
		// Copy this template and give it a new name.  Assign all of the data members of the EvoSpec
		// base class in the constructor.  Your subclass must not be abstract.
		// Never call new on this class, use the Instance attribute to get the instance instead.

		UberMercenarySpec()
		{
			m_Tamable = true;
			m_MinTamingToHatch = 0;
			m_PercentFemaleChance = .20;
			m_GuardianEggOrDeedChance = .01;
			m_AlwaysHappy = false;
			m_ProducesYoung = false;
			m_AbsoluteStatValues = false;
			m_PackSpecialItemChance = 0.02;
			m_MaxEvoResistance = 125;
			m_MaxTrainingStage = 0;
			m_CanAttackPlayers = false;

			m_Skills = new SkillName[11] { SkillName.Swords, SkillName.Macing, SkillName.Fencing, SkillName.Archery,
										  SkillName.Tactics, SkillName.Wrestling, SkillName.Parry, SkillName.Anatomy,
										  SkillName.MagicResist, SkillName.Healing, SkillName.Chivalry };
			m_MinSkillValues = new int[11] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 };
			m_MaxSkillValues = new int[11] { 120, 120, 120, 120, 120, 120, 120, 120, 120, 120, 120 };

			m_Stages = new BaseEvoStage[] { new UberMercenaryStageOne(), new UberMercenaryStageTwo(), new UberMercenaryStageThree(),
											new UberMercenaryStageFour(), new UberMercenaryStageFive(), new UberMercenaryStageSix(), new UberMercenaryStageSeven() };
		}

		// These next 2 lines facilitate the singleton pattern.  In your subclass only change the
		// BaseEvoSpec class name to your subclass of BaseEvoSpec class name and uncomment both lines.
		public static UberMercenarySpec Instance { get { return Nested.instance; } }
		class Nested { static Nested() { } internal static readonly UberMercenarySpec instance = new UberMercenarySpec();}
	}	

	// Define a subclass of BaseEvoStage for each stage in your creature and place them in the
	// array in your subclass of BaseEvoSpec.  See the example classes for how to do this.
	// Your subclass must not be abstract.

	public class UberMercenaryStageOne : BaseEvoStage
	{
		public UberMercenaryStageOne()
		{
            Title = "the mercenary";
			EvolutionMessage = "has attained a new level of combat";
			NextEpThreshold = 25000; EpMinDivisor = 10; EpMaxDivisor = 5;
			BodyValue = 0; ControlSlots = 3; VirtualArmor = 30;

			DamagesTypes = new ResistanceType[1] { ResistanceType.Physical };
			MinDamages = new int[1] { 85 };
			MaxDamages = new int[1] { 100 };

			ResistanceTypes = new ResistanceType[1] { ResistanceType.Physical };
			MinResistances = new int[1] { 10 };
			MaxResistances = new int[1] { 15 };

			DamageMin = 1; DamageMax = 5; HitsMin = 150; HitsMax = 200;
			StrMin = 30; StrMax = 35; DexMin = 30; DexMax = 35; IntMin = 30; IntMax = 35;
		}
	}

	public class UberMercenaryStageTwo : BaseEvoStage
	{
		public UberMercenaryStageTwo()
		{
			Title = "the Shadow Knight";
			EvolutionMessage = "has attained a new level of combat";
			NextEpThreshold = 75000; EpMinDivisor = 20; EpMaxDivisor = 10;
			VirtualArmor = 40;
		
			DamagesTypes = new ResistanceType[5] { ResistanceType.Physical, ResistanceType.Fire, ResistanceType.Cold,
													ResistanceType.Poison, ResistanceType.Energy };
			MinDamages = new int[5] { 10, 10, 10, 10, 10 };
			MaxDamages = new int[5] { 20, 20, 20, 20, 20 };

			ResistanceTypes = new ResistanceType[5] { ResistanceType.Physical, ResistanceType.Fire, ResistanceType.Cold,
														ResistanceType.Poison, ResistanceType.Energy };
			MinResistances = new int[5] { 10, 10, 10, 10, 10 };
			MaxResistances = new int[5] { 20, 20, 20, 20, 20 };

			DamageMin = 1; DamageMax = 2; HitsMin= 475; HitsMax = 500;
			StrMin = 15; StrMax = 16; DexMin = 15; DexMax = 16; IntMin = 15; IntMax = 16;
		}
	}

	public class UberMercenaryStageThree : BaseEvoStage
	{
		public UberMercenaryStageThree()
		{
			Title = "the Golden Knight";
			EvolutionMessage = "has attained a new level of combat";
			NextEpThreshold = 1250000; EpMinDivisor = 30; EpMaxDivisor = 20;
			VirtualArmor = 50;
		
			DamagesTypes = new ResistanceType[5] { ResistanceType.Physical, ResistanceType.Fire, ResistanceType.Cold,
													 ResistanceType.Poison, ResistanceType.Energy };
			MinDamages = new int[5] { 20, 20, 20, 20, 20 };
			MaxDamages = new int[5] { 25, 25, 25, 25, 25 };

			ResistanceTypes = new ResistanceType[5] { ResistanceType.Physical, ResistanceType.Fire, ResistanceType.Cold,
														ResistanceType.Poison, ResistanceType.Energy };
			MinResistances = new int[5] { 20, 20, 20, 20, 20 };
			MaxResistances = new int[5] { 25, 25, 25, 25, 25 };

			DamageMin = 1; DamageMax = 2; HitsMin= 100; HitsMax = 105;
			StrMin = 15; StrMax = 16; DexMin = 15; DexMax = 16; IntMin = 15; IntMax = 16;
		}
	}

	public class UberMercenaryStageFour : BaseEvoStage
	{
		public UberMercenaryStageFour()
		{
			Title = "the Blood Knight";
			EvolutionMessage = "has attained a new level of combat";
			NextEpThreshold = 3750000; EpMinDivisor = 50; EpMaxDivisor = 40;
			VirtualArmor = 60;
		
			DamagesTypes = null;
			MinDamages = null;
			MaxDamages = null;

			ResistanceTypes = new ResistanceType[5] { ResistanceType.Physical, ResistanceType.Fire, ResistanceType.Cold,
														ResistanceType.Poison, ResistanceType.Energy };
			MinResistances = new int[5] { 30, 30, 30, 30, 30 };
			MaxResistances = new int[5] { 35, 35, 35, 35, 35 };	

			DamageMin = 2; DamageMax = 4; HitsMin= 150; HitsMax = 175;
			StrMin = 100; StrMax = 105; DexMin = 55; DexMax = 65; IntMin = 15; IntMax = 16;
		}
	}

	public class UberMercenaryStageFive : BaseEvoStage
	{
		public UberMercenaryStageFive()
		{
			Title = "the Knight of Destiny";
			EvolutionMessage = "has attained a new level of combat";
			NextEpThreshold = 7750000; EpMinDivisor = 160; EpMaxDivisor = 100;
			VirtualArmor = 70;
		
			DamagesTypes = new ResistanceType[5] { ResistanceType.Physical, ResistanceType.Fire, ResistanceType.Cold,
													 ResistanceType.Poison, ResistanceType.Energy };
			MinDamages = new int[5] { 45, 45, 45, 45, 45 };
			MaxDamages = new int[5] { 50, 50, 50, 50, 50 };

			ResistanceTypes = new ResistanceType[5] { ResistanceType.Physical, ResistanceType.Fire, ResistanceType.Cold,
														ResistanceType.Poison, ResistanceType.Energy };
			MinResistances = new int[5] { 45, 45, 45, 45, 45 };
			MaxResistances = new int[5] { 50, 50, 50, 50, 50 };	

			DamageMin = 1; DamageMax = 2; HitsMin= 100; HitsMax = 105;
			StrMin = 15; StrMax = 16; DexMin = 15; DexMax = 16; IntMin = 15; IntMax = 16;
		}
	}

	public class UberMercenaryStageSix : BaseEvoStage
	{
		public UberMercenaryStageSix()
		{
            Title = "the Knight of Destiny";
			EvolutionMessage = "has attained mastery in combat!";
			NextEpThreshold = 15000000; EpMinDivisor = 540; EpMaxDivisor = 480;
			VirtualArmor = 80;
		
			DamagesTypes = new ResistanceType[5] { ResistanceType.Physical, ResistanceType.Fire, ResistanceType.Cold,
													 ResistanceType.Poison, ResistanceType.Energy };
			MinDamages = new int[5] { 60, 60, 60, 60, 60 };
			MaxDamages = new int[5] { 65, 65, 65, 65, 65 };

			ResistanceTypes = new ResistanceType[5] { ResistanceType.Physical, ResistanceType.Fire, ResistanceType.Cold,
														ResistanceType.Poison, ResistanceType.Energy };
			MinResistances = new int[5] { 75, 75, 75, 75, 75 };
			MaxResistances = new int[5] { 80, 80, 80, 80, 80 };	

			DamageMin = 2; DamageMax = 4; HitsMin= 120; HitsMax = 125;
			StrMin = 25; StrMax = 30; DexMin = 25; DexMax = 30; IntMin = 5; IntMax = 10;
		}
	}

	public class UberMercenaryStageSeven : BaseEvoStage
	{
		public UberMercenaryStageSeven()
		{
            Title = "the Knight of Destiny";
            EvolutionMessage = "has proven to be a loyal protector and friend!";
			NextEpThreshold = 15000000; EpMinDivisor = 800; EpMaxDivisor = 700;
			VirtualArmor = 85;
		
			ResistanceTypes = new ResistanceType[1] { ResistanceType.Physical };
			MinResistances = new int[1] { 80 };
			MaxResistances = new int[1] { 90 };	

			DamageMin = 2; DamageMax = 4; HitsMin= 25; HitsMax = 50;
			StrMin = 5; StrMax = 10; DexMin = 15; DexMax = 20; IntMin = 10; IntMax = 20;
		}
	}
}
