using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Mobiles;

namespace Server.SerpentIsle
{
    public class TriplicateSI : TalkingBaseCreature
    {
        private static bool m_Talked;

        string[] sicsay = new string[]
        {
            "I appear to have misplaced my spellbook.",
            "The journey was harrowing. Where did we go between Britannia and here?",
            "Certainly my abilities will protect us here.",
            "How did I get talked into this?",
            "The look on Rond's face is making me nervous.",
        };

        [Constructable]
        public TriplicateSI() : base(AIType.AI_Melee, FightMode.None, 10, 1, 1, 1)
        {
            InitBody();
            InitOutfit();
        }

        public TriplicateSI(Serial serial) : base(serial)
        {
        }

        public void InitBody()
        {
            Name = "Triplicate";
            Title = "the Sage";
            
            SpeechHue = Utility.RandomDyedHue();
            Body = 0x190;
            Hue = 33791;

            SetStr(30);
            SetDex(35);
            SetInt(42);

            SetFameLevel(1500);
            SetKarmaLevel(-800);

            SetSkill(SkillName.Alchemy, 40.0, 45.0);
            SetSkill(SkillName.Magery, 40.0, 45.0);
        }

        public void InitOutfit()
        {
            Item hair = new Item(8252);
            hair.Hue = 1146;
            hair.Layer = Layer.Hair;
            hair.Movable = false;
            AddItem(hair);

            Item fHair = new Item(8256);
            fHair.Hue = 1146;
            fHair.Layer = Layer.FacialHair;
            fHair.Movable = false;
            AddItem(fHair);

            Item spellBook = new Item(3834);
            spellBook.Hue = 0;
            spellBook.Movable = false;
            spellBook.Layer = Layer.OneHanded;
            AddItem(spellBook);

            Item robe = new Item(7939);
            robe.Hue = 289;
            robe.Movable = false;
            robe.Layer = Layer.OuterTorso;
            AddItem(robe);

            Item shoes = new Item(5903);
            shoes.Hue = 1705;
            shoes.Movable = false;
            shoes.Layer = Layer.Shoes;
            AddItem(shoes);
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            //base.OnMovement(m, oldLocation);
            if(m_Talked == false)
            {
                if(m.InRange(this, 5))
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
            switch(Utility.Random(6))
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
