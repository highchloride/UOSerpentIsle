#region Header
//   Vorspire    _,-'/-'/  DungeonZone.cs
//   .      __,-; ,'( '/
//    \.    `-.__`-._`:_,-._       _ , . ``
//     `:-._,------' ` _,`--` -: `_ , ` ,' :
//        `---..__,,--'  (C) 2017  ` -'. -'
//        #  Vita-Nex [http://core.vita-nex.com]  #
//  {o)xxx|===============-   #   -===============|xxx(o}
//        #        The MIT License (MIT)          #
#endregion

#region References
using System;

using Server;
using Server.Targeting;

using VitaNex.InstanceMaps;
#endregion

namespace VitaNex.Dungeons
{
	public sealed class DungeonZone : InstanceRegion
	{
		public Dungeon Dungeon { get; set; }

		public DungeonZone(string name, InstanceMap map, int priority, params Rectangle3D[] area)
			: base(name, map, priority, area)
		{ }

		public DungeonZone(string name, InstanceMap map, InstanceRegion parent, params Rectangle3D[] area)
			: base(name, map, parent, area)
		{ }

		public DungeonZone(string name, InstanceMap map, Rectangle3D[] area, GenericReader reader)
			: base(name, map, area, reader)
		{ }

		public override bool AllowAutoClaim(Mobile m)
		{
			if (m != null && Dungeon != null)
			{
				return Dungeon.Options.Rules.AllowPets || !m.InRegion(this);
			}

			return base.AllowAutoClaim(m);
		}

		public override void OnEnter(Mobile m)
		{
			base.OnEnter(m);

			if (Dungeon != null && !Dungeon.Deleted)
			{
				Dungeon.OnEnter(this, m);
			}
		}

		public override void OnExit(Mobile m)
		{
			base.OnExit(m);

			if (Dungeon != null && !Dungeon.Deleted)
			{
				Dungeon.OnExit(this, m);
			}
		}

		public override void OnEnter(Item i)
		{
			base.OnEnter(i);

			if (Dungeon != null && !Dungeon.Deleted)
			{
				Dungeon.OnEnter(this, i);
			}
		}

		public override void OnExit(Item i)
		{
			base.OnExit(i);

			if (Dungeon != null && !Dungeon.Deleted)
			{
				Dungeon.OnExit(this, i);
			}
		}

		public override void OnMove(Point3D oldLocation, Item i)
		{
			if (Dungeon != null && !Dungeon.Deleted)
			{
				Dungeon.OnMove(oldLocation, i);
			}
		}

		public override void OnMove(Point3D oldLocation, Mobile m)
		{
			if (Dungeon != null && !Dungeon.Deleted)
			{
				Dungeon.OnMove(oldLocation, m);
			}
		}

		public override bool AllowBeneficial(Mobile from, Mobile target)
		{
			if (Dungeon != null && !Dungeon.Deleted && !Dungeon.AllowBeneficial(this, from, target))
			{
				return false;
			}

			return base.AllowBeneficial(from, target);
		}

#if ServUO
		public override bool AllowHarmful(Mobile from, IDamageable target)
#else
		public override bool AllowHarmful(Mobile from, Mobile target)
#endif
		{
			if (Dungeon != null && !Dungeon.Deleted && !Dungeon.AllowHarmful(this, from, target))
			{
				return false;
			}

			return base.AllowHarmful(from, target);
		}

		public override bool AllowHousing(Mobile from, Point3D p)
		{
			if (Dungeon != null && !Dungeon.Deleted && !Dungeon.AllowHousing(this, from, p))
			{
				return false;
			}

			return base.AllowHousing(from, p);
		}

		public override bool AllowSpawn()
		{
			if (Dungeon != null && !Dungeon.Deleted && !Dungeon.AllowSpawn(this))
			{
				return false;
			}

			return base.AllowSpawn();
		}

		public override bool CanUseStuckMenu(Mobile m)
		{
			if (Dungeon != null && !Dungeon.Deleted && !Dungeon.CanUseStuckMenu(this, m))
			{
				return false;
			}

			return base.CanUseStuckMenu(m);
		}

		public override void OnAggressed(Mobile aggressor, Mobile aggressed, bool criminal)
		{
			if (Dungeon != null && !Dungeon.Deleted)
			{
				Dungeon.OnAggressed(this, aggressor, aggressed, criminal);
			}

			base.OnAggressed(aggressor, aggressed, criminal);
		}

