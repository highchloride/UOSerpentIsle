using System;
using Server.Items;

namespace Server.ACC.CSS.Systems.Ancient
{ 
	public class LesserBladeTrapDeed : HouseTrapDeed
	{
		[Constructable]
		public LesserBladeTrapDeed() : base( HouseTrapStrength.Lesser, HouseTrapType.Blades )
		{
		}

		public LesserBladeTrapDeed( Serial serial ) : base( serial )
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

	public class RegularBladeTrapDeed : HouseTrapDeed
	{
		[Constructable]
		public RegularBladeTrapDeed() : base( HouseTrapStrength.Regular, HouseTrapType.Blades )
		{
		}

		public RegularBladeTrapDeed( Serial serial ) : base( serial )
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

	public class GreaterBladeTrapDeed : HouseTrapDeed
	{
		[Constructable]
		public GreaterBladeTrapDeed() : base( HouseTrapStrength.Greater, HouseTrapType.Blades )
		{
		}

		public GreaterBladeTrapDeed( Serial serial ) : base( serial )
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

	public class DeadlyBladeTrapDeed : HouseTrapDeed
	{
		[Constructable]
		public DeadlyBladeTrapDeed() : base( HouseTrapStrength.Deadly, HouseTrapType.Blades )
		{
		}

		public DeadlyBladeTrapDeed( Serial serial ) : base( serial )
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

	public class LesserExplosionTrapDeed : HouseTrapDeed
	{
		[Constructable]
		public LesserExplosionTrapDeed() : base( HouseTrapStrength.Lesser, HouseTrapType.Explosion )
		{
		}

		public LesserExplosionTrapDeed( Serial serial ) : base( serial )
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

	public class RegularExplosionTrapDeed : HouseTrapDeed
	{
		[Constructable]
		public RegularExplosionTrapDeed() : base( HouseTrapStrength.Regular, HouseTrapType.Explosion )
		{
		}

		public RegularExplosionTrapDeed( Serial serial ) : base( serial )
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

	public class GreaterExplosionTrapDeed : HouseTrapDeed
	{
		[Constructable]
		public GreaterExplosionTrapDeed() : base( HouseTrapStrength.Greater, HouseTrapType.Explosion )
		{
		}

		public GreaterExplosionTrapDeed( Serial serial ) : base( serial )
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

	public class DeadlyExplosionTrapDeed : HouseTrapDeed
	{
		[Constructable]
		public DeadlyExplosionTrapDeed() : base( HouseTrapStrength.Deadly, HouseTrapType.Explosion )
		{
		}

		public DeadlyExplosionTrapDeed( Serial serial ) : base( serial )
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

	public class LesserFireColumnTrapDeed : HouseTrapDeed
	{
		[Constructable]
		public LesserFireColumnTrapDeed() : base( HouseTrapStrength.Lesser, HouseTrapType.FireColumn )
		{
		}

		public LesserFireColumnTrapDeed( Serial serial ) : base( serial )
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

	public class RegularFireColumnTrapDeed : HouseTrapDeed
	{
		[Constructable]
		public RegularFireColumnTrapDeed() : base( HouseTrapStrength.Regular, HouseTrapType.FireColumn )
		{
		}

		public RegularFireColumnTrapDeed( Serial serial ) : base( serial )
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

	public class GreaterFireColumnTrapDeed : HouseTrapDeed
	{
		[Constructable]
		public GreaterFireColumnTrapDeed() : base( HouseTrapStrength.Greater, HouseTrapType.FireColumn )
		{
		}

		public GreaterFireColumnTrapDeed( Serial serial ) : base( serial )
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

	public class DeadlyFireColumnTrapDeed : HouseTrapDeed
	{
		[Constructable]
		public DeadlyFireColumnTrapDeed() : base( HouseTrapStrength.Deadly, HouseTrapType.FireColumn )
		{
		}

		public DeadlyFireColumnTrapDeed( Serial serial ) : base( serial )
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

	public class LesserPoisonTrapDeed : HouseTrapDeed
	{
		[Constructable]
		public LesserPoisonTrapDeed() : base( HouseTrapStrength.Lesser, HouseTrapType.Poison )
		{
		}

		public LesserPoisonTrapDeed( Serial serial ) : base( serial )
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

	public class RegularPoisonTrapDeed : HouseTrapDeed
	{
		[Constructable]
		public RegularPoisonTrapDeed() : base( HouseTrapStrength.Regular, HouseTrapType.Poison )
		{
		}

		public RegularPoisonTrapDeed( Serial serial ) : base( serial )
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

	public class GreaterPoisonTrapDeed : HouseTrapDeed
	{
		[Constructable]
		public GreaterPoisonTrapDeed() : base( HouseTrapStrength.Greater, HouseTrapType.Poison )
		{
		}

		public GreaterPoisonTrapDeed( Serial serial ) : base( serial )
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

	public class DeadlyPoisonTrapDeed : HouseTrapDeed
	{
		[Constructable]
		public DeadlyPoisonTrapDeed() : base( HouseTrapStrength.Deadly, HouseTrapType.Poison )
		{
		}

		public DeadlyPoisonTrapDeed( Serial serial ) : base( serial )
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
