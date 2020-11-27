#region Header
//   Vorspire    _,-'/-'/  AspectAbility.cs
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
using System.Collections.Generic;
using System.Linq;

using VitaNex;
#endregion

namespace Server.Mobiles
{
	public interface IAspectSpawn : ISpawnable
	{
		BaseAspect Aspect { get; }
	}

	public abstract class AspectAbility
	{
		public sealed class State
		{
			public AspectAbility Ability { get; private set; }

			public BaseAspect Aspect { get; set; }
			public Mobile Target { get; set; }

			public bool Expires { get; set; }
			public DateTime Expire { get; set; }

			public bool TargetDeathPersist { get; set; }

			public bool IsValid
			{
				get
				{
					return Ability != null && Aspect != null && !Aspect.Deleted && Aspect.Alive && Target != null && !Target.Deleted &&
						   (Target.Alive || (TargetDeathPersist && (Target.Player || Target.IsDeadBondedPet)));
				}
			}

			public bool IsExpired { get { return CheckExpired(DateTime.UtcNow); } }

			public State(AspectAbility ability, BaseAspect aspect, Mobile target, TimeSpan duration)
			{
				Ability = ability;
				Aspect = aspect;
				Target = target;

				Expire = DateTime.UtcNow + duration;
			}

			public bool CheckExpired(DateTime utcNow)
			{
				return Expires && Expire < utcNow;
			}
		}

		private static PollTimer _Timer;

		public static AspectAbility[] Abilities { get; private set; }

		public static Dictionary<AspectAbility, Dictionary<Mobile, State>> States { get; private set; }

		static AspectAbility()
		{
			Abilities =
				typeof(AspectAbility).GetConstructableChildren()
									 .Select(t => t.CreateInstanceSafe<AspectAbility>())
									 .Where(a => a != null)
									 .ToArray();

			States = Abilities.ToDictionary(a => a, a => new Dictionary<Mobile, State>());
		}

		public static void Configure()
		{
			if (_Timer == null)
			{
				_Timer = PollTimer.FromSeconds(1.0, DefragmentStates, States.Any, false);
			}
		}

		public static void Initialize()
		{
			if (_Timer != null)
			{
				_Timer.Start();
			}
		}

		public static void DefragmentStates()
		{
			var now = DateTime.UtcNow;

			foreach (var states in States.Values)
			{
				states.RemoveValueRange(
					s =>
					{
						if (s == null)
						{
							return true;
						}

						if (!s.IsValid || s.CheckExpired(now))
						{
							if (s.Ability != null)
							{
								s.Ability.OnRemoved(s);
							}

							return true;
						}

						return false;
					});

				states.RemoveKeyRange(m => m == null || m.Deleted);
			}
		}

		public static List<AspectAbility> GetAbilities(BaseAspect aspect, bool checkLock)
		{
			return Abilities.Where(a => a.CanInvoke(aspect)).ToList();
		}

		public static bool HasAbility<TAbility>(BaseAspect aspect) where TAbility : AspectAbility
		{
			return Abilities.OfType<TAbility>().Any(a => a.HasFlags(aspect));
		}

		public abstract string Name { get; }

		public abstract AspectFlags Aspects { get; }

		public abstract TimeSpan Lockdown { get; }
		public abstract TimeSpan Cooldown { get; }

		public virtual TimeSpan Duration { get { return TimeSpan.Zero; } }

		public virtual double DamageFactor { get { return 1.0; } }

		public virtual bool Stackable { get { return false; } }

		public virtual bool MatchAnyAspect { get { return true; } }

		public void Damage(BaseAspect aspect, Mobile target)
		{
			aspect.DoHarmful(target, true);

			var damage = Utility.RandomMinMax(aspect.DamageMin, aspect.DamageMax);

			if (DamageFactor != 1.0)
			{
				damage = (int)Math.Ceiling(damage * DamageFactor);
			}

			if (damage > 0)
			{
				OnDamage(aspect, target, ref damage);
			}

			if (damage > 0)
			{
				target.Damage(damage, aspect);

				if (target.PlayDamagedAnimation())
				{
					target.PlayHurtSound();
				}
			}
		}

		protected virtual void OnDamage(BaseAspect aspect, Mobile target, ref int damage)
		{ }

		protected virtual void OnAdded(State state)
		{ }

		protected virtual void OnRemoved(State state)
		{ }