		public override bool AcceptsSpawnsFrom(Region region)
		{
			if (Dungeon != null && !Dungeon.Deleted && !Dungeon.AcceptsSpawnsFrom(this, region))
			{
				return false;
			}

			return base.AcceptsSpawnsFrom(region);
		}

		public override void AlterLightLevel(Mobile m, ref int global, ref int personal)
		{
			base.AlterLightLevel(m, ref global, ref personal);

			if (Dungeon != null && !Dungeon.Deleted)
			{
				Dungeon.OnAlterLightLevel(this, m, ref global, ref personal);
			}
		}

		public override bool CheckAccessibility(Item item, Mobile from)
		{
			if (Dungeon != null && !Dungeon.Deleted && !Dungeon.CheckAccessibility(this, item, from))
			{
				return false;
			}

			return base.CheckAccessibility(item, from);
		}

		public override TimeSpan GetLogoutDelay(Mobile m)
		{
			if (Dungeon != null && !Dungeon.Deleted)
			{
				return Dungeon.GetLogoutDelay(this, m);
			}

			return base.GetLogoutDelay(m);
		}

		public override bool OnDecay(Item item)
		{
			if (Dungeon != null && !Dungeon.Deleted && !Dungeon.OnDecay(this, item))
			{
				return false;
			}

			return base.OnDecay(item);
		}

		public override bool OnBeginSpellCast(Mobile m, ISpell s)
		{
			if (Dungeon != null && !Dungeon.Deleted && !Dungeon.OnBeginSpellCast(this, m, s))
			{
				return false;
			}

			return base.OnBeginSpellCast(m, s);
		}

		public override void OnBeneficialAction(Mobile helper, Mobile target)
		{
			if (Dungeon != null && !Dungeon.Deleted)
			{
				Dungeon.OnBeneficialAction(this, helper, target);
			}

			base.OnBeneficialAction(helper, target);
		}

#if ServUO
		public override bool OnCombatantChange(Mobile m, IDamageable oldMob, IDamageable newMob)
#else
		public override bool OnCombatantChange(Mobile m, Mobile oldMob, Mobile newMob)
#endif
		{
			if (Dungeon != null && !Dungeon.Deleted && !Dungeon.OnCombatantChange(this, m, oldMob, newMob))
			{
				return false;
			}

			return base.OnCombatantChange(m, oldMob, newMob);
		}

		public override void OnCriminalAction(Mobile m, bool message)
		{
			if (Dungeon != null && !Dungeon.Deleted)
			{
				Dungeon.OnCriminalAction(this, m, message);
			}

			base.OnCriminalAction(m, message);
		}

		public override bool OnDamage(Mobile m, ref int damage)
		{
			if (Dungeon != null && !Dungeon.Deleted && !Dungeon.OnDamage(this, m.FindMostRecentDamager(true), m, ref damage))
			{
				return false;
			}

			return base.OnDamage(m, ref damage);
		}

		public override bool OnBeforeDeath(Mobile m)
		{
			if (Dungeon != null && !Dungeon.Deleted && !Dungeon.OnBeforeDeath(this, m))
			{
				return false;
			}

			return base.OnBeforeDeath(m);
		}

		public override void OnDeath(Mobile m)
		{
			if (Dungeon != null && !Dungeon.Deleted)
			{
				Dungeon.OnDeath(this, m);
			}

			base.OnDeath(m);
		}

#if ServUO
		public override void OnDidHarmful(Mobile harmer, IDamageable harmed)
#else
		public override void OnDidHarmful(Mobile harmer, Mobile harmed)
#endif
		{
			if (Dungeon != null && !Dungeon.Deleted)
			{
				Dungeon.OnDidHarmful(this, harmer, harmed);
			}

			base.OnDidHarmful(harmer, harmed);
		}

		public override bool OnSingleClick(Mobile m, object o)
		{
			if (Dungeon != null && !Dungeon.Deleted && !Dungeon.OnSingleClick(this, m, o))
			{
				return false;
			}

			return base.OnSingleClick(m, o);
		}

