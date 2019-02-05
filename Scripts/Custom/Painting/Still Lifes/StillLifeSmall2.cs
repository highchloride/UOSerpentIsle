using System;

namespace Server.Items
{
	[FlipableAttribute(0x240F, 0x2410)]
	public class StillLifeSmall2 : Item
	{
		

		[Constructable]
		public StillLifeSmall2() : base( 0x240F )
		{
			 
			Weight = 3.0; 
			Hue = 0; 
			
		}


    [Constructable]
    public StillLifeSmall2(string artistName, string subject) : base(0x240F)
    {
      Name = string.Format("A painting of {0} by {1}", subject, artistName);
      Weight = 3.0;
      Hue = 0;

    }
    public StillLifeSmall2( Serial serial ) : base( serial )
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

