#region Header
//   Vorspire    _,-'/-'/  Dungeon_Actions.cs
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
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Targeting;

using VitaNex.Network;
#endregion

namespace VitaNex.Dungeons
{
	public abstract partial class Dungeon
	{
		public void Teleport(Mobile m, Point3D destLoc, Map destMap)
		{
			if (m == null || m.Deleted || destLoc == Point3D.Zero || destMap == null || destMap == Server.Map.Internal)
			{
				return;
			}

			var oldLoc = m.Location;
			var oldMap = m.Map;

			if (m.Player)
			{
				ScreenFX.FadeOut.Send(m);
			}

			PlaySound(m, Options.Sounds.Teleport);

			if (m.Player && (destMap != Map || Options.Rules.AllowPets))
			{
				BaseCreature.TeleportPets(m, destLoc, destMap, false);
			}

			m.MoveToWorld(destLoc, destMap);

			if (destMap == Map && m.Player)
			{
				CheckDismount(m);

				if (!Options.Rules.AllowPets && m is PlayerMobile)
				{
					StablePets((PlayerMobile)m);
				}
			}

			OnTeleported(m, oldLoc, oldMap);
		}

		protected virtual void OnTeleported(Mobile m, Point3D oldLocation, Map oldMap)
		{
			if (m == null || m.Deleted)
			{
				return;
			}

			if (!m.Hidden)
			{
				Effects.SendLocationParticles(
					EffectItem.Create(oldLocation, oldMap, EffectItem.DefaultDuration),
					0x3728,
					10,
					10,
					5023);
				Effects.SendLocationParticles(
					EffectItem.Create(m.Location, m.Map, EffectItem.DefaultDuration),
					0x3728,
					10,
					10,
					5023);
			}

			if (m is PlayerMobile)
			{
				OnTeleported((PlayerMobile)m, oldLocation, oldMap);
			}
		}

		protected virtual void OnTeleported(PlayerMobile pm, Point3D oldLocation, Map oldMap)
		{
			if (pm == null || pm.Deleted)
			{
				return;
			}

			Group.AddOrReplace(pm);

			Map.RecordBounce(pm, oldLocation, oldMap);
		}

		public virtual void OnEnterDungeon(Mobile m)
		{
			if (m == null || m.Deleted)
			{
				return;
			}

			if (m is PlayerMobile)
			{
				var pm = (PlayerMobile)m;

				ActiveGroup.AddOrReplace(pm);

				if (ActiveGroup.Count > 0 && ActiveLeader == null)
				{
					ActiveLeader = pm;
				}

				DungeonUI.DisplayTo(pm, false, this);
			}

			if (m.Player)
			{
				CheckDismount(m);

				if (!Options.Rules.AllowPets && m is PlayerMobile)
				{
					StablePets((PlayerMobile)m);
				}
			}

			m.Delta(MobileDelta.Noto);
		}

		public virtual void OnExitDungeon(Mobile m)
		{
			if (m == null)
			{
				return;
			}

			if (MobileSpawns.Remove(m))
			{
				OnSpawnDeactivate(m);
			}

			if (m.Deleted)
			{
				return;
			}

			if (m is PlayerMobile)
			{
				var pm = (PlayerMobile)m;

				ActiveGroup.Remove(pm);

				if (ActiveGroup.Count > 0 && ActiveLeader == pm)
				{
					ActiveLeader = ActiveGroup[0];
				}

				pm.CloseGump(typeof(DungeonUI));
				pm.CloseGump(typeof(DungeonLootUI));

				if (pm.Map == null || pm.Map == Server.Map.Internal)
				{
					StablePets(pm);
				}
				else
				{
					SummonPets(pm);
				}
			}

			m.Delta(MobileDelta.Noto);
			m.ProcessDelta();
		}

		public virtual void StablePets(PlayerMobile m)
		{
			if (Map != null)
			{
				Map.StablePets(m);
			}
		}