		public override bool OnDoubleClick(Mobile m, object o)
		{
			if (Dungeon != null && !Dungeon.Deleted && !Dungeon.OnDoubleClick(this, m, o))
			{
				return false;
			}

			return base.OnDoubleClick(m, o);
		}

		public override void OnGotBeneficialAction(Mobile helper, Mobile target)
		{
			if (Dungeon != null && !Dungeon.Deleted)
			{
				Dungeon.OnGotBeneficialAction(this, helper, target);
			}

			base.OnGotBeneficialAction(helper, target);
		}

#if ServUO
		public override void OnGotHarmful(Mobile harmer, IDamageable harmed)
#else
		public override void OnGotHarmful(Mobile harmer, Mobile harmed)
#endif
		{
			if (Dungeon != null && !Dungeon.Deleted)
			{
				Dungeon.OnGotHarmful(this, harmer, harmed);
			}

			base.OnGotHarmful(harmer, harmed);
		}

		public override bool OnHeal(Mobile m, ref int heal)
		{
			// There is no way to retrieve the healer.
			// There is no HealStore implementation of the DamageStore feature.
			//Mobile from = null;

			if (Dungeon != null && !Dungeon.Deleted && !Dungeon.OnHeal(this, null, m, ref heal))
			{
				return false;
			}

			return base.OnHeal(m, ref heal);
		}

		public override void OnLocationChanged(Mobile m, Point3D oldLocation)
		{
			if (Dungeon != null && !Dungeon.Deleted)
			{
				Dungeon.OnLocationChanged(this, m, oldLocation);
			}

			base.OnLocationChanged(m, oldLocation);
		}

		public override bool OnMoveInto(Mobile m, Direction d, Point3D newLocation, Point3D oldLocation)
		{
			if (Dungeon != null && !Dungeon.Deleted && !Dungeon.OnMoveInto(this, m, d, newLocation, oldLocation))
			{
				return false;
			}

			return base.OnMoveInto(m, d, newLocation, oldLocation);
		}

		public override bool OnResurrect(Mobile m)
		{
			if (Dungeon != null && !Dungeon.Deleted && !Dungeon.OnResurrect(this, m))
			{
				return false;
			}

			return base.OnResurrect(m);
		}

		public override bool OnSkillUse(Mobile m, int skill)
		{
			if (Dungeon != null && !Dungeon.Deleted && !Dungeon.OnSkillUse(this, m, skill))
			{
				return false;
			}

			return base.OnSkillUse(m, skill);
		}

		public override void OnSpeech(SpeechEventArgs args)
		{
			if (Dungeon != null && !Dungeon.Deleted)
			{
				Dungeon.OnSpeech(this, args);
			}

			base.OnSpeech(args);
		}

		public override void OnSpellCast(Mobile m, ISpell s)
		{
			if (Dungeon != null && !Dungeon.Deleted)
			{
				Dungeon.OnSpellCast(this, m, s);
			}

			base.OnSpellCast(m, s);
		}

		public override bool OnTarget(Mobile m, Target t, object o)
		{
			if (Dungeon != null && !Dungeon.Deleted && !Dungeon.OnTarget(this, m, t, o))
			{
				return false;
			}

			return base.OnTarget(m, t, o);
		}

		public override void SpellDamageScalar(Mobile caster, Mobile target, ref double damage)
		{
			base.SpellDamageScalar(caster, target, ref damage);

			if (Dungeon != null && !Dungeon.Deleted)
			{
				Dungeon.SpellDamageScalar(this, caster, target, ref damage);
			}
		}

		public override void OnRegister()
		{
			base.OnRegister();

			if (Dungeon != null && !Dungeon.Deleted)
			{
				Dungeon.Zones.AddOrReplace(this);
			}
		}

		public override void OnUnregister()
		{
			base.OnUnregister();

			if (Dungeon != null)
			{
				Dungeon.Zones.Remove(this);
			}
		}

		protected override void OnDelete()
		{
			base.OnDelete();

			if (Dungeon == null)
			{
				return;
			}

			Dungeon.Zones.Remove(this);
			Dungeon = null;
		}

		protected override void OnAfterDelete()
		{
			base.OnAfterDelete();

			if (Dungeon == null)
			{
				return;
			}

			Dungeon.Zones.Remove(this);
			Dungeon = null;
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.SetVersion(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			reader.GetVersion();
		}
	}
}
