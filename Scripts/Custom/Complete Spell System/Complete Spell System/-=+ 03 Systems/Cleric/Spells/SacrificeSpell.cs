using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using PSys= Server.Engines.PartySystem;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Cleric
{
	public class ClericSacrificeSpell : ClericSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
		                                                "Sacrifice", "Adoleo",
		                                                //SpellCircle.First,
		                                                212,
		                                                9041
		                                               );
        
        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

		public override int RequiredTithing{ get{ return 5; } }
		public override double RequiredSkill{ get{ return 5.0; } }

		public static void Initialize()
		{
			PlayerEvent.HitByWeapon += new PlayerEvent.OnWeaponHit( InternalCallback );
		}

		public ClericSacrificeSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			if ( CheckSequence() )
			{
				if ( !Caster.CanBeginAction( typeof( ClericSacrificeSpell ) ) )
				{
					Caster.EndAction( typeof( ClericSacrificeSpell ) );
					Caster.PlaySound( 0x244 );
					Caster.FixedParticles( 0x3709, 1, 30, 9965, 1152, 0, EffectLayer.Waist );
					Caster.FixedParticles( 0x376A, 1, 30, 9502, 1152, 0, EffectLayer.Waist );
					Caster.SendMessage( "You stop sacrificing your essence for the well being of others." );
				}
				else
				{
					Caster.BeginAction( typeof( ClericSacrificeSpell ) );
					Caster.FixedParticles( 0x3709, 1, 30, 9965, 1153, 7, EffectLayer.Waist );
					Caster.FixedParticles( 0x376A, 1, 30, 9502, 1153, 3, EffectLayer.Waist );
					Caster.PlaySound( 0x244 );
					Caster.SendMessage( "You begin sacrificing your essence for the well being of others." );
				}
			}
			FinishSequence();
		}

		private static void InternalCallback( Mobile attacker, Mobile defender, int damage, WeaponAbility a )
		{
			if ( !defender.CanBeginAction( typeof( ClericSacrificeSpell ) ) )
			{
				PSys.Party p = PSys.Party.Get( defender );

				if ( p != null )
				{
					foreach( PSys.PartyMemberInfo info in p.Members )
					{
						Mobile m = info.Mobile;

						if ( m != defender && m != attacker && !m.Poisoned )
						{
							m.Heal( damage / 2 );
							m.PlaySound( 0x202 );
							m.FixedParticles( 0x376A, 1, 62, 9923, 3, 3, EffectLayer.Waist );
							m.FixedParticles( 0x3779, 1, 46, 9502, 5, 3, EffectLayer.Waist );
						}
					}
				}
			}
		}
	}
}
