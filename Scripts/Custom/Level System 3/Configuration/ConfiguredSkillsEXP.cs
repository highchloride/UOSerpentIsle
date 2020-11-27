using System;
using System.Collections;
using Server;

namespace Server
{
	/*	Tried only using skills that are not passive or used via weapon
		default gain amount is 10 */
		/* usage: ConfiguredSkillsEXP css = new ConfiguredSkillsEXP(); */
    public class ConfiguredSkillsEXP
	{
		public bool EnableEXPFromSkills			= true;
		
		
		public bool BeggingGain					= true;
		public int 	BeggingGainAmount			= 10;
		public bool CampingGain					= true;
		public int 	CampingGainAmount			= 10;
		public bool CartographyGain				= true;
		public int 	CartographyGainAmount		= 10;
		public bool ForensicsGain				= true;
		public int 	ForensicsGainAmount			= 10;
		public bool ItemIDGain					= true;
		public int 	ItemIDGainAmount			= 10;
		public bool TasteIDGain					= true;
		public int 	TasteIDGainAmount			= 10;
		public bool ImbuingGain					= true;
		public int 	ImbuingGainAmount			= 10;
		public bool EvalIntGain					= true;
		public int 	EvalIntGainAmount			= 10;
		public bool SpiritSpeakGain				= true;
		public int 	SpiritSpeakGainAmount		= 10;
		public bool FishingGain					= true;
		public int 	FishingGainAmount			= 10;
		public bool HerdingGain					= true;
		public int 	HerdingGainAmount			= 10;
		public bool TrackingGain				= true;
		public int 	TrackingGainAmount			= 10;
		public bool DetectHiddenGain			= true;
		public int 	DetectHiddenGainAmount		= 10;
		public bool HidingGain					= true;
		public int 	HidingGainAmount			= 10;
		public bool PoisoningGain				= true;
		public int 	PoisoningGainAmount			= 10;
		public bool RemoveTrapGain				= true;
		public int 	RemoveTrapGainAmount		= 10;
		public bool StealingGain				= true;
		public int 	StealingGainAmount			= 10;
		public bool DiscordanceGain				= true;
		public int 	DiscordanceGainAmount		= 10;
		public bool PeacemakingGain				= true;
		public int 	PeacemakingGainAmount		= 10;
		public bool ProvocationGain				= true;
		public int 	ProvocationGainAmount		= 10;
		
		/* Passive Skills I allowed by accident, using them
			will cause rapid exp gain */
		public bool AnatomyGain					= false; /* every weapon hit gains exp */
		public int 	AnatomyGainAmount			= 10;
		public bool ArmsLoreGain				= false;
		public int 	ArmsLoreGainAmount			= 10;
		public bool AnimalLoreGain				= false;
		public int 	AnimalLoreGainAmount		= 10;
		public bool MeditationGain				= false;
		public int 	MeditationGainAmount		= 10;
		
		/* Will not work due to the nature of the skill */
		public bool AnimalTamingGain			= false;
		public int 	AnimalTamingGainAmount		= 10;
		
		/*	This is already part of crafting, this would only add to what
			is normally given */
		public bool BlacksmithGain				= false;
		public int 	BlacksmithGainAmount		= 10;
		public bool CarpentryGain				= false;
		public int 	CarpentryGainAmount			= 10;
		public bool AlchemyGain					= false;
		public int 	AlchemyGainAmount			= 10;
		public bool FletchingGain				= false;
		public int 	FletchingGainAmount			= 10;
		public bool CookingGain					= false;
		public int 	CookingGainAmount			= 10;
		public bool InscribeGain				= false;
		public int 	InscribeGainAmount			= 10;
		public bool TailoringGain				= false;
		public int 	TailoringGainAmount			= 10;
		public bool TinkeringGain				= false;
		public int 	TinkeringGainAmount			= 10;
		
	}
 
}