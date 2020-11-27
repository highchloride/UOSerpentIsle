#region Header
//   Vorspire    _,-'/-'/  AspectRaidBoss.cs
//   .      __,-; ,'( '/
//    \.    `-.__`-._`:_,-._       _ , . ``
//     `:-._,------' ` _,`--` -: `_ , ` ,' :
//        `---..__,,--'  (C) 2017  ` -'. -'
//        #  Vita-Nex [http://core.vita-nex.com]  #
//  {o)xxx|===============-   #   -===============|xxx(o}
//        #        The MIT License (MIT)          #
#endregion

using System;
using System.Linq;

using Server.Items;

namespace Server.Mobiles
{
    public class AspectRaidBoss : BaseAspect
    {
        private static readonly Body[] _ValidBodies;
        private static readonly AIType[] _ValidAI;

        private static readonly AspectFlags[] _ValidFlags;
        private static readonly AspectLevel[] _ValidLevels;

        static AspectRaidBoss()
        {
            _ValidBodies = Enumerable.Range(1, 4096).Select(b => (Body)b).Where(b => b.IsMonster).ToArray();

            _ValidAI = new[] { AIType.AI_Berserk, AIType.AI_Mage, AIType.AI_Melee };

            _ValidFlags = default(AspectFlags).EnumerateValues<AspectFlags>(false).Where(f => f != 0 && f != AspectFlags.All).ToArray();

            _ValidLevels = new[] { AspectLevel.Taxing, AspectLevel.Extreme };
        }

        private static AspectFlags GetRandomAspects()
        {
            if (_ValidFlags.Length == 0)
                return 0;

            if (_ValidFlags.Length < 4)
                return _ValidFlags.GetRandom();

            var length = Math.Min(8, _ValidFlags.Length);
            var count = Utility.RandomMinMax(length / 4, length / 2);

            var flags = AspectFlags.None;

            while (--count >= 0)
            {
                var f = _ValidFlags.GetRandom();

                if (!flags.HasFlag(f))
                    flags |= f;
                else
                    ++count;
            }

            return flags;
        }

        private AspectFlags _DefaultFlags = GetRandomAspects();

        public override AspectFlags DefaultAspects => _DefaultFlags;

        private AspectLevel _DefaultLevel = _ValidLevels.GetRandom();

        public override AspectLevel DefaultLevel => _DefaultLevel;

        [CommandProperty(AccessLevel.GameMaster)]
        public int AspectKeysDropped { get; set; }

        [Constructable]
        public AspectRaidBoss()
            : this(1)
        { }

        [Constructable]
        public AspectRaidBoss(int keys)
            : base(_ValidAI.GetRandom(), FightMode.Closest, 16, 1, 0.1, 0.2)
        {
            AspectKeysDropped = keys;

            Name = NameList.RandomName("daemon");
        }

        public AspectRaidBoss(Serial serial)
            : base(serial)
        { }

        protected override int InitBody()
        {
            return _ValidBodies.GetRandom();
        }

        public override void OnKilledBy(Mobile mob)
        {
            base.OnKilledBy(mob);

            if (mob != null && mob.Player)
            {
                var count = AspectKeysDropped;

                while (--count >= 0)
                    mob.GiveItem(new AspectRaidKey(), GiveFlags.All);
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.SetVersion(0);

            writer.WriteFlag(_DefaultFlags);
            writer.WriteFlag(_DefaultLevel);

            writer.Write(AspectKeysDropped);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            reader.GetVersion();

            _DefaultFlags = reader.ReadFlag<AspectFlags>();
            _DefaultLevel = reader.ReadFlag<AspectLevel>();

            AspectKeysDropped = reader.ReadInt();
        }
    }
}
