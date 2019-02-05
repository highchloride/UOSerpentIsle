using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Cleric
{
	public class ClericSmiteSpell : ClericSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
		                                                "Smite", "Ferio",
		                                                //SpellCircle.Eighth,
		                                                212,
		                                                9041
		                                               );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Eighth; }
        }

		public override int RequiredTithing{ get{ return 60; } }
		public override double RequiredSkill{ get{ return 80.0; } }

		public ClericSmiteSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
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
			else if ( CheckHSequence( m ) )
			{
				m.BoltEffect( 0x480 );

				SpellHelper.Turn( Caster, m );

				double damage = Caster.Skills[SkillName.SpiritSpeak].Value * ClericDivineFocusSpell.GetScalar( Caster );

				if ( Core.AOS )
				{
					SpellHelper.Damage( TimeSpan.Zero, m, Caster, damage, 0, 0, 0, 0, 100 );
				}
				else
				{
					SpellHelper.Damage( TimeSpan.Zero, m, Caster, damage );
				}
			}

			FinishSequence();
		}


		private class InternalTarget : Target
		{
			private ClericSmiteSpell m_Owner;

			public InternalTarget( ClericSmiteSpell owner ) : base( 12, false, TargetFlags.Harmful )
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
