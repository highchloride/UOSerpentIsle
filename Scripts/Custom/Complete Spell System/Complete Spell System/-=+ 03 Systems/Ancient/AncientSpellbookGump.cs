using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Network;
using Server.Prompts;


namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientSpellbookGump : CSpellbookGump
    {
        public override string TextHue { get { return "660066"; } }
        public override int BGImage { get { return 2203; } }
        public override int SpellBtn { get { return 2362; } }
        public override int SpellBtnP { get { return 2361; } }
        public override string Label1 { get { return "Ancient"; } }
        public override string Label2 { get { return "Spells"; } }
        public override Type GumpType { get { return typeof(AncientSpellbookGump); } }

        public AncientSpellbookGump(CSpellbook book)
            : base(book)
        {
        }
    }
}
