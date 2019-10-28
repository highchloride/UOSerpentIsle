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
using Server;
using Server.Mobiles;
using Server.ContextMenus;

namespace daat99
{

	public class MasterStorageStorageContextMenu : ContextMenuEntry
	{
		private PlayerMobile player;
		private MasterStorage backpack;
		public MasterStorageStorageContextMenu(Mobile from, Item item) : base(155, 1) //155=My Inventory
		{
			player = from as PlayerMobile;
			backpack = item as MasterStorage;
		}
		public override void OnClick()
		{
			if (backpack == null || !backpack.IsOwner(player))
				return;
			MasterStorageStorageGump.SendGump(player, backpack, 0);
		}
	}

	public class MasterStorageLedgerContextMenu : ContextMenuEntry
	{
		private PlayerMobile player;
		private MasterStorage backpack;
		public MasterStorageLedgerContextMenu( Mobile from, Item item ) : base( 123, 1 ) //123=Credits
		{
			player = from as PlayerMobile;
			backpack = item as MasterStorage;
		}
		public override void OnClick()
		{
			if ( backpack == null || !backpack.IsOwner(player)  )
				return;
			MasterStorageLedgerGump.SendGump(player, backpack);
		}
	}

	public class MasterStorageSetupContextMenu : ContextMenuEntry
	{
		private PlayerMobile player;
		private MasterStorage backpack;
		public MasterStorageSetupContextMenu( Mobile from, Item item ) : base( 127, 2 ) //97=Options
		{
			player = from as PlayerMobile;
			backpack = item as MasterStorage;
		}
		public override void OnClick()
		{
			if ( backpack == null || !backpack.IsOwner(player)  )
				return;
			MasterStorageSetupGump.SendGump(player, backpack, 0);
		}
	}

	public class MasterStorageFillContextMenu : ContextMenuEntry
	{
		private PlayerMobile player;
		private MasterStorage backpack;
		public MasterStorageFillContextMenu( Mobile from, Item item ) : base( 6230, 3 ) //6230=refill from stock
		{
			player = from as PlayerMobile;
			backpack = item as MasterStorage;
		}
		public override void OnClick()
		{
			if ( backpack == null || !backpack.IsOwner(player)  )
				return;
			backpack.RefillStorage(player);
		}
	}
}

