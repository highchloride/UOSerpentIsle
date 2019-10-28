using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Mobiles;

namespace Server.SerpentIsle
{
    class FarrahSI : TalkingBaseCreature
    {
        private static bool m_Talked;

        string[] kfcsay = new string[]
        {
            "Walk softly, we do not know what lairs we will travel.",
            "My name is Farrah, and I practice not being seen.",
            "Byron's cape makes a great distraction, even if he dost not know it.",
            "I hope thou dost not plan to make so much noise once we are through.",
            "One can accomplish much from the shadows.",
        };

        string[] sicsay = new string[]
        {
            "This may have been a mistake.",
            "I am starting to doubt Triplicate's abilities.",
            "Now that we are here, what do we do?",
            "Perhaps there are people somewhere nearby.",
        };

        [Constructable]
        public FarrahSI() : base(AIType.AI_Melee, FightMode.None, 10, 1, 1, 1)
        {
            InitBody();
            InitOutfit();
        }

        public FarrahSI(Serial serial) : base(serial)
        {
        }

        public void InitBody()
        {
            Name = "Farrah";
            Title = "of the Shadows";
            SpeechHue = Utility.RandomDyedHue();
            Body = 0x191;
            Hue = 33815;

            SetStr(35);
            SetDex(42);
            SetInt(35);

            SetFameLevel(1500);
            SetKarmaLevel(-800);

            SetSkill(SkillName.Hiding, 40.0, 45.0);
            SetSkill(SkillName.Stealing, 40.0, 45.0);
        }

        public void InitOutfit()
        {
            Item hair = new Item(8265);
            hair.Hue = 1109;
            hair.Layer = Layer.Hair;
            hair.Movable = false;
            AddItem(hair);

            Item dagger = new Item(3922);
            dagger.Hue = 0;
            dagger.Movable = false;
            dagger.Layer = Layer.OneHanded;
            AddItem(dagger);

            Item bonnet = new Item(5913);
            bonnet.Hue = 156;
            bonnet.Movable = false;
            bonnet.Layer = Layer.Helm;
            AddItem(bonnet);

            Item shirt = new Item(5399);
            shirt.Hue = 111;
            shirt.Movable = false;
            shirt.Layer = Layer.InnerTorso;
            AddItem(shirt);

            Item shortPants = new Item(5422);
            shortPants.Hue = 0;
            shortPants.Movable = false;
            shortPants.Layer = Layer.InnerLegs;
            AddItem(shortPants);

            Item shoes = new Item(5901);
            shoes.Hue = 0;
            shoes.Movable = false;
            shoes.Layer = Layer.Shoes;
            AddItem(shoes);

            PackGold(50, 200);
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            //base.OnMovement(m, oldLocation);
            if (m_Talked == false)
            {
                if (m.InRange(this, 5))
                {
                    m_Talked = true;
                    SayRandom(sicsay, this);
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
