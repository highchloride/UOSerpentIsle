#region Header
//   Vorspire    _,-'/-'/  BaseAspect.cs
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

using Server.ContextMenus;
using Server.Items;
using Server.Network;
using Server.Spells;

using VitaNex;
using VitaNex.FX;
using VitaNex.Network;
#endregion

namespace Server.Mobiles
{
    [CorpseName("remains of an Aspect")]
    public abstract class BaseAspect : BaseCreature
    {
        private static readonly SkillName[] _InitialSkills = ((SkillName)0).GetValues<SkillName>();

        public static readonly int MinAspectLevel = (int)AspectLevel.Normal;
        public static readonly int MaxAspectLevel = (int)AspectLevel.Insane;

        public static Type[] AspectTypes { get; private set; }

        public static List<BaseAspect> Instances { get; private set; }

        static BaseAspect()
        {
            Instances = new List<BaseAspect>();

            AspectTypes = typeof(BaseAspect).GetConstructableChildren();
        }

        public static BaseAspect CreateRandomAspect()
        {
            return CreateAspect(AspectTypes.GetRandom());
        }

        public static BaseAspect CreateAspect(Type t)
        {
            return t != null && t.IsChildOf<BaseAspect>() ? t.CreateInstanceSafe<BaseAspect>() : null;
        }

        public static TAspect CreateAspect<TAspect>() where TAspect : BaseAspect
        {
            return typeof(TAspect).CreateInstanceSafe<TAspect>();
        }

        private DateTime _NextAbility = DateTime.UtcNow;

        public override FoodType FavoriteFood { get { return FoodType.Gold; } }

        public override bool IgnoreYoungProtection { get { return true; } }

        public override bool CanBeParagon { get { return false; } }

        public override bool CanRummageCorpses { get { return false; } }
        public override bool CanDestroyObstacles { get { return true; } }
        public override bool CanFlee { get { return false; } }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool AutoDispel { get { return true; } }
        public override bool BardImmune { get { return true; } }
        public override bool Unprovokable { get { return true; } }
        public override bool Uncalmable { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }

        public override int TreasureMapLevel { get { return 10; } }
        public override double TreasureMapChance { get { return 0.50 + ((int)AspectLevel * 0.10); } }

        public virtual SkillName[] InitialSkills { get { return _InitialSkills; } }

        public virtual bool HealFromPoison { get { return Enraged; } }

        private AspectLevel _Level;

