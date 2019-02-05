using System;

namespace Server.Items
{
	public class PaintBucket : Item
	{
		

		[Constructable]
		public PaintBucket() : base( 0x14E0 ) 
		{
			Name = "Bucket of Paint"; 
			Weight = 10.0; 
			Hue = 0; 
			
		}

		public PaintBucket( Serial serial ) : base( serial )
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

