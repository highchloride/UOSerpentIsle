using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Ranger
{
	public class RangerLightningBowSpell : RangerSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
		                                                "Bow of Lightning", "Kurwa Vilya Cu",
		                                                //SpellCircle.Fifth,
		                                                212,
		                                                9041,
		                                                CReagent.Kindling,
		                                                Reagent.BlackPearl
		                                               );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fifth; }
        }

		public override double CastDelay{ get{ return 7.0; } }
		public override double RequiredSkill{ get{ return 90.0; } }
		public override int RequiredMana{ get{ return 30; } }

		public RangerLightningBowSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			if ( CheckSequence() )
			{
				Item weap = new RangerLightningBow( Caster );

				Caster.AddToBackpack( weap );
				Caster.SendMessage( "You create a magical bow and place it in your backpack." );

				Caster.PlaySound( 518 );

				Effects.SendLocationParticles( EffectItem.Create( Caster.Location, Caster.Map, EffectItem.DefaultDuration ), 0x376A, 1, 29, 1278, 2, 9962, 0 );
				Effects.SendLocationParticles( EffectItem.Create( new Point3D( Caster.X, Caster.Y, Caster.Z - 7 ), Caster.Map, EffectItem.DefaultDuration ), 0x37C4, 1, 29, 1278, 2, 9502, 0 );
			}
		}

		[FlipableAttribute( 0x13B2, 0x13B1 )]
		public class RangerLightningBow : BaseRanged
		{
			private Mobile m_Owner;
			private DateTime m_Expire;
			private Timer m_Timer;

			public override int EffectID{ get{ return 0xF42; } }
			public override Type AmmoType{ get{ return typeof( Arrow ); } }
			public override Item Ammo{ get{ return new Arrow(); } }

			public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.ParalyzingBlow; } }
			public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.MortalStrike; } }

			public override int AosStrengthReq{ get{ return 30; } }
			public override int AosMinDamage{ get{ return 16; } }
			public override int AosMaxDamage{ get{ return 18; } }
			public override int AosSpeed{ get{ return 25; } }

			public override int OldStrengthReq{ get{ return 20; } }
			public override int OldMinDamage{ get{ return 9; } }
			public override int OldMaxDamage{ get{ return 41; } }
			public override int OldSpeed{ get{ return 20; } }

			public override int DefMaxRange{ get{ return 10; } }

			public override int InitMinHits{ get{ return 31; } }
			public override int InitMaxHits{ get{ return 60; } }

			public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.ShootBow; } }

			[Constructable]
			public RangerLightningBow( Mobile owner ) : base( 0x13B2 )

			{
				WeaponAttributes.HitEnergyArea = 50;
				WeaponAttributes.HitLightning = 50;
				m_Owner = owner;
				Weight = 6.0;
				Layer = Layer.TwoHanded;
				Hue = 1278;
				BlessedFor = owner;
				Name = "Bow of Lightning";

				double time = ( owner.Skills[SkillName.Archery].Value / 20.0 ) * RangerHuntersAimSpell.GetScalar( owner );
				m_Expire = DateTime.Now + TimeSpan.FromMinutes( (int)time );
				m_Timer = new InternalTimer( this, m_Expire );

				m_Timer.Start();
			}

			public override void GetDamageTypes( Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy, out int chaos, out int direct )
			{
				phys = 0; fire = 0; cold = 0; pois = 0;
				nrgy = 100;
                chaos = 0;
                direct = 0;
			}

			public override void OnDelete()
			{
				if ( m_Timer != null )
					m_Timer.Stop();

				base.OnDelete();
			}

			public override bool CanEquip( Mobile m )
			{
				if ( m != m_Owner )
					return false;

				return true;
			}

			public void Remove()
			{
				m_Owner.SendMessage( "Your bow slowly dissipates." );
				Delete();
			}

			public RangerLightningBow( Serial serial ) : base( serial )
			{
			}
			public override void AddNameProperties( ObjectPropertyList list )
			{
				base.AddNameProperties( list );

				list.Add( 1049644, "Temparary Spell Enchantment" );
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );

				writer.Write( (int) 0 ); // version
				writer.Write( m_Owner );
				writer.Write( m_Expire );
			}

			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );

				int version = reader.ReadInt();
				m_Owner = reader.ReadMobile();
				m_Expire = reader.ReadDeltaTime();

				m_Timer = new InternalTimer( this, m_Expire );
				m_Timer.Start();
			}
		}

		private class InternalTimer : Timer
		{
			private RangerLightningBow m_Bow;
			private DateTime m_Expire;

			public InternalTimer( RangerLightningBow bow, DateTime expire ) : base( TimeSpan.Zero, TimeSpan.FromSeconds( 0.1 ) )
			{
				m_Bow = bow;
				m_Expire = expire;
			}

			protected override void OnTick()
			{
				if ( DateTime.Now >= m_Expire )
				{
					m_Bow.Remove();
					Stop();
				}
			}
		}
	}
}
