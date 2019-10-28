#region AuthorHeader
//
//	EvoSystem version 2.1, by Xanthos
//
//  Mercenary is based on a concept by Raelis, Sadoul and Grae 
//
#endregion AuthorHeader
using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Misc;
using Server.SkillHandlers;
using System.Collections;
using System.Collections.Generic;
using Server.Targeting;
using Xanthos.Utilities;
using Xanthos.Interfaces;

namespace Xanthos.Evo
{
	[CorpseName( "a sellsword corpse" )]
	public class Mercenary : BaseEvo, IEvoCreature
	{
		[CommandProperty( AccessLevel.Administrator )]
		public new bool CanHue
		{
			get { return false; }
		}

		private static Hashtable s_Keywords;
		private static string s_Version = "EvoSystem version 2.1, by Xanthos";

		public override BaseEvoSpec GetEvoSpec()
		{
			return MercenarySpec.Instance;
		}

		public override BaseEvoEgg GetEvoEgg()
		{
			return null;
		}

		public override bool AddPointsOnDamage { get { return false; } }
		public override bool AddPointsOnMelee { get { return true; } }
		public override Type GetEvoDustType() { return null; }

		enum Commands { None = 0, Restyle, Dress, Undress, Mount, Dismount, Stats, Unload, List, Arm, Grab, Loot, Attack, Tithe, Help, WhosYourDaddy, Last = Commands.WhosYourDaddy, }

		static Mercenary()
		{
			string [] keyWords = { " ", "restyle", "dress", "undress", "mount", "dismount", "stats", "unload", "list", "arm", "grab", "loot", "attack", "tithe", "help", "whosyourdaddy" };

			s_Keywords = new Hashtable( keyWords.Length, StringComparer.OrdinalIgnoreCase );

			for ( int i = 0; i < keyWords.Length; ++i )
				s_Keywords[keyWords[i]] = i;
		}

		public Mercenary( string name ) : base( name, AIType.AI_Melee, 0.01 )
		{
			SpeechHue = Utility.RandomDyedHue();
			Hue = Utility.RandomSkinHue();
			Item hair = new Item( Utility.RandomList( 0x203B, 0x2049, 0x2048, 0x204A ) );
			hair.Hue = Utility.RandomNondyedHue();
			hair.Layer = Layer.Hair;
			hair.Movable = false;
			AddItem( hair );

			if ( Female = Utility.RandomBool() )
				Body = 0x191;
			else
			{
				Body = 0x190;
				if ( Utility.RandomBool() )
				{
					Item beard = new Item( Utility.RandomList( 0x203E, 0x203F, 0x2040, 0x2041, 0x204B, 0x204C, 0x204D ) );
					beard.Hue = hair.Hue;
					beard.Layer = Layer.FacialHair;
					beard.Movable = false;
					AddItem( beard );
				}
			}

			if ( !(this is IEvoGuardian) )
			{
				Item weapon;
				switch ( Utility.Random( 6 ) )
				{
					case 0: weapon = new Kryss(); break;
					case 1: weapon = new Scimitar(); break;
					case 2: weapon = new WarAxe(); break;
					case 3: weapon = new Cutlass(); break;
					case 4: weapon = new HammerPick(); break;
					default: weapon = new WarFork(); break;
				}
				AddItem( weapon );
				AddItem( new Robe() );
			}

			if ( null == Backpack )
			{
				Container pack = new Backpack();
				pack.Movable = false;
				AddItem( pack );
			}
		}

		public Mercenary( Serial serial ) : base( serial ) {}

		public override FoodType FavoriteFood{ get{ return FoodType.Eggs | FoodType.Fish | FoodType.Meat | FoodType.FruitsAndVegies; } }

		protected override void PackSpecialItem()
		{
			Item item;

			switch ( Utility.Random( 4 ) )
			{
				case 0:	item = (Item)new MercenaryDeed(); break;
				case 1: item = (Item)new EtherealLlama(); break;
				case 3: item = (Item)new HoodedShroudOfShadows(); break;
				default: ((BallOfSummoning)(item = (Item)new BallOfSummoning())).Charges = 2000; break;
			}
			item.LootType = LootType.Blessed;
			PackItem( item );
		}

