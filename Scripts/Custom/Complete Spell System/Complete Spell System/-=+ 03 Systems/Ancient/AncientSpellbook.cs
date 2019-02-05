using System;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientSpellbook : CSpellbook
    {
        public override School School { get { return School.Ancient; } }

        [Constructable]
        public AncientSpellbook()
            : this((ulong)0, CSSettings.FullSpellbooks)
        {
        }

        [Constructable]
        public AncientSpellbook(bool full)
            : this((ulong)0, full)
        {
        }

        [Constructable]
        public AncientSpellbook(ulong content, bool full)
            : base(content, 0xEFA, full)
        {
            Hue = 1355;
            Name = "Ancient Spellbook";
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.AccessLevel == AccessLevel.Player)
            {
                Container pack = from.Backpack;
                if (!(Parent == from || (pack != null && Parent == pack)))
                {
                    from.SendMessage("The spellbook must be in your backpack [and not in a container within] to open.");
                    return;
                }
                else if (SpellRestrictions.UseRestrictions && !SpellRestrictions.CheckRestrictions(from, this.School))
                {
                    return;
                }
            }

            from.CloseGump(typeof(AncientSpellbookGump));
            from.SendGump(new AncientSpellbookGump(this));
        }

        public AncientSpellbook(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
