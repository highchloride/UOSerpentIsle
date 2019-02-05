using System;
using Server;
using Server.Mobiles;
using System.Linq;

namespace Server.Engines.Yahtzee
{
    [PropertyObject]
	public class PlayerEntry : IComparable<PlayerEntry>
	{
		public readonly int UpperBonusReq = 63;
		public readonly int UpperBonus = 35;
		
        [CommandProperty(AccessLevel.GameMaster)]
		public PlayerMobile Player { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
		public YahtzeeGame Game { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
		public int Aces { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
		public int Twos { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
		public int Threes { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
		public int Fours { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
		public int Fives { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
		public int Sixes { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
		public int ThreeOfAKind { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
		public int FourOfAKind { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
		public int FullHouse { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
		public int SmallStraight { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
		public int LargeStraight { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
		public int Yahtzee { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
		public int Chance { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
		public int YahtzeeBonus { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Score { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime Completed { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public YahtzeeScoreCard ScoreCard { get; set; }

        public int FemaleHappy { get { return Utility.RandomList(0x30A, 0x30B, 0x30C, 0x30F, 0x31A, 0x321, 0x337); } }
        public int MaleHappy { get { return Utility.RandomList(0x419, 0x41A, 0x41B, 0x41E, 0x42A, 0x42D, 0x431); } }

		public PlayerEntry(PlayerMobile pm, YahtzeeGame game)
		{
			Player = pm;
			Game = game;
		
			Aces = -1;
			Twos = -1;
			Threes = -1;
			Fours = -1;
			Fives = -1;
			Sixes = -1;
			
			ThreeOfAKind = -1;
			FourOfAKind = -1;
			FullHouse = -1;
			SmallStraight = -1;
			LargeStraight = -1;
			Yahtzee = -1;
			Chance = -1;
		}

        public int CompareTo(PlayerEntry entry)
        {
            if (entry == null)
                return -1;

            return Score > entry.Score ? -1 : Score == entry.Score ? 0 : 1;
        }

        public override string ToString()
        {
            return "...";
        }

        public int CalculateScore(ScoreType type, Roll yRoll, bool joker = false)
        {
            if (!joker && !ValidateTypeFromRoll(yRoll, type))
                return 0;

            int total = 0;
            int[] roll = yRoll.ToArray();

            switch (type)
            {
                default:
                case ScoreType.Aces:
                    ColUtility.ForEach(roll.Where(i => i == 1 || joker), i => total += i);
                    break;
                case ScoreType.Twos:
                    ColUtility.ForEach(roll.Where(i => i == 2 || joker), i => total += i);
                    break;
                case ScoreType.Threes:
                    ColUtility.ForEach(roll.Where(i => i == 3 || joker), i => total += i);
                    break;
                case ScoreType.Fours:
                    ColUtility.ForEach(roll.Where(i => i == 4 || joker), i => total += i);
                    break;
                case ScoreType.Fives:
                    ColUtility.ForEach(roll.Where(i => i == 5 || joker), i => total += i);
                    break;
                case ScoreType.Sixes:
                    ColUtility.ForEach(roll.Where(i => i == 6 || joker), i => total += i);
                    break;
                case ScoreType.ThreeOfAKind:
                    ColUtility.ForEach(roll, i => total += i);
                    break;
                case ScoreType.FourOfAKind:
                    ColUtility.ForEach(roll, i => total += i);
                    break;
                case ScoreType.FullHouse:
                    total = 25;
                    break;
                case ScoreType.SmallStraight:
                    total = 30;
                    break;
                case ScoreType.LargeStraight:
                    total = 40;
                    break;
                case ScoreType.Yahtzee:
                    if (HasScored(ScoreType.Yahtzee) && Yahtzee > 0)
                        total = 100;
                    else
                        total = 50;
                    break;
                case ScoreType.Chance:
                    ColUtility.ForEach(roll, i => total += i);
                    break;
            }

            return total;
        }

        public void ApplyScore(ScoreType type, int total)
        {
            if (Game == null)
                return;

            bool yahtzeebonus = false;

            switch (type)
            {
                default:
                case ScoreType.Aces:
                    Aces = total;
                    break;
                case ScoreType.Twos:
                    Twos = total;
                    break;
                case ScoreType.Threes:
                    Threes = total;
                    break;
                case ScoreType.Fours:
                    Fours = total;
                    break;
                case ScoreType.Fives:
                    Fives = total;
                    break;
                case ScoreType.Sixes:
                    Sixes = total;
                    break;
                case ScoreType.ThreeOfAKind:
                    ThreeOfAKind = total;
                    break;
                case ScoreType.FourOfAKind:
                    FourOfAKind = total;
                    break;
                case ScoreType.FullHouse:
                    if(total > 0) Player.PlaySound(Player.Female ? FemaleHappy : MaleHappy);
                    FullHouse = total;
                    break;
                case ScoreType.SmallStraight:
                    if (total > 0) Player.PlaySound(Player.Female ? FemaleHappy : MaleHappy);
                    SmallStraight = total;
                    break;
                case ScoreType.LargeStraight:
                    if (total > 0) Player.PlaySound(Player.Female ? FemaleHappy : MaleHappy);
                    LargeStraight = total;
                    break;
                case ScoreType.Yahtzee:
                    if (total > 0) Player.PlaySound(Player.Female ? FemaleHappy : MaleHappy);
                    if (HasScored(ScoreType.Yahtzee) && Yahtzee > 0)
                    {
                        yahtzeebonus = true;
                        YahtzeeBonus++;
                    }
                    else
                        Yahtzee = total;
                    break;
                case ScoreType.Chance:
                    Chance = total;
                    break;
            }

            Game.Players.ForEach(e =>
            {
                if (e.Player != null)
                {
                    if (e.Player != this.Player)
                        YahtzeeGame.SendMessage(e.Player, String.Format("{0} takes a score of {1} for {2}.", this.Player.Name, total.ToString(), GetScoreForType(type)));
                    else
                        YahtzeeGame.SendMessage(Player, String.Format("You take a score of {0} for {1}.", total.ToString(), GetScoreForType(type)));
                }
            });

            if (!yahtzeebonus)
                Game.CompleteTurn();
            else
            {
                YahtzeeGame.SendMessage(Player, "Apply your joker to any unused box!");
                Game.UsingJoker = true;

                Game.SendGumps();
            }
        }
		
		public int GetScore(bool final = false)
		{
			int bonus = final ? GetBonus() + GetYahtzeeBonus() : 0;
			
			return bonus + GetUpperScore() + GetLowerScore();
		}
		
		public int GetUpperScore()
		{
			return Actual(Aces) + Actual(Twos) + Actual(Threes) + Actual(Fours) + Actual(Fives) + Actual(Sixes);
		}
		
		public int GetLowerScore()
		{
            return Actual(ThreeOfAKind) + Actual(FourOfAKind) + Actual(FullHouse) + Actual(SmallStraight) + Actual(LargeStraight) + Actual(Yahtzee) + Actual(Chance);
		}
		
		public int GetBonus()
		{
			if(GetUpperScore() >= UpperBonusReq)
				return UpperBonus;
				
			return 0;
		}
		
		public int GetYahtzeeBonus()
		{
			return YahtzeeBonus * 100;
		}
		
		public int Actual(int value)
		{
			return Math.Max(0, value);
		}
		
		public bool HasScored(ScoreType type)
		{
			switch(type)
			{
				default:
				case ScoreType.Aces: return Aces > -1;
				case ScoreType.Twos: return Twos > -1;
				case ScoreType.Threes: return Threes > -1;
				case ScoreType.Fours: return Fours > -1;
				case ScoreType.Fives: return Fives > -1;
				case ScoreType.Sixes: return Sixes > -1;
                case ScoreType.ThreeOfAKind: return ThreeOfAKind > -1;
                case ScoreType.FourOfAKind: return FourOfAKind > -1;
                case ScoreType.FullHouse: return FullHouse > -1;
                case ScoreType.SmallStraight: return SmallStraight > -1;
                case ScoreType.LargeStraight: return LargeStraight > -1;
                case ScoreType.Yahtzee: return Yahtzee > -1;
                case ScoreType.Chance: return Chance > -1;
			}
		}
		
		public static bool ValidateTypeFromRoll(Roll gameroll, ScoreType type)
		{
            if (gameroll == null)
                return false;

            int[] roll = gameroll.ToArray();

			switch(type)
			{
				default:
				case ScoreType.Aces: return roll.Where(i => i == 1).Count() > 0;
				case ScoreType.Twos: return roll.Where(i => i == 2).Count() > 0;
				case ScoreType.Threes: return roll.Where(i => i == 3).Count() > 0;
				case ScoreType.Fours: return roll.Where(i => i == 4).Count() > 0;
				case ScoreType.Fives: return roll.Where(i => i == 5).Count() > 0;
				case ScoreType.Sixes: return roll.Where(i => i == 6).Count() > 0;
				case ScoreType.ThreeOfAKind:
                    Array.Sort(roll);
					return  (roll[0] == roll[1] && roll[0] == roll[2]) ||
							(roll[1] == roll[2] && roll[1] == roll[3]) ||
							(roll[2] == roll[3] && roll[2] == roll[4]);
				case ScoreType.FourOfAKind:
                    Array.Sort(roll);
					return  (roll[0] == roll[1] && roll[0] == roll[2] && roll[0] == roll[3]) ||
							(roll[1] == roll[2] && roll[1] == roll[3] && roll[1] == roll[4]);
				case ScoreType.FullHouse:
                    Array.Sort(roll);
                    return (roll[0] == roll[1] && roll[2] == roll[3] && roll[2] == roll[4]) ||
                            (roll[0] == roll[1] && roll[0] == roll[2] && roll[3] == roll[4]);
				case ScoreType.SmallStraight:
                    return (gameroll.HasRolled(1) && gameroll.HasRolled(2) && gameroll.HasRolled(3) && gameroll.HasRolled(4)) ||
                            (gameroll.HasRolled(2) && gameroll.HasRolled(3) && gameroll.HasRolled(4) && gameroll.HasRolled(5)) ||
                            (gameroll.HasRolled(3) && gameroll.HasRolled(4) && gameroll.HasRolled(5) && gameroll.HasRolled(6));
				case ScoreType.LargeStraight:
                    return (gameroll.HasRolled(1) && gameroll.HasRolled(2) && gameroll.HasRolled(3) && gameroll.HasRolled(4) && gameroll.HasRolled(5)) ||
                            (gameroll.HasRolled(2) && gameroll.HasRolled(3) && gameroll.HasRolled(4) && gameroll.HasRolled(5) && gameroll.HasRolled(6));
				case ScoreType.Yahtzee: return roll[0] == roll[1] && roll[0] == roll[2] && roll[0] == roll[3] && roll[0] == roll[4];
				case ScoreType.Chance: return true;
			}
		}
		
		public static string GetScoreForType(ScoreType type)
		{
			switch(type)
			{
				default:
				case ScoreType.Aces:
				case ScoreType.Twos:
				case ScoreType.Threes:
				case ScoreType.Fours:
				case ScoreType.Fives:
				case ScoreType.Sixes: return type.ToString();
				case ScoreType.ThreeOfAKind: return "Three of a Kind";
				case ScoreType.FourOfAKind: return "Four of a Kind";
				case ScoreType.FullHouse: return "Full House";
				case ScoreType.SmallStraight: return "Small Straight";
				case ScoreType.LargeStraight: return "Large Straight";
				case ScoreType.Yahtzee: return "Yahtzee";
				case ScoreType.Chance: return "Chance";
			}
		}

        public void Serialize(GenericWriter writer)
        {
            writer.Write(0);

            writer.Write(Player);

            writer.Write(Aces);
            writer.Write(Twos);
            writer.Write(Threes);
            writer.Write(Fours);
            writer.Write(Fives);
            writer.Write(Sixes);
            writer.Write(ThreeOfAKind);
            writer.Write(FourOfAKind);
            writer.Write(FullHouse);
            writer.Write(SmallStraight);
            writer.Write(LargeStraight);
            writer.Write(Yahtzee);
            writer.Write(Chance);

            writer.Write(YahtzeeBonus);
            writer.Write(Score);
            writer.Write(Completed);
            writer.Write(ScoreCard);
        }

        public PlayerEntry(GenericReader reader, YahtzeeGame game)
        {
            int version = reader.ReadInt();

            Game = game;
            Player = reader.ReadMobile() as PlayerMobile;

            Aces = reader.ReadInt();
            Twos = reader.ReadInt();
            Threes = reader.ReadInt();
            Fours= reader.ReadInt();
            Fives = reader.ReadInt();
            Sixes = reader.ReadInt();
            ThreeOfAKind = reader.ReadInt();
            FourOfAKind = reader.ReadInt();
            FullHouse = reader.ReadInt();
            SmallStraight = reader.ReadInt();
            LargeStraight = reader.ReadInt();
            Yahtzee = reader.ReadInt();
            Chance = reader.ReadInt();

            YahtzeeBonus = reader.ReadInt();
            Score = reader.ReadInt();
            Completed = reader.ReadDateTime();
            ScoreCard = reader.ReadItem() as YahtzeeScoreCard;

            if (game != null)
            {
                ScoreCard.Entry = this;
                ScoreCard.Game = game;
            }
        }
	}
}