		protected override void Evolve( bool hatching )
		{
			base.Evolve( hatching );
			InvalidateProperties();
		}
		
		public override string ApplyNameSuffix( string suffix )
		{
			if ( !IsBonded )
				return "";

			MercenarySpec spec = GetEvoSpec() as MercenarySpec;

			if ( null != spec && null != spec.Stages )
			{
                string title = GetMercTitle(spec, m_Stage); //UOSI - saves us calling this function 3 times

                if (suffix.Length == 0)
                    suffix = title;
                else if (suffix != title)
                    suffix = String.Concat(suffix, " ", title);
			}

            return base.ApplyNameSuffix( suffix );
		}

		private string GetMercTitle( MercenarySpec spec, int stage )
		{
			string title = "";

			if ( 0 <= m_Stage && m_FinalStage >= m_Stage )
			{
				if ( stage == m_FinalStage || stage == m_FinalStage - 1 )
					title = ", " + spec.Stages[ stage ].Title + " of " + ControlMaster.Name;
				else
					title = spec.Stages[ stage ].Title;
			}

			return title;
		}

		private string GetRestrictedUseTitle( bool isArtifact )
		{
			MercenarySpec spec = GetEvoSpec() as MercenarySpec;

			return null == spec || null == spec.Stages ? "a higher" : GetMercTitle( spec, isArtifact ? spec.ArtifactStage : spec.CraftedWeaponStage );
		}

		private bool CanUseRestrictedItem( bool isArtifact )
		{
			MercenarySpec spec = GetEvoSpec() as MercenarySpec;

			return ( null != spec && ( isArtifact ? spec.ArtifactStage : spec.CraftedWeaponStage ) <= m_Stage );
		}

		public override bool HandlesOnSpeech( Mobile from ) { return true; }