        [CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
        public AspectLevel AspectLevel
        {
            get { return _Level; }
            set
            {
                _Level = value;

                InitLevel();

                InvalidateProperties();
            }
        }

        public virtual AspectLevel DefaultLevel { get { return AspectLevel.Normal; } }

        private AspectAttributes _Aspects;

        [CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
        public AspectAttributes Aspects
        {
            get { return _Aspects ?? (_Aspects = new AspectAttributes(this)); }
            set { _Aspects = value ?? new AspectAttributes(this); }
        }

        public virtual AspectFlags DefaultAspects { get { return AspectFlags.None; } }

        public virtual double EnrageThreshold { get { return 0.05; } }

        // Default: 10% Increase to all stats
        private readonly StatBuffInfo _EnrageStatBuff = new StatBuffInfo(StatType.All, "Enraged", 10, TimeSpan.Zero);

        [CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
        public StatType EnrageBuffType
        {
            get { return _EnrageStatBuff.Type; }
            set
            {
                RemoveStatMod(_EnrageStatBuff.Name);

                _EnrageStatBuff.Type = value;
            }
        }

        [CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
        public string EnrageBuffName
        {
            get { return _EnrageStatBuff.Name; }
            set
            {
                RemoveStatMod(_EnrageStatBuff.Name);

                _EnrageStatBuff.Name = value;
            }
        }

        [CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
        public int EnrageBuffOffset
        {
            get { return _EnrageStatBuff.Offset; }
            set
            {
                RemoveStatMod(_EnrageStatBuff.Name);

                _EnrageStatBuff.Offset = value;
            }
        }

        [CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
        public TimeSpan EnrageBuffDuration
        {
            get { return _EnrageStatBuff.Duration; }
            set
            {
                RemoveStatMod(_EnrageStatBuff.Name);

                _EnrageStatBuff.Duration = value;
            }
        }

        [CommandProperty(AccessLevel.Counselor)]
        public bool Enraged { get { return Hits / (double)HitsMax <= EnrageThreshold; } }

        private long _NextEnrage;
        private long _NextEffect;

        private bool _ReflectMelee;
		private bool _ReflectSpell;

		[CommandProperty(AccessLevel.GameMaster, true)]
		public bool ReflectMelee
		{
			get { return _ReflectMelee; }
			set
			{
				_ReflectMelee = value;

				switch (SolidHueOverride)
				{
					case -1:
					{
						if (value)
						{
							SolidHueOverride = 51;
						}
					}
						break;
					case 51:
					{
						if (!value)
						{
							SolidHueOverride = -1;
						}
					}
						break;
				}
			}
		}

		[CommandProperty(AccessLevel.GameMaster, true)]
		public bool ReflectSpell
		{
			get { return _ReflectSpell; }
			set
			{
				_ReflectSpell = value;

				switch (SolidHueOverride)
				{
					case -1:
					{
						if (value)
						{
							SolidHueOverride = 62;
						}
					}
						break;
					case 62:
					{
						if (!value)
						{
							SolidHueOverride = -1;
						}
					}
						break;
				}
			}
		}

        public BaseAspect(AIType aiType, FightMode mode, int perception, int rangeFight, double activeSpeed, double passiveSpeed)
            : base(aiType, mode, perception, rangeFight, activeSpeed, passiveSpeed)
        {
            _Aspects = new AspectAttributes(this);
            _Level = DefaultLevel;

            Female = Utility.RandomBool();

            Title = InitTitle();

            Body = InitBody();

            Fame = 50000;
            Karma = -50000;

            SpeechHue = YellHue = 34;

            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Fire, 20);
            SetDamageType(ResistanceType.Cold, 20);
            SetDamageType(ResistanceType.Poison, 20);
            SetDamageType(ResistanceType.Energy, 20);

            SetResistance(ResistanceType.Physical, 25, 50);
            SetResistance(ResistanceType.Fire, 25, 50);
            SetResistance(ResistanceType.Cold, 25, 50);
            SetResistance(ResistanceType.Energy, 25, 50);
            SetResistance(ResistanceType.Poison, 25, 50);

            InitLevel();

            var pack = Backpack;

            if (pack != null)
            {
                pack.Delete();
            }

            AddItem(new BottomlessBackpack());

            PackItems();
            EquipItems();

            Instances.Add(this);
        }

		public BaseAspect(Serial serial)
			: base(serial)
		{
			Instances.Add(this);
		}

		public virtual void AspectChanged(AspectFlags a, bool state)
		{
			Title = InitTitle();

			InvalidateProperties();
		}

		protected virtual void InitLevel()
		{
			Scale(this);
		}

		public virtual double GetLevelFactor()
		{
			return 1.0 + (4.0 * ((int)AspectLevel / 4.0));
		}

		public void Scale(BaseCreature c)
		{
			if (c == null)
			{
				return;
			}

			if (c is BaseAspect && c != this)
			{
				return;
			}

			var factor = GetLevelFactor();

			if (c is IAspectSpawn asp)
			{
				if (asp.Aspect != this)
				{
					return;
				}

				factor *= 0.10;
			}
			else if (c.Team != Team)
			{
				return;
			}

			c.VirtualArmor = Math.Min(90, 40 + Scale(10, factor));

			c.SetStr(Scale(200, factor), Scale(250, factor));
			c.SetDex(Scale(200, factor), Scale(250, factor));
			c.SetInt(Scale(200, factor), Scale(250, factor));

			c.SetHits(Scale(50000, factor), Scale(75000, factor));
			c.SetStam(Scale(1000, factor), Scale(2500, factor));
			c.SetMana(Scale(1000, factor), Scale(2500, factor));

			var damage = Scale(10, factor);

			c.SetDamage(Math.Min(80, 15 + damage), Math.Min(90, 30 + damage));

			var skill = Math.Min(120.0, 95.0 + Scale(5.0, factor));

			c.SetAllSkills(skill, Math.Max(100, skill));
		}

		public int Scale(int value)
		{
			return Scale(value, GetLevelFactor());
		}

		public virtual int Scale(int value, double factor)
		{
			return (int)Math.Ceiling(value * factor);
		}

		public double Scale(double value)
		{
			return Scale(value, GetLevelFactor());
		}

		public virtual double Scale(double value, double factor)
		{
			return Math.Ceiling(value * factor);
		}

		protected abstract int InitBody();

		protected virtual string InitTitle()
		{
			var title = "the Aspect";

			if (Aspects.IsEmpty)
			{
				return title;
			}

			if (Aspects.All)
			{
				title += " of Infinity";
			}

			return title;
		}

		protected virtual void PackItems()
		{ }

		protected virtual void EquipItems()
		{ }

		public override void AlterMeleeDamageFrom(Mobile from, ref int damage)
		{
			if (damage > 0 && from != null && from != this && from.Player && ReflectMelee)
			{
				damage /= 10;

				from.Damage(damage, this);
			}

			base.AlterMeleeDamageFrom(from, ref damage);
		}

		public override void AlterSpellDamageFrom(Mobile from, ref int damage)
		{
			if (damage > 0 && from != null && from != this && from.Player && ReflectSpell)
			{
				damage /= 10;

				from.Damage(damage, this);
			}

			base.AlterSpellDamageFrom(from, ref damage);
		}

		public override int Damage(int amount, Mobile m, bool informMount)
		{
			// Poison will cause all damage to heal instead.
			if (HealFromPoison && Poison != null)
			{
				Hits += amount;

                if (Utility.RandomDouble() < 0.10)
                {
                    var message = $"*{Name} {Utility.RandomList("looks healthy", "looks stronger", "is absorbing damage", "is healing")}*";

                    NonlocalOverheadMessage(MessageType.Regular, 0x21, false, message);
                }

                return 0;
            }
			else
			{
				return base.Damage(amount, m, informMount);
			}
		}

		public override void OnPoisoned(Mobile m, Poison poison, Poison oldPoison)
		{
			if (HealFromPoison)
			{
				NonlocalOverheadMessage(MessageType.Regular, 0x21, false, "*The poison seems to have the opposite effect*");
				return;
			}

			base.OnPoisoned(m, poison, oldPoison);
		}

		public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
		{
			base.GetContextMenuEntries(from, list);

			list.RemoveAll(e => e is PaperdollEntry);
		}

		public override void OnThink()
		{
			base.OnThink();

			if (Deleted || Map == null || Map == Map.Internal || !this.InCombat())
			{
				return;
			}

			if ((ReflectMelee || ReflectSpell) && Core.TickCount > _NextEffect)
			{
				_NextEffect = Core.TickCount + 1000;
				new EffectInfo(this.Clone3D(0, 0, 20), Map, 14120, 0x33, 10, 30, EffectRender.SemiTransparent).Send();
			}

            if (EnrageThreshold > 0 && Enraged && Core.TickCount > _NextEnrage)
            {
                _NextEnrage = Core.TickCount + 10000;

                var buff = _EnrageStatBuff;

                if (buff.Offset != 0 && !string.IsNullOrWhiteSpace(buff.Name))
                {
                    var old = GetStatMod(buff.Name);

                    if (old == null)
                    {
                        var offset = buff.Offset / 100.0;

                        var s = buff.Type == StatType.All || buff.Type == StatType.Str;
                        var d = buff.Type == StatType.All || buff.Type == StatType.Dex;
                        var i = buff.Type == StatType.All || buff.Type == StatType.Int;

                        if (s || d || i)
                        {
                            var sv = RawStr * (s ? offset : 0);
                            var dv = RawDex * (d ? offset : 0);
                            var iv = RawInt * (i ? offset : 0);

                            var bonus = (int)Math.Ceiling((sv + dv + iv) / ((s ? 1 : 0) + (d ? 1 : 0) + (i ? 1 : 0)));

                            if (bonus != 0)
                            {
                                var clone = buff.Clone();

                                if (clone != null)
                                {
                                    clone.Offset = bonus;

                                    AddStatMod(clone);
                                }
                            }
                        }
                    }
                }
            }

			var now = DateTime.UtcNow;

			if (Aspects.IsEmpty)
			{
				_NextAbility = now.AddSeconds(1.0);
				return;
			}

			if (!this.InCombat(TimeSpan.Zero) || now <= _NextAbility)
			{
				return;
			}

			var ability = GetRandomAbility(true);

			if (ability == null || !ability.TryInvoke(this))
			{
				_NextAbility = now.AddSeconds(1.0);
				return;
			}

			var cooldown = GetAbilityCooldown(ability);

			_NextAbility = cooldown > TimeSpan.Zero ? now.Add(cooldown) : now;
		}

		protected virtual AspectAbility GetRandomAbility(bool checkLock)
		{
			var abilities = GetAbilities(checkLock);

			var a = abilities.GetRandom();

			abilities.Free(true);

			return a;
		}

		protected virtual TimeSpan GetAbilityCooldown(AspectAbility ability)
		{
			if (ability != null)
			{
				var cool = ability.Cooldown.TotalSeconds;

				if (cool > 0)
				{
					cool -= Scale(cool * 0.10);
					cool = Math.Max(0, cool);
				}

				if (cool > 0)
				{
					return TimeSpan.FromSeconds(cool);
				}
			}

			return TimeSpan.Zero;
		}

		public virtual List<AspectAbility> GetAbilities(bool checkLock)
		{
			return AspectAbility.GetAbilities(this, checkLock);
		}

		public virtual bool CanUseAbility(AspectAbility ability)
		{
			return true;
		}

		public virtual void OnAbility(AspectAbility ability)
		{ }

		public override void GenerateLoot()
		{
			AddLoot(LootPack.SuperBoss, 4);
		}

		public override void OnDelete()
		{
			base.OnDelete();

			Instances.Remove(this);
			Instances.Free(false);
		}

		public override void OnAfterDelete()
		{
			base.OnAfterDelete();

			Instances.Remove(this);
			Instances.Free(false);
		}

        #region Acquiring Targets
        public Mobile AcquireRandomTarget(int range)
        {
            return AcquireRandomTarget(Location, range);
        }

        public Mobile AcquireRandomTarget(Point3D p, int range)
        {
            return AcquireTargets(p, range).GetRandom();
        }

        public TMobile AcquireRandomTarget<TMobile>(int range)
            where TMobile : Mobile
        {
            return AcquireRandomTarget<TMobile>(Location, range);
        }

        public TMobile AcquireRandomTarget<TMobile>(Point3D p, int range)
            where TMobile : Mobile
        {
            return AcquireTargets<TMobile>(p, range).GetRandom();
        }

        public IEnumerable<TMobile> AcquireTargets<TMobile>(int range)
            where TMobile : Mobile
        {
            return AcquireTargets<TMobile>(Location, range);
        }

        public IEnumerable<TMobile> AcquireTargets<TMobile>(Point3D p, int range)
            where TMobile : Mobile
        {
            return AcquireTargets(p, range).OfType<TMobile>();
        }

        public IEnumerable<Mobile> AcquireTargets(int range)
        {
            return AcquireTargets(Location, range);
        }

        public virtual IEnumerable<Mobile> AcquireTargets(Point3D p, int range)
        {
            return p.FindMobilesInRange(Map, range)
                .Where(m => m != null && !m.Deleted && m != this && m.AccessLevel <= AccessLevel && m.Alive)
                .Where(m => CanBeHarmful(m, false, true) && SpellHelper.ValidIndirectTarget(this, m))
                .Where(m => !m.IsControlledBy(this))
                .Where(m => Team == 0 || !(m is BaseCreature) || Team != ((BaseCreature)m).Team)
                .Where(m => Party == null || m.Party == null || m.Party != Party)
                .Where(m => m.Player || m.IsControlled() || m.HasAggressed(this) || m.HasAggressor(this));
        }
        #endregion
        
        public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			var version = writer.SetVersion(3);

			switch (version)
			{
                case 3:
				case 2:
				{
					writer.WriteFlag(_Level);

					Aspects.Serialize(writer);
				}
					goto case 1;
				case 1:
					_EnrageStatBuff.Serialize(writer);
					goto case 0;
				case 0:
					break;
			}
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			var version = reader.GetVersion();

			switch (version)
			{
                case 3:
				case 2:
				{
					_Level = reader.ReadFlag<AspectLevel>();
					_Aspects = new AspectAttributes(reader);
				}
					goto case 1;
				case 1:
                    {
                        if (version < 3)
                            reader.ReadInt();

                        _EnrageStatBuff.Deserialize(reader);
                    }
					goto case 0;
				case 0:
					break;
			}

			if (_Aspects == null)
			{
				_Aspects = new AspectAttributes(this);

				Title = InitTitle();
			}

			if (version < 2)
			{
				_Level = DefaultLevel;

				InitLevel();
			}
		}

		private sealed class BottomlessBackpack : StrongBackpack
		{
			public BottomlessBackpack()
			{
				MaxItems = 0;
				Movable = false;
				Hue = 0;
				Weight = 0.0;
			}

			public BottomlessBackpack(Serial serial)
				: base(serial)
			{ }

			public override void OnSnoop(Mobile m)
			{
				if (m != null && m.AccessLevel > AccessLevel.Player)
				{
					base.OnSnoop(m);
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
