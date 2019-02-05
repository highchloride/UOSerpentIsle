using System;
using Server.Targeting;
using Server.Network;
using Server.Misc;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Druid
{
	public class DruidBlendWithForestSpell : DruidSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
		                                                "Blend With Forest", "Kes Ohm",
		                                                //SpellCircle.Sixth,
		                                                206,
		                                                9002,
		                                                false,
		                                                Reagent.Bloodmoss,
		                                                Reagent.Nightshade
		                                               );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

		public override double CastDelay{ get{ return 2.0; } }
		public override double RequiredSkill{ get{ return 75.0; } }
		public override int RequiredMana{ get{ return 60; } }
		private bool speak;

		public DruidBlendWithForestSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
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
				SpellHelper.Turn( Caster, m );

				speak=m.Squelched;

				m.PlaySound( 0x19 );
				m.Paralyze( TimeSpan.FromSeconds( 20.0 ) );
				m.FixedParticles( 0x375A, 2, 10, 5027, 0x3D, 2, EffectLayer.Waist );
				m.Hidden = true;
				m.Squelched = true;

				Point3D loc = new Point3D( m.X, m.Y, m.Z );
				Item item = new InternalItem( loc, Caster.Map, Caster,m , speak );
			}

			FinishSequence();
		}

		private class InternalItem : Item
		{
			private Timer m_Timer;
			private DateTime m_End;
			private Mobile m_Owner;
			private bool squeltched;

			public InternalItem( Point3D loc, Map map, Mobile caster, Mobile m, bool talk ) : base( 0xC9E )
			{
				Visible = false;
				Movable = false;
				m_Owner=m;
				squeltched = talk;

				MoveToWorld( loc, map );

				if ( caster.InLOS( this ) )
					Visible = true;
				else
					Delete();

				if ( Deleted )
					return;

				m_Timer = new InternalTimer( this, TimeSpan.FromSeconds( 20.0 ), m_Owner, squeltched );
				m_Timer.Start();

				m_End = DateTime.Now + TimeSpan.FromSeconds( 30.0 );
			}

			public InternalItem( Serial serial ) : base( serial )
			{
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );

				writer.Write( (int) 1 ); // version

				writer.Write( m_End - DateTime.Now );
				writer.Write(m_Owner);
				writer.Write(squeltched);
			}

			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );

				int version = reader.ReadInt();
				m_Owner = reader.ReadMobile();
				squeltched=reader.ReadBool();
				if(m_Owner!=null)
				{
					m_Owner.Hidden=false;
					m_Owner.Squelched=squeltched;
				}
				this.Delete();
			}

			public override void OnAfterDelete()
			{
				base.OnAfterDelete();

				if ( m_Timer != null )
					m_Timer.Stop();
				if(m_Owner!=null)
					m_Owner.Squelched=squeltched;
			}

			private class InternalTimer : Timer
			{
				private InternalItem m_Item;
				private Mobile m_Owner;
				private bool speak;

				public InternalTimer( InternalItem item, TimeSpan duration, Mobile caster, bool talk ) : base( duration )
				{
					m_Item = item;
					m_Owner=caster;
					speak=talk;
				}

				protected override void OnTick()
				{
					m_Item.Delete();
					m_Owner.Squelched=speak;
					m_Owner.Hidden=false;
				}
			}
		}

		private class InternalTarget : Target
		{
			private DruidBlendWithForestSpell m_Owner;

			public InternalTarget( DruidBlendWithForestSpell owner ) : base( 12, true, TargetFlags.None )
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
