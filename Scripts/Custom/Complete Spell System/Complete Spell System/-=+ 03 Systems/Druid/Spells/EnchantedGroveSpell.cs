using System;
using Server.Targeting;
using Server.Network;
using Server.Misc;
using Server.Items;
using System.Collections;
using Server.Mobiles;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Druid
{
	public class DruidEnchantedGroveSpell : DruidSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
		                                                "Enchanted Grove", "En Ante Ohm Sepa",
		                                                //SpellCircle.Eighth,
		                                                266,
		                                                9040,
		                                                false,
		                                                Reagent.MandrakeRoot,
		                                                CReagent.PetrafiedWood,
		                                                CReagent.SpringWater
		                                               );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Eighth; }
        }

		public override double CastDelay{ get{ return 7.0; } }
		public override double RequiredSkill{ get{ return 95.0; } }
		public override int RequiredMana{ get{ return 60; } }

		public DruidEnchantedGroveSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

		public void Target( IPoint3D p )
		{
			if ( !Caster.CanSee( p ) )
			{
				Caster.SendLocalizedMessage( 500237 ); // Target can not be seen.
			}
			else if ( SpellHelper.CheckTown( p, Caster ) && CheckSequence() )
			{
				if(this.Scroll!=null)
					Scroll.Consume();
				SpellHelper.Turn( Caster, p );

				SpellHelper.GetSurfaceTop( ref p );

				Effects.PlaySound( p, Caster.Map, 0x2 );

				Point3D loc = new Point3D( p.X, p.Y, p.Z );
				int grovex;
				int grovey;
				int grovez;
				InternalItem groveStone = new InternalItem( Caster.Location, Caster.Map, Caster );
				grovex=loc.X;
				grovey=loc.Y;
				grovez=loc.Z;
				groveStone.ItemID=0x08E3;
				groveStone.Name="sacred stone";
				Point3D stonexyz = new Point3D(grovex,grovey,grovez);
				groveStone.MoveToWorld( stonexyz, Caster.Map );

				InternalItem grovea = new InternalItem( Caster.Location, Caster.Map, Caster );
				grovex=loc.X-2;
				grovey=loc.Y-2;
				grovez=loc.Z;
				grovea.ItemID=3290;
				Point3D grovexyz = new Point3D(grovex,grovey,grovez);
				grovea.MoveToWorld( grovexyz, Caster.Map );

				InternalItem grovec = new InternalItem( Caster.Location, Caster.Map, Caster );
				grovex=loc.X;
				grovey=loc.Y-3;
				grovez=loc.Z;
				grovec.ItemID=3293;
				Point3D grovexyzb = new Point3D(grovex,grovey,grovez);
				grovec.MoveToWorld( grovexyzb, Caster.Map );

				InternalItem groved = new InternalItem( Caster.Location, Caster.Map, Caster );
				groved.ItemID=3290;
				grovex=loc.X+2;
				grovey=loc.Y-2;
				grovez=loc.Z;
				Point3D grovexyzc = new Point3D(grovex,grovey,grovez);
				groved.MoveToWorld( grovexyzc, Caster.Map );
				InternalItem grovee = new InternalItem( Caster.Location, Caster.Map, Caster );
				grovex=loc.X+3;
				grovee.ItemID=3290;
				grovey=loc.Y;
				grovez=loc.Z;
				Point3D grovexyzd = new Point3D(grovex,grovey,grovez);
				grovee.MoveToWorld( grovexyzd, Caster.Map );
				InternalItem grovef = new InternalItem( Caster.Location, Caster.Map, Caster );
				grovef.ItemID=3293;
				grovex=loc.X+2;
				grovey=loc.Y+2;
				grovez=loc.Z;
				Point3D grovexyze = new Point3D(grovex,grovey,grovez);
				grovef.MoveToWorld( grovexyze, Caster.Map );
				InternalItem groveg = new InternalItem( Caster.Location, Caster.Map, Caster );
				grovex=loc.X;
				groveg.ItemID=3290;
				grovey=loc.Y+3;
				grovez=loc.Z;
				Point3D grovexyzf = new Point3D(grovex,grovey,grovez);
				groveg.MoveToWorld( grovexyzf, Caster.Map );
				InternalItem groveh = new InternalItem( Caster.Location, Caster.Map, Caster );
				grovex=loc.X-2;
				groveh.ItemID=3293;
				grovey=loc.Y+2;
				grovez=loc.Z;
				Point3D grovexyzg = new Point3D(grovex,grovey,grovez);
				groveh.MoveToWorld( grovexyzg, Caster.Map );
				InternalItem grovei = new InternalItem( Caster.Location, Caster.Map, Caster );
				grovex=loc.X-3;
				grovei.ItemID=3293;
				grovey=loc.Y;
				grovez=loc.Z;
				Point3D grovexyzh = new Point3D(grovex,grovey,grovez);
				grovei.MoveToWorld( grovexyzh, Caster.Map );
				InternalItem leavesa = new InternalItem( Caster.Location, Caster.Map, Caster );
				grovex=loc.X-2;
				grovey=loc.Y-2;
				grovez=loc.Z;
				leavesa.ItemID=3291;
				Point3D leafxyz = new Point3D(grovex,grovey,grovez);
				leavesa.MoveToWorld( leafxyz, Caster.Map );

				InternalItem leavesc = new InternalItem( Caster.Location, Caster.Map, Caster );
				grovex=loc.X;
				grovey=loc.Y-3;
				grovez=loc.Z;
				leavesc.ItemID=3294;
				Point3D leafxyzb = new Point3D(grovex,grovey,grovez);
				leavesc.MoveToWorld( leafxyzb, Caster.Map );

				InternalItem leavesd = new InternalItem( Caster.Location, Caster.Map, Caster );
				leavesd.ItemID=3291;
				grovex=loc.X+2;
				grovey=loc.Y-2;
				grovez=loc.Z;
				Point3D leafxyzc = new Point3D(grovex,grovey,grovez);
				leavesd.MoveToWorld( leafxyzc, Caster.Map );
				InternalItem leavese = new InternalItem( Caster.Location, Caster.Map, Caster );
				grovex=loc.X+3;
				leavese.ItemID=3291;
				grovey=loc.Y;
				grovez=loc.Z;
				Point3D leafxyzd = new Point3D(grovex,grovey,grovez);
				leavese.MoveToWorld( leafxyzd, Caster.Map );
				InternalItem leavesf = new InternalItem( Caster.Location, Caster.Map, Caster );
				leavesf.ItemID=3294;
				grovex=loc.X+2;
				grovey=loc.Y+2;
				grovez=loc.Z;
				Point3D leafxyze = new Point3D(grovex,grovey,grovez);
				leavesf.MoveToWorld( leafxyze, Caster.Map );
				InternalItem leavesg = new InternalItem( Caster.Location, Caster.Map, Caster );
				grovex=loc.X;
				leavesg.ItemID=3291;
				grovey=loc.Y+3;
				grovez=loc.Z;
				Point3D leafxyzf = new Point3D(grovex,grovey,grovez);
				leavesg.MoveToWorld( leafxyzf, Caster.Map );
				InternalItem leavesh = new InternalItem( Caster.Location, Caster.Map, Caster );
				grovex=loc.X-2;
				leavesh.ItemID=3294;
				grovey=loc.Y+2;
				grovez=loc.Z;
				Point3D leafxyzg = new Point3D(grovex,grovey,grovez);
				leavesh.MoveToWorld( leafxyzg, Caster.Map );
				InternalItem leavesi = new InternalItem( Caster.Location, Caster.Map, Caster );
				grovex=loc.X-3;
				leavesi.ItemID=3294;
				grovey=loc.Y;
				grovez=loc.Z;
				Point3D leafxyzh = new Point3D(grovex,grovey,grovez);
				leavesi.MoveToWorld( leafxyzh, Caster.Map );
			}

			FinishSequence();
		}

		[DispellableField]
		private class InternalItem : Item
		{
			private Timer m_Timer;
			private Timer m_Bless;
			private DateTime m_End;
			private Mobile m_Caster;

			public override bool BlocksFit{ get{ return true; } }

			public InternalItem( Point3D loc, Map map, Mobile caster ) : base( 0x3274 )
			{
				Visible = false;
				Movable = false;
				MoveToWorld( loc, map );
				m_Caster=caster;

				if ( caster.InLOS( this ) )
					Visible = true;
				else
					Delete();

				if ( Deleted )
					return;

				m_Timer = new InternalTimer( this, TimeSpan.FromSeconds( 30.0 ) );
				m_Timer.Start();
				m_Bless = new BlessTimer( this, m_Caster );
				m_Bless.Start();

				m_End = DateTime.Now + TimeSpan.FromSeconds( 30.0 );
			}

			public InternalItem( Serial serial ) : base( serial )
			{
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );
				writer.Write( (int) 0 ); // version
				writer.Write( m_End - DateTime.Now );
			}

			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );
				int version = reader.ReadInt();
				TimeSpan duration = reader.ReadTimeSpan();

				m_Timer = new InternalTimer( this, duration );
				m_Timer.Start();

				m_End = DateTime.Now + duration;
			}

			public override void OnAfterDelete()
			{
				base.OnAfterDelete();

				if ( m_Timer != null )
					m_Timer.Stop();
			}

			private class InternalTimer : Timer
			{
				private InternalItem m_Item;

				public InternalTimer( InternalItem item, TimeSpan duration ) : base( duration )
				{
					m_Item = item;
				}

				protected override void OnTick()
				{
					m_Item.Delete();
				}
			}
		}
		private class InternalTarget : Target
		{
			private DruidEnchantedGroveSpell m_Owner;

			public InternalTarget( DruidEnchantedGroveSpell owner ) : base( 12, true, TargetFlags.None )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is IPoint3D )
					m_Owner.Target( (IPoint3D)o );
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
		private class BlessTimer : Timer
		{
			private Item m_DruidEnchantedGrove;
			private Mobile m_Caster;
			private DateTime m_Duration;

			private static Queue m_Queue = new Queue();

			public BlessTimer( Item ap, Mobile ca ) : base( TimeSpan.FromSeconds( 0.5 ), TimeSpan.FromSeconds( 1.0 ) )
			{
				Priority = TimerPriority.FiftyMS;

				m_DruidEnchantedGrove = ap;
				m_Caster=ca;
				m_Duration = DateTime.Now + TimeSpan.FromSeconds( 15.0 + ( Utility.RandomDouble() * 15.0 ) );
			}

			protected override void OnTick()
			{
				if ( m_DruidEnchantedGrove.Deleted )
					return;

				if ( DateTime.Now > m_Duration )
				{

					Stop();
				}
				else
				{
					ArrayList list = new ArrayList();

					foreach ( Mobile m in m_DruidEnchantedGrove.GetMobilesInRange( 5 ) )
					{
						if ( m.Player && m.Karma >= 0 && m.Alive )
							list.Add( m );
					}

					for ( int i = 0; i < list.Count; ++i )
					{
						Mobile m = (Mobile)list[i];
						bool friendly = true;

						for ( int j = 0; friendly && j < m_Caster.Aggressors.Count; ++j )
							friendly = ( ((AggressorInfo)m_Caster.Aggressors[j]).Attacker != m );

						for ( int j = 0; friendly && j < m_Caster.Aggressed.Count; ++j )
							friendly = ( ((AggressorInfo)m_Caster.Aggressed[j]).Defender != m );

						if ( friendly )
						{
							m.FixedEffect( 0x37C4, 1, 12, 1109, 3 ); // At player
							m.Mana += (1 + (m_Caster.Karma / 1000));
							m.Hits += (1 + (m_Caster.Karma / 1000));
						}
					}
				}
			}
		}
	}
}
