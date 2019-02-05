using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Cleric
{
	public class ClericAngelicFaithScroll : CSpellScroll
	{
		[Constructable]
		public ClericAngelicFaithScroll() : this( 1 )
		{
		}

		[Constructable]
		public ClericAngelicFaithScroll( int amount ) : base( typeof( ClericAngelicFaithSpell ), 0xE39, amount )
		{
			Name = "Angelic Faith";
			Hue = 0x1F0;
		}

		public ClericAngelicFaithScroll( Serial serial ) : base( serial )
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
