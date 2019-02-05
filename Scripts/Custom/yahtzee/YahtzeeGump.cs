using System;
using Server;
using Server.Gumps;
using Server.Mobiles;

namespace Server.Engines.Yahtzee
{
	public class YahtzeeGump : BaseGump
	{
		public YahtzeeGame Game { get; private set; }
		public PlayerEntry Entry { get; private set; }

        public bool Complete { get { return Game == null || Game.Round >= YahtzeeGame.Rounds; } }
		public bool ViewingOwn { get { return Entry != null && User == Entry.Player; } }
        public bool IsRolling 
        { 
            get
            {
                return ViewingOwn
                    && Game != null
                    && Game.TurnIndex >= 0
                    && Game.TurnIndex < Game.Players.Count
                    && Game.Players[Game.TurnIndex] == Entry
                    && !Complete;
            } 
        }
		
		public YahtzeeGump(PlayerEntry entry, PlayerMobile pm, YahtzeeGame game) : base(pm, 200, 120)
		{
			Game = game;
			Entry = entry;
		}
		
		public override void AddGumpLayout()
		{
            User.CloseGump(typeof(YahtzeeConfirmGump));

            Roll currentRoll = null;
            int roll = 0;

            if (Game != null)
            {
                roll = Game.RollIndex;
                currentRoll = Game.CurrentRoll;

                if (currentRoll == null)
                    Game.CurrentRoll = currentRoll = Roll.Zero;
            }

			if(Entry == null)
				return;

            int width = 500;

            if(Game != null)
			    AddBackground(30, 0, 440, 95, 3500);        // Dice Display

			AddBackground(0, 100, width, 735, 3500); 	    // Score card
            AddBackground(150, 80, 200, 40, 3000);

			//Dice Display
			if(IsRolling && roll < 3 && roll >= 0 && Game != null && !Game.UsingJoker)
			{
                AddButton(50, 17, 2328, 2328, 100, GumpButtonType.Reply, 0);
                AddButton(130, 17, 2328, 2328, 101, GumpButtonType.Reply, 0);
                AddButton(210, 17, 2328, 2328, 102, GumpButtonType.Reply, 0);
                AddButton(290, 17, 2328, 2328, 103, GumpButtonType.Reply, 0);
                AddButton(370, 17, 2328, 2328, 104, GumpButtonType.Reply, 0);

                if(currentRoll.One.Set)
                    AddImage(50, 17, 2328, 0x21);

                if (currentRoll.Two.Set)
                    AddImage(130, 17, 2328, 0x21);

                if (currentRoll.Three.Set)
                    AddImage(210, 17, 2328, 0x21);

                if (currentRoll.Four.Set)
                    AddImage(290, 17, 2328, 0x21);

                if (currentRoll.Five.Set)
                    AddImage(370, 17, 2328, 0x21);

				AddButton(156, 89, 4005, 4006, 1, GumpButtonType.Reply, 0); // ROLL
                AddLabel(190, 89, 0, "ROLL DICE");
                AddLabel(270, 89, 0, String.Format("Roll: {0}/{1}", (roll + 1).ToString(), "3"));
			}
			else
			{
                if (currentRoll != null)
                {
                    AddImage(50, 17, 2328, currentRoll.One.Set ? 0x21 : 0);
                    AddImage(130, 17, 2328, currentRoll.Two.Set ? 0x21 : 0);
                    AddImage(210, 17, 2328, currentRoll.Three.Set ? 0x21 : 0);
                    AddImage(290, 17, 2328, currentRoll.Four.Set ? 0x21 : 0);
                    AddImage(370, 17, 2328, currentRoll.Five.Set ? 0x21 : 0);
                }

                if (IsRolling)
                {
                    if (Game.UsingJoker)
                        AddHtml(156, 80, 187, 40, Center("Choose an unchosen box for your joker."), false, false);
                    else
                        AddLabel(156, 89, 0, "No more rolls for this turn.");
                }
                else if (Complete)
                {
                    if (Game != null)
                    {
                        PlayerEntry entry = Game.GetWinner();

                        if (entry != null)
                            AddHtml(156, 80, 187, 40, Center(String.Format("{0} has scored {1} for the win!", entry.Player.Name, entry.Score)), false, false);
                        else
                            AddHtml(156, 80, 187, 40, Center("This round is over!"), false, false);
                    }
                    else
                    {
                        AddHtml(156, 89, 187, 40, Center(String.Format("Game Played: {0}", Entry.Completed.ToShortDateString())), false, false);
                    }
                }
                else
                {
                    if(roll < 3 && roll >= 0)
                        AddLabel(270, 89, 0, String.Format("Roll: {0}/{1}", (roll + 1).ToString(), "3"));
                    else
                        AddLabel(156, 89, 0, "No more rolls for this turn.");
                }
			}

            if (currentRoll != null)
            {
                AddImage(77, 36, GetDiceID(currentRoll.One.Roll));
                AddImage(157, 36, GetDiceID(currentRoll.Two.Roll));
                AddImage(237, 36, GetDiceID(currentRoll.Three.Roll));
                AddImage(317, 36, GetDiceID(currentRoll.Four.Roll));
                AddImage(397, 36, GetDiceID(currentRoll.Five.Roll));
            }

            AddImageTiled(200, 142, 2, 300, 96);
            AddImageTiled(300, 142, 2, 300, 96);
            AddImageTiled(400, 142, 2, 300, 96);

            AddImageTiled(200, 472, 2, 360, 96);
            AddImageTiled(300, 472, 2, 360, 96);
            AddImageTiled(400, 472, 2, 360, 96);

            AddImageTiled(300, 680, 2, 30, 96);
            AddImageTiled(333, 680, 2, 30, 96);
            AddImageTiled(366, 680, 2, 30, 96);

            AddImageTiled(10, 140, 480, 2, 96);
            AddImageTiled(10, 170, 480, 2, 96);
            AddImageTiled(10, 200, 480, 2, 96);
            AddImageTiled(10, 230, 480, 2, 96);
            AddImageTiled(10, 260, 480, 2, 96);
            AddImageTiled(10, 290, 480, 2, 96);
            AddImageTiled(10, 320, 480, 2, 96);
            AddImageTiled(10, 350, 480, 2, 96);

            AddImageTiled(10, 380, 480, 2, 96);
            AddImageTiled(10, 410, 480, 2, 96);
            AddImageTiled(10, 440, 480, 2, 96);
            AddImageTiled(10, 470, 480, 2, 96);
            AddImageTiled(10, 500, 480, 2, 96);
            AddImageTiled(10, 530, 480, 2, 96);
            AddImageTiled(10, 560, 480, 2, 96);
            AddImageTiled(10, 590, 480, 2, 96);
            AddImageTiled(10, 620, 480, 2, 96);
            AddImageTiled(10, 650, 480, 2, 96);
            AddImageTiled(10, 680, 480, 2, 96);

            AddImageTiled(200, 710, 291, 2, 96);
            AddImageTiled(10, 740, 480, 2, 96);

            AddImageTiled(10, 770, 480, 2, 96);
            AddImageTiled(10, 800, 480, 2, 96);
            AddImageTiled(10, 830, 480, 2, 96);

			bool canChoose = Game != null && Game.RollIndex > 0 && IsRolling;
			
			//Score Display
			AddHtml(20, 115, 250, 16, "<basefont size=15><b>YAHTZEE</b>", false, false);

            if(ViewingOwn)
                AddHtml(0, 120, width, 16, Center("You are viewing your score card."), false, false);
            else
                AddHtml(0, 120, width, 16, Center(String.Format("You are viewing {0} score card.", Entry.Player != null ? Entry.Player.Name + "'s" : "an unknown persons")), false, false);
			
			AddHtml(0, 146, 200, 32, Center("<basefont size=8>UPPER SECTION"), false, false);
			AddHtml(203, 146, 100, 32, Center("HOW TO SCORE"), false, false);
            AddHtml(300, 146, 100, 16, Center("YOUR SCORE"), false, false);

            int yOffset = 0;
            int good = 11400;
            int bad = 11410;

            if(canChoose)
                AddHtml(400, 146 + yOffset, 100, 16, Center("Select"), false, false);

            AddLabel(15, 176 + yOffset, 0, "Aces");
            AddImage(120, 174 + yOffset, 1450);
            AddLabel(170, 176 + yOffset, 0, "= 1");

            AddHtml(205, 170 + yOffset, 90, 32, Center("Count and Add"), false, false);
            AddHtml(205, 185 + yOffset, 90, 32, Center("Only Aces"), false, false);
            AddLabel(305, 176 + yOffset, 0, Entry.Aces >= 0 ? Entry.Actual(Entry.Aces).ToString() : "");

            if (canChoose && !Entry.HasScored(ScoreType.Aces))
            {
                AddButton(405, 175 + yOffset, 4014, 4015, (int)ScoreType.Aces + 200, GumpButtonType.Reply, 0);
                AddImage(455, 180 + yOffset, PlayerEntry.ValidateTypeFromRoll(currentRoll, ScoreType.Aces) ? good : bad);
            }

            yOffset += 30;

            AddLabel(15, 176 + yOffset, 0, "Twos");
            AddImage(120, 174 + yOffset, 1451);
            AddLabel(170, 176 + yOffset, 0, "= 2");

            AddHtml(205, 170 + yOffset, 100, 32, Center("Count and Add"), false, false);
            AddHtml(205, 185 + yOffset, 100, 32, Center("Only Twos"), false, false);
            AddLabel(305, 176 + yOffset, 0, Entry.Twos >= 0 ? Entry.Actual(Entry.Twos).ToString() : "");

            if (canChoose && !Entry.HasScored(ScoreType.Twos))
            {
                AddButton(405, 175 + yOffset, 4014, 4015, (int)ScoreType.Twos + 200, GumpButtonType.Reply, 0);
                AddImage(455, 180 + yOffset, PlayerEntry.ValidateTypeFromRoll(currentRoll, ScoreType.Twos) ? good : bad);
            }

            yOffset += 30;

            AddLabel(15, 176 + yOffset, 0, "Threes");
            AddImage(120, 174 + yOffset, 1452);
            AddLabel(170, 176 + yOffset, 0, "= 3");

            AddHtml(205, 170 + yOffset, 100, 32, Center("Count and Add"), false, false);
            AddHtml(205, 185 + yOffset, 100, 32, Center("Only Threes"), false, false);
            AddLabel(305, 176 + yOffset, 0, Entry.Threes >= 0 ? Entry.Actual(Entry.Threes).ToString() : "");

            if (canChoose && !Entry.HasScored(ScoreType.Threes))
            {
                AddButton(405, 175 + yOffset, 4014, 4015, (int)ScoreType.Threes + 200, GumpButtonType.Reply, 0);
                AddImage(455, 180 + yOffset, PlayerEntry.ValidateTypeFromRoll(currentRoll, ScoreType.Threes) ? good : bad);
            }

            yOffset += 30;

            AddLabel(15, 176 + yOffset, 0, "Fours");
            AddImage(120, 174 + yOffset, 1453);
            AddLabel(170, 176 + yOffset, 0, "= 4");

            AddHtml(205, 170 + yOffset, 100, 32, Center("Count and Add"), false, false);
            AddHtml(205, 185 + yOffset, 100, 32, Center("Only Fours"), false, false);
            AddLabel(305, 176 + yOffset, 0, Entry.Fours >= 0 ? Entry.Actual(Entry.Fours).ToString() : "");

            if (canChoose && !Entry.HasScored(ScoreType.Fours))
            {
                AddButton(405, 175 + yOffset, 4014, 4015, (int)ScoreType.Fours + 200, GumpButtonType.Reply, 0);
                AddImage(455, 180 + yOffset, PlayerEntry.ValidateTypeFromRoll(currentRoll, ScoreType.Fours) ? good : bad);
            }

            yOffset += 30;

            AddLabel(15, 176 + yOffset, 0, "Fives");
            AddImage(120, 174 + yOffset, 1454);
            AddLabel(170, 176 + yOffset, 0, "= 5");

            AddHtml(205, 170 + yOffset, 100, 32, Center("Count and Add"), false, false);
            AddHtml(205, 185 + yOffset, 100, 32, Center("Only Fives"), false, false);
            AddLabel(305, 176 + yOffset, 0, Entry.Fives >= 0 ? Entry.Actual(Entry.Fives).ToString() : "");

            if (canChoose && !Entry.HasScored(ScoreType.Fives))
            {
                AddButton(405, 175 + yOffset, 4014, 4015, (int)ScoreType.Fives + 200, GumpButtonType.Reply, 0);
                AddImage(455, 180 + yOffset, PlayerEntry.ValidateTypeFromRoll(currentRoll, ScoreType.Fives) ? good : bad);
            }

            yOffset += 30;

            AddLabel(15, 176 + yOffset, 0, "Sixes");
            AddImage(120, 174 + yOffset, 1455);
            AddLabel(170, 176 + yOffset, 0, "= 6");

            AddHtml(205, 170 + yOffset, 100, 32, Center("Count and Add"), false, false);
            AddHtml(205, 185 + yOffset, 100, 32, Center("Only Sixes"), false, false);
            AddLabel(305, 176 + yOffset, 0, Entry.Sixes >= 0 ? Entry.Actual(Entry.Sixes).ToString() : "");

            if (canChoose && !Entry.HasScored(ScoreType.Sixes))
            {
                AddButton(405, 175 + yOffset, 4014, 4015, (int)ScoreType.Sixes + 200, GumpButtonType.Reply, 0);
                AddImage(455, 180 + yOffset, PlayerEntry.ValidateTypeFromRoll(currentRoll, ScoreType.Sixes) ? good : bad);
            }

            yOffset += 30; 
			bool final = Game == null || Game.Round >= YahtzeeGame.Rounds;
				
			AddHtml(15, 176 + yOffset, 200, 32, "<basefont size=7>TOTAL SCORE", false, false);
            AddLabel(305, 176 + yOffset, 0, Entry.GetUpperScore().ToString());

            yOffset += 30;
            AddHtml(15, 176 + yOffset, 100, 32, "<basefont size=7>BONUS", false, false);
			AddHtml(100, 170 + yOffset, 100, 32, Center("<basefont size=3>If total score is"), false, false);
            AddHtml(100, 185 + yOffset, 100, 32, Center("<basefont size=3>63 or over"), false, false);
			AddHtml(200, 176 + yOffset, 100, 32, Center("SCORE 35"), false, false);
			if(final) AddLabel(305, 176 + yOffset, 0, Entry.GetBonus().ToString());

            yOffset += 30;
			AddHtml(15, 176 + yOffset, 100, 32, "<basefont size=7>TOTAL", false, false);
			AddHtml(100, 175 + yOffset, 100, 32, Center("<basefont size=3>Of upper section"), false, false);
            AddLabel(305, 176 + yOffset, 0, (final ? Entry.GetUpperScore() + Entry.GetBonus() : Entry.GetUpperScore()).ToString());

            yOffset += 30;
			AddHtml(15, 176 + yOffset, 200, 32, "<basefont size=8>LOWER SECTION", false, false);
            yOffset += 30;

			AddLabel(15, 176 + yOffset, 0, "Three of a kind");
			AddHtml(202, 170 + yOffset, 100, 32, Center("Add total of all"), false, false);
            AddHtml(202, 185 + yOffset, 100, 32, Center("dice"), false, false);
			AddLabel(305, 176 + yOffset, 0, Entry.ThreeOfAKind >= 0 ? Entry.Actual(Entry.ThreeOfAKind).ToString() : "");

            if (canChoose && !Entry.HasScored(ScoreType.ThreeOfAKind))
            {
				AddButton(405, 175 + yOffset, 4014, 4015, (int)ScoreType.ThreeOfAKind + 200, GumpButtonType.Reply, 0);
                AddImage(455, 180 + yOffset, PlayerEntry.ValidateTypeFromRoll(currentRoll, ScoreType.ThreeOfAKind) ? good : bad);
            }

            yOffset += 30;
            AddLabel(15, 176 + yOffset, 0, "Four of a kind");
            AddHtml(202, 170 + yOffset, 100, 32, Center("Add total of all"), false, false);
            AddHtml(202, 185 + yOffset, 100, 32, Center("dice"), false, false);
            AddLabel(305, 176 + yOffset, 0, Entry.FourOfAKind >= 0 ? Entry.Actual(Entry.FourOfAKind).ToString() : "");

            if (canChoose && !Entry.HasScored(ScoreType.FourOfAKind))
            {
                AddButton(405, 175 + yOffset, 4014, 4015, (int)ScoreType.FourOfAKind + 200, GumpButtonType.Reply, 0);
                AddImage(455, 180 + yOffset, PlayerEntry.ValidateTypeFromRoll(currentRoll, ScoreType.FourOfAKind) ? good : bad);
            }

            yOffset += 30;
            AddLabel(15, 176 + yOffset, 0, "Full House");
            AddHtml(200, 176 + yOffset, 100, 32, Center("SCORE 25"), false, false);
            AddLabel(305, 176 + yOffset, 0, Entry.FullHouse >= 0 ? Entry.Actual(Entry.FullHouse).ToString() : "");

            if (canChoose && !Entry.HasScored(ScoreType.FullHouse))
            {
                AddButton(405, 175 + yOffset, 4014, 4015, (int)ScoreType.FullHouse + 200, GumpButtonType.Reply, 0);
                AddImage(455, 180 + yOffset, PlayerEntry.ValidateTypeFromRoll(currentRoll, ScoreType.FullHouse) ? good : bad);
            }

            yOffset += 30;
            AddLabel(15, 176 + yOffset, 0, "Sm. Straight");
            AddHtml(200, 176 + yOffset, 100, 32, Center("SCORE 30"), false, false);
            AddLabel(305, 176 + yOffset, 0, Entry.SmallStraight >= 0 ? Entry.Actual(Entry.SmallStraight).ToString() : "");

            if (canChoose && !Entry.HasScored(ScoreType.SmallStraight))
            {
                AddButton(405, 175 + yOffset, 4014, 4015, (int)ScoreType.SmallStraight + 200, GumpButtonType.Reply, 0);
                AddImage(455, 180 + yOffset, PlayerEntry.ValidateTypeFromRoll(currentRoll, ScoreType.SmallStraight) ? good : bad);
            }

            yOffset += 30;
            AddLabel(15, 176 + yOffset, 0, "Lg. Straight");
            AddHtml(200, 176 + yOffset, 100, 32, Center("SCORE 40"), false, false);
            AddLabel(305, 176 + yOffset, 0, Entry.LargeStraight >= 0 ? Entry.Actual(Entry.LargeStraight).ToString() : "");

            if (canChoose && !Entry.HasScored(ScoreType.LargeStraight))
            {
				AddButton(405, 175 + yOffset, 4014, 4015, (int)ScoreType.LargeStraight + 200, GumpButtonType.Reply, 0);
                AddImage(455, 180 + yOffset, PlayerEntry.ValidateTypeFromRoll(currentRoll, ScoreType.LargeStraight) ? good : bad);
            }

            yOffset += 30;
            AddLabel(15, 176 + yOffset, 0, "YAHTZEE");
            AddHtml(200, 176 + yOffset, 100, 32, Center("SCORE 50"), false, false);
            AddLabel(305, 176 + yOffset, 0, Entry.Yahtzee >= 0 ? Entry.Actual(Entry.Yahtzee).ToString() : "");

            if (canChoose && !Game.UsingJoker && Entry.Yahtzee != 0 && (!Entry.HasScored(ScoreType.Yahtzee) || PlayerEntry.ValidateTypeFromRoll(currentRoll, ScoreType.Yahtzee)))
            {
                AddButton(405, 175 + yOffset, 4014, 4015, (int)ScoreType.Yahtzee + 200, GumpButtonType.Reply, 0);
                AddImage(455, 180 + yOffset, PlayerEntry.ValidateTypeFromRoll(currentRoll, ScoreType.Yahtzee) ? good : bad);
            }

            yOffset += 30;
            AddLabel(15, 176 + yOffset, 0, "Chance");
            AddHtml(200, 170 + yOffset, 100, 32, Center("Score total of"), false, false);
            AddHtml(200, 185 + yOffset, 100, 32, Center("all 5 dice"), false, false);
            AddLabel(305, 175 + yOffset, 0, Entry.Chance >= 0 ? Entry.Actual(Entry.Chance).ToString() : "");

            if (canChoose && !Entry.HasScored(ScoreType.Chance))
            {
                AddButton(405, 175 + yOffset, 4014, 4015, (int)ScoreType.Chance + 200, GumpButtonType.Reply, 0);
                AddImage(455, 180 + yOffset, PlayerEntry.ValidateTypeFromRoll(currentRoll, ScoreType.Chance) ? good : bad);
            }

            yOffset += 30;
            AddHtml(5, 185 + yOffset, 200, 64, Center("YAHTZEE"), false, false);
            AddHtml(5, 200 + yOffset, 200, 64, Center("BONUS"), false, false);

            AddHtml(200, 170 + yOffset, 100, 32, Center("<b>X</b> FOR EACH"), false, false);
            AddHtml(200, 185 + yOffset, 100, 32, Center("BONUS"), false, false);

            yOffset += 30;
            AddHtml(200, 170 + yOffset, 100, 32, Center("SCORE 100"), false, false);
            AddHtml(200, 185 + yOffset, 100, 32, Center("PER <b>X</b>"), false, false);
            if (final) AddLabel(305, 176 + yOffset, 0, Entry.GetYahtzeeBonus().ToString());

            if (Entry.YahtzeeBonus > 0) AddHtml(305, 687, 28, 25, Center("<basefont size=8><b>X</B>"), false, false);
            if (Entry.YahtzeeBonus > 1) AddHtml(338, 687, 28, 25, Center("<basefont size=8><b>X</B>"), false, false);
            if (Entry.YahtzeeBonus > 2) AddHtml(371, 687, 28, 25, Center("<basefont size=8><b>X</B>"), false, false);

            yOffset += 30;
            AddHtml(15, 176 + yOffset, 100, 32, "<basefont size=7>TOTAL", false, false);
            AddHtml(105, 170 + yOffset, 100, 32, Center("<basefont size=3>Of Lower"), false, false);
            AddHtml(105, 185 + yOffset, 100, 32, Center("<basefont size=3>Section"), false, false);
            AddLabel(305, 176 + yOffset, 0, (final ? Entry.GetLowerScore() + Entry.GetYahtzeeBonus() : Entry.GetLowerScore()).ToString());

            yOffset += 30;
            AddHtml(15, 176 + yOffset, 100, 32, "<basefont size=7>TOTAL", false, false);
            AddHtml(105, 170 + yOffset, 100, 32, Center("<basefont size=3>Of upper"), false, false);
            AddHtml(105, 185 + yOffset, 100, 32, Center("<basefont size=3>section"), false, false);
            if (final) AddLabel(305, 176 + yOffset, 0, (Entry.GetUpperScore() + Entry.GetBonus()).ToString());

            yOffset += 30;
            AddHtml(15, 176 + yOffset, 200, 32, "<basefont size=10>GRAND TOTAL", false, false);
            if (final) AddLabel(305, 176 + yOffset, 0, Entry.GetScore(final).ToString());

            if (Game != null && !Complete && Game.Players.Count > 1)
            {
                for (int i = 0; i < Game.Players.Count; i++)
                {
                    PlayerEntry entry = Game.Players[i];

                    if (entry != Entry)
                        AddButton(15 + (90 * i), 833, 6, 6, i + 2, GumpButtonType.Reply, 0);

                    AddBackground(15 + (90 * i), 830, 90, 30, 3000);
                    AddHtml(15 + (92 * i), 835, 86, 16, Center(entry.Player == User ? "Mine" : entry.Player.Name), false, false);
                }
            }
		}
		
