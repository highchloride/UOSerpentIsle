using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Commands;
using VitaNex.FX;
using Server.Items;

namespace Server.Gumps
{
    public class GumpSailToSerpentIsle : Gump
    {
        Mobile caller;

        public GumpSailToSerpentIsle(Mobile from) : this()
        {
            caller = from;
        }

        public GumpSailToSerpentIsle() : base(200, 200)
        {
            this.Closable = true;
            this.Disposable = false;
            this.Dragable = true;
            this.Resizable = false;

            AddPage(0);
            
            AddBackground(0, 0, 267, 279, 9300);
            AddImage(9, 8, 2529);
            AddLabel(73, 26, 2, @"Journey Onward...");
            AddButton(162, 240, 12000, 12001, (int)Buttons.Button1, GumpButtonType.Reply, 0);
            AddButton(25, 240, 12018, 12019, (int)Buttons.Button2, GumpButtonType.Reply, 0);
            AddHtml(23, 73, 215, 137, @"Art thou prepared to set sail for the Serpent Pillars?

There is no telling when thou wilt return...", (bool)false, (bool)false);           
        }

        public enum Buttons
        {
            Button1,
            Button2,
        }


        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            SkillStudyBook book = null;
            ScrollCharCreate scroll = null;
            StatCodex codex = null;

            //REMOVE STARTING ITEMS
            foreach(Item item in from.Backpack.Items)
            {
                if(item.GetType() == typeof(SkillStudyBook))
                {
                    book = (SkillStudyBook)item;
                }
                else if(item.GetType() == typeof(ScrollCharCreate))
                {
                    scroll = (ScrollCharCreate)item;
                }
                else if(item.GetType() == typeof(StatCodex))
                {
                    codex = (StatCodex)item;
                }
            }

            if (book != null)
                from.Backpack.RemoveItem(book);

            if (scroll != null)
                from.Backpack.RemoveItem(scroll);

            if (codex != null)
                from.Backpack.RemoveItem(codex);

            //PROCESS
            switch (info.ButtonID)
            {
                case (int)Buttons.Button1:
                    {
                        var effect = new EnergyExplodeEffect(caller.ToPoint3D(), caller.Map, 7)
                        {
                            Reversed = true
                        };
                        effect.Send();
                        caller.SendSound(0x5C9);
                        Timer.DelayCall(TimeSpan.FromSeconds(2.2), new TimerCallback(SendToSerpentIsle));
                        break;
                    }
                case (int)Buttons.Button2:
                    {
                        caller.CantWalk = false;
                        break;
                    }

            }
        }

        private void SendToSerpentIsle()
        {
            caller.SendSound(0x5C9);
            caller.MoveToWorld(new Point3D(175, 1335, 0), Map.SerpentIsle);
            caller.CantWalk = false;
        }
    }
}
