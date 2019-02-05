using System;
using System.Collections;
using Server;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.Spells.Seventh;
using Server.Gumps;

namespace Server.ACC.CSS.Systems.Ranger
{
	public class RangerHuntersAimSpell : RangerSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
		                                                "Hunter's Aim", "Cu Ner Sinta",
		                                                //SpellCircle.Fourth,
		                                                212,
		                                                9041,
		                                                Reagent.Nightshade,
		                                                CReagent.SpringWater,
		                                                Reagent.Bloodmoss
		                                               );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

		public override double CastDelay{ get{ return 3.0; } }
		public override int RequiredMana{ get{ return 25; } }
		public override double RequiredSkill{ get{ return 50; } }

		private static Hashtable m_Table = new Hashtable();

		public RangerHuntersAimSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public static double GetScalar( Mobile m )
		{
			double val = 1.0;

			if ( m.CanBeginAction( typeof( RangerHuntersAimSpell ) ) )
				val = 1.5;

			return val;
		}
		public static bool HasEffect( Mobile m )
		{
			return ( m_Table[m] != null );
		}

		public static void RemoveEffect( Mobile m )
		{
			object[] mods = (object[])m_Table[m];

			if ( mods != null )
			{
				m.RemoveStatMod( ((StatMod)mods[0]).Name );
				m.RemoveStatMod( ((StatMod)mods[1]).Name );
				m.RemoveSkillMod( (SkillMod)mods[2] );
				m.RemoveSkillMod( (SkillMod)mods[3] );
			}

			m_Table.Remove( m );

			m.EndAction( typeof( RangerHuntersAimSpell ) );

			m.BodyMod = 0;
		}

		public override bool CheckCast()
		{
			if ( !base.CheckCast() )
			{
				return false;
			}
			else if ( !Caster.CanBeginAction( typeof( RangerHuntersAimSpell ) ) )
			{
				Caster.SendLocalizedMessage( 1005559 );
				return false;
			}


			return true;
		}

		public override void OnCast()
		{
			if ( !Caster.CanBeginAction( typeof( RangerHuntersAimSpell ) ) )
			{
				Caster.SendLocalizedMessage( 1005559 );
			}

			else if ( CheckSequence() )
			{
				object[] mods = new object[]
				{
					new StatMod( StatType.Dex, "[Ranger] Dex Offset", 5, TimeSpan.Zero ),
					new StatMod( StatType.Str, "[Ranger] Str Offset", 5, TimeSpan.Zero ),
					new DefaultSkillMod( SkillName.Archery, true, 20 ),
					new DefaultSkillMod( SkillName.Tactics, true, 20 ),

				};

				m_Table[Caster] = mods;

				Caster.AddStatMod( (StatMod)mods[0] );
				Caster.AddStatMod( (StatMod)mods[1] );
				Caster.AddSkillMod( (SkillMod)mods[2] );
				Caster.AddSkillMod( (SkillMod)mods[3] );

				double span = 1.0 * RangerHuntersAimSpell.GetScalar( Caster );
				new InternalTimer( Caster, TimeSpan.FromMinutes( (int)span ) ).Start();

				IMount mount = Caster.Mount;

				if ( mount != null )
					mount.Rider = null;


				Caster.BeginAction( typeof( RangerHuntersAimSpell ) );
			}
		}


		private class InternalTimer : Timer
		{
			private Mobile m_Owner;
			private DateTime m_Expire;

			public InternalTimer( Mobile owner, TimeSpan duration ) : base( TimeSpan.Zero, TimeSpan.FromSeconds( 0.1 ) )
			{
				m_Owner = owner;
				m_Expire = DateTime.Now + duration;

			}

			protected override void OnTick()
			{
				if ( DateTime.Now >= m_Expire )
				{
					RangerHuntersAimSpell.RemoveEffect( m_Owner );
					Stop();
				}
			}
		}
	}
}
