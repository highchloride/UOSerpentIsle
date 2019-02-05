using System;

namespace Server.Items
{
	public class Canvas : Item
	{
		public override double DefaultWeight
		{
			get { return 1.0; } 
		}

		[Constructable]
		public Canvas() : this( 1 ) //The default amount is 1
		{
		}

		[Constructable]
		public Canvas( int amount ) : base( 0x1763) 
        {
			Name = "Canvas"; 
          		Amount = amount; 
			Hue = 0; 
            		Stackable = true; 
			
		}

		public Canvas( Serial serial ) : base( serial )
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

