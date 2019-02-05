using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Cleric
{
	public class ClericTrialByFireScroll : CSpellScroll
	{
		[Constructable]
		public ClericTrialByFireScroll() : this( 1 )
		{
		}

		[Constructable]
		public ClericTrialByFireScroll( int amount ) : base( typeof( ClericTrialByFireSpell ), 0xE39, amount )
		{
			Name = "Trial By Fire";
			Hue = 0x1F0;
		}

		public ClericTrialByFireScroll( Serial serial ) : base( serial )
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
