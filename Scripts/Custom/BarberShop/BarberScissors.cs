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
	public class BarberScissors : Item
	{
		[Constructable]
		public BarberScissors() : base( 0xDFC )
		{
		}

		public BarberScissors( Serial serial ) : base( serial )
		{
		}

        public override void OnDoubleClick(Mobile from)
        {

            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                return;
            }
            if (from.FacialHairItemID != 0 && from.HairItemID != 0)
            {
                from.SendGump(new BarberScissorTarget(from));
                return;
            }

            //Shortest facial hairs // Goatee or Moustache //Full beard // Full beard w/ Moustache
            if (from.HairItemID == 0)
            {
                if (from.FacialHairItemID == 0x2040 || from.FacialHairItemID == 0x204D || from.FacialHairItemID == 0x2041 || from.FacialHairItemID == 0x203F || from.FacialHairItemID == 0x204B)
                {
                    from.SendMessage("You will need a razor to manage close facial hair.");
                    return;
                }                
                //Long Beard // Long beard w/ moustache
                else if (from.FacialHairItemID == 0x203E || from.FacialHairItemID == 0x204C)
                {
                    Point3D scissorloc = from.Location; // added cuthair
                    CutHair cuthair = new CutHair();
                    cuthair.Location = scissorloc;
                    cuthair.MoveToWorld(scissorloc, from.Map);

                    from.SendMessage("You cut your beard short.");
                    from.FacialHairItemID = 0x204B; // Full beard w moustache
                    return;
                }
                else
                {
                    from.SendMessage("You've nothing to cut.");
                    return;
                }
            }
            else
            {
                //veeery short // mohawk, krisna
                if (from.HairItemID == 0x2044 || from.HairItemID == 0x204A)
                {
                    from.SendMessage("You cannot cut your hair shorter. Try use a razor on it.");
                    return;
                }

                //short
                if (from.HairItemID == 0x2045 || from.HairItemID == 0x2047 || from.HairItemID == 0x203B || from.HairItemID == 0x2047 || from.HairItemID == 0x2FBF || from.HairItemID == 0x2FC0 || from.HairItemID == 0x2FC2 || from.HairItemID == 0x2FCE || from.HairItemID == 0x2FD0) 
                {
                    Point3D scissorloc = from.Location; // added cuthair
                    CutHair cuthair = new CutHair();
                    cuthair.Location = scissorloc;
                    cuthair.MoveToWorld(scissorloc, from.Map);

                    from.SendMessage("You cut your hair.");
                    from.HairItemID = 0x2048;
                    from.PlaySound(0x249); // added sound
                    return;
                }                

                //short / receeding

                if (from.HairItemID == 0x2048 || from.HairItemID == 0x2FC1 || from.HairItemID == 0x2FD1 || from.HairItemID == 0x203B) // receeding
                {

                    from.SendMessage("You cannot cut your hair shorter. Try using a razor on it.");
                    return;
                }

                if (from.HairItemID == 0)
                {
                    from.SendMessage("You cannot cut your hair shorter. There is none!");
                    return;
                }


                else
                {
                    Point3D scissorloc = from.Location;
                    CutHair cuthair = new CutHair();
                    cuthair.Location = scissorloc;
                    cuthair.MoveToWorld(scissorloc, from.Map);

                    from.SendMessage("You cut your hair.");
                    from.HairItemID = 0x2045;
                    from.PlaySound(0x249);
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

    public class BarberScissorTarget : Gump
    {
        private Mobile m_From;
        private object m_Targeted;

        public BarberScissorTarget(Mobile from) : base(0, 0)
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
                                state.Mobile.SendMessage("You cannot cut your hair shorter. Try use a razor on it.");
                                return;
                            }

                            //short
                            if (state.Mobile.HairItemID == 0x2045 || state.Mobile.HairItemID == 0x2047 || state.Mobile.HairItemID == 0x203B || state.Mobile.HairItemID == 0x2047 || state.Mobile.HairItemID == 0x2FBF || state.Mobile.HairItemID == 0x2FC0 || state.Mobile.HairItemID == 0x2FC2 || state.Mobile.HairItemID == 0x2FCE || state.Mobile.HairItemID == 0x2FD0)
                            {
                                Point3D scissorloc = state.Mobile.Location; // added cuthair
                                CutHair cuthair = new CutHair();
                                cuthair.Location = scissorloc;
                                cuthair.MoveToWorld(scissorloc, state.Mobile.Map);

                                state.Mobile.SendMessage("You cut your hair.");
                                state.Mobile.HairItemID = 0x2048;
                                state.Mobile.PlaySound(0x249); // added sound
                                return;
                            }

                            //short / receeding

                            if (state.Mobile.HairItemID == 0x2048 || state.Mobile.HairItemID == 0x2FC1 || state.Mobile.HairItemID == 0x2FD1 || state.Mobile.HairItemID == 0x203B) // receeding
                            {

                                state.Mobile.SendMessage("You cannot cut your hair shorter. Try using a razor on it.");
                                return;
                            }

                            if (state.Mobile.HairItemID == 0)
                            {
                                state.Mobile.SendMessage("You cannot cut your hair shorter. There is none!");
                                return;
                            }


                            else
                            {
                                Point3D scissorloc = state.Mobile.Location;
                                CutHair cuthair = new CutHair();
                                cuthair.Location = scissorloc;
                                cuthair.MoveToWorld(scissorloc, state.Mobile.Map);

                                state.Mobile.SendMessage("You cut your hair.");
                                state.Mobile.HairItemID = 0x2045;
                                state.Mobile.PlaySound(0x249);
                                return;
                            }
                        }
                        else
                        {   //Beard
                            if (state.Mobile.FacialHairItemID == 0x2040 || state.Mobile.FacialHairItemID == 0x204D || state.Mobile.FacialHairItemID == 0x2041 || state.Mobile.FacialHairItemID == 0x203F || state.Mobile.FacialHairItemID == 0x204B)
                            {
                                state.Mobile.SendMessage("You will need a razor to manage close facial hair.");
                                return;
                            }
                            //Long Beard // Long beard w/ moustache
                            else if (state.Mobile.FacialHairItemID == 0x203E || state.Mobile.FacialHairItemID == 0x204C)
                            {
                                Point3D scissorloc = state.Mobile.Location; // added cuthair
                                CutHair cuthair = new CutHair();
                                cuthair.Location = scissorloc;
                                cuthair.MoveToWorld(scissorloc, state.Mobile.Map);

                                state.Mobile.SendMessage("You cut your beard short.");
                                state.Mobile.FacialHairItemID = 0x204B; // Full beard w moustache
                                return;
                            }
                            else
                            {
                                state.Mobile.SendMessage("You've nothing to cut.");
                                return;
                            }
                        }
                        break;
                    }
            }

        }
    }
}
