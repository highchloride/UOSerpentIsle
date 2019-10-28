//===============================================================================
//                      This script was created by Gizmo's UoDevPro
//                      This script was created on 9/10/2019 3:31:56 PM
//===============================================================================


using System;
using Server;
using Server.Items;
using System.Collections.Generic;
using Server.SerpentIsle.Items.Corpses;

namespace Server.Mobiles
{
	[CorpseName( "a Totem Wolf coprse" )]
	public class TotemWolf : DireWolf
	{
		[Constructable]
		public TotemWolf() : base()
		{
			Name = "Totem Wolf";
			Hue = 0x157;
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

        public TotemWolf(Serial serial) : base(serial)
        { }

        //Generate Special Loot
        public override void OnDeath( Container c )
		{
			base.OnDeath(c);
			c.DropItem( new TotemWolfCarcass() );
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
