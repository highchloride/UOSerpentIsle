using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS
{
	public class CReagent
	{
		private static Type[] m_Types = new Type[4]
		{
			typeof( SpringWater ),
			typeof( DestroyingAngel ),
			typeof( PetrafiedWood ),
			typeof( Kindling )
		};

		public static Type SpringWater
		{
			get{ return m_Types[0]; }
			set{ m_Types[0] = value; }
		}
		public static Type DestroyingAngel
		{
			get{ return m_Types[1]; }
			set{ m_Types[1] = value; }
		}
		public static Type PetrafiedWood
		{
			get{ return m_Types[2]; }
			set{ m_Types[2] = value; }
		}
		public static Type Kindling
		{
			get{ return m_Types[3]; }
			set{ m_Types[3] = value; }
		}
	}
}