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

//comment the USE_OWLTR3 line if you don't have OWLTR 3.0+
//Runic storage isn't supported without the OWLTR3
//If you want the runic storage than you need to change your runic file so it'll match the OWLTR3 runic file (without the new ore/wood/leather types) and leave the "#define USE_OWLTR3" line as-is
#define USE_OWLTR3

using System;
using Server;
using Server.Items;
using System.Collections.Generic;

namespace daat99
{
#if USE_OWLTR3
	public class RunicStorage : BaseStorage
	{
		public RunicStorage() : base() { }
		public RunicStorage(GenericReader reader) : base(reader) { }
		public override string Name { get { return "Runic Storage"; } }
		private static Type[] defaultStoredTypes = new Type[] { typeof(BaseRunicTool) };
		public override Type[] DefaultStoredTypes { get { return RunicStorage.defaultStoredTypes; } }
		protected static new BaseStorage singleton; 
		public static new BaseStorage Storage { get { if (singleton == null) singleton = new RunicStorage(); return singleton; } }
		public override BaseStorage GetStorage() { return RunicStorage.Storage; }

        public override bool IsTypeStorable(Type typeToCheck)
        {
            return isValid(typeToCheck) && base.IsTypeStorable(typeToCheck);
        }
        public override bool IsTypeStorable(Type typeToCheck, bool canBeEqual)
        {
            return isValid(typeToCheck) && base.IsTypeStorable(typeToCheck, canBeEqual);
        }
        private bool isValid(Type typeToCheck)
        {
            return typeToCheck != typeof(BaseRunicTool);// && typeToCheck.GetInterface("IUsesRemaining") != null; 
        } 
		public override Dictionary<Type, int> GetStorableTypesFromItem(Item item)
		{
			Dictionary<Type, int> types = base.GetStorableTypesFromItem(item);
			BaseRunicTool runic = item as BaseRunicTool;
			if (runic != null)
			{
				types.Clear();
				types.Add(runic.GetType(), runic.UsesRemaining);
			}
			return types;
		}
	}

	public class RunicStorageDeed : BaseStorageDeed
	{
		[Constructable]
		public RunicStorageDeed() : base(RunicStorage.Storage) { }
		public RunicStorageDeed(Serial serial) : base(serial) { }
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
#endif
}
