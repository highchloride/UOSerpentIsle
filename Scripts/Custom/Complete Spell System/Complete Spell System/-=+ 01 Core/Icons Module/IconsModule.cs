using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.ACC.CM;

namespace Server.ACC.CSS.Modules
{
    public class IconsModule : Module
    {
        private Dictionary<Type, IconInfo> m_Icons;
        public Dictionary<Type, IconInfo> Icons { get { return m_Icons; } }
        //private Hashtable m_Icons;
        //public Hashtable Icons{ get{ return m_Icons; } }

        public override string Name() { return "Spell Icons"; }

        public IconsModule(Serial serial)
            : this(serial, null)
        {
        }

        public IconsModule(Serial serial, IconInfo info)
            : base(serial)
        {
            //m_Icons = new Hashtable();
            m_Icons = new Dictionary<Type, IconInfo>();

            Add(info);
        }

        public void Add(IconInfo info)
        {
            if (info == null)
                return;

            if (m_Icons.ContainsKey(info.SpellType))
                m_Icons[info.SpellType] = info;

            else
                m_Icons.Add(info.SpellType, info);
        }

        public IconInfo Get(Type type)
        {
            return m_Icons[type];
        }

        public bool Contains(Type type)
        {
            return m_Icons.ContainsKey(type);
        }

        public void Remove(Type type)
        {
            m_Icons.Remove(type);

            if (m_Icons.Count == 0)
                CentralMemory.RemoveModule(this.Owner, (Module)this);
        }

        public override void Append(Module mod, bool negatively)
        {
            IconsModule im = mod as IconsModule;
            List<Type> removeList = new List<Type>();

            foreach (KeyValuePair<Type,IconInfo> kvp in im.Icons)
            {
                if (negatively)
                    removeList.Add(kvp.Value.SpellType);
                else
                    Add(kvp.Value);
            }

            foreach (Type t in removeList)
            {
                Remove(t);
            }
            removeList.Clear();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version

            writer.Write(m_Icons.Count);
            foreach (KeyValuePair<Type,IconInfo> kvp in m_Icons)
            {
                kvp.Value.Serialize(writer);
            }
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            m_Icons = new Dictionary<Type, IconInfo>();

            int count = reader.ReadInt();
            for (int i = 0; i < count; i++)
            {
                IconInfo ii = new IconInfo(reader);
                if (ii.SpellType != null)
                    m_Icons.Add(ii.SpellType, ii);
            }
        }
    }
}