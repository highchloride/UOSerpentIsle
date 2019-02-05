using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Avatar
{
	public class AvatarHeavenlyLightScroll : CSpellScroll
	{
		[Constructable]
		public AvatarHeavenlyLightScroll() : this( 1 )
		{
		}

		[Constructable]
		public AvatarHeavenlyLightScroll( int amount ) : base( typeof( AvatarHeavenlyLightSpell ), 0xE39, amount )
		{
			Name = "Heavenly Light";
			Hue = 1174;
		}

		public AvatarHeavenlyLightScroll( Serial serial ) : base( serial )
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
