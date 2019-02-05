using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Druid
{
	public class DruidGraspingRootsScroll : CSpellScroll
	{
		[Constructable]
		public DruidGraspingRootsScroll() : this( 1 )
		{
		}

		[Constructable]
		public DruidGraspingRootsScroll( int amount ) : base( typeof( DruidGraspingRootsSpell ), 0xE39, amount )
		{
			Name = "Grasping Roots";
			Hue = 0x58B;
		}

		public DruidGraspingRootsScroll( Serial serial ) : base( serial )
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
