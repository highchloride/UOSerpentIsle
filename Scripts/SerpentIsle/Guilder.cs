using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Accounting;

namespace Server.Items
{
    class Guilder : Item
    {
        [Constructable]
        public Guilder()
                    : this(1)
        {
        }

        [Constructable]
        public Guilder(int amountFrom, int amountTo)
            : this(Utility.RandomMinMax(amountFrom, amountTo))
        {
        }

        [Constructable]
        public Guilder(int amount)
            : base(0x10A7)
        {
            this.Stackable = true;
            this.Amount = amount;
            this.Name = "Guilder";
        }

        public Guilder(Serial serial)
            : base(serial)
        {
        }

        public override double DefaultWeight
        {
            get
            {
                return (Core.ML ? (0.02 / 3) : 0.02);
            }
        }

        public override int GetDropSound()
        {
                return 0x04F; //Rustle kinda sounds like paper, Guilder are magicy bills
        }

        public override int GetTotal(TotalType type)
        {
            int baseTotal = base.GetTotal(type);

            if (type == TotalType.Items)
                baseTotal += this.Amount;

            return baseTotal;
        }

        public override bool StackWith(Mobile from, Item dropped, bool playSound)
        {
            if(dropped is Guilder)
            {
                return base.StackWith(from, dropped, playSound);
            }
            else
            {
                return false;
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

        protected override void OnAmountChange(int oldValue)
        {
            int newValue = this.Amount;

            this.UpdateTotal(this, TotalType.Items, newValue - oldValue);
        }
    }
}
