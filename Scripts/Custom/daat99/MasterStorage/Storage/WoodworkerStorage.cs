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

//comment the USE_OWLTR3 line if you don't have OWLTR 2.0+
#define USE_OWLTR3

using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Engines.Craft;

namespace daat99
{
	public class WoodworkerStorage : BaseStorage
	{
		public WoodworkerStorage() : base() { }
		public WoodworkerStorage(GenericReader reader) : base(reader) { }
		public override string Name { get { return "Woodworker Storage"; } }
#if USE_OWLTR3
		private static Type[] defaultStoredTypes = new Type[] { typeof(BaseWoodBoard), typeof(BaseLog), typeof(Arrow), typeof(Bolt), typeof(Feather), typeof(Shaft) };

#else
		private static Type[] defaultStoredTypes = new Type[] 
		{ 
			typeof(Arrow), typeof(Bolt), typeof(Feather), typeof(Shaft),
			typeof(FrostwoodBoard),		typeof(FrostwoodLog),
			typeof(HeartwoodBoard),		typeof(HeartwoodLog),
			typeof(BloodwoodBoard),		typeof(BloodwoodLog),
			typeof(YewBoard),			typeof(YewLog),
			typeof(AshBoard),			typeof(AshLog),
			typeof(OakBoard),			typeof(OakLog),
			//NOTE: Make sure Board and Log are the LAST on the list when all the other wood types appear before them!!!
			typeof(Board), typeof(Log)
		};
#endif
		public override Type[] DefaultStoredTypes { get { return WoodworkerStorage.defaultStoredTypes; } }
		protected static new BaseStorage singleton;
		public static new BaseStorage Storage { get { if (singleton == null) singleton = new WoodworkerStorage(); return singleton; } }
		public override BaseStorage GetStorage() { return WoodworkerStorage.Storage; }

	}
	public class WoodworkerStorageDeed : BaseStorageDeed
	{
		[Constructable]
		public WoodworkerStorageDeed() : base(WoodworkerStorage.Storage) { }
		public WoodworkerStorageDeed(Serial serial) : base(serial) { }
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
