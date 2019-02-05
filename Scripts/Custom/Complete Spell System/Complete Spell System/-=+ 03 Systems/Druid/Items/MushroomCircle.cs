using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.ACC.CSS.Systems.Druid
{
	public class MushroomCircle : Item
	{
		private Timer m_Timer;

		[Constructable]
		public MushroomCircle ()
		{
		}

		public MushroomCircle( Serial serial ) : base( serial )
		{
		}

		public override bool OnMoveOver( Mobile m )
		{
			m.SendMessage("The magic of the stones prevents you from crossing.");
			return false;
		}

		public override void OnAfterDelete()
		{
			if ( m_Timer != null )
				m_Timer.Stop();

			m_Timer = null;

			base.OnAfterDelete();
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			Delete();
		}
	}
}
