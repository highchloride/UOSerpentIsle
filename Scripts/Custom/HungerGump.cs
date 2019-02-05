// une petite commande .hungry parce que j'en ai marre de pas savoir si j'ai faim ou pas
// j'aurais pu faire plus simple pour la commande mais je voulais quelle soit integrable dans le Help
// elle affiche un pti gump mimi , qui vous donne numériquement la valeur de votre faim et soif 

/* a small hungry order because I have some enough of step knowledge if I am hungry or not
I could have made simpler for the order but I wanted which is integrable in Help it posts
a pti gump mimi, which numerically gives you the value of your hunger and thirst */

using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Gumps;
using Server.Commands;

namespace Server.Commands
{
	public class Hungry
	{
		public static void Initialize()
		{
			CommandSystem.Register( "Hunger", AccessLevel.Player, new CommandEventHandler( Hungry_OnCommand ) );
			CommandSystem.Register( "Thirst", AccessLevel.Player, new CommandEventHandler( Hungry_OnCommand ) );
		}
		
		[Usage( "Hunger || Thirst" )]
		[Description( "Show your level of hunger and thirst." )]
		public static void Hungry_OnCommand( CommandEventArgs e )
		{
			Mobile from = e.Mobile;
			from.CloseGump( typeof( gumpfaim ) );
			from.SendGump( new gumpfaim ( from ) );
		}
		
	}
}

namespace Server.Gumps
{
	public class gumpfaim : Gump
	{
		
		public gumpfaim(Mobile caller) : base(0,0)
		{
			PlayerMobile from = (PlayerMobile)caller;

			Closable = true;
			Dragable = true;
			
			AddPage(0);
			
			AddBackground( 0, 0, /*295*/ 215, 144, 5054);
			AddBackground( 7, 7, /*261*/ 203, 134, 3500);
            AddLabel(80, 25, 0, "Status");
			AddLabel( 60, 42, from.Hunger < 6 ? 33 : 0, string.Format( "Hunger: {0} / 20", from.Hunger));
			AddLabel( 60, 61, from.Thirst < 6 ? 33 : 0, string.Format( "Thirst: {0} / 20", from.Thirst));
			AddItem( 8, 58, 8093);
			AddItem( 19, 40, 4155);
            AddLabel(60, 90, 0, "Refresh");
            AddButton(115, 90, 2130, 2129, 1, GumpButtonType.Reply, 1); // Okay button
        }
		
		public override void OnResponse( Server.Network.NetState sender, RelayInfo info )
		{
            if (info == null || sender == null || sender.Mobile == null) return;

            if (info.ButtonID == 1)
            {
                PlayerMobile from = null;

                if (sender.Mobile is PlayerMobile)
                {
                    from = (PlayerMobile)sender.Mobile;
                    from.CloseGump(typeof(gumpfaim));
                    from.SendGump(new gumpfaim(from));
                }
            }
        }
	}
}
