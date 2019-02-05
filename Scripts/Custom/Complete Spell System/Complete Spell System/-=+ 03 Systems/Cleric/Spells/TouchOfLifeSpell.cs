using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Cleric
{
	public class ClericTouchOfLifeSpell : ClericSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
		                                                "Touch of Life", "Tactus Vitalis",
		                                                //SpellCircle.Third,
		                                                212,
		                                                9041
		                                               );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

		public override int RequiredTithing{ get{ return 10; } }
		public override double RequiredSkill{ get{ return 30.0; } }

		public ClericTouchOfLifeSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
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
			else if ( CheckBSequence( m, false ) )
			{
				SpellHelper.Turn( Caster, m );

				m.PlaySound( 0x202 );
				m.FixedParticles( 0x376A, 1, 62, 0x480, 3, 3, EffectLayer.Waist );
				m.FixedParticles( 0x3779, 1, 46, 0x481, 5, 3, EffectLayer.Waist );

				double toHeal = Caster.Skills[SkillName.SpiritSpeak].Value / 2.0 + Utility.Random( 5 );

				toHeal *= ClericDivineFocusSpell.GetScalar( Caster );

				m.Heal( (int)toHeal );
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private ClericTouchOfLifeSpell m_Owner;

			public InternalTarget( ClericTouchOfLifeSpell owner ) : base( 12, false, TargetFlags.Beneficial )
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
