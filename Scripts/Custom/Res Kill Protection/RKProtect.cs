using System;
using System.Collections.Generic;
using Server;
using Server.Network;

namespace Server.Misc
{


	public class RKProtect : Timer
	{
		private Mobile m_Mobile;

		public RKProtect(  Mobile m, double t ) : base( TimeSpan.FromSeconds( t ))
		{
			m_Mobile = m;
			m.Blessed = true;
			m.FixedParticles( 0x37C3, 10, 9, 5018, EffectLayer.Waist );
			//m.PlaySound( 506 );
			m.SendMessage( "Nothing can harm thee for the next {0} seconds.", t );
			m.SendMessage( "Type [RK to become mortal again." );
		}

		protected override void OnTick()
		{
			Mobile m = m_Mobile as Mobile;
			
			if ( m.Blessed == false )
			{
				Stop();
			}
			else
			{
				m_Mobile.Blessed = false;
				m.FixedParticles( 0x376A, 10, 15, 5018, EffectLayer.Waist );
				m.PlaySound( 481 );
				m.SendMessage( "Thou art now mortal!");

			Stop();
			}
		}
	}
}
