using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.ACC.CM;

namespace Server.ACC.CSS.Modules
{
    public class CastCommandsModule : Module
    {
        private Dictionary<string, CastInfo> m_CastCommands;
        public Dictionary<string, CastInfo> CastCommands { get { return m_CastCommands; } }

        public override string Name() { return "Cast Commands"; }

        public CastCommandsModule(Serial serial)
            : this(serial, null, null)
        {
        }

        public CastCommandsModule(Serial serial, string castCommand, CastInfo info)
            : base(serial)
        {
            m_CastCommands = new Dictionary<string, CastInfo>();

            Add(castCommand, info);
        }

        public void Add(string castCommand, CastInfo info)
        {
            if (castCommand == null || castCommand.Length == 0 || info == null)
                return;

            if (m_CastCommands.ContainsKey(castCommand.ToLower()))
                m_CastCommands[castCommand.ToLower()] = info;
            else
                m_CastCommands.Add(castCommand.ToLower(), info);
        }

        public CastInfo Get(string castCommand)
        {

            return Contains(castCommand) ? m_CastCommands[castCommand.ToLower()] : null;
        }

        public string GetCommandForInfo(CastInfo castInfo)
        {
            foreach (KeyValuePair<string, CastInfo> kvp in m_CastCommands)
            {
                if (kvp.Value.SpellType == castInfo.SpellType)
                {
                    return kvp.Key.ToLower();
                }
            }

            return "";
        }

        public bool Contains(string castCommand)
        {
            return m_CastCommands.ContainsKey(castCommand.ToLower());
        }

        public void Remove(string castCommand)
        {
            m_CastCommands.Remove(castCommand.ToLower());

            if (m_CastCommands.Count == 0)
                CentralMemory.RemoveModule(this.Owner, (Module)this);
        }

        public void RemoveCommandByInfo(CastInfo castInfo)
        {
            string command = "";
            foreach (KeyValuePair<string, CastInfo> kvp in m_CastCommands)
            {
                if (kvp.Value.SpellType == castInfo.SpellType)
                {
                    command = kvp.Key.ToLower();
                    break;
                }
            }

            if (command.Length > 0)
                Remove(command);
        }

        public override void Append(Module mod, bool negatively)
        {
            CastCommandsModule m = mod as CastCommandsModule;
            List<string> removeList = new List<string>();

            foreach (KeyValuePair<string, CastInfo> kvp in m.CastCommands)
            {
                if (negatively)
                    removeList.Add(kvp.Key);
                else
                    Add(kvp.Key, kvp.Value);
            }

            foreach (string s in removeList)
            {
                Remove(s);
            }
            removeList.Clear();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version

            writer.Write(m_CastCommands.Count);
            foreach (KeyValuePair<string, CastInfo> kvp in m_CastCommands)
            {
                writer.Write(kvp.Key);
                kvp.Value.Serialize(writer);
            }
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            m_CastCommands = new Dictionary<string, CastInfo>();

            int count = reader.ReadInt();
            for (int i = 0; i < count; i++)
            {
                string s = reader.ReadString();
                CastInfo ii = new CastInfo(reader);
                if (ii.SpellType != null)
                    m_CastCommands.Add(s, ii);
            }
        }
    }
}