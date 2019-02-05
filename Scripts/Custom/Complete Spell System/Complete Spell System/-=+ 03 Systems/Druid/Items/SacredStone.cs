using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.ACC.CSS.Systems.Druid
{
	public class SacredStone : Item
	{
		private Mobile m_Owner;
		private bool m_campSecure = false;
		private Timer m_campSecureTimer;
		private Timer m_Timer;

		public override bool HandlesOnMovement{ get{ return true; } }
		public static int CampingRange{ get{ return 5; } }

		[Constructable]
		public SacredStone( Mobile owner ) : base ( 0x8E3 )
		{
			Movable = false;
			Name="Sacred Stone";

			m_Owner = owner;

			m_Timer = new DecayTimer( this );
			m_Timer.Start();

			m_campSecureTimer = new SecureTimer( m_Owner, this );
			m_campSecureTimer.Start();
		}

		public SacredStone( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version

			writer.Write( m_Owner );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

			switch( version )
			{
				case 0:
					{
						m_Owner = reader.ReadMobile();
						m_Timer = new DecayTimer( this );
						m_Timer.Start();
						m_campSecureTimer = new SecureTimer( m_Owner, this );
						m_campSecureTimer.Start();
						break;
					}
			}
		}

		public override void OnMovement( Mobile m, Point3D oldLocation )
		{
			if( ( m is PlayerMobile ) && ( m == m_Owner ) )
			{
				bool inOldRange = Utility.InRange( oldLocation, Location, CampingRange );
				bool inNewRange = Utility.InRange( m.Location, Location, CampingRange );

				if ( inNewRange && !inOldRange )
					OnEnter( m );
				else if ( inOldRange && !inNewRange )
					OnExit( m );
			}
		}

		public virtual void OnEnter( Mobile m )
		{
			StartSecureTimer();
		}

		public virtual void OnExit( Mobile m )
		{
			StopSecureTimer();
			m.SendMessage( "You have left the grove." );
		}

		public override void OnAfterDelete()
		{
			if( m_Timer != null )
				m_Timer.Stop();
		}

		private class DecayTimer : Timer
		{
			private SacredStone m_Owner;

			public DecayTimer( SacredStone owner ) : base( TimeSpan.FromMinutes( 2.0 ) )
			{
				Priority = TimerPriority.FiveSeconds;
				m_Owner = owner;
			}

			protected override void OnTick()
			{
				m_Owner.Delete();
			}
		}


		[CommandProperty( AccessLevel.GameMaster )]
		public bool CampSecure
		{
			get { return m_campSecure; }
			set { m_campSecure = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Camper{ get { return m_Owner; } }

		public void StartSecureTimer()
		{
			Camper.SendMessage( "You start to feel secure" ); // You feel it would take a few moments to secure your camp.
			if( m_campSecureTimer.Running == false )
			{
				m_campSecure = false;
				m_campSecureTimer.Start();
			}
		}

		public void StopSecureTimer()
		{
			m_campSecure = false;
			m_campSecureTimer.Stop();
		}

		public override void OnDelete()
		{
			base.OnDelete();
			if( m_campSecureTimer != null )
				StopSecureTimer();
		}

		private class SecureTimer : Timer
		{
			private Mobile m_Owner;
			private SacredStone m_SacredStone;

			public SecureTimer( Mobile owner , SacredStone SacredStone ) : base( TimeSpan.FromSeconds( 30.0 ) )
			{
				Priority = TimerPriority.FiveSeconds;
				m_SacredStone = SacredStone;
				m_Owner = owner;
			}

			protected override void OnTick()
			{
				m_SacredStone.CampSecure = true;
				m_Owner.SendMessage( "The power of the grove washes over you." );
			}
		}
	}
}
