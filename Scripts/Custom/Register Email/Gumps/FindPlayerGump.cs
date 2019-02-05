using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Commands;
using System.Collections;
using Server.Gumps;

namespace Server.Gumps
{
	public class FindPlayersGump : Gump
	{
		public FindPlayersGump() : base(0, 0)
		{
			Closable = true;
			Dragable = true;
			Resizable = false;

			AddBackground( 131, 155, 271, 187, 9250 );

			AddLabel(217, 170, 999, "Email Player");
			AddHtml(147, 190, 242, 87, "", true, true);

			AddLabel(151, 282, 999, "Player Name:");
			AddAlphaRegion( 153, 301, 179, 25 );
			AddTextEntry(153, 301, 179, 25, 0, 2, "");

            AddButton(340, 301, 4005, 4006, 1, GumpButtonType.Reply, 0);
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			Mobile from = sender.Mobile;

            if (info.ButtonID == 1)
            {
                TextRelay txt = (TextRelay)info.GetTextEntry(2);
                string name = (string)(txt.Text.ToLower()).Trim();

                ArrayList players = (ArrayList)GetPlayers(name);

                if (players.Count == 0)
                {
                    from.SendMessage("No players with part or all of that name were found.");
                    return;
                }

                from.CloseGump(typeof(SelectPlayerGump));
                from.SendGump(new SelectPlayerGump(players));
            }
		}

        public ArrayList GetPlayers(string name)
        {
            ArrayList players = new ArrayList();
            foreach (Mobile m in World.Mobiles.Values)
            {
                if (m is PlayerMobile)
                {
                    string test = m.Name.ToLower();
                    if (test == name || test.Contains(name))
                        players.Add(m);
                }
            }

            return players;
        }
	}
}
