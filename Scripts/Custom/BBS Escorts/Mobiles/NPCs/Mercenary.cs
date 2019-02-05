using System;
using System.Collections.Generic;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    public class Mercenary : BaseCreature
    {
        [Constructable]
        public Mercenary()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Title = "the Mercenary";

            this.SpeechHue = Utility.RandomDyedHue();

            this.Hue = Utility.RandomSkinHue();

            this.Female = false;
            this.Body = 0x190;
            this.Name = NameList.RandomName("male");

            Utility.AssignRandomHair(this);
            Utility.AssignRandomFacialHair(this, this.HairHue);

            this.AddItem(new ThighBoots(0x1BB));
            this.AddItem(new LeatherChest());
            this.AddItem(new LeatherArms());
            this.AddItem(new LeatherLegs());
            this.AddItem(new LeatherCap());
            this.AddItem(new LeatherGloves());
            this.AddItem(new LeatherGorget());

            Item weapon;
            switch ( Utility.Random(6) )
            {
                case 0:
                    weapon = new Broadsword();
                    break;
                case 1:
                    weapon = new Cutlass();
                    break;
                case 2:
                    weapon = new Katana();
                    break;
                case 3:
                    weapon = new Longsword();
                    break;
                case 4:
                    weapon = new Scimitar();
                    break;
                default:
                    weapon = new VikingSword();
                    break;
            }
            weapon.Movable = false;
            this.AddItem(weapon);

            Item shield = new BronzeShield();
            shield.Movable = false;
            this.AddItem(shield);

            this.SetStr(150);
            this.SetDex(100);
            this.SetInt(25);

            this.SetHits(150);

            this.SetDamage(8, 15);

            this.SetDamageType(ResistanceType.Physical, 100);

            this.SetResistance(ResistanceType.Physical, 50, 60);
            this.SetResistance(ResistanceType.Fire, 45, 55);
            this.SetResistance(ResistanceType.Cold, 50, 60);
            this.SetResistance(ResistanceType.Poison, 40, 50);
            this.SetResistance(ResistanceType.Energy, 45, 55);

            this.SetSkill(SkillName.MagicResist, 65.1, 80.0);
            this.SetSkill(SkillName.Anatomy, 65.1, 80.0);
            this.SetSkill(SkillName.Healing, 65.1, 80.0);
            this.SetSkill(SkillName.Swords, 65.1, 80.0);
            this.SetSkill(SkillName.Tactics, 65.1, 80.0);
            this.SetSkill(SkillName.Parry, 65.1, 80.0);

            this.Fame = 3000;
            this.Karma = 3000;

            this.VirtualArmor = 25;

            new InternalTimer(this).Start();
        }

        public override bool CanHealOwner
        {
            get
            {
                return true;
            }
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            if (this.Map != null && attacker != this && 0.75 > Utility.RandomDouble())
            {
                if (attacker is BaseCreature)
                {
                    BaseCreature pet = (BaseCreature)attacker;
                    if (pet.ControlMaster != null && (attacker is BaseCreature))//Dragon || attacker is GreaterDragon || attacker is SkeletalDragon || attacker is WhiteWyrm || attacker is Drake))
                    {
                        this.Combatant = null;
                        pet.Combatant = null;
                        this.Combatant = null;
                        pet.ControlMaster = null;
                        pet.Controlled = false;
                        attacker.Emote(String.Format("* {0} decided to go wild *", attacker.Name));
                    }

                    if (pet.ControlMaster != null && 0.9 > Utility.RandomDouble())
                    {
                        this.Combatant = null;
                        pet.Combatant = pet.ControlMaster;
                        this.Combatant = null;
                        attacker.Emote(String.Format("* {0} is being angered *", attacker.Name));
                    }
                }
            }

            base.OnGotMeleeAttack(attacker);
        }


        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            BaseCreature c = defender as BaseCreature;

            if (c is BaseCreature && c.Controlled)
            {
                if (0.25 >= Utility.RandomDouble())
                    DoSpecial(defender);
            }
            else
            {
                DebugSay("Not a pet, just attack I shall!");
            }
        }

        public void DoSpecial(Mobile m)
        {
            PublicOverheadMessage(MessageType.Emote, 0x47E, true, "*The fearsome mercenary easily dispatches the beast!*");
            m.PlaySound(0x44A);
            m.Kill();
        }

        public Mercenary(Serial serial)
            : base(serial)
        {
        }

        private class InternalTimer : Timer  		//<---------- Timer Code Start (13 Lines Total)
        {
            private Mobile m_Mobile;
            public InternalTimer(Mobile mobile)
                : base(TimeSpan.FromSeconds(900.0))//15 min.
            {
                m_Mobile = mobile;
                Priority = TimerPriority.TwoFiftyMS;
            }
            protected override void OnTick()
            {
                m_Mobile.Delete();
            }
        }								


        public override bool ClickTitle
        {
            get
            {
                return false;
            }
        }
       /* public override bool IsEnemy(Mobile m)
        {
            if (m.Player || m is BaseVendor)
                return false;

            if (m is BaseCreature)
            {
                BaseCreature bc = (BaseCreature)m;

                Mobile master = bc.GetMaster();
                if (master != null)
                    return this.IsEnemy(master);
            }

            return m.Karma < 0;
        }*/

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            new InternalTimer(this).Start();
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class MercenaryCorpse : Corpse
    {
        public MercenaryCorpse(Mobile owner, HairInfo hair, FacialHairInfo facialhair, List<Item> equipItems)
            : base(owner, hair, facialhair, equipItems)
        {
        }

        public MercenaryCorpse(Serial serial)
            : base(serial)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            if (this.ItemID == 0x2006) // Corpse form
            {
                list.Add("a human corpse");
                list.Add(1049318, this.Name); // the remains of ~1_NAME~ the mercenary
            }
            else
            {
                list.Add("the remains of a mercenary"); 
            }
        }

        public override void OnSingleClick(Mobile from)
        {
            int hue = Notoriety.GetHue(Server.Misc.NotorietyHandlers.CorpseNotoriety(from, this));

            if (this.ItemID == 0x2006) // Corpse form
                from.Send(new MessageLocalized(this.Serial, this.ItemID, MessageType.Label, hue, 3, 1049318, "", this.Name)); // the remains of ~1_NAME~ the militia fighter
            else
                from.Send(new MessageLocalized(this.Serial, this.ItemID, MessageType.Label, hue, 3, 1049319, "", "")); // the remains of a militia fighter
        }

        public override void Open(Mobile from, bool checkSelfLoot)
        {
            if (from.InRange(this.GetWorldLocation(), 2))
            {
                from.SendMessage(0x22, "You find nothing useful on this corpse."); 
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
