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
using System.Collections.Generic;
using Server.Engines.Craft;
using Server.Network;
using Server.Mobiles;
using Server.Targeting;
using Server.ContextMenus;

namespace daat99
{
    public class BardStorage : BaseStorage
    {
        public BardStorage() : base() { }
        public BardStorage(GenericReader reader) : base(reader) { }
        public override string Name { get { return "Bard Storage"; } }
        private static Type[] defaultStoredTypes = new Type[] { typeof(BaseInstrument) };
        public override Type[] DefaultStoredTypes { get { return BardStorage.defaultStoredTypes; } }
        protected static new BaseStorage singleton;
        public static new BaseStorage Storage { get { if (singleton == null) singleton = new BardStorage(); return singleton; } }
        public override BaseStorage GetStorage() { return BardStorage.Storage; }

    }
	public class BardStorageDeed : BaseStorageDeed
	{
		[Constructable]
		public BardStorageDeed() : base(BardStorage.Storage) { }
		public BardStorageDeed(Serial serial) : base(serial) { }
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
