using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Engines.Harvest
{
	public class GraveRobbing : HarvestSystem
	{
		private static Type[] m_Spirit = new Type[]
		{
			typeof( Zombie ),
			typeof( Skeleton ),
			typeof( Ghoul ),
			typeof( Shade ),
			typeof( Spectre ),
			typeof( Wraith )
		};

		public static Type[] Spirit{ get{ return m_Spirit; } }


		private static Type[] m_GraveGoods = new Type[]
		{
			typeof( BonePile1 ),
			typeof( BonePile2 ),
			typeof( BonePile3 ),
			typeof( BonePile4 ),
			typeof( GraveTreasureChest1 ),
			typeof( GraveTreasureChest2 ),
			typeof( GraveTreasureChest3 ),
			typeof( GraveTreasureChest4 ),
			typeof( GraveTreasureChest5 ),
			typeof( GraveTreasureChest6 ),
             typeof( ForgottenContainer ),
		};

		public static Type[] GraveGoods{ get{ return m_GraveGoods; } }

		private static GraveRobbing m_System;

		public static GraveRobbing System
		{
			get
			{
				if ( m_System == null )
					m_System = new GraveRobbing();

				return m_System;
			}
		}

		private HarvestDefinition m_Definition;

		public HarvestDefinition Definition
		{
			get{ return m_Definition; }
		}

		private GraveRobbing()
		{
			HarvestResource[] res;
			HarvestVein[] veins;

			#region GraveRobbing
			HarvestDefinition grave = new HarvestDefinition();
			grave.BankWidth = 2;
			grave.BankHeight = 2;
			grave.MinTotal = 1;
			grave.MaxTotal = 2;
			grave.MinRespawn = TimeSpan.FromMinutes( 10.0 );
			grave.MaxRespawn = TimeSpan.FromMinutes( 20.0 );
			grave.Skill = SkillName.Mining;
			grave.Tiles = m_GraveTiles;
			grave.MaxRange = 2;
			grave.ConsumedPerHarvest = 1;
			grave.ConsumedPerFeluccaHarvest = 1;
			grave.EffectActions = new int[]{ 13 };
			grave.EffectSounds = new int[]{ 0x13E };
			grave.EffectCounts = (Core.AOS ? new int[]{ 1 } : new int[]{ 1, 2, 2, 2, 3 });
			grave.EffectDelay = TimeSpan.FromSeconds( 1.6 );
			grave.EffectSoundDelay = TimeSpan.FromSeconds( 0.9 );
			grave.NoResourcesMessage = "You see nothing worth taking."; // Nothing worth taking..
			grave.FailMessage = "You dig for a wile but find nothing."; // Nothing visible happens..
			grave.OutOfRangeMessage = 500446; // That is too far away.
			grave.PackFullMessage = 500720; // You don't have enough room in your backpack!
			grave.ToolBrokeMessage = "You broke your shovel."; // You broke your axe.

			res = new HarvestResource[]
			{
				new HarvestResource( 020.0, 010.0, 150.0, "You put some Bones in your backpack",		typeof( Bone ),	typeof( VengefulSpirit ) ),
				new HarvestResource( 050.0, 030.0, 150.0, "You put some Mandrakeroot in your backpack",		typeof( MandrakeRoot ) ),
				new HarvestResource( 050.0, 030.0, 150.0, "You put some Nightshade in your backpack",		typeof( Nightshade ) ),
				new HarvestResource( 050.0, 030.0, 150.0, "You put some SpidersSilk in your backpack",		typeof( SpidersSilk ) ),
				new HarvestResource( 065.0, 040.0, 150.0, "You put some DestroyingAngel in your backpack",	typeof( DestroyingAngel ) ),
				new HarvestResource( 065.0, 040.0, 150.0, "You put some PetrafiedWood in your backpack",	typeof( PetrafiedWood ) )
			};

			veins = new HarvestVein[]
			{
				new HarvestVein( 35.0, 0.0, res[0], res[0] ), 	// Bones
				new HarvestVein( 15.0, 0.5, res[1], res[0] ), 	// MandrakeRoot
				new HarvestVein( 15.0, 0.5, res[2], res[0] ), 	// Nightshade
				new HarvestVein( 15.0, 0.5, res[3], res[0] ), 	// SpidersSilk 
				new HarvestVein( 10.0, 0.5, res[4], res[0] ), 	// DestroyingAngel
				new HarvestVein( 10.0, 0.5, res[5], res[0] )	// PetrafiedWood
			};

			if ( Core.ML )
			{
				grave.BonusResources = new BonusHarvestResource[] // cos this is mining after all
				{
					new BonusHarvestResource( 0, 99.8998, null, null ),	//Nothing	//Note: Rounded the below to .0167 instead of 1/6th of a %.  Close enough
					new BonusHarvestResource( 100, .0167, 1072562, typeof( BlueDiamond ) ),
					new BonusHarvestResource( 100, .0167, 1072567, typeof( DarkSapphire ) ),
					new BonusHarvestResource( 100, .0167, 1072570, typeof( EcruCitrine ) ),
					new BonusHarvestResource( 100, .0167, 1072564, typeof( FireRuby ) ),
					new BonusHarvestResource( 100, .0167, 1072566, typeof( PerfectEmerald ) ),
					new BonusHarvestResource( 100, .0167, 1072568, typeof( Turquoise ) )
				};
			}

			grave.RandomizeVeins = Core.ML;

			grave.Resources = res;
			grave.Veins = veins;

			m_Definition = grave;
			Definitions.Add( grave );
			#endregion
		}

		//public override bool CheckHarvest( Mobile from, Item tool )
		//{
			//if ( !base.CheckHarvest( from, tool ) )
				//return false;

			//if ( tool.Parent != from )
			//{
				//from.SendMessage( "you must equip the axe to dig" );
				//return false;
			//}

			//return true;
		//}

		//public override bool CheckHarvest( Mobile from, Item tool, HarvestDefinition def, object toHarvest )
		//{
			//if ( !base.CheckHarvest( from, tool, def, toHarvest ) )
				//return false;

			//if ( tool.Parent != from )
			//{
				//from.SendMessage( "you must equip the axe to dig" );
				//return false;
			//}

			//return true;
		//}

		public override void OnBadHarvestTarget( Mobile from, Item tool, object toHarvest )
		{
			from.SendMessage( "you cant dig there" );
		}

		public override void OnHarvestStarted( Mobile from, Item tool, HarvestDefinition def, object toHarvest )
		{
			base.OnHarvestStarted( from, tool, def, toHarvest );
		}

		public override void OnHarvestFinished( Mobile from, Item tool, HarvestDefinition def, HarvestVein vein, HarvestBank bank, HarvestResource resource, object harvested)
		{
			int tileID; Map map; Point3D loc;

			if (GetHarvestDetails( from, tool, harvested, out tileID, out map, out loc ))
			{
				if ( 0.15 > Utility.RandomDouble() )
				{
					HarvestResource res = vein.PrimaryResource;
					
					if  ( res == resource )
					{
						try
						{
							Type chance; int num;
							double miningskill = from.Skills[SkillName.Mining].Base;

							if ( (0.67 > Utility.RandomDouble()) && (res.Types.Length >= 2) )
							{
								num = (((int)miningskill - 20) + Utility.Random(40)) / 20;

								if ( num >= 0 && num <= 5 && (0.90 > Utility.RandomDouble()) )
									chance = Spirit[num];
								else 
									chance = res.Types[1];
			
								BaseCreature spawned = Activator.CreateInstance( chance, new object[]{ 10 } ) as BaseCreature;
								if ( spawned != null )
								{
									spawned.MoveToWorld( loc, map );
									spawned.Say( "Who has disturbed me!" );
									spawned.Combatant = from;
								}
							}

							double stealingskill = from.Skills[SkillName.Stealing].Base;

							if ( (stealingskill < 60) ? (0.22 > Utility.RandomDouble()) : (0.17 > Utility.RandomDouble()) )
							{
								num = ((((int)stealingskill -30) + Utility.Random(11)) / 7) + 1;

								if ( num < 0 )
									num = 0;
								else if ( num > 9 )
									num = 9;

								BaseContainer goodies = Activator.CreateInstance( GraveGoods[Utility.Random(num)], new object[]{ } ) as BaseContainer;
								if ( goodies != null )
								{
									goodies.MoveToWorld( loc, map );
									from.SendMessage( "you dig up something interesting" );
								}
							}
						}
						catch {	}
					}
				}
			}
		}

		public static void Initialize()
		{
			Array.Sort( m_GraveTiles );
		}

		#region Tile lists
		private static int[] m_GraveTiles = new int[]
		{
			0x4ED3, 0x4EDF,	0x4EE0,	0x4EE1,	0x4EE2,	0x4EE8
		};
		#endregion
	}
}
