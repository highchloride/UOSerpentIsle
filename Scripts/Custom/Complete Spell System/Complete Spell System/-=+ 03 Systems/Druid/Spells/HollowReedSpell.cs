using System;
using Server.Targeting;
using Server.Network;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Druid
{
	public class DruidHollowReedSpell : DruidSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
		                                                "Hollow Reed", "En Crur Aeta Sec En Ess ",
                                                        //SpellCircle.Second,
		                                                203,
		                                                9061,
		                                                false,
		                                                Reagent.Bloodmoss,
		                                                Reagent.MandrakeRoot,
		                                                Reagent.Nightshade
		                                               );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

		public override double CastDelay{ get{ return 1.0; } }
		public override double RequiredSkill{ get{ return 30.0; } }
		public override int RequiredMana{ get{ return 30; } }

		public DruidHollowReedSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

		public void Target( Mobile m )
		{
			if ( !Caster.CanSee( m ) )
			{
				Caster.SendLocalizedMessage( 500237 ); // Target can not be seen.
			}
			else if ( CheckBSequence( m ) )
			{
				SpellHelper.Turn( Caster, m );
				SpellHelper.AddStatBonus( Caster, m, false, StatType.Str );
				SpellHelper.AddStatBonus( Caster, m, false, StatType.Dex );
                

				m.PlaySound( 0x15 );
				m.FixedParticles( 0x373A, 10, 15, 5018, EffectLayer.Waist );
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private DruidHollowReedSpell m_Owner;

			public InternalTarget( DruidHollowReedSpell owner ) : base( 12, false, TargetFlags.Beneficial )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is Mobile )
				{
					m_Owner.Target( (Mobile)o );
				}
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}
