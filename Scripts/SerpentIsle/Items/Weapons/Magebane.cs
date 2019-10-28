using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Mobiles;

namespace Server.Items
{
    class Magebane : Scimitar
    {
        /// <summary>
        /// The Magebane sword is a scimitar that sets the opponent's mana to 0 on hit. It isn't really that impressive otherwise.
        /// 10/2 - Now it burns mana. Impact halved against PCs.
        /// This is based directly on the U7 weapon.
        /// </summary>
        [Constructable]
        public Magebane() : base()
        {
            this.Weight = 6.0;
            Name = "Magebane";
            Hue = 0xBD;
            Attributes.BonusStr = 5;
            Attributes.AttackChance = 5;
            Attributes.WeaponSpeed = 5;
            Attributes.WeaponDamage = 25;
        }

        public Magebane(Serial serial) : base(serial)
        { }

        public override void OnHit(Mobile attacker, IDamageable damageable, double damageBonus)
        {
            int mana = ((Mobile)damageable).Mana;
            int hits = ((Mobile)damageable).Hits;

            if(mana > 0 && damageable is PlayerMobile)
            {
                damageable.PlaySound(0x1E9);
                damageable.FixedParticles(0x375A, 10, 15, 5037, EffectLayer.Waist);
                mana -= 40;
                hits -= 20;
            }
            else if(mana > 0)
            {
                damageable.PlaySound(0x1E9);
                damageable.FixedParticles(0x375A, 10, 15, 5037, EffectLayer.Waist);
                mana -= 80;
                hits -= 40;
            }

            //Prevents setting negative values
            ((Mobile)damageable).Mana = Math.Max(0, mana);
            ((Mobile)damageable).Hits = Math.Max(0, hits);

            base.OnHit(attacker, damageable, damageBonus);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
