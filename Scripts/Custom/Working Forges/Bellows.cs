 using System;
 using Server;
 using Server.Network;
 using Server.Items;
 using System.Collections;
 
namespace Server.Items
{

 public class Bellows1 : AddonComponent
    {
        [Constructable]
        public Bellows1()
            : base(0x197A)
        {
            Name = "Bellows";
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.SendMessage(89, "As you stoke the coals the heat intensifies.");
            Effects.SendLocationEffect(new Point3D(X + 1, Y, Z + 5), Map, 0x3735, 13);
            Effects.PlaySound(from.Location, from.Map, 0x2B);  // Bellows
            return;
            
        }

     public Bellows1(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // Version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

 public class Bellows2 : AddonComponent
 {
     [Constructable]
     public Bellows2()
         : base(0x199E)
     {
         Name = "Bellows";
     }

     public override void OnDoubleClick(Mobile from)
     {
         from.SendMessage(89, "As you stoke the coals the heat intensifies.");
         Effects.SendLocationEffect(new Point3D(X - 1, Y, Z + 5), Map, 0x3735, 13);
         Effects.PlaySound(from.Location, from.Map, 0x2B);  // Bellows
         return;

     }

     public Bellows2(Serial serial)
         : base(serial)
     {
     }

     public override void Serialize(GenericWriter writer)
     {
         base.Serialize(writer);
         writer.Write(0); // Version
     }

     public override void Deserialize(GenericReader reader)
     {
         base.Deserialize(reader);
         int version = reader.ReadInt();
     }
 }

 public class Bellows3 : AddonComponent
 {
     [Constructable]
     public Bellows3()
         : base(0x1992)
     {
         Name = "Bellows";
     }

     public override void OnDoubleClick(Mobile from)
     {
         from.SendMessage(89, "As you stoke the coals the heat intensifies.");
         Effects.SendLocationEffect(new Point3D(X, Y - 1, Z + 5), Map, 0x3735, 13);
         Effects.PlaySound(from.Location, from.Map, 0x2B);  // Bellows
         return;

     }

     public Bellows3(Serial serial)
         : base(serial)
     {
     }

     public override void Serialize(GenericWriter writer)
     {
         base.Serialize(writer);
         writer.Write(0); // Version
     }

     public override void Deserialize(GenericReader reader)
     {
         base.Deserialize(reader);
         int version = reader.ReadInt();
     }
 }

 public class Bellows4 : AddonComponent
 {
     [Constructable]
     public Bellows4()
         : base(0x1986)
     {
         Name = "Bellows";
     }

     public override void OnDoubleClick(Mobile from)
     {
         from.SendMessage(89, "As you stoke the coals the heat intensifies.");
         Effects.SendLocationEffect(new Point3D(X, Y + 1, Z + 5), Map, 0x3735, 13);
         Effects.PlaySound(from.Location, from.Map, 0x2B);  // Bellows
         return;

     }

     public Bellows4(Serial serial)
         : base(serial)
     {
     }

     public override void Serialize(GenericWriter writer)
     {
         base.Serialize(writer);
         writer.Write(0); // Version
     }

     public override void Deserialize(GenericReader reader)
     {
         base.Deserialize(reader);
         int version = reader.ReadInt();
     }
 }
    }