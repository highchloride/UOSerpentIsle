using System;
using Server;
using Server.Gumps;
using Server.Mobiles;

namespace Server.Items
{

	public class PaintBrush : Item
	{
		//public override CraftSystem CraftSystem{ get{ return DefPainting.CraftSystem; } }

		[Constructable]
		public PaintBrush( ) : base( 0xFC1 )
		{
			Weight = 2.0;
            Name = "Paint Brush";
            Hue = 0;
			
		}

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
            {
                from.SendGump(new PaintingGump(from, this));
            }
            else
            {
                from.SendLocalizedMessage(1042001); // This must be in your backpack to use it
            }
        }


        public PaintBrush(Serial serial): base(serial)
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

			if ( Weight == 1.0 )
				Weight = 2.0;
		}
	}
}