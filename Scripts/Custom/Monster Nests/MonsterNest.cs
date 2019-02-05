using System;
using System.Collections;
using System.IO;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
	public class MonsterNest : Item
	{
		private string m_NestSpawnType;
		private int m_MaxCount;
		private TimeSpan m_RespawnTime;
		private ArrayList m_Spawn;
		private int m_HitsMax;
		private int m_Hits;
		private int m_RangeHome;
		private DateTime m_Attackable;
		private int m_LootLevel;
		private Mobile m_Entity;

		[CommandProperty( AccessLevel.GameMaster )]
		public string NestSpawnType
		{
			get{ return m_NestSpawnType; }
			set{ m_NestSpawnType = value; }
		}
		[CommandProperty( AccessLevel.GameMaster )]
		public int MaxCount
		{
			get{ return m_MaxCount; }
			set{ m_MaxCount = value; }
		}
		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan RespawnTime
		{
			get{ return m_RespawnTime; }
			set{ m_RespawnTime = value; }
		}
		[CommandProperty( AccessLevel.GameMaster )]
		public int HitsMax
		{
			get{ return m_HitsMax; }
			set{ m_HitsMax = value; }
		}
		[CommandProperty( AccessLevel.GameMaster )]
		public int Hits
		{
			get{ return m_Hits; }
			set{ m_Hits = value; }
		}
		[CommandProperty( AccessLevel.GameMaster )]
		public int RangeHome
		{
			get{ return m_RangeHome; }
			set{ m_RangeHome = value; }
		}
		[CommandProperty( AccessLevel.GameMaster )]
		public int LootLevel
		{
			get{ return m_LootLevel; }
			set{ m_LootLevel = value; }
		}

		[Constructable]
		public MonsterNest() : base( 4962 )
		{
			m_NestSpawnType = null;
			Name = "Monster Nest";
			m_Spawn = new ArrayList();
			Weight = 0.1;
			Hue = 1818;
			HitsMax = 300;
			Hits = 300;
			RangeHome = 10;
			RespawnTime = TimeSpan.FromSeconds( 15.0 );
			new InternalTimer( this ).Start();
			new RegenTimer( this ).Start();
			Movable = false;
			m_Entity = new MonsterNestEntity( this );
			m_Entity.MoveToWorld( this.Location, this.Map );
		}

		public override void OnDelete()
		{
			base.OnDelete();

			if ( m_Spawn != null && m_Spawn.Count > 0 )
			{
				for( int i = 0; i < this.m_Spawn.Count; i++ )
				{
					Mobile m = (Mobile)this.m_Spawn[i];
					m.Delete();
				}
			}
			if ( this.m_Entity != null )
				this.m_Entity.Delete();
		}

		public void Damage( int damage )
		{
			this.Hits -= damage;
			this.PublicOverheadMessage( MessageType.Regular, 0x22, false, damage.ToString() );
			if ( this.Hits <= 0 )
				this.Destroy();
		}

		public override void OnDoubleClick( Mobile from )
		{
			from.SendMessage( 0, "You begin to attack the object." );
			if ( this.m_Entity != null )
				from.Combatant = this.m_Entity;
		}

		public virtual void AddLoot()
		{
			MonsterNestLoot loot = new MonsterNestLoot( 6585, this.Hue, this.m_LootLevel, "Monster Nest remains" );
			loot.MoveToWorld( this.Location, this.Map );
		}

		public void Destroy()
		{
			AddLoot();
			if ( m_Spawn != null && m_Spawn.Count > 0 )
			{
				for( int i = 0; i < this.m_Spawn.Count; i++ )
				{
					Mobile m = (Mobile)this.m_Spawn[i];
					m.Kill();
				}
			}
			if ( this.m_Entity != null )
				this.m_Entity.Delete();
			this.Delete();
		}

		public int Count()
		{
			int c = 0;
			if ( this.m_Spawn != null && this.m_Spawn.Count > 0 )
			{
				for( int i = 0; i < this.m_Spawn.Count; i++ )
				{
					Mobile m = (Mobile)this.m_Spawn[i];
					if ( m.Alive )
						c += 1;
				}
			}
			return c;
		}

		public void DoSpawn()
		{
			if ( this.m_Entity != null )
				this.m_Entity.MoveToWorld( this.Location, this.Map );
			if ( this.NestSpawnType != null && this.m_Spawn != null && this.Count() < this.m_MaxCount )
			{

				try
				{
					Type type = SpawnerType.GetType( this.NestSpawnType );
					object o = Activator.CreateInstance( type );
					if ( o != null && o is Mobile )
					{
						Mobile c = o as Mobile;
						if ( c is BaseCreature )
						{
							BaseCreature m = o as BaseCreature;
							this.m_Spawn.Add( m );
							m.OnBeforeSpawn( this.Location, this.Map );
							m.MoveToWorld( this.Location, this.Map );
							m.Home = this.Location;
							m.RangeHome = this.m_RangeHome;
						}
					}
				}
				catch
				{
					this.NestSpawnType = null;
				}
			}
		}			

		public MonsterNest( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
			writer.Write( (string) m_NestSpawnType );
			writer.WriteMobileList( m_Spawn );
			writer.Write( (int) m_MaxCount );
			writer.Write( (TimeSpan) m_RespawnTime );
			writer.Write( (int) m_HitsMax );
			writer.Write( (int) m_Hits );
			writer.Write( (int) m_RangeHome );
			writer.Write( (int) m_LootLevel );
			writer.Write( (Mobile) m_Entity );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			m_NestSpawnType = reader.ReadString();
			m_Spawn = reader.ReadMobileList();
			m_MaxCount = reader.ReadInt();
			m_RespawnTime = reader.ReadTimeSpan();
			m_HitsMax = reader.ReadInt();
			m_Hits = reader.ReadInt();
			m_RangeHome = reader.ReadInt();
			m_LootLevel = reader.ReadInt();
			m_Entity = reader.ReadMobile();
		}

		private class RegenTimer : Timer
		{
			private MonsterNest nest;
			public RegenTimer( MonsterNest n ) : base( TimeSpan.FromMinutes( 1.0 ) )
			{
				nest= n;
			}
			protected override void OnTick()
			{
				if ( nest != null && !nest.Deleted )
				{
					nest.Hits += nest.HitsMax / 10;
					if ( nest.Hits > nest.HitsMax )
						nest.Hits = nest.HitsMax;
					new RegenTimer( nest ).Start();
				}
			}
		}

		private class InternalTimer : Timer
		{
			private MonsterNest nest;
			public InternalTimer( MonsterNest n ) : base( n.RespawnTime )
			{
				nest= n;
			}
			protected override void OnTick()
			{
				if ( nest != null && !nest.Deleted )
				{
					nest.DoSpawn();
					new InternalTimer( nest ).Start();
				}
			}
		}
	}

	public class MonsterNestEntity : BaseCreature
	{
		private Item m_MonsterNest;

		[Constructable]
		public MonsterNestEntity( MonsterNest nest ) : base( AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			m_MonsterNest = nest;
			Name = nest.Name;
			Title = "";
			Body = 399;
			BaseSoundID = 0;
			this.Hue = 0;

			SetStr( 0 );
			SetDex( 0 );
			SetInt( 0 );

			SetHits( nest.HitsMax );

			SetDamage( 0, 0 );

			SetDamageType( ResistanceType.Physical, 0 );

			SetResistance( ResistanceType.Physical, 0 );
			SetResistance( ResistanceType.Fire, 0 );
			SetResistance( ResistanceType.Cold, 0 );
			SetResistance( ResistanceType.Poison, 0 );
			SetResistance( ResistanceType.Energy, 0 );

			Fame = 5000;
			Karma = -5000;

			VirtualArmor = 0;
			CantWalk = true;
		}

		public override void OnThink()
		{
			this.Frozen = true;
			this.Location = this.m_MonsterNest.Location;
			if ( this.m_MonsterNest != null && this.m_MonsterNest is MonsterNest )
			{
				MonsterNest nest = this.m_MonsterNest as MonsterNest;
				this.Hits = nest.Hits;
			}
		}

		public override void OnDamage( int amount, Mobile from, bool willkill )
		{
			if ( this.m_MonsterNest != null && this.m_MonsterNest is MonsterNest )
			{
				MonsterNest nest = this.m_MonsterNest as MonsterNest;
				nest.Damage( amount );
			}
			base.OnDamage( amount, from, willkill );
		}

		public override int GetAngerSound()
		{
			return 9999;
		}

		public override int GetIdleSound()
		{
			return 9999;
		}

		public override int GetAttackSound()
		{
			return 9999;
		}

		public override int GetHurtSound()
		{
			return 9999;
		}

		public override int GetDeathSound()
		{
			return 9999;
		}

		public override bool OnBeforeDeath()
		{
			return false;
		}

		public MonsterNestEntity( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			
			writer.Write( (int) 0 ); // version
			writer.Write( (Item) m_MonsterNest );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			
			int version = reader.ReadInt();
			m_MonsterNest = reader.ReadItem();
		}
	}


}