using System;

namespace Server.ACC.CSS.Systems.Ranger
{
	public class RangerSpellbookGump : CSpellbookGump
	{
		public override string TextHue  { get{ return "336633"; } }
		public override int    BGImage  { get{ return 2203; } }
		public override int    SpellBtn { get{ return 2362; } }
		public override int    SpellBtnP{ get{ return 2361; } }
		public override string Label1   { get{ return "Ranger"; } }
		public override string Label2   { get{ return "Spells"; } }
		public override Type   GumpType { get{ return typeof( RangerSpellbookGump ); } }

		public RangerSpellbookGump( CSpellbook book ) : base( book )
		{
		}
	}
}
