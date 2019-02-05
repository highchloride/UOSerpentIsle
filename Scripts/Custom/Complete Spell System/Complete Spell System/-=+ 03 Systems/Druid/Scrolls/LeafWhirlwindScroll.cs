using System;
using Server;
using Server.Items;

namespace Server.ACC.CSS.Systems.Druid
{
   public class DruidLeafWhirlwindScroll : CSpellScroll
   {
      [Constructable]
      public DruidLeafWhirlwindScroll() : this( 1 )
      {
      }

      [Constructable]
      public DruidLeafWhirlwindScroll( int amount ) : base( typeof( DruidLeafWhirlwindSpell ), 0xE39 )
      {
         Name = "Leaf Whirlwind";
         Hue = 0x58B;
      }

      public DruidLeafWhirlwindScroll( Serial serial ) : base( serial )
      {
      		ItemID=0xE39;
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

