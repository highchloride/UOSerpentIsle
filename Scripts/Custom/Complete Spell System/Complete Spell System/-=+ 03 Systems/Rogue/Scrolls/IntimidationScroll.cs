using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Rogue
{
	public class RogueIntimidationScroll : CSpellScroll
	{
		[Constructable]
		public RogueIntimidationScroll() : this( 1 )
		{
		}

		[Constructable]
		public RogueIntimidationScroll( int amount ) : base( typeof( RogueIntimidationSpell ), 0xE39, amount )
		{
			Name = "Intimidation";
			Hue = 0x20;
		}

		public RogueIntimidationScroll( Serial serial ) : base( serial )
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
