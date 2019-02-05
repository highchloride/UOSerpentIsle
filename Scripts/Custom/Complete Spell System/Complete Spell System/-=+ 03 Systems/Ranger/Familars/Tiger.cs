using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.ACC.CSS.Systems.Ranger
{
	[CorpseName( "a saber tooth tiger corpse" )]
	public class TigerFamiliar : BaseFamiliar
	{
		public TigerFamiliar()
		{
			Name = "a saber tooth tiger";
			Body = 251;
			Hue = 2213;
			BaseSoundID = 229;

			SetStr( 120, 135 );
			SetDex( 110 );
			SetInt( 60 );

			SetHits( 100, 120 );
			SetStam( 70 );
			SetMana( 0 );

			SetDamage( 20, 35 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 50 );
			SetResistance( ResistanceType.Fire, 50 );
			SetResistance( ResistanceType.Cold, 50 );
			SetResistance( ResistanceType.Poison, 50 );
			SetResistance( ResistanceType.Energy, 50 );

			SetSkill( SkillName.Wrestling, 100.0 );
			SetSkill( SkillName.Tactics, 100.0 );

			ControlSlots = 1;

		}


		public TigerFamiliar( Serial serial ) : base( serial )
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