		public virtual void SummonPets(PlayerMobile m)
		{
			if (Map != null)
			{
				Map.SummonPets(m);
			}
		}

		public bool CheckMounted(Mobile m, bool dismount)
		{
			return CheckMounted(GetZone(m), m, dismount);
		}

		public virtual bool CheckMounted(DungeonZone zone, Mobile m, bool dismount)
		{
			if (m == null || m.Deleted || !m.Mounted || m.Mount == null)
			{
				return false;
			}

			if (dismount)
			{
				CheckDismount(zone, m);
			}

			return true;
		}

		public bool CheckDismount(Mobile m)
		{
			return CheckDismount(GetZone(m), m);
		}

		public virtual bool CheckDismount(DungeonZone zone, Mobile m)
		{
			if (m == null || m.Deleted || IsSpawn(m))
			{
				return false;
			}

			if (m.AccessLevel > AccessLevel.Counselor)
			{
				return false;
			}

			if (m.Mounted && m.Mount != null)
			{
				bool dismount;

				if (m.Mount is EtherealMount)
				{
					dismount = !Options.Rules.CanMount;
				}
				else
				{
					dismount = !Options.Rules.CanMount;

					if (!dismount)
					{
						dismount = !Options.Rules.AllowPets;
					}
				}

				if (dismount)
				{
					Dismount(zone, m);
				}
			}

			if (m.Flying && !Options.Rules.CanFly)
			{
				m.Flying = false;
			}

			return true;
		}

		public void Dismount(Mobile m)
		{
			Dismount(GetZone(m), m);
		}

		public virtual void Dismount(DungeonZone zone, Mobile m)
		{
			if (m == null || m.Deleted || !m.Mounted || m.Mount == null)
			{
				return;
			}

			var mount = m.Mount;

			mount.Rider = null;

			OnDismounted(zone, m, mount);
		}

		protected virtual void OnDismounted(DungeonZone zone, Mobile m, IMount mount)
		{
			if (m != null && !m.Deleted && mount != null)
			{
				m.SendMessage("You have been dismounted.");
			}
		}

		public bool AllowSpawn(DungeonZone zone)
		{
			if (CheckAllowSpawn(zone))
			{
				OnAllowSpawnAccept(zone);
				return true;
			}

			OnAllowSpawnDeny(zone);
			return false;
		}

		public virtual bool CheckAllowSpawn(DungeonZone zone)
		{
			return Options.Rules.AllowSpawn;
		}

		protected virtual void OnAllowSpawnAccept(DungeonZone zone)
		{ }

		protected virtual void OnAllowSpawnDeny(DungeonZone zone)
		{ }

		public bool CanMoveThrough(Mobile m, IEntity e)
		{
			return CanMoveThrough(GetZone(m), m, e);
		}

		public bool CanMoveThrough(DungeonZone zone, Mobile m, IEntity e)
		{
			if (CheckCanMoveThrough(zone, m, e))
			{
				OnCanMoveThroughAccept(zone, m, e);
				return true;
			}

			OnCanMoveThroughDeny(zone, m, e);
			return false;
		}

		public virtual bool CheckCanMoveThrough(DungeonZone zone, Mobile m, IEntity e)
		{
			if (m == null || m.Deleted)
			{
				return false;
			}

			if (m.AccessLevel > AccessLevel.Counselor)
			{
				return true;
			}

#if NEWENTITY
			return e == null || e.Deleted || Options.Rules.CanMoveThrough;
#else
			return e == null || (e is Mobile && ((Mobile)e).Deleted) || (e is Item && ((Item)e).Deleted) || Options.Rules.CanMoveThrough;
#endif
		}

		protected virtual void OnCanMoveThroughAccept(DungeonZone zone, Mobile m, IEntity e)
		{ }

		protected virtual void OnCanMoveThroughDeny(DungeonZone zone, Mobile m, IEntity e)
		{ }

