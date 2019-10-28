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

namespace daat99
{
	public class JewelerStorage : BaseStorage
	{
		public JewelerStorage() : base() { }
		public JewelerStorage(GenericReader reader) : base(reader) { }
		public override string Name { get { return "Jeweler Storage"; } }
		private static Type[] defaultStoredTypes = new Type[] { typeof(Amber), typeof(Amethyst), typeof(Citrine), typeof(Diamond), typeof(Emerald), typeof(Ruby), typeof(Sapphire), typeof(StarSapphire), typeof(Tourmaline) };
		public override Type[] DefaultStoredTypes { get { return JewelerStorage.defaultStoredTypes; } }
		protected static new BaseStorage singleton; 
		public static new BaseStorage Storage { get { if (singleton == null) singleton = new JewelerStorage(); return singleton; } }
		public override BaseStorage GetStorage() { return JewelerStorage.Storage; }
	}
	public class JewelerStorageDeed : BaseStorageDeed
	{
		[Constructable]
		public JewelerStorageDeed() : base(JewelerStorage.Storage) { }
		public JewelerStorageDeed(Serial serial) : base(serial) { }
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
