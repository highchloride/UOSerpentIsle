using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Engines.XmlSpawner2
{
    public class BonusStatAtt : XmlAttachment
    {		
		private TimeSpan m_Duration = TimeSpan.FromSeconds(1.0); 
		
        public BonusStatAtt(ASerial serial) : base(serial)
        {
        }

        [Attachable]
        public BonusStatAtt()
        {
			Expiration = TimeSpan.FromMinutes(5);
        }

        [Attachable]
        public BonusStatAtt(double seconds, double duration)
        {
			Expiration = TimeSpan.FromMinutes(duration);
			m_Duration = TimeSpan.FromSeconds(duration);
        }
		
		public override void OnAttach()
		{
			base.OnAttach();
			if(AttachedTo is PlayerMobile)
			{
				((PlayerMobile)AttachedTo).AddStatMod(new StatMod(StatType.Dex, "XmlDex" + Name, 25, TimeSpan.Zero));
				((PlayerMobile)AttachedTo).AddStatMod(new StatMod(StatType.Str, "XmlStr" + Name, 25, TimeSpan.Zero));
				((PlayerMobile)AttachedTo).AddStatMod(new StatMod(StatType.Int, "XmlInt" + Name, 25, TimeSpan.Zero));
				((PlayerMobile)AttachedTo).SendMessage("Your loyal pet imbues you with its blood lust!");
				InvalidateParentProperties();
			}
			else
				Delete();
		}
		public override void OnDelete()
		{
			Configured c = new Configured();
			base.OnDelete();
			if(AttachedTo is PlayerMobile)
			{
				/* Sanity Check, ensure the StatMod is Removed */
				((PlayerMobile)AttachedTo).AddStatMod(new StatMod(StatType.Dex, "XmlDex" + Name, c.StatBonusDex, m_Duration ));
				((PlayerMobile)AttachedTo).AddStatMod(new StatMod(StatType.Str, "XmlStr" + Name, c.StatBonusStr, m_Duration ));
				((PlayerMobile)AttachedTo).AddStatMod(new StatMod(StatType.Int, "XmlInt" + Name, c.StatBonusInt, m_Duration ));
				InvalidateParentProperties();
			}
		}
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize(writer);
			writer.Write( (int) 0 );
			// version 
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			// version 0
		}
		
    }
}