		public bool AllowHousing(Mobile m, Point3D p)
		{
			return AllowHousing(GetZone(m), m, p);
		}

		public bool AllowHousing(DungeonZone zone, Mobile m, Point3D p)
		{
			if (CheckAllowHousing(zone, m, p))
			{
				OnAllowHousingAccept(zone, m, p);
				return true;
			}

			OnAllowHousingDeny(zone, m, p);
			return false;
		}

		public virtual bool CheckAllowHousing(DungeonZone zone, Mobile m, Point3D p)
		{
			if (m == null || m.Deleted)
			{
				return false;
			}

			if (m.AccessLevel > AccessLevel.Counselor)
			{
				return true;
			}

			return Options.Rules.AllowHousing;
		}

		protected virtual void OnAllowHousingAccept(DungeonZone zone, Mobile m, Point3D p)
		{ }

		protected virtual void OnAllowHousingDeny(DungeonZone zone, Mobile m, Point3D p)
		{
			if (m != null && !m.Deleted)
			{
				m.SendMessage("You can not place structures here at this time.");
			}
		}

		public bool CanUseStuckMenu(Mobile m)
		{
			return CanUseStuckMenu(GetZone(m), m);
		}

		public bool CanUseStuckMenu(DungeonZone zone, Mobile m)
		{
			if (CheckCanUseStuckMenu(zone, m))
			{
				OnCanUseStuckMenuAccept(zone, m);
				return true;
			}

			OnCanUseStuckMenuDeny(zone, m);
			return false;
		}

		public virtual bool CheckCanUseStuckMenu(DungeonZone zone, Mobile m)
		{
			if (m == null || m.Deleted)
			{
				return false;
			}

			if (m.AccessLevel > AccessLevel.Counselor)
			{
				return true;
			}

			return Options.Rules.CanUseStuckMenu;
		}

		protected virtual void OnCanUseStuckMenuAccept(DungeonZone zone, Mobile m)
		{ }

		protected virtual void OnCanUseStuckMenuDeny(DungeonZone zone, Mobile m)
		{
			if (m != null && !m.Deleted)
			{
				m.SendMessage("You can not use the stuck menu at this time.");
			}
		}

		public bool OnBeforeDeath(DungeonZone zone, Mobile m)
		{
			if (CheckDeath(zone, m))
			{
				OnDeathAccept(zone, m);
				return true;
			}

			OnDeathDeny(zone, m);
			return false;
		}

		public virtual bool CheckDeath(DungeonZone zone, Mobile m)
		{
			if (m == null || m.Deleted || m.Blessed)
			{
				return false;
			}

			if (m.AccessLevel > AccessLevel.Counselor)
			{
				return false;
			}

			return !m.Player || Options.Rules.CanDie;
		}

		protected virtual void OnDeathAccept(DungeonZone zone, Mobile m)
		{ }

		protected virtual void OnDeathDeny(DungeonZone zone, Mobile m)
		{ }

		public virtual void OnDeath(DungeonZone zone, Mobile m)
		{ }

		public bool OnSkillUse(DungeonZone zone, Mobile user, int skill)
		{
			if (CheckSkillUse(zone, user, skill))
			{
				OnSkillUseAccept(zone, user, skill);
				return true;
			}

			OnSkillUseDeny(zone, user, skill);
			return false;
		}

		public virtual bool CheckSkillUse(DungeonZone zone, Mobile user, int skill)
		{
			if (user == null || user.Deleted || skill < 0)
			{
				return false;
			}

			if (user.AccessLevel > AccessLevel.Counselor)
			{
				return true;
			}

			return !Options.Restrictions.Skills.IsRestricted(skill);
		}

		protected virtual void OnSkillUseAccept(DungeonZone zone, Mobile user, int skill)
		{ }

