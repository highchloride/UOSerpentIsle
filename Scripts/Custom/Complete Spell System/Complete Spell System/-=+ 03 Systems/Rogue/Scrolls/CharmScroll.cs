using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Rogue
{
	public class RogueCharmScroll : CSpellScroll
	{
		[Constructable]
		public RogueCharmScroll() : this( 1 )
		{
		}

		[Constructable]
		public RogueCharmScroll( int amount ) : base( typeof( RogueCharmSpell ), 0xE39, amount )
		{
			Name = "Charm";
			Hue = 0x20;
		}

		public RogueCharmScroll( Serial serial ) : base( serial )
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
