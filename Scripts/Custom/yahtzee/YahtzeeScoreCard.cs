using Server;
using System;
using Server.Mobiles;
using Server.Items;
using Server.Gumps;
using System.Collections.Generic;
using System.Linq;

namespace Server.Engines.Yahtzee
{
	public class YahtzeeScoreCard : Item
	{
        [CommandProperty(AccessLevel.GameMaster)]
		public PlayerEntry Entry { get; set; }
	
        [CommandProperty(AccessLevel.GameMaster)]
		public YahtzeeGame Game { get; set; }

		[Constructable]
        public YahtzeeScoreCard(PlayerEntry entry, YahtzeeGame game)
            : base(0xE17)
		{
            Entry = entry;
            Game = game;

            Name = "Yahtzee Score Card";
		}

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            if (Game == null && Entry != null)
                list.Add(1060739, Entry.Score.ToString()); // score: ~1_val~
            else if (Game != null)
            {
                for (int i = 0; i < Game.Players.Count; i++)
                {
                    list.Add(1060658 + i, String.Format("{0}\t{1}", "Player" + (i + 1).ToString(), Game.Players[i].Player.Name));
                }
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (Entry != null && IsChildOf(from.Backpack))
            {
                BaseGump.SendGump(new YahtzeeGump(Entry, from as PlayerMobile, Game));
            }
        }

        public override void Delete()
        {
            base.Delete();

            Entry = null;
        }

		public YahtzeeScoreCard(Serial serial) : base(serial)
		{
		}
		
		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			
			writer.Write((int)0);

            if (Entry != null && Entry.Game == null)
            {
                writer.Write(1);
                Entry.Serialize(writer);
            }
            else
                writer.Write(0);
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			
			int version = reader.ReadInt();

            if (reader.ReadInt() == 1)
                Entry = new PlayerEntry(reader, null);
		}
	}
}