/*
 created by:
		Hammerhand
*/
using System;
using Server;
using Server.Items;
using System.Collections.Generic;

namespace daat99
{
	public class CookStorage : BaseStorage
	{
		public CookStorage() : base() { }
        public CookStorage(GenericReader reader) : base(reader) { }
        public override string Name { get { return "Cook Storage"; } }
        private static Type[] defaultStoredTypes = new Type[] 
		{ 
			typeof(Food),		typeof(CookableFood),	typeof(Dough),		typeof(SweetDough),
			typeof(JarHoney),	typeof(BowlFlour),		typeof(WheatSheaf),	typeof(WoodenBowl),
			typeof(Eggshells),	typeof(SackFlour)
		};
        public override Type[] DefaultStoredTypes { get { return CookStorage.defaultStoredTypes; } }
		protected static new BaseStorage singleton;
        public static new BaseStorage Storage { get { if (singleton == null) singleton = new CookStorage(); return singleton; } }
        public override BaseStorage GetStorage() { return CookStorage.Storage; }
		public override Dictionary<Type, int> GetStorableTypesFromItem(Item item)
		{
			Dictionary<Type, int> types = base.GetStorableTypesFromItem(item);
			if (types == null)
				return new Dictionary<Type, int>(0);
			if (types.Count == 0)
				return types;

            //UOSI - I don't know what this does. Quantity has been removed from SackFlour as per Oct 2019 merge.
			SackFlour sack = item as SackFlour;
			//if (sack != null && sack.Quantity > 0 && types.ContainsKey(typeof(SackFlour)))
			//	types[typeof(SackFlour)] = sack.Quantity;
			return types;
		}
	}
	public class CookStorageDeed : BaseStorageDeed
	{
		[Constructable]
        public CookStorageDeed() : base(CookStorage.Storage) { }
        public CookStorageDeed(Serial serial) : base(serial) { }
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
