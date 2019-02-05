using System;

namespace Server.ACC.CSS.Systems.Rogue
{
	public class RogueSpellbookGump : CSpellbookGump
	{
		public override string TextHue  { get{ return "660066"; } }
		public override int    BGImage  { get{ return 2203; } }
		public override int    SpellBtn { get{ return 2362; } }
		public override int    SpellBtnP{ get{ return 2361; } }
		public override string Label1   { get{ return "Rogue"; } }
		public override string Label2   { get{ return "Abilities"; } }
		public override Type   GumpType { get{ return typeof( RogueSpellbookGump ); } }

		public RogueSpellbookGump( CSpellbook book ) : base( book )
		{
		}
	}
}
