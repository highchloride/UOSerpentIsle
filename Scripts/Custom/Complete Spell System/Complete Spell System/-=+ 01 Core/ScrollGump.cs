using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Spells;
using Server.ACC.CM;
using Server.ACC.CSS.Modules;

namespace Server.ACC.CSS
{
    public class ScrollGump : Gump
    {
        private CSpellInfo m_Info;
        private string m_TextHue;
        private CSpellbook m_Book;
        private CastInfo m_CastInfo;
        private CastCommandsModule m_CastCommandModule;

        public ScrollGump(CSpellbook book, CSpellInfo info, string textHue, Mobile sender)
            : base(485, 175)
        {
            if (info == null || book == null || !CSS.Running)
                return;

            m_Info = info;
            m_Book = book;
            m_TextHue = textHue;
            m_CastInfo = new CastInfo(info.Type, info.School);

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);
            AddBackground(0, 0, 200, 265, 9380);

            if (info.Name != null)
                AddHtml(30, 3, 140, 20, String.Format("<basefont color=#{0}><center>{1}</center></font>", textHue, info.Name), false, false);

            AddButton(30, 40, info.Icon, info.Icon, 3, GumpButtonType.Reply, 0);

            AddButton(90, 40, 2331, 2338, 1, GumpButtonType.Reply, 0);  //Cast
            AddLabel(120, 38, 0, "Cast");

            //AddButton( 90, 65, 2338, 2331, 2, GumpButtonType.Reply, 0 );  //Scribe
            //AddLabel( 120, 63, 0, "Scribe" );

            //Info
            string InfoString = "";
            if (info.Desc != null)
                InfoString += String.Format("<basefont color=black>{0}</font><br><br>", info.Desc);

            if (info.Regs != null)
            {
                string[] Regs = info.Regs.Split(';');
                InfoString += String.Format("<basefont color=black>Reagents :</font><br><basefont color=#{0}>", textHue);
                foreach (string r in Regs)
                    InfoString += String.Format("-{0}<br>", r.TrimStart());
                InfoString += "</font><br>";
            }

            if (info.Info != null)
            {
                string[] Info = info.Info.Split(';');
                InfoString += String.Format("<basefont color=#{0}>", textHue);
                foreach (string s in Info)
                    InfoString += String.Format("{0}<br>", s.TrimStart());
                InfoString += "</font><br>";
            }
            AddHtml(30, 95, 140, 130, InfoString, false, true);
            //End Info

            #region CastInfo
            if (CentralMemory.Running)
            {
                m_CastCommandModule = (CastCommandsModule)CentralMemory.GetModule(sender.Serial, typeof(CastCommandsModule));

                AddLabel(25, 242, 0, "Key :");
                if (m_CastCommandModule == null)
                    AddTextEntry(70, 242, 100, 20, 0, 5, "");  //Key
                else
                    AddTextEntry(70, 242, 100, 20, 0, 5, m_CastCommandModule.GetCommandForInfo(m_CastInfo));  //Key
                AddButton(175, 247, 2103, 2104, 4, GumpButtonType.Reply, 0);  //KeyButton
            }
            #endregion //CastInfo
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            if (info.ButtonID == 0 || !CSS.Running)
                return;

            else if (info.ButtonID == 1)
            {
                if (SpellRestrictions.UseRestrictions && !SpellRestrictions.CheckRestrictions(state.Mobile, m_Info.School))
                {
                    state.Mobile.SendMessage("You are not allowed to cast this spell.");
                    return;
                }

                if (!CSpellbook.MobileHasSpell(state.Mobile, m_Info.School, m_Info.Type))
                {
                    state.Mobile.SendMessage("You do not have this spell.");
                    return;
                }

                Spell spell = SpellInfoRegistry.NewSpell(m_Info.Type, m_Info.School, state.Mobile, null);
                if (spell == null)
                    state.Mobile.SendMessage("That spell is disabled.");
                else
                    spell.Cast();
            }

            else if (info.ButtonID == 2)
            {
                //Scribe
            }

            else if (info.ButtonID == 3)
            {
                if (!CentralMemory.Running)
                    return;

                if (SpellRestrictions.UseRestrictions && !SpellRestrictions.CheckRestrictions(state.Mobile, m_Info.School))
                    return;

                state.Mobile.SendGump(new IconPlacementGump(m_Book, state.Mobile, 100, 100, 10, m_Info.Icon, m_Info.Type, m_Info.Back, m_Book.School));
            }

            else if (info.ButtonID == 4)
            {
                if (!CentralMemory.Running)
                    return;

                string command = info.GetTextEntry(5).Text;

                if (command == null || command.Length == 0)
                {
                    if (m_CastCommandModule == null)
                    {
                        state.Mobile.SendGump(new ScrollGump(m_Book, m_Info, m_TextHue, state.Mobile));
                        return;
                    }

                    m_CastCommandModule.RemoveCommandByInfo(m_CastInfo);
                    state.Mobile.SendGump(new ScrollGump(m_Book, m_Info, m_TextHue, state.Mobile));
                }
                else
                {
                    if (m_CastCommandModule == null)
                    {
                        CentralMemory.AddModule(new CastCommandsModule(state.Mobile.Serial, command, m_CastInfo));
                        state.Mobile.SendGump(new ScrollGump(m_Book, m_Info, m_TextHue, state.Mobile));
                        return;
                    }

                    m_CastCommandModule.Add(command, m_CastInfo);
                    state.Mobile.SendGump(new ScrollGump(m_Book, m_Info, m_TextHue, state.Mobile));
                }
            }
        }
    }
}