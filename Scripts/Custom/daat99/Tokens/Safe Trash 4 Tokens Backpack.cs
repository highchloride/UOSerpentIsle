/*
 created by:
     /\            888                   888     .d8888b.   .d8888b.  
____/_ \____       888                   888    d88P  Y88b d88P  Y88b 
\  ___\ \  /       888                   888    888    888 888    888 
 \/ /  \/ /    .d88888  8888b.   8888b.  888888 Y88b. d888 Y88b. d888 
 / /\__/_/\   d88" 888     "88b     "88b 888     "Y888P888  "Y888P888 
/__\ \_____\  888  888 .d888888 .d888888 888           888        888 
    \  /      Y88b 888 888  888 888  888 Y88b.  Y88b  d88P Y88b  d88P 
     \/        "Y88888 "Y888888 "Y888888  "Y888  "Y8888P"   "Y8888P"  
*/
/// Trash 4 Tokens Backpack v0.1
///created by Daat99 26/03/2005
using System;
using System.Collections.Generic;
using Server.ContextMenus;

namespace Server.Items
{
	public class Trash4TokensBackpack : Container
	{
		public override int MaxWeight{ get{ return 0; } } // A value of 0 signals unlimited weight
		public override bool DisplaysContent{ get{ return false; } }
		public override int DefaultGumpID{ get{ return 0x3C; } }
		public override int DefaultDropSound{ get{ return 0x50; } }
		private bool cleaningBag = false;

		private DateTime m_NextTrash;
		public DateTime NextTrash{ get{ return m_NextTrash; } set{ m_NextTrash = value; } }


		public override Rectangle2D Bounds
		{
			get{ return new Rectangle2D( 18, 105, 144, 73 ); }
		}

		[Constructable]
		public Trash4TokensBackpack() : base( 0x9b2 )
		{
			Name = "A Safe Trash 4 Tokens Backpack"; 
			Movable = true;
			Hue = 1173;
			LootType = LootType.Blessed;
			MaxItems = 500;
		}

        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            if (dropped is Daat99Tokens || dropped is TokenCheck)
            {
                from.SendMessage("It would not be wise to trash tokens for tokens!");
                return false;
            }
            if (dropped is Gold)
            {
                from.SendMessage("You would be wiser to Tithe your Gold, rather than trash it!");
                return false;
            }
            if (dropped.LootType == LootType.Blessed)
            {
                from.SendMessage("It would be a good idea not to trash Blessed items!");
                return false;
            }
            List<Item> items = this.Items;
            if (items.Count > 0 && NextTrash <= DateTime.UtcNow)
            {
                from.SendMessage("3 minutes safety was over clearing trash before adding more");
                Empty(from);
            }
            if (!base.OnDragDrop(from, dropped))
                return false;
            NextTrash = (DateTime.UtcNow + TimeSpan.FromMinutes(3));
            return true;
        }

        public override bool OnDragDropInto(Mobile from, Item item, Point3D p)
        {
            if (item is Daat99Tokens || item is TokenCheck)
            {
                from.SendMessage("It would not be wise to trash tokens for tokens!");
                return false;
            }
            if (item is Gold)
            {
                from.SendMessage("You would be wiser to Tithe your Gold, rather than trash it!");
                return false;
            }
            if (item.LootType == LootType.Blessed)
            {
                from.SendMessage("It would be a good idea not to trash Blessed items!");
                return false;
            }
            List<Item> items = this.Items;
            if (items.Count > 0 && NextTrash <= DateTime.UtcNow)
            {
                from.SendMessage("3 minutes safety was over clearing trash before adding more");
                Empty(from);
            }
            if (!base.OnDragDropInto(from, item, p))
                return false;
            NextTrash = (DateTime.UtcNow + TimeSpan.FromMinutes(3));
            return true;
        }

		public override void OnDoubleClick( Mobile from )
		{
			List<Item> items = this.Items;
			if ( items.Count > 0 && NextTrash <= DateTime.UtcNow)
			{
				from.SendMessage("The 3 minutes safety was over, you can not recover the items.");
				Empty(from);
			}
			base.OnDoubleClick(from);
		}

		public override void OnItemRemoved( Item item ) 
		{
			if (cleaningBag)
				return;
			if (NextTrash <= DateTime.UtcNow)
			{
				item.Delete();
				Empty();
			}
			else 
				base.OnItemRemoved( item );
		} 

		public void Empty()
		{
			List<Item> items = this.Items;
			if ( items.Count > 0 )
			{
				Mobile from = RootParent as Mobile;
				if (from != null)
				{
					from.SendMessage( "You passed the 3 minutes safety, you can't recover the items." );
					Empty(from);
				}
				else
				{
					for ( int i = items.Count - 1; i >= 0; --i )
					{
						if ( i >= items.Count )
							continue;
						((Item)items[i]).Delete();
					}
				}
			}
		}

		public void Empty(Mobile from)
		{
			EmptyTrash(from, this);
		}

		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list )
		{
			base.GetContextMenuEntries( from, list );
			List<Item> items = this.Items;
			if ( items.Count > 0 )
				list.Add( new EmptyTrash4TokensBackpack( from, this ) );
		}

		public Trash4TokensBackpack( Serial serial ) : base( serial )
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
			NextTrash = DateTime.UtcNow;
		}

		public static void EmptyTrash(Mobile from, Trash4TokensBackpack backpack)
		{
			if (backpack == null)
				return;
			backpack.cleaningBag = true;
			List<Item> items = backpack.Items;
			int i_Reward = 0;
			if (items.Count > 0)
			{
				from.PlaySound(0x76);
				for ( int i = items.Count - 1; i >= 0; --i )
				{
					if ( i >= items.Count )
						continue;
					Item it = (Item)items[i] as Item;
					if ( it.Stackable == false && !((Item)items[i] is BaseBook) )
						i_Reward += Utility.RandomMinMax(2,5);
					((Item)items[i]).Delete();
				}
			}
			if (i_Reward > 0 && TokenSystem.GiveTokensToPlayer(from as Server.Mobiles.PlayerMobile, i_Reward))
				from.SendMessage(1173, "You were rewarded {0} Tokens to your account for cleaning the shard.", i_Reward);
			backpack.cleaningBag = false;
		}

		public class EmptyTrash4TokensBackpack : ContextMenuEntry
		{
			private Mobile m_From;
			private Trash4TokensBackpack m_Backpack;

			public EmptyTrash4TokensBackpack( Mobile from, Item item ) : base( 0154, 5 )
			{
				m_From = from;
				m_Backpack = item as Trash4TokensBackpack;
			}

			public override void OnClick()
			{
				EmptyTrash(m_From, m_Backpack);
			}
		}
		
		public override int GetTotal( TotalType type )
		{
			return ( type == TotalType.Weight ? 0 : base.GetTotal(type) );
		}
		
		public override void UpdateTotal( Item sender, TotalType type, int delta )
		{
			base.UpdateTotal( sender, type, (type == TotalType.Weight ? 0 : delta) );
		}
		
		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );
			list.Add( "Contents: {0}/{1} Items", TotalItems, MaxItems );
		}
	}
}