using System;
using Server;
using Server.Misc;
using Server.Spells;

namespace Server.ACC.CSS
{
	public abstract class CSpell : Spell
	{
		public virtual double RequiredSkill  { get{ return  0.0; } }
		public virtual int    RequiredMana   { get{ return    0; } }
		public virtual int    RequiredHealth { get{ return    0; } }
		public virtual int    RequiredTithing{ get{ return    0; } }
		public virtual double CastDelay      { get{ return -1.0; } }

		public CSpell( Mobile caster, Item scroll, SpellInfo info ) : base( caster, scroll, info )
		{
		}

		public override bool CheckCast()
		{
			if( SpellRestrictions.UseRestrictions )
				return SpellRestrictions.CheckRestrictions( Caster, this.GetType() );
			return true;
		}

		public override bool CheckFizzle()
		{
			if( CSkillCheck.UseDefaultSkills )
				return CSkillCheck.CheckSkill( Caster, this.GetType() ) ? base.CheckFizzle() : false;
			else
				return CSkillCheck.CheckSkill( Caster, this.GetType() );
		}

		public override TimeSpan GetCastDelay()
		{
			return CastDelay == -1 ? base.GetCastDelay() : TimeSpan.FromSeconds( CastDelay );
		}
	}
}