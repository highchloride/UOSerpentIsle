using Server.Targeting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Items
{
    public class SerpentTooth : Item
    {
        private SerpentsTeeth m_Tooth;

        [CommandProperty(AccessLevel.Seer)]
        public SerpentsTeeth Tooth
        {
            get { return m_Tooth; }
            set { m_Tooth = value;  }
        }

        [Constructable]
        public SerpentTooth() : base(0x5747)
        {
            Name = "Serpent Tooth";
        }

        public SerpentTooth(Serial serial)
            : base(serial)
        {

        }

        //public override bool OnDragDrop(Mobile from, Item dropped)
        //{
        //    SerpentJawbone bone;
        //    //Process if dropped on a Jawbone
        //    string type = dropped.GetType().ToString();
        //    from.SendMessage("Dropped on " + type);

        //    if(dropped.GetType() == typeof(SerpentJawbone))
        //    {
        //        bone = dropped as SerpentJawbone;
        //        bone.AddToJawbone(from, this);
        //        return true;
        //    }

        //    return base.OnDragDrop(from, dropped);
        //}

        public override void OnDoubleClick(Mobile from)
        {
            SerpentJawbone bone;
            JawboneTarget t;
            if(IsChildOf(from.Backpack) || Parent == from)
            {
                from.BeginTarget(3, false, TargetFlags.None, new TargetCallback(OnTarget));
                from.SendMessage("Select the Jawbone to add this tooth to.");
                //t = new JawboneTarget();
                //if(t.bone != null)
                //{
                //    t.bone.AddToJawbone(from, this);
                //}                
            }

            base.OnDoubleClick(from);
        }

        public virtual void OnTarget(Mobile from, Object obj)
        {
            SerpentJawbone jawbone = null;

            if (this.Deleted)
                return;

            if(obj is SerpentJawbone)
                jawbone = obj as SerpentJawbone;

            if (jawbone == null)
            {
                from.SendMessage("This can only be used with a Serpent's Jawbone.");
            }
            else
            {
                if(jawbone.AddToJawbone(this.m_Tooth))
                {
                    from.SendMessage("Thou hast added the tooth to thy Jawbone.");
                    this.Delete();
                }
                else
                {
                    from.SendMessage("This tooth is already in thy Jawbone.");
                }
            }
        }

        //Detect if the target is a serpent jawbone or not
        private class JawboneTarget : Target
        {
            public SerpentJawbone bone = null;
            public SerpentTooth Tooth = null;
            public JawboneTarget(SerpentTooth tooth) : base(5, false, TargetFlags.None)
            {
                Tooth = tooth;
                bone = new SerpentJawbone();
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (!(targeted is SerpentJawbone))
                {
                    from.SendMessage("Teeth can only used on a Serpent Jawbone.");
                }
                else if (targeted is SerpentJawbone)
                {
                    if(Tooth != null && bone != null)
                    {
                        //bone.AddToJawbone(from, Tooth);
                        

                        targeted.CallMethod("AddToJawbone", new object[] { from, Tooth });
                        from.SendMessage("This should add to the jawbone!");
                    }
                    else
                    {
                        Console.WriteLine("HOW DID THIS HAPPEN");
                    }
                    //base.OnTarget(from, targeted);
                }
                else
                {
                    base.OnTarget(from, targeted);
                }
            }
        }

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
