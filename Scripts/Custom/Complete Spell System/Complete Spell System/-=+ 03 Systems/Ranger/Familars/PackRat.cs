using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.ContextMenus;
using Server.Items;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;

namespace Server.ACC.CSS.Systems.Ranger
{
    [CorpseName("a pack rat corpse")]
    public class PackRatFamiliar : BaseFamiliar
    {
        public PackRatFamiliar()
        {
            Name = "a pack rat";
            Body = 0xD7;
            BaseSoundID = 0x188;

            SetStr(32, 74);
            SetDex(46, 65);
            SetInt(16, 30);

            SetHits(26, 39);
            SetMana(0);

            SetDamage(4, 8);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 15, 20);
            SetResistance(ResistanceType.Fire, 5, 10);
            SetResistance(ResistanceType.Poison, 25, 35);

            SetSkill(SkillName.MagicResist, 25.1, 30.0);
            SetSkill(SkillName.Tactics, 29.3, 44.0);
            SetSkill(SkillName.Wrestling, 29.3, 44.0);

            Fame = 300;
            Karma = -300;

            VirtualArmor = 18;

            ControlSlots = 1;

            Container pack = Backpack;

            if (pack != null)
                pack.Delete();

            pack = new StrongBackpack();
            pack.Movable = false;

            AddItem(pack);
        }

        private DateTime m_NextPickup;

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.Now < m_NextPickup)
                return;

            m_NextPickup = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 10));

            Container pack = this.Backpack;

            if (pack == null)
                return;

            ArrayList list = new ArrayList();

            foreach (Item item in this.GetItemsInRange(2))
            {
                if (item.Movable && item.Stackable)
                    list.Add(item);
            }

            int pickedUp = 0;

            for (int i = 0; i < list.Count; ++i)
            {
                Item item = (Item)list[i];

                if (!pack.CheckHold(this, item, false, true))
                    return;

                bool rejected;
                LRReason reject;

                NextActionTime = DateTime.Now.Ticks;

                Lift(item, item.Amount, out rejected, out reject);

                if (rejected)
                    continue;

                Drop(this, Point3D.Zero);

                if (++pickedUp == 3)
                    break;
            }
        }

        private void ConfirmRelease_Callback(Mobile from, bool okay, object state)
        {
            if (okay)
                EndRelease(from);
        }

        public override void BeginRelease(Mobile from)
        {
            Container pack = this.Backpack;

            if (pack != null && pack.Items.Count > 0)
                from.SendGump(new WarningGump(1060635, 30720, 1061672, 32512, 420, 280, new WarningGumpCallback(ConfirmRelease_Callback), null));
            else
                EndRelease(from);
        }

        #region Pack Animal Methods
        public override bool OnBeforeDeath()
        {
            if (!base.OnBeforeDeath())
                return false;

            PackAnimal.CombineBackpacks(this);

            return true;
        }

        public override DeathMoveResult GetInventoryMoveResultFor(Item item)
        {
            return DeathMoveResult.MoveToCorpse;
        }

        public override bool IsSnoop(Mobile from)
        {
            if (PackAnimal.CheckAccess(this, from))
                return false;

            return base.IsSnoop(from);
        }

        public override bool OnDragDrop(Mobile from, Item item)
        {
            if (CheckFeed(from, item))
                return true;

            if (PackAnimal.CheckAccess(this, from))
            {
                AddToBackpack(item);
                return true;
            }

            return base.OnDragDrop(from, item);
        }

        public override bool CheckNonlocalDrop(Mobile from, Item item, Item target)
        {
            return PackAnimal.CheckAccess(this, from);
        }

        public override bool CheckNonlocalLift(Mobile from, Item item)
        {
            return PackAnimal.CheckAccess(this, from);
        }

        public override void OnDoubleClick(Mobile from)
        {
            PackAnimal.TryPackOpen(this, from);
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            PackAnimal.GetContextMenuEntries(this, from, list);
        }
        #endregion

        public PackRatFamiliar(Serial serial)
            : base(serial)
        {
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
