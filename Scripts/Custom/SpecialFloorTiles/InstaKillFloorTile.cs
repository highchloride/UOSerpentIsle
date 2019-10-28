using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class InstaKillFloorTile : Item
	{
		private bool m_KillPlayers = true;
		private bool m_KillMonsters = true;

		[CommandProperty(AccessLevel.GameMaster)]
		public bool Players {get{ return m_KillPlayers; } set{ m_KillPlayers = value; } }

		[CommandProperty(AccessLevel.GameMaster)]
		public bool Monsters {get{ return m_KillMonsters; } set{ m_KillMonsters = value; } }

		[Constructable]
		public InstaKillFloorTile() : base( 1310 )
		{
			Movable = false;
		}

		public override bool OnMoveOver( Mobile from )
		{
			PlayerMobile pm = from as PlayerMobile;
			BaseCreature bc = from as BaseCreature;

			if (pm != null && m_KillPlayers && pm.AccessLevel == AccessLevel.Player )
				pm.Kill();

			if (bc != null && m_KillPlayers && bc.AccessLevel == AccessLevel.Player )
				bc.Kill();

			return true;
		}

		public InstaKillFloorTile( Serial serial ) : base( serial )
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

