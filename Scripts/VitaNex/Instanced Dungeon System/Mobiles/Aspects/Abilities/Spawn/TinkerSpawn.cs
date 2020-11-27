#region Header
//   Vorspire    _,-'/-'/  TinkerSpawn.cs
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

using Server.ContextMenus;
using Server.Items;

using VitaNex.Collections;
#endregion

namespace Server.Mobiles
{
	public class TinkerSpawnAspectAbility : SpawnAspectAbility
	{
		public override string Name { get { return "Ancient Tinker Spawn"; } }

		public override AspectFlags Aspects { get { return AspectFlags.Tech; } }

		public override TimeSpan Lockdown { get { return TimeSpan.FromSeconds(15); } }
		public override TimeSpan Cooldown { get { return TimeSpan.FromSeconds(30); } }

		public override int SpawnLimit { get { return base.SpawnLimit * 2; } }

		protected override IAspectSpawn CreateSpawn(BaseAspect aspect)
		{
			return new TinkerSpawner(aspect);
		}

		private sealed class TinkerSpawner : DamageableItem, IAspectSpawn, ISpawner
		{
			private Timer _Timer;
			private List<ISpawnable> _Spawn;

			[CommandProperty(AccessLevel.GameMaster, true)]
			public BaseAspect Aspect { get; private set; }

			public override int HitEffect { get { return 14120; } }

			public override int DestroySound { get { return 544; } }

			public override double IDChange { get { return 0.10; } }

			public override bool DeleteOnDestroy { get { return true; } }
			public override bool Alive { get { return !Destroyed; } }
			public override bool CanDamage { get { return true; } }

			public override string DefaultName { get { return "Ancient Tinker Summoning Stone"; } }

			#region ISpawner
			bool ISpawner.UnlinkOnTaming { get { return false; } }
			Point3D ISpawner.HomeLocation { get { return Aspect != null ? Aspect.Location : GetWorldLocation(); } }
			int ISpawner.HomeRange { get { return Aspect != null ? Aspect.RangePerception : 10; } }
			#endregion

			public TinkerSpawner(BaseAspect aspect)
				: base(0x2ADD, 0x32F4)
			{
				Aspect = aspect;

				Hue = 1150;

				Hits = HitsMax = Aspect.Scale(1000);

				Register(this);
			}

			public TinkerSpawner(Serial serial)
				: base(serial)
			{ }

			private void InitTimer()
			{
				if (_Timer == null || !_Timer.Running)
				{
					// Each level will take longer to spawn
					var time = TimeSpan.FromSeconds(Aspect.Scale(10.0)); 

					_Timer = Timer.DelayCall(time, time, Slice);
				}
			}

			private void StopTimer()
			{
				if (_Timer != null)
				{
					_Timer.Stop();
					_Timer = null;
				}
			}

			private void Slice()
			{
				if (Deleted || !Alive)
				{
					StopTimer();
					return;
				}

				if (Aspect == null || Aspect.Deleted || !Aspect.Alive || !Aspect.InRange(this, Aspect.RangePerception * 2))
				{
					Destroy();
					return;
				}

				if (!Aspect.InCombat())
				{
					return;
				}

				if (_Spawn == null)
				{
					_Spawn = ListPool<ISpawnable>.AcquireObject();
				}

				if (_Spawn.Count < 3)
				{
					var s = new Tinker(Aspect);

					_Spawn.Add(s);

					s.Spawner = this;

					Register(s);

					var p = this.GetRandomPoint3D(1, 2, Map, true, true);

					s.OnBeforeSpawn(p, Map);
					s.MoveToWorld(p, Map);
					s.OnAfterSpawn();
				}
			}

			public override void OnAfterSpawn()
			{
				InitTimer();

				base.OnAfterSpawn();
			}

			public override void OnDelete()
			{
				base.OnDelete();

				if (_Spawn != null)
				{
					_Spawn.ForEachReverse(o => o.Delete());
					_Spawn.Clear();
				}
			}
			
			public override void OnAfterDelete()
			{
				StopTimer();

				base.OnAfterDelete();

				Unregister(this);

				if (_Spawn != null)
				{
					ObjectPool.Free(ref _Spawn);
				}

				if (Aspect != null)
				{
					Aspect = null;
				}
			}

			#region ISpawner
			void ISpawner.Remove(ISpawnable spawn)
			{
				if (_Spawn != null)
				{
					_Spawn.Remove(spawn);
				}
			}

			void ISpawner.GetSpawnProperties(ISpawnable spawn, ObjectPropertyList list)
			{ }

			void ISpawner.GetSpawnContextEntries(ISpawnable spawn, Mobile m, List<ContextMenuEntry> list)
			{ }
			#endregion

			public override void Serialize(GenericWriter writer)
			{
				base.Serialize(writer);

				writer.SetVersion(0);

				writer.Write(Aspect);

				writer.WriteList(_Spawn, (w, o) => w.WriteEntity(o));
			}

			public override void Deserialize(GenericReader reader)
			{
				base.Deserialize(reader);

				reader.GetVersion();

				Aspect = reader.ReadMobile<BaseAspect>();

				_Spawn = reader.ReadList(r => r.ReadEntity<ISpawnable>(), _Spawn);

				_Spawn.ForEachReverse(o => o.Spawner = this);

				Register(this);

				Timer.DelayCall(InitTimer);
			}
		}

		private sealed class Tinker : AspectSpawn
		{
			public Tinker(BaseAspect aspect)
				: base(aspect, AIType.AI_Melee, FightMode.Closest, 0.2, 0.4)
			{
				Name = "Ancient Tinker";
				Body = 773;
			}

			public Tinker(Serial serial)
				: base(serial)
			{ }

			public override WeaponAbility GetWeaponAbility()
			{
				return Utility.RandomBool() ? WeaponAbility.Disarm : WeaponAbility.MortalStrike;
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
}