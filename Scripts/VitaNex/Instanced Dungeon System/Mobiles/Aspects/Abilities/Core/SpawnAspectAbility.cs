#region Header
//   Vorspire    _,-'/-'/  SpawnAspectAbility.cs
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

using Server.Spells;

using VitaNex.Collections;
#endregion

namespace Server.Mobiles
{
	public abstract class SpawnAspectAbility : AspectAbility
	{
		public static Dictionary<BaseAspect, List<IAspectSpawn>> Spawn { get; private set; }

		static SpawnAspectAbility()
		{
			Spawn = new Dictionary<BaseAspect, List<IAspectSpawn>>();
		}

		public static void Register(IAspectSpawn spawn)
		{
			if (spawn == null || spawn.Aspect == null)
			{
				return;
			}

			List<IAspectSpawn> list;

			if (!Spawn.TryGetValue(spawn.Aspect, out list) || list == null)
			{
				Spawn[spawn.Aspect] = list = ListPool<IAspectSpawn>.AcquireObject();
			}

			list.AddOrReplace(spawn);
		}

		public static void Unregister(IAspectSpawn spawn)
		{
			if (spawn == null || spawn.Aspect == null)
			{
				return;
			}

			List<IAspectSpawn> list;

			if (!Spawn.TryGetValue(spawn.Aspect, out list))
			{
				return;
			}

			if (list == null)
			{
				Spawn.Remove(spawn.Aspect);
				return;
			}

			list.Remove(spawn);

			if (list.Count == 0)
			{
				Spawn.Remove(spawn.Aspect);

				ObjectPool.Free(list);
			}
		}

		public virtual int SpawnLimit { get { return 5; } }

		protected abstract IAspectSpawn CreateSpawn(BaseAspect aspect);

		public override bool CanInvoke(BaseAspect aspect)
		{
			if (!base.CanInvoke(aspect))
			{
				return false;
			}

			List<IAspectSpawn> spawn;

			return !Spawn.TryGetValue(aspect, out spawn) || spawn == null || spawn.Count < SpawnLimit;
		}

		protected override void OnInvoke(BaseAspect aspect)
		{
			if (aspect == null || aspect.Deleted)
			{
				return;
			}

			Point3D loc;
			var tries = 30;

			do
			{
				loc = aspect.GetRandomPoint3D(4, 8, aspect.Map, true, true);
			}
			while (loc.FindEntitiesInRange<IAspectSpawn>(aspect.Map, 8).Any() && --tries >= 0);

			if (tries < 0)
			{
				return;
			}

			var s = CreateSpawn(aspect);

			if (s == null)
			{
				return;
			}

			Register(s);

			if (aspect.PlayAttackAnimation())
			{
				aspect.PlayAttackSound();

				aspect.TryParalyze(
					TimeSpan.FromSeconds(1.5),
					m =>
					{
						SpellHelper.Turn(m, loc);

						s.OnBeforeSpawn(loc, m.Map);
						s.MoveToWorld(loc, m.Map);
						s.OnAfterSpawn();
					});
			}
			else
			{
				SpellHelper.Turn(aspect, loc);

				s.OnBeforeSpawn(loc, aspect.Map);
				s.MoveToWorld(loc, aspect.Map);
				s.OnAfterSpawn();
			}
		}
	}
}
