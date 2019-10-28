//===============================================================================
//                      This script was created by Gizmo's UoDevPro
//                      This script was created on 9/10/2019 3:49:11 PM
//===============================================================================


using System;
using Server;
using Server.Items;
using System.Collections.Generic;
using Server.SerpentIsle.Items.Corpses;

namespace Server.Mobiles
{
	[CorpseName( "a Totem Bear coprse" )]
	public class TotemBear : GrizzlyBear
	{
		[Constructable]
		public TotemBear() : base()
		{
			Name = "Totem Bear";
			Hue = 0x562;
			SetStr( 110 );
			SetDex( 80 );
			SetInt( 60 );

			//Skills
			SetSkill( SkillName.Anatomy, 50.0, 100.0 );
			SetSkill( SkillName.Tactics, 50.0, 100.0 );
			SetSkill( SkillName.Wrestling, 50.0, 100.0 );

			Fame = 0;
			Karma = 0;
		}

        public TotemBear(Serial serial) : base(serial)
        { }

        //Generate Special Loot
        public override void OnDeath( Container c )
		{
			base.OnDeath(c);
            c.DropItem(new TotemBearCarcass());
        }

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0); // version
		}
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}
