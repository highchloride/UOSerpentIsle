using System;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using System.Collections.Generic;

namespace Server.Engines.XmlSpawner2
{
    public class LevelEquipXML : XmlAttachment
    {
		#region Weapon List Level 1 or Higher
		private static Type[] _Types1 = new[] 
		{ 
		typeof(BroadswordPlaceHolder)
		/* Example of Usage: typeof(BroadswordPlaceHolder),typeof(Axe),typeof(Katana) */
		};
		public static bool TypeLevel1Higher(Type test)
		{
			foreach ( var type in _Types1 )
			{
				if ( test == type || test.IsSubclassOf (type))
					return true;
			}

			return false;
		}
		#endregion
		
		#region Weapon List Level 20 or Higher
		private static Type[] _Types20 = new[] 
		{ 
		typeof(AxePlaceHolder)
		/* Example of Usage: typeof(AxePlaceHolder),typeof(Axe),typeof(Katana) */
		};
		public static bool TypeLevel20Higher(Type test)
		{
			foreach ( var type in _Types20 )
			{
				if ( test == type || test.IsSubclassOf (type))
					return true;
			}

			return false;
		}
		#endregion
		
		#region Weapon List Level 40 or Higher
		private static Type[] _Types40 = new[] 
		{ 
		typeof(BowPlaceHolder)
		/* Example of Usage: typeof(BowPlaceHolder),typeof(Axe),typeof(Katana) */
		};
		public static bool TypeLevel40Higher(Type test)
		{
			foreach ( var type in _Types40 )
			{
				if ( test == type || test.IsSubclassOf (type))
					return true;
			}

			return false;
		}
		#endregion
		
		#region Weapon List Level 60 or Higher
		private static Type[] _Types60 = new[] 
		{ 
		typeof(CutlassPlaceHolder)
		/* Example of Usage: typeof(CutlassPlaceHolder),typeof(Axe),typeof(Katana) */
		};
		public static bool TypeLevel60Higher(Type test)
		{
			foreach ( var type in _Types60 )
			{
				if ( test == type || test.IsSubclassOf (type))
					return true;
			}

			return false;
		}
		#endregion
		
		#region Weapon List Level 80 or Higher
		private static Type[] _Types80 = new[] 
		{ 
		typeof(DaggerPlaceHolder)
		/* Example of Usage: typeof(DaggerPlaceHolder),typeof(Axe),typeof(Katana) */
		};
		public static bool TypeLevel80Higher(Type test)
		{
			foreach ( var type in _Types80 )
			{
				if ( test == type || test.IsSubclassOf (type))
					return true;
			}

			return false;
		}
		#endregion
		
		#region Weapon List Level 100 or Higher
		private static Type[] _Types100 = new[] 
		{ 
		typeof(LongswordPlaceHolder)
		/* Example of Usage: typeof(LongswordPlaceHolder),typeof(Axe),typeof(Katana) */
		};
		public static bool TypeLevel100Higher(Type test)
		{
			foreach ( var type in _Types100 )
			{
				if ( test == type || test.IsSubclassOf (type))
					return true;
			}

			return false;
		}
		#endregion
		
        private string m_TestValue = null;
        private string m_FailMsg = null;
        private string m_PropertyListString = null;// string displayed in the properties list

        public LevelEquipXML(ASerial serial)
            : base(serial)
        {
        }

        [Attachable]
        public LevelEquipXML()
        {
            this.Test = String.Empty;
        }

        [Attachable]
        public LevelEquipXML(string name)
        {
            this.Name = name;
            this.Test = String.Empty;
        }

        [Attachable]
        public LevelEquipXML(string name, string test)
        {
            this.Name = name;
            this.Test = test;
        }

        [Attachable]
        public LevelEquipXML(string name, string test, double expiresin)
        {
            this.Name = name;
            this.Test = test;
            this.Expiration = TimeSpan.FromMinutes(expiresin);
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public string Test
        {
            get
            {
                return this.m_TestValue;
            }
            set
            {
                this.m_TestValue = value;
            }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public string FailMsg
        {
            get
            {
                return this.m_FailMsg;
            }
            set
            {
                this.m_FailMsg = value;
            }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public string PropertyListString
        {
            get
            {
                return this.m_PropertyListString;
            }
            set
            {
                this.m_PropertyListString = value;
                this.InvalidateParentProperties();
            }
        }
        public override bool CanEquip(Mobile from)
        {
			bool allowequip = true;
			
            if (from is PlayerMobile)
			{			
				XMLPlayerLevelAtt xmlplayer = (XMLPlayerLevelAtt)XmlAttach.FindAttachment(from, typeof(XMLPlayerLevelAtt));
				
				if (xmlplayer == null)
					return false;
				/* If return false, the weapon cannot be equiped */
				
				if (this.AttachedTo is BaseArmor)
				{
					((BaseArmor)this.AttachedTo).InvalidateProperties();

				}
				
				if (this.AttachedTo is BaseWeapon)
				{
					((BaseWeapon)this.AttachedTo).InvalidateProperties();
				
					var Level1Higher = TypeLevel1Higher(((BaseWeapon)this.AttachedTo).GetType());
					if ( Level1Higher )
					{
						if (xmlplayer.Levell >= 1)
							return allowequip;
						else
						{
							from.SendMessage("You must be at level 1 or higher to use this!");
							return false;
						}
					}				
					var Level20Higher = TypeLevel20Higher(((BaseWeapon)this.AttachedTo).GetType());
					if ( Level20Higher )
					{
						if (xmlplayer.Levell >= 20)
							return allowequip;
						else
						{
							from.SendMessage("You must be at level 20 or higher to use this!");
							return false;
						}
					}
					var Level40Higher = TypeLevel40Higher(((BaseWeapon)this.AttachedTo).GetType());
					if ( Level40Higher )
					{
						if (xmlplayer.Levell >= 40)
							return allowequip;
						else
						{
							from.SendMessage("You must be at level 40 or higher to use this!");
							return false;
						}
					}
					var Level60Higher = TypeLevel60Higher(((BaseWeapon)this.AttachedTo).GetType());
					if ( Level60Higher )
					{
						if (xmlplayer.Levell >= 60)
							return allowequip;
						else
						{
							from.SendMessage("You must be at level 60 or higher to use this!");
							return false;
						}
					}
					var Level80Higher = TypeLevel80Higher(((BaseWeapon)this.AttachedTo).GetType());
					if ( Level80Higher )
					{
						if (xmlplayer.Levell >= 80)
							return allowequip;
						else
						{
							from.SendMessage("You must be at level 80 or higher to use this!");
							return false;
						}
					}
					var Level100Higher = TypeLevel100Higher(((BaseWeapon)this.AttachedTo).GetType());
					if ( Level100Higher )
					{
						if (xmlplayer.Levell >= 100)
							return allowequip;
						else
						{
							from.SendMessage("You must be at level 100 or higher to use this!");
							return false;
						}
					}
				}

			}

            return allowequip;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1);
            // version 1
            writer.Write(this.m_PropertyListString);
            writer.Write(this.m_FailMsg);
            // version 0
            writer.Write(this.m_TestValue);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            switch (version)
            {
                case 1:
                    this.m_PropertyListString = reader.ReadString();
                    this.m_FailMsg = reader.ReadString();
                    goto case 0;
                case 0:
                    this.m_TestValue = reader.ReadString();
                    break;
            }
        }

        public override string DisplayedProperties(Mobile from)
        {
            return this.PropertyListString;
        }
    }
}