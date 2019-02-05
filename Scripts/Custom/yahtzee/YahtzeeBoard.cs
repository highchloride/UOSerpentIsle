using Server;
using System;
using Server.Mobiles;
using Server.Engines.Yahtzee;
using Server.Targeting;
using Server.Gumps;
using Server.Network;
using System.Collections.Generic;
using Server.ContextMenus;
using System.Linq;
using Server.Multis;

namespace Server.Items
{
	public class YahtzeeBoard : Item
	{
        [CommandProperty(AccessLevel.GameMaster)]
		public YahtzeeGame Game { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
		public RollOrder RollOrder { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
		public int PlayerCount { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public SecureLevel Level { get; set; }

        public List<Mobile> Pending { get; private set; }
	
		private object Lock = new object();
	
		[Constructable]
		public YahtzeeBoard() : base(4007)
		{
			PlayerCount = 2;
            Name = "A yahtzee dice and cup";

            Level = SecureLevel.Friends;
		}
		
		public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
		{
			base.GetContextMenuEntries(from, list);
			
            BaseHouse house = BaseHouse.FindHouseAt(this);

            var entry = new SimpleContextMenuEntry(from, 3000097, m =>
                {
                    if(!Deleted)
                        BaseGump.SendGump(new YahtzeeSetupGump(m as PlayerMobile, this));
                });

            if (!IsChildOf(from.Backpack))
            {
                if (!IsLockedDown || house == null || !house.HasSecureAccess(from, Level))
                {
                    entry.Enabled = false;
                }
            }
				
			list.Add(entry);
		}
		
		public override void OnDoubleClick(Mobile from)
		{
			if(from is PlayerMobile && Game == null && (Pending == null || !Pending.Contains(from)))
			{
                BaseGump.SendGump(new YahtzeeConfirmGump((PlayerMobile)from, "Join Yahtzee?", "Would you like to join the next yahtzee game?", () =>
                    {
                        lock (Lock)
                        {
                            if (Game != null)
                                YahtzeeGame.SendMessage(from, "The game has already begun. Maybe next time...", 2118);
                            else if (YahtzeeGame.IsInGame((PlayerMobile)from))
                                YahtzeeGame.SendMessage(from, "You can only play in one game of Yahtzee at a time!", 2118);
                            else
                            {
                                if (Pending == null)
                                {
                                    Pending = new List<Mobile>();
                                    Timer.DelayCall(TimeSpan.FromSeconds(120), () =>
                                    {
                                        if (Game == null)
                                        {
                                            Pending.ForEach(mob => mob.SendMessage("You lack the required players for Yahtzee!"));
                                            Pending.Clear();
                                            Pending = null;
                                        }
                                    });
                                }

                                Pending.Add(from);

                                if (Pending.Count > 1)
                                    ColUtility.ForEach(Pending.Where(mob => mob != from), mob => mob.SendMessage("{0} has joined the Yahtzee game!", from.Name));

                                if (Pending.Count >= PlayerCount)
                                {
                                    Game = new YahtzeeGame(Pending, this, RollOrder);
                                    Pending.ForEach(mob => YahtzeeGame.SendMessage(mob, "yahtzee will begin in 10 seconds!"));
                                    Pending.Clear();

                                    Timer.DelayCall(TimeSpan.FromSeconds(10), Game.BeginGame);
                                }
                                else
                                {
                                    Pending.ForEach(mob => YahtzeeGame.SendMessage(mob, String.Format("Still waiting on {0} more players!", PlayerCount - Pending.Count)));
                                }
                            }
                        }
                    }));
			}
            else if (Game != null && from is PlayerMobile)
            {
                PlayerEntry entry = Game.GetEntry((PlayerMobile)from);

                if(entry != null)
                    BaseGump.SendGump(new YahtzeeGump(entry, entry.Player, Game));
            }

		}
		
		public class YahtzeeSetupGump : BaseGump
		{
			public YahtzeeBoard Board { get; set; }

            public YahtzeeSetupGump(PlayerMobile pm, YahtzeeBoard board)
                : base(pm, 50, 50)
            {
                Board = board;
                //ButtonHandler = HandleButton;
            }

			public override void AddGumpLayout()
			{
				AddBackground(0, 0, 200, 300, 2620);

				AddHtml(0, 15, 200, 16, ColorAndCenter("#FFFFFF", "Yahtzee Setup"), false, false);
				
				AddGroup(1);
				AddHtml(15, 40, 200, 16, "<basefont color=#FFFFFF>Player Roll Order:", false, false);

                AddRadio(15, 60, 210, 211, Board.RollOrder == RollOrder.AsIs, 1);
                AddRadio(15, 80, 210, 211, Board.RollOrder == RollOrder.Random, 2);
                AddRadio(15, 100, 210, 211, Board.RollOrder == RollOrder.Roll, 3);
                
                AddHtml(50, 60, 200, 16, Color("#FFFFFF", "As Is"), false, false);
				AddHtml(50, 80, 200, 16, Color("#FFFFFF", "Random"), false, false);
				AddHtml(50, 100, 200, 16, Color("#FFFFFF", "Roll for Order"), false, false);
				
				AddGroup(2);
				AddHtml(15, 130, 200, 16, "<basefont color=#FFFFFF>Number of Players", false, false);
				for(int i = 0; i < YahtzeeGame.MaxPlayers; i++)
				{
					AddRadio(15, 150 + (i * 20), 210, 211, Board.PlayerCount == i + 1, i + 4);
					AddHtml(50, 150 + (i * 20), 100, 16, String.Format("<basefont color=#FFFFFF>{0}", (i + 1).ToString()), false, false);
				}
				
				AddButton(90, 270, 4023, 4024, 1, GumpButtonType.Reply, 0);
			}

            public override void OnResponse(RelayInfo info)
			{
                PlayerMobile from = User;
                int id = info.ButtonID;
				
				if(id == 0)
					return;
				
				RollOrder old = Board.RollOrder;

                if (info.IsSwitched(1) && old != RollOrder.AsIs)
					Board.RollOrder = RollOrder.AsIs;
                else if (info.IsSwitched(2) && old != RollOrder.Random)
					Board.RollOrder = RollOrder.Random;
                else if (info.IsSwitched(3) && old != RollOrder.Roll)
					Board.RollOrder = RollOrder.Roll;

                if (old != Board.RollOrder)
                {
                    YahtzeeGame.SendMessage(from, String.Format("Roll order has been changed to: {0}", Board.GetRollOrder()));
                    Board.InvalidateProperties();
                }

                int playerCount = Board.PlayerCount;

                for (int i = 1; i <= YahtzeeGame.MaxPlayers; i++)
                {
                    if (info.IsSwitched(i + 3) && playerCount != i)
                    {
                        Board.PlayerCount = i;
                        break;
                    }
                }

                if (playerCount != Board.PlayerCount)
                {
                    YahtzeeGame.SendMessage(User, String.Format("Amount of players have been changed: {0}", Board.PlayerCount.ToString()));
                    Board.InvalidateProperties();
                }

                Refresh();
			}
		}
		
		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);

            if (Game != null)
            {
                for (int i = 0; i < Game.Players.Count; i++)
                {
                    list.Add(1060658 + i, String.Format("{0}\t{1}", "Player" + (i + 1).ToString(), Game.Players[i].Player.Name));
                }
            }
            else
            {
                list.Add(1060659, String.Format("{0}\t{1}", "Roll Order", GetRollOrder()));
                list.Add(1060660, String.Format("{0}\t{1}", "Players", PlayerCount.ToString()));
            }
		}
		
		public string GetRollOrder()
		{
			switch(RollOrder)
			{
				default:
				case RollOrder.AsIs: return "As Is";
				case RollOrder.Random: return "Random";
				case RollOrder.Roll: return "Roll";
			}
		}

        public override void Delete()
        {
            base.Delete();

            if (Game != null)
                Game.CancelGame();
        }
		
		public YahtzeeBoard(Serial serial) : base(serial)
		{
		}
		
		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			
			writer.Write((int)0);

            writer.Write((int)Level);
			
			writer.Write((int)RollOrder);
			writer.Write(PlayerCount);

            if (Game != null)
            {
                writer.Write(1);
                Game.Serialize(writer);
            }
            else
                writer.Write(0);
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			
			int version = reader.ReadInt();

            Level = (SecureLevel)reader.ReadInt();

			RollOrder = (RollOrder)reader.ReadInt();
			PlayerCount = reader.ReadInt();

            if (reader.ReadInt() == 1)
            {
                Game = new YahtzeeGame(reader, this);
            }
		}
	}
}
