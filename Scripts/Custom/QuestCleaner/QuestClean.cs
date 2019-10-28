/**************************** QuestClean.cs *******************************
 *
 *					        (C) 2015, by Lokai
 *	
 * Lets you view information about completed and active quests, and lets
 * you delete quests from the targeted Player
 *   
/***************************************************************************
 *
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation; either version 2 of the License, or
 *   (at your option) any later version.
 *
 ***************************************************************************/

using System;
using Server.Commands;
using Server.Targeting;
using Server.Gumps;
using Server.Engines.Quests;
using Server.Mobiles;
using Server.Network;

namespace Server.Custom
{
    public class QuestCleanCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("QC", AccessLevel.GameMaster, new CommandEventHandler(QuestClean_OnCommand));
            CommandSystem.Register("QuestClean", AccessLevel.GameMaster, new CommandEventHandler(QuestClean_OnCommand));
        }

        [Usage("QuestClean")]
        [Aliases("QC")]
        [Description("Shows the QuestCleanGump.")]
        public static void QuestClean_OnCommand(CommandEventArgs e)
        {
            if (e.Arguments.Length == 0) e.Mobile.Target = new SelectPlayerTarget();
            else
            {
                //Try to find the Player nearby using the string argument.
                try
                {
                    IPooledEnumerable<Mobile> mobiles = e.Mobile.GetMobilesInRange(15);
                    foreach (var mobile in mobiles)
                    {
                        if (mobile is PlayerMobile)
                        {
                            PlayerMobile pm = (PlayerMobile) mobile;
                            if (pm.Name.ToLower().Contains(e.Arguments[0].ToLower()))
                            {
                                e.Mobile.SendGump(new QuestCleanGump(e.Mobile, pm));
                                return;
                            }
                        }
                    }
                }
                catch
                {
                    e.Mobile.Target = new SelectPlayerTarget();
                }
            }
            e.Mobile.SendMessage("Target a Player to view Quest Information.");
        }

        private class SelectPlayerTarget : Target
        {

            public SelectPlayerTarget()
                : base(18, true, TargetFlags.None)
            {
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is PlayerMobile)
                {
                    from.SendGump(new QuestCleanGump(from, (PlayerMobile)targeted));
                }
                else
                {
                    from.SendMessage("You must target a Player!");
                }
            }
        }
    }

    public class QuestCleanGump : CG
    {
        private PlayerMobile mPlayer;
        private int mPage;
        private int mIndex;

        public QuestCleanGump(Mobile from, PlayerMobile mobile)
            : this(from, mobile, 1)
        {
        }

        public QuestCleanGump(Mobile from, PlayerMobile mobile, int page)
            : this(from, mobile, page, 0, 50, 50)
        {
        }

        public QuestCleanGump(Mobile from, PlayerMobile mobile, int page, int index, int x, int y)
            : base(x, y)
        {
            mPlayer = mobile;
            mPage = page;
            mIndex = index;

            AddBackground(80, 15, 405, 655, 9300); // Will only be visible on older client
            AddImage(0, 0, 30236); // Image 30236 - Only visible on latest client
            AddHtml(432, 22, 70, 20, Italic("Active"), GreenHue, false, false);

            int questNum;
            string questString = "";

            if (page == 1)
            {
                // Add page forward button
                AddButton(439, 39, 9728, 9728, 3, GumpButtonType.Reply, 0);  //436,21
                AddHtml(95, 20, 300, 20, Bold(String.Format("{0}'s Completed Quests", mPlayer.Name)), GreenHue, false,
                    false);
                questNum = mobile.DoneQuests.Count;
                questString = "completed";
            }
            else
            {
                // Add page back button
                AddButton(439, 39, 9730, 9730, 4, GumpButtonType.Reply, 0);
                AddHtml(95, 20, 300, 20, Bold(String.Format("{0}'s Current Quests", mPlayer.Name)), GreenHue, false,
                    false);
                questNum = mobile.Quests.Count;
                questString = "current";
            }
            if (questNum > 0)
            {
                if (index < questNum - 1)
                {
                    AddButton(355, 50, 2224, 2224, 13, GumpButtonType.Reply, 0); // Add index forward button
                    AddImage(319, 51, 2509);
                }

                if (index > 0)
                {
                    AddButton(165, 50, 2223, 2223, 14, GumpButtonType.Reply, 0); // Add index back button
                    AddImage(190, 51, 2508);
                }

                try
                {
                    BaseQuest quest;
                    if (page == 1)
                    {
                        Type questType = mobile.DoneQuests[index].QuestType;
                        quest = (BaseQuest)Activator.CreateInstance(questType);
                        AddButton(245, 47, 5531, 5532, index + 1000, GumpButtonType.Reply, 0); // Button to delete from completed quests
                    }
                    else
                    {
                        quest = mobile.Quests[index];
                        AddButton(245, 47, 5531, 5532, index + 2000, GumpButtonType.Reply, 0); // Button to delete from active quests
                    }
                    AddDetail(quest);
                }
                catch
                {
                    AddLabel(90, 75, 0, String.Format("There was an exception reading {0}'s {1} quests.", mPlayer.Name, questString));
                }
            }
            else
            {
                AddLabel(90, 75, 0, String.Format("{0} does not have any {1} quests.", mPlayer.Name, questString));
            }

        }

        private void AddDetail(BaseQuest quest)
        {
            AddLabel(90, 75, GreenHue, "Title");
            AddHtml(90, 95, 185, 50, quest.Title, 0x20, true, true);
            AddLabel(280, 75, GreenHue, "Chain ID");
            AddHtml(280, 95, 185, 50, quest.ChainID.ToString(), 0x20, true, true);
            AddLabel(90, 150, GreenHue, "Description");
            AddHtml(90, 170, 380, 100, quest.Description, 0x20, true, true);
            AddLabel(90, 275, GreenHue, "Refuse");
            AddHtml(90, 295, 380, 100, quest.Refuse, 0x20, true, true);
            AddLabel(90, 400, GreenHue, "Complete");
            AddHtml(90, 420, 380, 100, quest.Complete, 0x20, true, true);
            AddLabel(90, 525, GreenHue, "Uncomplete");
            AddHtml(90, 545, 380, 100, quest.Uncomplete, 0x20, true, true);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            int button = info.ButtonID;

            if (button == 3)
            {
                sender.Mobile.SendGump(new QuestCleanGump(sender.Mobile, mPlayer, mPage + 1, 0, X + 5, Y + 5));
            }
            else if (button == 4)
            {
                sender.Mobile.SendGump(new QuestCleanGump(sender.Mobile, mPlayer, mPage - 1, 0, X + 5, Y + 5));
            }
            else if (button == 13)
            {
                sender.Mobile.SendGump(new QuestCleanGump(sender.Mobile, mPlayer, mPage, mIndex + 1, X + 5, Y + 5));
            }
            else if (button == 14)
            {
                sender.Mobile.SendGump(new QuestCleanGump(sender.Mobile, mPlayer, mPage, mIndex - 1, X + 5, Y + 5));
            }
            else if (button >= 2000)
            {
                string message = string.Format("Removed {0}'s: Active Quest: {1}", mPlayer.Name,
                    mPlayer.Quests[button - 2000].GetType().Name);
                CommandLogging.WriteLine(sender.Mobile, message);
                mPlayer.Quests[button - 2000].RemoveQuest();
                sender.Mobile.SendGump(new QuestCleanGump(sender.Mobile, mPlayer, mPage, 0, X + 5, Y + 5));
                sender.Mobile.SendGump(new DisplaySuccessGump(X + 5, Y + 5, message));
            }
            else if (button >= 1000)
            {
                string message = string.Format("Removed {0}'s: Completed Quest: {1}", mPlayer.Name,
                    mPlayer.DoneQuests[button - 1000].QuestType.Name);
                CommandLogging.WriteLine(sender.Mobile, message);
                mPlayer.DoneQuests.RemoveAt(button - 1000);
                sender.Mobile.SendGump(new QuestCleanGump(sender.Mobile, mPlayer, mPage, 0, X + 5, Y + 5));
                sender.Mobile.SendGump(new DisplaySuccessGump(X + 5, Y + 5, message));
            }
        }
    }

    public class DisplaySuccessGump : CG
    {
        public DisplaySuccessGump(int x, int y, string message)
            : base(x, y)
        {
            Closable = false;
            Disposable = false;
            Dragable = false;
            Resizable = false;

            AddBackground(0, 0, 300, 150, 9270); // Paper Background
            AddLabel(23, 23, 0x480, "Success!");

            string[] lines = message.Split(':');

            AddLabel(13, 63, 0x481, lines[0]);
            AddLabel(13, 83, 0x481, lines[1]);
            AddLabel(13, 103, 0x481, lines[2]);

            AddButton(114, 26, (int)WordButtons.Okay1, (int)WordButtons.Okay1, 2, GumpButtonType.Reply, 0); // OKAY Button
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (info.ButtonID == 2)
                sender.Mobile.CloseGump(typeof (DisplaySuccessGump));
        }
    }
}
