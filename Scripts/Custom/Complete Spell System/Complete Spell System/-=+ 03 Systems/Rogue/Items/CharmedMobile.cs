using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Misc;
using Server.Mobiles;

namespace Server.ACC.CSS.Systems.Rogue
{
	[CorpseName( "a corpse" )]
	public class CharmedMobile : BaseCreature
	{
		private BaseCreature m_Owner;

		[CommandProperty( AccessLevel.GameMaster )]
		public BaseCreature Owner
		{
			get{ return m_Owner; }
			set{ m_Owner= value; }
		}

		[Constructable]
		public CharmedMobile( BaseCreature owner ) : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			m_Owner = owner;
			Body = 777;
			Title = " The Mystic Lama Herder";
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public override bool ClickTitle{ get{ return false; } }

		public CharmedMobile( Serial serial ) : base( serial )
		{
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			list.Add( 1042971,this.Name );
			list.Add( 1049644,"charmed" );
		}

		public override bool OnBeforeDeath()
		{
			if( m_Owner != null )
			{
				m_Owner.MoveToWorld( this.Location, this.Map );
				m_Owner.Blessed = false;
				m_Owner.RevealingAction();
			}

			Delete();
			return false;
		}

		public override void OnAfterDelete()
		{
			if( m_Owner != null )
			{
				m_Owner.MoveToWorld( this.Location, this.Map );
				m_Owner.Blessed = false;
				m_Owner.RevealingAction();
			}

			base.OnAfterDelete();
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
			writer.Write( m_Owner);
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			m_Owner = reader.ReadMobile() as BaseCreature;

			Delete();
		}
	}
}