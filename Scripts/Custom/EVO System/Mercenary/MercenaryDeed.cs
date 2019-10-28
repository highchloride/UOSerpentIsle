#region AuthorHeader
//
//	EvoSystem version 2.1, by Xanthos
//
//
#endregion AuthorHeader
using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Xanthos.Interfaces;

namespace Xanthos.Evo
{
	public class MercenaryDeed : BaseEvoDeed
	{
		public override IEvoCreature GetEvoCreature()
		{
			return new Mercenary( "a sellsword" );
		}

		[Constructable]
		public MercenaryDeed() : base()
		{
			Name = "a sellsword contract";
		}

		public MercenaryDeed( Serial serial ) : base ( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int)0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}