		protected virtual void OnSkillUseDeny(DungeonZone zone, Mobile user, int skill)
		{
			if (user != null && !user.Deleted && skill >= 0)
			{
				user.SendMessage("You can not use that skill at this time.");
			}
		}

		public bool OnBeginSpellCast(DungeonZone zone, Mobile caster, ISpell spell)
		{
			if (CheckSpellCast(zone, caster, spell))
			{
				OnSpellCastAccept(zone, caster, spell);
				return true;
			}

			OnSpellCastDeny(zone, caster, spell);
			return false;
		}

		public virtual bool CheckSpellCast(DungeonZone zone, Mobile caster, ISpell spell)
		{
			if (caster == null || caster.Deleted || spell == null)
			{
				return false;
			}

			if (caster.AccessLevel > AccessLevel.Counselor)
			{
				return true;
			}

			if (!(spell is Spell))
			{
				return true;
			}

			return !Options.Restrictions.Spells.IsRestricted((Spell)spell);
		}

		protected virtual void OnSpellCastAccept(DungeonZone zone, Mobile caster, ISpell spell)
		{ }

		protected virtual void OnSpellCastDeny(DungeonZone zone, Mobile caster, ISpell spell)
		{
			if (caster != null && !caster.Deleted && spell != null)
			{
				caster.SendMessage("You can not use that spell at this time.");
			}
		}
		
		public bool OnDamage(DungeonZone zone, Mobile attacker, Mobile damaged, ref int damage)
		{
			if (attacker.IsControlled<PlayerMobile>())
			{
				damage = (int)(damage * Options.PetGiveDamageScalar);
			}

			if (damaged.IsControlled<PlayerMobile>())
			{
				damage = (int)(damage * Options.PetTakeDamageScalar);
			}

			if (CheckDamage(zone, attacker, damaged, ref damage))
			{
				OnDamageAccept(zone, attacker, damaged, ref damage);
				return true;
			}

			OnDamageDeny(zone, attacker, damaged, ref damage);
			return false;
		}

		public virtual bool CheckDamage(DungeonZone zone, Mobile attacker, Mobile damaged, ref int damage)
		{
			if (damaged == null || damaged.Deleted)
			{
				return false;
			}

			if (attacker != null && attacker.AccessLevel > AccessLevel.Counselor)
			{
				return true;
			}

			return Options.Rules.CanBeDamaged;
		}

		protected virtual void OnDamageAccept(DungeonZone zone, Mobile attacker, Mobile damaged, ref int damage)
		{ }

		public virtual void OnDamageDeny(DungeonZone zone, Mobile attacker, Mobile damaged, ref int damage)
		{
			if (damaged != null && !damaged.Deleted && damage > 0)
			{
				damaged.SendMessage("You have been spared damage, this time...");
			}
		}

		public bool OnHeal(DungeonZone zone, Mobile healer, Mobile healed, ref int heal)
		{
			if (CheckHeal(zone, healer, healed, ref heal))
			{
				OnHealAccept(zone, healer, healed, ref heal);
				return true;
			}

			OnHealDeny(zone, healer, healed, ref heal);
			return false;
		}

		public virtual bool CheckHeal(DungeonZone zone, Mobile healer, Mobile healed, ref int heal)
		{
			if (healed == null || healed.Deleted)
			{
				return false;
			}

			if (healer != null && healer.AccessLevel > AccessLevel.Counselor)
			{
				return true;
			}

			return Options.Rules.CanHeal;
		}

		/// <summary>
		/// </summary>
		/// <param name="zone"></param>
		/// <param name="healer">CONSTANT NULL</param>
		/// <param name="healed"></param>
		/// <param name="heal"></param>
		protected virtual void OnHealAccept(DungeonZone zone, Mobile healer, Mobile healed, ref int heal)
		{ }

