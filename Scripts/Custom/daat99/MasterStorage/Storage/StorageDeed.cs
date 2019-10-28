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
using Server;
using Server.Mobiles;
using System;

namespace daat99
{
	public class BaseStorageDeed : Item
	{
		private BaseStorage storage;
		[CommandProperty(AccessLevel.GameMaster)]
		public BaseStorage Storage { get { return storage; } private set { storage = value; } }

		[Constructable]
		public BaseStorageDeed(BaseStorage storage)
			: base(0x14F0)
		{
			Storage = storage;
			Weight = 1.0;
			Movable = true;
			Name = "Master Storage Deed: " + storage.Name;
		}

		public BaseStorageDeed(Serial serial) : base(serial) { }

		public override void OnDoubleClick(Mobile from)
		{
			if (!IsChildOf(from.Backpack))
				from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
			else
			{
				MasterStorage backpack = MasterStorageUtils.GetMasterStorage(from as PlayerMobile);
				if (backpack == null || backpack.Deleted)
					from.SendMessage(1173, "You must have your Master Storage in your backpack!");
				else if (backpack.HasStorage(Storage))
					from.SendMessage(1173, "You can store these items already.");
				else if (this.Deleted)
					from.SendMessage(1173, "You can't use this deed anymore.");
				else
				{
					backpack.AddStorage(storage);
					this.Delete();
					from.SendMessage(1173, "You can store {0} items now.", Storage.Name);
				}
			}
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0);
			Storage.Serialize(writer);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			Storage = new BaseStorage(reader);
		} 
    /*    public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); //note version is 1 now
            writer.Write(this.GetType().FullName); //serialize the type of the real storage
            Storage.Serialize(writer);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            if (version >= 1)
            {
                string typeName = reader.ReadString(); //read the storage type name
                if (!string.IsNullOrEmpty(typeName)) //make sure we have a name
                {
                    Type storageType = ScriptCompiler.FindTypeByFullName(typeName); //get the storage type
                    if (storageType != null) //make sure we have a type
                    {
                        Storage = Activator.CreateInstance(storageType, new object[] { reader }) as BaseStorage ; //create a storage from the type we saved with the data stream we have
                    }
                }
            }
            else
            {
                Storage = new BaseStorage(reader); //old behavior so we can load a save file from old server
            }
            if (Storage == null)
            {
                //something went wrong and we can't load the storage deed correctly, change the name so the GM/Admin can replace it with a new one.
                this.Name = "This deed is broken, please return it to a GM for replacement!!!";
            } 
        } */
	} 
} 
