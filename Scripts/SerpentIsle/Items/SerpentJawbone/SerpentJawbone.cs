using Server.Gumps;
using Server.Mobiles;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Server.Items
{
    public class SerpentJawbone : Item
    {
        private List<SerpentTooth> m_SerpentTeeth; //A list of SerpentTooth objects

        [CommandProperty(AccessLevel.Seer)]
        public List<SerpentTooth> SerpentTeeth
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
            m_SerpentTeeth = new List<SerpentTooth>();

        }

        public SerpentJawbone(Serial serial)
            : base(serial)
        {
        }

        public static bool IsValidTile(int itemID)
        {
            //Serpent Gate Tiles
            return (itemID == 16109 || itemID == 16110 || itemID == 16111 || itemID == 16112);
        }

        public override void OnDoubleClick(Mobile from)
        {
            //base.OnDoubleClick(from);

            if (!this.IsChildOf(from.Backpack))
                from.SendMessage("This must be in thy pack to use it.");
            else if (this.m_SerpentTeeth.Count == 0)
                from.SendMessage("Thy Jawbone is empty.");
            else
            {
                //Get the serpent gate the player is standing on
                IPooledEnumerable enumerable = Map.GetItemsInRange(from.Location, 0);

                SerpentTeleporterTileAddon serpentGate = null;

                foreach (Item item in enumerable)
                {
                    if (IsValidTile(item.ItemID))
                    {
                        AddonComponent component = item as AddonComponent;
                        serpentGate = component.Addon as SerpentTeleporterTileAddon;
                    }
                }

                //If not standing on a tele, show the gump instead.
                if (serpentGate != null)
                {
                    if (serpentGate.RequiredTooth != null) //If there's a required tooth, check for its presence. For Dark Path teleporters.
                    {
                        foreach (SerpentTooth tooth in SerpentTeeth)
                        {
                            if (serpentGate.RequiredTooth.GetType() == tooth.GetType())
                            {
                                from.SendMessage("Thy jawbone reacts with the gate!");
                                from.SetLocation(serpentGate.Destination, true); //Send the player to the destination of the tooth
                                return;
                            }
                        }
                        from.SendMessage("Thy jawbone appears to be missing the tooth for this gate."); //The player doesn't have the required tooth.
                    }
                    else //No required tooth - this is a world teleporter
                    {
                        from.SendMessage("Thy jawbone reacts with the gate!");
                        from.SetLocation(new Point3D(2185, 1491, 6), true);//Send to Dark Path
                        return;
                    }
                }
                else if (from is PlayerMobile)
                {
                    from.CloseGump(typeof(GumpSerpentJawbone));
                    GumpSerpentJawbone gump = new GumpSerpentJawbone(this.m_SerpentTeeth);
                    from.SendGump(gump);
                }
            }            
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            if (this.SerpentTeeth != null)
                list.Add(this.SerpentTeeth.Count + " teeth");
            else
                list.Add("0 teeth");
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version

            writer.WriteList(m_SerpentTeeth, (w, t) => w.WriteType(t));
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();

            if(m_SerpentTeeth == null)
            {
                m_SerpentTeeth = new List<SerpentTooth>();
            }
            List<Type> types = new List<Type>();
            types = reader.ReadList(r => r.ReadType());
            foreach(Type obj in types)
            {
                if(obj == typeof(SerpentToothMonitor))
                    m_SerpentTeeth.Add(Activator.CreateInstance<SerpentToothMonitor>());
            }
        }

        public class GumpSerpentJawbone : Gump
        {
            public GumpSerpentJawbone(List<SerpentTooth> serpentTeeth) : base(0,0)
            {
                this.Closable=true;
			    this.Disposable=true;
			    this.Dragable=true;
			    this.Resizable=false;

			    AddPage(0);

                AddBackground(0, 0, 363, 405, 9380);
                AddLabel(140, 5, 68, @"Collected Teeth");

                foreach(SerpentTooth tooth in serpentTeeth)
                {
                    if(tooth is SerpentToothMonitor)
                        AddItem(10, 50, tooth.ItemID, tooth.Hue); //Monitor
                    //else if (tooth is SerpentToothFawn)
                        
                    else if(tooth is SerpentToothMoonshade)
                        AddItem(10, 86, tooth.ItemID, tooth.Hue); //Moonshade
                }                    

                AddLabel(60, 50, 0, @"Monitor");

                //AddItem(177, 50, 22343);
                AddLabel(227, 50, 0, @"Fawn");
                
                AddLabel(60, 86, 0, @"Moonshade");

                //AddItem(177, 86, 22343);
                AddLabel(227, 86, 0, @"Sleeping Bull");

                //AddItem(10, 122, 22343);
                AddLabel(60, 122, 0, @"Furnace");

                //AddItem(177, 122, 22343);
                AddLabel(227, 122, 0, @"Emotion");

                //AddItem(10, 158, 22343);
                AddLabel(60, 158, 0, @"Skullcrusher");

                //AddItem(177, 158, 22343);
                AddLabel(227, 158, 0, @"Balance");
                
                //AddItem(10, 194, 22343);
                AddLabel(60, 194, 0, @"Spinebreaker");

                //AddItem(177, 194, 22343);
                AddLabel(227, 194, 0, @"Discipline");

                //AddItem(10, 230, 22343);
                AddLabel(60, 230, 0, @"Monk Isle");

                //AddItem(177, 230, 22343);
                AddLabel(227, 230, 0, @"Isle of Crypts");

                //AddItem(10, 266, 22343);
                AddLabel(60, 266, 0, @"Northern Forest");

                //AddItem(177, 266, 22343);
                AddLabel(227, 266, 0, @"Logic");

                //AddItem(10, 302, 22343);
                AddLabel(60, 302, 0, @"Ethic");

                //AddItem(177, 302, 22343);
                AddLabel(227, 302, 0, @"Tolerance");

                //AddItem(10, 338, 22343);
                AddLabel(60, 338, 0, @"Enthusiasm");

                //AddItem(177, 338, 22343);
                AddLabel(227, 338, 0, @"Mad Mage Isle");
            }
        }
    }
}
