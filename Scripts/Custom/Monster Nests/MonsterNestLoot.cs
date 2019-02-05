using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
	public class MonsterNestLoot : Item
	{
		private int m_LootLevel;
		[Constructable]
		public MonsterNestLoot(int itemid, int hue, int lootlevel, string name) : base()
		{
			Name = name + " (double click to loot)".ToString();
			Hue = hue;
			ItemID = itemid;
			Movable = false;
			m_LootLevel = lootlevel;
		}

		public override void OnDoubleClick( Mobile from )
		{
			ArrayList alist = new ArrayList();
			IPooledEnumerable eable = this.Map.GetMobilesInRange( this.Location, 20 );
			foreach( Mobile m in eable )
				alist.Add( m );
			eable.Free();
			if ( alist.Count > 0 )
			{
				for( int i = 0; i < alist.Count; i++ )
				{
					Mobile m = (Mobile)alist[i];
					if ( m is PlayerMobile )
						AddLoot( m );
				}
			}

			this.Delete();
		}

		public void AddLoot( Mobile m )
		{
			int chance = Utility.Random( 5, 20 ) * m_LootLevel;
			if ( chance < 10 )
				m.AddToBackpack( new Gold( Utility.Random( 500, 1000 )));
			else if ( chance < 20 )
				m.AddToBackpack( new Gold( Utility.Random( 1000, 2000 )));
			else if ( chance < 30 )
			{
				m.AddToBackpack( new BankCheck( Utility.Random( 2000, 3000 )));
			}
			else if ( chance < 40 )
			{
				m.AddToBackpack( new BankCheck( Utility.Random( 3000, 4000 )));
			}
			else if ( chance < 50 )
			{
				m.AddToBackpack( new BankCheck( Utility.Random( 4000, 5000 )));
			}
			else if ( chance < 60 )
			{
				m.AddToBackpack( new BankCheck( Utility.Random( 5000, 6000 )));
			}
			else if ( chance < 70 )
			{
				m.AddToBackpack( new BankCheck( Utility.Random( 6000, 7000 )));
			}
			else if ( chance < 80 )
			{
				m.AddToBackpack( new BankCheck( Utility.Random( 7000, 8000 )));
			}
			else if ( chance < 90 )
			{
				m.AddToBackpack( new BankCheck( Utility.Random( 8000, 9000 )));
			}
			else
			{
				m.AddToBackpack( new BankCheck( Utility.Random( 7000, 10000 )));
			}
		}

		public MonsterNestLoot( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
			writer.Write( (int) m_LootLevel );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			m_LootLevel = reader.ReadInt();
		}

		private class RegenTimer : Timer
		{
			private MonsterNest nest;
			public RegenTimer( MonsterNest n ) : base( TimeSpan.FromSeconds( 30.0 ))
			{
				nest = n;
			}
			protected override void OnTick()
			{
				if ( nest != null && !nest.Deleted )
				{
					if ( nest.Hits < 0 )
					{
						nest.Destroy();
						return;
					}
					nest.Hits += nest.HitsMax / 10;
					if ( nest.Hits > nest.HitsMax )
						nest.Hits = nest.HitsMax;
					new RegenTimer( nest ).Start();
				}
			}
		}

		private class SpawnTimer : Timer
		{
			private MonsterNest nest;
			public SpawnTimer( MonsterNest n ) : base( n.RespawnTime )
			{
				nest= n;
			}
			protected override void OnTick()
			{
				if ( nest != null && !nest.Deleted )
				{
					nest.DoSpawn();
					new SpawnTimer( nest ).Start();
				}
			}
		}
	}
}