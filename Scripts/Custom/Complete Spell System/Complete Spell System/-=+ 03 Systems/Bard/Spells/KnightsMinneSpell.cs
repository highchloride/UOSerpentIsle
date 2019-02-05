using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Bard
{
	public class BardKnightsMinneSpell : BardSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
		                                                "Knight's Minne", "Resistus",
		                                                //SpellCircle.First,
		                                                212,9041
		                                               );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

		public override double CastDelay{ get{ return 3; } }
		public override double RequiredSkill{ get{ return 45.0; } }
		public override int RequiredMana{ get{ return 12; } }

		public BardKnightsMinneSpell( Mobile caster, Item scroll) : base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			if( CheckSequence() )
			{
				ArrayList targets = new ArrayList();

				foreach ( Mobile m in Caster.GetMobilesInRange( 3 ) )
				{
					if ( Caster.CanBeBeneficial( m, false, true ) && !(m is Golem) )
						targets.Add( m );
				}

				for ( int i = 0; i < targets.Count; ++i )
				{
					Mobile m = (Mobile)targets[i];

					TimeSpan duration = TimeSpan.FromSeconds( Caster.Skills[SkillName.Musicianship].Value * 0.2 );
					int amount = (int)( Caster.Skills[SkillName.Musicianship].Value * .18 );

					m.SendMessage( "Your Physical resistance has increased." );
					ResistanceMod mod1 = new ResistanceMod( ResistanceType.Physical, + amount );

					m.AddResistanceMod( mod1 );

					m.FixedParticles( 0x373A, 10, 15, 5012, 0x450, 3, EffectLayer.Waist );

					new ExpireTimer( m, mod1, duration ).Start();
				}
			}

			FinishSequence();
		}

		private class ExpireTimer : Timer
		{
			private Mobile m_Mobile;
			private ResistanceMod m_Mods;

			public ExpireTimer( Mobile m, ResistanceMod mod, TimeSpan delay ) : base( delay )
			{
				m_Mobile = m;
				m_Mods = mod;
			}

			public void DoExpire()
			{
				m_Mobile.RemoveResistanceMod( m_Mods );

				Stop();
			}

			protected override void OnTick()
			{
				if ( m_Mobile != null )
				{
					m_Mobile.SendMessage( "The effect of Knight's Minne wears off." );
					DoExpire();
				}
			}
		}
	}
}
