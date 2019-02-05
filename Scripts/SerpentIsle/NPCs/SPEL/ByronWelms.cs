using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Mobiles;

namespace Server.SerpentIsle
{
    class ByronWelms : BaseVendor
    {
        private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();

        protected override List<SBInfo> SBInfos
        {
            get
            {
                return m_SBInfos;
            }
        }
        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBProvisioner());
        }
        private static bool m_Talked;

        string[] kfcsay = new string[]
        {
            "Welcome to our grand expedition!",
            "Byron Welms, at your service.",
            "Britannians will speak of us for ages after this!",
            "Be certain you have everything you need for the trip - we'll have little to spare.",
            "This will be but one of my many successful expeditions.",
        };

        [Constructable]
        public ByronWelms() : base("the Great Explorer")//base(AIType.AI_Melee, FightMode.None, 10, 1, 1, 1)
        {

        }

        public ByronWelms(Serial serial) : base(serial)
        {
        }
               
        public override void InitBody()
        {
            Title = "the Great Explorer";

            SpeechHue = Utility.RandomDyedHue();
            Body = 0x190;
            Hue = 33804;

            SetStr(37);
            SetDex(36);
            SetInt(38);

            SetFameLevel(1500);
            SetKarmaLevel(-800);

            SetSkill(SkillName.Cartography, 40.0, 45.0);
            SetSkill(SkillName.Veterinary, 40.0, 45.0);
        }

        public override void InitOutfit()
        {
            Item hair = new Item(8253);
            hair.Hue = 1146;
            hair.Layer = Layer.Hair;
            hair.Movable = false;
            AddItem(hair);

            Item quarterStaff = new Item(3721);
            quarterStaff.Hue = 0;
            quarterStaff.Movable = false;
            quarterStaff.Layer = Layer.OneHanded;
            AddItem(quarterStaff);

            Item featheredHat = new Item(5914);
            featheredHat.Hue = 0;
            featheredHat.Movable = false;
            featheredHat.Layer = Layer.Helm;
            AddItem(featheredHat);

            Item cloak = new Item(5397);
            cloak.Hue = 238;
            cloak.Movable = false;
            cloak.Layer = Layer.Cloak;
            AddItem(cloak);

            Item fancyShirt = new Item(7933);
            fancyShirt.Hue = 941;
            fancyShirt.Movable = false;
            fancyShirt.Layer = Layer.Shirt;
            AddItem(fancyShirt);

            Item doublet = new Item(8059);
            doublet.Hue = 238;
            doublet.Movable = false;
            doublet.Layer = Layer.MiddleTorso;
            AddItem(doublet);

            Item leatherLegs = new Item(5067);
            leatherLegs.Hue = 0;
            leatherLegs.Movable = false;
            leatherLegs.Layer = Layer.OuterLegs;
            AddItem(leatherLegs);

            Item shoes = new Item(5901);
            shoes.Hue = 0;
            shoes.Movable = false;
            shoes.Layer = Layer.Shoes;
            AddItem(shoes);
        }

        public override void TurnToTokuno()
        {
            Name = "Byron Welms";
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            //base.OnMovement(m, oldLocation);
            if (m_Talked == false)
            {
                if (m.InRange(this, 5))
                {
                    m_Talked = true;
                    SayRandom(kfcsay, this);
                    this.Move(GetDirectionTo(m.Location));
                    SpamTimer t = new SpamTimer();
                    t.Start();
                }
            }
        }

        private class SpamTimer : Timer
        {
            public SpamTimer() : base(TimeSpan.FromSeconds(45))
            {
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                m_Talked = false;
            }
        }

        private static void SayRandom(string[] say, Mobile m)
        {
            m.Say(say[Utility.Random(say.Length)]);
        }

        private static int GetRandomHue()
        {
            switch (Utility.Random(6))
            {
                default:
                case 0: return 0;
                case 1: return Utility.RandomBlueHue();
                case 2: return Utility.RandomGreenHue();
                case 3: return Utility.RandomRedHue();
                case 4: return Utility.RandomYellowHue();
                case 5: return Utility.RandomNeutralHue();
            }

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
