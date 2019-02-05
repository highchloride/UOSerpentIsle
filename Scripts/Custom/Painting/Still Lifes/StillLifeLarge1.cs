using System;

namespace Server.Items
{
    [FlipableAttribute(0x2411, 0x2412)]
    
	public class StillLifeLarge1 : Item
	{
		

		[Constructable]
		public StillLifeLarge1() : base( 0x2413 )
		{
			 
			Weight = 3.0; 
			Hue = 0; 
			
		}

    [Constructable]
    public StillLifeLarge1(string artistName, string subject) : base(0x2411)
    {
      Name = string.Format("A painting of {0} by {1}",subject,artistName);
      Weight = 3.0;
      Hue = 0;

    }

    public StillLifeLarge1( Serial serial ) : base( serial )
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

