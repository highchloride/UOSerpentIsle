using System;
using System.Collections;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Spells.Seventh;

namespace Server.ACC.CSS.Systems.Cleric
{
	public class ClericAngelicFaithSpell : ClericSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
		                                                "Angelic Faith", "Angelus Terum",
		                                                //SpellCircle.Eighth,
		                                                212,
		                                                9041
		                                               );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Eighth; }
        }

		public override int RequiredTithing{ get{ return 100; } }
		public override double RequiredSkill{ get{ return 80.0; } }

		private static Hashtable m_Table = new Hashtable();

		public ClericAngelicFaithSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
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
				m.RemoveStatMod( ((StatMod)mods[2]).Name );
				m.RemoveSkillMod( (SkillMod)mods[3] );
				m.RemoveSkillMod( (SkillMod)mods[4] );
				m.RemoveSkillMod( (SkillMod)mods[5] );
			}

			m_Table.Remove( m );

			m.EndAction( typeof( ClericAngelicFaithSpell ) );

			m.BodyMod = 0;
		}

		public override bool CheckCast()
		{
            if (!TransformationSpellHelper.CheckCast(Caster, this))
                return false;

            return base.CheckCast();
		}

		public override void OnCast()
		{
            TransformationSpellHelper.OnCast(Caster, this);

            if ( CheckSequence() )
			{
                RemoveEffect(Caster);

				object[] mods = new object[]
				{
					new StatMod( StatType.Str, "[Cleric] Str Offset", 20, TimeSpan.Zero ),
					new StatMod( StatType.Dex, "[Cleric] Dex Offset", 20, TimeSpan.Zero ),
					new StatMod( StatType.Int, "[Cleric] Int Offset", 20, TimeSpan.Zero ),
					new DefaultSkillMod( SkillName.Macing, true, 20 ),
					new DefaultSkillMod( SkillName.Healing, true, 20 ),
					new DefaultSkillMod( SkillName.Anatomy, true, 20 )
				};

				m_Table[Caster] = mods;

				Caster.AddStatMod( (StatMod)mods[0] );
				Caster.AddStatMod( (StatMod)mods[1] );
				Caster.AddStatMod( (StatMod)mods[2] );
				Caster.AddSkillMod( (SkillMod)mods[3] );
				Caster.AddSkillMod( (SkillMod)mods[4] );
				Caster.AddSkillMod( (SkillMod)mods[5] );

				double span = 10.0 * ClericDivineFocusSpell.GetScalar( Caster );
				new InternalTimer( Caster, TimeSpan.FromMinutes( (int)span ) ).Start();

				IMount mount = Caster.Mount;

				if ( mount != null )
					mount.Rider = null;

				Caster.BodyMod = 123;
				Caster.BeginAction( typeof( ClericAngelicFaithSpell ) );
				Caster.PlaySound( 0x165 );
				Caster.FixedParticles( 0x3728, 1, 13, 0x480, 92, 3, EffectLayer.Head );
			}

            FinishSequence();
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
					ClericAngelicFaithSpell.RemoveEffect( m_Owner );
					Stop();
				}
			}
		}
	}
}
