using System;
using System.Collections;
using Server;
using Server.Network;
using Server.Items;
using Server.Mobiles;

using System.Linq;
using VitaNex.FX;

namespace Server.Mobiles
{
    [CorpseName("a mysterious corpse")]
    public class mysterious : BaseCreature
    {
               
        [Constructable]
        public mysterious() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.08, 0.02)
        {
            Name = "mysterious";
            Body = 740;
            BaseSoundID = 0x372;

            SetStr(200);
            SetDex(150);
            SetInt(250);

            SetHits(50000);

            SetDamage(35, 70);

            SetDamageType(ResistanceType.Physical, 100);
            SetDamageType(ResistanceType.Cold, 75);

            SetResistance(ResistanceType.Physical, 45, 65);
            SetResistance(ResistanceType.Fire, 30, 65);
            SetResistance(ResistanceType.Cold, 45, 65);
            SetResistance(ResistanceType.Poison, 30, 55);
            SetResistance(ResistanceType.Energy, 45, 65);

            SetSkill(SkillName.EvalInt, 120.1, 130.0);
            SetSkill(SkillName.Magery, 90.1, 100.0);
            SetSkill(SkillName.Meditation, 100.1, 101.0);
            SetSkill(SkillName.Poisoning, 100.1, 101.0);
            SetSkill(SkillName.MagicResist, 175.2, 200.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 75.1, 100.0);

            Fame = 15000;
            Karma = -15000;
            

            if (Utility.Random(100) <1)    // Chance of getting any item
            {
                switch (Utility.Random(4)) // Items + 1 - UOSI Removed elves
                {
                    case 0:
                        {
                            PackItem(new RandomHumanMageArmor());
                            break;
                        }
                    case 1:
                        {
                            PackItem(new RandomHumanWarriorArmor());
                            break;
                        }
                    case 2:
                        {
                            PackItem(new RandomGargMageArmor());
                            break;
                        }
                    case 3:
                        {
                            PackItem(new RandomGargWarriorArmor());
                            break;
                        }
                }
            }
        }

        public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich );
			AddLoot( LootPack.Rich );
			AddLoot( LootPack.Gems, 2 );
		}
			

		public override bool BardImmune{ get{ return true; } }

        public void DoSpecialAbility(Mobile target)
        {

            if (0.25 >= Utility.RandomDouble())
            {
                new FireExplodeEffect(target, target.Map, 5, effectHandler: ExplosionDamage).Send();
                base.OnGotMeleeAttack(target);

            }
        
            if (0.25 >= Utility.RandomDouble())
            {
                new EnergyExplodeEffect(target, target.Map, 5, effectHandler: ExplosionDamage).Send();
                base.OnDamagedBySpell(target);
            }
        }

        public override void OnDamagedBySpell(Mobile from)
        {
            base.OnDamagedBySpell(from);

            DoSpecialAbility(from);
        }

       

        public override void OnGotMeleeAttack( Mobile from )
		{
			base.OnGotMeleeAttack( from );

            DoSpecialAbility(from);
        }

        public virtual void ExplosionDamage(EffectInfo info)
        {
            ArrayList list = new ArrayList();
            Effects.PlaySound(info.Source.Location, info.Map, 777);

            foreach (Mobile m in
                info.Source.Location.GetMobilesInRange(info.Map, 0)
                    .Where(m => m != null && !m.Deleted && m.CanBeHarmful(m, false, true)))
            {
                if (m == this || !CanBeHarmful(m))
                    continue;

                if (m.Player)
                    list.Add(m);
            }
            foreach (Mobile m in list)
            {
                DoHarmful(m);
                int toExplode = Utility.RandomMinMax(30, 40);
                m.Damage(toExplode, this);
            }
        }

        public mysterious( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

}
