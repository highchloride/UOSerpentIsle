using System;

namespace Server.Items
{
	[FlipableAttribute(0xEA2, 0xEA1)]
	public class MalePortrait : Item
	{
		

		[Constructable]
		public MalePortrait(string artistName, string subject) : base( 0xEA2 )
		{
            if (artistName == subject)
            {
                Name = string.Format("A self portrait of {0}", artistName, subject);
            }
            else
            {
                Name = string.Format("A portrait of {0} By {1}", artistName, subject);

                Weight = 3.0;
                Hue = 0;

            }
           
			
		}

		public MalePortrait( Serial serial ) : base( serial )
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

