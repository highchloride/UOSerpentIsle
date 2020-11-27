#region Header
//   Vorspire    _,-'/-'/  AspectRaidTeleporter.cs
//   .      __,-; ,'( '/
//    \.    `-.__`-._`:_,-._       _ , . ``
//     `:-._,------' ` _,`--` -: `_ , ` ,' :
//        `---..__,,--'  (C) 2017  ` -'. -'
//        #  Vita-Nex [http://core.vita-nex.com]  #
//  {o)xxx|===============-   #   -===============|xxx(o}
//        #        The MIT License (MIT)          #
#endregion

using System;

namespace Server.Items
{
    public class AspectRaidTeleporter : Teleporter
    {
        public override string DefaultName => "Aspect Raid Teleporter";

        [CommandProperty(AccessLevel.GameMaster)]
        public int AspectKeysRequired { get; set; }

        [Constructable]
        public AspectRaidTeleporter()
            : this(1)
        { }

        [Constructable]
        public AspectRaidTeleporter(Point3D pointDest, Map mapDest)
            : this(1, pointDest, mapDest)
        { }

        [Constructable]
        public AspectRaidTeleporter(Point3D pointDest, Map mapDest, bool creatures)
            : this(1, pointDest, mapDest, creatures)
        { }

        [Constructable]
        public AspectRaidTeleporter(int keys)
            : this(keys, Point3D.Zero, null, false)
        { }

        [Constructable]
        public AspectRaidTeleporter(int keys, Point3D pointDest, Map mapDest)
            : this(keys, pointDest, mapDest, false)
        { }

        [Constructable]
        public AspectRaidTeleporter(int keys, Point3D pointDest, Map mapDest, bool creatures)
            : base(pointDest, mapDest, creatures)
        {
            AspectKeysRequired = keys;
        }

        public AspectRaidTeleporter(Serial serial)
            : base(serial)
        { }

        public override bool CanTeleport(Mobile m)
        {
            if (!base.CanTeleport(m))
                return false;

            if (AspectKeysRequired > 0 && m.Player)
            {
                if (!m.Backpack.HasItem<AspectRaidKey>(AspectKeysRequired, false))
                {
                    if (AspectKeysRequired > 1)
                        m.SendMessage("{0} aspect raid keys are required to teleport.", AspectKeysRequired);
                    else
                        m.SendMessage("An aspect raid key is required to teleport.");

                    return false;
                }
            }

            return true;
        }

        public override void DoTeleport(Mobile m)
        {
            var req = AspectKeysRequired;

            var keys = m.Backpack.FindItemsByType<AspectRaidKey>(true);

            foreach (var key in keys)
            {
                if (key.Amount >= req)
                {
                    key.Consume(req);
                    break;
                }

                req -= key.Amount;

                key.Delete();

                if (req <= 0)
                    break;
            }

            if (AspectKeysRequired > 0)
            {
                if (AspectKeysRequired > 1)
                    m.SendMessage("{0} aspect raid keys were used to teleport.", AspectKeysRequired);
                else
                    m.SendMessage("An aspect raid was used to teleport.");
            }

            base.DoTeleport(m);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.SetVersion(0);

            writer.Write(AspectKeysRequired);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            reader.GetVersion();

            AspectKeysRequired = reader.ReadInt();
        }
    }
}
