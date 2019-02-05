using System;
using System.Collections.Generic;
using Server.ContextMenus;
using Server.Engines.XmlSpawner2;
using Server.Gumps;
using Server.Multis;
using Server.Network;

namespace Server.Items
{
    /// <summary>
    /// NAH 
    /// </summary>
    [Flipable(0x2AF9, 0x2AFD)]
    public class MusicPlayer : Item
    {
        [Constructable]
        public MusicPlayer()
            : base(0x2AF9)
        {
            this.Weight = 1.0;

        }

        public MusicPlayer(Serial serial)
            : base(serial)
        {
        }

        //This thing is gonna need a cost or something, can't have players willy-nilly changing the song on each other
        public override void OnDoubleClick(Mobile from)
        {
            //XmlMusic a = (XmlMusic)XmlAttach.FindAttachment(this, typeof(XmlMusic));

            //if (a != null)
            //{
            //    a.OnTrigger(this, from);
            //}
        }

        

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version


        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();


        }

        private void Animate()
        {
            //this.m_Count++;

            //if (this.m_Count >= 4)
            //{
            //    this.m_Count = 0;
            //    this.ItemID = this.m_ItemID;
            //}
            //else
            //    this.ItemID++;
        }
    }
}
