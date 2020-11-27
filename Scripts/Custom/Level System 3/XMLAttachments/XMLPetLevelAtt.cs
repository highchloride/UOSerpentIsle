using System;
using Server;
using Server.Misc;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using System.Collections.Generic;
using Server.ContextMenus;
using System.Threading.Tasks;

namespace Server.Engines.XmlSpawner2
{
    public class XMLPetLevelAtt : XmlAttachment
    {
        private int m_Levell;
        private int m_MaxLevel;
        private int m_Expp;
        private int m_ToLevell;
        private int m_kxp;
        private int m_SKPoints;
        private int m_StatPoints;
		private int m_StrPointsUsed;
		private int m_DexPointsUsed;
		private int m_IntPointsUsed;
		private int m_TotalStatPointsAquired;
		private bool m_BlockDefaultUse = true;
		private bool m_PetDeathToggle = false;
		
        [CommandProperty(AccessLevel.GameMaster)]
        public int Levell
        {
            get { return m_Levell; }
            set { m_Levell = value; InvalidateParentProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxLevel
        {
            get { return m_MaxLevel; }
            set { m_MaxLevel = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Expp
        {  
            get { return m_Expp = LevelCore.TExpPet(((BaseCreature)this.AttachedTo)); }
            set { m_Expp = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int ToLevell
        {
            get { return m_ToLevell; }
            set { m_ToLevell = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int kxp
        {
            get { return m_kxp; }
            set { m_kxp = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int SKPoints
        {
            get { return m_SKPoints; }
            set { m_SKPoints = value; }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public int StatPoints
        {
            get { return m_StatPoints; }
            set { m_StatPoints = value; }
        }
		
        [CommandProperty(AccessLevel.GameMaster)]
        public int StrPointsUsed
        {
            get { return m_StrPointsUsed; }
            set { m_StrPointsUsed = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int DexPointsUsed
        {
            get { return m_DexPointsUsed; }
            set { m_DexPointsUsed = value; }
        }
		
        [CommandProperty(AccessLevel.GameMaster)]
        public int IntPointsUsed
        {
            get { return m_IntPointsUsed; }
            set { m_IntPointsUsed = value; }
        }
		
        [CommandProperty(AccessLevel.GameMaster)]
        public int TotalStatPointsAquired
        {
            get { return m_TotalStatPointsAquired; }
            set { m_TotalStatPointsAquired = value; }
        }
		
		[CommandProperty(AccessLevel.GameMaster)]
        public bool BlockDefaultUse 
		{ 
			get { return m_BlockDefaultUse; } 
			set { m_BlockDefaultUse = value; } 
		}
		
		[CommandProperty(AccessLevel.GameMaster)]
        public bool PetDeathToggle 
		{ 
			get { return m_PetDeathToggle; } 
			set { m_PetDeathToggle = value; } 
		}

        public XMLPetLevelAtt(ASerial serial) : base(serial)
        {
        }

        [Attachable]
        public XMLPetLevelAtt()
        {
        }

        [Attachable]
        public XMLPetLevelAtt(int level)
        {
        }

        // disable the default use of the target
        public override bool BlockDefaultOnUse(Mobile from, object target)
        {
			ConfiguredPetXML c = new ConfiguredPetXML();
			
			if (c.EnableLvLMountChkonPetAtt == false)
				return false;
						
			var mount2 = ((BaseCreature)this.AttachedTo);
			XMLPlayerLevelAtt xmlplayer = (XMLPlayerLevelAtt)XmlAttach.FindAttachment(from, typeof(XMLPlayerLevelAtt));

			if (xmlplayer == null)
				return false;

			if (mount2 is BaseMount)
			{
				BaseMount mount = (BaseMount)mount2;
				if (mount is Beetle)
				{
					if (xmlplayer.Levell >= c.Beetle)
					{
						from.SendMessage("Your level allows you to ride {0}.", mount.Name);
						return false;
					}
					else
					{
						from.SendMessage("You must be Level {0} to ride me!", c.Beetle);	
						return (BlockDefaultUse);
					}
				}
				else if (mount is DesertOstard) 
				{
					if (xmlplayer.Levell >= c.DesertOstard)
					{
						from.SendMessage("Your level allows you to ride {0}.", mount.Name);
						return false;
					}
					else
					{
						from.SendMessage("You must be Level {0} to ride me!", c.DesertOstard);	
						return (BlockDefaultUse);
					}
				}
				else if (mount is FireSteed) 
				{
					if (xmlplayer.Levell >= c.FireSteed)
					{
						from.SendMessage("Your level allows you to ride {0}.", mount.Name);
						return false;
					}
					else
					{
						from.SendMessage("You must be Level {0} to ride me!", c.FireSteed);	
						return (BlockDefaultUse);
					}
				}
				else if (mount is ForestOstard) 
				{
					if (xmlplayer.Levell >= c.ForestOstard)
					{
						from.SendMessage("Your level allows you to ride {0}.", mount.Name);
						return false;
					}
					else
					{
						from.SendMessage("You must be Level {0} to ride me!", c.ForestOstard);	
						return (BlockDefaultUse);
					}
				}
				else if (mount is FrenziedOstard) 
				{
					if (xmlplayer.Levell >= c.FrenziedOstard)
					{
						from.SendMessage("Your level allows you to ride {0}.", mount.Name);
						return false;
					}
					else
					{
						from.SendMessage("You must be Level {0} to ride me!", c.FrenziedOstard);	
						return (BlockDefaultUse);
					}
				}
				else if (mount is HellSteed) 
				{
					if (xmlplayer.Levell >= c.HellSteed)
					{
						from.SendMessage("Your level allows you to ride {0}.", mount.Name);
						return false;
					}
					else
					{
						from.SendMessage("You must be Level {0} to ride me!", c.HellSteed);	
						return (BlockDefaultUse);
					}
				}
				else if (mount is Hiryu) 
				{
					if (xmlplayer.Levell >= c.Hiryu)
					{
						from.SendMessage("Your level allows you to ride {0}.", mount.Name);
						return false;
					}
					else
					{
						from.SendMessage("You must be Level {0} to ride me!", c.Hiryu);	
						return (BlockDefaultUse);
					}
				}
				else if (mount is Horse) 
				{
					if (xmlplayer.Levell >= c.Horse)
					{
						from.SendMessage("Your level allows you to ride {0}.", mount.Name);
						return false;
					}
					else
					{
						from.SendMessage("You must be Level {0} to ride me!", c.Horse);	
						return (BlockDefaultUse);
					}
				}
				else if (mount is Kirin) 
				{
					if (xmlplayer.Levell >= c.Kirin)
					{
						from.SendMessage("Your level allows you to ride {0}.", mount.Name);
						return false;
					}
					else
					{
						from.SendMessage("You must be Level {0} to ride me!", c.Kirin);	
						return (BlockDefaultUse);
					}
				}
				else if (mount is LesserHiryu) 
				{
					if (xmlplayer.Levell >= c.LesserHiryu)
					{
						from.SendMessage("Your level allows you to ride {0}.", mount.Name);
						return false;
					}
					else
					{
						from.SendMessage("You must be Level {0} to ride me!", c.LesserHiryu);	
						return (BlockDefaultUse);
					}
				}
				else if (mount is Nightmare) 
				{
					if (xmlplayer.Levell >= c.Nightmare)
					{
						from.SendMessage("Your level allows you to ride {0}.", mount.Name);
						return false;
					}
					else
					{
						from.SendMessage("You must be Level {0} to ride me!", c.Nightmare);	
						return (BlockDefaultUse);
					}
				}
				else if (mount is RidableLlama) 
				{
					if (xmlplayer.Levell >= c.RidableLlama)
					{
						from.SendMessage("Your level allows you to ride {0}.", mount.Name);
						return false;
					}
					else
					{
						from.SendMessage("You must be Level {0} to ride me!", c.RidableLlama);	
						return (BlockDefaultUse);
					}
				}
				else if (mount is Ridgeback) 
				{
					if (xmlplayer.Levell >= c.Ridgeback)
					{
						from.SendMessage("Your level allows you to ride {0}.", mount.Name);
						return false;
					}
					else
					{
						from.SendMessage("You must be Level {0} to ride me!", c.Ridgeback);	
						return (BlockDefaultUse);
					}
				}
				else if (mount is SavageRidgeback) 
				{
					if (xmlplayer.Levell >= c.SavageRidgeback)
					{
						from.SendMessage("Your level allows you to ride {0}.", mount.Name);
						return false;
					}
					else
					{
						from.SendMessage("You must be Level {0} to ride me!", c.SavageRidgeback);	
						return (BlockDefaultUse);
					}
				}
				else if (mount is ScaledSwampDragon) 
				{
					if (xmlplayer.Levell >= c.ScaledSwampDragon)
					{
						from.SendMessage("Your level allows you to ride {0}.", mount.Name);
						return false;
					}
					else
					{
						from.SendMessage("You must be Level {0} to ride me!", c.ScaledSwampDragon);	
						return (BlockDefaultUse);
					}
				}
				else if (mount is SeaHorse) 
				{
					if (xmlplayer.Levell >= c.SeaHorse)
					{
						from.SendMessage("Your level allows you to ride {0}.", mount.Name);
						return false;
					}
					else
					{
						from.SendMessage("You must be Level {0} to ride me!", c.SeaHorse);	
						return (BlockDefaultUse);
					}
				}
				else if (mount is SilverSteed) 
				{
					if (xmlplayer.Levell >= c.SilverSteed)
					{
						from.SendMessage("Your level allows you to ride {0}.", mount.Name);
						return false;
					}
					else
					{
						from.SendMessage("You must be Level {0} to ride me!", c.SilverSteed);	
						return (BlockDefaultUse);
					}
				}
				else if (mount is SkeletalMount) 
				{
					if (xmlplayer.Levell >= c.SkeletalMount)
					{
						from.SendMessage("Your level allows you to ride {0}.", mount.Name);
						return false;
					}
					else
					{
						from.SendMessage("You must be Level {0} to ride me!", c.SkeletalMount);	
						return (BlockDefaultUse);
					}
				}
				else if (mount is SwampDragon) 
				{
					if (xmlplayer.Levell >= c.SwampDragon)
					{
						from.SendMessage("Your level allows you to ride {0}.", mount.Name);
						return false;
					}
					else
					{
						from.SendMessage("You must be Level {0} to ride me!", c.SwampDragon);	
						return (BlockDefaultUse);
					}
				}
				else if (mount is Unicorn) 
				{
					if (xmlplayer.Levell >= c.Unicorn)
					{
						from.SendMessage("Your level allows you to ride {0}.", mount.Name);
						return false;
					}
					else
					{
						from.SendMessage("You must be Level {0} to ride me!", c.Unicorn);	
						return (BlockDefaultUse);
					}
				}
				else if (mount is Reptalon) 
				{
					if (xmlplayer.Levell >= c.Reptalon)
					{
						from.SendMessage("Your level allows you to ride {0}.", mount.Name);
						return false;
					}
					else
					{
						from.SendMessage("You must be Level {0} to ride me!", c.Reptalon);	
						return (BlockDefaultUse);
					}
				}
				else if (mount is WildTiger) 
				{
					if (xmlplayer.Levell >= c.WildTiger)
					{
						from.SendMessage("Your level allows you to ride {0}.", mount.Name);
						return false;
					}
					else
					{
						from.SendMessage("You must be Level {0} to ride me!", c.WildTiger);	
						return (BlockDefaultUse);
					}
				}
				else if (mount is Windrunner) 
				{
					if (xmlplayer.Levell >= c.Windrunner)
					{
						from.SendMessage("Your level allows you to ride {0}.", mount.Name);
						return false;
					}
					else
					{
						from.SendMessage("You must be Level {0} to ride me!", c.Windrunner);	
						return (BlockDefaultUse);
					}
				}
				else if (mount is Lasher) 
				{
					if (xmlplayer.Levell >= c.Lasher)
					{
						from.SendMessage("Your level allows you to ride {0}.", mount.Name);
						return false;
					}
					else
					{
						from.SendMessage("You must be Level {0} to ride me!", c.Lasher);	
						return (BlockDefaultUse);
					}
				}
				else if (mount is Eowmu) 
				{
					if (xmlplayer.Levell >= c.Eowmu)
					{
						from.SendMessage("Your level allows you to ride {0}.", mount.Name);
						return false;
					}
					else
					{
						from.SendMessage("You must be Level {0} to ride me!", c.Eowmu);	
						return (BlockDefaultUse);
					}
				}
				else if (mount is DreadWarhorse) 
				{
					if (xmlplayer.Levell >= c.DreadWarhorse)
					{
						from.SendMessage("Your level allows you to ride {0}.", mount.Name);
						return false;
					}
					else
					{
						from.SendMessage("You must be Level {0} to ride me!", c.DreadWarhorse);	
						return (BlockDefaultUse);
					}
				}
				else if (mount is CuSidhe) 
				{
					if (xmlplayer.Levell >= c.CuSidhe)
					{
						from.SendMessage("Your level allows you to ride {0}.", mount.Name);
						return false;
					}
					else
					{
						from.SendMessage("You must be Level {0} to ride me!", c.CuSidhe);	
						return (BlockDefaultUse);
					}
				}
				else
				{
					/*	If mount isn't on the list and has an attachment it will hit this return
						statement. Add it to this list and also add an entry to configuration file. */
					return false;
				}
				
			}
			if (mount2 is BaseCreature)
			{
				return false;
			}
				
			from.SendMessage("You are not at the right Level to ride me!");	
            return (BlockDefaultUse); //Fail catch for Creatures on this list but have the attachment
        }		
		
		public override void OnAttach()
		{
			base.OnAttach();
			if(AttachedTo is BaseCreature)
			{
				XMLPetAttacksBonus xmlpetbonus = (XMLPetAttacksBonus)XmlAttach.FindAttachment((((BaseCreature)this.AttachedTo)), typeof(XMLPetAttacksBonus));
				ConfiguredPetXML cp = new ConfiguredPetXML();
				if(this.m_Levell == 0)
				{
					m_Levell = 1;
					m_MaxLevel = cp.StartMaxLvl;
					m_ToLevell = 100;
				}
				
				if (xmlpetbonus != null)
				{
					if (cp.PetAttackBonus == false)
						xmlpetbonus.Delete();
					return;
				}
				else
				{
					if (cp.PetAttackBonus == true)
					{
						XmlAttach.AttachTo(((BaseCreature)this.AttachedTo), new XMLPetAttacksBonus());
					}
					else
						return;
				}
			}
			else
				Delete();
		}
		public override void OnDelete()
		{
			base.OnDelete();
			if(AttachedTo is BaseCreature)
			{
				InvalidateParentProperties();
			}
		}
		
		public override bool HandlesOnMovement { get { return true; } }
		public override void OnMovement(MovementEventArgs e )
		{
			base.OnMovement(e);
			ConfiguredPetXML cpd = new ConfiguredPetXML(); 
			if (cpd.LoseExpLevelOnDeath == true)
			{
				if (((BaseCreature)this.AttachedTo).Hits == 0 && PetDeathToggle == false)
				{				
					LevelHandlerPet.PetOnDeath(((BaseCreature)this.AttachedTo));
					this.PetDeathToggle = true;
				}
				else
				{
					if (((BaseCreature)this.AttachedTo).Hits > 0 )
					{
						this.PetDeathToggle = false;
					}
				}
			}
		}
		/*
		public override bool HandlesOnKilled  { get { return true; } }
		public override void OnKilled(Mobile killed, Mobile killer)
		{
			XMLPlayerLevelAtt xmlplayer = (XMLPlayerLevelAtt)XmlAttach.FindAttachment(killer, typeof(XMLPlayerLevelAtt));
			if (xmlplayer is null)
				return;
			
			killed.SayTo(killer, "You killed me!");
			
		}
		*/
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize(writer);
			
			writer.Write( (int) 1 );
			// version 1
			writer.Write(m_BlockDefaultUse);
			writer.Write(m_PetDeathToggle);
			// version 0
            writer.Write((int) m_Levell);
            writer.Write((int) m_MaxLevel);
            writer.Write((int) m_Expp);
            writer.Write((int) m_ToLevell);
            writer.Write((int) m_kxp);
            writer.Write((int) m_SKPoints);
            writer.Write((int) m_StatPoints);
            writer.Write((int) m_StrPointsUsed);
            writer.Write((int) m_DexPointsUsed);
            writer.Write((int) m_IntPointsUsed);
            writer.Write((int) m_TotalStatPointsAquired);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
			// version 0
            switch (version)
            {
                case 1:
                    m_BlockDefaultUse = reader.ReadBool();
					m_PetDeathToggle  = reader.ReadBool();
                    goto case 0;
				case 0:
				// version 0
				m_Levell = reader.ReadInt();
				m_MaxLevel = reader.ReadInt();
				m_Expp = reader.ReadInt();
				m_ToLevell = reader.ReadInt();
				m_kxp = reader.ReadInt();
				m_SKPoints = reader.ReadInt();
				m_StatPoints = reader.ReadInt();
				m_StrPointsUsed = reader.ReadInt();
				m_DexPointsUsed = reader.ReadInt();
				m_IntPointsUsed = reader.ReadInt();
				m_TotalStatPointsAquired = reader.ReadInt();
				break;
			}
		}
    }
}
