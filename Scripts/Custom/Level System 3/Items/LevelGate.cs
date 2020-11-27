using System;
using Server;
using Server.Misc;
using Server.Gumps;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
	public class LevelGate : Moongate
	{
		private int m_RequiredLevel; //set 0 by default
        [CommandProperty(AccessLevel.GameMaster)]
        public int RequiredLevel
        {
            get { return m_RequiredLevel; }
            set { m_RequiredLevel = value; InvalidateProperties(); }
        }
		
		[Constructable]
		public LevelGate () : base( )  
		{
			Movable = false;
			Name = "Level Gate";
            Hue = 1922;
			Light = LightType.Circle300;
			this.ItemID = 0x1FD4;
        }

		public override bool OnMoveOver( Mobile m )
        {	
			XMLPlayerLevelAtt gate = (XMLPlayerLevelAtt)XmlAttach.FindAttachment(m, typeof(XMLPlayerLevelAtt));
			 
			if (gate != null && gate.Levell >= RequiredLevel && m is PlayerMobile)
			{		
				if (m.InRange(GetWorldLocation(), 1))
					CheckGate(m, 1);
			}
			else
			{
				if (m is PlayerMobile)
				{
					m.SendMessage( "You do not meet the level requirement for this gate." );
					return false; 
				}
			}
			return true;
		}
		
		public override void OnDoubleClick(Mobile m)
		{
			XMLPlayerLevelAtt gate = (XMLPlayerLevelAtt)XmlAttach.FindAttachment(m, typeof(XMLPlayerLevelAtt));
			 
			if (gate == null)
			{
				m.SendMessage( "You do not meet the level requirement for this gate." );
			}
			
			else if (gate.Levell != RequiredLevel)
			{		
				m.SendMessage( "You do not meet the level requirement for this gate." );
			}
			else 
			{
				if (m.InRange(GetWorldLocation(), 1))
					CheckGate(m, 1);
			}
		}
		
		public LevelGate ( Serial serial ) : base( serial )
		{
		}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
            writer.Write((int) m_RequiredLevel);
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			m_RequiredLevel = reader.ReadInt();
		}
	}
}
