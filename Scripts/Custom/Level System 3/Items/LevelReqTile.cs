using System; 
using Server; 
using Server.Items;
using Server.Gumps; 
using Server.Network; 
using Server.Menus; 
using Server.Mobiles;
using Server.Menus.Questions;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{ 
	public class LevelReqTile : Item 
	{ 
		private int m_RequiredLevel = 1;
		private int m_RequiredExactLevelVar = 1;
		private bool m_RequiredExactLevel = false;
		
        [CommandProperty(AccessLevel.GameMaster)]
        public int RequiredLevel
        {
            get { return m_RequiredLevel; }
            set { m_RequiredLevel = value; InvalidateProperties(); }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public int RequiredExactLevelVar
        {
            get { return m_RequiredExactLevelVar; }
            set { m_RequiredExactLevelVar = value; InvalidateProperties(); }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public bool RequiredExactLevel
        {
            get { return m_RequiredExactLevel; }
            set { m_RequiredExactLevel = value; InvalidateProperties(); }
        }

		[Constructable] 
		public LevelReqTile() : base( 6108 ) 
		{ 
			Movable = false; 
			Name = "Level Requirement Tile"; 
			Visible = false;
		} 

		public override bool OnMoveOver( Mobile from )
		{
			XMLPlayerLevelAtt gate = (XMLPlayerLevelAtt)XmlAttach.FindAttachment(from, typeof(XMLPlayerLevelAtt));
			if (gate == null)
				return false;
            if (RequiredExactLevel == true)
			{
				if (gate.Levell == RequiredExactLevelVar)
				{
					return true;
				}
				else
				{
					from.SendMessage( "You do not meet the level requirement for this gate." );
					return false;
				}
			}
			else if (RequiredExactLevel == false && gate.Levell < RequiredLevel)
			{
				return false;
			}
            else
            {
                return true;
            }
            return base.OnMoveOver( from );
		}
		public LevelReqTile( Serial serial ) : base( serial ) 
		{ 
		} 

		public override void Serialize( GenericWriter writer ) 
		{ 
			base.Serialize( writer ); 

			writer.Write((int) 0 ); // version 
			writer.Write((int) m_RequiredLevel);
			writer.Write((int) m_RequiredExactLevelVar);
			writer.Write(m_RequiredExactLevel);
		} 

		public override void Deserialize( GenericReader reader ) 
		{ 
			base.Deserialize( reader ); 

			int version = reader.ReadInt(); 
			m_RequiredLevel = reader.ReadInt();
			m_RequiredExactLevelVar = reader.ReadInt();
			m_RequiredExactLevel = reader.ReadBool();
		} 
	} 
}