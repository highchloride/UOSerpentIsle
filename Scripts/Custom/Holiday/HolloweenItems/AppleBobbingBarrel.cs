using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Items
{
	public class AppleBobbinBarrel : Item
	{
		private Timer m_Timer;

		[Constructable]
		public AppleBobbinBarrel() : base(0xE7B)
		{
			Weight = 2.0;
			Name = "An Apple Bobbing Barrel";
		}

		public override bool OnMoveOver( Mobile m ) { return false; }

		public override void OnDoubleClick(Mobile from)
		{
			if (!from.InRange( GetWorldLocation(), 1 ) ) from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1019045 );
			else if( !from.CanBeginAction( typeof( AppleBobbinBarrel ))) from.SendMessage( "You already have your head under the water" );
			else
			{
				from.BeginAction( typeof( AppleBobbinBarrel ));
				Timer.DelayCall( TimeSpan.FromSeconds( 6.0 ), new TimerStateCallback( EndBobbing ), from );
				from.SendMessage("You dunk your head in the water trying frantically to sink your teeth into an apple!");
				from.CantWalk = true;
				from.Direction = from.GetDirectionTo(this);
				from.Animate(32, 5, 1, true, true, 0);
				from.PlaySound(37);
			}
		}

		public static void EndBobbing( object state )
		{
			Mobile m = (Mobile)state;
			PlayerMobile pm = m as PlayerMobile;
			pm.CantWalk = false;
			if (pm != null && Utility.RandomDouble() <= .30)
			{
				switch(Utility.Random(8))
				{
					case 0: default: pm.AddToBackpack(new AppleTiny()); break;
					case 1: pm.AddToBackpack(new AppleSmall()); break;
					case 2: pm.AddToBackpack(new AppleMediumSmall()); break;
					case 3: pm.AddToBackpack(new AppleMedium()); break;
					case 4: pm.AddToBackpack(new AppleMediumLarge()); break;
					case 5: pm.AddToBackpack(new AppleLarge()); break;
					case 6: pm.AddToBackpack(new AppleExtraLarge()); break;
					case 7: pm.AddToBackpack(new AppleGigantic()); break;
				}
				pm.SendMessage("You bite into an apple and pull your soaking wet head out of the water!");
				pm.PublicOverheadMessage(MessageType.Regular, 0xFE, false, "*" + pm.Name + " victoriously pulls an apple from the barrel using only their teeth!*");
			}
			else if (pm != null)
			{
				pm.SendMessage("You fail to bite into any of the apples in the barrel...");
				pm.PublicOverheadMessage(MessageType.Regular, 0xFE, false, "*" + pm.Name + " is soaking wet without an apple to show for it...*");
			}
			m.EndAction( typeof( AppleBobbinBarrel ));
		}

		public AppleBobbinBarrel(Serial serial) : base(serial) { }
		public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
		public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
	}

	public class AppleTiny : Apple
	{
		[Constructable] public AppleTiny() : this( 1 ){}
		[Constructable] public AppleTiny( int amount ) : base(amount) { Name = "Tiny Apple"; }
		public AppleTiny( Serial serial ) : base( serial ) { }
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 );}
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt();}
	}
	public class AppleSmall : Apple
	{
		[Constructable] public AppleSmall() : this( 1 ){}
		[Constructable] public AppleSmall( int amount ) : base(amount) { Name = "Small Apple"; }
		public AppleSmall( Serial serial ) : base( serial ) { }
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 );}
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt();}
	}
	public class AppleMediumSmall : Apple
	{
		[Constructable] public AppleMediumSmall() : this( 1 ){}
		[Constructable] public AppleMediumSmall( int amount ) : base(amount) { Name = "Medium Small Apple"; }
		public AppleMediumSmall( Serial serial ) : base( serial ) { }
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 );}
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt();}
	}
	public class AppleMedium : Apple
	{
		[Constructable] public AppleMedium() : this( 1 ){}
		[Constructable] public AppleMedium( int amount ) : base(amount) { Name = "Medium Apple"; }
		public AppleMedium( Serial serial ) : base( serial ) { }
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 );}
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt();}
	}
	public class AppleMediumLarge : Apple
	{
		[Constructable] public AppleMediumLarge() : this( 1 ){}
		[Constructable] public AppleMediumLarge( int amount ) : base(amount) { Name = "Medium Large Apple"; }
		public AppleMediumLarge( Serial serial ) : base( serial ) { }
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 );}
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt();}
	}
	public class AppleLarge : Apple
	{
		[Constructable] public AppleLarge() : this( 1 ){}
		[Constructable] public AppleLarge( int amount ) : base(amount) { Name = "Large Apple"; }
		public AppleLarge( Serial serial ) : base( serial ) { }
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 );}
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt();}
	}
	public class AppleExtraLarge : Apple
	{
		[Constructable] public AppleExtraLarge() : this( 1 ){}
		[Constructable] public AppleExtraLarge( int amount ) : base(amount) { Name = "Extra Large Apple"; }
		public AppleExtraLarge( Serial serial ) : base( serial ) { }
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 );}
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt();}
	}
	public class AppleGigantic : Apple
	{
		[Constructable] public AppleGigantic() : this( 1 ){}
		[Constructable] public AppleGigantic( int amount ) : base(amount) { Name = "Gigantic Apple"; }
		public AppleGigantic( Serial serial ) : base( serial ) { }
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 );}
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt();}
	}
}
