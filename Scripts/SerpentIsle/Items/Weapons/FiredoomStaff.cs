using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Items
{
    class FiredoomStaff : BaseStaff, IUsesRemaining
    {
        [Constructable]
        public FiredoomStaff() : base(0xDF0)
        {
            this.Weight = 6.0;
            this.Hue = 1263;
            this.UsesRemaining = 20;
        }

        public FiredoomStaff(Serial serial) : base(serial)
        { }

        //public override WeaponAbility PrimaryAbility
        //{
        //    get
        //    {
        //        return WeaponAbility.FiredoomBlast;
        //    }
        //}        

        public override void OnDoubleClick(Mobile from)
        {

            base.OnDoubleClick(from);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
