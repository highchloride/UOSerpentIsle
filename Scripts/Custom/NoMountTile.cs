using System; 
using Server; 
using Server.Items;
using Server.Gumps; 
using Server.Network; 
using Server.Menus; 
using Server.Mobiles;
using Server.Menus.Questions;
using Xanthos.ShrinkSystem;

namespace Server.Items
{ 
	public class NoMountTile : Item 
	{ 

		[Constructable] 
		public NoMountTile() : base( 6108 ) 
		{ 
			Movable = false; 
			Name = "No Mount Tile"; 
			Visible = false;
		} 

		public override bool OnMoveOver( Mobile from )
		{
            if (from.AccessLevel == AccessLevel.Player)
            {
                if (from.Mounted)
                {
                    from.SendMessage("Thou must dismount to enter.");
                    return false;
                }
                else if (from.Followers > 0)
                {
                    foreach (Mobile follower in (from as PlayerMobile).AllFollowers)
                    {
                        if (follower is Mercenary)
                            continue;
                        else
                        {
                            from.SendMessage("Pets cannot join thee inside.");
                            return false;
                        }

                    }
                }
                else
                {
                    return base.OnMoveOver(from);
                }
            }
            return base.OnMoveOver( from );
		}
		public NoMountTile( Serial serial ) : base( serial ) 
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
