using System;
using Server;
using Server.Spells;
using Server.ACC.CM;

namespace Server.ACC.CSS.Modules
{
	public class IconInfo
	{
		private Type m_SpellType;
		public  Type SpellType{ get{ return m_SpellType; } set{ m_SpellType = value; } }

		private int m_Icon;
		public  int Icon{ get{ return m_Icon; } set{ m_Icon = value; } }

		private Point3D m_Location;
		public  Point3D Location{ get{ return m_Location; } set{ m_Location = value; } }

		private School m_School;
		public  School School{ get{ return m_School; } set{ m_School = value; } }

		public IconInfo( Type type, int icon, int x, int y, int back, School school )
		{
			m_SpellType = type;
			m_Icon      = icon;
			m_Location      = new Point3D( x, y, back );
			m_School    = school;
		}

		public IconInfo( GenericReader reader )
		{
			Deserialize( reader );
		}

		public void Serialize( GenericWriter writer )
		{
			writer.Write( (int)1 ); //version

			writer.Write( (string)m_SpellType.Name );
			writer.Write( (int)m_Icon );
			writer.Write( (Point3D)m_Location );
			writer.Write( (int)m_School );
		}

		public void Deserialize( GenericReader reader )
		{
			int version = reader.ReadInt();
			switch( version )
			{
				case 1:
				{
					m_SpellType = ScriptCompiler.FindTypeByName(reader.ReadString());
					m_Icon      = reader.ReadInt();
					m_Location      = reader.ReadPoint3D();
					m_School    = (School)reader.ReadInt();

					break;
				}
				case 0:
				{
					int bad = reader.ReadInt();
					m_Icon    = reader.ReadInt();
					m_Location    = reader.ReadPoint3D();

					m_SpellType = null;
					m_School  = School.Invalid;
					break;
				}
			}
		}
	}
}