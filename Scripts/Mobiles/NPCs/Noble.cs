using System;
using Server.Items;
#region BBS Quests
using Server;
using EDI = Server.Mobiles.EscortDestinationInfo;
using Server.Multis;
#endregion

namespace Server.Mobiles
{
    public class Noble : BaseEscortable
    {

        #region BBS Quests [Locations]
        //UOSI Changed both of these to reflect my towns
        private static readonly string[] m_MLTownNames = new string[]
        {
            "Monitor", "Fawn", "Sleeping Bull", "Moonshade"

        };
        private static readonly string[] m_MLDestinations = new string[]
        {
            "Monitor", "Fawn", "Sleeping Bull", "Moonshade"
        };
        #endregion

     #region BBS Quests  
  [Constructable]
        public Noble()
            : this(null)
       {
       }

        public Noble(BaseCamp c)
            : base(c)
        {
     #endregion

            if (this.Female)
                this.Title = "the noblewoman";
            else
                this.Title = "the nobleman";

            //this.Title = "the noble";

            this.SetSkill(SkillName.Parry, 80.0, 100.0);
            this.SetSkill(SkillName.Swords, 80.0, 100.0);
            this.SetSkill(SkillName.Tactics, 80.0, 100.0);
        }

        public Noble(Serial serial)
            : base(serial)
        {
        }

        public override bool CanTeach
        {
            get
            {
                return true;
            }
        }
        public override bool ClickTitle
        {
            get
            {
                return false;
            }
        }// Do not display 'the noble' when single-clicking

        public override string[] GetPossibleDestinations()
        {
            

            #region BBS Quests
            if (this.IsPrisoner)
                return m_MLDestinations;

            else
                return m_MLTownNames;
            #endregion
        }

        public override void InitOutfit()
        {
            if (this.Female)
                this.AddItem(new FancyDress());
            else
                this.AddItem(new FancyShirt(GetRandomHue()));

            int lowHue = GetRandomHue();

            this.AddItem(new ShortPants(lowHue));

            if (this.Female)
                this.AddItem(new ThighBoots(lowHue));
            else
                this.AddItem(new Boots(lowHue));

            if (!this.Female)
                this.AddItem(new BodySash(lowHue));

            this.AddItem(new Cloak(GetRandomHue()));

            if (!this.Female)
                this.AddItem(new Longsword());

            Utility.AssignRandomHair(this);

            this.PackGold(200, 250);
        }

        public override void OnDeath(Container c)
        {

            if (0.01 > Utility.RandomDouble())
                c.DropItem(new Sandals(1676));//Abysmal Green
            else
                c.DropItem(new Sandals(Utility.RandomGreenHue()));//greens

            base.OnDeath(c);
        }

        public override void OnDamagedBySpell(Mobile caster)
        {
            if (caster == this)
                return;

            this.SpawnMercenary(caster);
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            SpawnMercenary(attacker);

        }

        public void SpawnMercenary(Mobile target)
        {
            Map map = target.Map;

            if (map == null)
                return;

            int mercenarys = 0;

            foreach (Mobile m in this.GetMobilesInRange(3))
            {
                if (m is Mercenary)
                    ++mercenarys;
            }

            if (mercenarys < 2)
            {
                BaseCreature mercenary = new Mercenary();

                mercenary.Team = this.Team;

                Point3D loc = target.Location;
                bool validLocation = false;

                for (int j = 0; !validLocation && j < 7; ++j)
                {
                    int x = target.X + Utility.Random(3) - 1;
                    int y = target.Y + Utility.Random(3) - 1;
                    int z = map.GetAverageZ(x, y);

                    if (validLocation = map.CanFit(x, y, this.Z, 16, false, false))
                        loc = new Point3D(x, y, this.Z);
                    else if (validLocation = map.CanFit(x, y, z, 16, false, false))
                        loc = new Point3D(x, y, z);
                }

                mercenary.MoveToWorld(loc, map);

                mercenary.Combatant = target;
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

        private static int GetRandomHue()
        {
            switch ( Utility.Random(6) )
            {
                default:
                case 0:
                    return 0;
                case 1:
                    return Utility.RandomBlueHue();
                case 2:
                    return Utility.RandomGreenHue();
                case 3:
                    return Utility.RandomRedHue();
                case 4:
                    return Utility.RandomYellowHue();
                case 5:
                    return Utility.RandomNeutralHue();
            }
        }
    }
}
