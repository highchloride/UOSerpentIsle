using System;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Misc
{
    public class LevelEquipXMLDynamicDynamicAttachToEquipment
    {		
        public static void Initialize()
        {
            EventSink.OnItemObtained += new OnItemObtainedEventHandler(EventSink_OnItemObtained);	
            EventSink.ItemCreated += new ItemCreatedEventHandler(EventSink_ItemCreated);	
            EventSink.OnItemUse += new OnItemUseEventHandler(EventSink_OnItemUse);	
        }

        private static void EventSink_OnItemObtained(OnItemObtainedEventArgs e)
        {
			ConfiguredEquipment cfe = new ConfiguredEquipment();
            Item item = e.Item;
			Mobile m = e.Mobile;
			
			if (cfe.AttachOnEquipCreateDynamicSystem == false)
				return;
			
			if (m is PlayerMobile)
			{
				if (item is BaseArmor || item is BaseWeapon || item is BaseClothing || item is BaseJewel)
				{
					PlayerMobile pm = (PlayerMobile)m;

					LevelEquipXMLDynamic xmleqiip = (LevelEquipXMLDynamic)XmlAttach.FindAttachment(item, typeof(LevelEquipXMLDynamic));
					if (xmleqiip == null)
					{
						XmlAttach.AttachTo(item, new LevelEquipXMLDynamic());
					}
				}
			}
        }
        private static void EventSink_ItemCreated(ItemCreatedEventArgs e)
        {
			ConfiguredEquipment cfe = new ConfiguredEquipment();
            Item item = e.Item;
			
			if (cfe.AttachOnEquipCreateDynamicSystem == false)
				return;
			
			if (item is BaseArmor || item is BaseWeapon || item is BaseClothing || item is BaseJewel)
			{
				LevelEquipXMLDynamic xmleqiip = (LevelEquipXMLDynamic)XmlAttach.FindAttachment(item, typeof(LevelEquipXMLDynamic));
				if (xmleqiip == null)
				{
					XmlAttach.AttachTo(item, new LevelEquipXMLDynamic());
				}
			}
        }
        private static void EventSink_OnItemUse(OnItemUseEventArgs e)
        {
			ConfiguredEquipment cfe = new ConfiguredEquipment();
            Item item = e.Item;
			
			if (cfe.AttachOnEquipCreateDynamicSystem == false)
				return;
			
			if (item is BaseArmor || item is BaseWeapon || item is BaseClothing || item is BaseJewel)
			{
				LevelEquipXMLDynamic xmleqiip = (LevelEquipXMLDynamic)XmlAttach.FindAttachment(item, typeof(LevelEquipXMLDynamic));
				if (xmleqiip == null)
				{
					XmlAttach.AttachTo(item, new LevelEquipXMLDynamic());
				}
			}
        }
    }
}