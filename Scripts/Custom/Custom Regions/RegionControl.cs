//Cleaned up and .net 4.0'ized by Tresdni
//http://www.uobloodoath.com
#region References

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Server.Gumps;
using Server.Regions;
using Server.Spells;

#endregion

namespace Server.Items
{
    [Flags]
    public enum RegionFlag : uint
    {
        None = 0x00000000,
        AllowBenefitPlayer = 0x00000001,
        AllowHarmPlayer = 0x00000002,
        AllowHousing = 0x00000004,
        AllowSpawn = 0x00000008,

        CanBeDamaged = 0x00000010,
        CanHeal = 0x00000020,
        CanRessurect = 0x00000040,
        CanUseStuckMenu = 0x00000080,
        ItemDecay = 0x00000100,

        ShowEnterMessage = 0x00000200,
        ShowExitMessage = 0x00000400,

        AllowBenefitNPC = 0x00000800,
        AllowHarmNPC = 0x00001000,

        CanMountEthereal = 0x000002000,
        // ToDo: Change to "CanEnter"
        CanEnter = 0x000004000,

        CanLootPlayerCorpse = 0x000008000,
        CanLootNPCCorpse = 0x000010000,
        // ToDo: Change to "CanLootOwnCorpse"
        CanLootOwnCorpse = 0x000020000,

        CanUsePotions = 0x000040000,

        IsGuarded = 0x000080000,

        EmptyNPCCorpse = 0x000400000,
        EmptyPlayerCorpse = 0x000800000,
        DeleteNPCCorpse = 0x001000000,
        DeletePlayerCorpse = 0x002000000,
        ResNPCOnDeath = 0x004000000,
        ResPlayerOnDeath = 0x008000000,
        MoveNPCOnDeath = 0x010000000,
        MovePlayerOnDeath = 0x020000000,

        NoPlayerItemDrop = 0x040000000,
        NoNPCItemDrop = 0x080000000
    }

    public sealed class RegionControl : Item
    {
        private static List<RegionControl> m_AllControls = new List<RegionControl>();

        public static List<RegionControl> AllControls
        {
            get { return m_AllControls; }
        }

        #region Region Flags

        private RegionFlag m_Flags;

        private bool GetFlag(RegionFlag flag)
        {
            return ((m_Flags & flag) != 0);
        }

