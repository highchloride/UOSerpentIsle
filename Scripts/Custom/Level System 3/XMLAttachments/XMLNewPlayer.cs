using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Engines.XmlSpawner2
{
    public class XMLNewPlayer : XmlAttachment
    {
		private TimeSpan m_Duration = TimeSpan.FromSeconds(1.0); 
		private int m_Value = 0; 
		
        public XMLNewPlayer(ASerial serial) : base(serial)
        {
        }

        [Attachable]
        public XMLNewPlayer()
        {
        }

        [Attachable]
        public XMLNewPlayer(int value)
        {
        }
		
        [Attachable]
        public XMLNewPlayer(int value, double duration)
        {
			m_Duration = TimeSpan.FromSeconds(duration);
        }

		public override void OnAttach()
		{
			Configured c = new Configured();
			base.OnAttach();
			if(AttachedTo is PlayerMobile)
			{
				XMLPlayerLevelAtt xmlplayer = (XMLPlayerLevelAtt)XmlAttach.FindAttachment(((PlayerMobile)AttachedTo), typeof(XMLPlayerLevelAtt));
				if (xmlplayer == null)
				{
					return;
				}
				else
				{
					if (xmlplayer.Levell < c.WhatLevelToDelete)
					{
						((PlayerMobile)AttachedTo).AddStatMod(new StatMod(StatType.Dex, "XmlDex" + Name, c.StatBonusDex, TimeSpan.Zero));
						((PlayerMobile)AttachedTo).AddStatMod(new StatMod(StatType.Str, "XmlStr" + Name, c.StatBonusStr, TimeSpan.Zero));
						((PlayerMobile)AttachedTo).AddStatMod(new StatMod(StatType.Int, "XmlInt" + Name, c.StatBonusInt, TimeSpan.Zero));
						InvalidateParentProperties();
					}
					else
					{
						Delete();
					}
				}
			}
			else
				Delete();
		}
		public override void OnDelete()
		{
			Configured c = new Configured();
			base.OnDelete();
			if(AttachedTo is PlayerMobile)
			{
				/* Sanity Check, ensure the StatMod is Removed */
				((PlayerMobile)AttachedTo).AddStatMod(new StatMod(StatType.Dex, "XmlDex" + Name, c.StatBonusDex, m_Duration ));
				((PlayerMobile)AttachedTo).AddStatMod(new StatMod(StatType.Str, "XmlStr" + Name, c.StatBonusStr, m_Duration ));
				((PlayerMobile)AttachedTo).AddStatMod(new StatMod(StatType.Int, "XmlInt" + Name, c.StatBonusInt, m_Duration ));
				InvalidateParentProperties();
				//((PlayerMobile)AttachedTo).InvalidateProperties();
			}
		}
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize(writer);
			writer.Write( (int) 0 );
			// version 
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			// version 0
		}
		
    }
}
