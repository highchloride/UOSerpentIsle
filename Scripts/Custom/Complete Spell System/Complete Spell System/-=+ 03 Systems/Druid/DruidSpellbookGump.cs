using System;

namespace Server.ACC.CSS.Systems.Druid
{
	public class DruidSpellbookGump : CSpellbookGump
	{
		public override string TextHue  { get{ return "006600"; } }
		public override int    BGImage  { get{ return 2203; } }
		public override int    SpellBtn { get{ return 2224; } } //95?
		public override int    SpellBtnP{ get{ return 2224; } } //97?
		public override string Label1   { get{ return "Druid"; } }
		public override string Label2   { get{ return "Spells"; } }
		public override Type   GumpType { get{ return typeof( DruidSpellbookGump ); } }

		public DruidSpellbookGump( CSpellbook book ) : base( book )
		{
		}

/*
		public override void AddBorderDeco()
		{
			AddItem( 243, 215, 3307 );

			for( int i = 0; i < 6; i++ ) //Outer Left
				AddItem( 75, 75+i*42, 6024 );

			for( int i = 0; i < 6; i++ ) //Outer Right
				AddItem( 430, 75+i*42, 6022 );

			for( int i = 0; i < 9; i++ ) //Outer Top
				AddItem( 85+i*42, 75, 6025 );

			for( int i = 0; i < 9; i++ ) //Outer Bottom
				AddItem( 75+i*42, 310, 6023 );

			for( int i = 0; i < 5; i++ ) //Inner Left
				AddItem( 100, 100+i*42, 6022 );

			for( int i = 0; i < 5; i++ ) //Inner Right
				AddItem( 405, 100+i*42, 6024 );

			for( int i = 0; i < 8; i++ ) //Inner Top
				AddItem( 100+i*42, 85, 6023 );

			for( int i = 0; i < 8; i++ ) //Inner Bottom
				AddItem( 100+i*42, 299, 6025 );

			AddItem( 243, 130, 3307 );
			AddItem( 223, 60, 3310 );
		}

		public override void AddIndiDeco( bool leftside )
		{
		}

		public override void AddButtonDeco( int offset )
		{
			AddItem( (offset > 7 ? 255 : 95), (130 + (offset > 7 ? (offset - 8) * 20 : offset * 20)), 6020 );
		}

		public override void AddPrevPageDeco()
		{
			AddItem( 128, 112, 6021 );
		}

		public override void AddNextPageDeco()
		{
			AddItem( 380, 108, 6020 );
		}
*/
	}
}
