using System;
using Server.Multis;
using Server.Regions;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.Ancient
{
    public enum HouseTrapStrength
    {
        Lesser = 1,
        Regular = 2,
        Greater = 3,
        Deadly = 4,
        None = 0
    }

    public enum HouseTrapType
    {
        Blades = 1,
        FireColumn = 2,
        Explosion = 3,
        Poison = 4
    }

    public class BaseHouseTrap : BaseTrap
    {
        public BaseHouseTrap(HouseTrapStrength p_Strength, HouseTrapType p_Type)
            : base(0x3133)
        {
            TrapType = p_Type;
            TrapStrength = p_Strength;
        }

        public BaseHouseTrap(Serial serial)
            : base(serial)
        {
        }

        private Mobile m_Placer;
        private bool m_Detected;
        private HouseTrapType m_TrapType;
        private HouseTrapStrength m_TrapStrength;

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Placer
        {
            get { return m_Placer; }
            set { m_Placer = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Detected
        {
            get { return m_Detected; }
            set { m_Detected = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public HouseTrapType TrapType
        {
            get { return m_TrapType; }
            set { m_TrapType = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public HouseTrapStrength TrapStrength
        {
            get { return m_TrapStrength; }
            set { m_TrapStrength = value; }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version 

            writer.Write((Mobile)m_Placer);
            writer.Write((int)TrapType);
            writer.Write((int)TrapStrength);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    {
                        m_Placer = reader.ReadMobile();
                        TrapType = (HouseTrapType)reader.ReadInt();
                        TrapStrength = (HouseTrapStrength)reader.ReadInt();
                        break;
                    }
            }

            ItemID = 12595;
        }

        public override bool PassivelyTriggered { get { return true; } }
        public override TimeSpan PassiveTriggerDelay { get { return TimeSpan.FromSeconds(2.0); } } // Two seconds to get the f**k off the trap
        public override int PassiveTriggerRange { get { return 0; } } // Have to be ON TOP of the trap to activate it..
        public override TimeSpan ResetDelay { get { return TimeSpan.FromSeconds(0.5); } } // Resets after half a second

        public override void OnTrigger(Mobile from) // Add Types of Trap Poison, Blade, Explosion etc...
        {
            if (from != null && m_Placer != null)
            {
                if (from != m_Placer && from.Z == this.Z && from.Alive && from.AccessLevel == AccessLevel.Player) // Must not be the placer, must be alive, and standing on the trap for it to work.
                {
                    if (m_Placer.GuildFealty != from.GuildFealty)
                    {
                        switch (TrapType)
                        {
                            case HouseTrapType.FireColumn:
                                Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 5052);
                                Effects.PlaySound(Location, Map, 0x225);
                                break;
                            case HouseTrapType.Blades:
                                Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x37A0, 10, 30, 5052);
                                Effects.PlaySound(Location, Map, 0x23A);
                                break;
                            case HouseTrapType.Poison:
                                Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x11A6, 10, 30, 5052);
                                Effects.PlaySound(Location, Map, 0x1DE);
                                break;
                            case HouseTrapType.Explosion:
                                Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x36BD, 10, 30, 5052);
                                Effects.PlaySound(Location, Map, 0x234);
                                break;
                        }

                        if (TrapType == HouseTrapType.Poison)
                            switch (TrapStrength)
                            {
                                case HouseTrapStrength.Lesser:
                                    m_Placer.DoHarmful(from);
                                    from.ApplyPoison(m_Placer, Poison.Lesser);
                                    break;
                                case HouseTrapStrength.Regular:
                                    m_Placer.DoHarmful(from);
                                    from.ApplyPoison(m_Placer, Poison.Regular);
                                    break;
                                case HouseTrapStrength.Greater:
                                    m_Placer.DoHarmful(from);
                                    from.ApplyPoison(m_Placer, Poison.Greater);
                                    break;
                                case HouseTrapStrength.Deadly:
                                    m_Placer.DoHarmful(from);
                                    from.ApplyPoison(m_Placer, Poison.Deadly);
                                    break;
                                case HouseTrapStrength.None:
                                    break;
                            }
                        else if (TrapType == HouseTrapType.Blades)
                            switch (TrapStrength)
                            {
                                case HouseTrapStrength.Lesser:
                                    m_Placer.DoHarmful(from);
                                    AOS.Damage(from, m_Placer, Utility.RandomMinMax(5, 20), 0, 100, 0, 0, 0);
                                    break;
                                case HouseTrapStrength.Regular:
                                    m_Placer.DoHarmful(from);
                                    AOS.Damage(from, m_Placer, Utility.RandomMinMax(10, 40), 0, 100, 0, 0, 0); break;
                                case HouseTrapStrength.Greater:
                                    m_Placer.DoHarmful(from);
                                    AOS.Damage(from, m_Placer, Utility.RandomMinMax(50, 100), 0, 100, 0, 0, 0); break;
                                case HouseTrapStrength.Deadly:
                                    m_Placer.DoHarmful(from);
                                    AOS.Damage(from, m_Placer, Utility.RandomMinMax(80, 120), 0, 100, 0, 0, 0); break;
                                case HouseTrapStrength.None:
                                    break;
                            }
                        else
                            switch (TrapStrength)
                            {
                                case HouseTrapStrength.Lesser:
                                    m_Placer.DoHarmful(from);
                                    AOS.Damage(m_Placer, from, Utility.RandomMinMax(5, 20), 100, 0, 0, 0, 0);
                                    break;
                                case HouseTrapStrength.Regular:
                                    m_Placer.DoHarmful(from);
                                    AOS.Damage(from, m_Placer, Utility.RandomMinMax(10, 40), 100, 0, 0, 0, 0); break;
                                case HouseTrapStrength.Greater:
                                    m_Placer.DoHarmful(from);
                                    AOS.Damage(from, m_Placer, Utility.RandomMinMax(50, 100), 100, 0, 0, 0, 0); break;
                                case HouseTrapStrength.Deadly:
                                    m_Placer.DoHarmful(from);
                                    AOS.Damage(from, m_Placer, Utility.RandomMinMax(80, 120), 100, 0, 0, 0, 0); break;
                                case HouseTrapStrength.None:
                                    break;
                            }

                        if (0.3 > Utility.RandomDouble())
                        {
                            m_Placer.SendMessage("A trap you placed has broken!");
                            Blood shards = new Blood();
                            shards.ItemID = 0xC2D;
                            shards.Map = this.Map;
                            shards.Location = this.Location;
                            Effects.PlaySound(this.Location, this.Map, 0x305);
                            this.Delete();
                        }
                    }
                }
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this.GetWorldLocation(), 1))
            {
                if (m_Placer == null || from == m_Placer)
                {
                    from.AddToBackpack(new HouseTrapDeed(TrapStrength, TrapType));

                    this.Delete();

                    from.SendMessage("You disassemble the trap.");
                }
                else
                {
                    from.SendMessage("You can not disassemble that trap.");
                }
            }
            else
            {
                from.SendLocalizedMessage(500446); // That is too far away.
            }
        }
    }

    public class HouseTrap : BaseHouseTrap
    {
        [Constructable]
        public HouseTrap(Mobile from, HouseTrapStrength p_Strength, HouseTrapType p_Type)
            : base(p_Strength, p_Type)
        {
            Name = "";
            Visible = false;

            switch (p_Strength)
            {
                case HouseTrapStrength.Lesser:
                    Name = Name + "Lesser";
                    break;
                case HouseTrapStrength.Regular:
                    Name = Name + "Regular";
                    break;
                case HouseTrapStrength.Greater:
                    Name = Name + "Greater";
                    break;
                case HouseTrapStrength.Deadly:
                    Name = Name + "Deadly";
                    break;
                case HouseTrapStrength.None:
                    Name = Name + "None";
                    break;
            }

            Name = Name + " ";

            switch (p_Type)
            {
                case HouseTrapType.Blades:
                    Name = Name + "Blade";
                    break;
                case HouseTrapType.FireColumn:
                    Name = Name + "Fire Column";
                    break;
                case HouseTrapType.Explosion:
                    Name = Name + "Explosion";
                    break;
                case HouseTrapType.Poison:
                    Name = Name + "Poison";
                    break;
            }

            Name = Name + " Trap";

            Placer = from;
            Movable = false;
            MoveToWorld(from.Location, from.Map);
        }

        public HouseTrap(Serial serial)
            : base(serial)
        {
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

    public class HouseTrapDeed : Item
    {
        private HouseTrapType m_TrapType;

        [CommandProperty(AccessLevel.GameMaster)]
        public HouseTrapType TrapType
        {
            get { return m_TrapType; }
            set { m_TrapType = value; }
        }

        private HouseTrapStrength m_TrapStrength;

        [CommandProperty(AccessLevel.GameMaster)]
        public HouseTrapStrength TrapStrength
        {
            get { return m_TrapStrength; }
            set { m_TrapStrength = value; }
        }

        [Constructable]
        public HouseTrapDeed(HouseTrapStrength p_Strength, HouseTrapType p_Type)
            : base(0x14F0)
        {
            Name = "a ";

            switch (p_Strength)
            {
                case HouseTrapStrength.Lesser:
                    Name = Name + "Lesser";
                    break;
                case HouseTrapStrength.Regular:
                    Name = Name + "Regular";
                    break;
                case HouseTrapStrength.Greater:
                    Name = Name + "Greater";
                    break;
                case HouseTrapStrength.Deadly:
                    Name = Name + "Deadly";
                    break;
                case HouseTrapStrength.None:
                    Name = Name + "None";
                    break;
            }

            Name = Name + " ";

            switch (p_Type)
            {
                case HouseTrapType.Blades:
                    Name = Name + "Blade";
                    break;
                case HouseTrapType.FireColumn:
                    Name = Name + "Fire Column";
                    break;
                case HouseTrapType.Explosion:
                    Name = Name + "Explosion";
                    break;
                case HouseTrapType.Poison:
                    Name = Name + "Poison";
                    break;
            }

            Name = Name + " Trap Deed";

            m_TrapType = p_Type;
            m_TrapStrength = p_Strength;

            Weight = 1.0;
        }

        public HouseTrapDeed(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
            writer.Write((int)TrapType);
            writer.Write((int)TrapStrength);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            switch (version)
            {
                case 1:
                    m_TrapType = (HouseTrapType)reader.ReadInt();
                    m_TrapStrength = (HouseTrapStrength)reader.ReadInt();
                    break;
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            Container pack = from.Backpack;

            if (from.InRange(this.GetWorldLocation(), 1))
            {
                if (pack != null && IsChildOf(pack))
                {
                    if (!SpellHelper.IsTown(from.Location, from))
                    {
                        if (!NonTrapLocations(from))
                        {
                            this.Delete();
                            new HouseTrap(from, TrapStrength, TrapType);
                        }
                        else
                        {
                            from.SendMessage("You cannot place that there.");
                        }
                    }
                    else
                    {
                        from.SendMessage("!     , .");
                    }
                }
                else
                {
                    from.SendLocalizedMessage(1060640); // Must be in backpack...
                }
            }
            else
            {
                from.SendLocalizedMessage(500446); // That is too far away.
            }
        }

        public bool NonTrapLocations(Mobile from)
        {
            Map map = from.Map;

            if (map == null)
                return false;

            IPooledEnumerable eable = map.GetItemsInRange(from.Location, 0);

            foreach (Item item in eable)
            {
                if ((item.Z + 16) > from.Z && (from.Z + 16) > item.Z && item.ItemID == 0x1BBF)
                {
                    eable.Free();
                    return true;
                }
            }

            eable.Free();

            return false;
        }

    }
}
