using System;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Bard
{
	public class BardFoeRequiemSpell : BardSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
		                                                "Foe Requiem", "Sonicus",
		                                                //SpellCircle.Sixth,
		                                                212,9041
		                                               );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

		public BardFoeRequiemSpell( Mobile caster, Item scroll) : base( caster, scroll, m_Info )
		{
		}

		public override double CastDelay{ get{ return 3; } }
		public override double RequiredSkill{ get{ return 55.0; } }
		public override int RequiredMana{ get{ return 18; } }

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
			else if ( CheckHSequence( m ) )
			{
				Mobile source = Caster;

				SpellHelper.Turn( Caster, m );

				double damage = ( Utility.Random( 28, 23 ) + Utility.Random( 5, 1 ) );

				if ( CheckResisted( m ) )
				{
					m.SendLocalizedMessage( 501783 ); // You feel yourself resisting magical energy.
					damage *= .4;
				}

				//sound damage, all resistances
				SpellHelper.Damage( this, m, damage, 20, 20, 20, 20, 20 );

				m.FixedParticles( 0x374A, 10, 15, 5028, EffectLayer.Head );
				source.MovingParticles( m, 0x379F, 7, 0, false, true, 3043, 4043, 0x211 );
				m.PlaySound( 0x1EA );
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private BardFoeRequiemSpell m_Owner;

			public InternalTarget( BardFoeRequiemSpell owner ) : base( 12, false, TargetFlags.Harmful )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is Mobile )
					m_Owner.Target( (Mobile)o );
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}
