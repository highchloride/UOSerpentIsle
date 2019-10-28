using System;
using Server.Gumps;

namespace Server.Custom
{
    public enum WordButtons
    {
        AccountArcOff = 1023,
        AccountArcPressed = 1024,
        AccountArcNormal = 1025,
        AccountShieldOff = 5522,
        AccountShieldNormal = 5523,
        AccountShieldPressed = 5524,
        AddOff = 2460,
        AddPressed = 2461,
        AddNormal = 2462,
        ApplyYellowNormal = 238,
        ApplyYellowOff = 239,
        ApplyYellowPressed = 240,
        ApplyOrangeOff = 2122,
        ApplyOrangePressed = 2123,
        ApplyOrangeNormal = 2124,
        ApplyTiltOff = 2421,
        ApplyTiltPressed = 2422,
        ApplyTiltNormal = 2423,
        ApplyStoneNormal = 5204,
        ApplyStonePressed = 5205,
        AutoOff = 2111,
        AutoPressed = 2112,
        AutoNormal = 2113,
        Backscroll1 = 16,
        Backscroll2 = 45,
        Britannia = 1307,
        Cancel1 = 241,
        Cancel2 = 1144,
        Cancel3 = 1218,
        Cancel4 = 2071,
        Cancel5 = 2119,
        Cancel6 = 2418,
        Cancel7 = 2453,
        Cancel8 = 5200,
        Character = 2030,
        CharacterDelete = 1101,
        CharacterName = 1801,
        CharacterSelect = 1048,
        Chat = 2018,
        City = 1310,
        Connection = 2369,
        Continue1 = 1215,
        Continue2 = 1232,
        Continue3 = 1268,
        Continue4 = 5033,
        Continue5 = 5042,
        CreateCharacter = 1107,
        Credits1 = 1285,
        Credits2 = 5507,
        Credits3 = 5543,
        Default1 = 244,
        Default2 = 2125,
        Default3 = 2424,
        Default4 = 5206,
        Delete1 = 2463,
        Delete2 = 5530,
        Done = 1304,
        EnterBritannia = 1011,
        FacialHair = 1277,
        Female1 = 1264,
        Female2 = 1805,
        Filters = 2412,
        Friends = 2415,
        Gender = 1271,
        General = 2406,
        Guild = 22450,
        HairStyle = 1274,
        Help1 = 2031,
        Help2 = 5525,
        Information = 1020,
        Intro = 1014,
        Journal1 = 2012,
        Journal2 = 2096,
        LogOut = 2009,
        Macros = 2409,
        Mail = 5516,
        MainMenu1 = 1045,
        MainMenu2 = 1110,
        Male1 = 1263,
        Male2 = 1808,
        Manual = 2114,
        Movie = 5510,
        MyUO = 5519,
        New = 5533,
        NEWGRP = 5420,
        Next = 2469,
        Okay1 = 247,
        Okay2 = 1147,
        Okay3 = 2074,
        Okay4 = 2128,
        Okay5 = 2141,
        Okay6 = 2311,
        Okay7 = 2427,
        Okay8 = 2450,
        Options1 = 2006,
        Options2 = 5001,
        PasswordChange = 1104,
        Peace1 = 23,
        Peace2 = 2021,
        PercentFull = 2366,
        PlayCharacter = 1113,
        Previous1 = 1235,
        Previous2 = 1265,
        Previous3 = 1301,
        Previous4 = 2322,
        Previous5 = 2466,
        Previous6 = 5035,
        Profile = 2516,
        Quit1 = 1026,
        Quit2 = 1282,
        Quit3 = 5513,
        Remove = 5409,
        Reply = 5405,
        Save = 5202,
        ScrollLock = 43,
        Send = 5407,
        Setup = 1017,
        Skill_1 = 1238,
        Skill_2 = 1241,
        Skill_3 = 1244,
        Skills1 = 18,
        Skills2 = 2015,
        Skills3 = 2105,
        Status1 = 1,
        Status2 = 2027,
        Stragegy1 = 5,
        Stragegy2 = 2131,
        Template = 1212,
        TimeZone = 2363,
        Tips = 2507,
        Tutorial = 5501,
        Update = 2515,
        War1 = 24,
        War2 = 2024
    }

    public enum MapImages
    {
        Felucca = 5593,
        Trammel = 5594,
        Ilshenar = 5595,
        Malas = 5596,
        Tokuno = 5597,
        TerMur = 5598,
        MapFrame = 5599
    }

    public class CG : Gump
	{
		public const int LabelHue = 0x480;
        public const int GreenHue = 0x40;
        public const int RedHue = 0x20;

		public CG(int x, int y) : base(x, y)
		{
		}

		public void AddOKButton( int x, int y, int buttonID, string text )
		{
			AddButton( x, y - 1, 4023, 4025, buttonID, GumpButtonType.Reply, 0 );
			AddHtml( x + 35, y, 240, 20, Color( text, GreenHue ), false, false );
		}

		public void AddPageFwdButton( int x, int y, int buttonID, string text )
		{
			AddButton( x, y - 1, 5541, 5542, 0, GumpButtonType.Page, buttonID );
			AddHtml( x - 145, y, 240, 20, Color( text, LabelHue ), false, false );
		}

		public void AddHomePageButton( int x, int y )
		{
			AddButton( x, y - 1, 4005, 4007, 0, GumpButtonType.Page, 1 );
			AddLabel( x + 35, y, LabelHue, "Start Over.");
		}

		public void AddPageBkwdButton( int x, int y, int buttonID, string text )
		{
			AddButton( x, y - 1, 5538, 5539, 0, GumpButtonType.Page, buttonID );
			AddHtml( x + 35, y, 240, 20, Color( text, LabelHue ), false, false );
		}

        public void AddHtml(int x, int y, int width, int height, object intOrString, int color, bool background,
            bool scrollbar)
        {
            if (intOrString is int)
                AddHtmlLocalized(x, y, width, height, (int) intOrString, color, background, scrollbar);
            else if (intOrString is string)
                AddHtml(x, y, width, height, Color((string) intOrString, color), background, scrollbar);
        }

        public void AddHtml(int x, int y, int width, int height, object intOrString, string args, int color,
            bool background, bool scrollbar)
        {
            if (intOrString is int)
                AddHtmlLocalized(x, y, width, height, (int) intOrString, args, color, background, scrollbar);
            else if (intOrString is string)
                AddHtml(x, y, width, height, String.Format(Color((String) intOrString, color), args), background,
                    scrollbar);
        }

        public string Center( string text )
		{
			return String.Format( "<CENTER>{0}</CENTER>", text );
		}

		public string Color( string text, int color )
		{
			return String.Format( "<BASEFONT COLOR=#{0:X6}>{1}</BASEFONT>", color, text );
		}

		public string Bold( string text )
		{
			return String.Format( "<BOLD>{0}</BOLD>", text );
		}

		public string Italic( string text )
		{
			return String.Format( "<ITALIC>{0}</ITALIC>", text );
		}
	}
}
