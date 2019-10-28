using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Xanthos.Interfaces;

namespace Xanthos.Evo
{
	[CorpseName( "a guardian hiryu corpse" )]
	public class GuardianHiryu : EvoHiryu, IEvoGuardian
	{
		public override bool AddPointsOnDamage { get { return false; } }
		public override bool AddPointsOnMelee { get { return false; } }

		[Constructable]
		public GuardianHiryu() : base( "A Guardian Hiryu" )
		{
		}

		public GuardianHiryu( Serial serial ) : base( serial )
		{
		}

		protected override void Init()
		{
			base.Init();			// Create and fully evolve the creature

			// Buff it up
			SetStr( Str * 3 );
			SetDex( Dex * 3 );
			SetStam( Stam * 3 );
			SetInt( (int)(Int * 2) );
			SetMana( (int)(Mana * 2) );
			SetHits( Hits * 10 );

			BaseEvoSpec spec = GetEvoSpec();

			if ( null != spec && null != spec.Skills )
			{
				for ( int i = 0;  i < spec.Skills.Length; i++ )
				{
					SetSkill( spec.Skills[ i ], (double)(spec.MaxSkillValues[ i ]) * 1.10, (double)(spec.MaxSkillValues[ i ]) * 1.50 );
				}
			}
			this.Tamable = false;	// Not appropriate as a pet
			Title = "";
		}

		protected override void PackSpecialItem() { }
		
		public override void GenerateLoot()
		{
			BaseEvoSpec spec = GetEvoSpec();

			if ( null != spec && spec.GuardianEggOrDeedChance > Utility.RandomDouble() )
			{
				BaseEvoEgg egg = GetEvoEgg();

				if ( null != egg )
					PackItem( egg );
			}
			AddLoot( LootPack.UltraRich, 4 );
			AddLoot( LootPack.FilthyRich );
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write( (int)0 );			
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}