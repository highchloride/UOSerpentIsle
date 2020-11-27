namespace Server.Items
{
    public class SmallTent : BaseAddon
    {
        public override BaseAddonDeed Deed => null;

        [Constructable]
        public SmallTent()
        {

	// 0x36A --> 0x1F2 bottom walls
	//0x369 --> 0x1F1 front walls
	//0x36E --> 0x1F9 back walls
	//0x36F --> 0x1F8 top walls

            int hue = 1000;

            AddonComponent comp = new AddonComponent(0x369);
            comp.Hue = hue;
            AddComponent(comp, 0, 3, 0);

            comp = new AddonComponent(0x36A);
            comp.Hue = hue;
            AddComponent(comp, 1, 1, 0);


            comp = new AddonComponent(0x36A);
            comp.Hue = hue;
            AddComponent(comp, 1, -1, 0);


            comp = new AddonComponent(0x36F);
            comp.Hue = hue;
            AddComponent(comp, -3, -1, 0);


            comp = new AddonComponent(0x36F);
            comp.Hue = hue;
            AddComponent(comp, -3, 1, 0);


            comp = new AddonComponent(0x36E);
            comp.Hue = hue;
            AddComponent(comp, -1, -2, 0);


            comp = new AddonComponent(0x36C); // top fron corner --> 0x1F4
            comp.Hue = hue;
            AddComponent(comp, -3, 3, 0);


            comp = new AddonComponent(0x369);
            comp.Hue = hue;
            AddComponent(comp, -2, 3, 0);


            comp = new AddonComponent(0x368); // bottom front corner --> 0x1F5
            comp.Hue = hue;
            AddComponent(comp, 1, 3, 0);

            comp = new AddonComponent(0x36A);
            comp.Hue = hue;
            AddComponent(comp, 1, 2, 0);

            comp = new AddonComponent(0x36A);
            comp.Hue = hue;
            AddComponent(comp, 1, 0, 0);

            comp = new AddonComponent(0x36D);
            comp.Hue = hue;
            AddComponent(comp, 1, -2, 0);

            comp = new AddonComponent(0x36E);
            comp.Hue = hue;
            AddComponent(comp, 0, -2, 0);

            comp = new AddonComponent(0x36E);
            comp.Hue = hue;
            AddComponent(comp, -2, -2, 0);

            comp = new AddonComponent(0x36F);
            comp.Hue = hue;
            AddComponent(comp, -3, 0, 0);

            comp = new AddonComponent(0x36F);
            comp.Hue = hue;
            AddComponent(comp, -3, 2, 0);

            AddComponent(new AddonComponent(0x36B), -3, -2, 0);

            // South/East Corner
            comp = new AddonComponent(0x663);
            comp.Hue = hue;
            AddComponent(comp, 1, 3, 18);
            comp = new AddonComponent(0x663);
            comp.Hue = hue;
            AddComponent(comp, 0, 2, 21);

            // North/East Corner
            comp = new AddonComponent(0x664);
            comp.Hue = hue;
            AddComponent(comp, 1, -1, 18);
            comp = new AddonComponent(0x664);
            comp.Hue = hue;
            AddComponent(comp, 0, 0, 21);


            // South/West Corner
            comp = new AddonComponent(0x666);
            comp.Hue = hue;
            AddComponent(comp, -2, 3, 18);
            comp = new AddonComponent(0x666);
            comp.Hue = hue;
            AddComponent(comp, -1, 2, 21);


            // North/West Corner
            comp = new AddonComponent(0x665);
            comp.Hue = hue;
            AddComponent(comp, -2, -1, 18);
            comp = new AddonComponent(0x665);
            comp.Hue = hue;
            AddComponent(comp, -1, 0, 21);


	    //south facing
            comp = new AddonComponent(0x601);
            comp.Hue = hue;
            AddComponent(comp, 0, 3, 18);

            comp = new AddonComponent(0x601);
            comp.Hue = hue;
            AddComponent(comp, -1, 3, 18);


	   // north facing
           comp = new AddonComponent(0x600);
           comp.Hue = hue;
           AddComponent(comp, -1, -1, 18);
           comp = new AddonComponent(0x600);
           comp.Hue = hue;
           AddComponent(comp, -0, -1, 18);

	    // west facing
	    comp = new AddonComponent(0x5FF);
            comp.Hue = hue;
            AddComponent(comp, -2, 0, 18);
	    comp = new AddonComponent(0x5FF);
            comp.Hue = hue;
            AddComponent(comp, -2, 1, 18);
	    comp = new AddonComponent(0x5FF);
            comp.Hue = hue;
            AddComponent(comp, -2, 2, 18);

	    comp = new AddonComponent(0x634); //0x5FF
            comp.Hue = hue;
            AddComponent(comp, -1, 1, 21);

	    //east facing
	    comp = new AddonComponent(0x602);
            comp.Hue = hue;
            AddComponent(comp, 1, 0, 18);  
	    comp = new AddonComponent(0x602);
            comp.Hue = hue;
            AddComponent(comp, 1, 1, 18);
	    comp = new AddonComponent(0x602);
            comp.Hue = hue;
            AddComponent(comp, 1, 2, 18);  


	    comp = new AddonComponent(0x635); //0x602
            comp.Hue = hue;
            AddComponent(comp, 0, 1, 21);

        }

        public SmallTent(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}