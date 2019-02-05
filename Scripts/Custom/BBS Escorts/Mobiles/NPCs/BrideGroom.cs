/*Change to Old School BrideGroom if bride on death she drops silver sandals
 *may need to change hue(2475)or update Hues.mul in your uo client*/

using System;
using Server.Items;

namespace Server.Mobiles
{
    public class BrideGroom : BaseEscortable
    {

        #region BBS Quests [Locations]
        private static readonly string[] m_MLTownNames = new string[]
        {
            "Cove", "Britain", "Jhelom",
			"Minoc", "Trinsic", "Moonglow", 
			"Vesper", "Yew", "Skara Brae",
			"Nujel'm", "Serpent's Hold",   
            "Magincia", "New Haven" 

        };
        private static readonly string[] m_TownNames = new string[]
        {
            "Cove", "Britain", "Jhelom",
			"Minoc", "Trinsic", "Moonglow", 
			"Vesper", "Yew", "Skara Brae",
			"Nujel'm", "Serpent's Hold",    
            "Magincia", "Ocllo"  

        };
        private static readonly string[] m_SITownNames = new string[]
        {
            "Monitor", "Fawn", "Sleeping Bull", "Moonshade"
        };

        #endregion

        [Constructable]
        public BrideGroom()
        {
            if (this.Female)
                this.Title = "the bride";
            else
                this.Title = "the groom";			
        }

        public BrideGroom(Serial serial)
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
        }// Do not display 'the groom' when single-clicking

        public override string[] GetPossibleDestinations()
        {
            #region BBS Quests
            //if (Map == Map.Trammel)
            //    return m_MLTownNames;

            //else
            //    return m_TownNames;

            return m_SITownNames;
            #endregion

            
        }

        public override void InitOutfit()
        {
            if (this.Female)
				
                this.AddItem(new FancyDress());
            else
                this.AddItem(new FancyShirt());

            int lowHue = GetRandomHue();

            this.AddItem(new LongPants(lowHue));

            if (this.Female)
                this.AddItem(new Shoes(lowHue));
            else
                this.AddItem(new Boots(lowHue));

            if (Utility.RandomBool())
                this.HairItemID = 0x203B;
            else
                this.HairItemID = 0x203C;

            this.HairHue = this.Race.RandomHairHue();

            this.PackGold(200, 250);
        }

        public override void OnDeath(Container c)
        {

            if (this.Female)
            {
                if (0.01 > Utility.RandomDouble())
                    c.DropItem(new Sandals(1072));//silver 
            }
            else if (0.01 > Utility.RandomDouble())
            {
                c.DropItem(new Sandals(1280));//black
            }
            else c.DropItem(new Sandals(Utility.RandomHairHue()));//0x44E, 46)));//hair

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
