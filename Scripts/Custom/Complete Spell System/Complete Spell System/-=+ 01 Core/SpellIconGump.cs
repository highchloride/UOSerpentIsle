using System;
using System.Text;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Spells;
using Server.ACC.CM;
using Server.ACC.CSS.Modules;

namespace Server.ACC.CSS
{
    public class SpellIconGump : Gump
    {
        private IconInfo m_Info;

        public SpellIconGump(IconInfo info)
            : base(((Point3D)info.Location).X, ((Point3D)info.Location).Y)
        {
            m_Info = info;

            Closable = false;
            Disposable = false;
            Dragable = false;
            Resizable = false;

            AddPage(0);
            AddBackground(0, 0, 54, 54, ((Point3D)info.Location).Z);
            AddButton(45, 0, 9008, 9010, 1, GumpButtonType.Reply, 0);
            AddButton(5, 5, m_Info.Icon, m_Info.Icon, 2, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            if (!CSS.Running || !CentralMemory.Running)
                return;

            switch (info.ButtonID)
            {
                case 1:
                    {
                        IconsModule mod = new IconsModule(state.Mobile.Serial, m_Info);
                        CentralMemory.AppendModule(state.Mobile.Serial, (Module)mod, true);
                        state.Mobile.SendMessage("That icon has been removed and will not open again.");
                        break;
                    }

                case 2:
                    {
                        if (!Multis.DesignContext.Check(state.Mobile))
                        {
                            state.Mobile.SendMessage("You cannot cast while customizing!");
                            state.Mobile.SendGump(new SpellIconGump(m_Info));
                            return;
                        }
                        if (SpellRestrictions.UseRestrictions && !SpellRestrictions.CheckRestrictions(state.Mobile, m_Info.SpellType))
                        {
                            state.Mobile.SendMessage("You are not allowed to cast this spell.");
                            return;
                        }

                        if (!CSpellbook.MobileHasSpell(state.Mobile, m_Info.School, m_Info.SpellType))
                        {
                            state.Mobile.SendMessage("You do not have this spell.");
                            goto case 1;
                        }

                        Spell spell = SpellInfoRegistry.NewSpell(m_Info.SpellType, m_Info.School, state.Mobile, null);
                        if (spell != null)
                            spell.Cast();
                        else
                        {
                            state.Mobile.SendMessage("This spell has been disabled.");
                            goto case 1;
                        }

                        state.Mobile.SendGump(new SpellIconGump(m_Info));
                        break;
                    }
            }
        }
    }
}