		/// <summary>
		/// </summary>
		/// <param name="zone"></param>
		/// <param name="healer">CONSTANT NULL</param>
		/// <param name="healed"></param>
		/// <param name="heal"></param>
		protected virtual void OnHealDeny(DungeonZone zone, Mobile healer, Mobile healed, ref int heal)
		{
			if (healed != null && !healed.Deleted && heal > 0)
			{
				healed.SendMessage("You can not be healed at this time.");
			}
		}

		public bool OnResurrect(DungeonZone zone, Mobile m)
		{
			if (CheckResurrect(zone, m))
			{
				OnResurrectAccept(zone, m);
				return true;
			}

			OnResurrectDeny(zone, m);
			return false;
		}

		public virtual bool CheckResurrect(DungeonZone zone, Mobile m)
		{
			if (m == null || m.Deleted)
			{
				return false;
			}

			if (m.AccessLevel > AccessLevel.Counselor)
			{
				return true;
			}

			return Options.Rules.CanResurrect;
		}

		protected virtual void OnResurrectAccept(DungeonZone zone, Mobile m)
		{ }

		protected virtual void OnResurrectDeny(DungeonZone zone, Mobile m)
		{
			if (m != null && !m.Deleted)
			{
				m.SendMessage("You can not be resurrected at this time.");
			}
		}

		public void OnSpeech(DungeonZone zone, SpeechEventArgs args)
		{
			if (args.Mobile is PlayerMobile)
			{
				var pm = (PlayerMobile)args.Mobile;

				if (HandleSubCommand(zone, pm, args.Speech))
				{
					args.Handled = true;
					args.Blocked = true;
					return;
				}
			}

			if (CheckSpeech(zone, args))
			{
				OnSpeechAccept(zone, args);
				return;
			}

			args.Handled = true;
			args.Blocked = true;

			OnSpeechDeny(zone, args);
		}

		public virtual bool CheckSpeech(DungeonZone zone, SpeechEventArgs e)
		{
			if (e.Mobile == null || e.Mobile.Deleted)
			{
				return false;
			}

			if (e.Mobile.AccessLevel > AccessLevel.Counselor)
			{
				return true;
			}

			return Options.Rules.AllowSpeech;
		}

		protected virtual void OnSpeechAccept(DungeonZone zone, SpeechEventArgs args)
		{ }

		protected virtual void OnSpeechDeny(DungeonZone zone, SpeechEventArgs args)
		{
			if (args.Mobile != null && !args.Mobile.Deleted)
			{
				args.Mobile.SendMessage("You can not talk at this time.");
			}
		}

		public bool AllowBeneficial(DungeonZone zone, Mobile source, Mobile target)
		{
			if (CheckAllowBeneficial(zone, source, target))
			{
				OnAllowBeneficialAccept(zone, source, target);
				return true;
			}

			OnAllowBeneficialDeny(zone, source, target);
			return false;
		}

		public virtual bool CheckAllowBeneficial(DungeonZone zone, Mobile source, Mobile target)
		{
			if (source != null && !source.Deleted && source.AccessLevel > AccessLevel.Counselor)
			{
				return true;
			}

			return Options.Rules.AllowBeneficial;
		}

		protected virtual void OnAllowBeneficialAccept(DungeonZone zone, Mobile source, Mobile target)
		{ }

