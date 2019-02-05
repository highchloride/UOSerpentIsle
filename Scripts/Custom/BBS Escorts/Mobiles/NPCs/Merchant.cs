using System;
using Server.Items;

namespace Server.Mobiles
{
    public class Merchant : BaseEscortable
    {
        #region BBS Quests [Locations]
        private static readonly string[] m_MLTownNames = new string[]
        {
            "Cove", "Britain", "Jhelom",
			"Minoc", "Trinsic", "Moonglow", 
			"Vesper", "Yew", "Skara Brae",
			"Nujel'm", "Serpent's Hold",   
            "New Haven", "Buccaneer's Den"/*, 
            "Magincia"*/

        };
        private static readonly string[] m_TownNames = new string[]
        {
            "Cove", "Britain", "Jhelom",
			"Minoc", "Trinsic", "Moonglow", 
			"Vesper", "Yew", "Skara Brae",
			"Nujel'm", "Serpent's Hold",    
            "Ocllo"/*, "Magincia"*/

        };
        private static readonly string[] m_SITownNames = new string[]
        {
            "Monitor", "Fawn", "Sleeping Bull", "Moonshade"
        };
        #endregion

        [Constructable]
        public Merchant()
        {
            this.Title = "the merchant";
            this.SetSkill(SkillName.ItemID, 55.0, 78.0);
            this.SetSkill(SkillName.ArmsLore, 55, 78);
        }

        public Merchant(Serial serial)
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
        }// Do not display 'the merchant' when single-clicking

        public override string[] GetPossibleDestinations()
        {
            #region BBS Quests
            //if (Map == Map.Trammel)
            //    return m_MLTownNames;

            //else
            //    return m_TownNames;
            #endregion
            return m_SITownNames;
        }

        public override void InitOutfit()
        {
            if (this.Female)
                this.AddItem(new PlainDress());
            else
                this.AddItem(new Shirt(GetRandomHue()));

            int lowHue = GetRandomHue();

            this.AddItem(new ThighBoots());

            if (this.Female)
                this.AddItem(new FancyDress(lowHue));
            else
                this.AddItem(new FancyShirt(lowHue));
            this.AddItem(new LongPants(lowHue));

            if (!this.Female)
                this.AddItem(new BodySash(lowHue));

            //if ( !Female )
            //AddItem( new Longsword() );

            Utility.AssignRandomHair(this);

            this.PackGold(200, 250);
        }

        public override void OnDeath(Container c)
        {

            if (0.01 > Utility.RandomDouble())
                c.DropItem(new Sandals(1674));//Abysmal Orange
            else
                c.DropItem(new Sandals(Utility.RandomOrangeHue()));//oranges

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
            switch( Utility.Random(6) )
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
