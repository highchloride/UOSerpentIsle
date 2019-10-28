#region AuthorHeader
//
//	EvoSystem version 2.1, by Xanthos
//
//
#endregion AuthorHeader
using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Xanthos.Interfaces;

namespace Xanthos.Evo
{
	[CorpseName( "a guardian mercenary corpse" )]
	public class GuardianMercenary : Mercenary, IEvoGuardian
	{
		private double kReflectDamagePercent = 20;
		
		private bool m_RedOrBlue;

		public override bool AddPointsOnDamage { get { return false; } }
		public override bool AddPointsOnMelee { get { return false; } }
		public override Poison HitPoison{ get{ return Poison.Lethal; }}

		[Constructable]
		public GuardianMercenary() : base( "A Guardian Mercenary" )
		{
		}

		public GuardianMercenary( Serial serial ) : base( serial )
		{
		}

		protected override void Init()
		{
			base.Init();			// Create and fully evolve the creature

			// Buff it up
			SetStr( Str * Utility.RandomMinMax( 2, 4 ));
			SetDex( Dex * Utility.RandomMinMax( 2, 4 ));
			SetStam( Stam * Utility.RandomMinMax( 2, 4 ));
			SetInt( (int)(Int * 2) );
			SetMana( (int)(Mana * 2) );
			SetHits( Hits * Utility.RandomMinMax( 15, 20 ));
			DamageMin *= Utility.RandomMinMax( 1, 5 ); DamageMax *= Utility.RandomMinMax( 15, 25 );
			VirtualArmor *= 2;

			ColdDamage *= 2;
			PoisonDamage *= 2;
			PhysicalDamage = 0;

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
			m_RedOrBlue = Utility.RandomBool();
			FightMode = (FightMode)Utility.RandomMinMax( (int)FightMode.Aggressor, (int)FightMode.Evil );

			// Now dress it up
			AddItem( new LeatherArms() );
			AddItem( new LeatherChest() );
			m_RedOrBlue = Utility.RandomBool();
			FightMode = (FightMode)Utility.RandomMinMax( (int)FightMode.Aggressor, (int)FightMode.Evil );
			AddItem( new LeatherLegs() );
			AddItem( new Boots() );
			SpiritOfTheTotem totem = new SpiritOfTheTotem(); totem.LootType = .05 > Utility.RandomDouble() ? LootType.Regular : LootType.Blessed;
			BladeOfInsanity blade = new BladeOfInsanity(); blade.LootType = LootType.Blessed; blade.Movable = false; blade.MaxHitPoints = -1;
			GauntletsOfNobility gloves = new GauntletsOfNobility(); gloves.LootType = .05 > Utility.RandomDouble() ? LootType.Regular : LootType.Blessed;
			JackalsCollar collar = new JackalsCollar(); collar.LootType = .05 > Utility.RandomDouble() ? LootType.Regular : LootType.Blessed;
			AddItem( totem );
			AddItem( blade );
			AddItem( gloves );
			AddItem( collar );
			PackItem( new Bandage( Utility.RandomMinMax( 5000, 7000 ) ) );
			new Nightmare().Rider = this;
		}

		public override bool AlwaysMurderer{ get{ return m_RedOrBlue; }}
		protected override void PackSpecialItem() { }
		
		public override void GenerateLoot()
		{
			BaseEvoSpec spec = GetEvoSpec();

			if ( null != spec && spec.GuardianEggOrDeedChance > Utility.RandomDouble() )
			{
				MercenaryDeed deed = new MercenaryDeed(); deed.LootType = LootType.Regular;
				PackItem( deed );
			}
			AddLoot( LootPack.UltraRich, 4 );
			AddLoot( LootPack.FilthyRich );
		}

		public override void OnDamage( int amount, Mobile from, bool willKill )
		{
			base.OnDamage( amount, from, willKill );

			if ( kReflectDamagePercent > 0 && null != from && !(from.Deleted))
				from.Damage( (int)(Math.Round( amount / kReflectDamagePercent )), this );
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