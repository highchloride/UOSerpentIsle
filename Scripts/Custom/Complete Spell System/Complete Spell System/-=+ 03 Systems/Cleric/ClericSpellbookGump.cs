using System;

namespace Server.ACC.CSS.Systems.Cleric
{
	public class ClericSpellbookGump : CSpellbookGump
	{
		public override string TextHue  { get{ return "336666"; } }
		public override int    BGImage  { get{ return 2203; } }
		public override int    SpellBtn { get{ return 2362; } }
		public override int    SpellBtnP{ get{ return 2361; } }
		public override string Label1   { get{ return "Cleric"; } }
		public override string Label2   { get{ return "Prayers"; } }
		public override Type   GumpType { get{ return typeof( ClericSpellbookGump ); } }

		public ClericSpellbookGump( ClericSpellbook book ) : base( book )
		{
		}
	}
}