		public override void OnSpeech( SpeechEventArgs e )
		{
			Mobile from = e.Mobile;

			if ( ControlMaster != from || IsDeadPet )
			{
				base.OnSpeech( e );
				return;
			}

			object command = Commands.None;

			if ( e.Speech.Length > "all ".Length && e.Speech.Substring( 0, "all ".Length ).ToLower() == "all " )
				command = s_Keywords[ e.Speech.Substring( "all ".Length ) ];
			else if ( e.Speech.Length > Name.Length + 1 && e.Speech.Substring( 0, Name.Length + 1 ).ToLower() == Name.ToLower() + ' ' )
				command = s_Keywords[ e.Speech.Substring( Name.Length + 1 ) ];

			switch ( (command == null ? (int)Commands.None : (int)command) )
			{
				case (int)Commands.Restyle:
					from.SendGump( new Xanthos.Evo.HairstylistBuyGump( from, this ) );
					break;
				case (int)Commands.Dress:
					Say("I shall attmept to equip all the items in my pack.");
					{
						List<Server.Item> items = Backpack.Items;
				
						for ( int i = items.Count - 1; i >= 0; --i )
						{
							Item item = (Item)items[i];

							if ( item is BaseWeapon || item is BaseClothing || item is BaseArmor || item is BaseJewel )
							{
								Backpack.DropItem( item );
								OnDragDrop( from, item );
							}
						}
					}
					break;
				case (int)Commands.Undress:
					Say("I shall give you everything I am wearing.");
					{
						List<Server.Item> items = Items;
			
						for ( int i = items.Count - 1; i >= 0; --i )
						{
							Item item = (Item)items[i];
							if ( !(item is Container || item is IMountItem) && item.Layer != Layer.FacialHair && item.Layer != Layer.Hair )
								from.AddToBackpack( item );
						}
					}
					break;
				case (int)Commands.Mount:
					if ( null == Mount )
					{
						IMount mount = FindMyMount( Backpack );

						if ( null == mount )
							from.Target = new MountTarget( from, this );
						else
							mount.Rider = this;
					}
					break;
				case (int)Commands.Dismount:
					if ( null != Mount )
					{
						IMount mount = FindMyMount( null );

						if ( null != mount )
						{
							mount.Rider = null;
							if ( mount is EtherealMount )
								Backpack.DropItem( mount as EtherealMount );
							else
								((BaseMount)mount).ControlOrder = OrderType.Follow;
						}
					}
					break;
				case (int)Commands.Stats:
					from.SendGump( new AnimalLoreGump( this ) );
					break;
				case (int)Commands.Unload:
					Say("I shall give you everything in my pack.");
					{
						List<Server.Item> items = Backpack.Items;
			
						for ( int i = items.Count - 1; i >= 0; --i )
						{
							from.AddToBackpack( (Item)items[i] );
						}
					}
					break;
				case (int)Commands.List:
					Say("I am carrying:");
					foreach( Item item in Backpack.Items )
					{
						if ( null != item )
							Say( "{0} {1}", item.Amount, item.GetType().Name );
					}
					break;
				case (int)Commands.Arm:
					{
						Item item = Backpack.FindItemByType( typeof( BaseWeapon ) );

						if ( null == item )
							Say( "I have no weapons to arm myself with." );
						else
						{
							Backpack.DropItem( item );
							OnDragDrop( from, item );
						}
					}
					break;
				case (int)Commands.Grab:
					GrabItems( false );
					break;
				case (int)Commands.Loot:
					GrabItems( true );
					break;
				case (int)Commands.Tithe:
					{
						bool ankhInRange = false;

						foreach ( Item item in GetItemsInRange( 2 ) )
						{
							if ( item is AnkhNorth || item is AnkhWest )
							{
								ankhInRange = true;
								break;
							}
						}

						if ( !ankhInRange )
							Say( "I must be near a shrine to tithe." );
						else
						{

							Container pack = Backpack;
							if ( null != pack )
							{
								Item item = pack.FindItemByType( typeof( Gold ) );
								int tithe;

								if ( null != item && item.Amount > 0 && pack.ConsumeTotal( typeof( Gold ), (tithe = item.Amount) ))
								{
									Emote( "*Tithes gold as a sign of devotion.*" );
									TithingPoints += tithe;
									PlaySound( 0x243 );
									PlaySound( 0x2E6 );
								}
								Say( "I now have {0} tithing points.",  TithingPoints );
							}
						}
					}
					break;
				case (int)Commands.Help:
					Say("I will follow these commands: restyle, dress, undress, mount, dismount, unload, list, arm, grab, loot, attack, tithe and stats.");
					break;
				case (int)Commands.WhosYourDaddy:
					Say( s_Version );
					break;
				case (int)Commands.Attack:
					switch ( Utility.Random( 3 ) )
					{
						case 0: WeaponAbility.SetCurrentAbility( this, ((BaseWeapon)Weapon).PrimaryAbility ); break;
						case 1: WeaponAbility.SetCurrentAbility( this, ((BaseWeapon)Weapon).SecondaryAbility ); break;
						case 2: new Server.Spells.Chivalry.ConsecrateWeaponSpell( this, null ).Cast(); break;
					}
					goto default;
				default:
					base.OnSpeech( e );
					return;
			}
			e.Handled = true;
		}

		public override void OnThink()
		{
			base.OnThink();

			if ( Hits < HitsMax - 100 && null == BandageContext.GetContext( this ))
			{
				Item item = Backpack.FindItemByType( typeof(Bandage) );

				if ( null != item && null != BandageContext.BeginHeal( this , this ))
					item.Consume( 1 );
			}
			if ( Hunger < 10 || Loyalty <= BaseCreature.MaxLoyalty / 10 )
			{
				CheckFeedSelf();
			}
		}

