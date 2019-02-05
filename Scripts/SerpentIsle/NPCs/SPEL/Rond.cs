using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Mobiles;

namespace Server.SerpentIsle
{
    public class Rond : BaseVendor
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
            m_SBInfos.Add(new SBWeaponSmith());
            m_SBInfos.Add(new SBLeatherArmor());
            m_SBInfos.Add(new SBWoodenShields());
        }

        private static bool m_Talked;

        string[] kfcsay = new string[]
        {
            "More meat for the grinder!",
            "My name is Rond, Champion of the Trinsic Fighting Pits.",
            "I've been paid well to guard this expedition.",
            "Surely you do not intend to travel with such poor weapons?",
            "Of course there are fighting pits in Trinsic! You must not have visited in a while!",
        };

        [Constructable]
        public Rond() : base("the Pit-Fighter")//base(AIType.AI_Melee, FightMode.None, 10, 1, 1, 1)
        {

        }

        public Rond(Serial serial) : base(serial)
        {
        }

        public override void InitBody()
        {
            Title = "the Pit-Fighter";

            SpeechHue = Utility.RandomDyedHue();
            Body = 0x190;
            Hue = 33786;

            SetStr(43);
            SetDex(38);
            SetInt(30);

            SetFameLevel(1500);
            SetKarmaLevel(-800);

            SetSkill(SkillName.Swords, 40.0, 45.0);
            SetSkill(SkillName.Tactics, 40.0, 45.0);
        }

        public override void InitOutfit()
        {
            Item hair = new Item(8251);
            hair.Hue = 1118;
            hair.Layer = Layer.Hair;
            hair.Movable = false;
            AddItem(hair);

            Item maul = new Item(5179);
            maul.Hue = 0;
            maul.Movable = false;
            maul.Layer = Layer.OneHanded;
            AddItem(maul);

            Item shield = new Item(7026);
            shield.Hue = 0;
            shield.Movable = false;
            shield.Layer = Layer.TwoHanded;
            AddItem(shield);

            Item studdedChest = new Item(5083);
            studdedChest.Hue = 0;
            studdedChest.Movable = false;
            studdedChest.Layer = Layer.MiddleTorso;
            AddItem(studdedChest);

            Item studdedGorget = new Item(5078);
            studdedGorget.Hue = 0;
            studdedGorget.Movable = false;
            studdedGorget.Layer = Layer.Neck;
            AddItem(studdedGorget);

            Item bodySash = new Item(5441);
            bodySash.Hue = 148;
            bodySash.Movable = false;
            bodySash.Layer = Layer.OuterTorso;
            AddItem(bodySash);

            Item studdedArms = new Item(5084);
            studdedArms.Hue = 0;
            studdedArms.Movable = false;
            studdedArms.Layer = Layer.Arms;
            AddItem(studdedArms);

            Item leatherLegs = new Item(5067);
            leatherLegs.Hue = 0;
            leatherLegs.Movable = false;
            leatherLegs.Layer = Layer.OuterLegs;
            AddItem(leatherLegs);

            Item shoes = new Item(5903);
            shoes.Hue = 789;
            shoes.Movable = false;
            shoes.Layer = Layer.Shoes;
            AddItem(shoes);

            PackGold(50, 200);
        }

        public override void TurnToTokuno()
        {
            Name = "Rond";
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
