using System;

namespace Server.Items
{
    [FlipableAttribute(0x2417, 0x2418)]
    public class AbstractPainting1 : Item
	{


        [Constructable]
        public AbstractPainting1(string artistName, string subject) : base(0x2417)
        {
            Weight = 3.0;
            Hue = 0;
            Name = string.Format("An abstract painting titled {0} by {1}", subject, artistName);

        }


        public AbstractPainting1( Serial serial ) : base( serial )
		{
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

