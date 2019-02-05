using Server;
using System;
using System.Collections.Generic;

namespace Server.Engines.Yahtzee
{
    [PropertyObject]
    public class Roll
    {
        public static Roll Zero { get { return new Roll(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public RollEntry One { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public RollEntry Two { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public RollEntry Three { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public RollEntry Four { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public RollEntry Five { get; set; }

        public Roll(int one = 0, int two = 0, int three = 0, int four = 0, int five = 0)
        {
            One = new RollEntry(one);
            Two = new RollEntry(two);
            Three = new RollEntry(three);
            Four = new RollEntry(four);
            Five = new RollEntry(five);
        }

        public void Reroll()
        {
            if (!One.Set) One.Roll = Utility.RandomMinMax(1, 6);
            if (!Two.Set) Two.Roll = Utility.RandomMinMax(1, 6);
            if (!Three.Set) Three.Roll = Utility.RandomMinMax(1, 6);
            if (!Four.Set) Four.Roll = Utility.RandomMinMax(1, 6);
            if (!Five.Set) Five.Roll = Utility.RandomMinMax(1, 6);
        }

        public void Clear()
        {
            One.Set = false;
            Two.Set = false;
            Three.Set = false;
            Four.Set = false;
            Five.Set = false;

            One.Roll = Two.Roll = Three.Roll = Four.Roll = Five.Roll = 0;
        }

        public int[] ToArray()
        {
            return new int[] { One.Roll, Two.Roll, Three.Roll, Four.Roll, Five.Roll };
        }

        public bool HasRolled(int num)
        {
            return One.Roll == num || Two.Roll == num || Three.Roll == num || Four.Roll == num || Five.Roll == num;
        }

        public override string ToString()
        {
            return String.Format("{0}-{1}-{2}-{3}-{4}", One.Roll, Two.Roll, Three.Roll, Four.Roll, Five.Roll);
        }

        public Roll(GenericReader reader)
        {
            int version = reader.ReadInt();

            One = new RollEntry(reader.ReadInt(), reader.ReadBool());
            Two = new RollEntry(reader.ReadInt(), reader.ReadBool());
            Three = new RollEntry(reader.ReadInt(), reader.ReadBool());
            Four = new RollEntry(reader.ReadInt(), reader.ReadBool());
            Five = new RollEntry(reader.ReadInt(), reader.ReadBool());
        }

        public void Serialize(GenericWriter writer)
        {
            writer.Write(0);

            writer.Write(One.Roll);
            writer.Write(One.Set);

            writer.Write(Two.Roll);
            writer.Write(Two.Set);

            writer.Write(Three.Roll);
            writer.Write(Three.Set);

            writer.Write(Four.Roll);
            writer.Write(Four.Set);

            writer.Write(Five.Roll);
            writer.Write(Five.Set);
        }
    }

    [PropertyObject]
    public class RollEntry
    {
        [CommandProperty(AccessLevel.GameMaster)]
        public int Roll { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Set { get; set; }

        public RollEntry(int roll, bool set = false)
        {
            Roll = roll;
            Set = set;
        }

        public override string ToString()
        {
            if (Set)
                return Roll.ToString() + "- Set";

            return Roll.ToString() + "- Not Set";
        }
    }
}