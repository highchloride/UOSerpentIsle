using System; 
using System.Collections.Generic;
using Felladrin.Commands;
using Server.Gumps;
using Server; 

namespace Server.Mobiles 
{ 
	public class Barber : BaseVendor 
	{ 
		private List<SBInfo> m_SBInfos = new List<SBInfo>();
		protected override List<SBInfo> SBInfos{ get { return m_SBInfos; } } 

		[Constructable]
		public Barber() : base( "the barber" ) 
		{ 
			SetSkill( SkillName.Alchemy, 80.0, 100.0 );
			SetSkill( SkillName.Magery, 90.0, 110.0 );
			SetSkill( SkillName.TasteID, 85.0, 100.0 );
		} 

		public override void InitSBInfo() 
		{ 
			m_SBInfos.Add( new SBBarber() ); 
		} 

		public Barber( Serial serial ) : base( serial ) 
		{ 
		}

        public override void OnSpeech(SpeechEventArgs e)
        {
            base.OnSpeech(e);

            var from = e.Mobile;

            if (this is BaseVendor && from.InRange(this, Core.AOS ? 1 : 4) && !e.Handled)
            {
                if ((e.Speech.Contains("vendor")) && (e.Speech.Contains("haircut") || e.Speech.Contains("shave")))
                {
                    e.Handled = true;

                    this.FocusMob = from;

                    from.CloseGump(typeof(ChangeHairHueGump));
                    from.CloseGump(typeof(ChangeHairstyleGump));

                    from.SendGump(new ChangeHairStyle.ChangeHairHueGump(from, ChangeHairStyle.Config.PriceForHairHue, true, true, (ChangeHairStyle.Config.DisplayRegularHues ? ChangeHairStyle.ChangeHairHueEntry.RegularEntries : ChangeHairStyle.ChangeHairHueEntry.BrightEntries)));

                    if (ChangeHairStyle.Config.PriceForHairHue > 0)
                        from.SendMessage(65, "You'll be charged {0} gold, directly from your bank, if you choose to change your hair hue.", ChangeHairStyle.Config.PriceForHairHue);

                    if (from.Race == Race.Human)
                    {
                        from.SendGump(new ChangeHairStyle.ChangeHairstyleGump(from, ChangeHairStyle.Config.PriceForHairStyle, false, ChangeHairStyle.ChangeHairstyleEntry.HairEntries));

                        if (ChangeHairStyle.Config.PriceForHairStyle > 0)
                            from.SendMessage(67, "You'll be charged {0} gold, directly from your bank, if you choose to change your hair style.", ChangeHairStyle.Config.PriceForHairStyle);

                        if (!from.Female)
                        {
                            from.SendGump(new ChangeHairStyle.ChangeHairstyleGump(from, ChangeHairStyle.Config.PriceForFacialHairStyle, true, ChangeHairStyle.ChangeHairstyleEntry.BeardEntries));

                            if (ChangeHairStyle.Config.PriceForFacialHairStyle > 0)
                                from.SendMessage(66, "You'll be charged {0} gold, directly from your bank, if you choose to change your facial hair style.", ChangeHairStyle.Config.PriceForFacialHairStyle);
                        }
                    }
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
}
