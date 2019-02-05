using Server;
using System.Collections.Generic;
using System;
using Server.Mobiles;
using Server.Items;
using System.Linq;
using Server.Gumps;

namespace Server.Engines.Yahtzee
{
	public enum RollOrder
	{
		AsIs,
		Random,
		Roll
	}
	
	public enum ScoreType
	{
		Aces,
		Twos,
		Threes, 
		Fours,
		Fives,
		Sixes,
		ThreeOfAKind,
		FourOfAKind,
		FullHouse,
		SmallStraight,
		LargeStraight,
		Yahtzee,
		Chance
	}

    [PropertyObject]
	public class YahtzeeGame
	{
		public static List<YahtzeeGame> Games { get; set; }
	
		public static readonly int Rounds = 14;
		public static readonly int MaxPlayers = 4;

        [CommandProperty(AccessLevel.GameMaster)]
		public YahtzeeBoard Board { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
		public int TurnIndex { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
		public int RollIndex { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
		public int Round { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
		public Roll CurrentRoll { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
		public RollOrder RollOrder { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool UsingJoker { get; set; }

		public List<PlayerEntry> Players { get; set; }
		
		public static void Configure()
		{
			Games = new List<YahtzeeGame>();
		}
		
		public YahtzeeGame(List<Mobile> list, YahtzeeBoard board, RollOrder order = RollOrder.AsIs)
		{
			Board = board;
            RollOrder = order;

			Players = new List<PlayerEntry>();
			
			list.ForEach(m => Players.Add(new PlayerEntry(m as PlayerMobile, this)));
			
			TurnIndex = 0;
			RollIndex = 0;
			
			Games.Add(this);
		}
		
		public void BeginGame()
		{
			if(Players.Count < 1)
			{
				CancelGame();
				return;
			}

            if (Players.Count > 0)
            {
                switch (RollOrder)
                {
                    case RollOrder.AsIs:
                        break;
                    case RollOrder.Random:
                        if (Players.Count == 2)
                        {
                            if (Utility.RandomDouble() > 0.5)
                            {
                                PlayerEntry entry = Players[1];
                                Players.Remove(entry);
                                Players.Insert(0, entry);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < 25; i++)
                            {
                                int random = Utility.Random(Players.Count);

                                if (random > 0)
                                {
                                    PlayerEntry entry = Players[random];
                                    Players.Remove(entry);
                                    Players.Insert(0, entry);
                                }
                            }
                        }

                        break;
                    case RollOrder.Roll:
                        Dictionary<PlayerMobile, int> table = new Dictionary<PlayerMobile, int>();
                        foreach (PlayerEntry e in Players)
                        {
                            table.Add(e.Player, SumRoll(Roll().ToArray()));
                        }

                        Players.Clear();
                        foreach (KeyValuePair<PlayerMobile, int> kvp in table.OrderBy(k => -k.Value))
                        {
                            YahtzeeGame.SendMessage(kvp.Key, String.Format("You have rolled a {0}!", kvp.Value));
                            Players.Add(new PlayerEntry(kvp.Key, this));
                        }

                        table.Clear();
                        break;
                }
            }
			
			TurnIndex = 0;
			RollIndex = 0;
			Round = 1;
			
			Players.ForEach(e => YahtzeeGame.SendMessage(e.Player, "Yahtzee has begun!"));

            GiveScorecards();

            if (Board != null)
                Board.InvalidateProperties();

			Timer.DelayCall(TimeSpan.FromSeconds(.5), new TimerCallback(SendGumps));
		}

        private void GiveScorecards()
        {
            Players.ForEach(e =>
                {
                    var card = new YahtzeeScoreCard(e, this);

                    e.Player.Backpack.DropItem(card);
                    e.ScoreCard = card;
                    YahtzeeGame.SendMessage(e.Player, "A score card has been added to your backpack.");

                    e.ScoreCard.InvalidateProperties();
                });
        }
		
		public void CompleteTurn()
		{
            if (CurrentRoll != null)
                CurrentRoll.Clear();

            if (UsingJoker)
                UsingJoker = false;

            if (TurnIndex == Players.Count - 1)
            {
                Round++;
                TurnIndex = 0;
            }
            else
                TurnIndex++;

            if (Round >= Rounds)
            {
                EndGame();
                return;
            }

            PlayerEntry next = Players[TurnIndex];

            if (Players.Count > 1)
            {
                Players.ForEach(e =>
                    {
                        if (e.Player == next.Player)
                            YahtzeeGame.SendMessage(e.Player, "It's your turn!", 75);
                        else
                        {
                            if (next.Player.NetState == null)
                                YahtzeeGame.SendMessage(e.Player, String.Format("It is now {0}'s turn, however they are no longer online!", next.Player.Name), 75);
                            else
                                YahtzeeGame.SendMessage(e.Player, String.Format("It is now {0}'s turn!", next.Player.Name), 75);
                        }
                    });
            }

            RollIndex = 0;
            Timer.DelayCall(TimeSpan.FromSeconds(.5), SendGumps);
		}
		
		public void EndGame()
		{
            TallyScore();

            Timer.DelayCall(TimeSpan.FromSeconds(.5), SendGumps);
			
			if(Games.Contains(this))
				Games.Remove(this);

            if (Board != null)
            {
                Board.Game = null;
                Board.InvalidateProperties();
            }

            Players.ForEach(e =>
                {
                    e.Game = null;
                    e.Completed = DateTime.Now;

                    if (e.ScoreCard.Game != null)
                        e.ScoreCard.Game = null;

                    e.ScoreCard.InvalidateProperties();
                    e.ScoreCard = null;

                    if (YahtzeeStats.CheckScore(e))
                    {
                        Players.ForEach(entry =>
                            {
                                if (entry.Player == e.Player)
                                    YahtzeeGame.SendMessage(entry.Player, String.Format("Your score of {0} has been entered into the Yahtzee top 10!", e.Score.ToString()));
                                else
                                    YahtzeeGame.SendMessage(entry.Player, String.Format("{0}'s score of {1} has been entered into the Yahtzee Top 10!", e.Player.Name, e.Score.ToString()));
                            });
                        
                    }
                });

            Timer.DelayCall(TimeSpan.FromSeconds(1), Players.Clear);
		}

        private void TallyScore()
        {
            Players.ForEach(e =>
                {
                    //Upper Score + Bonus if applicable
                    int score = e.GetUpperScore() + e.GetBonus();

                    // Lower Score + any yahtzee bonus
                    score += e.GetLowerScore() + e.GetYahtzeeBonus();

                    e.Score = score;
                });
        }

        public PlayerEntry GetWinner()
        {
            PlayerEntry winner = null;

            Players.ForEach(e =>
                {
                    if (winner == null || e.Score > winner.Score)
                        winner = e;
                });

            return winner;
        }

		public void CancelGame()
		{
			Players.ForEach(entry => 
                { 
                    YahtzeeGame.SendMessage(entry.Player, "Your current yahtzee game has been canceled.", 2118);
                });


            if (Board != null)
            {
                Board.Game = null;
                Board.InvalidateProperties();
            }

            CloseGumps();
			
			Timer.DelayCall(TimeSpan.FromSeconds(1), Players.Clear);
			
			if(Games.Contains(this))
				Games.Remove(this);
		}
		
		public void SendGumps()
		{
			Players.ForEach(entry =>
			{
                BaseGump g = entry.Player.FindGump(typeof(YahtzeeGump)) as BaseGump;
				
				if(g != null)
					g.Refresh();
				else
                    BaseGump.SendGump(new YahtzeeGump(entry, entry.Player, this));
			});
		}
		
		public void CloseGumps()
		{
			Players.ForEach( entry => {
                entry.Player.CloseGump(typeof(YahtzeeGump));
                entry.Player.CloseGump(typeof(YahtzeeConfirmGump));
            });
		}
		
		public void RollDice(PlayerMobile pm)
		{
			if(CurrentRoll == null || RollIndex == 0)
				CurrentRoll = Roll();
			else
				CurrentRoll.Reroll();

            RollIndex++;
            string roll = RollIndex == 1 ? "1st" : RollIndex == 2 ? "2nd" : "final";
            bool isYahtzee = PlayerEntry.ValidateTypeFromRoll(CurrentRoll, ScoreType.Yahtzee);

            Players.ForEach(e =>
                {
                    if (pm == e.Player)
                    {
                        if (isYahtzee)
                            YahtzeeGame.SendMessage(e.Player, "You have rolled a Yahtzee!!!");
                        else
                            YahtzeeGame.SendMessage(e.Player, String.Format("You have made your {0} roll.", roll));
                    }
                    else
                    {
                        if (isYahtzee)
                            YahtzeeGame.SendMessage(e.Player, String.Format("{0} has rolled a Yahtzee!!!", e.Player.Name));
                        else
                            YahtzeeGame.SendMessage(e.Player, String.Format("{0} has made thier {1} roll.", e.Player.Name, roll));
                    }
                });

            if (Board != null)
            {
                if (isYahtzee)
                    Effects.PlaySound(Board.GetWorldLocation(), Board.Map, 0x3D);
                else
                {
                    DoRollEffects(pm);
                }
            }

			Timer.DelayCall(TimeSpan.FromSeconds(.25), SendGumps);
		}
		
		public Roll Roll()
		{
            return new Roll(Utility.RandomMinMax(1, 6), Utility.RandomMinMax(1, 6), Utility.RandomMinMax(1, 6), Utility.RandomMinMax(1, 6), Utility.RandomMinMax(1, 6));
		}
		
		public int SumRoll(int[] roll)
		{
			int v = 0;
			
			foreach(int i in roll)
				v += i;
				
			return v;
		}

        private void DoRollEffects(PlayerMobile pm)
        {
            if(!pm.Mounted)
                pm.Animate(32, 5, 1, true, false, 0);

            Effects.PlaySound(Board.GetWorldLocation(), Board.Map, 0x4F);
            /*for (int i = 0; i < 3; i++)
            {
                Timer.DelayCall(TimeSpan.FromMilliseconds(i * 300), // 0x247, 0x4F
                    () => Effects.PlaySound(Board.GetWorldLocation(), Board.Map, 0x42));
            }*/
        }

        private bool IsPlayer(PlayerMobile pm)
		{
			foreach(PlayerEntry score in Players)
				if(score.Player == pm)
					return true;
			return false;
		}

        public PlayerEntry GetEntry(PlayerMobile pm)
        {
            return Players.FirstOrDefault(e => e.Player == pm);
        }

        public static void SendMessage(Mobile m, string message, int hue = 62)
        {
            m.SendMessage(62, String.Format("Yahtzee: {0}", message));
        }

		public static bool IsInGame(PlayerMobile pm)
		{
			foreach(YahtzeeGame g in Games)
			{
                foreach (PlayerEntry entry in g.Players)
                {
                    if (entry.Player == pm)
                        return true;
                }
			}
			
			return false;
		}

        public override string ToString()
        {
            return "...";
        }

        public YahtzeeGame(GenericReader reader, YahtzeeBoard board)
        {
            Players = new List<PlayerEntry>();

            int version = reader.ReadInt();

            Board = board;

            TurnIndex = reader.ReadInt();
            RollIndex = reader.ReadInt();
            Round = reader.ReadInt();
            UsingJoker = reader.ReadBool();

            CurrentRoll = new Roll(reader);

            int count = reader.ReadInt();
            for (int i = 0; i < count; i++)
            {
                PlayerEntry entry = new PlayerEntry(reader, this);

                if (entry != null && entry.Player != null)
                    Players.Add(entry);
            }

            if (Players.Count <= 0)
                CancelGame();
        }

        public void Serialize(GenericWriter writer)
        {
            writer.Write(0);

            writer.Write(TurnIndex);
            writer.Write(RollIndex);
            writer.Write(Round);
            writer.Write(UsingJoker);
            
            if (CurrentRoll == null)
                CurrentRoll = Server.Engines.Yahtzee.Roll.Zero;

            CurrentRoll.Serialize(writer);

            writer.Write(Players.Count);
            Players.ForEach(e => e.Serialize(writer));
        }
	}
}