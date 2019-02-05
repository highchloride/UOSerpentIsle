using System;

namespace Server.Items
{
	[FlipableAttribute(0x2887, 0x2886)]
	public class AbstractPainting3 : Item
	{


        [Constructable]
        public AbstractPainting3(string artistName, string subject) : base(0x2887)
        {
            Name = string.Format("An abstract painting titled {0} by {1}", subject, artistName);
            Weight = 3.0;
            Hue = 0;
            

        }

        public AbstractPainting3( Serial serial ) : base( serial )
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