        private void SetFlag(RegionFlag flag, bool value)
        {
            if (value)
            {
                m_Flags |= flag;
            }
            else
            {
                m_Flags &= ~flag;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool AllowBenefitPlayer
        {
            get { return GetFlag(RegionFlag.AllowBenefitPlayer); }
            set { SetFlag(RegionFlag.AllowBenefitPlayer, value); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool AllowHarmPlayer
        {
            get { return GetFlag(RegionFlag.AllowHarmPlayer); }
            set { SetFlag(RegionFlag.AllowHarmPlayer, value); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool AllowHousing
        {
            get { return GetFlag(RegionFlag.AllowHousing); }
            set { SetFlag(RegionFlag.AllowHousing, value); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool AllowSpawn
        {
            get { return GetFlag(RegionFlag.AllowSpawn); }
            set { SetFlag(RegionFlag.AllowSpawn, value); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool CanBeDamaged
        {
            get { return GetFlag(RegionFlag.CanBeDamaged); }
            set { SetFlag(RegionFlag.CanBeDamaged, value); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool CanMountEthereal
        {
            get { return GetFlag(RegionFlag.CanMountEthereal); }
            set { SetFlag(RegionFlag.CanMountEthereal, value); }
        }

        // ToDo: Change to "CanEnter"
        [CommandProperty(AccessLevel.GameMaster)]
        public bool CanEnter
        {
            get { return GetFlag(RegionFlag.CanEnter); }
            private set { SetFlag(RegionFlag.CanEnter, value); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool CanHeal
        {
            get { return GetFlag(RegionFlag.CanHeal); }
            set { SetFlag(RegionFlag.CanHeal, value); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool CanRessurect
        {
            get { return GetFlag(RegionFlag.CanRessurect); }
            set { SetFlag(RegionFlag.CanRessurect, value); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool CanUseStuckMenu
        {
            get { return GetFlag(RegionFlag.CanUseStuckMenu); }
            set { SetFlag(RegionFlag.CanUseStuckMenu, value); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool ItemDecay
        {
            get { return GetFlag(RegionFlag.ItemDecay); }
            set { SetFlag(RegionFlag.ItemDecay, value); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool AllowBenefitNPC
        {
            get { return GetFlag(RegionFlag.AllowBenefitNPC); }
            set { SetFlag(RegionFlag.AllowBenefitNPC, value); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool AllowHarmNPC
        {
            get { return GetFlag(RegionFlag.AllowHarmNPC); }
            set { SetFlag(RegionFlag.AllowHarmNPC, value); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool ShowEnterMessage
        {
            get { return GetFlag(RegionFlag.ShowEnterMessage); }
            set { SetFlag(RegionFlag.ShowEnterMessage, value); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool ShowExitMessage
        {
            get { return GetFlag(RegionFlag.ShowExitMessage); }
            set { SetFlag(RegionFlag.ShowExitMessage, value); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool CanLootPlayerCorpse
        {
            get { return GetFlag(RegionFlag.CanLootPlayerCorpse); }
            set { SetFlag(RegionFlag.CanLootPlayerCorpse, value); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool CanLootNPCCorpse
        {
            get { return GetFlag(RegionFlag.CanLootNPCCorpse); }
            set { SetFlag(RegionFlag.CanLootNPCCorpse, value); }
        }

        // ToDo: Change to "CanLootOwnCorpse"
        [CommandProperty(AccessLevel.GameMaster)]
        public bool CanLootOwnCorpse
        {
            get { return GetFlag(RegionFlag.CanLootOwnCorpse); }
            private set { SetFlag(RegionFlag.CanLootOwnCorpse, value); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool CanUsePotions
        {
            get { return GetFlag(RegionFlag.CanUsePotions); }
            set { SetFlag(RegionFlag.CanUsePotions, value); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsGuarded
        {
            get { return GetFlag(RegionFlag.IsGuarded); }
            set
            {
                SetFlag(RegionFlag.IsGuarded, value);
                if (Region != null)
                {
                    Region.Disabled = !value;
                }

                Timer.DelayCall(TimeSpan.FromSeconds(2.0), UpdateRegion);
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool EmptyNPCCorpse
        {
            get { return GetFlag(RegionFlag.EmptyNPCCorpse); }
            set { SetFlag(RegionFlag.EmptyNPCCorpse, value); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool EmptyPlayerCorpse
        {
            get { return GetFlag(RegionFlag.EmptyPlayerCorpse); }
            set { SetFlag(RegionFlag.EmptyPlayerCorpse, value); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool DeleteNPCCorpse
        {
            get { return GetFlag(RegionFlag.DeleteNPCCorpse); }
            private set { SetFlag(RegionFlag.DeleteNPCCorpse, value); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool DeletePlayerCorpse
        {
            get { return GetFlag(RegionFlag.DeletePlayerCorpse); }
            private set { SetFlag(RegionFlag.DeletePlayerCorpse, value); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool ResNPCOnDeath
        {
            get { return GetFlag(RegionFlag.ResNPCOnDeath); }
            set { SetFlag(RegionFlag.ResNPCOnDeath, value); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool ResPlayerOnDeath
        {
            get { return GetFlag(RegionFlag.ResPlayerOnDeath); }
            set { SetFlag(RegionFlag.ResPlayerOnDeath, value); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool MoveNPCOnDeath
        {
            get { return GetFlag(RegionFlag.MoveNPCOnDeath); }
            set
            {
                if (MoveNPCToMap == null || MoveNPCToMap == Map.Internal || MoveNPCToLoc == Point3D.Zero)
                {
                    SetFlag(RegionFlag.MoveNPCOnDeath, false);
                }
                else
                {
                    SetFlag(RegionFlag.MoveNPCOnDeath, value);
                }
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool MovePlayerOnDeath
        {
            get { return GetFlag(RegionFlag.MovePlayerOnDeath); }
            set
            {
                if (MovePlayerToMap == null || MovePlayerToMap == Map.Internal || MovePlayerToLoc == Point3D.Zero)
                {
                    SetFlag(RegionFlag.MovePlayerOnDeath, false);
                }
                else
                {
                    SetFlag(RegionFlag.MovePlayerOnDeath, value);
                }
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool NoPlayerItemDrop
        {
            get { return GetFlag(RegionFlag.NoPlayerItemDrop); }
            private set { SetFlag(RegionFlag.NoPlayerItemDrop, value); }
        }


        [CommandProperty(AccessLevel.GameMaster)]
        public bool NoNPCItemDrop
        {
            get { return GetFlag(RegionFlag.NoNPCItemDrop); }
            private set { SetFlag(RegionFlag.NoNPCItemDrop, value); }
        }

        # endregion

        #region Region Restrictions

        public BitArray RestrictedSpells { get; private set; }

        public BitArray RestrictedSkills { get; private set; }

        # endregion

        # region Region Related Objects

        private Rectangle3D[] m_RegionArea;

        public CustomRegion Region { get; private set; }

        public Rectangle3D[] RegionArea
        {
            get { return m_RegionArea; }
            set { m_RegionArea = value; }
        }

        # endregion

        # region Control Properties

        private bool m_Active = true;

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Active
        {
            get { return m_Active; }
            set
            {
                if (m_Active == value)
                {
                    return;
                }
                m_Active = value;
                UpdateRegion();
            }
        }

        # endregion

        # region Region Properties

        private string m_RegionName;
        private int m_RegionPriority;
        private MusicName m_Music;
        private TimeSpan m_PlayerLogoutDelay;
        private int m_LightLevel;

        private Map m_MoveNPCToMap;
        private Point3D m_MoveNPCToLoc;
        private Map m_MovePlayerToMap;
        private Point3D m_MovePlayerToLoc;

        [CommandProperty(AccessLevel.GameMaster)]
        public string RegionName
        {
            get { return m_RegionName; }
            set
            {
                if (Map != null && !RegionNameTaken(value))
                {
                    m_RegionName = value;
                }
                else if (Map != null)
                {
                    Console.WriteLine("RegionName not changed for {0}, {1} already has a Region with the name of {2}",
                        this, Map, value);
                }
                else if (Map == null)
                {
                    Console.WriteLine("RegionName not changed for {0} to {1}, it's Map value was null", this, value);
                }

                UpdateRegion();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int RegionPriority
        {
            get { return m_RegionPriority; }
            set
            {
                m_RegionPriority = value;
                UpdateRegion();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public MusicName Music
        {
            get { return m_Music; }
            set
            {
                m_Music = value;
                UpdateRegion();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan PlayerLogoutDelay
        {
            get { return m_PlayerLogoutDelay; }
            set
            {
                m_PlayerLogoutDelay = value;
                UpdateRegion();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int LightLevel
        {
            get { return m_LightLevel; }
            set
            {
                m_LightLevel = value;
                UpdateRegion();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Map MoveNPCToMap
        {
            get { return m_MoveNPCToMap; }
            set
            {
                if (value != Map.Internal)
                {
                    m_MoveNPCToMap = value;
                }
                else
                {
                    SetFlag(RegionFlag.MoveNPCOnDeath, false);
                }
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D MoveNPCToLoc
        {
            get { return m_MoveNPCToLoc; }
            set
            {
                if (value != Point3D.Zero)
                {
                    m_MoveNPCToLoc = value;
                }
                else
                {
                    SetFlag(RegionFlag.MoveNPCOnDeath, false);
                }
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Map MovePlayerToMap
        {
            get { return m_MovePlayerToMap; }
            set
            {
                if (value != Map.Internal)
                {
                    m_MovePlayerToMap = value;
                }
                else
                {
                    SetFlag(RegionFlag.MovePlayerOnDeath, false);
                }
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D MovePlayerToLoc
        {
            get { return m_MovePlayerToLoc; }
            set
            {
                if (value != Point3D.Zero)
                {
                    m_MovePlayerToLoc = value;
                }
                else
                {
                    SetFlag(RegionFlag.MovePlayerOnDeath, false);
                }
            }
        }

        private Point3D m_CustomGoLocation;

        [CommandProperty(AccessLevel.GameMaster)]
        private Point3D CustomGoLocation
        {
            get { return Region.GoLocation; }
            set
            {
                Region.GoLocation = value;
                m_CustomGoLocation = value;
                UpdateRegion();
            }
        }

        # endregion

        private bool Initialized;

        [Constructable]
        public RegionControl() : base(5609)
        {
            Visible = false;
            Movable = false;
            Name = "Region Controller";

            if (m_AllControls == null)
            {
                m_AllControls = new List<RegionControl>();
            }
            m_AllControls.Add(this);

            m_RegionName = FindNewName("Custom Region");
            m_RegionPriority = Server.Region.DefaultPriority;

            RestrictedSpells = new BitArray(SpellRegistry.Types.Length);
            RestrictedSkills = new BitArray(SkillInfo.Table.Length);

            DoChooseArea(null, Map, Location, Location, this);


            UpdateRegion();
        }


        [Constructable]
        public RegionControl(Rectangle2D rect) : base(5609)
        {
            Visible = false;
            Movable = false;
            Name = "Region Controller";

            if (m_AllControls == null)
            {
                m_AllControls = new List<RegionControl>();
            }
            m_AllControls.Add(this);

            m_RegionName = FindNewName("Custom Region");
            m_RegionPriority = Server.Region.DefaultPriority;

            RestrictedSpells = new BitArray(SpellRegistry.Types.Length);
            RestrictedSkills = new BitArray(SkillInfo.Table.Length);

            Rectangle3D newrect = Server.Region.ConvertTo3D(rect);
            DoChooseArea(null, Map, newrect.Start, newrect.End, this);

            if (Region != null)
            {
                Region.GoLocation = new Point3D(0, 0, 0);
            }

            UpdateRegion();
        }

        [Constructable]
        public RegionControl(Rectangle3D rect) : base(5609)
        {
            Visible = false;
            Movable = false;
            Name = "Region Controller";

            if (m_AllControls == null)
            {
                m_AllControls = new List<RegionControl>();
            }
            m_AllControls.Add(this);

            m_RegionName = FindNewName("Custom Region");
            m_RegionPriority = Server.Region.DefaultPriority;

            RestrictedSpells = new BitArray(SpellRegistry.Types.Length);
            RestrictedSkills = new BitArray(SkillInfo.Table.Length);

            DoChooseArea(null, this.Map, rect.Start, rect.End, this);

            if (Region != null)
            {
                Region.GoLocation = new Point3D(0, 0, 0);
            }

            UpdateRegion();
        }

        [Constructable]
        public RegionControl(IEnumerable<Rectangle2D> rects) : base(5609)
        {
            Visible = false;
            Movable = false;
            Name = "Region Controller";

            if (m_AllControls == null)
            {
                m_AllControls = new List<RegionControl>();
            }
            m_AllControls.Add(this);

            m_RegionName = FindNewName("Custom Region");
            m_RegionPriority = Server.Region.DefaultPriority;

            RestrictedSpells = new BitArray(SpellRegistry.Types.Length);
            RestrictedSkills = new BitArray(SkillInfo.Table.Length);

            foreach (Rectangle3D newrect in rects.Select(Server.Region.ConvertTo3D))
            {
                DoChooseArea(null, Map, newrect.Start, newrect.End, this);
            }

            if (Region != null)
            {
                Region.GoLocation = new Point3D(0, 0, 0);
            }

            UpdateRegion();
        }

        [Constructable]
        public RegionControl(IEnumerable<Rectangle3D> rects) : base(5609)
        {
            Visible = false;
            Movable = false;
            Name = "Region Controller";

            if (m_AllControls == null)
            {
                m_AllControls = new List<RegionControl>();
            }
            m_AllControls.Add(this);

            m_RegionName = FindNewName("Custom Region");
            m_RegionPriority = Server.Region.DefaultPriority;

            RestrictedSpells = new BitArray(SpellRegistry.Types.Length);
            RestrictedSkills = new BitArray(SkillInfo.Table.Length);

            foreach (Rectangle3D rect3d in rects)
            {
                DoChooseArea(null, this.Map, rect3d.Start, rect3d.End, this);
            }

            if (Region != null)
            {
                Region.GoLocation = new Point3D(0, 0, 0);
            }

            UpdateRegion();
        }

        public RegionControl(Serial serial) : base(serial)
        {
        }

        #region Control Special Voids

        private bool RegionNameTaken(string testName)
        {
            return m_AllControls != null && m_AllControls.Any(control => control.RegionName == testName && control != this);
        }

        private string FindNewName(string oldName)
        {
            int i = 1;

            string newName = oldName;
            while (RegionNameTaken(newName))
            {
                newName = oldName;
                newName += String.Format(" {0}", i);
                i++;
            }

            return newName;
        }

        private void UpdateRegion()
        {
            if (Region != null)
            {
                if (!Initialized)
                {
                    Region.GoLocation = new Point3D(0, 0, 0);
                    Initialized = true;
                }
                Region.Unregister();
            }

            if (this.Map != null && this.Active)
            {
                if (this != null && this.RegionArea != null && this.RegionArea.Length > 0)
                {
                    Region = new CustomRegion(this) {GoLocation = m_CustomGoLocation};

                    Region.Register();
                }
                else
                {
                    Region = null;
                }
            }
            else
            {
                Region = null;
            }
        }

        public void RemoveArea(int index, Mobile from)
        {
            try
            {
                var rects = m_RegionArea.ToList();

                rects.RemoveAt(index);
                m_RegionArea = rects.ToArray();

                UpdateRegion();
                if (from != null)
                {
                    from.SendMessage("Area Removed!");
                }
            }
            catch
            {
                if (from != null)
                {
                    from.SendMessage("Removing of Area Failed!");
                }
            }
        }

        private static int GetRegistryNumber(ISpell s)
        {
            var t = SpellRegistry.Types;

            for (int i = 0; i < t.Length; i++)
            {
                if (s.GetType() == t[i])
                {
                    return i;
                }
            }

            return -1;
        }


        public bool IsRestrictedSpell(ISpell s)
        {
            if (RestrictedSpells.Length != SpellRegistry.Types.Length)
            {
                RestrictedSpells = new BitArray(SpellRegistry.Types.Length);

                for (int i = 0; i < RestrictedSpells.Length; i++)
                {
                    RestrictedSpells[i] = false;
                }
            }

            int regNum = GetRegistryNumber(s);


            return regNum >= 0 && RestrictedSpells[regNum];
        }

        public bool IsRestrictedSkill(int skill)
        {
            if (RestrictedSkills.Length == SkillInfo.Table.Length)
            {
                return skill >= 0 && RestrictedSkills[skill];
            }

            RestrictedSkills = new BitArray(SkillInfo.Table.Length);

            for (int i = 0; i < RestrictedSkills.Length; i++)
            {
                RestrictedSkills[i] = false;
            }

            return skill >= 0 && RestrictedSkills[skill];
        }

        public void ChooseArea(Mobile m)
        {
            BoundingBoxPicker.Begin(m, CustomRegion_Callback, this);
        }

        private void CustomRegion_Callback(Mobile from, Map map, Point3D start, Point3D end, object state)
        {
            DoChooseArea(from, map, start, end, state);
        }

        private void DoChooseArea(Mobile from, Map map, Point3D start, Point3D end, object control)
        {
            var areas = new List<Rectangle3D>();

            if (m_RegionArea != null)
            {
                areas.AddRange(m_RegionArea);
            }

            if (start.Z == end.Z || start.Z < end.Z)
            {
                if (start.Z != Server.Region.MinZ)
                {
                    --start.Z;
                }
                if (end.Z != Server.Region.MaxZ)
                {
                    ++end.Z;
                }
            }
            else
            {
                if (start.Z != Server.Region.MaxZ)
                {
                    ++start.Z;
                }
                if (end.Z != Server.Region.MinZ)
                {
                    --end.Z;
                }
            }

            Rectangle3D newrect = new Rectangle3D(start, end);
            areas.Add(newrect);

            m_RegionArea = areas.ToArray();

            UpdateRegion();
        }

        # endregion

        #region Control Overrides

        public override void OnDoubleClick(Mobile m)
        {
            if (m.AccessLevel < AccessLevel.GameMaster)
            {
                return;
            }

            if (RestrictedSpells.Length != SpellRegistry.Types.Length)
            {
                RestrictedSpells = new BitArray(SpellRegistry.Types.Length);

                for (int i = 0; i < RestrictedSpells.Length; i++)
                {
                    RestrictedSpells[i] = false;
                }

                m.SendMessage("Resetting all restricted Spells due to Spell change");
            }

            if (RestrictedSkills.Length != SkillInfo.Table.Length)
            {
                RestrictedSkills = new BitArray(SkillInfo.Table.Length);

                for (int i = 0; i < RestrictedSkills.Length; i++)
                {
                    RestrictedSkills[i] = false;
                }

                m.SendMessage("Resetting all restricted Skills due to Skill change");
            }


            m.CloseGump(typeof (RegionControlGump));
            m.SendGump(new RegionControlGump(this));
            m.SendMessage("Don't forget to props this object for more options!");
            m.CloseGump(typeof (RemoveAreaGump));
            m.SendGump(new RemoveAreaGump(this));
        }

        public override void OnMapChange()
        {
            UpdateRegion();
            base.OnMapChange();
        }

        public override void OnDelete()
        {
            if (Region != null)
            {
                Region.Unregister();
            }

            if (m_AllControls != null)
            {
                m_AllControls.Remove(this);
            }

            base.OnDelete();
        }

        # endregion

        #region Ser/Deser Helpers

        private static void WriteBitArray(GenericWriter writer, BitArray ba)
        {
            writer.Write(ba.Length);

            for (int i = 0; i < ba.Length; i++)
            {
                writer.Write(ba[i]);
            }
        }

        private static BitArray ReadBitArray(GenericReader reader)
        {
            int size = reader.ReadInt();

            BitArray newBA = new BitArray(size);

            for (int i = 0; i < size; i++)
            {
                newBA[i] = reader.ReadBool();
            }

            return newBA;
        }


        private static void WriteRect3DArray(GenericWriter writer, Rectangle3D[] ary)
        {
            if (ary == null)
            {
                writer.Write(0);
                return;
            }

            writer.Write(ary.Length);

            foreach (Rectangle3D rect in ary)
            {
                writer.Write(rect.Start);
                writer.Write(rect.End);
            }
        }

        private static IEnumerable<Rectangle2D> ReadRect2DArray(GenericReader reader)
        {
            int size = reader.ReadInt();
            var newAry = new List<Rectangle2D>();

            for (int i = 0; i < size; i++)
            {
                newAry.Add(reader.ReadRect2D());
            }

            return newAry;
        }

        private static Rectangle3D[] ReadRect3DArray(GenericReader reader)
        {
            int size = reader.ReadInt();
            var newAry = new List<Rectangle3D>();

            for (int i = 0; i < size; i++)
            {
                Point3D start = reader.ReadPoint3D();
                Point3D end = reader.ReadPoint3D();
                newAry.Add(new Rectangle3D(start, end));
            }

            return newAry.ToArray();
        }

        # endregion

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version

            writer.Write(CustomGoLocation);

            WriteRect3DArray(writer, m_RegionArea);

            writer.Write((int) m_Flags);

            WriteBitArray(writer, RestrictedSpells);
            WriteBitArray(writer, RestrictedSkills);

            writer.Write(m_Active);

            writer.Write(m_RegionName);
            writer.Write(m_RegionPriority);
            writer.Write((int) m_Music);
            writer.Write(m_PlayerLogoutDelay);
            writer.Write(m_LightLevel);

            writer.Write(m_MoveNPCToMap);
            writer.Write(m_MoveNPCToLoc);
            writer.Write(m_MovePlayerToMap);
            writer.Write(m_MovePlayerToLoc);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            Point3D customGoLoc = new Point3D(0, 0, 0);
            switch (version)
            {
                case 1:
                {
                    customGoLoc = reader.ReadPoint3D();
                    goto case 0;
                }
                case 0:
                {
                    m_RegionArea = ReadRect3DArray(reader);

                    m_Flags = (RegionFlag) reader.ReadInt();

                    RestrictedSpells = ReadBitArray(reader);
                    RestrictedSkills = ReadBitArray(reader);

                    m_Active = reader.ReadBool();

                    m_RegionName = reader.ReadString();
                    m_RegionPriority = reader.ReadInt();
                    m_Music = (MusicName) reader.ReadInt();
                    m_PlayerLogoutDelay = reader.ReadTimeSpan();
                    m_LightLevel = reader.ReadInt();

                    m_MoveNPCToMap = reader.ReadMap();
                    m_MoveNPCToLoc = reader.ReadPoint3D();
                    m_MovePlayerToMap = reader.ReadMap();
                    m_MovePlayerToLoc = reader.ReadPoint3D();

                    break;
                }         
            }

            m_AllControls.Add(this);

            if (RegionNameTaken(m_RegionName))
            {
                m_RegionName = FindNewName(m_RegionName);
            }

            UpdateRegion();
            m_CustomGoLocation = customGoLoc;
            CustomGoLocation = customGoLoc;
            UpdateRegion();
        }
    }
}