		private void CheckFeedSelf()
		{
			if ( !IsDeadPet && null != Backpack )
			{
				Item item = Backpack.FindItemByType( typeof( Food ) );

				if ( null == item )
					return;

				((Food)item).Eat( this ); Stam += 15;

				if ( Loyalty < BaseCreature.MaxLoyalty && 0.5 >= Utility.RandomDouble() )
				{
					++Loyalty;
					this.Emote( "*Looks happier.*" );

					if ( IsBondable && !IsBonded )
					{
						Mobile master = ControlMaster;

						if ( master != null && master.Skills[SkillName.AnimalTaming].Value >= MinTameSkill )
						{
							if ( BondingBegin == DateTime.MinValue )
							{
								BondingBegin = DateTime.Now;
							}
							else if ( (BondingBegin + BondingDelay) <= DateTime.Now )
							{
								IsBonded = true;
								BondingBegin = DateTime.MinValue;
								master.SendLocalizedMessage( 1049666 ); // Your pet has bonded with you!
								InvalidateProperties();
							}
						}
					}
				}
			}
		}

		public void GrabItems( bool ignoreNoteriety )
		{
			ArrayList items = new ArrayList();
			bool rejected = false;
			bool lootAdded = false;

			Emote( "*Rummages through items on the ground.*" );

			foreach ( Item item in GetItemsInRange( 2 ) )
			{
				if ( item.Movable )
					items.Add( item );
				else if ( item is Corpse )
				{	// Either criminally loot any corpses or loot only corpses that we have rights to
					if ( ignoreNoteriety || NotorietyHandlers.CorpseNotoriety( this, (Corpse)item ) != Notoriety.Innocent )
					{
						Emote( "*Rummages through items in a corpse.*" );
						foreach ( Item corpseItem in ((Corpse)item).Items )
							items.Add( corpseItem );
					}
				}
			}
			foreach ( Item item in items )
			{
				if ( !Backpack.CheckHold( this, item, false, true ) )
					rejected = true;
				else
				{
					bool isRejected;
					LRReason reason;

					NextActionTime = Core.TickCount;
					Lift( item, item.Amount, out isRejected, out reason );

					if ( !isRejected )
					{
						Drop( this, Point3D.Zero );
						lootAdded = true;
					}
					else
					{
						rejected = true;
					}
				}
			}
			if ( lootAdded )
				PlaySound( 0x2E6 ); //drop gold sound
			if ( rejected )
				Say( "I could not pick up all of the items." );
		}

		public override bool CheckGold( Mobile from, Item dropped )
		{
			if ( dropped is Gold )
				return false;

			return base.CheckGold( from, dropped );
		}

		public IMount FindMyMount( Container pack )
		{
			List<Server.Item> items = ( null == pack ) ? Items : pack.Items;

			foreach ( Item item in items )
			{
				if ( item is IMountItem )
					return ((IMountItem)item).Mount;

				else if ( item.Layer == Layer.Mount )
					return (IMount)item;
			}
			return null;
		}

