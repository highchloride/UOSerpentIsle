using System;
using System.Collections;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS
{
	public class SpellInfoRegistry
	{
		private static Hashtable m_Registry = new Hashtable();
		public  static Hashtable Registry{ get{ return m_Registry; } set{ m_Registry = value; } }

		public static void Register( Type type, string name, string desc, string regs, string info, int icon, int back, School school )
		{
			if( !m_Registry.ContainsKey( school ) )
				m_Registry.Add( school, new ArrayList() );

			if( ((ArrayList)m_Registry[school]).Count < 64 )
			{
				CSpellInfo inf = new CSpellInfo( type, name, desc, regs, info, icon, back, school, true );

				((ArrayList)m_Registry[school]).Add( inf );
			}
			else
				Console.WriteLine( "You cannot register more than 64 spells to the {0} school.", school );
		}

		public static void DisEnable( School school, Type type, bool enabled )
		{
			foreach( CSpellInfo info in (ArrayList)m_Registry[school] )
			{
				if( info.Type == type )
					info.Enabled = enabled;
			}
		}

		public static bool CheckRegistry( School school, Type type )
		{
			if( !CSS.Running )
				return false;

			if( school == School.Invalid )
				return true;

			if( m_Registry.ContainsKey( school ) )
			{
				foreach( CSpellInfo info in (ArrayList)m_Registry[school] )
				{
					if( info.Type == type && info.Enabled )
						return true;
				}

				return false;
			}

			return false;
		}

		public static bool CheckBooksForSchool( Mobile mobile, Type type )
		{
			Item item = mobile.FindItemOnLayer( Layer.OneHanded );
			if( item is CSpellbook && CheckRegistry( ((CSpellbook)item).School, type ) )
				return true;

			Container pack = mobile.Backpack;
			if( pack == null )
				return false;

			for( int i = 0; i < pack.Items.Count; i++ )
			{
				item = (Item)pack.Items[i];
				if( item is CSpellbook && CheckRegistry( ((CSpellbook)item).School, type ) )
					return true;
			}

			return false;
		}

		public static CSpellInfo GetInfo( School school, Type type )
		{
			if( m_Registry.ContainsKey( school ) )
			{
				foreach( CSpellInfo info in (ArrayList)m_Registry[school] )
				{
					if( info.Type == type && info.Enabled )
						return info;
				}
			}

			return null;
		}

		public static ArrayList GetSpellsForSchool( School school )
		{
			ArrayList ret = new ArrayList();
			if( m_Registry.ContainsKey( school ) )
			{
				foreach( CSpellInfo info in (ArrayList)m_Registry[school] )
				{
					ret.Add( info.Type );
				}
			}

			return ret;
		}

		private static object[] m_Params = new object[2];

		public static Spell NewSpell( Type type, School school, Mobile caster, Item scroll )
		{
			if( !CSS.Running )
				return null;
			else if( school == School.Invalid && scroll == null && !CheckBooksForSchool( caster, type ) )
				return null;
			else if( !CheckRegistry( school, type ) )
				return null;
			else if( !SpellRestrictions.UseRestrictions && SpellRestrictions.CheckRestrictions( caster, type ) )
				return null;

			m_Params[0] = caster;
			m_Params[1] = scroll;

			return (Spell)Activator.CreateInstance( type, m_Params );;
		}
	}

	public class CSpellInfo
	{
		private Type   m_Type;
		private string m_Name;
		private string m_Desc;
		private string m_Regs;
		private string m_Info;
		private int    m_Icon;
		private int    m_Back;
		private School m_School;
		private bool   m_Enabled;

		public Type   Type   { get{ return m_Type; } }
		public string Name   { get{ return m_Name; } }
		public string Desc   { get{ return m_Desc; } }
		public string Regs   { get{ return m_Regs; } }
		public string Info   { get{ return m_Info; } }
		public int    Icon   { get{ return m_Icon; } }
		public int    Back   { get{ return m_Back; } }
		public School School { get{ return m_School; } }
		public bool   Enabled{ get{ return m_Enabled; } set{ m_Enabled = value; } }

		public CSpellInfo( Type type, string name, string desc, string regs, string info, int icon, int back, School school, bool enabled )
		{
			m_Type    = type;
			m_Name    = name;
			m_Desc    = desc;
			m_Regs    = regs;
			m_Info    = info;
			m_Icon    = icon;
			m_Back    = back;
			m_School  = school;
			m_Enabled = enabled;
		}

		public CSpellInfo( GenericReader reader )
		{
			Deserialize( reader );
		}

		public void Serialize( GenericWriter writer )
		{
			writer.Write( (int)0 ); //version

			writer.Write( (string)m_Type.Name );
			writer.Write( (int)m_School );
			writer.Write( (bool)m_Enabled );
		}

		public void Deserialize( GenericReader reader )
		{
			int version = reader.ReadInt();

			switch( version )
			{
				case 0:
				{
					m_Type = ScriptCompiler.FindTypeByName(reader.ReadString());
					m_School = (School)reader.ReadInt();
					m_Enabled = reader.ReadBool();
					break;
				}
			}
		}
	}
}