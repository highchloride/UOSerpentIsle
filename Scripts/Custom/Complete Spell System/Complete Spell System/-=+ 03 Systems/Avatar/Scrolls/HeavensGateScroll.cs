using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Avatar
{
	public class AvatarHeavensGateScroll : CSpellScroll
	{
		[Constructable]
		public AvatarHeavensGateScroll() : this( 1 )
		{
		}

		[Constructable]
		public AvatarHeavensGateScroll( int amount ) : base( typeof( AvatarHeavensGateSpell ), 0xE39, amount )
		{
			Name = "Heaven's Gate";
			Hue = 1174;
		}

		public AvatarHeavensGateScroll( Serial serial ) : base( serial )
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
