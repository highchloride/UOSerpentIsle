using System;
using System.Collections;
using System.Collections.Generic;
using Server.ContextMenus;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS
{
    public class CSpellScroll : Item
    {
        private Type m_SpellType;

        public Type SpellType
        {
            get
            {
                return m_SpellType;
            }
        }

        public CSpellScroll(Serial serial)
            : base(serial)
        {
        }

        [Constructable]
        public CSpellScroll(Type spellType, int itemID)
            : this(spellType, itemID, 1)
        {
        }

        [Constructable]
        public CSpellScroll(Type spellType, int itemID, int amount)
            : base(itemID)
        {
            Stackable = true;
            Weight = 1.0;
            Amount = amount;

            m_SpellType = spellType;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1); // version

            writer.Write((string)m_SpellType.Name);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    {
                        m_SpellType = ScriptCompiler.FindTypeByName(reader.ReadString());

                        break;
                    }
                case 0:
                    {
                        m_SpellType = null;
                        int bad = reader.ReadInt();

                        break;
                    }
            }
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!Multis.DesignContext.Check(from))
                return; // They are customizing

            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                return;
            }

            Spell spell = SpellInfoRegistry.NewSpell(m_SpellType, School.Invalid, from, this);

            if (spell != null)
                spell.Cast();
            else
                from.SendLocalizedMessage(502345); // This spell has been temporarily disabled.
        }
    }
}