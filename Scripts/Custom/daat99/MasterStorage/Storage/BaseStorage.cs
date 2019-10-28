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
using System.Collections.Generic;
using Server;
using Server.Items;

namespace daat99
{
	public class BaseStorage
	{
		private static Type[] defaultStoredTypes = new Type[0] { };
		public virtual Type[] DefaultStoredTypes { get { return BaseStorage.defaultStoredTypes; } }
		private List<Type> storedTypes;
		public List<Type> StoredTypes
		{
			get { return storedTypes; }
			private set { storedTypes = value; }
		}

		protected static BaseStorage singleton;
		public static BaseStorage Storage { get{ if ( singleton == null ) singleton = new BaseStorage(); return singleton; } }
		public virtual BaseStorage GetStorage() { return BaseStorage.Storage; }
		private string name;
		public virtual string Name { get { return name; } }
		protected BaseStorage()
		{
			storedTypes = new List<Type>(DefaultStoredTypes);
        }

		//deserialize constructor
		internal BaseStorage(GenericReader reader)
		{
			int version = reader.ReadInt();

			name = reader.ReadString();

			int count = reader.ReadInt();
			StoredTypes = new List<Type>(count);
			while (count-- > 0)
			{
				try
				{
					Type type = Type.GetType(reader.ReadString());
					if (type != null && !StoredTypes.Contains(type))
						StoredTypes.Add(type);
				}
				catch { }
			}
		}
		public virtual bool IsItemStorable(Item itemToCheck)
		{
			if (!CanStoreItemLootType(itemToCheck))
				return false;
			return IsTypeStorable(itemToCheck.GetType());
		}
		public virtual bool IsTypeStorable(Type typeToCheck)
		{
			return IsTypeStorable(typeToCheck, true);
		}

		public virtual bool IsTypeStorable(Type typeToCheck, bool canBeEqual)
		{
			foreach (Type type in StoredTypes)
			{
				if ((type.IsInterface && type.IsAssignableFrom(typeToCheck)) ||
					 ((canBeEqual && typeToCheck == type) || typeToCheck.IsSubclassOf(type)))
					return true;
			}
			return false;
		}

		public virtual void RemoveStorableType(Type typeToRemove)
		{
			if (StoredTypes.Contains(typeToRemove))
				StoredTypes.Remove(typeToRemove);
		}

		public virtual void AddStorableType(Type typeToAdd)
		{
			if (IsTypeStorable(typeToAdd, false))
				return;
			if (StoredTypes.Contains(typeToAdd))
				return;
			StoredTypes.Add(typeToAdd);
		}

		public virtual void ResetTypesList()
		{
			StoredTypes = new List<Type>(DefaultStoredTypes);
		}

		public void Serialize(GenericWriter writer)
		{
			writer.Write(0); //version

			writer.Write(Name);

			writer.Write(StoredTypes.Count);
			foreach (Type type in StoredTypes)
				writer.Write(type.FullName);
		}

		public virtual bool CanStoreItemLootType( Item item )
		{
			if (item.Insured || item.LootType == LootType.Blessed || item.LootType == LootType.Newbied)
				return false;
			return true;
		}
		
		public virtual Dictionary<Type, int> GetStorableTypesFromItem(Item item)
		{
			Dictionary<Type, int> types = new Dictionary<Type, int>();
			if (!IsItemStorable(item))
				return types;
			IUsesRemaining iUsesRemainingItem = item as IUsesRemaining;
			if (iUsesRemainingItem != null)
			{
				types.Add(item.GetType(), iUsesRemainingItem.UsesRemaining);
			}
			else if (item.Stackable)
			{
				types.Add(item.GetType(), item.Amount);
			}
			else
				types.Add(item.GetType(), 1);
			return types;
		}
	}
}