        public override void OnResponse(RelayInfo info)
		{
            if (Game == null)
                return;

			Mobile from = User;

            int buttonID = info.ButtonID;
			int roll = Game.RollIndex;
			
			if(IsRolling && buttonID == 1 && roll >= 0 && roll < 3)
			{
				Game.RollDice(User);
				return;
			}
			
			if(!Complete && buttonID >= 2 && buttonID <= 6)
			{
				PlayerEntry entry = Game.Players[buttonID - 2];
                Entry = Game.Players[buttonID - 2];
                Refresh();

                User.SendSound(0x55);

				from.SendMessage("You are now viewing {0} score card.", entry.Player == from ? "your" : entry.Player.Name + "'s");
			}
			
			if(IsRolling && buttonID >= 100 && buttonID <= 104 && roll < 3 && !Game.UsingJoker)
			{
                if (roll > 0)
                {
                    switch (buttonID - 100)
                    {
                        case 0: Game.CurrentRoll.One.Set = Game.CurrentRoll.One.Set ? false : true; break;
                        case 1: Game.CurrentRoll.Two.Set = Game.CurrentRoll.Two.Set ? false : true; break;
                        case 2: Game.CurrentRoll.Three.Set = Game.CurrentRoll.Three.Set ? false : true; break;
                        case 3: Game.CurrentRoll.Four.Set = Game.CurrentRoll.Four.Set ? false : true; break;
                        case 4: Game.CurrentRoll.Five.Set = Game.CurrentRoll.Five.Set ? false : true; break;
                    }
                }

				Game.SendGumps();
				return;
			}
			
			if(IsRolling && buttonID >= 200)
			{
				ScoreType type = (ScoreType)buttonID - 200;
				int score = Entry.CalculateScore(type, Game.CurrentRoll, Game.UsingJoker);

                string body;

                if (type == ScoreType.Yahtzee && Entry.HasScored(ScoreType.Yahtzee) && Entry.Yahtzee > 0)
                    body = "If you choose a yahtzee bonus, 100 points per bonus will be added at the end of the game. You will then choose a joker for any unchosen box.";
                else
                {
                    body = String.Format("Apply {0} points to {1}?", score.ToString(), PlayerEntry.GetScoreForType(type));

                    if (roll < 3 && type != ScoreType.Yahtzee)
                        body += String.Format("<basefont color=red> You still have {0} roll(s) left!", 3 - roll);
                }

                BaseGump.SendGump(new YahtzeeConfirmGump(User, "Apply Score", body, () =>
                    {
                        Entry.ApplyScore(type, score);
                    }));
			}
		}
		