		protected virtual void OnAllowBeneficialDeny(DungeonZone zone, Mobile source, Mobile target)
		{
			if (source != null && !source.Deleted && target != null && !target.Deleted && source != target)
			{
				source.SendMessage("You can not perform beneficial actions on your target.");
			}
		}
		
#if ServUO
		public bool AllowHarmful(DungeonZone zone, Mobile source, IDamageable target)
#else
		public bool AllowHarmful(DungeonZone zone, Mobile source, Mobile target)
#endif
		{
			if (CheckAllowHarmful(zone, source, target))
			{
				OnAllowHarmfulAccept(zone, source, target);
				return true;
			}

			OnAllowHarmfulDeny(zone, source, target);
			return false;
		}

#if ServUO
		public virtual bool CheckAllowHarmful(DungeonZone zone, Mobile source, IDamageable target)
#else
		public virtual bool CheckAllowHarmful(DungeonZone zone, Mobile source, Mobile target)
#endif
		{
			if (source != null && !source.Deleted && source.AccessLevel > AccessLevel.Counselor)
			{
				return true;
			}

			return Options.Rules.AllowHarmful;
		}

#if ServUO
		protected virtual void OnAllowHarmfulAccept(DungeonZone zone, Mobile source, IDamageable target)
#else
		protected virtual void OnAllowHarmfulAccept(DungeonZone zone, Mobile source, Mobile target)
#endif
		{ }

#if ServUO
		protected virtual void OnAllowHarmfulDeny(DungeonZone zone, Mobile source, IDamageable target)
#else
		protected virtual void OnAllowHarmfulDeny(DungeonZone zone, Mobile source, Mobile target)
#endif
		{
			if (source != null && !source.Deleted && target != null && !target.Deleted && source != target)
			{
				source.SendMessage("You can not perform harmful actions on your target.");
			}
		}

		public virtual bool AcceptsSpawnsFrom(DungeonZone zone, Region region)
		{
			return zone != null && AllowSpawn(zone) && region != null && region.IsPartOf(zone);
		}

		public virtual void OnAlterLightLevel(DungeonZone zone, Mobile m, ref int global, ref int personal)
		{ }

		public virtual void OnSpellCast(DungeonZone zone, Mobile m, ISpell s)
		{ }

		public virtual void OnAggressed(DungeonZone zone, Mobile aggressor, Mobile aggressed, bool criminal)
		{ }

		public virtual void OnBeneficialAction(DungeonZone zone, Mobile helper, Mobile target)
		{ }

#if ServUO
		public virtual bool OnCombatantChange(DungeonZone zone, Mobile m, IDamageable oldMob, IDamageable newMob)
#else
		public virtual bool OnCombatantChange(DungeonZone zone, Mobile m, Mobile oldMob, Mobile newMob)
#endif
		{
			if (m != null && !m.Deleted && m.Alive && MobileSpawns.Contains(m))
			{
				if (oldMob == null && newMob != null)
				{
					OnSpawnActivate(m);
				}
				else if (oldMob != null && newMob == null)
				{
					OnSpawnDeactivate(m);
				}
			}

			return true;
		}

		public virtual void OnCriminalAction(DungeonZone zone, Mobile m, bool message)
		{ }

#if ServUO
		public virtual void OnDidHarmful(DungeonZone zone, Mobile harmer, IDamageable harmed)
#else
		public virtual void OnDidHarmful(DungeonZone zone, Mobile harmer, Mobile harmed)
#endif
		{ }

		public virtual bool OnDoubleClick(DungeonZone zone, Mobile m, object o)
		{
			if (m == null || m.Deleted || o == null)
			{
				return false;
			}

			if (o is Item)
			{
				return OnDoubleClick(zone, m, (Item)o);
			}

			if (o is Mobile)
			{
				return OnDoubleClick(zone, m, (Mobile)o);
			}

			return true;
		}

		public virtual bool OnDoubleClick(DungeonZone zone, Mobile m, Item item)
		{
			if (m == null || m.Deleted || item == null || item.Deleted)
			{
				return false;
			}

			if (m.AccessLevel > AccessLevel.Counselor)
			{
				return true;
			}
			/*
			if (item is IShrinkItem)
			{
				if (!Options.Rules.AllowPets)
				{
					m.SendMessage("You are not allowed to summon pets in this dungeon.");
					return false;
				}

				if (Options.Restrictions.Pets.IsRestricted(((IShrinkItem)item).Link))
				{
					m.SendMessage("You can not use that in this dungeon.");
					return false;
				}
			}
			*/
			if (item is EtherealMount && !Options.Rules.CanMountEthereal)
			{
				m.SendMessage("You are not allowed to ride a mount in this dungeon.");
				return false;
			}

			if (Options.Restrictions.Items.IsRestricted(item))
			{
				m.SendMessage("You can not use that in this dungeon.");
				return false;
			}

			return true;
		}

