#region Header
//   Vorspire    _,-'/-'/  AspectRaidKey.cs
//   .      __,-; ,'( '/
//    \.    `-.__`-._`:_,-._       _ , . ``
//     `:-._,------' ` _,`--` -: `_ , ` ,' :
//        `---..__,,--'  (C) 2017  ` -'. -'
//        #  Vita-Nex [http://core.vita-nex.com]  #
//  {o)xxx|===============-   #   -===============|xxx(o}
//        #        The MIT License (MIT)          #
#endregion

namespace Server.Items
{
    public class AspectRaidKey : Item
    {
        public override string DefaultName => "Aspect Raid Key";

        [Constructable]
        public AspectRaidKey()
            : base(0x1AE1)
        { }

        public AspectRaidKey(Serial serial)
            : base(serial)
        { }

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
