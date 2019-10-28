using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Server.Mobiles;
using Server.Items;
using Server.Spells.Necromancy;
using Server.Spells.Fourth;
using Server.Spells.Seventh;
using Server.Spells.Mysticism;

namespace Server.Regions
{
    public class GorlabRegion : DangerRegion
    {
        public GorlabRegion(XmlElement xml, Map map, Region parent)
            : base(xml, map, parent)
        {
        }

        public override void OnEnter(Mobile m)
        {
            base.OnEnter(m);

            if (m.AccessLevel > AccessLevel.Player)
                return;

            //Check if the player has the dream crystal, otherwise knock em out and go to Dream Land.
            bool hasCrystal = false;

            if(m is PlayerMobile)
            {
                List<Item> items = ((PlayerMobile)m).Backpack.Items;
                foreach(Item item in items)
                {
                    if (item is DreamCrystal)
                    {
                        hasCrystal = true;
                    }
                }

                if(!hasCrystal)
                {
                    Timer.DelayCall(TimeSpan.FromSeconds(5), () => OnBeforeDream(m as PlayerMobile));
                }
            }            
        }

        private void OnBeforeDream(PlayerMobile m)
        {
            if ((!m.Alive) || !(m.Region.Name == "the Gorlab Swamp") || (m.AccessLevel > AccessLevel.Player))
                return;

            m.SendMessage("Thou art exhausted and in need of rest.");
            m.Stam = (int)(m.StamMax * 0.10f);

            Timer.DelayCall(TimeSpan.FromSeconds(5), () => OnBeginDream(m as PlayerMobile));
        }
        private void OnBeginDream(PlayerMobile m)
        {
            if ((!m.Alive))
                return;

            m.SendMessage("Thou hast fallen asleep.");

            //Create the player's box and label it
            MetalBox playerBox = new MetalBox();
            playerBox.Name = m.Name;

            //Items list so we can remove them
            List<Item> equipItems = m.Items;

            //Remove clothing/equipment
            for (int x = m.Items.Count - 1; x >= 0; x--)
            {
                if (!(m.Items[x] is Backpack))
                    playerBox.AddItem(m.Items[x]);
            }

            //Create the box for the backpack items
            MetalBox backpackBox = new MetalBox();
            playerBox.AddItem(backpackBox);

            //Remove backpack items
            for (int y = m.Backpack.Items.Count - 1; y >= 0; y--)
            {
                if ((m.Backpack.Items[y] is HelmOfCourage) || (m.Backpack.Items[y] is CrystalRoseOfLove) || (m.Backpack.Items[y] is MirrorOfTruth) || (m.Backpack.Items[y] is StaffOrb))
                { }
                else
                {
                    backpackBox.AddItem(m.Backpack.Items[y]);
                }
            }

            //Move the container away
            playerBox.MoveToWorld(new Point3D(0, 0, 0), Map.SerpentIsle);

            //Move pets away
            foreach (Mobile mobile in m.AllFollowers)
            {
                mobile.MoveToWorld(new Point3D(0, 0, 0), Map.SerpentIsle);
                mobile.Frozen = true;
            }

            //Send to random destination in the dream
            switch (Utility.Random(4))
            {
                case 0:
                    m.MoveToWorld(new Point3D(1988, 1536, 0), Map.SerpentIsle);
                    m.SetDirection(Direction.East);
                    break;
                case 1:
                    m.MoveToWorld(new Point3D(2050, 1491, 0), Map.SerpentIsle);
                    m.SetDirection(Direction.North);
                    break;
                case 2:
                    m.MoveToWorld(new Point3D(2040, 1389, 0), Map.SerpentIsle);
                    m.SetDirection(Direction.East);
                    break;
                case 3:
                    m.MoveToWorld(new Point3D(1904, 1376, 0), Map.SerpentIsle);
                    m.SetDirection(Direction.West);
                    break;
            }

            //Restore player to full 
            ((PlayerMobile)m).Hits = m.HitsMax;
            ((PlayerMobile)m).Mana = m.ManaMax;
            ((PlayerMobile)m).Stam = m.StamMax;
            ((PlayerMobile)m).Hunger = 20;
            ((PlayerMobile)m).Thirst = 20;

            ((PlayerMobile)m).Paralyzed = false;
            ((PlayerMobile)m).Poison = null;

            EvilOmenSpell.TryEndEffect(m);
            StrangleSpell.RemoveCurse(m);
            CorpseSkinSpell.RemoveCurse(m);
            CurseSpell.RemoveEffect(m);
            MortalStrike.EndWound(m);
            BloodOathSpell.RemoveCurse(m);
            MindRotSpell.ClearMindRotScalar(m);

            BuffInfo.RemoveBuff(m, BuffIcon.Clumsy);
            BuffInfo.RemoveBuff(m, BuffIcon.FeebleMind);
            BuffInfo.RemoveBuff(m, BuffIcon.Weaken);
            BuffInfo.RemoveBuff(m, BuffIcon.Curse);
            BuffInfo.RemoveBuff(m, BuffIcon.MassCurse);
            BuffInfo.RemoveBuff(m, BuffIcon.MortalStrike);
            BuffInfo.RemoveBuff(m, BuffIcon.Mindrot);

            //Additionally, remove any polymorphs
            PolymorphSpell.EndPolymorph(m);
            StoneFormSpell.EndEffect(m);

            m.BodyMod = 0;
            m.HueMod = -1;
        }
    }
}
