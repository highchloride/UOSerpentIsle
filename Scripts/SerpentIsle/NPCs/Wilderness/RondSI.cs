using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Mobiles;

namespace Server.SerpentIsle
{
    public class RondSI : TalkingBaseCreature
    {
        private static bool m_Talked;

        string[] sicsay = new string[]
        {
            "This place is freezing!",
            "I should've asked for more gold...",
            "At least the wolves look the same as in Britannia!",
            "Keep thy hands in plain view, stranger.",
        };

        [Constructable]
        public RondSI() : base(AIType.AI_Melee, FightMode.None, 10, 1, 1, 1)
        {
            InitBody();
            InitOutfit();
        }

        public RondSI(Serial serial) : base(serial)
        {
        }

        public void InitBody()
        {
            Name = "Rond";
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

        public void InitOutfit()
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
