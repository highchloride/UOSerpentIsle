using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.ACC.CSS.Systems.Druid
{
	[CorpseName( "a dryad corpse" )]
	public class DryadFamiliar : BaseCreature
	{
		[Constructable]
		public DryadFamiliar () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a dryad";
			Body = 401;
			Hue = 33770;
			BaseSoundID = 0x4B0;

			SetStr( 200 );
			SetDex( 200 );
			SetInt( 100 );

			SetHits( 175 );
			SetStam( 50 );

			SetDamage( 6, 9 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Energy, 50 );

			SetResistance( ResistanceType.Physical, 40, 50 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 35, 45 );
			SetResistance( ResistanceType.Poison, 50, 60 );
			SetResistance( ResistanceType.Energy, 70, 80 );

			SetSkill( SkillName.Meditation, 110.0 );
			SetSkill( SkillName.EvalInt, 110.0 );
			SetSkill( SkillName.Magery, 110.0 );
			SetSkill( SkillName.MagicResist, 110.0 );
			SetSkill( SkillName.Tactics, 110.0 );
			SetSkill( SkillName.Wrestling, 110.0 );

			VirtualArmor = 45;
			ControlSlots = 2;

			Item hair = new Item( Utility.RandomList( 0x203B, 0x203C, 0x203D, 0x2044, 0x2045, 0x2047, 0x2049, 0x204A ) );
			hair.Hue = Utility.RandomHairHue();
			hair.Layer = Layer.Hair;
			hair.Movable = false;
			AddItem( hair );

			Item sash = new BodySash();
			sash.Hue = Utility.RandomList( 1165, 1166, 1167, 1168, 1169, 1170, 1171, 1172 );
			sash.Movable = false;
			AddItem( sash );

			Item shoes = new Sandals();
			shoes.Hue = Utility.RandomList( 1165, 1166, 1167, 1168, 1169, 1170, 1171, 1172 );
			shoes.Movable = false;
			AddItem( shoes );

			Item skirt = new LeatherSkirt();
			skirt.Hue = Utility.RandomList( 1165, 1166, 1167, 1168, 1169, 1170, 1171, 1172 );
			skirt.Movable = false;
			AddItem( skirt );

			Item garland = new FlowerGarland();
			garland.Hue = Utility.RandomList( 1165, 1166, 1167, 1168, 1169, 1170, 1171, 1172 );
			garland.Movable = false;
			AddItem( garland );
		}

		public DryadFamiliar( Serial serial ) : base( serial )
		{
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
