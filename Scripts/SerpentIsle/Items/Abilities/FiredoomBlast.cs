using System;
using System.Collections.Generic;
using Server.Spells;

namespace Server.Items
{
    public class FiredoomBlast : WeaponAbility
    {
        public FiredoomBlast()
        { }

        public override bool ConsumeAmmo
        {
            get
            {
                return false;
            }
        }

    }
}
