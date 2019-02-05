using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Rogue
{
	public class RogueShadowScroll : CSpellScroll
	{
		[Constructable]
		public RogueShadowScroll() : this( 1 )
		{
		}

		[Constructable]
		public RogueShadowScroll( int amount ) : base( typeof( RogueShadowSpell ), 0xE39, amount )
		{
			Name = "Shadow";
			Hue = 0x20;
		}

		public RogueShadowScroll( Serial serial ) : base( serial )
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
