using System;
using System.Collections;
using System.Collections.Generic;

namespace Server.Items
{
    public enum SerpentsTeeth
    {
        Monitor = 0,
        Fawn = 1,
        Moonshade = 3,
        SleepingBull = 2
    }        

    public class SerpentJawbone : Item
    {

        private List<SerpentsTeeth> m_SerpentTeeth;

        [CommandProperty(AccessLevel.Seer)]
        public List<SerpentsTeeth> SerpentTeeth
        {
            get { return m_SerpentTeeth; }
            set
            {
                m_SerpentTeeth = value;
                InvalidateProperties();
            }
        }

		[Constructable]
		public SerpentJawbone() : base(0x0F05)
		{
			Name = "Serpent's Jawbone";
		}

        public SerpentJawbone(Serial serial)
            : base(serial)
        {
        }

        public bool AddToJawbone(SerpentsTeeth tooth)
        {
            if(m_SerpentTeeth.Contains(tooth))
            {
                return false;
            }
            else
            {
                m_SerpentTeeth.Add(tooth);
                return true;
            }
            
        }

        //This might let us add any tooth of any sort to the jawbone
        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            if(dropped is SerpentTooth)
            {
                SerpentTooth tooth = (SerpentTooth)dropped;
                Console.WriteLine("Dropped a serpent's tooth");
                if(!AddToJawbone(tooth.Tooth))
                {
                    from.SendMessage("Thy jawbone already contains this tooth.");
                }
                else
                {
                    tooth.Delete();
                    from.SendMessage("Thy tooth has been added to the jawbone.");
                }
            }

            Console.WriteLine("Dunno what this is.");

            return base.OnDragDrop(from, dropped);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();
        }
    }
}
