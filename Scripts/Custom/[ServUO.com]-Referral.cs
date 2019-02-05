using Server;
using System;

namespace Server.Items
{

    public class ReferralSash : BodySash
    {
        public override int InitMinHits { get { return 255; } }
        public override int InitMaxHits { get { return 255; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Referral
        {
            get { return _Referral; }
            set
            {
                _Referral = value;
                Attributes.Luck = GetLuckAmount(_Referral);
                InvalidateProperties();
            }
        }

        private int _Referral;


        [Constructable]
        public ReferralSash() : base()
        {
            Name = "A Referral Sash";
            Attributes.Luck = 25;

        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            list.Add("I've referred {0} players to the server", _Referral);
        }

        public int GetLuckAmount(int referral)
        {
            return (25 + (_Referral * 2));
        }

        public ReferralSash(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
            writer.Write((int)_Referral);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            _Referral = reader.ReadInt();

        }
    }

}