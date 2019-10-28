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

namespace daat99
{ 
   	public class MasterStorageGoldLedgerDeed : Item 
   	{ 
      	[Constructable] 
      	public MasterStorageGoldLedgerDeed() : base( 0x14F0 ) 
      	{ 
			Weight = 1.0;  
        	Movable = true;
        	Name="Master Storage Gold Ledger deed";   
      	}

      	public MasterStorageGoldLedgerDeed( Serial serial ) : base( serial ) {  } 
		
		public override void OnDoubleClick( Mobile from ) 
      	{
			if ( !IsChildOf( from.Backpack ) )
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			else
			{
				MasterStorage backpack = MasterStorageUtils.GetMasterStorage(from as PlayerMobile);
				if ( backpack == null )
					from.SendMessage("You must have your Master Storage in your backpack!");
				else if ( backpack.GoldLedger )
					from.SendMessage("You already have gold ledger enabled on your Master Storage backpack.");
				else if ( !this.Deleted && !backpack.Deleted )
				{
					backpack.GoldLedger = true;
					this.Delete();
					from.SendMessage("You enabled the gold ledger on your Master Storage backpack.");
				}
			}
		}
		
		public override void Serialize( GenericWriter writer ) 
      	{ 
			base.Serialize( writer ); 

         	writer.Write( (int) 0 ); 
		} 

      	public override void Deserialize( GenericReader reader ) 
      	{ 
        	base.Deserialize( reader ); 
			int version = reader.ReadInt(); 
		}
   	} 
} 
