using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Regions;
using Server.Targeting;
using System.Collections;
using Server.Spells.Third;
using Server.Spells.Fourth;
using Server.Spells.Necromancy;

namespace Server.Engines.XmlSpawner2
{
    public class XMLPetAttacksBonus : XmlAttachment
    {
		ConfiguredPetXML c = new ConfiguredPetXML();
		
		private int proximityrange = 5;  
		public bool m_PetSpeak				= true;
		public bool m_TeleToTargetSwitch	= true;
		public bool m_MassProvokeSwitch		= true;
		public bool m_MassPeace				= true;
		public bool m_SuperHeal				= true;
		public bool m_BlessedPower			= true;
		public bool m_AreaFireBlast			= true;
		public bool m_AreaIceBlast			= true;
		public bool m_AreaAirBlast			= true;
		public bool m_AuraStatBoost			= true;

        [CommandProperty(AccessLevel.GameMaster)]
        public bool AuraStatBoost
        {
            get { return m_AuraStatBoost; }
            set { m_AuraStatBoost = value; }
        }
		
        [CommandProperty(AccessLevel.GameMaster)]
        public bool AreaAirBlast
        {
            get { return m_AreaAirBlast; }
            set { m_AreaAirBlast = value; }
        }
		
        [CommandProperty(AccessLevel.GameMaster)]
        public bool AreaIceBlast
        {
            get { return m_AreaIceBlast; }
            set { m_AreaIceBlast = value; }
        }
		
        [CommandProperty(AccessLevel.GameMaster)]
        public bool AreaFireBlast
        {
            get { return m_AreaFireBlast; }
            set { m_AreaFireBlast = value; }
        }
		
        [CommandProperty(AccessLevel.GameMaster)]
        public bool BlessedPower
        {
            get { return m_BlessedPower; }
            set { m_BlessedPower = value; }
        }
		
        [CommandProperty(AccessLevel.GameMaster)]
        public bool SuperHeal
        {
            get { return m_SuperHeal; }
            set { m_SuperHeal = value; }
        }
		
        [CommandProperty(AccessLevel.GameMaster)]
        public bool MassPeace
        {
            get { return m_MassPeace; }
            set { m_MassPeace = value; }
        }
		
        [CommandProperty(AccessLevel.GameMaster)]
        public bool MassProvoke
        {
            get { return m_MassProvokeSwitch; }
            set { m_MassProvokeSwitch = value; }
        }
		
        [CommandProperty(AccessLevel.GameMaster)]
        public bool TeleportToTarget
        {
            get { return m_TeleToTargetSwitch; }
            set { m_TeleToTargetSwitch = value; }
        }
		
        [CommandProperty(AccessLevel.GameMaster)]
        public bool PetSpeak
        {
            get { return m_PetSpeak; }
            set { m_PetSpeak = value; }
        }


        public XMLPetAttacksBonus(ASerial serial) : base(serial)
        {
        }

        [Attachable]
        public XMLPetAttacksBonus()
        {
        }

        [Attachable]
        public XMLPetAttacksBonus(int level)
        {
        }
		
		public override void OnAttach()
		{
			base.OnAttach();
			if(AttachedTo is PlayerMobile)
			{
				Delete();
			}
		}
		
		public override void OnDelete()
		{
			base.OnDelete();
			if(AttachedTo is BaseCreature)
			{

			}
		}
		
		public static void MassProvokeXML(BaseCreature mobile, Mobile player)
		{	
			ArrayList list = new ArrayList();

			foreach ( Mobile m in mobile.GetMobilesInRange( 15 ) )
			{
				if ( m == mobile || !m.CanBeHarmful( m ) )
					continue;

				if ( m is BaseCreature )
					list.Add( m );
				else if ( m.Player )
					list.Add( m );
			}

			foreach ( Mobile m in list )
			{
				m.DoHarmful( m );
				player.Combatant = null;
				m.Combatant = player;
				m.PlaySound( 0x403 );
				m.Emote("*you see {0} looks furious*", m.Name);
			}
		}
		
