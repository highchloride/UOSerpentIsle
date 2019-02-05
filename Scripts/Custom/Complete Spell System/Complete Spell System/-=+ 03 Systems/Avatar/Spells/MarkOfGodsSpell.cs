using System;
using Server.Items;
using Server.Targeting;
using Server.Network;
using Server.Regions;
using Server.Mobiles;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Avatar
{
	public class AvatarMarkOfGodsSpell : AvatarSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
		                                                "Mark Of Gods", "Britemus Por Ylemis",
		                                                //SpellCircle.Fifth,
		                                                -1,
		                                                9002
		                                               );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fifth; }
        }

		public override double RequiredSkill{ get{ return 20; } }
		public override int RequiredMana{ get{ return 10; } }
		public override int RequiredTithing{ get{ return 10; } }

		public AvatarMarkOfGodsSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

		public override bool CheckCast()
		{
			if ( !base.CheckCast() )
				return false;

			return SpellHelper.CheckTravel( Caster, TravelCheckType.Mark );
		}

		public void Target( RecallRune rune )
		{
			if ( !Caster.CanSee( rune ) )
			{
				Caster.SendLocalizedMessage( 500237 ); // Target can not be seen.
			}
			else if ( !SpellHelper.CheckTravel( Caster, TravelCheckType.Mark ) )
			{
			}
			else if ( SpellHelper.CheckMulti( Caster.Location, Caster.Map, !Core.AOS ) )
			{
				Caster.SendLocalizedMessage( 501942 ); // That location is blocked.
			}
			else if ( !rune.IsChildOf( Caster.Backpack ) )
			{
				Caster.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1062422 ); // You must have this rune in your backpack in order to mark it.
			}
			else if ( CheckSequence() )
			{
				rune.Mark( Caster );
				Caster.FixedParticles( 0x376A, 9, 32, 0x13AF, EffectLayer.Waist );
				Caster.PlaySound( 0x1FA );
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private AvatarMarkOfGodsSpell m_Owner;

			public InternalTarget( AvatarMarkOfGodsSpell owner ) : base( 12, false, TargetFlags.None )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is RecallRune )
				{
					m_Owner.Target( (RecallRune) o );
				}
				else
				{
					from.Send( new MessageLocalized( from.Serial, from.Body, MessageType.Regular, 0x3B2, 3, 501797, from.Name, "" ) ); // I cannot mark that object.
				}
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}
