using System;
using System.Collections;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Spells;
using Server.Network;

namespace Server.ACC.CSS
{
    public abstract class CSpellbookGump : Gump
    {
        private CSpellbook m_Book;
        private ArrayList m_Spells;

        private int Pages;
        private int CurrentPage;

        public abstract string TextHue { get; }
        public abstract int BGImage { get; }
        public abstract int SpellBtn { get; }
        public abstract int SpellBtnP { get; }
        public abstract string Label1 { get; }
        public abstract string Label2 { get; }
        public abstract Type GumpType { get; }

        public CSpellbookGump(CSpellbook book)
            : base(50, 100)
        {
            if (!CSS.Running)
                return;

            m_Book = book;
            m_Spells = book.SchoolSpells;

            Pages = (int)Math.Ceiling((book.SpellCount / 16.0));

            /*			if( Pages > 1 && book.Mark > 0 )
                        {
                            ArrayList temp = new ArrayList();
                            for( int i = 0; i < book.Mark*16 && i < m_Spells.Count; i++ )
                                temp.Add( m_Spells[i] );
                            m_Spells.RemoveRange( 0, (book.Mark*16)-1 );
                            m_Spells.AddRange( temp );
                        }
            */

            AddPage(0);
            AddImage(100, 100, BGImage);

            CurrentPage = 1;

            for (int i = 0; i < Pages; i++, CurrentPage++)
            {
                AddPage(CurrentPage);

                //Hidden Buttons
                for (int j = (CurrentPage - 1) * 16, C = 0; j < CurrentPage * 16 && j < m_Spells.Count; j++, C++)
                {
                    if (HasSpell((Type)m_Spells[j]))
                    {
                        AddButton((C > 7 ? 305 : 145), 142 + (C > 7 ? (C - 8) * 20 : C * 20), 2482, 2482, j + 1000, GumpButtonType.Reply, 0);
                    }
                }
                AddImage(100, 100, BGImage);
                AddHtml(165, 107, 100, 20, String.Format("<basefont color=#{0}><Center>{1}</Center>", TextHue, Label1), false, false);
                AddHtml(285, 107, 100, 20, String.Format("<basefont color=#{0}><Center>{1}</Center>", TextHue, Label2), false, false);
                //End Hidden Buttons

                //Prev/Next Buttons
                if (Pages > 1)
                {
                    if (CurrentPage > 1)
                        AddButton(122, 109, 2205, 2205, 0, GumpButtonType.Page, Pages - CurrentPage - 1);
                    if (CurrentPage < Pages)
                        AddButton(394, 104, 2206, 2206, 0, GumpButtonType.Page, CurrentPage + 1);
                }
                //End Prev/Next Buttons

                //Spell Buttons/Labels
                for (int j = (CurrentPage - 1) * 16, C = 0; j < CurrentPage * 16 && j < m_Spells.Count; j++, C++)
                {
                    if (HasSpell((Type)m_Spells[j]))
                    {
                        CSpellInfo info = SpellInfoRegistry.GetInfo(m_Book.School, (Type)m_Spells[j]);
                        if (info == null)
                            continue;

                        AddHtml((C > 7 ? 305 : 145), 140 + (C > 7 ? (C - 8) * 20 : C * 20), 120, 20, String.Format("<basefont color=#{0}>{1}</basefont>", TextHue, info.Name), false, false);
                        AddButton((C > 7 ? 285 : 125), 143 + (C > 7 ? (C - 8) * 20 : C * 20), SpellBtn, SpellBtnP, j + 2000, GumpButtonType.Reply, 0);
                        AddButton((C > 7 ? 410 : 250), 142 + (C > 7 ? (C - 8) * 20 : C * 20), 5411, 5411, j + 1000, GumpButtonType.Reply, 0);
                    }
                }
                //End Spell Buttons/Labels
            }
        }

        public bool HasSpell(Type type)
        {
            return (m_Book != null && m_Book.HasSpell(type));
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            if (info.ButtonID == 0 || !CSS.Running)
                return;

            else if (info.ButtonID >= 1000 && info.ButtonID < (1000 + m_Spells.Count))
            {
                if (SpellRestrictions.UseRestrictions && !SpellRestrictions.CheckRestrictions(state.Mobile, m_Book.School))
                {
                    state.Mobile.SendMessage("You are not allowed to cast this spell.");
                    return;
                }

                CSpellInfo si = SpellInfoRegistry.GetInfo(m_Book.School, (Type)m_Spells[info.ButtonID - 1000]);
                if (si == null)
                {
                    state.Mobile.SendMessage("That spell is disabled.");
                    return;
                }
                state.Mobile.CloseGump(typeof(ScrollGump));
                state.Mobile.SendGump(new ScrollGump(m_Book, si, TextHue, state.Mobile));
                //				m_Book.Mark = (info.ButtonID-1000)/16;
                //				state.Mobile.SendMessage( "{0}", m_Book.Mark );
            }

            else if (info.ButtonID >= 2000 && info.ButtonID < (2000 + m_Spells.Count))
            {
                if (SpellRestrictions.UseRestrictions && !SpellRestrictions.CheckRestrictions(state.Mobile, m_Book.School))
                {
                    state.Mobile.SendMessage("You are not allowed to cast this spell.");
                    return;
                }

                if (!CSpellbook.MobileHasSpell(state.Mobile, m_Book.School, (Type)m_Spells[info.ButtonID - 2000]))
                {
                    state.Mobile.SendMessage("You do not have this spell.");
                    return;
                }

                Spell spell = SpellInfoRegistry.NewSpell((Type)m_Spells[info.ButtonID - 2000], m_Book.School, state.Mobile, null);
                if (spell == null)
                    state.Mobile.SendMessage("That spell is disabled.");
                else
                    spell.Cast();
                //				m_Book.Mark = (info.ButtonID-2000)/16;
                //				state.Mobile.SendMessage( "{0}", m_Book.Mark );
            }

            object[] Params = new object[1] { m_Book };
            CSpellbookGump gump = Activator.CreateInstance(GumpType, Params) as CSpellbookGump;
            if (gump != null)
                state.Mobile.SendGump(gump);

            //GumpUpTimer
        }
    }
}