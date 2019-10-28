using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class DreamlandKillFloorTile : Item
	{
		private bool m_KillPlayers = true;
		private bool m_KillMonsters = false;

		[CommandProperty(AccessLevel.GameMaster)]
		public bool Players {get{ return m_KillPlayers; } set{ m_KillPlayers = value; } }

		[CommandProperty(AccessLevel.GameMaster)]
		public bool Monsters {get{ return m_KillMonsters; } set{ m_KillMonsters = value; } }

		[Constructable]
		public DreamlandKillFloorTile() : base( 1310 )
		{
			Movable = false;
		}

        //UOSI - Returns false so you don't get sent back to dreamland.
        public override bool OnMoveOver( Mobile from )
		{
			PlayerMobile pm = from as PlayerMobile;
			BaseCreature bc = from as BaseCreature;

            if (pm != null && m_KillPlayers && pm.AccessLevel == AccessLevel.Player)
                pm.Kill();            

			return false;
		}

		public DreamlandKillFloorTile( Serial serial ) : base( serial )
		{ }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write((bool)m_KillPlayers);
			writer.Write((bool)m_KillMonsters);
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			m_KillPlayers = (bool)reader.ReadBool();
			m_KillMonsters = (bool)reader.ReadBool();
		}
	}
}

