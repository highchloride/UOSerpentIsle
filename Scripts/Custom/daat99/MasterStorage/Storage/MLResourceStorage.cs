/*
 created by:
 * Hammerhand
*/
using System;
using Server;
using Server.Items;

namespace daat99
{
	public class MLResourceStorage : BaseStorage
	{
		public MLResourceStorage() : base() { }
		public MLResourceStorage(GenericReader reader) : base(reader) { }
		public override string Name { get { return "MLResource Storage"; } }
		private static Type[] defaultStoredTypes = new Type[] 
		{ 
				typeof(Blight),			typeof(LuminescentFungi),		typeof(CapturedEssence),	typeof(EyeOfTheTravesty), 
				typeof(Corruption),		typeof(DreadHornMane),			typeof(ParasiticPlant),		typeof(Muculent), 
				typeof(DiseasedBark),	typeof(BarkFragment),			typeof(GrizzledBones),		typeof(LardOfParoxysmus), 
				typeof(Scourge),		typeof(Putrefaction),			typeof(Taint),				typeof(PristineDreadHorn), 
				typeof(SwitchItem),		typeof(PerfectEmerald),			typeof(DarkSapphire),		typeof(Turquoise), 
				typeof(EcruCitrine),	typeof(WhitePearl),				typeof(FireRuby),			typeof(BlueDiamond), 
				typeof(BrilliantAmber) 
		};
		public override Type[] DefaultStoredTypes { get { return MLResourceStorage.defaultStoredTypes; } }
		protected static new BaseStorage singleton;
		public static new BaseStorage Storage { get { if (singleton == null) singleton = new MLResourceStorage(); return singleton; } }
		public override BaseStorage GetStorage() { return MLResourceStorage.Storage; }
	}
	public class MLResourceStorageDeed : BaseStorageDeed
	{
		[Constructable]
		public MLResourceStorageDeed() : base(MLResourceStorage.Storage) { }
		public MLResourceStorageDeed(Serial serial) : base(serial) { }
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
