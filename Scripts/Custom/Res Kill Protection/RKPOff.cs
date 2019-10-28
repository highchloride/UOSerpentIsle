using Server;
using Server.Mobiles;
using Server.Network;
using Server.Misc;
using System;

namespace Server.Commands
{
    class RKPOff
    {
        private static Mobile m_caller;

        public static void Initialize()
        {
            CommandSystem.Register("RK", AccessLevel.Player, new CommandEventHandler(OnCommand));
        }
        public static void OnCommand ( CommandEventArgs e )
        {
            m_caller = e.Mobile;
			
			if ( m_caller.Blessed == false )
			{
				m_caller.SendMessage( "Thou art already a mortal!");
			}
            else if (m_caller.IsStaff())
            {
                m_caller.SendMessage("Use a StaffOrb to lose your immortality.");
            }
			else
			{
				m_caller.Blessed = false;
				m_caller.FixedParticles( 0x376A, 10, 15, 5018, EffectLayer.Waist );
				m_caller.PlaySound( 513 );
				m_caller.SendMessage( "Thou art now mortal.");

			}
        }
    }
}
