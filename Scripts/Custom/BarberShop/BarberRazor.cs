using System;
using Server;
using System.Collections;
using Server.Targeting;
using Server.Mobiles;
using Server.Items;
using Server.Gumps;
using Server.Network;

namespace Server.Items
{
	public class BarberRazor : Item
	{
		[Constructable]
		public BarberRazor() : base( 0xEC4 )
		{
            Name = "Barber Razor";
		}

		public BarberRazor( Serial serial ) : base( serial )
		{

		}

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                return;
            }
            if(from.FacialHairItemID != 0 && from.HairItemID != 0)
            {
                from.SendGump(new BarberRazorTarget(from));
                return;
            }

            //Shortest facial hairs // Goatee or Moustache
            if (from.HairItemID == 0)
            {
                if (from.FacialHairItemID == 0x2040 || from.FacialHairItemID == 0x204D)
                {
                    Point3D scissorloc = from.Location; // added cuthair
                    CutHair cuthair = new CutHair();
                    cuthair.Location = scissorloc;
                    cuthair.MoveToWorld(scissorloc, from.Map);

                    from.SendMessage("You shave off all of your beard.");
                    from.FacialHairItemID = 0; // no hair
                    return;
                }
                //Full beard // Full beard w/ Moustache
                else if (from.FacialHairItemID == 0x2041 || from.FacialHairItemID == 0x203F || from.FacialHairItemID == 0x204B)
                {
                    Point3D scissorloc = from.Location; // added cuthair
                    CutHair cuthair = new CutHair();
                    cuthair.Location = scissorloc;
                    cuthair.MoveToWorld(scissorloc, from.Map);

                    from.SendMessage("You shave off some of your beard.");
                    from.FacialHairItemID = 0x204D; // moustache and goatee
                    return;
                }
                //Long Beard // Long beard w/ moustache
                else if (from.FacialHairItemID == 0x203E || from.FacialHairItemID == 0x204C)
                {
                    from.SendMessage("You will need to cut your beard before you can use a razor on it.");
                    return;
                }
                else
                {
                    from.SendMessage("You've nothing to shave.");
                    return;
                }
            }
            else
            {
                //veeery short // mohawk, krisna
                if (from.HairItemID == 0x2044 || from.HairItemID == 0x204A)
                {
                    Point3D scissorloc = from.Location; // added cuthair
                    CutHair cuthair = new CutHair();
                    cuthair.Location = scissorloc;
                    cuthair.MoveToWorld(scissorloc, from.Map);

                    from.SendMessage("You shave your hair.");
                    from.HairItemID = 0; // no hair
                    return;
                }

                // Middle
                if (from.HairItemID == 0x2045 || from.HairItemID == 0x2047 || from.HairItemID == 0x203B || from.HairItemID == 0x2047 || from.HairItemID == 0x2FBF || from.HairItemID == 0x2FC0 || from.HairItemID == 0x2FC2 || from.HairItemID == 0x2FCE || from.HairItemID == 0x2FD0)
                {
                    Point3D scissorloc = from.Location; // added cuthair
                    CutHair cuthair = new CutHair();
                    cuthair.Location = scissorloc;
                    cuthair.MoveToWorld(scissorloc, from.Map);

                    from.SendMessage("You shave your hair.");
                    from.HairItemID = 0x2044; // mohawk
                    return;
                }

                // Short
                if (from.HairItemID == 0x2048 || from.HairItemID == 0x2FC1 || from.HairItemID == 0x2FD1 || from.HairItemID == 0x203B) // receeding
                {
                    Point3D scissorloc = from.Location; // added cuthair
                    CutHair cuthair = new CutHair();
                    cuthair.Location = scissorloc;
                    cuthair.MoveToWorld(scissorloc, from.Map);

                    from.SendMessage("You shave your hair.");
                    from.HairItemID = 0; // no hair
                    return;
                }

                if (from.HairItemID == 0)
                {
                    from.SendMessage("You cannot shave your hair. You are bald!");
                    return;
                }

                else
                {
                    from.SendMessage("You cannot shave your hair. First cut it a bit.");
                    return;
                }
            }
        }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

    public class BarberRazorTarget : Gump
    {
        private Mobile m_From;
        private object m_Targeted;

        public BarberRazorTarget(Mobile from) : base(0, 0)
        {
            if (from == null) return;

            m_From = from;

            Closable = false;
            Dragable = true;
            AddPage(0);
            AddBackground(10, 200, 200, 130, 5054);

            AddLabel(18, 210, 68, String.Format("Cut Hair or Beard?"));

            AddRadio(32, 255, 9721, 9724, false, 1); // accept/yes radio
            AddRadio(132, 255, 9721, 9724, true, 2); // decline/no radio
            //AddHtmlLocalized(72, 255, 200, 30, 1049016, 0x7fff, false, false); // Yes
            AddLabel(70, 255, 0, "Hair");
            //AddHtmlLocalized(172, 255, 200, 30, 1049017, 0x7fff, false, false); // No
            AddLabel(170, 255, 0, "Beard");
            AddButton(80, 289, 2130, 2129, 3, GumpButtonType.Reply, 0); // Okay button

        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            if (info == null || state == null || state.Mobile == null) return;

            int radiostate = -1;
            if (info.Switches.Length > 0)
            {
                radiostate = info.Switches[0];
            }
            switch (info.ButtonID)
            {
                default:
                    {
                        if (radiostate == 1)
                        {    // Hair
                            //veeery short // mohawk, krisna
                            if (state.Mobile.HairItemID == 0x2044 || state.Mobile.HairItemID == 0x204A)
                            {
                                Point3D scissorloc = state.Mobile.Location; // added cuthair
                                CutHair cuthair = new CutHair();
                                cuthair.Location = scissorloc;
                                cuthair.MoveToWorld(scissorloc, state.Mobile.Map);

                                state.Mobile.SendMessage("You shave your hair.");
                                state.Mobile.HairItemID = 0; // no hair
                                return;
                            }

                            // Middle
                            if (state.Mobile.HairItemID == 0x2045 || state.Mobile.HairItemID == 0x2047 || state.Mobile.HairItemID == 0x203B || state.Mobile.HairItemID == 0x2047 || state.Mobile.HairItemID == 0x2FBF || state.Mobile.HairItemID == 0x2FC0 || state.Mobile.HairItemID == 0x2FC2 || state.Mobile.HairItemID == 0x2FCE || state.Mobile.HairItemID == 0x2FD0)
                            {
                                Point3D scissorloc = state.Mobile.Location; // added cuthair
                                CutHair cuthair = new CutHair();
                                cuthair.Location = scissorloc;
                                cuthair.MoveToWorld(scissorloc, state.Mobile.Map);

                                state.Mobile.SendMessage("You shave your hair.");
                                state.Mobile.HairItemID = 0x2044; // mohawk
                                return;
                            }

                            // Short
                            if (state.Mobile.HairItemID == 0x2048 || state.Mobile.HairItemID == 0x2FC1 || state.Mobile.HairItemID == 0x2FD1 || state.Mobile.HairItemID == 0x203B) // receeding
                            {
                                Point3D scissorloc = state.Mobile.Location; // added cuthair
                                CutHair cuthair = new CutHair();
                                cuthair.Location = scissorloc;
                                cuthair.MoveToWorld(scissorloc, state.Mobile.Map);

                                state.Mobile.SendMessage("You shave your hair.");
                                state.Mobile.HairItemID = 0; // no hair
                                return;
                            }
                            else
                            {
                                state.Mobile.SendMessage("You cannot shave your hair. First cut it a bit.");
                                return;
                            }
                        }
                        else
                        {
                            if (state.Mobile.FacialHairItemID == 0x2040 || state.Mobile.FacialHairItemID == 0x204D)
                            {
                                Point3D scissorloc = state.Mobile.Location; // added cuthair
                                CutHair cuthair = new CutHair();
                                cuthair.Location = scissorloc;
                                cuthair.MoveToWorld(scissorloc, state.Mobile.Map);

                                state.Mobile.SendMessage("You shave off all of your beard.");
                                state.Mobile.FacialHairItemID = 0; // no hair
                                return;
                            }
                            else if (state.Mobile.FacialHairItemID == 0x2041 || state.Mobile.FacialHairItemID == 0x203F || state.Mobile.FacialHairItemID == 0x204B)
                            {
                                Point3D scissorloc = state.Mobile.Location; // added cuthair
                                CutHair cuthair = new CutHair();
                                cuthair.Location = scissorloc;
                                cuthair.MoveToWorld(scissorloc, state.Mobile.Map);

                                state.Mobile.SendMessage("You shave off some of your beard.");
                                state.Mobile.FacialHairItemID = 0x204D; // moustache and goatee
                                return;
                            }
                            else if (state.Mobile.FacialHairItemID == 0x203E || state.Mobile.FacialHairItemID == 0x204C)
                            {
                                state.Mobile.SendMessage("You will need to cut your beard before you can use a razor on it.");
                                return;
                            }
                        }
                        break;
                    }
            }

        }
    }
}
