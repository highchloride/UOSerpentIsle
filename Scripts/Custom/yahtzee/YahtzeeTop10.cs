using Server;
using System;
using Server.Mobiles;
using Server.Engines.Yahtzee;
using System.Collections.Generic;
using System.Linq;
using Server.Gumps;

namespace Server.Items
{
	public class YahtzeeStatsBoard : Item
	{
        [Constructable]
        public YahtzeeStatsBoard()
            : base(7775)
        {
            Movable = false;
            Name = "Yahtzee Top 10";
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from is PlayerMobile && from.InRange(this.GetWorldLocation(), 3))
            {
                BaseGump.SendGump(new InternalGump(from as PlayerMobile));
            }
        }

        public class InternalGump : BaseGump
        {
            public InternalGump(PlayerMobile user)
                : base(user, 50, 50)
            {
            }

            public override void AddGumpLayout()
            {
                AddBackground(0, 0, 410, 345, 5170);

                AddLabel(40, 3, 55, "Top 10 Yahtzee Scores");
                
                AddLabel(20, 20, 1149, "View");
                AddLabel(70, 20, 1149, "Player");
                AddLabel(250, 20, 1149, "Score");

                int y = 40;

                YahtzeeStats.Top10.Sort();

                for (int i = 0; i < YahtzeeStats.Top10.Count; i++)
                {
                    var entry = YahtzeeStats.Top10[i];

                    AddButton(25, (y + 3) + (i * 25), 2224, 2224, i + 1, GumpButtonType.Reply, 0);
                    AddLabel(70, y + (i * 25), 300, entry.Player.Name);
                    AddLabel(250, y + (i * 25), 300, entry.Score.ToString());
                }
            }

            public override void OnResponse(RelayInfo info)
            {
                var id = info.ButtonID - 1;

                if (id >= 0 && id < YahtzeeStats.Top10.Count)
                {
                    Refresh();
                    BaseGump.SendGump(new YahtzeeGump(YahtzeeStats.Top10[id], User, null));
                }
            }
        }

		public YahtzeeStatsBoard(Serial serial) : base(serial)
		{
		}
		
		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			
			writer.Write((int)0);
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			
			int version = reader.ReadInt();
		}
	}

    public static class YahtzeeStats
    {
        public const string FilePath = @"Saves\\CustomGames\\Yahtzee.bin";

        public static void Configure()
        {
            EventSink.WorldSave += OnSave;
            EventSink.WorldLoad += OnLoad;

            Top10 = new List<PlayerEntry>();
        }

        public static void OnSave(WorldSaveEventArgs e)
        {
            Persistence.Serialize(
                FilePath,
                writer =>
                {
                    writer.Write((int)0);

                    writer.Write(Top10.Count);

                    Top10.ForEach(entry => entry.Serialize(writer));
                });
        }

        public static void OnLoad()
        {
            Top10 = new List<PlayerEntry>();

            Persistence.Deserialize(
                FilePath,
                reader =>
                {
                    int version = reader.ReadInt();

                    int count = reader.ReadInt();
                    for (int i = 0; i < count; i++)
                    {
                        PlayerEntry entry = new PlayerEntry(reader, null);

                        if (entry.Player != null)
                            Top10.Add(entry);
                    }
                });

            Top10.Sort();
        }

        public static List<PlayerEntry> Top10 { get; private set; }

        public static bool CheckScore(PlayerEntry entry)
        {
            if(Top10 == null)
                Top10 = new List<PlayerEntry>();

            Top10.Sort();

            if (Top10.Count < 10)
            {
                Top10.Add(entry);
                return true;
            }
            else
            {
                List<PlayerEntry> copy = new List<PlayerEntry>(Top10);
                foreach (PlayerEntry e in copy)
                {
                    if (entry.Score > e.Score)
                    {
                        Top10.Remove(Top10[Top10.Count-1]);
                        Top10.Add(entry);
                        return true;
                    }
                }
            }

            return false;
        }
    }
}