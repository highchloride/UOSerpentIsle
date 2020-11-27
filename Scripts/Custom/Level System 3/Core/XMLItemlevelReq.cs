using System;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Misc
{
    public class XMLItemlevelReq
    {
        public static void Initialize()
        {
            EventSink.OnItemObtained += new OnItemObtainedEventHandler(EventSink_OnItemObtained);	
            EventSink.ItemCreated += new ItemCreatedEventHandler(EventSink_ItemCreated);	
            EventSink.OnItemUse += new OnItemUseEventHandler(EventSink_OnItemUse);	
        }

        private static void EventSink_OnItemObtained(OnItemObtainedEventArgs e)
        {
			ConfiguredEquipment c = new ConfiguredEquipment();
            Item item = e.Item;
			Mobile m = e.Mobile;
			
			if (c.AttachOnEquipCreate == false)
				return;
			
			if (m is PlayerMobile)
			{
				if (item is BaseArmor || item is BaseWeapon)
				{
					PlayerMobile pm = (PlayerMobile)m;

					LevelEquipXML xmleqiip = (LevelEquipXML)XmlAttach.FindAttachment(item, typeof(LevelEquipXML));
					if (xmleqiip == null)
					{
						XmlAttach.AttachTo(item, new LevelEquipXML());
					}
				}
			}
        }
        private static void EventSink_ItemCreated(ItemCreatedEventArgs e)
        {
			ConfiguredEquipment c = new ConfiguredEquipment();
            Item item = e.Item;
			
			if (c.AttachOnEquipCreate == false)
				return;
			
			if (item is BaseArmor || item is BaseWeapon)
			{
				LevelEquipXML xmleqiip = (LevelEquipXML)XmlAttach.FindAttachment(item, typeof(LevelEquipXML));
				if (xmleqiip == null)
				{
					XmlAttach.AttachTo(item, new LevelEquipXML());
				}
			}
        }
        private static void EventSink_OnItemUse(OnItemUseEventArgs e)
        {
			ConfiguredEquipment c = new ConfiguredEquipment();
            Item item = e.Item;
			
			if (c.AttachOnEquipCreate == false)
				return;
			
			if (item is BaseArmor || item is BaseWeapon)
			{
				LevelEquipXML xmleqiip = (LevelEquipXML)XmlAttach.FindAttachment(item, typeof(LevelEquipXML));
				if (xmleqiip == null)
				{
					XmlAttach.AttachTo(item, new LevelEquipXML());
				}
			}
        }
    }
}