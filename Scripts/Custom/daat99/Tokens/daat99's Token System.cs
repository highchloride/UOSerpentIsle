/*
 created by:
     /\            888                   888     .d8888b.   .d8888b.  
____/_ \____       888                   888    d88P  Y88b d88P  Y88b 
\  ___\ \  /       888                   888    888    888 888    888 
 \/ /  \/ /    .d88888  8888b.   8888b.  888888 Y88b. d888 Y88b. d888 
 / /\__/_/\   d88" 888     "88b     "88b 888     "Y888P888  "Y888P888 
/__\ \_____\  888  888 .d888888 .d888888 888           888        888 
    \  /      Y88b 888 888  888 888  888 Y88b.  Y88b  d88P Y88b  d88P 
     \/        "Y88888 "Y888888 "Y888888  "Y888  "Y8888P"   "Y8888P"  
*/
///daat99's Token System
///Made by daat99 based on idea by Viago :)
///Thanx for Murzin for all the grammer corrections :)
using Server.Mobiles;
using daat99;

namespace Server.Items
{
	public static class TokenSystem
	{
		public static bool GiveTokensToPlayer(PlayerMobile player, int amount)
		{
			return GiveTokensToPlayer(player, amount, true);
		}
		public static bool GiveTokensToPlayer(PlayerMobile player, int amount, bool informPlayer)
		{
			return MasterStorageUtils.GiveTypeToPlayer(player, typeof(Daat99Tokens), amount, informPlayer);
		}

		public static bool TakePlayerTokens(PlayerMobile player, int amount)
		{
			return TakePlayerTokens(player, amount, true);
		}
		public static bool TakePlayerTokens(PlayerMobile player, int amount, bool informPlayer)
		{
			return MasterStorageUtils.TakeTypeFromPlayer(player, typeof(Daat99Tokens), amount, informPlayer);
		}
	}
	public class TokenCheck : Item
	{
		private int i_TokensAmount;
		
		[CommandProperty(AccessLevel.Administrator)]
		public int tokensAmount { get { return i_TokensAmount; } set { i_TokensAmount = value; InvalidateProperties(); } }
		
		[Constructable]
		public TokenCheck() : this( 100 ) { }
		[Constructable]
		public TokenCheck(int tokensAmount) : this ( tokensAmount, false ) { }
		[Constructable]
		public TokenCheck( int tokensAmount, bool blessed ) : base( 5359 )
		{
			Stackable = false;
			Name = "A Token Check";
			Hue = 1173;
			Weight = 1.0;
			if ( blessed )
				LootType = LootType.Blessed;
			i_TokensAmount = tokensAmount;
		}
		
		public override void OnDoubleClick(Mobile from)
		{
			if ( this.IsChildOf( from.Backpack ) )
			{
				if (!(this.Deleted))
				{
					if ( TokenSystem.GiveTokensToPlayer(from as PlayerMobile, i_TokensAmount) )
					{
						from.SendMessage(1173, "You added {0} Tokens to your account using a token check.", i_TokensAmount);
						this.Delete();
					}
					else
						from.SendMessage(1173, "You were unable to add tokens to your account.");
				}
				else
				{
					from.PlaySound(1069); //play Hey!! sound
					from.SendMessage(1173, "Hey, don't try to rob the bank!!!");
				}
			}
			else
				from.SendMessage(1173, "The check must be in your backpack to be used.");
		}

		public override int LabelNumber{ get{ return 1041361; } } // A bank check

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties( list );

			list.Add( 1060738, i_TokensAmount.ToString() ); // value: ~1_val~
		}
		
		public TokenCheck( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( i_TokensAmount );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch (version)
			{
				case 0:
				{
					i_TokensAmount = reader.ReadInt();
					break;
				}
			}
		}
	}
	
	public class Daat99Tokens : Item
	{
		public Daat99Tokens( int min, int max ) : this( Utility.Random( min, max-min ) )
		{
		}

		[Constructable]
		public Daat99Tokens() : this( 1 )
		{
		}

		[Constructable]
		public Daat99Tokens( int amount ) : base( 0xEED )
		{
			Stackable = true;
			Name = "Tokens";
			Hue = 1173;
			Weight = 0;
			Amount = amount;
			LootType = LootType.Blessed;
		}
		
		public override void OnDoubleClick(Mobile from)
		{
			if ( this.IsChildOf(from.Backpack) && !this.Deleted )
			{
				if (TokenSystem.GiveTokensToPlayer(from as PlayerMobile, Amount))
				{
					from.SendMessage(1173, "You added {0} tokens to your account.", Amount);
					this.Delete();
				}
				else
				{
					from.PlaySound(1069); //play Hey!! sound
					from.SendMessage(1173, "Please make sure your Master Storage is inside your backpack with an active Tokens account.");
				}
			}
			else
			{
				from.PlaySound(1069); //play Hey!! sound
				from.SendMessage(1173, "Hey, don't try to rob the bank!!!");
			}
		}
		
		public Daat99Tokens( Serial serial ) : base( serial )
		{
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

	public class GiveTokens
	{ 
		public static void CalculateTokens(Mobile m, BaseCreature creature)
		{
			if (creature.IsBonded)
				return;

			double d_Resists = ((creature.PhysicalResistance + creature.FireResistance + creature.ColdResistance + creature.PoisonResistance + creature.EnergyResistance)/50); //set the amount of resists the monster have
			if (d_Resists < 1.0) //if it have less then total on 100 resists set to 1
				d_Resists = 1.0;
			int i_Hits = (creature.HitsMax/50); //set the amount of max hp the creature had.
			double d_TempTokReward = (i_Hits + ((i_Hits * d_Resists)/10) ); //set the temp reward
						
			int i_FameKarma = creature.Fame; //set fame\karma reward bonus
			if (creature.Karma < 0)
				i_FameKarma -= creature.Karma;
			else
				i_FameKarma += creature.Karma;
			i_FameKarma = (i_FameKarma/500);
			if (i_FameKarma < 0)
				i_FameKarma = 0;
			d_TempTokReward += i_FameKarma; //add the fame\karma reward to the temp reward

			if (creature.AI == AIType.AI_Mage) //if it's mage add some tokens, it have spells
			{
				double d_Mage = ((creature.Skills[SkillName.Meditation].Value + creature.Skills[SkillName.Magery].Value + creature.Skills[SkillName.EvalInt].Value)/8);
				d_TempTokReward += d_Mage;
			}
							
			if (creature.HasAbility(SpecialAbility.DragonBreath)) //give bonus for creatures that have fire breath
			{
				double d_FireBreath = (creature.HitsMax/75);
				d_TempTokReward += d_FireBreath; //add the firebreath bonus to temp reward
			}	
							
			int i_TokReward = ((int)d_TempTokReward); //set the reward you'll actually get as half then the temp reward.
			i_TokReward = Utility.RandomMinMax((int)(i_TokReward*0.4), (int)(i_TokReward*0.5));
			if (i_TokReward < 1)
				i_TokReward = 1; //set minimum reward to 1
							
			RewardTokens(m, i_TokReward);
		}

		public static void RewardTokens(Mobile m, int amount)
		{
			if (amount < 1)
				return;
			MasterStorageUtils.GiveTypeToPlayer(m as PlayerMobile, typeof(Daat99Tokens), amount, true);
		}
	}
}
