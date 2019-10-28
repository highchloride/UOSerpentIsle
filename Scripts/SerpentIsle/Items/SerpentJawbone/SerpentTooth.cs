using Server.Targeting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Items
{
    public class SerpentTooth : Item
    {
        [Constructable]
        public SerpentTooth() : base(0x5747)
        {
            Name = "Serpent Tooth";
            
        }

        public SerpentTooth(Serial serial)
            : base(serial)
        {

        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack) || Parent == from)
            {
                from.BeginTarget(3, false, TargetFlags.None, new TargetCallback(OnTarget));
                //from.Target = new TargetCallback(OnTarget);
                from.SendMessage("Select the Jawbone to add this tooth to.");
            }
            else
            {
                from.SendMessage("That must be in your pack to use it.");
            }

            base.OnDoubleClick(from);
        }

        public virtual void OnTarget(Mobile from, object obj)
        {
            var jawbone = obj as SerpentJawbone;

            if (this.Deleted)
                return;

            if(obj == null)
            {
                from.SendMessage("This can only be used with a Serpent's Jawbone.");
                return;
            }
            
            if (!jawbone.SerpentTeeth.Contains(this))
            {
                jawbone.SerpentTeeth.Add(this);
                jawbone.InvalidateProperties();
                from.SendMessage("Thou hast added the tooth to thy Jawbone.");
                this.Delete();
            }
            else
            {
                from.SendMessage("This tooth is already in thy Jawbone.");
            }
        }

        //public override bool OnDroppedOnto(Mobile from, Item target)
        //{
        //    var jawbone = target as SerpentJawbone;
        //    if (this.Deleted)
        //        return false;

        //    if(jawbone == null)
        //    {
        //        from.SendMessage("This can only be added to a Serpent Jawbone.");
        //        return false;
        //    }

        //    if (!jawbone.SerpentTeeth.Contains(this))
        //    {
        //        jawbone.SerpentTeeth.Add(this);
        //        from.SendMessage("Thou hast added the tooth to thy Jawbone.");
        //        this.Delete();
        //        return true;
        //    }
        //    else
        //    {
        //        from.SendMessage("This tooth is already in thy Jawbone.");
        //        return false;
        //    }

        //}

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();
        }
    }    
}