		public virtual bool OnDoubleClick(DungeonZone zone, Mobile m, Mobile target)
		{
			if (m == null || m.Deleted || target == null || target.Deleted)
			{
				return false;
			}
			
			if (target is BaseCreature)
			{
				return OnDoubleClick(zone, m, (BaseCreature)target);
			}

			return true;
		}

		public virtual bool OnDoubleClick(DungeonZone zone, Mobile m, BaseCreature target)
		{
			if (m == null || m.Deleted || target == null || target.Deleted)
			{
				return false;
			}

			if (m.AccessLevel > AccessLevel.Counselor)
			{
				return true;
			}

			if (target.IsControlledBy(m))
			{
				if (!Options.Rules.AllowPets)
				{
					if (m is PlayerMobile)
					{
						((PlayerMobile)m).AutoStablePets();
					}

					return false;
				}

				if (target is BaseMount && !Options.Rules.CanMount)
				{
					m.SendMessage("You are not allowed to ride a mount in this dungeon.");
					return false;
				}

				if (Options.Restrictions.Pets.IsRestricted(target))
				{
					m.SendMessage("You can not use that in this dungeon.");
					return false;
				}
			}

			return true;
		}

		public virtual bool OnSingleClick(DungeonZone zone, Mobile m, object o)
		{
			if (m == null || m.Deleted || o == null)
			{
				return false;
			}

			if (o is Item)
			{
				return OnSingleClick(zone, m, (Item)o);
			}

			if (o is Mobile)
			{
				return OnSingleClick(zone, m, (Mobile)o);
			}

			return true;
		}

		public virtual bool OnSingleClick(DungeonZone zone, Mobile m, Item item)
		{
			if (m == null || m.Deleted || item == null || item.Deleted)
			{
				return false;
			}

			if (m.AccessLevel > AccessLevel.Counselor)
			{
				return true;
			}

			return !Options.Restrictions.Items.IsRestricted(item);
		}

		public virtual bool OnSingleClick(DungeonZone zone, Mobile m, Mobile target)
		{
			if (m == null || m.Deleted || target == null || target.Deleted)
			{
				return false;
			}

			if (target is BaseCreature)
			{
				return OnSingleClick(zone, m, (BaseCreature)target);
			}

			return true;
		}

		public virtual bool OnSingleClick(DungeonZone zone, Mobile m, BaseCreature target)
		{
			if (m == null || m.Deleted || target == null || target.Deleted)
			{
				return false;
			}

			if (m.AccessLevel > AccessLevel.Counselor)
			{
				return true;
			}

			if (target is BaseMount && target.GetMaster() == m && Options.Restrictions.Pets.IsRestricted(target))
			{
				return false;
			}

			return true;
		}

		public virtual void OnGotBeneficialAction(DungeonZone zone, Mobile helper, Mobile target)
		{ }

#if ServUO
		public virtual void OnGotHarmful(DungeonZone zone, Mobile harmer, IDamageable harmed)
#else
		public virtual void OnGotHarmful(DungeonZone zone, Mobile harmer, Mobile harmed)
#endif
		{ }

		public virtual bool OnTarget(DungeonZone zone, Mobile m, Target target, object o)
		{
			return m != null && !m.Deleted && target != null && o != null;
		}

		public virtual bool CheckAccessibility(DungeonZone zone, Item item, Mobile from)
		{
			return true;
		}

		public virtual bool OnDecay(DungeonZone zone, Item item)
		{
			return true;
		}

		public virtual void SpellDamageScalar(DungeonZone zone, Mobile caster, Mobile target, ref double damage)
		{ }
	}
}