		public static void TeleToTarget(BaseCreature mobile, Mobile player)
		{
			if ( mobile.Combatant != null )
			if ( !player.InRange( mobile, 5 ) )
			{                 
				Point3D from = mobile.Location;
				Point3D to = player.Location;
				if ( mobile.Mana >= 10 )
				{
					mobile.Location = to;
					mobile.Mana -= 10;
					mobile.Say( "Grrrr" );
					Effects.SendLocationParticles( EffectItem.Create( from, mobile.Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 2023 );
					Effects.SendLocationParticles( EffectItem.Create(   to, mobile.Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 5023 );
					mobile.PlaySound( 0x1FE );
				}
			}
		}
		
		public static void MassPeaceXML(BaseCreature mobile, Mobile player)
		{
			ArrayList list = new ArrayList();
			
			foreach ( Mobile m in mobile.GetMobilesInRange( 15 ) )
			{
				if ( m == mobile || !m.CanBeHarmful( m ) )
					continue;

				if ( m is BaseCreature && (((BaseCreature)m).Controlled || ((BaseCreature)m).Summoned || ((BaseCreature)m).Team != mobile.Team) )
					list.Add( m );
				else if ( m.Player )
					list.Add( m );
			}

			foreach ( Mobile m in list )
			{
				m.DoHarmful( m );
				m.Combatant = null;
				m.PlaySound( 0x418 );
				m.Emote("*you see {0} looks peacful*", m.Name);
			}
		}
		
		public static void SuperHealXML(BaseCreature bc, Mobile player)
		{
			if (!bc.Alive)
                    bc.Resurrect();
			if (bc.Hits < bc.HitsMax)
				bc.Hits = bc.HitsMax;
			if (bc.Mana < bc.ManaMax)
				bc.Mana = bc.ManaMax;
			if (bc.Stam < bc.StamMax)
				bc.Stam = bc.StamMax;
			if (bc.Poison != null)
				bc.Poison = null;
			if (bc.Paralyzed == true)
				bc.Paralyzed = false;
			EvilOmenSpell.TryEndEffect(bc);
			StrangleSpell.RemoveCurse(bc);
			CorpseSkinSpell.RemoveCurse(bc);
			CurseSpell.RemoveEffect(bc);
			MortalStrike.EndWound(bc);
			BloodOathSpell.RemoveCurse(bc);
			MindRotSpell.ClearMindRotScalar(bc);
			bc.Loyalty = BaseCreature.MaxLoyalty;
			Effects.SendTargetEffect(bc, 0x3709, 32);
			Effects.SendTargetEffect(bc, 0x376A, 32);
			bc.PlaySound(0x208);
			bc.Emote("*you see {0} looks refreshed!*", bc.Name);
		}
		
		public static void BlessedXML(BaseCreature bc, Mobile player)
		{
			bc.Emote("*you see {0} looks ready to fight harder!*", bc.Name);
			bc.AddStatMod(new StatMod(StatType.Dex, "XmlDex", Utility.RandomMinMax( 20, 60 ), TimeSpan.FromSeconds( 25.0 )));
			bc.AddStatMod(new StatMod(StatType.Str, "XmlStr", Utility.RandomMinMax( 20, 60 ), TimeSpan.FromSeconds( 25.0 )));
			bc.AddStatMod(new StatMod(StatType.Int, "XmlInt", Utility.RandomMinMax( 20, 60 ), TimeSpan.FromSeconds( 25.0 )));
			bc.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
			bc.PlaySound(0x1EA);
			bc.InvalidateProperties();
		}
		
		public static void AreaFireBlastXML(BaseCreature mobile, Mobile player, Mobile master)
		{
			ArrayList list = new ArrayList();
			
			foreach ( Mobile m in mobile.GetMobilesInRange( 10 ) )
			{
				if ( m == mobile || !m.CanBeHarmful( m ) )
					continue;

				if ( m is BaseCreature && (((BaseCreature)m).Controlled || ((BaseCreature)m).Summoned || ((BaseCreature)m).Team != mobile.Team) )
					list.Add( m );
				else if ( m.Player )
					list.Add( m );
			}
			mobile.Emote("*you see {0} shoots a distructive fire attack!*", mobile.Name);
			mobile.Mana -= 25;
			mobile.Stam -= 15;
			foreach ( Mobile m in list )
			{
				if ( m == master)
					return;
				m.DoHarmful( m );
				m.FixedParticles( 0x3709, 10, 30, 5052, EffectLayer.Waist );
				m.PlaySound( 0x208 );
				m.SendMessage( "Your skin blisters as the fire burns you!" );
				m.Damage (((Utility.Random( 25, 35 )) - (m.FireResistance /2)));
			}
		}
		
		public static void AreaIceBlastXML(BaseCreature mobile, Mobile player, Mobile master)
		{
			ArrayList list = new ArrayList();

			foreach ( Mobile m in mobile.GetMobilesInRange( 10 ) )
			{
				if ( m == mobile || !m.CanBeHarmful( m ) )
					continue;

				if ( m is BaseCreature && (((BaseCreature)m).Controlled || ((BaseCreature)m).Summoned || ((BaseCreature)m).Team != mobile.Team) )
					list.Add( m );
				else if ( m.Player )
					list.Add( m );
			}
			mobile.Emote("*you see {0} shoots a distructive ice attack!*", mobile.Name);
			mobile.Mana -= 25;
			mobile.Stam -= 15;
			foreach ( Mobile m in list )
			{
				if ( m == master)
					return;
				m.DoHarmful( m );
				m.FixedParticles( 0x1fb7, 50, 50, 5052, EffectLayer.Waist );
				m.PlaySound( 279 );
				m.PlaySound( 280 );
				m.SendMessage( "Your skin numbs as the cold freezes you!" );
				m.Damage( ((Utility.Random( 25, 35 )) - (m.ColdResistance /2)) );
			}
		}
		
		public static void AreaAirBlastXML(BaseCreature mobile, Mobile player, Mobile master)
		{
			ArrayList list = new ArrayList();

			foreach ( Mobile m in mobile.GetMobilesInRange( 10 ) )
			{		
				if ( m == mobile || !m.CanBeHarmful( m ) )
					continue;
				

				if ( m is BaseCreature && (((BaseCreature)m).Controlled || ((BaseCreature)m).Summoned || ((BaseCreature)m).Team != mobile.Team) )
					list.Add( m );
				else if ( m.Player )
					list.Add( m );
			}
			mobile.Emote("*you see {0} shoots a distructive air attack!*", mobile.Name);
			mobile.Mana -= 25;
			mobile.Stam -= 15;
			foreach ( Mobile m in list )
			{
				if ( m == master)
					return;
				m.DoHarmful( m );
				m.FixedParticles( 0x3728, 50, 50, 5052, EffectLayer.Waist );
				m.PlaySound( 655 );
				m.SendMessage( "Your lose your breath as the air hits you!" );
				int toStrike = Utility.RandomMinMax( 25, 35 );
				m.Damage( toStrike, mobile );
			}
		}
		
		public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
		{
		    ConfiguredPetXML cp = new ConfiguredPetXML();
			Mobile master = ((BaseCreature)this.AttachedTo).ControlMaster;
			XMLPlayerLevelAtt xmlplayer = (XMLPlayerLevelAtt)XmlAttach.FindAttachment(master, typeof(XMLPlayerLevelAtt));
			XMLPetLevelAtt petxml = (XMLPetLevelAtt)XmlAttach.FindAttachment(((BaseCreature)this.AttachedTo), typeof(XMLPetLevelAtt));
			if(attacker == null) return;
			
			if (petxml == null)
				return;

			switch (Utility.RandomMinMax(1, 3))
			{
				case 1:	if (AreaFireBlast && cp.AreaFireBlastChance >= Utility.RandomDouble()) // 0.5 is 50% , 0.05 is 5%
						{
							if (cp.AreaFireBlast == false)
								return;
							if (petxml.Levell < cp.AreaFireBlastReq)
								return;
							AreaFireBlastXML(((BaseCreature)this.AttachedTo), defender, master);
						}	break;
				case 2:	if (AreaIceBlast && cp.AreaIceBlastChance >= Utility.RandomDouble())
						{
							if (cp.AreaIceBlast == false)
								return;
							if (petxml.Levell < cp.AreaIceBlastReq)
								return;
							AreaIceBlastXML(((BaseCreature)this.AttachedTo), defender, master);
						};	break;
				case 3: if (AreaAirBlast && cp.AreaAirBlastChance >= Utility.RandomDouble())
						{
							if (cp.AreaAirBlast == false)
								return;
							if (petxml.Levell < cp.AreaAirBlastReq)
								return;
							AreaAirBlastXML(((BaseCreature)this.AttachedTo), defender, master);
						};	break;
			}
		}

		public override bool HandlesOnMovement { get { return true; } }
		public override void OnMovement(MovementEventArgs e )
		{
			base.OnMovement(e);
		    ConfiguredPetXML cp = new ConfiguredPetXML();
			XMLPetLevelAtt petxml = (XMLPetLevelAtt)XmlAttach.FindAttachment(((BaseCreature)this.AttachedTo), typeof(XMLPetLevelAtt));
			Mobile master = ((BaseCreature)this.AttachedTo).ControlMaster;
			XMLPlayerLevelAtt xmlplayer = (XMLPlayerLevelAtt)XmlAttach.FindAttachment(master, typeof(XMLPlayerLevelAtt));

			if(e.Mobile == null) return;
			
			if (petxml == null)
				return;
			
			if (AuraStatBoost == true)
			{
				if (cp.AuraStatBoost == false)
					return;
				if (petxml.Levell < cp.AuraStatBoostReq)
					return;
				if (xmlplayer == null)
					return;
				BonusStatAtt10 aurastatboost10		= (BonusStatAtt10)XmlAttach.FindAttachment(master, typeof(BonusStatAtt10));
				BonusStatAtt20 aurastatboost20 		= (BonusStatAtt20)XmlAttach.FindAttachment(master, typeof(BonusStatAtt20));
				BonusStatAtt30 aurastatboost30 		= (BonusStatAtt30)XmlAttach.FindAttachment(master, typeof(BonusStatAtt30));
				BonusStatAtt40 aurastatboost40 		= (BonusStatAtt40)XmlAttach.FindAttachment(master, typeof(BonusStatAtt40));
				BonusStatAtt50 aurastatboost50 		= (BonusStatAtt50)XmlAttach.FindAttachment(master, typeof(BonusStatAtt50));
				BonusStatAtt60 aurastatboost60 		= (BonusStatAtt60)XmlAttach.FindAttachment(master, typeof(BonusStatAtt60));
				BonusStatAtt70 aurastatboost70 		= (BonusStatAtt70)XmlAttach.FindAttachment(master, typeof(BonusStatAtt70));
				BonusStatAtt80 aurastatboost80 		= (BonusStatAtt80)XmlAttach.FindAttachment(master, typeof(BonusStatAtt80));
				BonusStatAtt90 aurastatboost90 		= (BonusStatAtt90)XmlAttach.FindAttachment(master, typeof(BonusStatAtt90));
				BonusStatAtt100 aurastatboost100	= (BonusStatAtt100)XmlAttach.FindAttachment(master, typeof(BonusStatAtt100));
				BonusStatAtt140 aurastatboost140 	= (BonusStatAtt140)XmlAttach.FindAttachment(master, typeof(BonusStatAtt140));
				BonusStatAtt160 aurastatboost160 	= (BonusStatAtt160)XmlAttach.FindAttachment(master, typeof(BonusStatAtt160));
				BonusStatAtt180 aurastatboost180	= (BonusStatAtt180)XmlAttach.FindAttachment(master, typeof(BonusStatAtt180));
				BonusStatAtt200 aurastatboost200 	= (BonusStatAtt200)XmlAttach.FindAttachment(master, typeof(BonusStatAtt200));
				BonusStatAtt201 aurastatboost201 	= (BonusStatAtt201)XmlAttach.FindAttachment(master, typeof(BonusStatAtt201));

				if (xmlplayer.Levell <= 10 && aurastatboost10 == null)
					XmlAttach.AttachTo(master, new BonusStatAtt10());
				else if (xmlplayer.Levell > 10 && xmlplayer.Levell <= 20 && aurastatboost20 == null)
					XmlAttach.AttachTo(master, new BonusStatAtt20());
				else if (xmlplayer.Levell > 20 && xmlplayer.Levell <= 30 && aurastatboost30 == null)
					XmlAttach.AttachTo(master, new BonusStatAtt30());
				else if (xmlplayer.Levell > 30 && xmlplayer.Levell <= 40 && aurastatboost40 == null)
					XmlAttach.AttachTo(master, new BonusStatAtt40());
				else if (xmlplayer.Levell > 40 && xmlplayer.Levell <= 50 && aurastatboost50 == null)
					XmlAttach.AttachTo(master, new BonusStatAtt50());
				else if (xmlplayer.Levell > 50 && xmlplayer.Levell <= 60 && aurastatboost60 == null)
					XmlAttach.AttachTo(master, new BonusStatAtt60());
				else if (xmlplayer.Levell > 60 && xmlplayer.Levell <= 70 && aurastatboost70 == null)
					XmlAttach.AttachTo(master, new BonusStatAtt70());
				else if (xmlplayer.Levell > 70 && xmlplayer.Levell <= 80 && aurastatboost80 == null)
					XmlAttach.AttachTo(master, new BonusStatAtt80());
				else if (xmlplayer.Levell > 80 && xmlplayer.Levell <= 90 && aurastatboost90 == null)
					XmlAttach.AttachTo(master, new BonusStatAtt90());
				else if (xmlplayer.Levell > 90 && xmlplayer.Levell <= 100 && aurastatboost100 == null)
					XmlAttach.AttachTo(master, new BonusStatAtt100());
				else if (xmlplayer.Levell > 100 && xmlplayer.Levell <= 140 && aurastatboost140 == null)
					XmlAttach.AttachTo(master, new BonusStatAtt140());
				else if (xmlplayer.Levell > 140 && xmlplayer.Levell <= 160 && aurastatboost160 == null)
					XmlAttach.AttachTo(master, new BonusStatAtt160());
				else if (xmlplayer.Levell > 160 && xmlplayer.Levell <= 180 && aurastatboost180 == null)
					XmlAttach.AttachTo(master, new BonusStatAtt180());
				else if (xmlplayer.Levell > 180 && xmlplayer.Levell <= 200 && aurastatboost200 == null)
					XmlAttach.AttachTo(master, new BonusStatAtt200());
				else if (xmlplayer.Levell > 200 && aurastatboost201 == null)
					XmlAttach.AttachTo(master, new BonusStatAtt201());
			}

			switch (Utility.RandomMinMax(1, 6))
			{
				case 1:	if (TeleportToTarget && cp.TelePortToTarChance >= Utility.RandomDouble()) // 0.5 is 50% , 0.05 is 5%
						{
							if (cp.TelePortToTarget == false)
								return;
							if (petxml.Levell < cp.TelePortToTargetReq)
								return;
							if ( ((BaseCreature)this.AttachedTo).Combatant == null )
								return;
							TeleToTarget(((BaseCreature)this.AttachedTo), e.Mobile);
						}	break;
				case 2:	if (MassProvoke && cp.MassProvokeChance >= Utility.RandomDouble())
						{
							if (cp.MassProvokeToAtt == false)
								return;
							if (petxml.Levell < cp.MassProvokeToAttReq)
								return;
							if ( ((BaseCreature)this.AttachedTo).Combatant == null )
								return;
							MassProvokeXML(((BaseCreature)this.AttachedTo), e.Mobile);
						};	break;
				case 3: if (MassPeace && cp.MassPeaceChance	  >= Utility.RandomDouble())
						{
							if (cp.MassPeaceArea == false)
								return;
							if (petxml.Levell < cp.MassPeaceReq)
								return;
							if ( ((BaseCreature)this.AttachedTo).Combatant == null )
								return;
							MassPeaceXML(((BaseCreature)this.AttachedTo), e.Mobile);
						};	break;
				case 4: if (SuperHeal && cp.SuperHealChance  >= Utility.RandomDouble())
						{
							if (cp.SuperHeal == false)
								return;
							if (petxml.Levell < cp.SuperHealReq)
								return;
							if (cp.PetShouldBeBonded == true)
							{
								if (((BaseCreature)this.AttachedTo).IsBonded == false)
									return;
								else
								{
									SuperHealXML(((BaseCreature)this.AttachedTo), e.Mobile);
									break;
								}
							}
							SuperHealXML(((BaseCreature)this.AttachedTo), e.Mobile);
						};	break;
				case 5: if (BlessedPower && cp.BlessedPowerChance  >= Utility.RandomDouble())
						{
							if (cp.BlessedPower == false)
								return;
							if (petxml.Levell < cp.BlessedPowerReq)
								return;
							if ( ((BaseCreature)this.AttachedTo).Combatant == null )
								return;
							BlessedXML(((BaseCreature)this.AttachedTo), e.Mobile);
						};	break;
//				case 6: 
//				case 7: from.Hue = 0x7F7; break;
//				case 8: from.Hue = 0x7F8; break;
//				case 9: from.Hue = 0x7F9; break;

			}

			if(AttachedTo is BaseCreature && Utility.InRange( e.Mobile.Location, ((BaseCreature)this.AttachedTo).Location, proximityrange ))
			{
				OnTrigger(null, e.Mobile);
			} 
			else
				return;
		}
		public override void OnTrigger(object activator, Mobile m)
		{
			ConfiguredPetXML cp = new ConfiguredPetXML();
			
            if (AttachedTo is BaseCreature)
            {
				Point3D loc = new Point3D(0, 0, 0);
				Map map;
				if (PetSpeak)
				{
					if (cp.PetSpeak == false)
						return;
					PetSoundsTalk(((BaseCreature)this.AttachedTo)); 
				}
            }
        }
		public static void PetSoundsTalk(BaseCreature mobile)
		{		
			ConfiguredPetXML cpp = new ConfiguredPetXML();
			double chance = Utility.RandomDouble();
			if (chance < cpp.PetSpeakChance) //
			{
				PetSpeakRandom( petspeak, mobile ); 
//				mobile.PlaySound( 0x420 );   // coughing
			}
		}
		private static bool pb_petsounds;
		static string[] petspeak = new string[]
		{ 
			"*Grumble!*",
			"*Sniff*",
			"*Scatch*",
			"*Sniff*",
			"*Shakes Body*",
			"*Feels Restless*"
		};
		private static void PetSpeakRandom( string[] say, Mobile m )
		{
			m.Say( say[Utility.Random( say.Length )] );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize(writer);

			writer.Write( (int) 0 );
			// version 0
			writer.Write((bool)m_PetSpeak);
			writer.Write((bool)m_TeleToTargetSwitch);
			writer.Write((bool)m_MassProvokeSwitch);
			writer.Write((bool)m_MassPeace);
			writer.Write((bool)m_SuperHeal);
			writer.Write((bool)m_BlessedPower);
			writer.Write((bool)m_AreaFireBlast);
			writer.Write((bool)m_AreaIceBlast);
			writer.Write((bool)m_AreaAirBlast);
			writer.Write((bool)m_AuraStatBoost);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
			// version 0
			m_PetSpeak = reader.ReadBool();
			m_TeleToTargetSwitch = reader.ReadBool();
			m_MassProvokeSwitch = reader.ReadBool();
			m_MassPeace = reader.ReadBool();	
			m_SuperHeal = reader.ReadBool();
			m_BlessedPower = reader.ReadBool();
			m_AreaFireBlast = reader.ReadBool();
			m_AreaIceBlast = reader.ReadBool();
			m_AreaAirBlast = reader.ReadBool();
			m_AuraStatBoost = reader.ReadBool();
		}
		
    }
}