		public bool HasFlags(BaseAspect aspect)
		{
			if (aspect == null || aspect.Aspects == AspectFlags.None || Aspects == AspectFlags.None)
			{
				return false;
			}

			if (Aspects == aspect.Aspects)
			{
				return true;
			}

			return MatchAnyAspect &&
				   Aspects.EnumerateValues<AspectFlags>(true).Any(a => a != AspectFlags.None && aspect.Aspects.GetFlag(a));
		}

		public bool CheckLock(BaseAspect aspect, bool locked)
		{
			return aspect != null && (locked ? !aspect.CanBeginAction(this) : aspect.CanBeginAction(this));
		}

		public void SetLock(BaseAspect aspect, bool locked)
		{
			if (aspect == null)
			{
				return;
			}

			if (locked)
			{
				aspect.BeginAction(this);
				OnLocked(aspect);
			}
			else
			{
				aspect.EndAction(this);
				OnUnlocked(aspect);
			}
		}

		protected IEnumerable<TMobile> AcquireTargets<TMobile>(
			BaseAspect aspect,
			bool cache = true,
			Func<TMobile, bool> filter = null) where TMobile : Mobile
		{
			return AcquireTargets(aspect, aspect.Location, aspect.RangePerception, cache, filter);
		}

		protected IEnumerable<TMobile> AcquireTargets<TMobile>(
			BaseAspect aspect,
			Point3D p,
			bool cache = true,
			Func<TMobile, bool> filter = null) where TMobile : Mobile
		{
			return AcquireTargets(aspect, p, aspect.RangePerception, cache, filter);
		}

		protected IEnumerable<TMobile> AcquireTargets<TMobile>(
			BaseAspect aspect,
			int range,
			bool cache = true,
			Func<TMobile, bool> filter = null) where TMobile : Mobile
		{
			return AcquireTargets(aspect, aspect.Location, range, cache, filter);
		}

		protected virtual IEnumerable<TMobile> AcquireTargets<TMobile>(
			BaseAspect aspect,
			Point3D p,
			int range,
			bool cache = true,
			Func<TMobile, bool> filter = null) where TMobile : Mobile
		{
			if (aspect == null || aspect.Deleted || aspect.Map == null || aspect.Map == Map.Internal)
			{
				yield break;
			}

			var targets = aspect.AcquireTargets<TMobile>(p, range);

			foreach (var t in (filter != null ? targets.Where(filter) : targets))
			{
				if (cache && Duration > TimeSpan.Zero)
				{
					SetTargetState(aspect, t, Duration);
				}

				yield return t;
			}
		}

		public State GetTargetState(Mobile m)
		{
			return States.GetValue(this).GetValue(m);
		}

		public void SetTargetState(BaseAspect aspect, Mobile target, TimeSpan duration)
		{
			Dictionary<Mobile, State> states;

			if (!States.TryGetValue(this, out states) || states == null)
			{
				States[this] = states = new Dictionary<Mobile, State>();
			}

			State state;

			if (!states.TryGetValue(target, out state) || state == null)
			{
				states[target] = state = new State(this, aspect, target, duration);
			}
			else
			{
				OnRemoved(state);

				state.Aspect = aspect;
				state.Target = target;

				if (Stackable && !state.IsExpired)
				{
					state.Expire += duration;
				}
				else
				{
					state.Expire = DateTime.UtcNow.Add(duration);
				}
			}

			OnAdded(state);
		}

		public virtual bool CanInvoke(BaseAspect aspect)
		{
			return aspect != null && !aspect.Deleted && aspect.Alive && !aspect.Blessed && //
				   aspect.InCombat(TimeSpan.Zero) && HasFlags(aspect) && CheckLock(aspect, false) && aspect.CanUseAbility(this);
		}

		public bool TryInvoke(BaseAspect aspect)
		{
			if (CanInvoke(aspect))
			{
				return VitaNexCore.TryCatchGet(
					() =>
					{
						SetLock(aspect, true);

						OnInvoke(aspect);

						aspect.OnAbility(this);

						var locked = Lockdown.TotalSeconds;

						if (locked > 0)
						{
							locked -= aspect.Scale(locked * 0.10);
						}

						locked = Math.Max(0, locked);

						Timer.DelayCall(TimeSpan.FromSeconds(locked), a => SetLock(a, false), aspect);

						return true;
					},
					x => x.ToConsole(true));
			}

			return false;
		}

		protected abstract void OnInvoke(BaseAspect aspect);

		protected virtual void OnLocked(BaseAspect aspect)
		{ }

		protected virtual void OnUnlocked(BaseAspect aspect)
		{ }
	}
}