		public override bool OnDragDrop( Mobile from, Item dropped )
		{
			Item itemEquipped;

			if (( ControlMaster != from && this != from ) || IsDeadPet )
				return base.OnDragDrop( from, dropped );

			bool isArtifact = Xanthos.Utilities.Misc.IsArtifact( dropped );

			if ( ( Xanthos.Utilities.Misc.IsPlayerConstructed( dropped ) || isArtifact ) && !CanUseRestrictedItem( isArtifact ) )
			{
				from.SendMessage( Name + " must attain {0} level to equip {1} items.", GetRestrictedUseTitle( isArtifact ), isArtifact ? "artifact" : "crafted" );
				from.AddToBackpack( dropped );
				return false;
			}

			if ( dropped is Bandage || dropped is Food )
			{
				Backpack.DropItem( dropped );
				from.SendMessage( "You give " + Name + " supplies." );
				return true;
			}
			else if ( dropped is Gold )
			{
				Backpack.DropItem( dropped );
				from.SendMessage( "You give " + Name + " gold." );
				return true;
			}
			else if ( dropped is BaseWeapon )
			{
				itemEquipped = FindItemOnLayer( Layer.TwoHanded );

				if ( null != itemEquipped && ((BaseWeapon)dropped).CheckConflictingLayer( this, itemEquipped, Layer.TwoHanded ))
					from.AddToBackpack( itemEquipped );

				itemEquipped = FindItemOnLayer( Layer.OneHanded );
				if ( null != itemEquipped && ((BaseWeapon)dropped).CheckConflictingLayer( this, itemEquipped, Layer.OneHanded ))
					from.AddToBackpack( itemEquipped );

				itemEquipped = FindItemOnLayer( Layer.FirstValid );
				if ( null != itemEquipped && ((BaseWeapon)dropped).CheckConflictingLayer( this, itemEquipped, Layer.FirstValid ))
					from.AddToBackpack( itemEquipped );

				Backpack.DropItem( dropped );
				AddItem( dropped );
				from.SendMessage( "You give " + Name + " a weapon." );
				return true;
			}
			else if ( dropped is BaseArmor )
			{
				BaseArmor armor = (BaseArmor)dropped;

				if ( !armor.AllowMaleWearer && Body.IsMale )
				{
					from.SendLocalizedMessage( 1010388 ); // Only females can wear this.
					from.AddToBackpack( armor );
				}
				else if ( !armor.AllowFemaleWearer && Body.IsFemale )
				{
					from.SendMessage( "Only males can wear this." );
					from.AddToBackpack( armor );
				}
				else
				{
					itemEquipped = FindItemOnLayer( dropped.Layer );
					if ( null != itemEquipped )
						from.AddToBackpack( itemEquipped );

					Backpack.DropItem( dropped );
					AddItem( dropped );
					from.SendMessage( "You give " + Name + " armor." );
					return true;
				}
			}
			else if ( dropped is BaseClothing || dropped is BaseJewel )
			{
				if ( null != ( itemEquipped = FindItemOnLayer( dropped.Layer ) ))
					from.AddToBackpack( itemEquipped );

				Backpack.DropItem( dropped );
				AddItem( dropped );
				from.SendMessage( "You give " + Name + (dropped is BaseJewel ? " jewelry." : " clothing.") );
				return true;
			}

			return base.OnDragDrop( from, dropped );
		}

		private bool HasRangedEquipped()
		{
			Item item = FindItemOnLayer( Layer.TwoHanded );

			return ( item != null && item is BaseRanged );
		}

		public override void OnItemAdded( Item item )
		{
			base.OnItemAdded( item );
			if ( item is BaseRanged )
			{
				ChangeAIType( AIType.AI_Archer );
			}
		}

		public override void OnItemRemoved( Item item )
		{
			base.OnItemRemoved( item );
			if ( item is BaseRanged )
			{
				ChangeAIToDefault();
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize(writer);
			writer.Write( (int)0 );			
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			if ( HasRangedEquipped() )
				ChangeAIType( AIType.AI_Archer );
		}
	}

	internal class MountTarget : Target
	{
		private Mercenary m_Merc;

		public MountTarget( Mobile from, Mercenary merc ) : base( 1, false, TargetFlags.None )
		{
			m_Merc = merc;
			from.SendMessage( "Choose a mount for " + m_Merc.Name + " to ride." );
		}

		protected override void OnTarget( Mobile from, object obj )
		{
			DoOnTarget( from, obj, m_Merc );
		}

		public static void DoOnTarget( Mobile from, object o, Mercenary merc )
		{
			EtherealMount ethy = o as EtherealMount;
			if ( null != ethy )
			{
				if ( null != ethy.Rider )
					from.SendMessage( "This ethereal mount is already in use by someone else." );

				else if ( !ethy.IsChildOf( from.Backpack ) )
					from.SendMessage( "The ethereal mount must be in your pack for you to use it." );
				else
					ethy.Rider = merc;
			}
			else
			{
				BaseMount mount = o as BaseMount;

				if ( null == mount )
					from.SendMessage( "That is not a mount." );

				else if ( null != mount.Rider )
					from.SendMessage( "This mount is already in use by someone else." );

				else if ( mount.ControlMaster != from )
					from.SendMessage( "You do not own this mount." );
				else
					mount.Rider = merc;
			}
		}
	}
}
