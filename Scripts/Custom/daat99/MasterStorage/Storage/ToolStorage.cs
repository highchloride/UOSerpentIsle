/*
 created by:
     /\            888                   888     .d8888b.   .d8888b.  
____/_ \____       888                   888    d88P  Y88b d88P  Y88b 
\  ___\ \  /       888                   888    888    888 888    888 
 \/ /  \/ /    .d88888  8888b.   8888b.  888888 Y88b. d888 Y88b. d888 
 / /\__/_/\   d88" 888     "88b     "88b 888     "Y888P888  "Y888P888 
/__\ \_____\  888  888 .d888888 .d888888 888           888        888 
    \  /      Y88b 888 888  888 888  888 Y88b.  Y88b  d88P Y88b  d88P 
     \/        "Y88888 "Y888888 "Y888888  "Y888  "Y8888P"   "Y8888P"  
*/
using System;
using Server;
using Server.Items;
using Server.Engines.Harvest;
using Server.Engines.Craft;
using Server.Network;
using Server.Targeting;
using Server.ContextMenus;

namespace daat99
{
	public class ToolStorage : BaseStorage
	{
		public ToolStorage() : base() { }
		public ToolStorage(GenericReader reader) : base(reader) { }
		public override string Name { get { return "Tool Storage"; } }
        private static Type[] defaultStoredTypes = new Type[] { typeof(Blowpipe), typeof(DovetailSaw), typeof(DrawKnife), typeof(FletcherTools),
			typeof(FlourSifter), typeof(Froe), typeof(Hammer), typeof(Inshave), typeof(JointingPlane), typeof(MalletAndChisel), typeof(MapmakersPen), 
            typeof(MortarPestle),typeof(MouldingPlane),	typeof(Nails), typeof(RollingPin), typeof(Saw), typeof(Scorp), typeof(ScribesPen),
			typeof(SewingKit),	typeof(Skillet), typeof(SledgeHammer), typeof(SmithHammer), typeof(SmoothingPlane), typeof(TinkerTools), 
            typeof(Tongs), typeof(Pickaxe), typeof(GargoylesPickaxe), typeof(SturdyPickaxe), typeof(Shovel), typeof(SturdyShovel), 
            typeof(ProspectorsTool), typeof(TinkersTools) };
		public override Type[] DefaultStoredTypes { get { return ToolStorage.defaultStoredTypes; } }
		protected static new BaseStorage singleton; 
		public static new BaseStorage Storage { get { if (singleton == null) singleton = new ToolStorage(); return singleton; } }
		public override BaseStorage GetStorage() { return ToolStorage.Storage; }

	}
	public class ToolStorageDeed : BaseStorageDeed
	{
        [Constructable]
        public ToolStorageDeed() : base(ToolStorage.Storage) { }
		public ToolStorageDeed(Serial serial) : base(serial) { }
		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0);
		}
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}