		private int GetDiceID(int v)
		{
			return 1450 + (v - 1);
		}
	}

    public class YahtzeeConfirmGump : BaseGump
    {
        public string Title { get; private set; }
        public string Body { get; private set; }
        public Action ConfirmCallback { get; private set; }
        public Action CancelCallback { get; private set; }

        public YahtzeeConfirmGump(PlayerMobile pm, string title, string body, Action confirmcallback, Action cancelcallback = null) : base(pm, 75, 75)
        {
            ConfirmCallback = confirmcallback;
            CancelCallback = cancelcallback;

            Title = title;
            Body = body;
        }

        public override void AddGumpLayout()
        {
            AddBackground(0, 0, 300, 200, 3500); // 2620

            AddHtml(0, 25, 300, 16, Center(Title), false, false);
            AddHtml(12, 60, 278, 100, Color("#111111", Body), false, true);

            AddHtml(45, 168, 100, 16, "Confirm", false, false);
            AddHtml(185, 168, 100, 16, "Cancel", false, false);

            AddButton(10, 168, 4023, 4025, 1, GumpButtonType.Reply, 0);
            AddButton(150, 168, 4017, 4019, 2, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(RelayInfo info)
        {
            switch (info.ButtonID)
            {
                case 1:
                    if (ConfirmCallback != null) 
                        ConfirmCallback();
                    break;
                case 2:
                    if (CancelCallback != null) 
                        CancelCallback();
                    break;
            }
        }
    }
}