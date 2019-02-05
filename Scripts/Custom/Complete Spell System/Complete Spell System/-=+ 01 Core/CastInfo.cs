using System;
using Server;
using Server.Spells;
using Server.ACC.CM;

namespace Server.ACC.CSS.Modules
{
    public class CastInfo
    {
        private Type m_SpellType;
        public Type SpellType { get { return m_SpellType; } set { m_SpellType = value; } }

        private School m_School;
        public School School { get { return m_School; } set { m_School = value; } }

        public CastInfo(Type type, School school)
        {
            m_SpellType = type;
            m_School = school;
        }

        public CastInfo(GenericReader reader)
        {
            Deserialize(reader);
        }

        public void Serialize(GenericWriter writer)
        {
            writer.Write((int)0); //version

            writer.Write((string)m_SpellType.Name);
            writer.Write((int)m_School);
        }

        public void Deserialize(GenericReader reader)
        {
            int version = reader.ReadInt();
            switch (version)
            {
                case 0:
                    {
                        m_SpellType = ScriptCompiler.FindTypeByName(reader.ReadString());
                        m_School = (School)reader.ReadInt();

                        break;
                    }
            }
        }
    }
}