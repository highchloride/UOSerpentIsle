using System;
using System.Collections;
using Server.Network;
using Server.Items;
using Server.Targeting;
using Server.Gumps;
using Server.Mobiles;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Ranger
{
	public class RangerFamiliarSpell : RangerSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
		                                                "Animal Companion", "Sinta Kurwa Ner Arda Moina",
		                                                //SpellCircle.Sixth,
		                                                203,
		                                                9031,
		                                                CReagent.DestroyingAngel,
		                                                CReagent.SpringWater,
		                                                CReagent.PetrafiedWood
		                                               );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

		public override double CastDelay{ get{ return 3.0; } }
		public override double RequiredSkill{ get{ return 30.0; } }
		public override int RequiredMana{ get{ return 17; } }

		public RangerFamiliarSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		private static Hashtable m_Table = new Hashtable();

		public static Hashtable Table{ get{ return m_Table; } }

		public override bool CheckCast()
		{
			BaseCreature check = (BaseCreature)m_Table[Caster];

			if ( check != null && !check.Deleted )
			{
				Caster.SendLocalizedMessage( 1061605 ); // You already have a familiar.
				return false;
			}

			return base.CheckCast();
		}

		public override void OnCast()
		{
			if ( CheckSequence() )
			{
				Caster.CloseGump( typeof( RangerFamiliarGump ) );
				Caster.SendGump( new RangerFamiliarGump( Caster, m_Entries ) );
			}

			FinishSequence();
		}

		private static RangerFamiliarEntry[] m_Entries = new RangerFamiliarEntry[]
		{
			new RangerFamiliarEntry( typeof( PackRatFamiliar ), "Pack Rat",  30.0,  30.0 ),
			new RangerFamiliarEntry( typeof( IceHoundFamiliar ), "Ice Hound",  50.0,  50.0 ),
			new RangerFamiliarEntry( typeof( ThunderHoundFamiliar ), "Thunder Hound",  60.0,  60.0 ),
			new RangerFamiliarEntry( typeof( HellHoundFamiliar ), "Hell Hound",  80.0,  80.0 ),
			new RangerFamiliarEntry( typeof( VampireWolfFamiliar ), "Vampire Wolf", 100.0, 100.0 ),
			new RangerFamiliarEntry( typeof( TigerFamiliar ), "Saber Tooth Tiger", 115.0, 115.0 )

		};

		public static RangerFamiliarEntry[] Entries{ get{ return m_Entries; } }
	}

	public class RangerFamiliarEntry
	{
		private Type m_Type;
		private object m_Name;
		private double m_ReqAnimalLore;
		private double m_ReqAnimalTaming;

		public Type Type{ get{ return m_Type; } }
		public object Name{ get{ return m_Name; } }
		public double ReqAnimalLore{ get{ return m_ReqAnimalLore; } }
		public double ReqAnimalTaming{ get{ return m_ReqAnimalTaming; } }

		public RangerFamiliarEntry( Type type, object name, double reqAnimalLore, double reqAnimalTaming )
		{
			m_Type = type;
			m_Name = name;
			m_ReqAnimalLore = reqAnimalLore;
			m_ReqAnimalTaming = reqAnimalTaming;
		}
	}

	public class RangerFamiliarGump : Gump
	{
		private Mobile m_From;
		private RangerFamiliarEntry[] m_Entries;

		private const int  EnabledColor16 = 0x0F20;
		private const int DisabledColor16 = 0x262A;

		private const int  EnabledColor32 = 0x18CD00;
		private const int DisabledColor32 = 0x4A8B52;

		public RangerFamiliarGump( Mobile from, RangerFamiliarEntry[] entries ) : base( 200, 100 )
		{
			m_From = from;
			m_Entries = entries;

			AddPage( 0 );

			AddBackground( 10, 10, 250, 178, 9270 );
			AddAlphaRegion( 20, 20, 230, 158 );

			AddImage( 220, 20, 10464 );
			AddImage( 220, 72, 10464 );
			AddImage( 220, 124, 10464 );

			AddItem( 188, 16, 6883 );
			AddItem( 198, 168, 6881 );
			AddItem( 8, 15, 6882 );
			AddItem( 2, 168, 6880 );

			AddHtmlLocalized( 30, 26, 200, 20, 1060147, EnabledColor16, false, false ); // Chose thy familiar...

			double lore = from.Skills[SkillName.AnimalLore].Base;
			double taming = from.Skills[SkillName.AnimalTaming].Base;

			for ( int i = 0; i < entries.Length; ++i )
			{
				object name = entries[i].Name;

				bool enabled = ( lore >= entries[i].ReqAnimalLore && taming >= entries[i].ReqAnimalTaming );

				AddButton( 27, 53 + (i * 21), 9702, 9703, i + 1, GumpButtonType.Reply, 0 );

				if ( name is int )
					AddHtmlLocalized( 50, 51 + (i * 21), 150, 20, (int)name, enabled ? EnabledColor16 : DisabledColor16, false, false );
				else if ( name is string )
					AddHtml( 50, 51 + (i * 21), 150, 20, String.Format( "<BASEFONT COLOR=#{0:X6}>{1}</BASEFONT>", enabled ? EnabledColor32 : DisabledColor32, name ), false, false );
			}
		}

		private static Hashtable m_Table = new Hashtable();

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			int index = info.ButtonID - 1;

			if ( index >= 0 && index < m_Entries.Length )
			{
				RangerFamiliarEntry entry = m_Entries[index];

				double lore = m_From.Skills[SkillName.AnimalLore].Base;
				double taming = m_From.Skills[SkillName.AnimalTaming].Base;

				BaseCreature check = (BaseCreature)RangerFamiliarSpell.Table[m_From];

				if ( check != null && !check.Deleted )
				{
					m_From.SendLocalizedMessage( 1061605 ); // You already have a familiar.
				}
				else if ( lore < entry.ReqAnimalLore || taming < entry.ReqAnimalTaming )
				{
					// That familiar requires ~1_NECROMANCY~ Necromancy and ~2_SPIRIT~ Spirit Speak.
					m_From.SendMessage( String.Format( "That familiar requires {0:F1} Animal Lore and {1:F1} Animal Taming.", entry.ReqAnimalLore, entry.ReqAnimalTaming ) );

					m_From.CloseGump( typeof( RangerFamiliarGump ) );
					m_From.SendGump( new RangerFamiliarGump( m_From, RangerFamiliarSpell.Entries ) );
				}
				else if ( entry.Type == null )
				{
					m_From.SendMessage( "That familiar has not yet been defined." );

					m_From.CloseGump( typeof( RangerFamiliarGump ) );
					m_From.SendGump( new RangerFamiliarGump( m_From, RangerFamiliarSpell.Entries ) );
				}
				else
				{
					try
					{
						BaseCreature bc = (BaseCreature)Activator.CreateInstance( entry.Type );

						bc.Skills.MagicResist = m_From.Skills.MagicResist;

						if ( BaseCreature.Summon( bc, m_From, m_From.Location, -1, TimeSpan.FromDays( 1.0 ) ) )
						{
							m_From.FixedParticles( 0x3728, 1, 10, 9910, EffectLayer.Head );
							bc.PlaySound( bc.GetIdleSound() );
							RangerFamiliarSpell.Table[m_From] = bc;
						}
					}
					catch
					{
					}
				}
			}
			else
			{
				m_From.SendLocalizedMessage( 1061825 ); // You decide not to summon a familiar.
			}
		}
	}
}
