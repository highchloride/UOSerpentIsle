using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Druid
{
	public class DruidSwarmOfInsectsScroll : CSpellScroll
	{
		[Constructable]
		public DruidSwarmOfInsectsScroll() : this( 1 )
		{
		}

		[Constructable]
		public DruidSwarmOfInsectsScroll( int amount ) : base( typeof( DruidSwarmOfInsectsSpell ), 0xE39 )
		{
			Name = "Swarm Of Insects";
			Hue = 0x58B;
		}

		public DruidSwarmOfInsectsScroll( Serial serial ) : base( serial )
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
