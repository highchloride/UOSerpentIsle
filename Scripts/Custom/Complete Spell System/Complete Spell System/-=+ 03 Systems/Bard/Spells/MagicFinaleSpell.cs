using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Bard
{
	public class BardMagicFinaleSpell : BardSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
		                                                "Magic Finale", "Dispersus",
		                                                //SpellCircle.First,
		                                                212,9041
		                                               );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

		public override double CastDelay{ get{ return 3; } }
		public override double RequiredSkill{ get{ return 80.0; } }
		public override int RequiredMana{ get{ return 15; } }

		public BardMagicFinaleSpell( Mobile caster, Item scroll) : base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			if( CheckSequence() )
			{
				ArrayList targets = new ArrayList();

				foreach ( Mobile m in Caster.GetMobilesInRange( 4 ) )
				{
					if ( m is BaseCreature && ((BaseCreature)m).Summoned )
						targets.Add( m );
				}

				Caster.FixedParticles( 0x3709, 1, 30, 9965, 5, 7, EffectLayer.Waist );

				for ( int i = 0; i < targets.Count; ++i )
				{
					Mobile m = (Mobile)targets[i];

					Effects.SendLocationParticles( EffectItem.Create( m.Location, m.Map, EffectItem.DefaultDuration ), 0x3728, 8, 20, 5042 );

					m.Delete();
				}
			}

			FinishSequence();
		}
	}
}

