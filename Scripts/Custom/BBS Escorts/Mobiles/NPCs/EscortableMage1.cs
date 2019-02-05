using System;
using Server.Items;

namespace Server.Mobiles
{
    public class EscortableMage1 : BaseEscortable
    {

        #region BBS Quests [Locations]
        private static readonly string[] m_MLTownNames = new string[]
        {
            "Britain", "Jhelom",
			"Trinsic", "Moonglow", 
			"Vesper",  "Skara Brae",
			"Serpent's Hold", "New Haven",  
            /*"Cove", "Yew", "Buccaneer's Den", 
            "Nujel'm", "Magincia", "Minoc"*/

            /*Restricting Mage to only
             *request towns with mage shops*/
        };
        private static readonly string[] m_TownNames = new string[]
        {
           "Britain", "Jhelom",
			"Trinsic", "Moonglow", 
			"Vesper",  "Skara Brae",
			"Serpent's Hold", "Ocllo"/*,   
            /*"Cove", "Yew", "Buccaneer's Den", 
            "Nujel'm", "Magincia", "Minoc"*/

             /*Restricting Mage to only
             *request towns with mage shops*/
        };
        private static readonly string[] m_SITownNames = new string[]
        {
            "Moonshade"
        };
        #endregion

        [Constructable]
        public EscortableMage1()
        {
            this.Title = "the mage";

            this.SetSkill(SkillName.EvalInt, 80.0, 100.0);
            this.SetSkill(SkillName.Inscribe, 80.0, 100.0);
            this.SetSkill(SkillName.Magery, 80.0, 100.0);
            this.SetSkill(SkillName.Meditation, 80.0, 100.0);
            this.SetSkill(SkillName.MagicResist, 80.0, 100.0);
        }

        public EscortableMage1(Serial serial)
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
        }// Do not display 'the mage' when single-clicking

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
            this.AddItem(new Robe(GetRandomHue()));

            int lowHue = GetRandomHue();

            this.AddItem(new ShortPants(lowHue));

            if (this.Female)
                this.AddItem(new ThighBoots(lowHue));
            else
                this.AddItem(new Boots(lowHue));

            Utility.AssignRandomHair(this);

            this.PackGold(200, 250);
        }

        public override void OnDeath(Container c)
        {

            if (0.01 > Utility.RandomDouble())
                c.DropItem(new Sandals(1673));//Abysmal Blue
            else
                c.DropItem(new Sandals(Utility.RandomBlueHue()));//blues

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
            switch ( Utility.Random(5) )
            {
                default:
                case 0:
                    return Utility.RandomBlueHue();
                case 1:
                    return Utility.RandomGreenHue();
                case 2:
                    return Utility.RandomRedHue();
                case 3:
                    return Utility.RandomYellowHue();
                case 4:
                    return Utility.RandomNeutralHue();
            }
        }
    }
}
