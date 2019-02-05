using System;
using Server;

namespace Server.Items
{
	public class DestroyingAngel : BaseReagent, ICommodity
	{
		TextDefinition ICommodity.Description
		{
			get
			{
				return String.Format( "{0} destroying angel", Amount );
			}
		}

        //int ICommodity.DescriptionNumber { get { return LabelNumber; } }

        bool ICommodity.IsDeedable { get { return false; } }

		[Constructable]
		public DestroyingAngel() : this( 1 )
		{
		}

		[Constructable]
		public DestroyingAngel( int amount ) : base( 0xE1F, amount )
		{
			Hue = 0x290;
			Name = "destroying angel";
		}

		public DestroyingAngel( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}
