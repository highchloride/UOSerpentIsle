#region Header
//   Vorspire    _,-'/-'/  ManaLeachSpawn.cs
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

using VitaNex.FX;
#endregion

namespace Server.Mobiles
{
	public class ManaLeachSpawnAspectAbility : SpawnAspectAbility
	{
		public override string Name { get { return "Mana Leach Spawn"; } }

		public override AspectFlags Aspects
		{
			get { return AspectFlags.Elements | AspectFlags.Faith | AspectFlags.Nature; }
		}

		public override TimeSpan Lockdown { get { return TimeSpan.FromSeconds(45); } }
		public override TimeSpan Cooldown { get { return TimeSpan.FromSeconds(30); } }

		protected override IAspectSpawn CreateSpawn(BaseAspect aspect)
		{
			return new ManaLeach(aspect);
		}

		private class ManaLeach : AspectSpawn
		{
			private EnergyExplodeEffect _Aura;
			private long _NextAura;

			public ManaLeach(BaseAspect aspect)
				: base(aspect, AIType.AI_Mage, FightMode.None, 0.2, 0.4)
			{
				Name = "Mana Leach";
				Body = 129;

				CantWalk = true;
			}

			public ManaLeach(Serial serial)
				: base(serial)
			{ }

			public override void OnThink()
			{
				base.OnThink();

				if (Deleted || !Alive || !Aspect.InCombat())
				{
					return;
				}

				if (!this.InCombat() && Utility.RandomDouble() < 0.01)
				{
					var t = Aspect.AcquireRandomTarget(Aspect.RangePerception);

					if (t != null)
					{
						if (this.PlayAttackAnimation())
						{
							this.PlayAttackSound();
						}

						MoveToWorld(t.Location, t.Map);
					}
				}

				if (_Aura == null)
				{
					_Aura = new EnergyExplodeEffect(Location, Map, 3)
					{
						AverageZ = false,
						Reversed = true,
						Interval = TimeSpan.FromMilliseconds(500.0),
						EffectMutator = e =>
						{
							if (e.ProcessIndex == 0 && Utility.RandomDouble() < 0.33)
							{
								e.SoundID = 252;
							}

							e.Hue = Hue;
						},
						EffectHandler = HandleAura,
						Callback = () =>
						{
							if (this.PlayAttackAnimation())
							{
								this.PlayAttackSound();
							}
						}
					};
				}

				if (_Aura.Sending || Core.TickCount < _NextAura)
				{
					return;
				}

				_NextAura = Core.TickCount + 5000;

				_Aura.Start = Location;
				_Aura.Map = Map;
				_Aura.Send();
			}

			private void HandleAura(EffectInfo e)
			{
				if (Deleted || !Alive || e.ProcessIndex != 0)
				{
					return;
				}

				foreach (var t in Aspect.AcquireTargets(e.Source.Location, 0))
				{
					var sap = t.ManaMax * 0.10;

					if (!t.Player)
					{
						sap = Math.Min(100, sap);
					}

					t.Mana -= (int)sap;

					if (Aspect != null)
					{
						Aspect.Mana += (int)sap;
					}
				}
			}

			public override void OnAfterDelete()
			{
				base.OnAfterDelete();

				if (_Aura != null)
				{
					_Aura.Clear();
					_Aura = null;
				}
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